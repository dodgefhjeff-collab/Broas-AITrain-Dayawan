using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterStation.Client.Model.UserDto;
using WaterStation.Client.Controller;

namespace WaterStation.Client.UserManageMoudle
{
    public partial class FormRoleInfo : Form
    {
        public FormRoleInfo()
        {
            InitializeComponent();
            //var functionInfo = Global.roleFunctionInfoDtos.Find(it => it.FunctionID == Convert.ToInt32(this.Tag) && it.RoleID == Global.userInfoDto.RoleID);
            //if (functionInfo == null)
            //{
            //    MessageBox.Show("您没有此功能权限请联系管理员");
            //    return;
            //}
            ////加载按钮权限
            //int butAuthority = Global.roleFunctionInfoDtos.Find(it => it.FunctionID == Convert.ToInt32(this.Tag) && it.RoleID == Global.userInfoDto.RoleID).Operation;
            //foreach (Control temp in uiGroupBox1.Controls)
            //{
            //    if (temp is UISymbolButton)
            //    {
            //        UISymbolButton uISymbolButton = temp as UISymbolButton;
            //        uISymbolButton.Enabled = Global.GetBitStatue(butAuthority, Convert.ToInt32(uISymbolButton.Tag));
            //    }
            //}
            InitializeDataGridView(dataroleinfo, true);
            _=GetAllRoleInfoAsync();
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
            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
            if (singleChoose)
            {
                colCB.HeaderText = "选择";
                dataGrid.Columns.Insert(index, colCB);
                return;
            }
            DatagridViewCheckBoxHeaderCell cbHeader = new DatagridViewCheckBoxHeaderCell();
            colCB.HeaderCell = cbHeader;
            colCB.HeaderText = "";
            cbHeader.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeader_OnCheckBoxClicked);
            dataGrid.Columns.Insert(index, colCB);
            dataGrid.Columns[index].Width = 50;
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
            Point _cellLocation = new Point();
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

        private RoleInfoDto _queryRoleInfoDto;
        public RoleInfoDto queryRoleInfoDto
        {
            get
            {
                _queryRoleInfoDto = new();
                _queryRoleInfoDto.FullName = txtname.Text.Trim();
                _queryRoleInfoDto.Code = textCode.Text.Trim();
                return _queryRoleInfoDto;
            }
            set
            { }
        }



        private async void Query_Click(object sender, EventArgs e)
        {
            if (await GetAllRoleInfoAsync())
                ShowTip.ShowSuccessTip("查询成功");
        }

        private void Add_Click(object sender, EventArgs e)
        {
            FormRoleEdit formRoleInfoEdit = new FormRoleEdit();
            formRoleInfoEdit.ShowDialog();
            if (formRoleInfoEdit.DialogResult == DialogResult.OK)
            {
                formRoleInfoEdit.Close();
                formRoleInfoEdit.Dispose();
                _ = GetAllRoleInfoAsync();
            }
        }

        //update  
        private void uiSymbolButton5_Click(object sender, EventArgs e)
        {
            try
            {
                List<RoleInfoDto> RoleInfoDtos = new List<RoleInfoDto>();
                int rowcount = dataGridView.Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    if (Convert.ToBoolean(dataGridView.Rows[i].Cells[0].Value))
                    {
                        RoleInfoDto RoleInfoDto = dataGridView.Rows[i].DataBoundItem as RoleInfoDto;
                        RoleInfoDtos.Add(RoleInfoDto);
                    }
                }
                if (RoleInfoDtos.Count != 1)
                {
                    ShowTip.ShowErrorTip("选择要修改的配置数据");
                    return;
                }
                FormRoleEdit formRoleInfoEdit = new FormRoleEdit(RoleInfoDtos[0]);
                formRoleInfoEdit.ShowDialog();
                if (formRoleInfoEdit.DialogResult == DialogResult.OK)
                {
                    formRoleInfoEdit.Close();
                    formRoleInfoEdit.Dispose();
                    _ = GetAllRoleInfoAsync();
                }
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
        }

        private async void txtcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (await GetAllRoleInfoAsync())
                    ShowTip.ShowSuccessTip("查询成功");
        }

        public async Task<bool> GetAllRoleInfoAsync()
        {
            try
            {
                var headerResult = await DataAPI.GetAllRoleAsync(queryRoleInfoDto);
                if (headerResult.Count > 0)
                {
                    DataGridViewRead(false);
                    dataroleinfo.DataSource = headerResult;
                    DataGridViewRead(true);
                }
                else
                    ShowTip.ShowErrorTip("暂无数据");
                return true;
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
                return false;
            }
        }

        private async void txtname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (await GetAllRoleInfoAsync())
                    ShowTip.ShowSuccessTip("查询成功");
        }

        private async void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            List<RoleInfoDto> resultDtos = new List<RoleInfoDto>();
            int rowcount = dataGridView.Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                if (Convert.ToBoolean(dataGridView.Rows[i].Cells[0].Value))
                {
                    RoleInfoDto resultDto = dataGridView.Rows[i].DataBoundItem as RoleInfoDto;
                    resultDtos.Add(resultDto);
                }
            }
            if (resultDtos.Count != 1)
            {
                ShowTip.ShowErrorTip("选择要删除的配置数据");
                return;
            }
            var headerResult = await DataAPI.DeleteRoleAsync(resultDtos.First().ID);
            if (headerResult)
            {
                ShowTip.ShowSuccessTip("删除成功");
                _ = GetAllRoleInfoAsync();
            }
            else
                ShowTip.ShowErrorTip("删除失败");
        }

        private void FormRoleInfo_Load(object sender, EventArgs e)
        {
            this.dataroleinfo.CellContentClick += new DataGridViewCellEventHandler(meauOptionBtn_Click);
        }

        private void meauOptionBtn_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataroleinfo.Columns[e.ColumnIndex].Name == "MeauOptionBtn")
            {
                var row = this.dataroleinfo.Rows[e.RowIndex].DataBoundItem as RoleInfoDto;
                FormReFunctionEdit subForm = new FormReFunctionEdit(row);
                subForm.ShowDialog();
            }
        }
    }
}
