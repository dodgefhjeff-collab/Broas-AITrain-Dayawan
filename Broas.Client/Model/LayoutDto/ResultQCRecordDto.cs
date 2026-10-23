using System;

namespace WaterStation.Client.Model.LayoutDto
{
    public class ResultQCRecordDto
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 质控因子
        /// </summary>
        public string QCName { get; set; }
        /// <summary>
        /// 质控时间
        /// </summary>
        public DateTime QCTime { get; set; }

        /// <summary>
        /// 首次测量时间
        /// </summary>
        public DateTime FirstTestTime { get; set; }
        /// <summary>
        /// 首次测量值
        /// </summary>
        public decimal LiuliangVal { get; set; }
        /// <summary>
        /// 二次测量时间
        /// </summary>
        public DateTime SecondTestTime { get; set; }
        /// <summary>
        /// 二次测量值
        /// </summary>
        public decimal YuliangVal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
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

        public DateTime SatrtTime { get; set; } = new DateTime(1900, 1, 1);

        public DateTime EndTime { get; set; } = new DateTime(1900, 1, 1);

    }
}
