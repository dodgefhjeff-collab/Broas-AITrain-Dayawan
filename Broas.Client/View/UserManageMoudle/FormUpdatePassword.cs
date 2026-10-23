using Sunny.UI;
using System;
using WaterStation.Client.Controller;
using System.Configuration;

namespace WaterStation.Client.UserManageMoudle
{
    public partial class FormUpdatePassword : UIEditForm
    {
        public FormUpdatePassword()
        {
            InitializeComponent();
            ButtonOkClick += FormUpdatePassword_ButtonOkClick;
            UserName = Global.userInfoDto.UserName;
        }

        protected override bool CheckData()
        {
            return CheckEmpty(txtpassword, "请输入密码")
                   && CheckEmpty(txtpassword1, "请再次确认密码");
        }

        private string username;
        public string UserName
        {
            get
            {
                if (username == null)
                    username = txtusername.Text.Trim();
                return username;
            }
            set
            {
                username = value;
                txtusername.Text = username;
            }
        }

        public string Password
        {
            get { return txtpassword.Text.Trim(); }
        }

        public string Password1
        {
            get { return txtpassword1.Text.Trim(); }
        }


        private async void FormUpdatePassword_ButtonOkClick(object sender, EventArgs e)
        {
            try
            {
                if (Password.Length > 8 || Password1.Length > 8)
                {
                    ShowTip.ShowErrorTip("密码长度不超过8位");
                    return;
                }

                if (Password != Password)
                {
                    ShowTip.ShowErrorTip("两次输入密码不相同");
                    return;
                }

                var headerResult = await DataAPI.UpdatePasswordAsync(Global.userInfoDto.ID, Password);
                if (headerResult)
                {
                    if (username == "admin")
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings["default"].Value = $"{username};{Password}";
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    ShowTip.ShowSuccessTip("修改成功");
                }
                else
                    ShowTip.ShowSuccessTip("修改失败");
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
        }
    }
}
