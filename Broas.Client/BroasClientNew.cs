using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using WaterStation.Client.Model.UserDto;
using WaterStation.Client;
using WaterStation.Client.BatteryManageMoudle;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.Commom;
using WaterStation.Client.UserManageMoudle;

namespace Broas.Client
{
    public partial class BroasClientNew : UIHeaderAsideMainFooterFrame
    {
        private const string ConfigKeyStationName = "StationName";
        private const string PlaceholderText = "站名：双击编辑";
        private const int MaxStationNameLength = 40;
        private static readonly Color StationTitleColor = Color.White;
        private static readonly Color StationPlaceholderColor = Color.FromArgb(200, 220, 230);

        private readonly ToolTip _stationTip = new();
        private TextBox _stationEditBox;
        private bool _stationEditing;
        private bool _stationCommitting;

        private List<RoleFunctionInfoDto> roleFunctionInfos;
        private List<FunctionInfo> functionInfos;
        //private void MakeInk(string path, string error)
        //{
        //    if (!File.Exists(path))
        //    {
        //        try
        //        {
        //            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
        //            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(path);
        //            shortcut.TargetPath = Application.ExecutablePath;
        //            shortcut.WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        //            shortcut.Description = Application.ExecutablePath;
        //            shortcut.Save();
        //            if (!File.Exists(path))
        //                MessageBox.Show(error);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"error:{ex}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}
        public BroasClientNew()
        {
            var userinfo = Global.userInfoDto;//判断是否是直接通主界面加载
            if (userinfo == null)
                LoginInfo();//模拟用户登录 
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            InitStationNameTitle();
            this.Resize += BroasClientNew_Resize;
            UserControlIniti();
            MyMessager msg = new MyMessager();
            Application.AddMessageFilter(msg);
            ClientMonitortimer.Start();
            MainTabControl.ControlRemoved += MainTabControl_ControlRemoved;
            //MakeInk(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "水站系统.lnk"), "创建桌面快捷键失败");
            //string path = Path.Combine(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp", "水站系统.lnk");
            //MakeInk(path, "开机自启设置失败");
        }
        private void MainTabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            Form form = (Form)e.Control.Controls[0];
            form.Close();
        }
        /// <summary>
        /// 界面初始化设定
        /// 登陆成功后跳转到该页面
        /// </summary>
        private async void UserControlIniti()
        {
            roleFunctionInfos = await GetRoleFunctionInfoTree();
            functionInfos = await GetFunctionInfo();
            Global.functionInfoDtos = functionInfos;
            var x = await DataAPI.GetAllSystemParaAsync();
            if (x == null) return;
            foreach (var g in x)
            {
                Global.EQNames += g.EquipmentName;
            }
            CreateTreeNode();
            Aside.ShowOneNode = true;
            TreeNode node = new ();
            NavMenuItem item = new ();
            item.Text = "实时数据";
            //node.
            Aside_MenuItemClick(node, item, 1004);
            TreeNode node2 = new ();
            NavMenuItem item2 = new ();
            item2.Text = "调试";
            //node.
            Aside_MenuItemClick(node2, item2, 1004);
            this.lblCurentUser.Text = Global.userInfoDto.UserName;//获取当前登录人信息
            logHelper.WriteLogDB("操作", "进入系统");
        }
        /// <summary>
        /// 根据角色id 获取当前角色功能
        /// </summary>
        /// <returns></returns>
        private async Task<List<RoleFunctionInfoDto>> GetRoleFunctionInfoTree()
        {
            long roleID = Global.userInfoDto.RoleID;
            Global.roleFunctionInfoDtos = await DataAPI.GetAllRoleFunctionAsync();
            return await DataAPI.GetRoleFunctionByRoleidAsync(roleID);
        }
        /// <summary>
        /// 获取所有的功能模块信息
        /// lb 20220412
        /// </summary>
        /// <returns></returns>
        private async Task<List<FunctionInfo>> GetFunctionInfo()
        {
            return await DataAPI.GetAllFunctionAsync();
        }

        private void CreateTreeNode()
        {
            //TreeNode treeNode = new TreeNode();
            TreeNode parnent = null;
            int pageIndex = 1000;
            foreach (RoleFunctionInfoDto roleFunctionInfo in roleFunctionInfos)
            {
                //一级节点
                //Global.roleFunctionInfoDtos.AddRange(roleFunctionInfo.Child);
                string parent = functionInfos.Find(p => p.ID == roleFunctionInfo.FunctionID).FullName;
                int symbol = functionInfos.Find(p => p.ID == roleFunctionInfo.FunctionID).Symbol;
                parnent = Aside.CreateNode(parent, symbol, 30, pageIndex);
                //TreeNode parnent = Aside.CreateNode(parent, symbol, 22, pageIndex);
                //二级节点
                foreach (RoleFunctionInfoDto temp in roleFunctionInfo.Child)
                {
                    string child = functionInfos.Find(p => p.ID == temp.FunctionID).FullName;
                    int childSymbol = functionInfos.Find(p => p.ID == temp.FunctionID).Symbol;
                    //Aside.CreateChildNode(parnent, child, ++pageIndex);
                    Aside.CreateChildNode(parnent, childSymbol, 30, child, ++pageIndex);
                }
            }
            Aside.MenuItemClick += Aside_MenuItemClick;
        }
        /// <summary>
        /// 鼠标点击右侧菜单事件
        /// lb 20220727
        /// </summary>
        /// <param name="node"></param>
        /// <param name="item"></param>
        /// <param name="pageIndex"></param>
        private void Aside_MenuItemClick(TreeNode node, NavMenuItem item, int pageIndex)
        {
            if (item != null)
            {
                string menuText = item.Text;
                foreach (TabPage tab in MainTabControl.TabPages)
                {
                    if (tab.Text == menuText)
                    {
                        MainTabControl.SelectedTab = tab;
                        return;
                    }
                }
                string url = functionInfos.Find(p => p.FullName == menuText).URL;
                long functionid = functionInfos.Find(p => p.FullName == menuText).ID;//获取功能id lb 20220829
                //LoadFormString(MainTabControl, menuText, url); 20220829
                var functionInfo = Global.roleFunctionInfoDtos.Find(it => it.FunctionID == Convert.ToInt32(functionid) && it.RoleID == Global.userInfoDto.RoleID);
                if (functionInfo == null)
                    MessageBox.Show("您没有此功能权限请联系管理员");
                else
                    LoadFormString(MainTabControl, menuText, url);
            }
        }

        private void LoadFormString(TabControl maintabControl, string menuText, string url)
        {
            if (String.IsNullOrEmpty(url)) return;
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string className = assemblyName + "." + url;
            Form form = (Form)Assembly.Load(assemblyName).CreateInstance(className);
            form.Text = menuText;
            form.FormBorderStyle = FormBorderStyle.None;
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            TabPage tabPage = new (menuText);
            tabPage.Font = new ("宋体", 9F);
            tabPage.Controls.Add(form);
            maintabControl.Controls.Add(tabPage);
            maintabControl.SelectedTab = tabPage;
            form.Show();
        }

        private void ZoonoClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_stationEditing)
                CancelStationEdit();

            if (closeClient) Environment.Exit(0);
            //DialogResult result = MessageBox.Show("是否退出系统?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (result == DialogResult.OK)
            //{
            //    this.Dispose();
            //    Application.Exit();

            //}
            //else
            //{
            //    e.Cancel = true;
            //}
            if (!ShowAskDialog("是否退出系统?"))
                e.Cancel = true;
            else
                logHelper.WriteLogDB("操作", "用户退出系统");
        }

        static int iOperCount = 0;
        private bool closeClient = false;
        internal class MyMessager : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                //如果检测到有鼠标或则键盘的消息，则使计数为0.
                if (m.Msg == 0x0200 || m.Msg == 0x0201 || m.Msg == 0x0204 || m.Msg == 0x0207)
                    iOperCount = 0;
                return false;
            }
        }
        private void ClientMonitortimer_Tick(object sender, EventArgs e)
        {
            try
            {
                iOperCount++;
                if (iOperCount > 36000000)
                {
                    ClientMonitortimer.Stop();
                    //DialogResult result = MessageBox.Show("是否重新登陆系统?", "操作提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    //if (result == DialogResult.OK)
                    if (ShowAskDialog("系统提示", "超时,是否重新登陆系统?"))
                    {
                        Global.ReStart = 1;
                        closeClient = true;
                        this.Close();
                    }
                    else
                    {
                        closeClient = true;
                        Application.Exit();
                        this.Dispose();
                    }
                }
            }
            catch
            {

            }
        }

        private void uiAvatar1_Click(object sender, EventArgs e)
        {
            FormUpdatePassword formUpdate = new ();
            formUpdate.ShowDialog();
        }
        /// <summary>
        /// 系统退出功能
        /// lb 20220711
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (ShowAskDialog("系统提示", "是否注销系统?"))
            {
                #region 模拟注销
                Login login = new ();
                login.ShowDialog();
                if (login.DialogResult == DialogResult.OK)
                {
                    login.Close();
                    login.Dispose();
                }
                else
                {
                    login.Close();
                    login.Dispose();
                }
                UserControlIniti();//重新加载登录相关信息 lb 20220829
                #endregion
                //Login login = new Login();
                //login.ShowDialog();
            }
            else
            {
                //closeClient = true;
                //Application.Exit();
                //this.Dispose();
            }
        }


        /// <summary>
        /// 模拟点击登录按钮
        /// lb 20220727
        /// </summary>    
        private void LoginInfo()
        {
            try
            {
                string defaultUser = ConfigurationManager.AppSettings["default"];
                var defaultArr = defaultUser.Split(';');
                string username = defaultArr[0];// "admin";
                string password = defaultArr[1];// "1";
                var userInfoDto = DataAPI.LoginAsync(username, password);
                var userinfoData = userInfoDto.Result;
                Global.userInfoDto = userinfoData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region 顶部站名

        /// <summary>
        /// 初始化标题下方可编辑站名
        /// </summary>
        private void InitStationNameTitle()
        {
            try
            {
                EnsureStationNameConfig();
                BindStationName();
                _stationTip.SetToolTip(lblStationName, "双击编辑站名");
                CenterHeaderTitles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化站名失败：" + ex.Message);
            }
        }

        private void lblStationName_DoubleClick(object sender, EventArgs e)
        {
            BeginStationEdit(ReadAppSetting(ConfigKeyStationName));
        }

        /// <summary>
        /// 双击站名后弹出文本框编辑
        /// </summary>
        private void BeginStationEdit(string currentValue)
        {
            try
            {
                if (_stationEditing || lblStationName == null || lblStationName.IsDisposed)
                    return;

                _stationEditing = true;
                lblStationName.Visible = false;

                _stationEditBox = new TextBox
                {
                    Location = lblStationName.Location,
                    Size = lblStationName.Size,
                    Font = lblStationName.Font,
                    Text = currentValue ?? string.Empty,
                    MaxLength = MaxStationNameLength,
                    BorderStyle = BorderStyle.FixedSingle,
                    TextAlign = HorizontalAlignment.Center
                };
                _stationEditBox.KeyDown += StationEditBox_KeyDown;
                _stationEditBox.LostFocus += StationEditBox_LostFocus;
                Header.Controls.Add(_stationEditBox);
                _stationEditBox.BringToFront();
                _stationEditBox.Focus();
                _stationEditBox.SelectAll();
            }
            catch (Exception)
            {
                CancelStationEdit();
                ShowTip.ShowErrorTip("无法进入编辑，请稍后重试");
            }
        }

        private void StationEditBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CommitStationEdit();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                CancelStationEdit();
                BindStationName();
            }
        }

        private void StationEditBox_LostFocus(object sender, EventArgs e)
        {
            CommitStationEdit();
        }

        /// <summary>
        /// 校验并保存站名；校验失败时保持编辑状态
        /// </summary>
        private void CommitStationEdit()
        {
            if (!_stationEditing || _stationEditBox == null || _stationCommitting)
                return;

            _stationCommitting = true;
            try
            {
                string newValue = _stationEditBox.Text?.Trim() ?? string.Empty;
                if (!TryValidateStationName(newValue, out string error))
                {
                    ShowTip.ShowErrorTip(error);
                    return;
                }

                string oldValue = ReadAppSetting(ConfigKeyStationName);
                CancelStationEdit();
                if (string.Equals(oldValue, newValue, StringComparison.Ordinal))
                {
                    BindStationName();
                    return;
                }

                if (!SaveStationName(newValue))
                {
                    ShowTip.ShowErrorTip("保存失败，请稍后重试");
                    BindStationName();
                    return;
                }

                BindStationName();
                ShowTip.ShowSuccessTip("站名已保存");
            }
            catch (Exception)
            {
                CancelStationEdit();
                BindStationName();
                ShowTip.ShowErrorTip("保存失败，请稍后重试");
            }
            finally
            {
                _stationCommitting = false;
                if (_stationEditing && _stationEditBox != null && !_stationEditBox.IsDisposed)
                    _stationEditBox.Focus();
            }
        }

        private void CancelStationEdit()
        {
            if (!_stationEditing && _stationEditBox == null)
                return;

            _stationEditing = false;
            if (_stationEditBox != null)
            {
                _stationEditBox.KeyDown -= StationEditBox_KeyDown;
                _stationEditBox.LostFocus -= StationEditBox_LostFocus;
                Header.Controls.Remove(_stationEditBox);
                _stationEditBox.Dispose();
                _stationEditBox = null;
            }
            if (lblStationName != null && !lblStationName.IsDisposed)
                lblStationName.Visible = true;
        }

        private void BindStationName()
        {
            if (lblStationName == null || lblStationName.IsDisposed)
                return;
            string value = ReadAppSetting(ConfigKeyStationName);
            bool empty = string.IsNullOrWhiteSpace(value);
            lblStationName.Text = empty ? PlaceholderText : "站名：" + value;
            lblStationName.ForeColor = empty ? StationPlaceholderColor : StationTitleColor;
            CenterHeaderTitles();
        }

        private void BroasClientNew_Resize(object sender, EventArgs e)
        {
            CenterHeaderTitles();
            if (_stationEditing && _stationEditBox != null && !_stationEditBox.IsDisposed)
            {
                _stationEditBox.Location = lblStationName.Location;
                _stationEditBox.Size = lblStationName.Size;
            }
        }

        /// <summary>
        /// 系统标题与站名水平居中到 Header
        /// </summary>
        private void CenterHeaderTitles()
        {
            if (Header == null)
                return;
            if (uiLabel1 != null && !uiLabel1.IsDisposed)
            {
                uiLabel1.Left = Math.Max(0, (Header.ClientSize.Width - uiLabel1.Width) / 2);
                uiLabel1.Top = 2;
            }
            if (lblStationName != null && !lblStationName.IsDisposed)
            {
                lblStationName.Left = Math.Max(0, (Header.ClientSize.Width - lblStationName.Width) / 2);
                lblStationName.Top = 42;
            }
        }

        /// <summary>
        /// 站名：非空、不含控制字符，最长 40
        /// </summary>
        private static bool TryValidateStationName(string value, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(value))
            {
                error = "站名不能为空";
                return false;
            }
            if (value.Length > MaxStationNameLength)
            {
                error = $"站名不能超过{MaxStationNameLength}个字符";
                return false;
            }
            foreach (char c in value)
            {
                if (char.IsControl(c))
                {
                    error = "站名不能包含控制字符";
                    return false;
                }
            }
            return true;
        }

        private bool SaveStationName(string value)
        {
            try
            {
                if (!SaveAppSetting(ConfigKeyStationName, value))
                    return false;
                logHelper.WriteLogDB("操作", $"修改站名为{value}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string ReadAppSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key]?.Trim() ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 写入 App.config 并刷新内存缓存；同时同步调试页已打开的 Configuration 对象
        /// </summary>
        private static bool SaveAppSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            if (FormSLPLC.configuration != null)
            {
                if (FormSLPLC.configuration.AppSettings.Settings[key] == null)
                    FormSLPLC.configuration.AppSettings.Settings.Add(key, value);
                else
                    FormSLPLC.configuration.AppSettings.Settings[key].Value = value;
            }
            return true;
        }

        /// <summary>
        /// 旧配置没有 StationName 时补上，避免首次保存失败
        /// </summary>
        private void EnsureStationNameConfig()
        {
            try
            {
                if (ConfigurationManager.AppSettings.AllKeys != null
                    && Array.IndexOf(ConfigurationManager.AppSettings.AllKeys, ConfigKeyStationName) >= 0)
                    return;
                SaveAppSetting(ConfigKeyStationName, string.Empty);
            }
            catch (Exception)
            {
                // 忽略补齐失败，首次保存时再尝试写入
            }
        }

        #endregion
    }
}
