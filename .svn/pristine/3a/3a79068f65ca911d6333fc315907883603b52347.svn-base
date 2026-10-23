using Sunny.UI;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.LayoutDto;
namespace WaterStation.Client.LayoutManageMoudle
{

    public partial class QCData : Form
    {
        public QCData()
        {
            InitializeComponent();
            //uidateStartTime.Value =Convert.ToDateTime( DateTime.Now.AddYears(-1).ToString("G"));
            //dateEndTime.Value = Convert.ToDateTime(DateTime.Now.AddYears(0).ToString("G"));
            //comallotsate.DataSource = ComboxSource.AllotSateSources;
            //comallotsate.DisplayMember = "DisplayMember";
            //comallotsate.ValueMember = "ValueMember";
            //comallotsate.SelectedIndex = 0;
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
            uidateStartTime.Text = null;
            dateEndTime.Text = null;
            uidateStartTimeBY.Text = null;
            dateEndTimeBY.Text= null;
            dvLDdata.AutoGenerateColumns = false;
            uiPagination1.TotalCount = 50;
            _=GetAllQCRecord();
            dvBYdata.AutoGenerateColumns = false;
            uiPagination2.TotalCount = 50;
            _=GetAllQCRecordBY();
        }

        private bool Init = true;

        private QueryQCRecordDto _queryQCRecordDto;
        public QueryQCRecordDto queryQCRecordDto
        {
            get
            {
                _queryQCRecordDto = new QueryQCRecordDto();
                if(!string.IsNullOrEmpty(txtQCName.Text))                
                    _queryQCRecordDto.QCName = txtQCName.Text.Trim();
                if (!string.IsNullOrEmpty(uidateStartTime.Text))
                    _queryQCRecordDto.SatrtTime = uidateStartTime.Value;
                if (!string.IsNullOrEmpty(dateEndTime.Text))
                    _queryQCRecordDto.EndTime = dateEndTime.Value;
                _queryQCRecordDto.Remark = "2";
                _queryQCRecordDto.PageIndex = 1;
                _queryQCRecordDto.PageSize = uiPagination2.PageSize;
                return _queryQCRecordDto;
            }
            set
            {
            }
        }

        private QueryQCRecordDto _queryQCRecordDtoBY;
        public QueryQCRecordDto queryQCRecordDtoBY
        {
            get
            {
                _queryQCRecordDtoBY = new QueryQCRecordDto();
                if (!string.IsNullOrEmpty(txtQCNameBY.Text))
                    _queryQCRecordDtoBY.QCName = txtQCNameBY.Text.Trim();
                if (!string.IsNullOrEmpty(uidateStartTimeBY.Text))
                    _queryQCRecordDtoBY.SatrtTime = uidateStartTimeBY.Value;
                if (!string.IsNullOrEmpty(dateEndTimeBY.Text))
                    _queryQCRecordDtoBY.EndTime = dateEndTimeBY.Value;
                _queryQCRecordDtoBY.Remark = "1";
                _queryQCRecordDtoBY.PageIndex = 1;
                _queryQCRecordDtoBY.PageSize = uiPagination1.PageSize;
                return _queryQCRecordDtoBY;
            }
            set
            {
            }
        }
        #region 添加全选功能
        private DataGridView dataGridView;
        private void DataGridViewRead(bool read)
        {
            if (read)
            {
                for (int i = 1; i < dataGridView.Columns.Count; i++)
                {
                    dataGridView.Columns[i].ReadOnly = true;
                }
            }
            else
            {
                for (int i = 1; i < dataGridView.Columns.Count; i++)
                {
                    dataGridView.Columns[i].ReadOnly = false;
                }
            }
        }
        private void InitializeDataGridView(DataGridView dataGrid)
        {
            dataGridView = dataGrid;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.AllowUserToAddRows = false;
            int index = 0;
            DataGridViewCheckBoxColumn colCB = new DataGridViewCheckBoxColumn();
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

            protected override void Paint(System.Drawing.Graphics graphics,
                System.Drawing.Rectangle clipBounds,
                System.Drawing.Rectangle cellBounds,
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
        /// 查询所有零点信息
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetAllQCRecord()
        {
            //Init = true;
            //UIWaitFormService.ShowWaitForm();
            try
            {
                var headerResult = await DataAPI.GetAllQCRecordAsync(queryQCRecordDto);
                if (headerResult.IsSucceed)
                {
                   // DataGridViewRead(false);
                    dvLDdata.DataSource = headerResult.Result;
                    uiPagination1.TotalCount = headerResult.Total;
                    //DataGridViewRead(true);
                    return true;
                }
                else
                {
                   ShowTip.ShowErrorTip("暂无数据");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                Init = false;
                UIWaitFormService.HideWaitForm();
            }
        }

        private async Task<bool> GetAllQCRecordBY()
        {
            //Init = true;
            //UIWaitFormService.ShowWaitForm();
            try
            {
                var headerResult = await DataAPI.GetAllQCRecordAsync(queryQCRecordDtoBY);
                if (headerResult.IsSucceed)
                {
                    dvBYdata.DataSource = headerResult.Result;
                    uiPagination2.TotalCount = headerResult.Total;
                    return true;
                }
                else
                {
                    ShowTip.ShowErrorTip("暂无数据");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            //UIWaitFormService.ShowWaitForm();
            try
            {
                _queryQCRecordDto.PageIndex = pageIndex;
                _queryQCRecordDto.PageSize = uiPagination1.PageSize;
                var headerResult = await DataAPI.GetAllQCRecordAsync(_queryQCRecordDto);
                if (headerResult.IsSucceed)
                {
                    //DataGridViewRead(false);
                    dvLDdata.DataSource = headerResult.Result;
                    uiPagination1.TotalCount = headerResult.Total;
                    //DataGridViewRead(true);
                    ShowTip.ShowSuccessTip("OK");
                }
                else
                {
                    ShowTip.ShowErrorTip("暂无数据");
                }
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
            finally
            {
                UIWaitFormService.HideWaitForm();
            }
        }

        private async void uiPagination2_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            if (Init) return;
           // UIWaitFormService.ShowWaitForm();
            try
            {
                _queryQCRecordDtoBY.PageIndex = pageIndex;
                _queryQCRecordDtoBY.PageSize = uiPagination2.PageSize;
                var headerResult = await DataAPI.GetAllQCRecordAsync(_queryQCRecordDtoBY);
                if (headerResult.IsSucceed)
                {
                    dvBYdata.DataSource = headerResult.Result;
                    uiPagination2.TotalCount = headerResult.Total;
                    ShowTip.ShowSuccessTip("OK");
                }
                else
                {
                    ShowTip.ShowErrorTip("暂无数据");
                }
            }
            catch 
            {
                return;
            }
            finally
            {
                UIWaitFormService.HideWaitForm();
            }
        }

        private async void Query_Click(object sender, EventArgs e)
        {
            if (await GetAllQCRecord())
                ShowTip.ShowSuccessTip("查询记录成功");
        }

        private async void QueryBY_Click(object sender, EventArgs e)
        {
            if (await GetAllQCRecordBY())
                ShowTip.ShowSuccessTip("查询记录成功");
        }
    }
}
