using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Sunny.UI;
using WaterStation.Client;
using WaterStation.Client.Model.LayoutDto;

namespace WaterStation.Client.BatteryManageMoudle
{
    /// <summary>
    /// 仪表质控定时调度 + 质控设置 UI。
    /// 单一定时器（60s）+ 内存调度表 + 同分钟去重 + 静默下发仪表指令。
    /// </summary>
    public partial class FormSLPLC
    {
        #region 调度字段与常量

        private const string InstrumentQCEnabledText = "已开启";
        private const string InstrumentQCDisabledText = "开启";

        private readonly object _instrumentQCLock = new();
        private readonly List<InstrumentQCScheduleItem> _instrumentQCSchedules = new();
        private readonly ConcurrentDictionary<string, string> _instrumentQCLastTriggered = new();
        private System.Timers.Timer _instrumentQCTimer;

        /// <summary>核查类型定义：仅零点(LD)、标样(BY)。</summary>
        private static readonly (string Prefix, InstrumentQCCheckType CheckType, bool HasConcentration, string DefaultConcentration)[] InstrumentQCCheckDefs =
        {
            ("LD", InstrumentQCCheckType.ZeroCheck, true, "0"),
            ("BY", InstrumentQCCheckType.SampleCheck, true, "1")
        };

        private static readonly string[] InstrumentQCFactorKeys = { "ZD", "AD", "ZL", "COD" };

        #endregion

        #region UI 字段与布局常量

        private static readonly Color InstrumentQCPrimary = Color.FromArgb(26, 101, 143);

        private const int QcColTypeX = 16;
        private const int QcColWeekLabelX = 148;
        private const int QcColDayX = 192;
        private const int QcColHourX = 256;
        private const int QcColMinX = 320;
        private const int QcColConcLabelX = 380;
        private const int QcColConcX = 470;
        private const int QcColToggleX = 540;
        private const int QcInputWidth = 44;
        private const int QcConcWidth = 53;
        private const int QcToggleWidth = 73;
        private const int QcRowHeight = 34;
        private const int QcGroupWidth = 640;
        private const int QcGroupHeight = 108;

        private TabPage _tabPageQC;
        private UITabControl _uiTabQC;
        private TabPage _tabPageInstrumentQC;
        private UIPanel _panelInstrumentQC;
        private bool _instrumentQCUIInitialized;
        private bool _instrumentQCBinding;

        private readonly Dictionary<string, InstrumentQCRowControls> _instrumentQCRows = new();

        private sealed class InstrumentQCRowControls
        {
            public string ConfigKey { get; set; }
            public InstrumentQCCheckType CheckType { get; set; }
            public string FactorKey { get; set; }
            public UITextBox Day { get; set; }
            public UITextBox Hour { get; set; }
            public UITextBox Minute { get; set; }
            public UITextBox Concentration { get; set; }
            public UIButton Toggle { get; set; }
        }

        #endregion

        #region 初始化 / 销毁

        /// <summary>
        /// 初始化仪表质控：补齐配置键、加载调度表、启动定时器、构建 UI。
        /// </summary>
        private void InitInstrumentQC()
        {
            try
            {
                EnsureInstrumentQCAppSettingsExist();
                LoadInstrumentQCSchedulesFromConfig();
                StartInstrumentQCTimer();
                InitInstrumentQCUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "初始化仪表质控失败");
            }
        }

        /// <summary>
        /// 停止并释放仪表质控定时器。
        /// </summary>
        private void StopInstrumentQCTimer()
        {
            if (_instrumentQCTimer == null)
                return;

            try
            {
                _instrumentQCTimer.Stop();
                _instrumentQCTimer.Elapsed -= InstrumentQCTimer_Elapsed;
                _instrumentQCTimer.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "停止仪表质控定时器异常");
            }
            finally
            {
                _instrumentQCTimer = null;
            }
        }

        #endregion

        #region 配置加载 / 保存

        /// <summary>
        /// 确保 16 路仪表质控 App.config 键存在。
        /// </summary>
        private void EnsureInstrumentQCAppSettingsExist()
        {
            bool changed = false;
            foreach (string factor in InstrumentQCFactorKeys)
            {
                foreach (var def in InstrumentQCCheckDefs)
                {
                    string key = $"{def.Prefix}Zhikong{factor}";
                    if (configuration.AppSettings.Settings[key] != null)
                        continue;

                    string defaultValue = def.HasConcentration
                        ? $"1;00;00;{InstrumentQCDisabledText};{def.DefaultConcentration}"
                        : $"1;00;00;{InstrumentQCDisabledText}";

                    configuration.AppSettings.Settings.Add(key, defaultValue);
                    changed = true;
                }
            }

            if (changed)
            {
                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        /// <summary>
        /// 从 App.config 加载全部仪表质控调度到内存。
        /// </summary>
        private void LoadInstrumentQCSchedulesFromConfig()
        {
            lock (_instrumentQCLock)
            {
                _instrumentQCSchedules.Clear();
                foreach (string factor in InstrumentQCFactorKeys)
                {
                    foreach (var def in InstrumentQCCheckDefs)
                    {
                        string key = $"{def.Prefix}Zhikong{factor}";
                        string raw = GetInstrumentQCAppSetting(key, def.HasConcentration
                            ? $"1;00;00;{InstrumentQCDisabledText};{def.DefaultConcentration}"
                            : $"1;00;00;{InstrumentQCDisabledText}");
                        _instrumentQCSchedules.Add(ParseInstrumentQCSchedule(key, factor, def.CheckType, def.HasConcentration, raw));
                    }
                }
            }
        }

        private static string GetInstrumentQCAppSetting(string key, string defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        /// <summary>
        /// 解析单路配置字符串：星期;时;分;状态[;浓度]
        /// </summary>
        private InstrumentQCScheduleItem ParseInstrumentQCSchedule(
            string configKey,
            string factorKey,
            InstrumentQCCheckType checkType,
            bool hasConcentration,
            string raw)
        {
            string[] parts = (raw ?? string.Empty).Split(';');
            string day = parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0]) ? parts[0].Trim() : "1";
            int hour = 0;
            int minute = 0;
            if (parts.Length > 1)
                int.TryParse(parts[1].Trim(), out hour);
            if (parts.Length > 2)
                int.TryParse(parts[2].Trim(), out minute);

            hour = Math.Clamp(hour, 0, 23);
            minute = Math.Clamp(minute, 0, 59);

            string status = parts.Length > 3 ? parts[3].Trim() : InstrumentQCDisabledText;
            bool enabled = string.Equals(status, InstrumentQCEnabledText, StringComparison.Ordinal);
            string concentration = hasConcentration
                ? (parts.Length > 4 && !string.IsNullOrWhiteSpace(parts[4]) ? parts[4].Trim() : "0")
                : null;

            return new InstrumentQCScheduleItem
            {
                ConfigKey = configKey,
                FactorKey = factorKey,
                AnalyzerName = ResolveInstrumentQCAnalyzerName(factorKey),
                CheckType = checkType,
                Day = day,
                Hour = hour,
                Minute = minute,
                Enabled = enabled,
                Concentration = concentration ?? "0"
            };
        }

        private static string ResolveInstrumentQCAnalyzerName(string factorKey) => factorKey switch
        {
            "ZD" => "总氮",
            "AD" => "氨氮",
            "ZL" => "总磷",
            "COD" => "COD",
            _ => factorKey
        };

        /// <summary>
        /// 获取调度表快照（供 UI 绑定）。
        /// </summary>
        private IReadOnlyList<InstrumentQCScheduleItem> GetInstrumentQCSchedules()
        {
            lock (_instrumentQCLock)
            {
                return _instrumentQCSchedules
                    .Select(CloneInstrumentQCSchedule)
                    .ToList();
            }
        }

        private static InstrumentQCScheduleItem CloneInstrumentQCSchedule(InstrumentQCScheduleItem src)
        {
            return new InstrumentQCScheduleItem
            {
                ConfigKey = src.ConfigKey,
                FactorKey = src.FactorKey,
                AnalyzerName = src.AnalyzerName,
                CheckType = src.CheckType,
                Day = src.Day,
                Hour = src.Hour,
                Minute = src.Minute,
                Enabled = src.Enabled,
                Concentration = src.Concentration
            };
        }

        /// <summary>
        /// 保存单路调度（含开关状态），写 App.config 并刷新内存。
        /// </summary>
        private bool SaveInstrumentQCSchedule(InstrumentQCScheduleItem item, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (item == null || string.IsNullOrWhiteSpace(item.ConfigKey))
            {
                errorMessage = "调度配置无效";
                return false;
            }

            string day = (item.Day ?? string.Empty).Trim();
            if (!IsValidInstrumentQCDay(day))
            {
                errorMessage = "周期格式错误：00=每天，0-6=星期（周日为0）";
                return false;
            }

            int hour = Math.Clamp(item.Hour, 0, 23);
            int minute = Math.Clamp(item.Minute, 0, 59);
            string concentration = string.IsNullOrWhiteSpace(item.Concentration) ? "0" : item.Concentration.Trim();
            string statusText = item.Enabled ? InstrumentQCEnabledText : InstrumentQCDisabledText;
            string value = item.HasConcentration
                ? $"{day};{hour:D2};{minute:D2};{statusText};{concentration}"
                : $"{day};{hour:D2};{minute:D2};{statusText}";

            try
            {
                if (configuration.AppSettings.Settings[item.ConfigKey] == null)
                    configuration.AppSettings.Settings.Add(item.ConfigKey, value);
                else
                    configuration.AppSettings.Settings[item.ConfigKey].Value = value;

                configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                errorMessage = $"保存配置失败：{ex.Message}";
                _logger.Error(ex, "保存仪表质控配置失败");
                return false;
            }

            lock (_instrumentQCLock)
            {
                var existing = _instrumentQCSchedules.FirstOrDefault(x => x.ConfigKey == item.ConfigKey);
                if (existing != null)
                {
                    existing.Day = day;
                    existing.Hour = hour;
                    existing.Minute = minute;
                    existing.Enabled = item.Enabled;
                    existing.Concentration = concentration;
                    existing.AnalyzerName = ResolveInstrumentQCAnalyzerName(existing.FactorKey);
                }
                else
                {
                    item.Day = day;
                    item.Hour = hour;
                    item.Minute = minute;
                    item.Concentration = concentration;
                    item.AnalyzerName = ResolveInstrumentQCAnalyzerName(item.FactorKey);
                    _instrumentQCSchedules.Add(CloneInstrumentQCSchedule(item));
                }
            }

            return true;
        }

        /// <summary>
        /// 批量保存当前 UI 编辑的全部调度。
        /// </summary>
        private bool SaveInstrumentQCSchedules(IEnumerable<InstrumentQCScheduleItem> items, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (items == null)
            {
                errorMessage = "无配置可保存";
                return false;
            }

            foreach (var item in items)
            {
                if (!SaveInstrumentQCSchedule(item, out errorMessage))
                    return false;
            }

            return true;
        }

        private static bool IsValidInstrumentQCDay(string day)
        {
            if (string.IsNullOrWhiteSpace(day))
                return false;

            day = day.Trim();
            if (day == "00")
                return true;

            // 0-6：星期（周日=0）
            return int.TryParse(day, out int dow) && dow >= 0 && dow <= 6 && day.Length == 1;
        }

        #endregion

        #region 定时匹配与下发

        private void StartInstrumentQCTimer()
        {
            if (_instrumentQCTimer != null)
                return;

            _instrumentQCTimer = new System.Timers.Timer(60000);
            _instrumentQCTimer.Elapsed += InstrumentQCTimer_Elapsed;
            _instrumentQCTimer.AutoReset = true;
            _instrumentQCTimer.Start();
            _logger.Info("仪表质控定时器已启动（间隔 60 秒）");
        }

        private void InstrumentQCTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                TickInstrumentQCSchedules(DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "仪表质控定时调度异常");
            }
        }

        /// <summary>
        /// 判断当前时刻是否命中调度。
        /// 00=每天；0-6=星期。
        /// </summary>
        internal static bool IsInstrumentQCScheduleMatched(InstrumentQCScheduleItem item, DateTime now)
        {
            if (item == null || !item.Enabled)
                return false;

            if (now.Hour != item.Hour || now.Minute != item.Minute)
                return false;

            string day = (item.Day ?? string.Empty).Trim();
            if (day == "00")
                return true;

            if (day.Length == 1 && int.TryParse(day, out int dow) && dow >= 0 && dow <= 6)
                return (int)now.DayOfWeek == dow;

            return false;
        }

        /// <summary>
        /// 遍历调度表，命中且未去重则静默下发指令。
        /// </summary>
        private void TickInstrumentQCSchedules(DateTime now)
        {
            List<InstrumentQCScheduleItem> snapshot;
            lock (_instrumentQCLock)
            {
                snapshot = _instrumentQCSchedules.Select(CloneInstrumentQCSchedule).ToList();
            }

            string minuteKey = now.ToString("yyyyMMddHHmm");
            foreach (var item in snapshot)
            {
                if (!IsInstrumentQCScheduleMatched(item, now))
                    continue;

                // 未配置的设备因子不触发
                if (!IsInstrumentQCFactorEnabled(item.FactorKey))
                    continue;

                string dedupKey = $"{item.ConfigKey}:{minuteKey}";
                if (!_instrumentQCLastTriggered.TryAdd(dedupKey, minuteKey))
                    continue;

                // 清理过期去重键，避免无限增长
                foreach (var old in _instrumentQCLastTriggered.Keys.ToList())
                {
                    if (!old.EndsWith($":{minuteKey}", StringComparison.Ordinal)
                        && _instrumentQCLastTriggered.TryGetValue(old, out var oldMinute)
                        && oldMinute != minuteKey)
                    {
                        _instrumentQCLastTriggered.TryRemove(old, out _);
                    }
                }

                string cmdTail = ResolveInstrumentQCCommandTail(item.CheckType);
                if (cmdTail == null)
                {
                    _logger.Warn($"仪表质控：未知核查类型 {item.CheckType}，键={item.ConfigKey}");
                    continue;
                }

                string analyzerName = item.AnalyzerName;
                string factorKey = item.FactorKey;
                string checkDisplay = item.CheckTypeDisplayName;
                string configKey = item.ConfigKey;
                _ = Task.Run(() =>
                {
                    try
                    {
                        _logger.Info($"仪表质控定时触发：{analyzerName} {checkDisplay}（{configKey}）");
                        ExecuteInstrumentQCCommand(factorKey, cmdTail, analyzerName, checkDisplay, showTip: false);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"仪表质控下发失败：{configKey}");
                    }
                });
            }
        }

        /// <summary>
        /// 核查类型 → 指令尾段。
        /// </summary>
        private static string ResolveInstrumentQCCommandTail(InstrumentQCCheckType checkType)
        {
            return checkType switch
            {
                InstrumentQCCheckType.ZeroCheck => "00 03",
                InstrumentQCCheckType.SampleCheck => "00 02",
                InstrumentQCCheckType.SpanCheck => "00 04",
                InstrumentQCCheckType.AddSample => "00 07",
                _ => null
            };
        }

        /// <summary>
        /// 因子 → Modbus 站址。
        /// </summary>
        private static string ResolveInstrumentQCStation(string factorKey) => factorKey switch
        {
            "ZD" => "01",
            "AD" => "02",
            "COD" => "03",
            "ZL" => "04",
            _ => null
        };

        /// <summary>
        /// 静默/手动统一下发仪表质控指令。
        /// </summary>
        private void ExecuteInstrumentQCCommand(
            string factorKey,
            string commandTail,
            string analyzerName,
            string checkDisplayName,
            bool showTip)
        {
            string station = ResolveInstrumentQCStation(factorKey);
            if (station == null)
            {
                _logger.Warn($"仪表质控：未知因子 {factorKey}");
                if (showTip)
                    ShowTip.ShowErrorTip($"未知因子：{factorKey}");
                return;
            }

            string strSend = $"{station} 10 12 00 00 01 02 {commandTail}";
            try
            {
                if (serialPortHJ == null)
                {
                    _logger.Warn($"仪表质控下发失败：串口未初始化（{analyzerName}{checkDisplayName}）");
                    if (showTip)
                        ShowTip.ShowErrorTip("仪表串口未初始化");
                    return;
                }

                if (!serialPortHJ.IsOpen)
                    serialPortHJ.Open();

                var sendData = strToHexByte(CalcCRC16(strSend));
                if (!SendYBData(sendData))
                {
                    _logger.Warn($"仪表质控下发失败：SendYBData 返回 false（{analyzerName}{checkDisplayName}）");
                    if (showTip)
                        ShowTip.ShowErrorTip($"{analyzerName}{checkDisplayName}下发失败");
                    return;
                }

                WriteLogDB("操作", $"{analyzerName}{checkDisplayName}（定时）");
                _logger.Info($"仪表质控指令已下发：{strSend}");
                if (showTip)
                    ShowTip.ShowSuccessTip($"{analyzerName}{checkDisplayName}已下发");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"仪表质控下发异常：{analyzerName}{checkDisplayName}");
                if (showTip)
                    ShowTip.ShowErrorTip($"{analyzerName}{checkDisplayName}下发异常：{ex.Message}");
            }
        }

        #endregion

        #region 质控设置 UI

        /// <summary>
        /// 初始化仪表质控页签与控件。
        /// </summary>
        private void InitInstrumentQCUI()
        {
            if (IsDisposed || DesignMode)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(InitInstrumentQCUI));
                return;
            }

            try
            {
                EnsureInstrumentQCTabCreated();
                BindInstrumentQCFromSchedules();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "初始化仪表质控界面失败");
                ShowTip.ShowErrorTip($"初始化仪表质控界面失败：{ex.Message}");
            }
        }

        private void EnsureInstrumentQCTabCreated()
        {
            if (_instrumentQCUIInitialized)
                return;

            _tabPageQC = new TabPage
            {
                Name = "tabPageQC",
                Text = "质控设置",
                BackColor = Color.White,
                Padding = new Padding(0)
            };

            _uiTabQC = new UITabControl
            {
                Name = "uiTabQC",
                Dock = DockStyle.Fill,
                DrawMode = TabDrawMode.OwnerDrawFixed,
                FillColor = Color.White,
                Font = new Font("微软雅黑", 10F, FontStyle.Regular, GraphicsUnit.Point),
                ItemSize = new Size(150, 40),
                MainPage = "",
                MenuStyle = UIMenuStyle.Custom,
                SizeMode = TabSizeMode.Fixed,
                Style = UIStyle.Custom,
                StyleCustomMode = true,
                TabBackColor = Color.White,
                TabSelectedColor = InstrumentQCPrimary,
                TabSelectedForeColor = Color.White,
                TabSelectedHighColor = InstrumentQCPrimary,
                TabUnSelectedForeColor = Color.Black
            };

            _tabPageInstrumentQC = new TabPage
            {
                Name = "tabPageInstrumentQC",
                Text = "仪表质控",
                BackColor = Color.White,
                AutoScroll = true
            };

            _panelInstrumentQC = new UIPanel
            {
                Name = "panelInstrumentQC",
                Dock = DockStyle.Fill,
                FillColor = Color.White,
                RectColor = Color.White,
                Style = UIStyle.Custom,
                StyleCustomMode = true,
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point),
                AutoScroll = true
            };

            _tabPageInstrumentQC.Controls.Add(_panelInstrumentQC);
            _uiTabQC.Controls.Add(_tabPageInstrumentQC);
            _tabPageQC.Controls.Add(_uiTabQC);

            BuildInstrumentQCContent();

            if (!uiTabControl1.Controls.Contains(_tabPageQC))
                uiTabControl1.Controls.Add(_tabPageQC);

            _instrumentQCUIInitialized = true;
        }

        private void BuildInstrumentQCContent()
        {
            _panelInstrumentQC.Controls.Clear();
            _instrumentQCRows.Clear();

            // 与仪表控制页一致：仅 Global.EQNames 中存在的因子才显示
            var factors = new List<(string Key, string Title)>();
            if (IsInstrumentQCFactorEnabled("ZD"))
                factors.Add(("ZD", "总氮"));
            if (IsInstrumentQCFactorEnabled("AD"))
                factors.Add(("AD", "氨氮"));
            if (IsInstrumentQCFactorEnabled("ZL"))
                factors.Add(("ZL", "总磷"));
            if (IsInstrumentQCFactorEnabled("COD"))
                factors.Add(("COD", ResolveInstrumentQCAnalyzerName("COD")));

            InstrumentQCCheckType[] checkOrder =
            {
                InstrumentQCCheckType.ZeroCheck,
                InstrumentQCCheckType.SampleCheck
            };

            int groupTop = 8;
            foreach (var (factorKey, factorTitle) in factors)
            {
                var group = CreateInstrumentQCGroupBox(factorTitle, groupTop, QcGroupWidth, QcGroupHeight);
                int rowY = 28;
                foreach (var checkType in checkOrder)
                {
                    string prefix = checkType == InstrumentQCCheckType.ZeroCheck ? "LD" : "BY";
                    string configKey = $"{prefix}Zhikong{factorKey}";
                    AddInstrumentQCRow(group, configKey, factorKey, checkType, rowY);
                    rowY += QcRowHeight;
                }

                _panelInstrumentQC.Controls.Add(group);
                groupTop += QcGroupHeight + 8;
            }

            var btnSave = new UIButton
            {
                Text = "保存配置",
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point),
                FillColor = InstrumentQCPrimary,
                RectColor = Color.Transparent,
                Radius = 15,
                Style = UIStyle.Custom,
                StyleCustomMode = true,
                Cursor = Cursors.Hand,
                Size = new Size(110, 34),
                Location = new Point(16, groupTop + 8)
            };
            btnSave.Click += BtnInstrumentQCSaveAll_Click;

            var lblHint = new UILabel
            {
                Text = "说明：仅显示已配置设备因子；00=每天，0-6=星期（周日为0）。修改后点「保存配置」落盘；开关切换会立即保存。",
                Font = new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(140, groupTop + 12),
                Size = new Size(780, 40),
                ForeColor = Color.DimGray
            };

            _panelInstrumentQC.Controls.Add(btnSave);
            _panelInstrumentQC.Controls.Add(lblHint);
        }

        /// <summary>
        /// 因子是否在系统参数设备名中（与 GetYBDisplay / Global.EQNames 一致）。
        /// </summary>
        private static bool IsInstrumentQCFactorEnabled(string factorKey)
        {
            if (string.IsNullOrEmpty(Global.EQNames))
                return false;

            string name = factorKey switch
            {
                "ZD" => "总氮",
                "AD" => "氨氮",
                "ZL" => "总磷",
                "COD" => "COD",
                _ => null
            };
            return name != null && Global.EQNames.Contains(name);
        }

        private UIGroupBox CreateInstrumentQCGroupBox(string title, int top, int width, int height)
        {
            return new UIGroupBox
            {
                Text = title,
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point),
                FillColor = Color.White,
                RectColor = Color.Transparent,
                Style = UIStyle.Custom,
                StyleCustomMode = true,
                Location = new Point(8, top),
                Size = new Size(width, height),
                Padding = new Padding(2, 3, 2, 3)
            };
        }

        private void AddInstrumentQCRow(
            UIGroupBox group,
            string configKey,
            string factorKey,
            InstrumentQCCheckType checkType,
            int y)
        {
            string typeName = checkType == InstrumentQCCheckType.ZeroCheck ? "零点核查" : "标样核查";
            bool hasConcentration = true;

            var lblType = new UILabel
            {
                Text = typeName + "：",
                Font = new Font("微软雅黑", 10.8F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(QcColTypeX, y + 4),
                AutoSize = true
            };

            var lblWeek = new UILabel
            {
                Text = "周期",
                Font = new Font("微软雅黑", 10.8F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(QcColWeekLabelX, y + 4),
                AutoSize = true
            };

            var txtDay = CreateInstrumentQCTextBox(new Point(QcColDayX, y), QcInputWidth);
            var lblColon1 = CreateInstrumentQCStaticLabel(":", new Point(QcColDayX + QcInputWidth + 4, y + 4));
            var txtHour = CreateInstrumentQCTextBox(new Point(QcColHourX, y), QcInputWidth);
            var lblColon2 = CreateInstrumentQCStaticLabel(":", new Point(QcColHourX + QcInputWidth + 4, y + 4));
            var txtMin = CreateInstrumentQCTextBox(new Point(QcColMinX, y), QcInputWidth);

            group.Controls.Add(lblType);
            group.Controls.Add(lblWeek);
            group.Controls.Add(txtDay);
            group.Controls.Add(lblColon1);
            group.Controls.Add(txtHour);
            group.Controls.Add(lblColon2);
            group.Controls.Add(txtMin);

            UITextBox txtConc = null;
            if (hasConcentration)
            {
                var lblConc = new UILabel
                {
                    Text = "核查浓度：",
                    Font = new Font("微软雅黑", 10.8F, FontStyle.Regular, GraphicsUnit.Point),
                    Location = new Point(QcColConcLabelX, y + 4),
                    AutoSize = true
                };
                txtConc = CreateInstrumentQCTextBox(new Point(QcColConcX, y), QcConcWidth);
                group.Controls.Add(lblConc);
                group.Controls.Add(txtConc);
            }

            var btnToggle = new UIButton
            {
                Text = "开启",
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point),
                FillColor = InstrumentQCPrimary,
                RectColor = Color.Transparent,
                Radius = 15,
                Style = UIStyle.Custom,
                StyleCustomMode = true,
                Cursor = Cursors.Hand,
                Size = new Size(QcToggleWidth, 29),
                Location = new Point(QcColToggleX, y),
                Tag = configKey
            };
            btnToggle.Click += BtnInstrumentQCToggle_Click;
            group.Controls.Add(btnToggle);

            _instrumentQCRows[configKey] = new InstrumentQCRowControls
            {
                ConfigKey = configKey,
                FactorKey = factorKey,
                CheckType = checkType,
                Day = txtDay,
                Hour = txtHour,
                Minute = txtMin,
                Concentration = txtConc,
                Toggle = btnToggle
            };
        }

        private static UITextBox CreateInstrumentQCTextBox(Point location, int width)
        {
            return new UITextBox
            {
                Cursor = Cursors.IBeam,
                FillColor = Color.White,
                Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point),
                Location = location,
                Margin = new Padding(0),
                MinimumSize = new Size(1, 1),
                Padding = new Padding(5),
                Radius = 15,
                RectColor = Color.Silver,
                Size = new Size(width, 29),
                Style = UIStyle.Custom,
                StyleCustomMode = true,
                TextAlignment = ContentAlignment.MiddleCenter,
                Text = "00"
            };
        }

        private static UILabel CreateInstrumentQCStaticLabel(string text, Point location)
        {
            return new UILabel
            {
                Text = text,
                Font = new Font("微软雅黑", 10.8F, FontStyle.Regular, GraphicsUnit.Point),
                Location = location,
                AutoSize = true
            };
        }

        private void BindInstrumentQCFromSchedules()
        {
            _instrumentQCBinding = true;
            try
            {
                var schedules = GetInstrumentQCSchedules();
                foreach (var item in schedules)
                {
                    if (!_instrumentQCRows.TryGetValue(item.ConfigKey, out var row))
                        continue;

                    row.Day.Text = item.Day ?? "1";
                    row.Hour.Text = item.Hour.ToString("D2");
                    row.Minute.Text = item.Minute.ToString("D2");
                    if (row.Concentration != null)
                        row.Concentration.Text = string.IsNullOrWhiteSpace(item.Concentration) ? "0" : item.Concentration;
                    row.Toggle.Text = item.EnabledButtonText;
                }
            }
            finally
            {
                _instrumentQCBinding = false;
            }
        }

        private bool TryCollectInstrumentQCItem(InstrumentQCRowControls row, out InstrumentQCScheduleItem item, out string error)
        {
            item = null;
            error = string.Empty;

            string day = (row.Day.Text ?? string.Empty).Trim();
            bool dayOk = day == "00"
                || (day.Length == 1 && int.TryParse(day, out int dow) && dow >= 0 && dow <= 6);
            if (!dayOk)
            {
                error = $"{row.ConfigKey} 周期格式错误（00=每天，0-6=星期）";
                return false;
            }

            if (!int.TryParse((row.Hour.Text ?? string.Empty).Trim(), out int hour) || hour < 0 || hour > 23)
            {
                error = $"{row.ConfigKey} 小时格式错误（0-23）";
                return false;
            }

            if (!int.TryParse((row.Minute.Text ?? string.Empty).Trim(), out int minute) || minute < 0 || minute > 59)
            {
                error = $"{row.ConfigKey} 分钟格式错误（0-59）";
                return false;
            }

            bool enabled = row.Toggle.Text == InstrumentQCEnabledText;
            string concentration = row.Concentration == null
                ? "0"
                : (string.IsNullOrWhiteSpace(row.Concentration.Text) ? "0" : row.Concentration.Text.Trim());

            item = new InstrumentQCScheduleItem
            {
                ConfigKey = row.ConfigKey,
                FactorKey = row.FactorKey,
                CheckType = row.CheckType,
                Day = day,
                Hour = hour,
                Minute = minute,
                Enabled = enabled,
                Concentration = concentration,
                AnalyzerName = ResolveInstrumentQCAnalyzerName(row.FactorKey)
            };
            return true;
        }

        private void BtnInstrumentQCToggle_Click(object sender, EventArgs e)
        {
            if (sender is not UIButton btn || btn.Tag is not string configKey)
                return;

            if (!_instrumentQCRows.TryGetValue(configKey, out var row))
                return;

            if (!TryCollectInstrumentQCItem(row, out var item, out string error))
            {
                ShowTip.ShowErrorTip(error);
                return;
            }

            item.Enabled = !item.Enabled;
            if (!SaveInstrumentQCSchedule(item, out string saveError))
            {
                ShowTip.ShowErrorTip(saveError);
                return;
            }

            row.Toggle.Text = item.EnabledButtonText;
            ShowTip.ShowSuccessTip($"{item.AnalyzerName}{item.CheckTypeDisplayName}已{(item.Enabled ? "开启" : "关闭")}");
        }

        private void BtnInstrumentQCSaveAll_Click(object sender, EventArgs e)
        {
            if (_instrumentQCBinding)
                return;

            var items = new List<InstrumentQCScheduleItem>();
            foreach (var row in _instrumentQCRows.Values)
            {
                if (!TryCollectInstrumentQCItem(row, out var item, out string error))
                {
                    ShowTip.ShowErrorTip(error);
                    return;
                }
                items.Add(item);
            }

            if (!SaveInstrumentQCSchedules(items, out string saveError))
            {
                ShowTip.ShowErrorTip(saveError);
                return;
            }

            ShowTip.ShowSuccessTip("仪表质控配置已保存");
        }

        #endregion
    }
}
