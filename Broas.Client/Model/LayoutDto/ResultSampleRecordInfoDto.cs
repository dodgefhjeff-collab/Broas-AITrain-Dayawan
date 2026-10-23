using System;

namespace WaterStation.Client.Model.LayoutDto
{
    public class ResultSampleRecordInfoDto
    {
        #region 属性
        /// <summary>
        /// 主键ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 货位编号
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public long WarehouseID { get; set; }

        /// <summary>
        /// 库区ID
        /// </summary>
        public long WarehouseAreaID { get; set; }

        /// <summary>
        /// 巷道ID
        /// </summary>
        public long AisleID { get; set; }

        /// <summary>
        /// 货位类型ID
        /// </summary>
        public long LocationTypeID { get; set; }

        /// <summary>
        /// 货位区域号
        /// </summary>
        public string LocationAreaCode { get; set; }

        /// <summary>
        /// 货位排
        /// </summary>
        public int LocationRow { get; set; }

        /// <summary>
        /// 货位列
        /// </summary>
        public int LocationColumn { get; set; }

        /// <summary>
        /// 货位层
        /// </summary>
        public int LocationLayer { get; set; }

        /// <summary>
        /// 货位深位
        /// </summary>
        public int LocationDepth { get; set; }

        /// <summary>
        /// 允许上架
        /// </summary>
        public bool AllowUp { get; set; }

        /// <summary>
        /// 允许下架
        /// </summary>
        public bool AllowDown { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 货位任务状态 0任务空闲 1 入库锁定  2 出库锁定
        /// </summary>
        public int AllotState { get; set; }

        /// <summary>
        /// 冻结状态
        /// </summary>
        public bool IsFrozen { get; set; }

        /// <summary>
        /// 1=空闲, 2=工作中,3=异常,4=维修
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 是否火警
        /// </summary>
        public bool IsFireAlarm { get; set; }

        public int CheckStateAllowUp { get; set; }

        public int CheckStateAllowDown { get; set; }

        public int CheckStateFrozen { get; set; }


        #region 联查仓库,库区,巷道 ,货位类型

        /// <summary>
        /// 仓库编号
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; } = string.Empty;

        /// <summary>
        ///库区编号
        /// </summary>
        public string WarehouseAreaCode { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        public string WarehouseAreaName { get; set; } = string.Empty;

        /// <summary>
        /// 巷道编号
        /// </summary>
        public string AisleCode { get; set; }

        /// <summary>
        /// 巷道名称
        /// </summary>
        public string AisleName { get; set; }

        /// <summary>
        /// 货位类型编号
        /// </summary>
        public string LocationTypeCode { get; set; }

        /// <summary>
        /// 货位类型名称
        /// </summary>
        public string LocationTypeName { get; set; }

        #endregion




        #endregion

        #region 新增字段 20220721 
        /// <summary>
        /// 留样品编号
        /// </summary>
        public string SampleCode { get; set; }

        /// <summary>
        /// 留样时间
        /// </summary>
        public DateTime SampleTime { get; set; }
        /// <summary>
        /// 超标数据
        /// </summary>
        public string OverproofData { get; set; }

        /// <summary>
        /// 超标数值
        /// </summary>
        public string OverproofValue { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        public string Adduser { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        public DateTime Addtime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public DateTime SatrtTime { get; set; } = new DateTime(1900, 1, 1);

        public DateTime EndTime { get; set; } = new DateTime(1900, 1, 1);
        #endregion


    }
}
