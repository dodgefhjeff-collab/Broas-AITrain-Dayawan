using Sunny.UI;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterStation.Client.Model.Commom;
using WaterStation.Client.Model.LayoutDto;
using WaterStation.Client.Controller;

namespace WaterStation.Client.LayoutManageMoudle
{
    public partial class FormAisleGroupRelationAdd : UIEditForm
    {
        public FormAisleGroupRelationAdd()
        {
            InitializeComponent();
            _ = GetData();
            ButtonOkClick += FormLocationGroupRelationInfo_ButtonOkClick;
        }
        private Snowflake snowflake = new ();

        private SystemParaSetInfoDto _systemParaSetInfoDto;

        public SystemParaSetInfoDto systemParaSetInfoDto
        {
            get
            {
                _systemParaSetInfoDto.EquipmentName = txtEquipmentName.Text.Trim();
                _systemParaSetInfoDto.ComName = txtName.Text.Trim();
                _systemParaSetInfoDto.BTL = Convert.ToString(comBaudRate.SelectedValue);
                _systemParaSetInfoDto.Filed1 = Convert.ToString(comDataBit.SelectedValue);
                _systemParaSetInfoDto.StopTZW = Convert.ToString(cmStopBit.SelectedValue);
                _systemParaSetInfoDto.checkJYW = Convert.ToString(cmCheckBit.SelectedValue);
                return _systemParaSetInfoDto;
            }
            set
            {

            }
        }
        public FormAisleGroupRelationAdd(SystemParaSetInfoDto _dto)
        {
            InitializeComponent();
            if (_systemParaSetInfoDto == null)
            {
                _ = GetData();
                _systemParaSetInfoDto = new SystemParaSetInfoDto();
                _systemParaSetInfoDto.ID = _dto.ID;
                txtEquipmentName.Text = _dto.EquipmentName;
                txtName.Text = _dto.ComName;
                comBaudRate.SelectedItem = ComboxSource.GetBaudRate.Find(it => it.ValueMember == Convert.ToInt32(_dto.BTL));
                comDataBit.SelectedItem = ComboxSource.GetDataBit.Find(it => it.ValueMember == Convert.ToInt32(_dto.Filed1));
                cmStopBit.SelectedItem = ComboxSource.GetStopBit.Find(it => it.ValueMember == Convert.ToInt32(_dto.StopTZW));
                cmCheckBit.SelectedItem = ComboxSource.GetCheckBit.Find(it => it.ValueMember == Convert.ToInt32(_dto.checkJYW));
            }
            ButtonOkClick += Update_ButtonOkClick;
        }
        private async void FormLocationGroupRelationInfo_ButtonOkClick(object sender, EventArgs e)
        {
            try
            {
                SystemParaSetInfo systemPara = new SystemParaSetInfo();

                if (string.IsNullOrEmpty(txtName.Text))
                {
                    ShowTip.ShowErrorTip("请填写串口名称");
                    return;
                }
                if (comBaudRate.SelectedValue == null)
                {
                    ShowTip.ShowErrorTip("选择波特率");
                    return;
                }
                if (comDataBit.SelectedValue == null)
                {
                    ShowTip.ShowErrorTip("选择数据位");
                    return;
                }
                if (cmStopBit.SelectedValue == null)
                {
                    ShowTip.ShowErrorTip("选择停止位");
                    return;
                }
                systemPara.ID = snowflake.GetId();
                systemPara.EquipmentName = this.txtEquipmentName.Text.Trim();
                systemPara.ComName = txtName.Text.Trim();
                systemPara.BTL = Convert.ToString(comBaudRate.SelectedValue);
                systemPara.Filed1 = Convert.ToString(comDataBit.SelectedValue);//数据位
                systemPara.StopTZW = Convert.ToString(cmStopBit.SelectedValue);
                systemPara.checkJYW = Convert.ToString(cmCheckBit.SelectedValue);//校验位
                var headerResult = await DataAPI.AddSystemParaAsync(systemPara);
                if (headerResult)
                    this.DialogResult = DialogResult.OK;
                else
                    ShowErrorTip("新增失败");
            }
            catch (Exception ex)
            {
                ShowErrorTip(ex.ToString());
            }
        }

        private async Task GetData()
        {
            try
            {

                comBaudRate.DataSource = ComboxSource.GetBaudRate;
                comBaudRate.DisplayMember = "DisplayMember";
                comBaudRate.ValueMember = "ValueMember";
                comBaudRate.SelectedIndex = 0;

                comDataBit.DataSource = ComboxSource.GetDataBit;
                comDataBit.DisplayMember = "DisplayMember";
                comDataBit.ValueMember = "ValueMember";
                comDataBit.SelectedIndex = 0;

                cmStopBit.DataSource = ComboxSource.GetStopBit;
                cmStopBit.DisplayMember = "DisplayMember";
                cmStopBit.ValueMember = "ValueMember";
                cmStopBit.SelectedIndex = 0;

                cmCheckBit.DataSource = ComboxSource.GetCheckBit;
                cmCheckBit.DisplayMember = "DisplayMember";
                cmCheckBit.ValueMember = "ValueMember";
                cmCheckBit.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowErrorTip(ex.ToString());
            }
        }
        private async void Update_ButtonOkClick(object sender, EventArgs e)
        {
            try
            {
                SystemParaSetInfo systemPara = new() { ID = systemParaSetInfoDto.ID, EquipmentName = systemParaSetInfoDto.EquipmentName, ComName = systemParaSetInfoDto.ComName, BTL = systemParaSetInfoDto.BTL, checkJYW = systemParaSetInfoDto.checkJYW, Filed1 = systemParaSetInfoDto.Filed1, StopTZW = systemParaSetInfoDto.StopTZW };
                var headerResult = await DataAPI.UpdateSystemParaAsync(systemPara);
                if (headerResult)
                    this.DialogResult = DialogResult.OK;
                else
                    ShowTip.ShowErrorTip("更新失败");
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
        }
    }
}
