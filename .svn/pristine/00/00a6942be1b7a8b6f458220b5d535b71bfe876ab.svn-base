namespace WaterStation.Client.Model.LayoutDto
{
    /// <summary>
    /// 仪表质控核查类型。
    /// </summary>
    public enum InstrumentQCCheckType
    {
        /// <summary>零点核查 LD</summary>
        ZeroCheck = 0,
        /// <summary>跨度核查 KD</summary>
        SpanCheck = 1,
        /// <summary>标样核查 BY</summary>
        SampleCheck = 2,
        /// <summary>加标回收 JB</summary>
        AddSample = 3
    }

    /// <summary>
    /// 仪表质控单路定时调度配置（内存模型，与 App.config 同步）。
    /// </summary>
    public class InstrumentQCScheduleItem
    {
        /// <summary>配置键，如 LDZhikongZD</summary>
        public string ConfigKey { get; set; }

        /// <summary>因子后缀：ZD/AD/ZL/COD</summary>
        public string FactorKey { get; set; }

        /// <summary>分析仪显示名：总氮/氨氮/总磷/COD</summary>
        public string AnalyzerName { get; set; }

        /// <summary>核查类型</summary>
        public InstrumentQCCheckType CheckType { get; set; }

        /// <summary>周期：00=每天，0-6=星期（周日=0）</summary>
        public string Day { get; set; } = "1";

        /// <summary>小时 0-23</summary>
        public int Hour { get; set; }

        /// <summary>分钟 0-59</summary>
        public int Minute { get; set; }

        /// <summary>是否已开启定时</summary>
        public bool Enabled { get; set; }

        /// <summary>核查浓度（加标无此字段）</summary>
        public string Concentration { get; set; } = "0";

        /// <summary>是否需要浓度字段</summary>
        public bool HasConcentration => CheckType != InstrumentQCCheckType.AddSample;

        /// <summary>核查类型显示名</summary>
        public string CheckTypeDisplayName => CheckType switch
        {
            InstrumentQCCheckType.ZeroCheck => "零点核查",
            InstrumentQCCheckType.SpanCheck => "跨度核查",
            InstrumentQCCheckType.SampleCheck => "标样核查",
            InstrumentQCCheckType.AddSample => "加标回收",
            _ => CheckType.ToString()
        };

        /// <summary>开关按钮文案：已开启 / 开启</summary>
        public string EnabledButtonText => Enabled ? "已开启" : "开启";
    }
}
