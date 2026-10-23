using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.RunLogDto;
using NLog;

namespace WaterStation.Client.ReceiptManageMoudle
{
    public partial class FormReceiptHeader : Form
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public FormReceiptHeader()
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
            uiPagination1.TotalCount = 50;
            datestartTime.Text = null;
            dateEndTime.Text = null;
            _ = GetAllRunlogAsync();
            dgvBJ.AutoGenerateColumns = false;
            uiPagination3.TotalCount = 50;
            datestartTimeOP.Text = null;
            dateEndTimeOP.Text = null;
            _ = GetAllRunlogOPAsync();
            uiPagination2.TotalCount = 50;
            datestartTimeBJ.Text = null;
            dateEndTimeBJ.Text = null;
            _ = GetAllBJRunlogAsync();
        }

        private bool Init = true;

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

        private QueryRunAllLogInfoDto queryRunlog;
        public QueryRunAllLogInfoDto _queryRunlog
        {
            get
            {
                queryRunlog = new QueryRunAllLogInfoDto
                {
                    LogContent = txtReceiptCode.Text.Trim()//日志内容
                };
                if (!string.IsNullOrEmpty(datestartTime.Text))
                    queryRunlog.ReceivingSatrtTime = datestartTime.Value;
                if (!string.IsNullOrEmpty(dateEndTime.Text))
                    queryRunlog.ReceivingEndTime = dateEndTime.Value;
                queryRunlog.LogType = "流程";
                queryRunlog.PageIndex = 1;
                queryRunlog.PageSize = uiPagination1.PageSize;
                return queryRunlog;
            }
            set
            {

            }
        }
        private QueryRunAllLogInfoDto queryRunlogOP;
        private QueryRunAllLogInfoDto _queryRunlogOP
        {
            get
            {
                queryRunlogOP = new QueryRunAllLogInfoDto
                {
                    LogContent = txtReceiptCodeOP.Text.Trim()
                };
                if (!string.IsNullOrEmpty(datestartTimeOP.Text))
                    queryRunlogOP.ReceivingSatrtTime = datestartTimeOP.Value;
                if (!string.IsNullOrEmpty(dateEndTimeOP.Text))
                    queryRunlogOP.ReceivingEndTime = dateEndTimeOP.Value;
                queryRunlogOP.LogType = "操作";
                queryRunlogOP.PageIndex = 1;
                queryRunlogOP.PageSize = uiPagination3.PageSize;
                return queryRunlogOP;
            }
            set
            {

            }
        }
        private QueryRunAllLogInfoDto queryRunlogBJ;
        private QueryRunAllLogInfoDto _queryRunlogBJ
        {
            get
            {
                queryRunlogBJ = new QueryRunAllLogInfoDto
                {
                    LogContent = txtReceiptCodeBJ.Text.Trim()//日志内容
                };
                if (!string.IsNullOrEmpty(datestartTimeBJ.Text))
                    queryRunlogBJ.ReceivingSatrtTime = datestartTimeBJ.Value;
                if (!string.IsNullOrEmpty(dateEndTimeBJ.Text))
                    queryRunlogBJ.ReceivingEndTime = dateEndTimeBJ.Value;
                queryRunlogBJ.LogType = "报警";
                queryRunlogBJ.PageIndex = 1;
                queryRunlogBJ.PageSize = uiPagination1.PageSize;
                return queryRunlogBJ;
            }
            set
            {

            }
        }
        private async void Query_Click(object sender, EventArgs e)
        {
            try
            {
                if (await GetAllRunlogAsync())
                    ShowTip.ShowSuccessTip("查询成功");
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
                _logger.Error(ex, "查询运行日志时发生异常");
            }
        }
        private async void QueryBJ_Click(object sender, EventArgs e)
        {
            try
            {
                if (await GetAllBJRunlogAsync())
                    ShowTip.ShowSuccessTip("查询成功");
            }
            catch (Exception ex)
            {

                ShowTip.ShowErrorTip(ex.ToString());
                _logger.Error(ex, "查询报警运行日志时发生异常");
            }
        }

        private async void QueryOP_Click(object sender, EventArgs e)
        {
            try
            {
                if (await GetAllRunlogOPAsync())
                    ShowTip.ShowSuccessTip("查询成功");
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
                _logger.Error(ex, "查询操作日志时发生异常");
            }
        }

        private async void txtReceiptCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (await GetAllRunlogAsync())
                    ShowTip.ShowSuccessTip("查询成功");
        }

        private async Task<bool> GetAllRunlogAsync()
        {
            Init = true;
            //UIWaitFormService.ShowWaitForm();
            try
            {
                var headerResult = await DataAPI.GetAllRunLogAsync(_queryRunlog);
                if (headerResult.IsSucceed)
                {
                    //DataGridViewRead(false);
                    datauserinfo.DataSource = headerResult.Result;
                    uiPagination1.TotalCount = headerResult.Total;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
                _logger.Error(ex, "获取运行日志时发生异常");
                return false;
            }
            finally
            {
                Init = false;
                UIWaitFormService.HideWaitForm();
            }
        }

        private async Task<bool> GetAllBJRunlogAsync()
        {
            Init = true;
            try
            {
                var headerResult = await DataAPI.GetAllRunLogAsync(_queryRunlogBJ);
                if (headerResult.IsSucceed)
                {
                    dgvBJ.DataSource = headerResult.Result;
                    uiPagination2.TotalCount = headerResult.Total;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
                _logger.Error(ex, "获取报警运行日志时发生异常");
                return false;
            }
            finally
            {
                Init = false;
                UIWaitFormService.HideWaitForm();
            }
        }

        private async Task<bool> GetAllRunlogOPAsync()
        {
            Init = true;
            try
            {
                var headerResult = await DataAPI.GetAllRunLogAsync(_queryRunlogOP);
                if (headerResult.IsSucceed)
                {
                    dgvOP.DataSource = headerResult.Result;
                    uiPagination3.TotalCount = headerResult.Total;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
                _logger.Error(ex, "获取操作日志时发生异常");
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
                _queryRunlog = new QueryRunAllLogInfoDto();
                queryRunlog.PageIndex = pageIndex;
                queryRunlog.PageSize = uiPagination1.PageSize;
                var headerResult = await DataAPI.GetAllRunLogAsync(queryRunlog);
                if (headerResult.IsSucceed)
                {
                    //DataGridViewRead(false);
                    datauserinfo.DataSource = headerResult.Result;
                    uiPagination1.TotalCount = headerResult.Total;
                    // DataGridViewRead(true);
                    // 移除不必要的OK提示，翻页是正常操作，不需要提示
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
        private async void uiPagination2_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            if (Init) return;
            //UIWaitFormService.ShowWaitForm();
            try
            {
                queryRunlogBJ = new QueryRunAllLogInfoDto();
                queryRunlogBJ.PageIndex = pageIndex;
                queryRunlogBJ.PageSize = uiPagination2.PageSize;
                var headerResult = await DataAPI.GetAllRunLogAsync(queryRunlogBJ);
                if (headerResult.IsSucceed)
                {
                    //DataGridViewRead(false);
                    dgvBJ.DataSource = headerResult.Result;
                    uiPagination2.TotalCount = headerResult.Total;
                    // DataGridViewRead(true);
                    // 移除不必要的OK提示，翻页是正常操作，不需要提示
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

        private async void uiPagination3_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            if (Init) return;
            try
            {
                queryRunlogOP = new QueryRunAllLogInfoDto();
                queryRunlogOP.LogContent = txtReceiptCodeOP.Text.Trim();
                if (!string.IsNullOrEmpty(datestartTimeOP.Text))
                    queryRunlogOP.ReceivingSatrtTime = datestartTimeOP.Value;
                if (!string.IsNullOrEmpty(dateEndTimeOP.Text))
                    queryRunlogOP.ReceivingEndTime = dateEndTimeOP.Value;
                queryRunlogOP.LogType = "操作";
                queryRunlogOP.PageIndex = pageIndex;
                queryRunlogOP.PageSize = uiPagination3.PageSize;
                var headerResult = await DataAPI.GetAllRunLogAsync(queryRunlogOP);
                if (headerResult.IsSucceed)
                {
                    dgvOP.DataSource = headerResult.Result;
                    uiPagination3.TotalCount = headerResult.Total;
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
    }
}
