using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WaterStation.Client.LayoutManageMoudle
{
    /// <summary>
    /// 吸光度光谱曲线弹窗
    /// </summary>
    public partial class XgdCurvePopup : UIForm
    {
        private static readonly Color[] SeriesColors =
        {
            Color.FromArgb(26, 101, 143),
            Color.OrangeRed,
            Color.ForestGreen,
            Color.MediumPurple,
            Color.DarkGoldenrod
        };

        public XgdCurvePopup(DateTime collectTime, IReadOnlyList<XgdCurveSeries> seriesList)
        {
            InitializeComponent();
            Text = $"吸光度曲线 - {collectTime:yyyy/M/d HH:mm}";
            lblTitle.Text = $"采集时间：{collectTime:yyyy/M/d HH:mm}（共 {seriesList.Count} 条光谱）";
            RenderChart(seriesList);
        }

        private void RenderChart(IReadOnlyList<XgdCurveSeries> seriesList)
        {
            var option = new UILineOption
            {
                ToolTip = { Visible = true },
                Title = new UITitle
                {
                    Text = "吸光度光谱",
                    SubText = "横轴：波长点序号，纵轴：吸光度"
                }
            };
            option.XAxisType = UIAxisType.Value;
            option.XAxis.Name = "波长点序号";
            option.YAxis.Name = "吸光度";
            option.YAxis.AxisLabel.DecimalCount = 3;
            option.YAxis.AxisLabel.AutoFormat = false;

            for (int s = 0; s < seriesList.Count; s++)
            {
                var data = seriesList[s];
                string seriesName = seriesList.Count == 1
                    ? "吸光度"
                    : data.SnapshotTime.ToString("HH:mm:ss");
                var color = SeriesColors[s % SeriesColors.Length];
                var series = option.AddSeries(new UILineSeries(seriesName, color));
                series.Smooth = true;
                for (int i = 0; i < data.Values.Count; i++)
                    series.Add(i + 1, data.Values[i]);
            }

            lcXgd.SetOption(option);
            lcXgd.Refresh();
        }
    }

    /// <summary>
    /// 单条吸光度光谱快照
    /// </summary>
    public class XgdCurveSeries
    {
        public DateTime SnapshotTime { get; set; }
        public List<double> Values { get; set; } = new();
    }
}
