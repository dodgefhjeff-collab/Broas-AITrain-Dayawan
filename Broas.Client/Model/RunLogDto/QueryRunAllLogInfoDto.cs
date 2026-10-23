using System;

namespace WaterStation.Client.Model.RunLogDto
{
    /// <summary>
    /// 所有运行日志
    /// </summary>
    public class QueryRunAllLogInfoDto
    {

        #region  所有运行日志

        /// <summary>
        /// 主键ID
        /// </summary>

        public long ID { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReceiptCode { get; set; }

        /// <summary>
        /// 生产入库、重工入库、退料入库等
        /// </summary>
        public int ReceiptType { get; set; }

        /// <summary>
        /// 1有效，2手动关闭，3自动关闭；
        /// </summary>
        public int ReceiptState { get; set; }

        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime ReceivingSatrtTime { get; set; } = new DateTime(1900, 1, 1);


        /// <summary>
        /// 收货日期
        /// </summary>
        public DateTime ReceivingEndTime { get; set; } = new DateTime(1900, 1, 1);

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
        /// 0,1,2 (全选)
        /// </summary>
        public int CheckState { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int PageCount { get; set; }


        #endregion

        #region 新增字段
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
        /// 日志来源
        /// </summary>
        public string LogSource { get; set; }

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
