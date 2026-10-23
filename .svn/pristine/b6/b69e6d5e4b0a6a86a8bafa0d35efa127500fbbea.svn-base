using Sunny.UI;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WaterStation.Client.Model.UserDto;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.Commom;

namespace WaterStation.Client.UserManageMoudle
{
    public partial class FormRoleFunctionSubEdit : UIEditForm
    {
        private void FormRoleFunctionSubEdit_Load(object sender, EventArgs e)
        {
            GetFunctionInfoDtoAsync();
        }
        public FormRoleFunctionSubEdit()
        {
            InitializeComponent();
            ButtonOkClick += FormRoleFunctionInfoEdit_ButtonOkClick;
        }

        private async void FormRoleFunctionInfoEdit_ButtonOkClick(object sender, EventArgs e)
        {
            try
            {
                var headerResult = await DataAPI.AddRoleFunctionInfoAsync(RoleFunctionInfoDto);
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

        public FormRoleFunctionSubEdit(RoleFunctionInfoDto _RoleFunctionInfoDto)
        {
            InitializeComponent();
            RoleFunctionInfoDto = _RoleFunctionInfoDto;
            if (_RoleFunctionInfoDto.ID == 0)
                ButtonOkClick += FormRoleFunctionInfoEdit_ButtonOkClick;
            else
                ButtonOkClick += FormRoleFunctionInfoEdit_ButtonOkClick1;
            GetParentRoleFunctionInfoTree(_RoleFunctionInfoDto.RoleID);
        }

        private async void FormRoleFunctionInfoEdit_ButtonOkClick1(object sender, EventArgs e)
        {
            try
            {
                RoleFunctionInfo roleFunction = new()
                {
                    FunctionID = RoleFunctionInfoDto.FunctionID,
                    ParentID = RoleFunctionInfoDto.ParentID,
                    TreeIndex = RoleFunctionInfoDto.TreeIndex,
                };
                var headerResult = await DataAPI.UpdateRoleFunctionInfoAsync(roleFunction);
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

        private RoleFunctionInfoDto _RoleFunctionInfoDto;

        public RoleFunctionInfoDto RoleFunctionInfoDto
        {
            get
            {
                _RoleFunctionInfoDto ??= new RoleFunctionInfoDto();
                _RoleFunctionInfoDto.FunctionID = (nodeSelect.SelectedItem as FunctionInfo).ID;
                if (parentNodeSelect.SelectedItem != null)
                    _RoleFunctionInfoDto.ParentID = (parentNodeSelect.SelectedItem as RoleFunctionInfoDto).ID;
                _RoleFunctionInfoDto.TreeIndex = int.TryParse(treeIndex.Text.Trim(), out var intValue) ? intValue : 0;
                return _RoleFunctionInfoDto;
            }
            set
            {
                _RoleFunctionInfoDto = value;
                treeIndex.Text = _RoleFunctionInfoDto.TreeIndex.ToString();
            }
        }

        protected override bool CheckData()
        {
            return CheckEmpty(parentNodeSelect, "请输入上级权限")
                && CheckEmpty(nodeSelect, "请输入权限名称") && CheckEmpty(treeIndex, "请输入排序值") && CheckIsInt(treeIndex, "排序值请输入数字");

        }

        private void GetFunctionInfoDtoAsync()
        {
            var functionData = Global.functionInfoDtos;
            this.nodeSelect.DataSource = functionData;
            this.nodeSelect.DisplayMember = "FullName";
            this.nodeSelect.ValueMember = "ID";
            if (_RoleFunctionInfoDto.FunctionID != 0)
                this.nodeSelect.SelectedItem = functionData.FirstOrDefault(s => s.ID == _RoleFunctionInfoDto.FunctionID);
        }

        /// <summary>
        /// 获取父级权限下拉
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private async void GetParentRoleFunctionInfoTree(long roleID)
        {
            var headerResult = await DataAPI.GetRoleFunctionDataByRoleId(roleID);
            if (headerResult.Count > 0)
            {
                var roleFunctionInfoDtos = headerResult;
                var parentFunctions = roleFunctionInfoDtos.Where(s => s.ParentID == 0).ToList();
                this.parentNodeSelect.DataSource = parentFunctions;
                this.parentNodeSelect.DisplayMember = "FunctionName";
                this.parentNodeSelect.ValueMember = "ID";
                if (_RoleFunctionInfoDto.ParentID != 0)
                    this.parentNodeSelect.SelectedItem = roleFunctionInfoDtos.FirstOrDefault(s => s.ID == _RoleFunctionInfoDto.ParentID);
            }
        }
    }
}
