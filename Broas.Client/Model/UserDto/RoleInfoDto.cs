using System;

namespace WaterStation.Client.Model.UserDto
{
    public class RoleInfoDto
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 角色说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime UpdateTime { get; set; }




    }
}
