using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WaterStation.Client.Controller;
using System.Configuration;
using WaterStation.Client.Model.UserDto;

namespace WaterStation.Client.UserManageMoudle
{
    public partial class FormUserInfo : Form
    {
        public FormUserInfo()
        {
            InitializeComponent();
            //加载按钮权限
            //var functionInfo = Global.roleFunctionInfoDtos.Find(it => it.FunctionID == Convert.ToInt32(this.Tag) && it.RoleID == Global.userInfoDto.RoleID);
            //if (functionInfo==null)
            //{
            //    MessageBox.Show("您没有此功能权限请联系管理员");
            //    return;
            //}
            //int butAuthority = Global.roleFunctionInfoDtos.Find(it => it.FunctionID == Convert.ToInt32(this.Tag) && it.RoleID == Global.userInfoDto.RoleID).Operation;

            //foreach (Control temp in uiGroupBox1.Controls)
            //{
            //    if (temp is UISymbolButton)
            //    {
            //        UISymbolButton uISymbolButton = temp as UISymbolButton;
            //        uISymbolButton.Enabled = Global.GetBitStatue(butAuthority, Convert.ToInt32(uISymbolButton.Tag));
            //    }
            //}
            InitializeDataGridView(datauserinfo, true);
            Getdata();
        }

        private async void Getdata()
        {
            try
            {
                UserInfoDto userInfoDto = new UserInfoDto();
                userInfoDto.UserName = txtusername.Text.Trim();
                userInfoDto.FullName = txtfullname.Text.Trim();
                userInfoDto.Department = txtdeparment.Text.Trim();
                userInfoDto.JobNumber = txtjobnumber.Text.Trim();
                DataGridViewRead(false);
                datauserinfo.DataSource = await DataAPI.GetAllUserInfoAsync(userInfoDto);
                DataGridViewRead(true);
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        #region 添加全选功能
        private DataGridView dataGridView;
        private void DataGridViewRead(bool read)
        {
            if (read)
            {
                for (int i = 1; i < dataGridView.Columns.Count; i++)
                    dataGridView.Columns[i].ReadOnly = true;
            }
            else
            {
                for (int i = 1; i < dataGridView.Columns.Count; i++)
                    dataGridView.Columns[i].ReadOnly = false;
            }
        }

        private void InitializeDataGridView(DataGridView dataGrid, bool singleChoose)
        {
            dataGridView = dataGrid;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.AllowUserToAddRows = false;
            int index = 0;
            DataGridViewCheckBoxColumn colCB = new ();
            if (singleChoose)
            {
                colCB.HeaderText = "选择";
                dataGrid.Columns.Insert(index, colCB);
                return;
            }
            DatagridViewCheckBoxHeaderCell cbHeader = new ();
            colCB.HeaderCell = cbHeader;
            colCB.HeaderText = "";
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);
            dataGrid.Columns.Insert(index, colCB);
            dataGrid.Columns[index].Width = 50;
            //for (int i = 1; i < dataGrid.Columns.Count; i++)
            //{
            //    dataGrid.Columns[i].ReadOnly = true;
            //}

            //index++;
            //dataGrid.Columns[index].HeaderText = "姓名";
            //dataGrid.Columns[index].Width = 90;
        }

        private void cbHeader_OnCheckBoxClicked(bool state)
        {
            //结束编辑状态
            dataGridView.EndEdit();
            dataGridView.Rows.OfType<DataGridViewRow>().ToList().ForEach(t => t.Cells[0].Value = state);
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

        public class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
        {
            Point checkBoxLocation;
            Size checkBoxSize;
            bool _checked = false;
            Point _cellLocation = new ();
            System.Windows.Forms.VisualStyles.CheckBoxState _cbState = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;

            public event CheckBoxClickedHandler OnCheckBoxClicked;

            public DatagridViewCheckBoxHeaderCell()
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


        #endregion

        #region 查询
        private void txtusername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Getdata();
        }

        private void txtfullname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Getdata();
        }

        private void txtdeparment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Getdata();
        }

        private void txtjobnumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Getdata();
        }

        #endregion

        /// <summary>
        /// 获取查询当行，多行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Query_Click(object sender, EventArgs e)
        {
            Getdata();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modifyPassword_Click(object sender, EventArgs e)
        {
            FormUpdatePassword formUpdate = new ();
            formUpdate.ShowDialog();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                List<UserInfoDto> userInfoDtos = new ();
                int rowcount = dataGridView.Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    if (Convert.ToBoolean(dataGridView.Rows[i].Cells[0].Value))
                    {
                        UserInfoDto userInfoDto = dataGridView.Rows[i].DataBoundItem as UserInfoDto;
                        userInfoDtos.Add(userInfoDto);
                    }
                }
                if (userInfoDtos.Count != 1)
                {
                    ShowTip.ShowErrorTip("选择要删除的用户");
                    return;
                }

                var result = await DataAPI.DeleteUserAsync(userInfoDtos[0].ID);
                if (result)
                    ShowTip.ShowSuccessTip("删除成功");
                else
                    ShowTip.ShowErrorTip("删除失败");
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, EventArgs e)
        {
            FormUserInfoEdit formUserInfoEdit = new ();
            formUserInfoEdit.ShowDialog();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void InitePassword_Click(object sender, EventArgs e)
        {
            try
            {
                List<UserInfoDto> userInfoDtos = new ();
                int rowcount = dataGridView.Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    if (Convert.ToBoolean(dataGridView.Rows[i].Cells[0].Value))
                    {
                        UserInfoDto userInfoDto = dataGridView.Rows[i].DataBoundItem as UserInfoDto;
                        userInfoDtos.Add(userInfoDto);
                    }
                }
                if (userInfoDtos.Count <= 0)
                {
                    ShowTip.ShowErrorTip("选择要初始化密码的用户");
                    return;
                }
                List<string> usernames = userInfoDtos.Select(it => it.UserName).ToList();
                var result = await DataAPI.InitPasswordAsync(usernames);
                if (result)
                {
                    if (usernames.Contains("admin"))
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings["default"].Value = $"admin;666666";
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    ShowTip.ShowSuccessTip("重置密码成功");
                }
                else
                    ShowTip.ShowErrorTip("重置密码失败");
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton5_Click(object sender, EventArgs e)
        {
            List<UserInfoDto> userInfoDtos = new ();
            int rowcount = dataGridView.Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                if (Convert.ToBoolean(dataGridView.Rows[i].Cells[0].Value))
                {
                    UserInfoDto userInfoDto = dataGridView.Rows[i].DataBoundItem as UserInfoDto;
                    userInfoDtos.Add(userInfoDto);
                }
            }
            if (userInfoDtos.Count != 1)
            {
                ShowTip.ShowErrorTip("选择要修改的用户");
                return;
            }
            FormUserInfoEdit formUserInfoEdit = new (userInfoDtos[0]);
            formUserInfoEdit.ShowDialog();
        }
    }
}
