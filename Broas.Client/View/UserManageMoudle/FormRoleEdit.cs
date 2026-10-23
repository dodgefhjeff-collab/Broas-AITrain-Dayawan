using Sunny.UI;
using System;
using WaterStation.Client.Model.UserDto;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.Commom;

namespace WaterStation.Client.UserManageMoudle
{
    public partial class FormRoleEdit : UIEditForm
    {
        private void FormRoleEdit_Load(object sender, EventArgs e)
        {

        }

        public FormRoleEdit()
        {
            InitializeComponent();
            ButtonOkClick += FormRoleInfoEdit_ButtonOkClick;
            GetRoleInfoDtoAsync();
        }
        private Snowflake snowflake = new Snowflake();

        private async void FormRoleInfoEdit_ButtonOkClick(object sender, EventArgs e)
        {
            try
            {
                RoleInfo roleInfo = new()
                {
                    ID = snowflake.GetId(),
                    FullName = RoleInfoDto.FullName,
                    Code = RoleInfoDto.Code,
                    Description = RoleInfoDto.Description,
                    UpdateTime = DateTime.Now
                };
                var headerResult = await DataAPI.AddRoleAsync(roleInfo);
                if (headerResult)
                    ShowTip.ShowErrorTip("新增成功");
                else
                    ShowTip.ShowErrorTip("新增失败");
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
        }
        /// <summary>
        ///  获取角色信息
        /// </summary>
        /// <param name="_RoleInfoDto"></param>
        public FormRoleEdit(RoleInfoDto _RoleInfoDto)
        {
            InitializeComponent();
            RoleInfoDto = _RoleInfoDto;
            ButtonOkClick += FormRoleInfoEdit_ButtonOkClick1;
            GetRoleInfoDtoAsync(_RoleInfoDto.ID);
        }

        private async void FormRoleInfoEdit_ButtonOkClick1(object sender, EventArgs e)
        {
            try
            {
                RoleInfo roleInfo = new()
                {
                    ID = RoleInfoDto.ID,
                    FullName = RoleInfoDto.FullName,
                    Code = RoleInfoDto.Code,
                    Description = RoleInfoDto.Description
                };
                var headerResult = await DataAPI.UpdateRoleAsync(roleInfo);
                if (headerResult)
                    ShowTip.ShowSuccessTip("修改成功");
                else
                    ShowTip.ShowErrorTip("修改失败");
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
        }

        private RoleInfoDto _RoleInfoDto;
        /// <summary>
        /// 修改权限或者新增权限
        /// </summary>
        public RoleInfoDto RoleInfoDto
        {
            get
            {
                _RoleInfoDto ??= new RoleInfoDto();
                _RoleInfoDto.FullName = etextName.Text;
                _RoleInfoDto.Code = etextCode.Text;
                _RoleInfoDto.Description = etextDescriptiion.Text;
                return _RoleInfoDto;
            }
            set
            {
                _RoleInfoDto = value;
                etextName.Text = _RoleInfoDto.FullName;
                etextCode.Text = _RoleInfoDto.Code;
                etextDescriptiion.Text = _RoleInfoDto.Description;
            }
        }
        /// <summary>
        /// 非空验证
        /// </summary>
        /// <returns></returns>
        protected override bool CheckData()
        {
            return CheckEmpty(etextCode, "请输入角色编号")
                && CheckEmpty(etextName, "请输入角色名称");
        }

        private async void GetRoleInfoDtoAsync(long id = 0)
        {
            if (id > 0)
            {
                var data = await DataAPI.GetRoleInfoByIDAsync(id);
                _RoleInfoDto = data;
            }
        }
    }
}
