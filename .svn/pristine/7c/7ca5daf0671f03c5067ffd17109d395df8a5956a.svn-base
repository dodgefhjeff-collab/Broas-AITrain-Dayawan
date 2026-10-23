using SqlSugar;
using System;

namespace WaterStation.Client.Model.LayoutDto
{
    public class QCRecord
    {
        /// <summary>
        /// 主键ID
        /// </summary>
       
        [SugarColumn(IsPrimaryKey = true)]
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

    }
}
