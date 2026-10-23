using SqlSugar;
using System;

namespace WaterStation.Client.Model.LayoutDto
{
    /// <summary>
    /// 系统参数设置实体
    /// 
    /// 20220901
    /// </summary>
    [SugarTable("SystemParaSetInfo")]
    public class SystemParaSetInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long ID { get; set; }
       
        /// <summary>
        /// 串口名称
        /// </summary>
        public string ComName { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public string BTL { get; set; }
        /// <summary>
        /// 停止位
        /// </summary>
        public string StopTZW { get; set; }
        /// <summary>
        /// 校验码
        /// </summary>
        public string checkJYW { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime addtime { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string adduser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime updateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Filed1 { get; set; }
    }
}
