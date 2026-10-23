namespace WaterStation.Client.Model.UserDto
{
    public class QueryRoleInfoDto
    {
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

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
}
