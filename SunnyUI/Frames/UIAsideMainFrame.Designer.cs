namespace Sunny.UI
{
    partial class UIAsideMainFrame
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
            this.Aside = new Sunny.UI.UINavMenu();
            this.UIPanelaside = new Sunny.UI.UIPanel();
            this.UIPanelaside.SuspendLayout();
            this.SuspendLayout();
            // 
            // Aside
            // 
            this.Aside.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Aside.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.Aside.ExpandSelectFirst = true;
            this.Aside.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Aside.FullRowSelect = true;
            this.Aside.ItemHeight = 100;
            this.Aside.Location = new System.Drawing.Point(3, 35);
            this.Aside.MenuStyle = Sunny.UI.UIMenuStyle.Custom;
            this.Aside.Name = "Aside";
            this.Aside.ShowLines = false;
            this.Aside.Size = new System.Drawing.Size(187, 915);
            this.Aside.Style = Sunny.UI.UIStyle.Custom;
            this.Aside.TabIndex = 1;
            // 
            // UIPanelaside
            // 
            this.UIPanelaside.BackColor = System.Drawing.Color.Transparent;
            this.UIPanelaside.Controls.Add(this.Aside);
            this.UIPanelaside.Dock = System.Windows.Forms.DockStyle.Left;
            this.UIPanelaside.FillColor = System.Drawing.Color.Transparent;
            this.UIPanelaside.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.UIPanelaside.Location = new System.Drawing.Point(0, 35);
            this.UIPanelaside.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UIPanelaside.MinimumSize = new System.Drawing.Size(1, 1);
            this.UIPanelaside.Name = "UIPanelaside";
            this.UIPanelaside.Radius = 50;
            this.UIPanelaside.RectColor = System.Drawing.Color.Silver;
            this.UIPanelaside.Size = new System.Drawing.Size(196, 415);
            this.UIPanelaside.Style = Sunny.UI.UIStyle.Custom;
            this.UIPanelaside.TabIndex = 11;
            this.UIPanelaside.Text = "UIPanelaside";
            // 
            // UIAsideMainFrame
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UIPanelaside);
            this.Name = "UIAsideMainFrame";
            this.Text = "UIAsideMainFrame";
            this.Controls.SetChildIndex(this.UIPanelaside, 0);
            this.UIPanelaside.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected UINavMenu Aside;
        protected UIPanel UIPanelaside;

    }
}