using Sunny.UI;
using System;
using System.Windows.Forms;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.Commom;

namespace WaterStation.Client
{
    public partial class Login : UILoginForm
    {
        public Login()
        {
            InitializeComponent();
            ButtonLoginClick += Login_ButtonLoginClick;
            ButtonCancelClick += Login_ButtonCancelClick;
        }


        private void Login_ButtonCancelClick(object sender, EventArgs e)
        {
            logHelper.WriteLogDB("操作", "用户取消登录");
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// 点击登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Login_ButtonLoginClick(object sender, EventArgs e)
        {
            try
            {
                string username = UserName;
                string password = Password;
                LoginMessage.Text = string.Empty;
                var userInfoDto = await DataAPI.LoginAsync(username, password);
                if (userInfoDto != null)
                {
                    this.DialogResult = DialogResult.OK;
                    Global.userInfoDto = userInfoDto;
                    logHelper.WriteLogDB("操作", "用户登录成功");
                }
                else
                    LoginMessage.Text = "登录失败";
            }
            catch
            {
                LoginMessage.Text = "调用WepApi异常";
            }
        }
    }
}
