using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterStation.Client.Controller;
using System.Collections.Generic;
using WaterStation.Client.Model.LayoutDto;
using WaterStation.Client.Model.Commom;

namespace WaterStation.Client.LayoutManageMoudle
{
    public partial class CountReport : Form
    {
        UILineOption optionCOD;//cod
        private readonly Snowflake _Snowflake = new();//生成id
        public class YuceData
        {
            public int batch_size {  get; set; } 
            public double results {  get; set; }
        }

        public CountReport()
        {
            InitializeComponent();
            getCODData();
            System.Timers.Timer timer = new()
            {
                Interval = 60 * 1000,
                Enabled = true
            };
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute == 0)
                GetYuceDataCOD();
            getCODData();
        }
        public  class Hourdata
        {
            public List<List<List<double>>> batch_data {get; set; }   
        }
        private async void GetYuceDataCOD()
        {
            var a = DataAPI.GetHourData(1, "仪表数据");
            Hourdata datas = new()
            {
                batch_data = new()
            };
            List<List<double>> data2 = new();
            for (int i = 0; i < 24; i++)
                data2.Add(new List<double>() {
                    Convert.ToDouble( a[i].TotalCODVal),Convert.ToDouble(a[i].XSYDVal),Convert.ToDouble( a[i].TotalNitrogenVal), Convert.ToDouble(a[i].AmmoniaADValue),
                    Convert.ToDouble(a[i].TotalPhosphorusVal), Convert.ToDouble(a[i].TurbidityVal) });
            datas.batch_data.Add (data2);
            var YuceValCOD = await GetYuceData("http://127.0.0.1:8001/predict", datas);
            if(YuceValCOD != null)
            {
                MeasureData b = new()
                {
                    ID = Math.Abs(_Snowflake.GetId()),
                    DataType = "预测数据",
                    CollectTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00")).AddHours(1),
                    TotalCODVal = Math.Round(Convert.ToDecimal(YuceValCOD.results), 3),
                    Remark = "小时"
                };
                await DataAPI.AddMeasureDataAsync(b);
            }
        }
        /// <summary>
        /// 获取COD数据
        /// </summary>
        private void getCODData()
        {
            #region 曲线图
            var a = DataAPI.GetHourData(0, "仪表数据");
            var b = DataAPI.GetHourData(0, "预测数据");
            optionCOD = new UILineOption();
            optionCOD.ToolTip.Visible = true;
            optionCOD.Title = new UITitle
            {
                Text = "COD",
                SubText = "趋势图"
            };
            optionCOD.XAxisType = UIAxisType.DateTime;
            var seriesCOD = optionCOD.AddSeries(new UILineSeries("实时", Color.BlueViolet));
            for (int i = 0; i < a.Count; i++)
                seriesCOD.Add(a[i].CollectTime, Convert.ToDouble(a[i].TotalCODVal));
            var seriesYuceCOD = optionCOD.AddSeries(new UILineSeries("预测", Color.Gold));
            for (int i = 0; i < b.Count; i++)
                seriesYuceCOD.Add(b[i].CollectTime, Convert.ToDouble(b[i].TotalCODVal));
            seriesYuceCOD.Smooth = true;
            //seriesCOD.Symbol = UILinePointSymbol.Square;
            //seriesCOD.SymbolSize = 4;
            //seriesCOD.SymbolLineWidth = 2;
            //seriesCOD.SymbolColor = Color.BlueViolet;
            seriesCOD.Smooth = true;//平滑曲线

            //optionCOD.GreaterWarningArea = new UILineWarningArea(14);
            //optionCOD.LessWarningArea = new UILineWarningArea(0, Color.Gold);
            //optionCOD.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Red, Name = "上限", Value = 5000 });
            //optionCOD.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Gold, Name = "下界", Value = 0 });
            optionCOD.XAxis.Name = "时间";
            optionCOD.YAxis.Name = "数值";
            optionCOD.XAxis.AxisLabel.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            optionCOD.XAxis.AxisLabel.AutoFormat = false;
            optionCOD.YAxis.AxisLabel.DecimalCount = 3;
            optionCOD.YAxis.AxisLabel.AutoFormat = false;
            lcCOD.SetOption(optionCOD);
            lcCOD.Refresh();
            #endregion
        }

        /// <summary>
        /// 如果是本地服务，不使用https
        /// </summary>
        /// <returns></returns>
        private static HttpClientHandler GetInsecureHandler()
        {
            var handler = new HttpClientHandler
            {
                UseDefaultCredentials = true
            };
            return handler;
        }
        public static HttpClient CreateHttpClient()
        {
            var handler = GetInsecureHandler();
            HttpClient httpClient = new(handler);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            return httpClient;
        }

        public async Task<YuceData> GetYuceData(string url, object data)
        {
            try
            {
                var client = CreateHttpClient();
                string content = JsonConvert.SerializeObject(data);
                var buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(url, byteContent).ConfigureAwait(false);
                var obj = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<YuceData>(obj);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ShowTip.ShowErrorTip($"GetAsync End, url:{url}, HttpStatusCode:{response.StatusCode}, result:{result}");
                }
                return result;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    ShowTip.ShowErrorTip($"response :{responseContent}"+ ex);
                }
                return null;
            }
        }

        private void btn1Start_Click(object sender, EventArgs e)
        {
            GetYuceDataCOD();
        }
    }
}
