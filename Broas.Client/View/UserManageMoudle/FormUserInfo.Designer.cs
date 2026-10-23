
namespace WaterStation.Client.UserManageMoudle
{
    partial class FormUserInfo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserInfo));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.headeruiPanel = new Sunny.UI.UIPanel();
            this.datauserinfo = new Sunny.UI.UIDataGridView();
            this.uiGroupBox1 = new Sunny.UI.UICheckBoxGroup();
            this.uiSymbolButton5 = new Sunny.UI.UISymbolButton();
            this.InitePassword = new Sunny.UI.UISymbolButton();
            this.txtjobnumber = new Sunny.UI.UITextBox();
            this.txtdeparment = new Sunny.UI.UITextBox();
            this.txtfullname = new Sunny.UI.UITextBox();
            this.txtusername = new Sunny.UI.UITextBox();
            this.uiSymbolLabel4 = new Sunny.UI.UISymbolLabel();
            this.uiSymbolLabel3 = new Sunny.UI.UISymbolLabel();
            this.uiSymbolLabel2 = new Sunny.UI.UISymbolLabel();
            this.uiSymbolLabel1 = new Sunny.UI.UISymbolLabel();
            this.uiLine1 = new Sunny.UI.UILine();
            this.Add = new Sunny.UI.UISymbolButton();
            this.Query = new Sunny.UI.UISymbolButton();
            this.Delete = new Sunny.UI.UISymbolButton();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.headeruiPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datauserinfo)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            this.uiPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // headeruiPanel
            // 
            this.headeruiPanel.BackColor = System.Drawing.Color.Transparent;
            this.headeruiPanel.Controls.Add(this.datauserinfo);
            this.headeruiPanel.Controls.Add(this.uiGroupBox1);
            this.headeruiPanel.FillColor = System.Drawing.Color.White;
            this.headeruiPanel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.headeruiPanel.Location = new System.Drawing.Point(17, 19);
            this.headeruiPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.headeruiPanel.MinimumSize = new System.Drawing.Size(1, 1);
            this.headeruiPanel.Name = "headeruiPanel";
            this.headeruiPanel.RectColor = System.Drawing.Color.Transparent;
            this.headeruiPanel.Size = new System.Drawing.Size(946, 755);
            this.headeruiPanel.Style = Sunny.UI.UIStyle.Custom;
            this.headeruiPanel.StyleCustomMode = true;
            this.headeruiPanel.TabIndex = 2;
            this.headeruiPanel.Text = null;
            // 
            // datauserinfo
            // 
            this.datauserinfo.AllowUserToAddRows = false;
            this.datauserinfo.AllowUserToDeleteRows = false;
            this.datauserinfo.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.datauserinfo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.datauserinfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.datauserinfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.datauserinfo.BackgroundColor = System.Drawing.Color.White;
            this.datauserinfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.datauserinfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.datauserinfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datauserinfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.datauserinfo.ColumnHeadersHeight = 32;
            this.datauserinfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.datauserinfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column8});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.datauserinfo.DefaultCellStyle = dataGridViewCellStyle6;
            this.datauserinfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datauserinfo.EnableHeadersVisualStyles = false;
            this.datauserinfo.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.datauserinfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.datauserinfo.Location = new System.Drawing.Point(0, 143);
            this.datauserinfo.Name = "datauserinfo";
            this.datauserinfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datauserinfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.datauserinfo.RowHeadersWidth = 51;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.datauserinfo.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.datauserinfo.RowTemplate.Height = 29;
            this.datauserinfo.SelectedIndex = -1;
            this.datauserinfo.ShowRect = false;
            this.datauserinfo.Size = new System.Drawing.Size(946, 612);
            this.datauserinfo.StripeOddColor = System.Drawing.Color.White;
            this.datauserinfo.Style = Sunny.UI.UIStyle.Custom;
            this.datauserinfo.StyleCustomMode = true;
            this.datauserinfo.TabIndex = 3;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.uiSymbolButton5);
            this.uiGroupBox1.Controls.Add(this.InitePassword);
            this.uiGroupBox1.Controls.Add(this.txtjobnumber);
            this.uiGroupBox1.Controls.Add(this.txtdeparment);
            this.uiGroupBox1.Controls.Add(this.txtfullname);
            this.uiGroupBox1.Controls.Add(this.txtusername);
            this.uiGroupBox1.Controls.Add(this.uiSymbolLabel4);
            this.uiGroupBox1.Controls.Add(this.uiSymbolLabel3);
            this.uiGroupBox1.Controls.Add(this.uiSymbolLabel2);
            this.uiGroupBox1.Controls.Add(this.uiSymbolLabel1);
            this.uiGroupBox1.Controls.Add(this.uiLine1);
            this.uiGroupBox1.Controls.Add(this.Add);
            this.uiGroupBox1.Controls.Add(this.Query);
            this.uiGroupBox1.Controls.Add(this.Delete);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox1.FillColor = System.Drawing.Color.White;
            this.uiGroupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.RectColor = System.Drawing.Color.Transparent;
            this.uiGroupBox1.SelectedIndexes = ((System.Collections.Generic.List<int>)(resources.GetObject("uiGroupBox1.SelectedIndexes")));
            this.uiGroupBox1.Size = new System.Drawing.Size(946, 143);
            this.uiGroupBox1.Style = Sunny.UI.UIStyle.Custom;
            this.uiGroupBox1.StyleCustomMode = true;
            this.uiGroupBox1.TabIndex = 3;
            this.uiGroupBox1.Text = "用户管理";
            // 
            // uiSymbolButton5
            // 
            this.uiSymbolButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiSymbolButton5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            this.uiSymbolButton5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiSymbolButton5.Location = new System.Drawing.Point(266, 36);
            this.uiSymbolButton5.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolButton5.Name = "uiSymbolButton5";
            this.uiSymbolButton5.Radius = 15;
            this.uiSymbolButton5.Size = new System.Drawing.Size(95, 28);
            this.uiSymbolButton5.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolButton5.StyleCustomMode = true;
            this.uiSymbolButton5.Symbol = 108;
            this.uiSymbolButton5.SymbolSize = 19;
            this.uiSymbolButton5.TabIndex = 15;
            this.uiSymbolButton5.Tag = "2";
            this.uiSymbolButton5.Text = "修改";
            this.uiSymbolButton5.Click += new System.EventHandler(this.uiSymbolButton5_Click);
            // 
            // InitePassword
            // 
            this.InitePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.InitePassword.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            this.InitePassword.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.InitePassword.Location = new System.Drawing.Point(504, 36);
            this.InitePassword.MinimumSize = new System.Drawing.Size(1, 1);
            this.InitePassword.Name = "InitePassword";
            this.InitePassword.Radius = 15;
            this.InitePassword.Size = new System.Drawing.Size(95, 28);
            this.InitePassword.Style = Sunny.UI.UIStyle.Custom;
            this.InitePassword.StyleCustomMode = true;
            this.InitePassword.Symbol = 57345;
            this.InitePassword.SymbolSize = 19;
            this.InitePassword.TabIndex = 14;
            this.InitePassword.Tag = "5";
            this.InitePassword.Text = "重置密码";
            this.InitePassword.Click += new System.EventHandler(this.InitePassword_Click);
            // 
            // txtjobnumber
            // 
            this.txtjobnumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtjobnumber.FillColor = System.Drawing.Color.White;
            this.txtjobnumber.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtjobnumber.Location = new System.Drawing.Point(744, 101);
            this.txtjobnumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtjobnumber.Maximum = 2147483647D;
            this.txtjobnumber.Minimum = -2147483648D;
            this.txtjobnumber.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtjobnumber.Name = "txtjobnumber";
            this.txtjobnumber.Padding = new System.Windows.Forms.Padding(5);
            this.txtjobnumber.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            this.txtjobnumber.Size = new System.Drawing.Size(112, 23);
            this.txtjobnumber.Style = Sunny.UI.UIStyle.Custom;
            this.txtjobnumber.StyleCustomMode = true;
            this.txtjobnumber.TabIndex = 12;
            this.txtjobnumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtjobnumber_KeyDown);
            // 
            // txtdeparment
            // 
            this.txtdeparment.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtdeparment.FillColor = System.Drawing.Color.White;
            this.txtdeparment.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtdeparment.Location = new System.Drawing.Point(522, 100);
            this.txtdeparment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtdeparment.Maximum = 2147483647D;
            this.txtdeparment.Minimum = -2147483648D;
            this.txtdeparment.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtdeparment.Name = "txtdeparment";
            this.txtdeparment.Padding = new System.Windows.Forms.Padding(5);
            this.txtdeparment.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            this.txtdeparment.Size = new System.Drawing.Size(112, 23);
            this.txtdeparment.Style = Sunny.UI.UIStyle.Custom;
            this.txtdeparment.StyleCustomMode = true;
            this.txtdeparment.TabIndex = 12;
            this.txtdeparment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtdeparment_KeyDown);
            // 
            // txtfullname
            // 
            this.txtfullname.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtfullname.FillColor = System.Drawing.Color.White;
            this.txtfullname.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtfullname.Location = new System.Drawing.Point(304, 100);
            this.txtfullname.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtfullname.Maximum = 2147483647D;
            this.txtfullname.Minimum = -2147483648D;
            this.txtfullname.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtfullname.Name = "txtfullname";
            this.txtfullname.Padding = new System.Windows.Forms.Padding(5);
            this.txtfullname.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            this.txtfullname.Size = new System.Drawing.Size(112, 23);
            this.txtfullname.Style = Sunny.UI.UIStyle.Custom;
            this.txtfullname.StyleCustomMode = true;
            this.txtfullname.TabIndex = 11;
            this.txtfullname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtfullname_KeyDown);
            // 
            // txtusername
            // 
            this.txtusername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtusername.FillColor = System.Drawing.Color.White;
            this.txtusername.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtusername.Location = new System.Drawing.Point(105, 100);
            this.txtusername.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtusername.Maximum = 2147483647D;
            this.txtusername.Minimum = -2147483648D;
            this.txtusername.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtusername.Name = "txtusername";
            this.txtusername.Padding = new System.Windows.Forms.Padding(5);
            this.txtusername.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            this.txtusername.Size = new System.Drawing.Size(112, 23);
            this.txtusername.Style = Sunny.UI.UIStyle.Custom;
            this.txtusername.StyleCustomMode = true;
            this.txtusername.TabIndex = 10;
            this.txtusername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtusername_KeyDown);
            // 
            // uiSymbolLabel4
            // 
            this.uiSymbolLabel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiSymbolLabel4.Location = new System.Drawing.Point(666, 99);
            this.uiSymbolLabel4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel4.Name = "uiSymbolLabel4";
            this.uiSymbolLabel4.Padding = new System.Windows.Forms.Padding(23, 0, 0, 0);
            this.uiSymbolLabel4.Size = new System.Drawing.Size(83, 28);
            this.uiSymbolLabel4.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolLabel4.Symbol = 61442;
            this.uiSymbolLabel4.SymbolSize = 19;
            this.uiSymbolLabel4.TabIndex = 9;
            this.uiSymbolLabel4.Text = "工号";
            this.uiSymbolLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiSymbolLabel3
            // 
            this.uiSymbolLabel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiSymbolLabel3.Location = new System.Drawing.Point(447, 100);
            this.uiSymbolLabel3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel3.Name = "uiSymbolLabel3";
            this.uiSymbolLabel3.Padding = new System.Windows.Forms.Padding(23, 0, 0, 0);
            this.uiSymbolLabel3.Size = new System.Drawing.Size(83, 28);
            this.uiSymbolLabel3.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolLabel3.Symbol = 61442;
            this.uiSymbolLabel3.SymbolSize = 19;
            this.uiSymbolLabel3.TabIndex = 8;
            this.uiSymbolLabel3.Text = "部门";
            this.uiSymbolLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiSymbolLabel2
            // 
            this.uiSymbolLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiSymbolLabel2.Location = new System.Drawing.Point(233, 102);
            this.uiSymbolLabel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel2.Name = "uiSymbolLabel2";
            this.uiSymbolLabel2.Padding = new System.Windows.Forms.Padding(23, 0, 0, 0);
            this.uiSymbolLabel2.Size = new System.Drawing.Size(83, 28);
            this.uiSymbolLabel2.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolLabel2.Symbol = 61442;
            this.uiSymbolLabel2.SymbolSize = 19;
            this.uiSymbolLabel2.TabIndex = 7;
            this.uiSymbolLabel2.Text = "姓名";
            this.uiSymbolLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiSymbolLabel1
            // 
            this.uiSymbolLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiSymbolLabel1.Location = new System.Drawing.Point(15, 100);
            this.uiSymbolLabel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiSymbolLabel1.Name = "uiSymbolLabel1";
            this.uiSymbolLabel1.Padding = new System.Windows.Forms.Padding(23, 0, 0, 0);
            this.uiSymbolLabel1.Size = new System.Drawing.Size(83, 28);
            this.uiSymbolLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiSymbolLabel1.Symbol = 61442;
            this.uiSymbolLabel1.SymbolSize = 19;
            this.uiSymbolLabel1.TabIndex = 6;
            this.uiSymbolLabel1.Text = "用户名";
            this.uiSymbolLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLine1
            // 
            this.uiLine1.FillColor = System.Drawing.Color.White;
            this.uiLine1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiLine1.LineColor = System.Drawing.Color.Silver;
            this.uiLine1.Location = new System.Drawing.Point(15, 73);
            this.uiLine1.MinimumSize = new System.Drawing.Size(2, 2);
            this.uiLine1.Name = "uiLine1";
            this.uiLine1.Size = new System.Drawing.Size(1090, 17);
            this.uiLine1.Style = Sunny.UI.UIStyle.Custom;
            this.uiLine1.StyleCustomMode = true;
            this.uiLine1.TabIndex = 5;
            this.uiLine1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Add
            // 
            this.Add.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Add.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            this.Add.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Add.Location = new System.Drawing.Point(142, 36);
            this.Add.MinimumSize = new System.Drawing.Size(1, 1);
            this.Add.Name = "Add";
            this.Add.Radius = 15;
            this.Add.Size = new System.Drawing.Size(95, 28);
            this.Add.Style = Sunny.UI.UIStyle.Custom;
            this.Add.StyleCustomMode = true;
            this.Add.Symbol = 61543;
            this.Add.SymbolSize = 19;
            this.Add.TabIndex = 1;
            this.Add.Tag = "1";
            this.Add.Text = "新增";
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Query
            // 
            this.Query.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Query.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            this.Query.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Query.Location = new System.Drawing.Point(25, 36);
            this.Query.MinimumSize = new System.Drawing.Size(1, 1);
            this.Query.Name = "Query";
            this.Query.Radius = 15;
            this.Query.Size = new System.Drawing.Size(95, 28);
            this.Query.Style = Sunny.UI.UIStyle.Custom;
            this.Query.StyleCustomMode = true;
            this.Query.Symbol = 61529;
            this.Query.SymbolSize = 19;
            this.Query.TabIndex = 4;
            this.Query.Tag = "0";
            this.Query.Text = "查询";
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // Delete
            // 
            this.Delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Delete.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(101)))), ((int)(((byte)(143)))));
            this.Delete.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Delete.Location = new System.Drawing.Point(387, 36);
            this.Delete.MinimumSize = new System.Drawing.Size(1, 1);
            this.Delete.Name = "Delete";
            this.Delete.Radius = 15;
            this.Delete.Size = new System.Drawing.Size(95, 28);
            this.Delete.Style = Sunny.UI.UIStyle.Custom;
            this.Delete.StyleCustomMode = true;
            this.Delete.Symbol = 61544;
            this.Delete.SymbolSize = 19;
            this.Delete.TabIndex = 3;
            this.Delete.Tag = "3";
            this.Delete.Text = "删除";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.headeruiPanel);
            this.uiPanel1.FillColor = System.Drawing.Color.White;
            this.uiPanel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uiPanel1.Location = new System.Drawing.Point(10, 14);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Radius = 50;
            this.uiPanel1.RectColor = System.Drawing.Color.Transparent;
            this.uiPanel1.Size = new System.Drawing.Size(980, 799);
            this.uiPanel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiPanel1.StyleCustomMode = true;
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "uiPanel1";
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "UserName";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "用户名";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 67;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.DataPropertyName = "FullName";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Column3.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column3.HeaderText = "姓名";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.DataPropertyName = "Department";
            this.Column4.HeaderText = "部门";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Width = 120;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column5.DataPropertyName = "JobNumber";
            this.Column5.HeaderText = "工号";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 160;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column6.DataPropertyName = "RoleFullName";
            this.Column6.HeaderText = "角色";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Width = 120;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column8.DataPropertyName = "UpdateTime";
            dataGridViewCellStyle5.Format = "yyyy-MM-dd HH:mm:ss";
            this.Column8.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column8.HeaderText = "更新时间";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.Width = 160;
            // 
            // FormUserInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1174, 802);
            this.Controls.Add(this.uiPanel1);
            this.Name = "FormUserInfo";
            this.Tag = "5";
            this.Text = "用户信息维护";
            this.headeruiPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datauserinfo)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Sunny.UI.UISymbolButton uiSymbolButton2;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UISymbolButton uiSymbolButton4;
        private Sunny.UI.UISymbolButton uiSymbolButton3;
        private Sunny.UI.UISymbolButton uiSymbolButton1;
        private Sunny.UI.UIPanel headeruiPanel;
        private Sunny.UI.UISymbolButton Delete;
        private Sunny.UI.UISymbolButton Query;
        private Sunny.UI.UICheckBoxGroup uiGroupBox1;
        private Sunny.UI.UILine uiLine1;
        private Sunny.UI.UISymbolLabel uiSymbolLabel2;
        private Sunny.UI.UISymbolLabel uiSymbolLabel1;
        private Sunny.UI.UISymbolLabel uiSymbolLabel3;
        private Sunny.UI.UITextBox txtusername;
        private Sunny.UI.UISymbolLabel uiSymbolLabel4;
        private Sunny.UI.UITextBox txtjobnumber;
        private Sunny.UI.UITextBox txtdeparment;
        private Sunny.UI.UITextBox txtfullname;
        private Sunny.UI.UIDataGridView datauserinfo;
        private Sunny.UI.UISymbolButton InitePassword;
        private Sunny.UI.UISymbolButton uiSymbolButton5;
        private Sunny.UI.UISymbolButton Add;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}