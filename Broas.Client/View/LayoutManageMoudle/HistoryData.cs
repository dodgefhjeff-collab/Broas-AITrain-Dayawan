using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using NPOI.SS.UserModel;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using WaterStation.Client.Model.LayoutDto;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.Commom;
using NLog;

namespace WaterStation.Client.LayoutManageMoudle
{
    /// <summary>
    /// 数据采集查询
    /// </summary>
    public partial class HistoryData : UIForm
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        List<ResultMeasureDataInfoDto> listYB = new();
        List<ResultMeasureDataInfoDto> listAI = new();

        public HistoryData()
        {
            InitializeComponent();
            dpStartTime.Text = null;
            dateEndTime.Text = null;
            InitializeDataGridViewWC(dgvAI, true);
            uiPagination2.TotalCount = 50;
            cmbTimeType.SelectedIndex = 0;
            _ = GetDeviceData();
            dpStartTimeYB.Text = null;
            dateEndTimeYB.Text = null;
            dgvYB.AutoGenerateColumns = false;
            uiPagination1.TotalCount = 50;
            _ = GetDeviceDataYB();
            GetYBDisplay();
            dgvAI.CellDoubleClick += dgvAI_CellDoubleClick;
        }

        private void GetYBDisplay()
        {
            try
            {
                if (Global.EQNames.Contains("总氮"))
                {
                    dgvAI.Columns["dgvColumnZD"].Visible = true;
                    dgvYB.Columns["dgvYBColumnZD"].Visible = true;
                    dgvYB.Columns["dgvYBColumnZDTime"].Visible = true;
                }
                if (Global.EQNames.Contains("氨氮"))
                {
                    dgvAI.Columns["dgvColumnAD"].Visible = true;
                    dgvYB.Columns["dgvYBColumnAD"].Visible = true;
                    dgvYB.Columns["dgvYBColumnADTime"].Visible = true;
                }
                if (Global.EQNames.Contains("COD"))
                {
                    dgvAI.Columns["dgvColumnCOD"].Visible = true;
                    dgvYB.Columns["dgvYBColumnCOD"].Visible = true;
                    dgvYB.Columns["dgvYBColumnCODTime"].Visible = true;
                }
                if (Global.EQNames.Contains("总磷"))
                {
                    dgvAI.Columns["dgvColumnZL"].Visible = true;
                    dgvYB.Columns["dgvYBColumnZL"].Visible = true;
                    dgvYB.Columns["dgvYBColumnZLTime"].Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
                _logger.Error(ex, "获取设备显示配置时发生异常");
            }
        }

        private bool Init = true;
        private DataGridView dataGridViewWC;

        private string GetAiDataType() =>
            cmbTimeType.SelectedIndex == 0 ? "AI数据" : "AI周期数据";

        private QueryMeasureDataInfoDto BuildAiQuery(int pageIndex, int pageSize)
        {
            var dto = new QueryMeasureDataInfoDto
            {
                DataType = GetAiDataType(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            if (!string.IsNullOrEmpty(dpStartTime.Text))
                dto.SatrtTime = dpStartTime.Value;
            if (!string.IsNullOrEmpty(dateEndTime.Text))
                dto.EndTime = dateEndTime.Value;
            return dto;
        }

        private QueryMeasureDataInfoDto BuildYbQuery(int pageIndex, int pageSize)
        {
            var dto = new QueryMeasureDataInfoDto
            {
                DataType = "仪表数据",
                Remark = "周期",
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            if (!string.IsNullOrEmpty(dpStartTimeYB.Text))
                dto.SatrtTime = dpStartTimeYB.Value;
            if (!string.IsNullOrEmpty(dateEndTimeYB.Text))
                dto.EndTime = dateEndTimeYB.Value;
            return dto;
        }

        private async Task<bool> QueryAndBindAsync(
            QueryMeasureDataInfoDto query,
            DataGridView grid,
            Sunny.UI.UIPagination pagination,
            string logContext,
            bool treatEmptyAsFailure = false)
        {
            var headerResult = await DataAPI.GetMeasureDataPageAsync(query);
            if (headerResult?.IsSucceed == true)
            {
                grid.DataSource = headerResult.Result;
                pagination.TotalCount = headerResult.Total;
                if (treatEmptyAsFailure && headerResult.Total == 0)
                {
                    ShowTip.ShowErrorTip("暂无数据!");
                    return false;
                }
                return true;
            }

            grid.DataSource = null;
            pagination.TotalCount = 0;
            _logger.Warn("{Context}查询失败", logContext);
            ShowTip.ShowErrorTip("查询失败或暂无数据!");
            return false;
        }
        private void InitializeDataGridViewWC(DataGridView dataGrid, bool singleChoose)
        {
            dataGridViewWC = dataGrid;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AutoGenerateColumns = false;
            int index = 0;
            DataGridViewCheckBoxColumn colCB = new();
            if (singleChoose)
            {
                colCB.HeaderText = "选择";
                dataGrid.Columns.Insert(index, colCB);
                return;
            }
            DatagridViewWCCheckBoxHeaderCell cbHeader = new();
            colCB.HeaderCell = cbHeader;
            colCB.HeaderText = "";
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeaderWC_OnCheckBoxClicked);
            dataGrid.Columns.Insert(index, colCB);
            dataGrid.Columns[index].Width = 50;
        }
        private void cbHeaderWC_OnCheckBoxClicked(bool state)
        {
            //结束编辑状态
            dataGridViewWC.EndEdit();
            dataGridViewWC.Rows.OfType<DataGridViewRow>().ToList().ForEach(t => t.Cells[0].Value = state);
        }
        public delegate void CheckBoxClickedHandler(bool state);
        public class DataGridViewCheckBoxHeaderCellEventArgs : EventArgs
        {
            bool _bChecked;
            public DataGridViewCheckBoxHeaderCellEventArgs(bool bChecked)
            {
                _bChecked = bChecked;
            }
            public bool Checked
            {
                get { return _bChecked; }
            }
        }
        public class DatagridViewWCCheckBoxHeaderCell : DataGridViewColumnHeaderCell
        {
            Point checkBoxLocation;
            Size checkBoxSize;
            bool _checked = false;
            Point _cellLocation = new Point();
            System.Windows.Forms.VisualStyles.CheckBoxState _cbState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;

            public event CheckBoxClickedHandler OnCheckBoxClicked;

            public DatagridViewWCCheckBoxHeaderCell()
            {
            }

            protected override void Paint(Graphics graphics,
                Rectangle clipBounds,
                Rectangle cellBounds,
                int rowIndex,
                DataGridViewElementStates dataGridViewElementState,
                object value,
                object formattedValue,
                string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    dataGridViewElementState, value,
                    formattedValue, errorText, cellStyle,
                    advancedBorderStyle, paintParts);
                Point p = new Point();
                Size s = CheckBoxRenderer.GetGlyphSize(graphics,
                System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
                p.X = cellBounds.Location.X +
                    (cellBounds.Width / 2) - (s.Width / 2);
                p.Y = cellBounds.Location.Y +
                    (cellBounds.Height / 2) - (s.Height / 2);
                _cellLocation = cellBounds.Location;
                checkBoxLocation = p;
                checkBoxSize = s;
                if (_checked)
                    _cbState = System.Windows.Forms.VisualStyles.
                        CheckBoxState.CheckedNormal;
                else
                    _cbState = System.Windows.Forms.VisualStyles.
                        CheckBoxState.UncheckedNormal;
                CheckBoxRenderer.DrawCheckBox
                (graphics, checkBoxLocation, _cbState);
            }

            protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
            {
                Point p = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);
                if (p.X >= checkBoxLocation.X && p.X <=
                    checkBoxLocation.X + checkBoxSize.Width
                && p.Y >= checkBoxLocation.Y && p.Y <=
                    checkBoxLocation.Y + checkBoxSize.Height)
                {
                    _checked = !_checked;
                    if (OnCheckBoxClicked != null)
                    {
                        OnCheckBoxClicked(_checked);
                        this.DataGridView.InvalidateCell(this);
                    }
                }
                base.OnMouseClick(e);
            }
        }
        private async Task<bool> GetDeviceData()
        {
            try
            {
                var query = BuildAiQuery(1, uiPagination2.PageSize);
                return await QueryAndBindAsync(query, dgvAI, uiPagination2, "AI数据", treatEmptyAsFailure: true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "获取AI数据时发生异常");
                ShowTip.ShowErrorTip($"查询异常: {ex.Message}");
                return false;
            }
            finally
            {
                Init = false;
                UIWaitFormService.HideWaitForm();
            }
        }

        private async Task<bool> GetDeviceDataYB()
        {
            try
            {
                var query = BuildYbQuery(1, uiPagination1.PageSize);
                return await QueryAndBindAsync(query, dgvYB, uiPagination1, "仪表数据", treatEmptyAsFailure: true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "获取仪表数据时发生异常");
                ShowTip.ShowErrorTip($"查询异常: {ex.Message}");
                return false;
            }
            finally
            {
                Init = false;
                UIWaitFormService.HideWaitForm();
            }
        }

        private async void uiPagination1_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            if (Init) return;
            try
            {
                var query = BuildYbQuery(pageIndex, uiPagination1.PageSize);
                await QueryAndBindAsync(query, dgvYB, uiPagination1, "仪表数据翻页");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "仪表数据翻页查询时发生异常");
            }
            finally
            {
                UIWaitFormService.HideWaitForm();
            }
        }

        private async void uiPagination2_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            if (Init) return;
            try
            {
                var query = BuildAiQuery(pageIndex, uiPagination2.PageSize);
                await QueryAndBindAsync(query, dgvAI, uiPagination2, "AI数据翻页");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "AI数据翻页查询时发生异常");
            }
            finally
            {
                UIWaitFormService.HideWaitForm();
            }
        }

        private async void btnSearchWC_Click(object sender, EventArgs e)
        {
            if (await GetDeviceData())
                ShowTip.ShowSuccessTip("查询成功");
        }
        private async Task getAllAIdata()
        {
            var query = BuildAiQuery(1, int.MaxValue);
            var headerResult = await DataAPI.GetMeasureDataPageAsync(query);
            if (headerResult?.IsSucceed == true)
                listAI = headerResult.Result ?? new List<ResultMeasureDataInfoDto>();
        }

        private async Task getAllYBdata()
        {
            var query = BuildYbQuery(1, int.MaxValue);
            var headerResult = await DataAPI.GetMeasureDataPageAsync(query);
            if (headerResult?.IsSucceed == true)
                listYB = headerResult.Result ?? new List<ResultMeasureDataInfoDto>();
        }

        private async void btnExcelAI_Click(object sender, EventArgs e)
        {
            await getAllAIdata();
            //要显示的列（选择你要显示的列，不一定是整个对象的列）
            Dictionary<string, string> dic = new() { { "CollectTime", "采集时间" } };
            if (Global.EQNames.Contains("总氮"))
                dic.Add("TotalNitrogenVal", "总氮");
            if (Global.EQNames.Contains("氨氮"))
                dic.Add("AmmoniaADValue", "氨氮");
            if (Global.EQNames.Contains("COD"))
                dic.Add("TotalCODVal", "COD");
            if (Global.EQNames.Contains("总磷"))
                dic.Add("TotalPhosphorusVal", "总磷");
            string FileName = "AI数据" + DateTime.Now.GetHashCode().ToString();
            SaveFileDialog saveFileDialog = new()
            {
                //设置文件标题
                Title = "导出Excel文件",
                //设置文件类型
                Filter = "Excel 工作簿(*.xlsx)|*.xlsx|Excel 97-2003 工作簿(*.xls)|*.xls",
                //设置默认文件类型显示顺序  
                FilterIndex = 1,
                //是否自动在文件名中添加扩展名
                AddExtension = true,
                //是否记忆上次打开的目录
                RestoreDirectory = true,
                //设置默认文件名
                FileName = FileName
            };
            //按下确定选择的按钮  
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径 
                string localFilePath = saveFileDialog.FileName.ToString();
                //调用通用方法，重要得是理解如何每个sheet页、每行、每列的赋值
                PageSaveToXls(localFilePath, listAI, dic);
            }
        }

        private async void btnSearchYB_Click(object sender, EventArgs e)
        {
            if (await GetDeviceDataYB())
                ShowTip.ShowSuccessTip("查询成功");
        }

        private async void btnExcelYB_Click(object sender, EventArgs e)
        {
            await getAllYBdata();
            Dictionary<string, string> dic = new() { { "CollectTime", "采集时间" } };
            //, { "TotalNitrogenVal", "总氮" }, { "AmmoniaADValue", "氨氮" }, { "TotalCODVal", "COD" }, { "TotalPhosphorusVal", "总磷" }, { "XSYDVal", "COD-Mn" }, { "TurbidityVal", "浊度" } 
            if (Global.EQNames.Contains("总氮"))
            {
                dic.Add("TotalNitrogenVal", "总氮");
                dic.Add("TotalNitrogenTime", "总氮测试时间");
            }
            if (Global.EQNames.Contains("氨氮"))
            {
                dic.Add("AmmoniaADValue", "氨氮");
                dic.Add("AmmoniaADTime", "氨氮测试时间");
            }
            if (Global.EQNames.Contains("COD"))
            {
                dic.Add("TotalCODVal", "COD");
                dic.Add("TotalCODTime", "COD测试时间");
            }
            if (Global.EQNames.Contains("总磷"))
            {
                dic.Add("TotalPhosphorusVal", "总磷");
                dic.Add("TotalPhosphorustime", "总磷测试时间");
            }
            string FileName = "仪表数据" + DateTime.Now.GetHashCode().ToString();
            SaveFileDialog saveFileDialog = new()
            {
                //设置文件标题
                Title = "导出Excel文件",
                //设置文件类型
                Filter = "Excel 工作簿(*.xlsx)|*.xlsx|Excel 97-2003 工作簿(*.xls)|*.xls",
                //设置默认文件类型显示顺序  
                FilterIndex = 1,
                //是否自动在文件名中添加扩展名
                AddExtension = true,
                //是否记忆上次打开的目录
                RestoreDirectory = true,
                //设置默认文件名
                FileName = FileName
            };
            //按下确定选择的按钮  
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //获得文件路径 
                string localFilePath = saveFileDialog.FileName.ToString();
                //调用通用方法，重要得是理解如何每个sheet页、每行、每列的赋值
                PageSaveToXls(localFilePath, listYB, dic);
            }
        }

        private void uiPanel4_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 保存到excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">保存路径</param>
        /// <param name="list">要存存储得集合</param>
        /// <param name="dic">字段名</param>
        /// <returns></returns>
        public static bool PageSaveToXls<T>(string path, List<T> list, Dictionary<string, string> dic)
        {
            bool isSave = false;
            IWorkbook book = null;
            FileStream stream = null;
            Process process = null;

            try
            {
                if (list == null || list.Count == 0)
                    return isSave;

                //excel的sheet页
                ISheet sheet = null;
                //excel的sheet页的每一行
                IRow row = null;

                stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                //扩展名
                string extension = Path.GetExtension(path);
                switch (extension.ToLower())
                {
                    case ".xls":
                        book = new HSSFWorkbook();//NPOI.dll
                        break;
                    case ".xlsx":
                        book = new XSSFWorkbook();//NPOI.OOXML.dll
                        break;
                }

                Type val = null;
                //所有列
                List<string> fields = dic.Keys.ToList();
                //所有列对应的名称
                List<string> fieldsName = dic.Values.ToList();
                int columnIndex = dic.Count;
                int rowIndex = 0;
                int page = 1;

                foreach (T item in list)
                {
                    val = item.GetType();
                    //写表头
                    if (rowIndex == 0)
                    {
                        sheet = book.CreateSheet("Sheet" + page);
                        row = sheet.CreateRow(rowIndex);
                        for (int i = 0; i < columnIndex; i++)
                            row.CreateCell(i, CellType.String).SetCellValue(fieldsName[i]);
                        rowIndex++;
                    }
                    //写数据
                    row = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < columnIndex; i++)
                        row.CreateCell(i, CellType.String).SetCellValue(Convert.ToString(val.GetProperty(fields[i]).GetValue(item)));
                    rowIndex++;
                    //大于60000行数据就换页
                    if (rowIndex > 60000)
                    {
                        rowIndex = 0;
                        page++;
                    }
                }

                book.Write(stream);
                isSave = true;

                //成功提示
                if (MessageBox.Show("导出成功，是否立即打开？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    process = new Process();
                    ProcessStartInfo processStartInfo = new(path)
                    {
                        UseShellExecute = true
                    };
                    process.StartInfo = processStartInfo;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                //输出日志
                ShowTip.ShowErrorTip($"导出Excel失败: {ex.Message}");
            }
            finally
            {
                // 确保资源被正确释放
                try
                {
                    stream?.Close();
                    stream?.Dispose();
                }
                catch { }

                try
                {
                    // IWorkbook没有实现IDisposable,但需要清理
                    book = null;
                }
                catch { }

                try
                {
                    process?.Dispose();
                }
                catch { }

                // 强制垃圾回收(仅在导出大量数据后)
                if (list != null && list.Count > 10000)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            return isSave;
        }

        private void dgvAI_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex == 0)
                return;

            if (cmbTimeType.SelectedIndex != 0)
            {
                ShowTip.ShowErrorTip("请切换到「分钟」类型后查看吸光度曲线");
                return;
            }

            DateTime? collectTime = GetRowCollectTime(e.RowIndex);
            if (!collectTime.HasValue)
            {
                ShowTip.ShowErrorTip("无法读取该行的采集时间");
                return;
            }

            ShowXgdCurve(collectTime.Value);
        }

        private DateTime? GetRowCollectTime(int rowIndex)
        {
            if (dgvAI.Rows[rowIndex].DataBoundItem is ResultMeasureDataInfoDto dto)
                return dto.CollectTime;

            object cellVal = dgvAI.Rows[rowIndex].Cells["dataGridViewTextBoxColumn4"].Value;
            if (cellVal == null || cellVal == DBNull.Value)
                return null;

            return Convert.ToDateTime(cellVal);
        }

        private void ShowXgdCurve(DateTime collectTime)
        {
            try
            {
                var seriesList = LoadXgdCurvePoints(collectTime);
                if (seriesList.Count == 0)
                {
                    ShowTip.ShowErrorTip($"该分钟（{collectTime:yyyy/M/d HH:mm}）暂无吸光度日志数据");
                    return;
                }

                using var popup = new XgdCurvePopup(collectTime, seriesList);
                popup.ShowDialog(this);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "加载吸光度曲线失败");
                ShowTip.ShowErrorTip("加载吸光度曲线失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 从本地吸光度日志加载指定分钟内的光谱曲线点
        /// </summary>
        private static List<XgdCurveSeries> LoadXgdCurvePoints(DateTime collectTime)
        {
            var result = new List<XgdCurveSeries>();
            DateTime minuteStart = new(
                collectTime.Year, collectTime.Month, collectTime.Day,
                collectTime.Hour, collectTime.Minute, 0);
            DateTime minuteEnd = minuteStart.AddMinutes(1);

            string logPath = logHelper.ResolveAbsorbanceLogPath(minuteStart);
            if (!File.Exists(logPath))
                return result;

            const string lineSeparator = "---";
            string[] lines = File.ReadAllLines(logPath, Encoding.UTF8);

            foreach (string rawLine in lines)
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                    continue;

                int sepIndex = rawLine.IndexOf(lineSeparator, StringComparison.Ordinal);
                if (sepIndex < 0)
                    continue;

                string timePart = rawLine.Substring(0, sepIndex).Trim();
                string payload = rawLine.Substring(sepIndex + lineSeparator.Length).Trim();
                if (payload.StartsWith("#", StringComparison.Ordinal))
                    continue;

                if (!TryParseAbsorbanceLogTime(timePart, out DateTime logTime))
                    continue;
                if (logTime < minuteStart || logTime >= minuteEnd)
                    continue;

                var values = ParseAbsorbanceValues(payload);
                if (values.Count == 0)
                    continue;

                result.Add(new XgdCurveSeries
                {
                    SnapshotTime = logTime,
                    Values = values
                });
            }

            return result;
        }

        private static bool TryParseAbsorbanceLogTime(string timePart, out DateTime logTime)
        {
            if (DateTime.TryParseExact(timePart, "yyyy-MM-dd HH-mm-ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out logTime))
                return true;

            return DateTime.TryParse(timePart, out logTime);
        }

        private static List<double> ParseAbsorbanceValues(string payload)
        {
            var values = new List<double>();
            string[] parts = payload.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                if (double.TryParse(part.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double v))
                    values.Add(v);
            }
            return values;
        }
    }
}
