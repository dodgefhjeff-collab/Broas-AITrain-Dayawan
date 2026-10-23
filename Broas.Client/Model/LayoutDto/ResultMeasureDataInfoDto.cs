using System;

namespace WaterStation.Client.Model.LayoutDto
{
    /// <summary>
    /// 历史数据查询
    /// update 20221110
    /// </summary>
    public class ResultMeasureDataInfoDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        ///设备编号
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        ///设备编号
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        ///采集时间
        /// </summary>
        public DateTime CollectTime { get; set; }
     
        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        ///AmmoniaADValue
        /// </summary>
        public decimal AmmoniaADValue { get; set; }

        /// <summary>
        ///总氮
        /// </summary>
        public decimal TotalNitrogenVal { get; set; }

        /// <summary>
        ///总磷
        /// </summary>
        public decimal TotalPhosphorusVal { get; set; }

        /// <summary>
        ///COD值
        /// </summary>
        public decimal TotalCODVal { get; set; }

        /// <summary>
        ///浊度值
        /// </summary>
        public decimal TurbidityVal { get; set; }
        /// <summary>
        ///硝酸盐氮
        /// </summary>
        public decimal XSYDVal { get; set; }

        /// <summary>
        ///室温
        /// </summary>
        public decimal RoomTemperature { get; set; }

        /// <summary>
        ///氨氮测量时间
        /// </summary>
        public DateTime AmmoniaADTime { get; set; }

        /// <summary>
        ///总氮测量时间
        /// </summary>
        public DateTime TotalNitrogenTime { get; set; }

        /// <summary>
        ///总磷测试时间
        /// </summary>
        public DateTime TotalPhosphorustime { get; set; }
        /// <summary>
        ///COD测量时间
        /// </summary>
        public DateTime TotalCODTime { get; set; }

        /// <summary>
        ///五参数测试时间
        /// </summary>
        public DateTime XSYDTime { get; set; }

    }
}
