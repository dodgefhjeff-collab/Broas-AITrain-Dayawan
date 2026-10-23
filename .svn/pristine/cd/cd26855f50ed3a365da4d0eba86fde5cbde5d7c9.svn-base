using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WaterStation.Client.Model.UserDto;
using WaterStation.Client.Controller;

namespace WaterStation.Client.UserManageMoudle
{
    public partial class FormReFunctionEdit : UIEditForm
    {
        private void FormReFunctionEdit_Load(object sender, EventArgs e)
        {
            roleTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
        }

        public FormReFunctionEdit()
        {
            InitializeComponent();
        }

        public FormReFunctionEdit(RoleInfoDto _dto)
        {
            InitializeComponent();
            if (_RoleInfoDto == null)
            {
                _RoleInfoDto = new RoleInfoDto();
                _RoleInfoDto.ID = _dto.ID;
                _RoleInfoDto.FullName = _dto.FullName;
                _RoleInfoDto.Description = _dto.Description;
                _RoleInfoDto.Code = _dto.Code;
                txtrolename.Text = _RoleInfoDto.FullName;
                txtdesciption.Text = _RoleInfoDto.Description;
                rolecode.Text = _RoleInfoDto.Code;
            }
            GetReFunctionTree(_dto.ID);
        }

        protected override bool CheckData()
        {
            return CheckEmpty(txtrolename, "请输入名称");
        }


        private async void GetReFunctionTree(long rid)
        {
            var roleFunctionData = await DataAPI.GetRoleFunctionByRoleidAsync(rid);
            var functionInfos = Global.functionInfoDtos;
            //一级节点
            var parentReFunctiionData = roleFunctionData.Where(s => s.ParentID == 0).ToList();
            //先清后加
            this.roleTreeView.Nodes.Clear();
            foreach (var item in roleFunctionData)
            {
                //一级节点
                string parent = functionInfos.Find(p => p.ID == item.FunctionID).FullName;
                //二级节点
                var childNodes = new List<TreeNode>();
                foreach (RoleFunctionInfoDto temp in item.Child)
                {
                    string child = functionInfos.Find(p => p.ID == temp.FunctionID).FullName;
                    var nodeChild = new TreeNode(child);
                    nodeChild.Tag = temp;
                    childNodes.Add(nodeChild);
                }
                var nodeParent = new TreeNode(parent, childNodes.ToArray());
                nodeParent.Tag = item;
                this.roleTreeView.Nodes.Add(nodeParent);
            }
        }

        /// <summary>
        /// 树单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            roleTreeView.SelectedNode = e.Node; // 
            SetTreeNodeChecked(roleTreeView.SelectedNode);
        }


        /// <summary>
        /// 节点全选/取消
        /// </summary>
        /// <param name="tn"></param>
        private void SetTreeNodeChecked(TreeNode tn)
        {
            if (tn == null) return; // 若为空，则返回
            if (tn.Nodes.Count > 0)
            {
                foreach (TreeNode item in tn.Nodes)
                {
                    item.Checked = tn.Checked;
                    SetTreeNodeChecked(item);
                }
            }
        }
        #region tools
        /// <summary>
        /// 将ViewModel中的值传递给Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <param name="hasDifferentType">是否存在名字相同类型不同的字段</param>
        public static void MergeModel<T>(ref T target, object source, bool hasDifferentType = false)
        {
            try
            {
                if (source == null)
                    return;
                PropertyInfo[] sourceProperties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo item in sourceProperties)
                {
                    string name = item.Name;
                    object value = item.GetValue(source, null);
                    PropertyInfo propertyInfo = target.GetType().GetProperty(name);
                    if (propertyInfo != null && value != null)
                    {
                        if (value is string)
                            value = value.ToString().Trim();
                        if (hasDifferentType)
                            if (item.PropertyType == propertyInfo.PropertyType)
                                propertyInfo.SetValue(target, value, null);
                        else
                            propertyInfo.SetValue(target, value, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private RoleInfoDto _RoleInfoDto;
        public RoleInfoDto RoleInfoDto
        {
            get
            {
                _RoleInfoDto ??= new RoleInfoDto();
                _RoleInfoDto.FullName = txtrolename.Text.Trim();
                _RoleInfoDto.Description = txtdesciption.Text.Trim();
                _RoleInfoDto.Code = rolecode.Text.Trim();
                return _RoleInfoDto;
            }
            set
            {
                _RoleInfoDto = value;
                if (_RoleInfoDto == null)
                {
                    ShowTip.ShowErrorTip("加载数据异常");
                    return;
                }
                txtrolename.Text = _RoleInfoDto.FullName;
                txtdesciption.Text = _RoleInfoDto.Description;
                rolecode.Text = _RoleInfoDto.Code;
            }
        }

        /// <summary>
        /// 新增编辑权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            var nodes = this.roleTreeView.Nodes;
            var count = 0;
            RoleFunctionInfoDto checkNodeTag = null;
            foreach (TreeNode item in nodes)
            {
                if (item.Checked)
                {
                    count++;
                    checkNodeTag = item.Tag as RoleFunctionInfoDto;
                }
                foreach (TreeNode node in item.Nodes)
                {
                    if (node.Checked)
                    {
                        count++;
                        checkNodeTag = node.Tag as RoleFunctionInfoDto;
                    }
                }
                if (count > 1)
                {
                    ShowTip.ShowErrorTip("只允许单个权限编辑");
                    return;
                }
            }
            if (checkNodeTag == null)
            {
                checkNodeTag = new RoleFunctionInfoDto();
                checkNodeTag.RoleID = RoleInfoDto.ID;
            }
            FormRoleFunctionSubEdit subForm = new FormRoleFunctionSubEdit(checkNodeTag);
            subForm.ShowDialog();
            if (subForm.DialogResult == DialogResult.OK)
            {
                subForm.Close();
                subForm.Dispose();
                GetReFunctionTree(checkNodeTag.RoleID);
            }
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            var nodes = this.roleTreeView.Nodes;
            List<RoleFunctionInfoDto> checkNodeTags = new ();
            foreach (TreeNode item in nodes)
            {
                if (item.Checked)
                {
                    var checkNodeTag = item.Tag as RoleFunctionInfoDto;
                    checkNodeTags.Add(checkNodeTag);
                }
                foreach (TreeNode node in item.Nodes)
                {
                    if (node.Checked)
                    {
                        var checkNodeTag = node.Tag as RoleFunctionInfoDto;
                        checkNodeTags.Add(checkNodeTag);
                    }
                }
            }

            if (checkNodeTags.Count > 0)
            {
                var headerResult = await DataAPI.DeleteRoleFunctionInfoAsync(checkNodeTags);
                if (headerResult)
                {
                    ShowTip.ShowSuccessTip("删除成功");
                    GetReFunctionTree(RoleInfoDto.ID);
                }
                else
                    ShowTip.ShowErrorTip("删除失败");
            }
            else
            {
                ShowTip.ShowErrorTip("请选择权限删除");
                return;
            }
        }
    }
}
