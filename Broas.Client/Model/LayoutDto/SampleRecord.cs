using SqlSugar;
using System;

namespace WaterStation.Client.Model.LayoutDto
{
    public class SampleRecord
    {
        /// <summary>
        /// 主键ID
        /// </summary>
       
        [SugarColumn(IsPrimaryKey = true)]
        public long ID { get; set; }

        /// <summary>
        /// 留样因子名称
        /// </summary>
        public string SampleName { get; set; }

        /// <summary>
        /// 留样因子编码
        /// </summary>
        public string SampleCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 留样时间
        /// </summary>
        public DateTime SampleTime { get; set; }

        /// <summary>
        /// 因子测量时间
        /// </summary>
        public DateTime TestTime { get; set; }
        /// <summary>
        /// 因子测量值
        /// </summary>
        public decimal TestVal { get; set; }
       

    }
}
