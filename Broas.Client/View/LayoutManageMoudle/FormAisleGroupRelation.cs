using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.LayoutDto;

namespace WaterStation.Client.LayoutManageMoudle
{
    /// <summary>
    /// 系统参数设置
    /// </summary>
    public partial class FormAisleGroupRelation : Form
    {
        public FormAisleGroupRelation()
        {
            InitializeComponent();
            //加载按钮权限
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
            _ = GetAllSystemParaAsync();
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
        /// <summary>
        /// 参数设置
        ///  20220901
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Query_Click(object sender, EventArgs e)
        {
            if (await GetAllSystemParaAsync())
                ShowTip.ShowSuccessTip("查询成功");
        }

        private void uiSymbolButton5_Click(object sender, EventArgs e)
        {
            List<SystemParaSetInfoDto> aisleGroupRelations = new List<SystemParaSetInfoDto>();
            int rowcount = dataGridView.Rows.Count;
            for (int i = 0; i < rowcount; i++)
            {
                if (Convert.ToBoolean(dataGridView.Rows[i].Cells[0].Value))
                {
                    SystemParaSetInfoDto aisleGroupRelation = dataGridView.Rows[i].DataBoundItem as SystemParaSetInfoDto;
                    aisleGroupRelations.Add(aisleGroupRelation);
                }
            }
            if (aisleGroupRelations.Count == 0)
            {
                ShowTip.ShowErrorTip("请选择要查询的巷道分组关联配置");
                return;
            }

            if (aisleGroupRelations.Count != 1)
            {
                ShowTip.ShowErrorTip("您已选择多条记录，请检查");
                return;
            }

        }
        private QuerySystemParaSetInfoDto _queryDto;
        public QuerySystemParaSetInfoDto queryDto
        {
            get
            {
                _queryDto = new ();
                if (!string.IsNullOrEmpty(txtName.Text))
                    _queryDto.EquipmentName = txtName.Text.Trim();
                return _queryDto;
            }
            set
            {
            }
        }
        /// <summary>
        /// 系统参数设置 查询
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetAllSystemParaAsync()
        {
            try
            {
                var headerResult = await DataAPI.GetSystemParaAsync(queryDto);
                if (headerResult.IsSucceed)
                {
                    DataGridViewRead(false);
                    if (!string.IsNullOrEmpty(txtName.Text.Trim()))
                        headerResult.Result = headerResult.Result.Where(it => it.EquipmentName.Contains(txtName.Text.Trim())).ToList();//设备名称
                    datauserinfo.DataSource = headerResult.Result;
                    DataGridViewRead(true);
                    return true;
                }
                else
                {
                    ShowTip.ShowErrorTip(headerResult.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
                return false;
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            FormAisleGroupRelationAdd formAisleGroupRelationAdd = new FormAisleGroupRelationAdd();
            formAisleGroupRelationAdd.ShowDialog();
            if (formAisleGroupRelationAdd.DialogResult == DialogResult.OK)
                _ = GetAllSystemParaAsync();
        }

        private async void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                List<SystemParaSetInfoDto> aisleGroupRelations = new List<SystemParaSetInfoDto>();
                int rowcount = dataGridView.Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    if (Convert.ToBoolean(dataGridView.Rows[i].Cells[0].Value))
                    {
                        SystemParaSetInfoDto aisleGroupRelation = dataGridView.Rows[i].DataBoundItem as SystemParaSetInfoDto;
                        aisleGroupRelations.Add(aisleGroupRelation);
                    }
                }
                if (aisleGroupRelations.Count == 0)
                {
                    ShowTip.ShowErrorTip("选择要删除的参数配置");
                    return;
                }
                if (aisleGroupRelations.Count != 1)
                {
                    ShowTip.ShowErrorTip("您已选择多条记录，只允许删除一条，请检查");
                    return;
                }
                if (UIMessageBox.ShowAsk("确认删除？"))
                {
                    //执行代码
                    var headerResult = await DataAPI.DeleteSystemParaAsync(aisleGroupRelations[0].ID);
                    if (headerResult)
                        _ = GetAllSystemParaAsync();
                    else
                        ShowTip.ShowErrorTip("删除失败");
                }
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
        }

        private async void txtName_keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (await GetAllSystemParaAsync())
                    ShowTip.ShowSuccessTip("查询成功");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<SystemParaSetInfoDto> aisleGroupRelations = new List<SystemParaSetInfoDto>();
            int rowcount = dataGridView.Rows.Count;
            for (int i = 0; i < rowcount; i++)
                if (Convert.ToBoolean(dataGridView.Rows[i].Cells[0].Value))
                {
                    SystemParaSetInfoDto aisleGroupRelation = dataGridView.Rows[i].DataBoundItem as SystemParaSetInfoDto;
                    aisleGroupRelations.Add(aisleGroupRelation);
                }
            if (aisleGroupRelations.Count == 0)
            {
                ShowTip.ShowErrorTip("请选择要修改的串口配置");
                return;
            }

            if (aisleGroupRelations.Count != 1)
            {
                ShowTip.ShowErrorTip("您已选择多条记录，请检查");
                return;
            }

            FormAisleGroupRelationAdd formLocationGroupRelation = new (aisleGroupRelations[0]);
            formLocationGroupRelation.ShowDialog();
        }
    }
}
