using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterStation.Client.Model.UserDto
{
    public class RoleFunctionInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long ID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleID { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public long FunctionID { get; set; }

        /// <summary>
        /// 一级根界面ID
        /// </summary>
        public long ParentID { get; set; }

        /// <summary>
        /// 界面序号
        /// </summary>
        public int TreeIndex { get; set; }

        /// <summary>
        /// 是否弹出子界面 ture 弹出  false 不弹出  
        /// </summary>
        public bool IsPopUpWindow { get; set; }

        /// <summary>
        /// 界面控件操作选项
        /// </summary>
        public int Operation { get; set; }

        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<RoleFunctionInfo> Child { get; set; } 
    }
}
