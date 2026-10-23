using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterStation.Client.Model.UserDto
{
    public class FunctionInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 功能编号
        /// </summary>
       public string Code { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
       public string FullName { get; set; }

        /// <summary>
        /// 界面URL
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 权限说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 操作选项
        /// </summary>
        public int Operation { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public int Symbol { get; set; }




    }
}
