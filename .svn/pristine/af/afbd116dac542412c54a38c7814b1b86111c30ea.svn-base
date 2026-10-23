using Sunny.UI;
using System;
using WaterStation.Client.Model.UserDto;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.Commom;

namespace WaterStation.Client.UserManageMoudle
{
    public partial class FormUserInfoEdit : UIEditForm
    {
        public FormUserInfoEdit()
        {
            InitializeComponent();
            ButtonOkClick += FormUserInfoEdit_ButtonOkClick;
            GetRoleInfoDto();
            txtusername.ReadOnly = false;
        }

        public FormUserInfoEdit(UserInfoDto _userInfoDto)
        {
            InitializeComponent();
            userInfoDto = _userInfoDto;
            ButtonOkClick += FormUserInfoEdit_ButtonOkClick1;
            GetRoleInfoDto();
            txtusername.ReadOnly = true;
        }
        private Snowflake snowflake = new Snowflake();

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FormUserInfoEdit_ButtonOkClick1(object sender, EventArgs e)
        {
            try
            {
                UserInfo user = new()
                {
                    ID = userInfoDto.ID,
                    UserName = userInfoDto.UserName,
                    Password = userInfoDto.Password,
                    FullName = userInfoDto.FullName,
                    Department = userInfoDto.Department,
                    JobNumber = userInfoDto.JobNumber,
                    RoleID = userInfoDto.RoleID,
                    IsDelete = userInfoDto.ISDelete,
                    UpdateTime = userInfoDto.UpdateTime
                };
                var headerResult = await DataAPI.UpdateUserAsync(user);
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

        protected override bool CheckData()
        {
            return CheckEmpty(txtusername, "请输入用户名")
                   && CheckEmpty(txtfullname, "请输入姓名")
                   && CheckEmpty(txtdepartment, "请选择部门")
                   && CheckEmpty(txtjobnumber, "请输入工号")
                   && CheckEmpty(uiComboBox1, "请选择角色");
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FormUserInfoEdit_ButtonOkClick(object sender, EventArgs e)
        {
            try
            {
                UserInfo user = new()
                {
                    ID = snowflake.GetId(),
                    UserName = txtusername.Text.Trim(),
                    Password = GenerateMD5.CreateMD5("666666"),
                    FullName = txtfullname.Text.Trim(),
                    Department = txtdepartment.Text.Trim(),
                    JobNumber = txtjobnumber.Text.Trim(),
                    RoleID = Convert.ToInt64(uiComboBox1.SelectedValue),
                    IsDelete = false,
                    UpdateTime = DateTime.Now
                };
                var headerResult = await DataAPI.AddUserAsync(user);
                if (headerResult)
                    ShowTip.ShowSuccessTip("新增成功");
                else
                    ShowTip.ShowErrorTip("新增失败");
            }
            catch (Exception ex)
            {
                ShowTip.ShowErrorTip(ex.ToString());
            }
        }

        private UserInfoDto _infoDto;
        public UserInfoDto userInfoDto
        {
            get
            {
                if (_infoDto == null)
                    _infoDto = new UserInfoDto();
                _infoDto.UserName = txtusername.Text.Trim();
                _infoDto.Password = "8888";
                _infoDto.FullName = txtfullname.Text.Trim();
                _infoDto.Department = txtdepartment.Text.Trim();
                _infoDto.JobNumber = txtjobnumber.Text.Trim();
                _infoDto.RoleID = Convert.ToInt64(uiComboBox1.SelectedValue);
                _infoDto.ISDelete = false;
                _infoDto.UpdateTime = DateTime.Now;
                return _infoDto;
            }
            set
            {
                _infoDto = value;
                txtusername.Text = value.UserName;
                txtfullname.Text = value.FullName;
                txtdepartment.Text = value.Department;
                txtjobnumber.Text = value.JobNumber;

            }
        }

        private async void GetRoleInfoDto()
        {

            var userInfoDtos = await DataAPI.GetAllRoleListAsync();
            uiComboBox1.ValueMember = "ID";
            uiComboBox1.DisplayMember = "FullName";
            uiComboBox1.DataSource = userInfoDtos;
            if (_infoDto != null)
            {
                var item = userInfoDtos.Find(it => it.ID == _infoDto.RoleID);
                uiComboBox1.SelectedItem = item;
            }
        }
    }
}
