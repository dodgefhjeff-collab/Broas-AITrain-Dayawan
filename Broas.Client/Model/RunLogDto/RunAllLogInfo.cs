using SqlSugar;
using System;

namespace WaterStation.Client.Model.RunLogDto
{
    /// <summary>
    /// 所有运行日志信息
    /// 
    /// 20220801
    /// </summary>
    public class RunAllLogInfo
    {
        #region  属性

        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long ID { get; set; }

       
        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime ReceivingTime { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public string Creator { get; set; }


        /// <summary>
        /// 启用状态
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Addtime { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        public string AddUser { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Remark { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType { get; set; }
        #endregion
    }
}
