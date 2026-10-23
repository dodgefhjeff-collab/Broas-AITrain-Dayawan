
namespace WaterStation.Client
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LoginMessage = new Sunny.UI.UIScrollingText();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(0, 20);
            this.lblTitle.Size = new System.Drawing.Size(318, 32);
            this.lblTitle.Text = "户外小型站管理系统";
            // 
            // uiPanel1
            // 
            this.uiPanel1.Location = new System.Drawing.Point(425, 123);
            this.uiPanel1.Size = new System.Drawing.Size(208, 246);
            // 
            // lblSubText
            // 
            this.lblSubText.Location = new System.Drawing.Point(434, 424);
            this.lblSubText.Style = Sunny.UI.UIStyle.Custom;
            this.lblSubText.Text = "深圳市博瑞思集团有限公司";
            // 
            // LoginMessage
            // 
            this.LoginMessage.BackColor = System.Drawing.Color.Transparent;
            this.LoginMessage.FillColor = System.Drawing.Color.Transparent;
            this.LoginMessage.FillDisableColor = System.Drawing.Color.Transparent;
            this.LoginMessage.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LoginMessage.ForeColor = System.Drawing.Color.Red;
            this.LoginMessage.Location = new System.Drawing.Point(423, 381);
            this.LoginMessage.MinimumSize = new System.Drawing.Size(1, 1);
            this.LoginMessage.Name = "LoginMessage";
            this.LoginMessage.RectColor = System.Drawing.Color.Transparent;
            this.LoginMessage.Size = new System.Drawing.Size(208, 23);
            this.LoginMessage.Style = Sunny.UI.UIStyle.Custom;
            this.LoginMessage.TabIndex = 12;
            this.LoginMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Login
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(750, 450);
            this.Controls.Add(this.LoginMessage);
            this.Name = "Login";
            this.Style = Sunny.UI.UIStyle.Custom;
            this.SubText = "深圳市博瑞思集团有限公司";
            this.Text = "智佳能仓库数据管理系统";
            this.Title = "户外小型站管理系统";
            this.TopMost = true;
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.lblSubText, 0);
            this.Controls.SetChildIndex(this.uiPanel1, 0);
            this.Controls.SetChildIndex(this.LoginMessage, 0);
            this.ResumeLayout(false);

        }

        #endregion
        private Sunny.UI.UIScrollingText LoginMessage;
    }
}