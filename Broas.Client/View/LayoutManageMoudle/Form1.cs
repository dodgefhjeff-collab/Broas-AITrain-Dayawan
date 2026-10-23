using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

using System.IO;
using Broas.WMS.Client.LayoutManageMoudle.LayoutDto;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.LayoutDto;

namespace SerialPort
{
    public partial class Form1 : Form
    {
       Snowflake snowflake = new Snowflake();
        HJ212_2017 hj212 = new HJ212_2017();//hj212数据解析对象
        static string constr = "data source=.;initial catalog= broasWMS;user id= sa;pwd= 12356";
        SqlConnection con = new SqlConnection(constr);//开始连接数据库
        string Var_Atten_1;
        int All_S_Bytes = 0;   //发送字符数统计数
        int All_R_Bytes = 0;   //接收字符数统计数

        string version = string.Empty;//获取版本号
        System.Timers.Timer timer = new System.Timers.Timer();

        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(mydefault_parameters); //加载默认状态下串口的参数
            this.txtVersion.Text = "0";
            version = this.txtVersion.Text;



        }
        /// <summary>
        /// 获取烟感报警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 得到intHour,intMinute,intSecond，是当前系统时间  
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;

            int hour = 10;
            int minuter = 30;
            int second = 1;

            //自己设置hour,minuter,second,到设定的时间就开始执行程序  
            //if (intHour == hour && intMinute == minuter && intSecond == second)
            if (intSecond % 2 == 0)
            {
                //写入你要执行的方法函数
                //System.Windows.Forms.MessageBox.Show(intHour.ToString());
                try
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richTextBox1.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte("01 03 00 03 00 01 74 0A");
                    }


                    if (this.SendData(sendData))
                    {
                        //label81.Invoke(new MethodInvoker(delegate
                        //{
                        //    label81.Text = (richTextBox1.Text.Length).ToString();
                        //    //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        //}));

                    }
                    else
                    {

                    }
                    //richTextBox2.Invoke(new MethodInvoker(delegate
                    //{
                    //    richTextBox2.Text = richTextBox2.Text + "  发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";

                    //}));


                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 获取温湿度
        /// czb 20220623
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 得到intHour,intMinute,intSecond，是当前系统时间  
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;

            int hour = 10;
            int minuter = 30;
            int second = 1;

            //自己设置hour,minuter,second,到设定的时间就开始执行程序  
            //if (intHour == hour && intMinute == minuter && intSecond == second)
            if (intSecond % 2 == 0)
            {
                //写入你要执行的方法函数
                //System.Windows.Forms.MessageBox.Show(intHour.ToString());
                try
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richTextBox1.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte("01 03 00 00 00 02 C4 0B");
                    }


                    if (this.SendData(sendData))
                    {
                        //label81.Invoke(new MethodInvoker(delegate
                        //{
                        //    label81.Text = (richTextBox1.Text.Length).ToString();
                        //    //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        //}));

                    }
                    else
                    {

                    }
                    //richTextBox2.Invoke(new MethodInvoker(delegate
                    //{
                    //    richTextBox2.Text = richTextBox2.Text + "  发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";

                    //}));


                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取浸水
        /// czb 20220623
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 得到intHour,intMinute,intSecond，是当前系统时间  
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;

            int hour = 10;
            int minuter = 30;
            int second = 1;

            //自己设置hour,minuter,second,到设定的时间就开始执行程序  
            //if (intHour == hour && intMinute == minuter && intSecond == second)
            if (intSecond % 2 == 0)
            {
                //写入你要执行的方法函数
                //System.Windows.Forms.MessageBox.Show(intHour.ToString());
                try
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richTextBox1.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte("01 03 00 00 00 01 84 0A");
                    }


                    if (this.SendData(sendData))
                    {
                        //label81.Invoke(new MethodInvoker(delegate
                        //{
                        //    label81.Text = (richTextBox1.Text.Length).ToString();
                        //    //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        //}));

                    }
                    else
                    {

                    }
                    //richTextBox2.Invoke(new MethodInvoker(delegate
                    //{
                    //    richTextBox2.Text = richTextBox2.Text + "  发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";

                    //}));


                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 仪表五参数
        /// czb 20220623
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer4_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            // 得到intHour,intMinute,intSecond，是当前系统时间  
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;

            int hour = 10;
            int minuter = 30;
            int second = 1;

            //自己设置hour,minuter,second,到设定的时间就开始执行程序  
            //if (intHour == hour && intMinute == minuter && intSecond == second)
            if (intSecond % 2 == 0)
            {
                //写入你要执行的方法函数
                //System.Windows.Forms.MessageBox.Show(intHour.ToString());
                try
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richTextBox1.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte("01 03 00 00 00 10 44 06");
                    }


                    if (this.SendData(sendData))
                    {
                        //label81.Invoke(new MethodInvoker(delegate
                        //{
                        //    label81.Text = (richTextBox1.Text.Length).ToString();
                        //    //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        //}));

                    }
                    else
                    {

                    }
                    //richTextBox2.Invoke(new MethodInvoker(delegate
                    //{
                    //    richTextBox2.Text = richTextBox2.Text + "  发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";

                    //}));


                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw ex;
                }
            }
        }
        //串口自定义参数
        private void mydefault_parameters(object sender, EventArgs e)
        {
            comboBox2.Items.Add("1200");
            comboBox2.Items.Add("2400");
            comboBox2.Items.Add("4800");
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("14400");
            comboBox2.Items.Add("19200");
            comboBox2.Items.Add("38400");
            comboBox2.Items.Add("56000");
            comboBox2.Items.Add("57600");
            comboBox2.Items.Add("115200");
            comboBox2.Items.Add("128000");
            comboBox2.Items.Add("230400");
            comboBox2.Items.Add("256000");
            comboBox2.Items.Add("600000");
            comboBox2.Text = "115200";

            comboBox3.Items.Add("5");
            comboBox3.Items.Add("6");
            comboBox3.Items.Add("7");
            comboBox3.Items.Add("8");
            comboBox3.Text = "8";

            comboBox4.Items.Add("1");
            comboBox4.Items.Add("1.5");
            comboBox4.Items.Add("2");
            comboBox4.Text = "1";

            comCheck.Items.Add("None");
            comCheck.Items.Add("Odd");
            comCheck.Items.Add("Even");
            comCheck.Items.Add("Mark");
            comCheck.Items.Add("Space");
            comCheck.Text = "None";

            richTextBox2.ReadOnly = true;
            serialPort1.DataReceived += serialPort1_DataReceived;   //加载串口接收信息
        }

        private void 串口信息_Paint(object sender, PaintEventArgs e)
        {

        }
        /// <summary>
        /// 检验位
        /// </summary>
        private void comboBox_Port_Update()
        {
            comboBox1.Items.Clear();
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    comboBox1.Items.Add(sValue);
                }
            }
        }
        /// <summary>
        /// 串口号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox1.Text;
        }
        /// <summary>
        /// 串口号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox_Port_Update();
        }
        /// <summary>
        /// 更新串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)   //button: 更新串口
        {
            comboBox1.Items.Clear();
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    comboBox1.Items.Add(sValue);
                }
            }
        }
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)  //button: 打开串口
        {
            serialPort1.BaudRate = Convert.ToInt32(this.comboBox2.Text);
            if (!serialPort1.IsOpen)
            {
                try
                {
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comCheck.Enabled = false;

                    //ovalShape1.BackColor = Color.Red;
                    serialPort1.Open();
                    btnOpen.Text = "关闭串口";
                    //btnClear.Text = "打开串口";

                    timer.Enabled = true;
                    timer.Interval = 1000;//执行间隔时间,单位为毫秒  
                    timer.Start();
                    //指定每天时间1执行
                    // timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);//暂时屏蔽 陈族波 烟感
                    //timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer2_Elapsed);//暂时屏蔽 陈族波 温度湿度
                    //  timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer3_Elapsed);//暂时屏蔽 陈族波 浸水传送器

                    //timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer4_Elapsed);//暂时屏蔽 陈族波 仪表五参数20220624
                }
                catch
                {
                    MessageBox.Show("串口" + serialPort1.PortName + "打开失败！\r\n可能是串口不存在或者已被占用。");
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comCheck.Enabled = true;
                }

            }
            else
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comCheck.Enabled = true;

                serialPort1.Close();
                timer.Stop();
                //ovalShape1.BackColor = Color.White;
                btnOpen.Text = "打开串口";
                //btnClear.Text = "清除";
            }

            //get10("");


        }
        /// <summary>
        /// 串口发送
        /// czb 20220725
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {

                    #region 发送数据hex
                    //byte[] data = new byte[1];    //发送数据字符数组，每次发送一个字节的数据
                    //string strs = richTextBox1.Text.Replace(" ", "").Replace("\r\n", "");     //剔除字符串中的空格和回车
                    ////如果字符串的个数为单数，在最后一位前补0
                    //if (strs.Length % 2 == 1)
                    //{
                    //    strs = strs.Insert(strs.Length - 1, "0");
                    //}
                    ////将字符串中两个字符组成一个8进制数，循环次数为字符串长度的一半
                    //for (int i = 0; i < strs.Length / 2; i++)
                    //{
                    //    //将两位字符转换成一位16进制数
                    //    data[0] = Convert.ToByte(strs.Substring(i * 2, 2), 16);
                    //    //将发送数据字符数组的数据发送
                    //    serialPort1.Write(data, 0, 1);
                    //}
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";
                    #endregion


                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richTextBox1.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richTextBox1.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richTextBox1.Text.Trim()));//获取crc验证码
                        string crc1= CRC16(System.Text.Encoding.GetEncoding("gb2312").GetBytes(richTextBox1.Text.Trim()));//获取crc验证码
                        string crc2 = CRC16(System.Text.Encoding.ASCII.GetBytes(richTextBox1.Text.Trim()));//获取crc验证码
                        //MessageBox.Show(crc);
                        var hj212Code = GetBackCmd(richTextBox1.Text.Trim());//czb 20220719 hj212协议解码
                        //var sendStr = "##" + strLength + richTextBox1.Text.Trim() + crc + "\r\n"+ hj212Code+ "\r\n";
                        var sendStr =  hj212Code;
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释
                        #region hj212 协议解析 czb 20220726
                        if (!hj212.Crc16(sendStr) || !hj212.DecodeData(sendStr)) //未通过验证或解码失败
                        {
                            return; //不解码
                        }

                        string CN = hj212.DATA_HEAD["CN"];          //数据类型
                        string MN = hj212.DATA_HEAD["MN"];          //设备MN
                        StringBuilder sBuilder = new StringBuilder();
                     
                        //实时数据采集
                        if (CN=="2011")
                        {
                            long id = System.Math.Abs( snowflake.GetId());
                            var ResourceCode = "检测指标实时数据";
                            logHelper.WriteLogInfo("检测指标实时数据" + sendStr, version);
                            //检测指标实时数据

                            sBuilder.Append( string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark])  VALUES  ({0},'{1}','{2}','{3}','{4}')", id, ResourceCode, DateTime.Now.ToString(), sendStr, "工控机主动获取检测仪数据" ));
                        }
                        // 取设备运行状态数据
                        if (CN == "2021")
                        {
                            long id = System.Math.Abs(snowflake.GetId());
                            var ResourceCode = "取设备运行状态";
                            logHelper.WriteLogInfo("检测指标实时数据" + sendStr, version);
                            //检测指标实时数据

                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark])  VALUES  ({0},'{1}','{2}','{3}','{4}')", id, ResourceCode, DateTime.Now.ToString(), sendStr, "工控机主动获取检测仪状态数据"));
                        }

                        // 取设备运行状态数据
                        if (CN == "3021")
                        {
                             //每隔四小时传一次
                            //QN=20160801085857223;ST=32;CN=3021;PW=123456;MN=010000A8900016 F000169DC0; Flag = 5; CP =&& PolId = w01018; InfoId = i13004; i13004 - Info = 168.0 &&
                            long id = System.Math.Abs(snowflake.GetId());
                            var ResourceCode = "设置设备参数";
                            logHelper.WriteLogInfo("检测指标实时数据" + sendStr, version);
                            //检测指标实时数据

                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark]) VALUES  ({0},'{1}','{2}','{3}','{4}')", id, ResourceCode, DateTime.Now.ToString(), sendStr, "工控机设置设备参数"));
                        }
                        #region  表 C.3 设置现场机时间
                        // 20220801  czb 

                        if (CN == "1012")
                        {
                            //每隔四小时传一次
                            //QN=20160801085857223;ST=32;CN=1012;PW=123456;MN=010000A8900016  F000169DC0; Flag = 5; CP =&& PolId = w01018; SystemTime = 20160801085857 &&
                            long id = System.Math.Abs(snowflake.GetId());
                            var ResourceCode = "表 C.3 设置现场机时间";
                            logHelper.WriteLogInfo("表 C.3 设置现场机时间" + sendStr, version);
                            //检测指标实时数据

                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark]) VALUES  ({0},'{1}','{2}','{3}','{4}')", id, ResourceCode, DateTime.Now.ToString(), sendStr, "工控机设置设备现场时间"));
                        }
                        #endregion
                        //表 C.3 设置现场机时间
                        //QN=20160801085857223;ST=32;CN=1012;PW=123456;MN=010000A8900016  F000169DC0; Flag = 5; CP =&& PolId = w01018; SystemTime = 20160801085857 &&
                        #region  表 C.32 启动清洗/反吹
                        // 20220801  czb 

                        if (CN == "3013")
                        {
                            //每隔四小时传一次
                            //QN=20160801085857223;ST=32;CN=3013;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& PolId = w01018 &&
                                                            long id = System.Math.Abs(snowflake.GetId());
                            var ResourceCode = "表 C.3 设置现场机时间";
                            logHelper.WriteLogInfo("表 C.3 设置现场机时间" + sendStr, version);
                            //检测指标实时数据

                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark]) VALUES  ({0},'{1}','{2}','{3}','{4}')", id, ResourceCode, DateTime.Now.ToString(), sendStr, "工控机设置设备现场时间"));
                        }
                        #endregion
                        #region  表 C.34 超标留样
                        // 20220802 czb 

                        if (CN == "3015")
                        {                          
                            //QN=20160801085857223;ST=32;CN=3015;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&&&&
                                                            long id = System.Math.Abs(snowflake.GetId());
                            var ResourceCode = "表 C.34 超标留样";
                            logHelper.WriteLogInfo("表 C.34 超标留样" + sendStr, version);
                            //检测指标实时数据

                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],[LogType]) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", id, ResourceCode, DateTime.Now.ToString(), sendStr, "工控机主动发送命令设备超标留样","工控机发起"));
                        }
                        #endregion


                        #region  表 C.36 提取采样时间周期
                        // 20220802 czb 

                        if (CN == "3017")
                        {
                            //QN=20160801085857223;ST=32;CN=3017;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& PolId = w01018 &&
                          long id = System.Math.Abs(snowflake.GetId());
                            var ResourceCode = "表 C.36 提取采样时间周期";
                            logHelper.WriteLogInfo("表 C.36 提取采样时间周期" + sendStr, version);
                            //检测指标实时数据

                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],[LogType]) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", id, ResourceCode, DateTime.Now.ToString(), sendStr, "工控机主动发送命令设备提取采样周期", "工控机发起"));
                        }
                        #endregion
                        #region  表 C.30 零点校准量程校准
                        // 20220802 czb 

                        if (CN == "3011")
                        {
                            //QN=20160801085857223;ST=32;CN=3011;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& PolId = w01018 &&
                                                            long logID = System.Math.Abs(snowflake.GetId());
                            var LogSource = "工控机主动发送给检测仪";
                            var PolIdVal = string.Empty;
                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                            {
                                string PolId = "PolId".ToUpper(); //污染物编码
                                PolIdVal = hj212.CP[PolId];


                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                            }
                            logHelper.WriteLogInfo("表 C.30 零点校准量程校准" + sendStr, version);
                            //检测指标实时数据
                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, LogSource, DateTime.Now.ToString(), sendStr, PolIdVal, "表 C.36 提取采样时间周期"));
                        }
                        #endregion
                        #region  表 C.35 设置采样时间周期
                        // 20220802 czb 

                        if (CN == "3016")
                        {
                            //QN=20160801085857223;ST=32;CN=3016;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& PolId = w01018; CstartTime=000000; CTime=2 &&
                            long logID = System.Math.Abs(snowflake.GetId());
                            var LogSource = "工控机主动发送给检测仪采样周期";
                            var PolIdVal = string.Empty;
                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                            {
                                string PolId = "PolId".ToUpper(); //污染物编码
                                PolIdVal = hj212.CP[PolId];
                               
                            }
                            string CstartTime = string.Empty;

                            if (hj212.CP.ContainsKey("CstartTime".ToUpper()))
                            {
                                CstartTime = Convert.ToString(hj212.CP["CstartTime".ToUpper()]);//采样开始时间
                            }

                            var CTime = string.Empty;//采样周期
                            if (hj212.CP.ContainsKey("CTime".ToUpper()))
                            {
                                CTime = Convert.ToString(hj212.CP["CTime".ToUpper()]);//采样周期
                            }
                            logHelper.WriteLogInfo("表 C.35 设置采样时间周期" + sendStr, version);
                            //检测指标实时数据
                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, LogSource, DateTime.Now.ToString(), sendStr, PolIdVal+";"+ CstartTime+";"+ CTime, "表 C.35 设置采样时间周期"));
                        }
                        #endregion

                        #region  表 C.33 比对采样
                        // 20220802 czb 

                        if (CN == "3014")
                        {
                            //QN=20160801085857223;ST=32;CN=3014;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& PolId = w01018 &&
                                                            long logID = System.Math.Abs(snowflake.GetId());
                            var LogSource = "工控机主动发送给检测仪比对采样";
                            var PolIdVal = string.Empty;
                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                            {
                                string PolId = "PolId".ToUpper(); //污染物编码
                                PolIdVal = hj212.CP[PolId];

                            }
                          
                          
                            logHelper.WriteLogInfo("表 C.30 零点校准量程校准" + sendStr, version);
                            //检测指标实时数据
                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, LogSource, DateTime.Now.ToString(), sendStr, PolIdVal  , "表 C.33 比对采样"));
                        }
                        #endregion
                        #region 表 C.31 即时采样
                        // 20220802 czb 

                        if (CN == "3012")
                        {
                            //QN=20160801085857223;ST=32;CN=3012;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& PolId = w01018 &&
                                                            long logID = System.Math.Abs(snowflake.GetId());
                            var LogSource = "工控机主动发送给检测仪及时采用";
                            var PolIdVal = string.Empty;
                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                            {
                                string PolId = "PolId".ToUpper(); //污染物编码
                                PolIdVal = hj212.CP[PolId];

                            }


                            logHelper.WriteLogInfo(" 表 C.31 即时采样" + sendStr, version);
                            //检测指标实时数据
                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, LogSource, DateTime.Now.ToString(), sendStr, PolIdVal, "表 C.31 即时采样"));
                        }
                        #endregion

                        #region 表 C.37 提取出样时间
                        // 20220802 czb 

                        if (CN == "3018")
                        {
                            //QN=20160801085857223;ST=32;CN=3018;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& PolId = w01018 &&
                            long logID = System.Math.Abs(snowflake.GetId());
                            var LogSource = "工控机主动发送给检测仪提取出样时间";
                            var PolIdVal = string.Empty;
                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                            {
                                string PolId = "PolId".ToUpper(); //污染物编码
                                PolIdVal = hj212.CP[PolId];
                            }
                            logHelper.WriteLogInfo("表 C.37 提取出样时间" + sendStr, version);
                            //检测指标实时数据
                            sBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, LogSource, DateTime.Now.ToString(), sendStr, PolIdVal, "表 C.37 提取出样时间"));
                        }
                        #endregion
                        //QN=20160801085857223;ST=32;CN=3013;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& PolId = w01018 &&
                        if (sBuilder.ToString().Length>0)
                        {
                            //执行数据库语句
                            using (SqlCommand com = new SqlCommand(sBuilder.ToString(), con))
                            {
                                try
                                {
                                    con.Open();
                                    //   MessageBox.Show("成功连接数据库");
                                    com.ExecuteNonQuery();

                                    //  int x = (int)com.ExecuteScalar();
                                    //  MessageBox.Show(string.Format("成功读取{0},条记录", x));
                                }
                                catch (Exception ex)
                                {
                                    logHelper.WriteLogInfo(ex.Message, version);
                                    throw ex;

                                }
                                finally
                                {
                                    con.Close();
                                    //  MessageBox.Show("成功关闭数据库连接", "提示信息", MessageBoxButtons.YesNoCancel);

                                }

                            }
                        }
                     
                        #endregion
                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                   
                    }
                    //else if (rbtnSendUTF8.Checked)
                    //{
                    //    sendData = Encoding.UTF8.GetBytes(txtSendData.Text.Trim());
                    //}
                    //else if (rbtnSendUnicode.Checked)
                    //{
                    //    sendData = Encoding.Unicode.GetBytes(txtSendData.Text.Trim());
                    //}
                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richTextBox1.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richTextBox1.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }


                #region 方法1
                //if (!string.IsNullOrEmpty(richTextBox1.Text))
                //   {
                //       //Var_Atten_1 = Convert.ToString(richTextBox1.Text);

                //       //// Var_Atten_1 = Var_Atten_1.ToString("X6");
                //       //serialPort1.Write("\r");
                //       //serialPort1.Write(Var_Atten_1);
                //       //All_S_Bytes += Convert.ToString(Var_Atten_1).Length;
                //       //flag = 1;
                //   }
                //   else
                //   {
                //       MessageBox.Show("有数据为空！请完整填写数据");
                //   }


                /*****
                if (flag == 1 && serialPort1.IsOpen)
                {

                    // 十六进制重构 
                    HexSerialPort1(0XEE);  //帧头

                    HexSerialPort1(0X11);  //衰减控制标志

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_1)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_2)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_3)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_4)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_5)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_6)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_7)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_8)));

                    HexSerialPort1(0XFF);  //帧尾

                  ***/


                //All_S_Bytes += 1;

                //label81.Text = Convert.ToString(All_S_Bytes);
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";
                //}
                #endregion



            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }

        }

        /******  串口接收程序 ******/
        /// <summary>
        /// 串口接收数据
        /// czb 20220726
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

      
            #region 方法二
            //UTF8Encoding unicode = new UTF8Encoding();
            //Byte[] readBytes = new Byte[serialPort1.BytesToRead];
            //int SDataTemp = this.serialPort1.Read(readBytes, 0, readBytes.Length);
            //All_R_Bytes += SDataTemp;

            //this.richTextBox2.Invoke
            //(
            //    new MethodInvoker
            //    (
            //        delegate
            //        {
            //            string returnStr = "";
            //            for (int i = 0; i < SDataTemp; i++)
            //            {
            //                returnStr += readBytes[i].ToString("X2") + " ";
            //            }
            //            int cs = 1;
            //            bool chen = cs > 0;
            //            if (chen)
            //            {
            //                //Encoding.GetEncoding("utf-8")
            //                //  this.richTextBox2.AppendText(Encoding.GetEncoding("utf-8").GetString(readBytes));
            //                //  this.richTextBox2.AppendText(Encoding.GetEncoding("gb2312").GetString(readBytes));
            //                this.richTextBox2.AppendText(unicode.GetString(readBytes));
            //            }
            //            else
            //            {
            //                this.richTextBox2.AppendText(unicode.GetString(readBytes));
            //            }

            //            richTextBox2.SelectionStart = richTextBox2.TextLength;
            //            label86.Text = Convert.ToString(All_R_Bytes);
            //        }
            //    )
            //);
            #endregion

            #region 方法三

            try
            {
                if (rdojsHex.Checked)
                {
                    if (serialPort1.IsOpen)
                    {
                        byte[] ReDatas = new byte[serialPort1.BytesToRead];
                        serialPort1.Read(ReDatas, 0, ReDatas.Length);//读取数据
                                                                     //int SDataTemp = this.serialPort1.Read(ReDatas, 0, ReDatas.Length);
                                                                     //All_R_Bytes += SDataTemp;
                        this.AddData(ReDatas);//输出数据
                                              //this.richTextBox2.Invoke(new MethodInvoker(
                                              //    delegate {
                                              //        label86.Text = Convert.ToString(All_R_Bytes);
                                              //    }
                                              //    ));

                    }
                    else
                    {
                        MessageBox.Show("请先打开串口");
                    }
                }
                else if (rdotxtjs.Checked)
                {
                    UTF8Encoding unicode = new UTF8Encoding();
                    Byte[] readBytes = new Byte[serialPort1.BytesToRead];
                    int SDataTemp = this.serialPort1.Read(readBytes, 0, readBytes.Length);
                    All_R_Bytes += SDataTemp;

                    this.richTextBox2.Invoke
                    (
                        new MethodInvoker
                        (
                            delegate
                            {
                                string returnStr = "";
                                for (int i = 0; i < SDataTemp; i++)
                                {
                                    returnStr += readBytes[i].ToString("X2") + " ";
                                }
                                int cs = 1;
                                bool chen = cs > 0;
                                if (chen)
                                {
                                  string AcceptInfo1 = Encoding.GetEncoding("gb2312").GetString(readBytes);
                                    string AcceptInfo = Encoding.UTF8.GetString(readBytes);//改为国标utf8  czb 20220801
                                    //Encoding.GetEncoding("utf-8")
                                    //  this.richTextBox2.AppendText(Encoding.GetEncoding("utf-8").GetString(readBytes));
                                    if (!hj212.Crc16(AcceptInfo) || !hj212.DecodeData(AcceptInfo)) //未通过验证或解码失败
                                    {
                                        return; //不解码
                                    }

                                    string CN = hj212.DATA_HEAD["CN"];          //数据类型
                                    string MN = hj212.DATA_HEAD["MN"];          //设备MN
                                                                                //string DATATIME = hj212.CP["DATATIME"];     //上报时间
                                                                           
                                    //string sql = string.Empty;
                                    StringBuilder stringBuilder = new StringBuilder();
                                    //上传实时污染物数据
                                    if (CN == "2011")
                                    {
                                       
                                        //w01018-SampleTime 采样时间

                                        //w21001 总氮
                                        if (AcceptInfo.Contains("w21001"))
                                        {
                                            var SampleTime = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                            if (hj212.CP.ContainsKey("w21001-SampleTime".ToUpper()))
                                            {
                                                var w01018SampleTime = "w21001-SampleTime".ToUpper();
                                                var dateSample = hj212.CP[w01018SampleTime];

                                                if (dateSample != null && dateSample.Length>=14)
                                                {
                                                    SampleTime = Convert.ToDateTime(dateSample.Substring(0, 4) + "-" + dateSample.Substring(4, 2) + "-" + dateSample.Substring(6, 2) + " " + dateSample.Substring(8, 2) + ":" + dateSample.Substring(10, 2) + ":" + dateSample.Substring(12, 2));
                                                }
                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            var dataVal = 0.0M;//采集数据值
                                            if (hj212.CP.ContainsKey("21001-Rtd".ToUpper()))
                                            {
                                                dataVal = Convert.ToDecimal(hj212.CP["21001-Rtd".ToUpper()]);//采集数据值
                                            }

                                            var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                            if (hj212.CP.ContainsKey("DATATIME"))
                                            {
                                                var DATATIMES = hj212.CP["DATATIME"];
                                                if (DATATIMES != null && DATATIMES.Length >= 14)
                                                {
                                                    DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                }
                                            }
                                            long MeasureID = System.Math.Abs(snowflake.GetId());
                                            stringBuilder.Append(string.Format(@"INSERT INTO MeasureDataInfo ([ID] ,EquipmentCode                                                
                                                               ,[EquipmentName]
                                                               ,[DataType]
                                                               ,[CollectTime]  ,[MeasureTime],CollectValue,AisleName)  VALUES  ({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}')", MeasureID, "ZongDan", "总氮分析仪", "总氮", SampleTime, DATATIME, dataVal,"0") + ";");
                                        }
                                        //氨氮 w21003
                                        if (AcceptInfo.Contains("w21003"))
                                        {
                                            var SampleTime = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                            if (hj212.CP.ContainsKey("w21003-SampleTime".ToUpper()))
                                            {
                                                var w01018SampleTime = "w21003-SampleTime".ToUpper();
                                                var dateSample = hj212.CP[w01018SampleTime];

                                                if (dateSample != null && dateSample.Length>=14)
                                                {
                                                    SampleTime = Convert.ToDateTime(dateSample.Substring(0, 4) + "-" + dateSample.Substring(4, 2) + "-" + dateSample.Substring(6, 2) + " " + dateSample.Substring(8, 2) + ":" + dateSample.Substring(10, 2) + ":" + dateSample.Substring(12, 2));
                                                }
                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            var dataVal = 0.0M;//采集数据值
                                            if (hj212.CP.ContainsKey("21003-Rtd".ToUpper()))
                                            {
                                                dataVal = Convert.ToDecimal(hj212.CP["21003-Rtd".ToUpper()]);//采集数据值
                                            }

                                            var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                            if (hj212.CP.ContainsKey("DATATIME"))
                                            {
                                                var DATATIMES = hj212.CP["DATATIME"];
                                                if (DATATIMES != null && DATATIMES.Length >= 14)
                                                {
                                                    DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                }
                                            }
                                            long MeasureID = System.Math.Abs(snowflake.GetId());
                                            stringBuilder.Append(string.Format(@"INSERT INTO MeasureDataInfo ([ID] ,EquipmentCode                                                
                                                               ,[EquipmentName]
                                                               ,[DataType]
                                                               ,[CollectTime]  ,[MeasureTime],CollectValue,AisleName)  VALUES  ({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}')", MeasureID, "AnDan", "氨氮分析仪", "氨氮", SampleTime, DATATIME, dataVal,"0") + ";");
                                        }
                                        //总磷 w21003
                                        if (AcceptInfo.Contains("w21011"))
                                        {
                                            var SampleTime = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                            if (hj212.CP.ContainsKey("w21011-SampleTime".ToUpper()))
                                            {
                                                var w01018SampleTime = "w21011-SampleTime".ToUpper();
                                                var dateSample = hj212.CP[w01018SampleTime];

                                                if (dateSample != null && dateSample.Length>=14)
                                                {
                                                    SampleTime = Convert.ToDateTime(dateSample.Substring(0, 4) + "-" + dateSample.Substring(4, 2) + "-" + dateSample.Substring(6, 2) + " " + dateSample.Substring(8, 2) + ":" + dateSample.Substring(10, 2) + ":" + dateSample.Substring(12, 2));
                                                }
                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            var dataVal = 0.0M;//采集数据值
                                            if (hj212.CP.ContainsKey("21011-Rtd".ToUpper()))
                                            {
                                                dataVal = Convert.ToDecimal(hj212.CP["21011-Rtd".ToUpper()]);//采集数据值
                                            }

                                            var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                            if (hj212.CP.ContainsKey("DATATIME"))
                                            {
                                                var DATATIMES = hj212.CP["DATATIME"];
                                                if (DATATIMES != null && DATATIMES.Length >= 14)
                                                {
                                                    DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                }
                                                //DATATIME = Convert.ToDateTime(hj212.CP["DATATIME"]);     //测量时间
                                            }
                                            long MeasureID = System.Math.Abs(snowflake.GetId());
                                            stringBuilder.Append(string.Format(@"INSERT INTO MeasureDataInfo ([ID] ,EquipmentCode                                                
                                                               ,[EquipmentName]
                                                               ,[DataType]
                                                               ,[CollectTime]  ,[MeasureTime],CollectValue,AisleName)  VALUES  ({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}')", MeasureID, "zonglin", "总磷分析仪", "总磷", SampleTime, DATATIME, dataVal,"0") + ";");
                                        }
                                        //化学需氧量 w01018
                                        if (AcceptInfo.Contains("w01018"))
                                        {
                                            var SampleTime = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                            if (hj212.CP.ContainsKey("w01018-SampleTime".ToUpper()))
                                            {
                                               var w01018SampleTime = "w01018-SampleTime".ToUpper();
                                                var dateSample = hj212.CP[w01018SampleTime];
                                               
                                                if (dateSample!=null && dateSample.Length>=14)
                                                {
                                                    SampleTime = Convert.ToDateTime(dateSample.Substring(0, 4) + "-" + dateSample.Substring(4, 2) + "-" + dateSample.Substring(6, 2) + " "+dateSample.Substring(8, 2) + ":" + dateSample.Substring(10, 2) + ":" + dateSample.Substring(12, 2) );
                                                }
                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            var dataVal = 0.0M;//采集数据值
                                            if (hj212.CP.ContainsKey("01018-Rtd".ToUpper()))
                                            {
                                                dataVal = Convert.ToDecimal(hj212.CP["01018-Rtd".ToUpper()]);//采集数据值
                                            }
                                           
                                            var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                            if (hj212.CP.ContainsKey("DATATIME"))
                                            {
                                                var DATATIMES = hj212.CP["DATATIME"];
                                                if (DATATIMES!=null && DATATIMES.Length>=14)
                                                {
                                                    DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                }
                                            
                                                //DATATIME = Convert.ToDateTime(hj212.CP["DATATIME"]);     //测量时间
                                            }
                                            long MeasureID = System.Math.Abs(snowflake.GetId());
                                            stringBuilder.Append(string.Format(@"INSERT INTO MeasureDataInfo ([ID] ,EquipmentCode                                                
                                                               ,[EquipmentName]
                                                               ,[DataType]
                                                               ,[CollectTime]  ,[MeasureTime],CollectValue,AisleName)  VALUES  ({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}')", MeasureID,"huaxue", "需氧量分析仪", "化学需氧量", SampleTime, DATATIME, dataVal,"0")+";");
                                        }
                                        long id = System.Math.Abs(snowflake.GetId());
                                        var ResourceCode = "检测指标实时数据";
                                        logHelper.WriteLogInfo("检测指标实时数据" + AcceptInfo, version);
                                        //检测指标实时数据
                                        stringBuilder.Append( string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark]) VALUES  ({0},'{1}','{2}','{3}','{4}')", id, ResourceCode, DateTime.Now.ToString(), AcceptInfo, "数据分析仪器上传数据"));
                                    }
                                  // 取设备运行状态数据
                                   if (CN == "2021")
                                    {
                                        //QN=20160801085857223;ST=32;CN=2021;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& DataTime = 20160801085857; SB1 - RS = 1; SB2 - RS = 0…&&
                                        var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                        if (hj212.CP.ContainsKey("DATATIME"))
                                        {
                                            var DATATIMES = hj212.CP["DATATIME"];
                                            if (DATATIMES != null && DATATIMES.Length >= 14)
                                            {
                                                DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                            }
                                            var EquipmentStatus1 =-2;//获取设备运行状态
                                            if (hj212.CP.ContainsKey("SB1-RS".ToUpper()))
                                            {
                                                EquipmentStatus1 = Convert.ToInt32(hj212.CP["SB1-RS".ToUpper()]);//采集数据值
                                            }
                                            var EquipmentStatus2 = -2;//获取设备运行状态
                                            if (hj212.CP.ContainsKey("SB2-RS".ToUpper()))
                                            {
                                                EquipmentStatus2 = Convert.ToInt32(hj212.CP["SB2-RS".ToUpper()]);//采集数据值
                                            }
                                            //DATATIME = Convert.ToDateTime(hj212.CP["DATATIME"]);     //测量时间
                                            long MeasureID = System.Math.Abs(snowflake.GetId());
                                            stringBuilder.Append(string.Format(@"INSERT INTO MeasureDataInfo ([ID] ,EquipmentCode                                                
                                                               ,[EquipmentName]
                                                               ,[DataType]
                                                               ,[MeasureTime],CollectValue,AisleName)  VALUES  ({0},'{1}','{2}','{3}','{4}',{5},'{6}')", MeasureID, "huaxue", "需氧量分析仪", "化学需氧量设备状态", DATATIME, EquipmentStatus1, "0") + ";");
                                            var ResourceCode = "检测指标实时数据";
                                            logHelper.WriteLogInfo("检测指标实时数据" + AcceptInfo, version);
                                            //取设备运行状态数据
                                            stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark]) VALUES  ({0},'{1}','{2}','{3}','{4}')", MeasureID, ResourceCode, DateTime.Now.ToString(), AcceptInfo, "数据分析仪器上传设备状态数据"));

                                        }

                                    }
                                    // 提取运行设备的参数
                                    if (CN == "")
                                    {
                                        //QN=20160801085857223;ST=32;CN=2021;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& DataTime = 20160801085857; SB1 - RS = 1; SB2 - RS = 0…&&
                                        var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                        if (hj212.CP.ContainsKey("DATATIME"))
                                        {
                                            var DATATIMES = hj212.CP["DATATIME"];
                                            if (DATATIMES != null && DATATIMES.Length >= 14)
                                            {
                                                DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                            }
                                            var EquipmentStatus1 = -2;//获取设备运行状态
                                            if (hj212.CP.ContainsKey("SB1-RS".ToUpper()))
                                            {
                                                EquipmentStatus1 = Convert.ToInt32(hj212.CP["SB1-RS".ToUpper()]);//采集数据值
                                            }
                                            var EquipmentStatus2 = -2;//获取设备运行状态
                                            if (hj212.CP.ContainsKey("SB2-RS".ToUpper()))
                                            {
                                                EquipmentStatus2 = Convert.ToInt32(hj212.CP["SB2-RS".ToUpper()]);//采集数据值
                                            }
                                            //DATATIME = Convert.ToDateTime(hj212.CP["DATATIME"]);     //测量时间
                                            long MeasureID = System.Math.Abs(snowflake.GetId());
                                            stringBuilder.Append(string.Format(@"INSERT INTO MeasureDataInfo ([ID] ,EquipmentCode                                                
                                                               ,[EquipmentName]
                                                               ,[DataType]
                                                               ,[MeasureTime],CollectValue,AisleName)  VALUES  ({0},'{1}','{2}','{3}','{4}',{5},'{6}')", MeasureID, "huaxue", "需氧量分析仪", "化学需氧量设备状态", DATATIME, EquipmentStatus1, "0") + ";");
                                            var ResourceCode = "检测指标实时数据";
                                            logHelper.WriteLogInfo("检测指标实时数据" + AcceptInfo, version);
                                            //取设备运行状态数据
                                            stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark]) VALUES  ({0},'{1}','{2}','{3}','{4}')", MeasureID, ResourceCode, DateTime.Now.ToString(), AcceptInfo, "数据分析仪器上传设备状态数据"));

                                        }

                                    }
                                    // 设置运行设备的参数
                                    if (CN == "3021")
                                    {
                                        //QN=20160801085857223;ST=32;CN=2021;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& DataTime = 20160801085857; SB1 - RS = 1; SB2 - RS = 0…&&
                                        var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                        if (hj212.CP.ContainsKey("DATATIME"))
                                        {
                                            var DATATIMES = hj212.CP["DATATIME"];
                                            if (DATATIMES != null && DATATIMES.Length >= 14)
                                            {
                                                DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                            }
                                            var EquipmentStatus1 = -2;//获取设备运行状态
                                            if (hj212.CP.ContainsKey("SB1-RS".ToUpper()))
                                            {
                                                EquipmentStatus1 = Convert.ToInt32(hj212.CP["SB1-RS".ToUpper()]);//采集数据值
                                            }
                                            var EquipmentStatus2 = -2;//获取设备运行状态
                                            if (hj212.CP.ContainsKey("SB2-RS".ToUpper()))
                                            {
                                                EquipmentStatus2 = Convert.ToInt32(hj212.CP["SB2-RS".ToUpper()]);//采集数据值
                                            }
                                            //DATATIME = Convert.ToDateTime(hj212.CP["DATATIME"]);     //测量时间
                                            long MeasureID = System.Math.Abs(snowflake.GetId());
                                            stringBuilder.Append(string.Format(@"INSERT INTO MeasureDataInfo ([ID] ,EquipmentCode                                                
                                                               ,[EquipmentName]
                                                               ,[DataType]
                                                               ,[MeasureTime],CollectValue,AisleName)  VALUES  ({0},'{1}','{2}','{3}','{4}',{5},'{6}')", MeasureID, "huaxue", "需氧量分析仪", "化学需氧量设备状态", DATATIME, EquipmentStatus1, "0") + ";");
                                            var ResourceCode = "检测指标实时数据";
                                            logHelper.WriteLogInfo("检测指标实时数据" + AcceptInfo, version);
                                            //取设备运行状态数据
                                            stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID]      ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark]) VALUES  ({0},'{1}','{2}','{3}','{4}')", MeasureID, ResourceCode, DateTime.Now.ToString(), AcceptInfo, "数据分析仪器上传设备状态数据"));

                                        }

                                    }

                                    #region 获取检测设备运行日志 czb 20220801
                                    //上传实时污染物数据
                                    if (CN == "3020")
                                    {

                                        //PolId=w01018 污染物编码
                                        //i11001-Info =//清洗管路// 日志内容
                                        //w21001 总氮
                                        if (AcceptInfo.Contains("w21001"))
                                        {

                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21001")//化学需氧量
                                            {
                                                var logContent = string.Empty;//日志信息
                                                if (hj212.CP.ContainsKey("i11001-Info".ToUpper()))
                                                {
                                                    logContent = Convert.ToString(hj212.CP["i11001-Info".ToUpper()]);//日志信息
                                                }

                                                var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                                if (hj212.CP.ContainsKey("DATATIME"))
                                                {
                                                    var DATATIMES = hj212.CP["DATATIME"];
                                                    if (DATATIMES != null && DATATIMES.Length >= 14)
                                                    {
                                                        DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                    }
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "总氮检测仪上传日志信息";//日志来源
                                                //获取设备日志信息
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DATATIME, logContent, "总氮检测仪上传日志信息", "表 C.40上传现场机信息(日志)"));
                                            }


                                        }
                                        //氨氮 w21003
                                        if (AcceptInfo.Contains("w21003"))
                                        {
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21003")//化学需氧量
                                            {
                                                var logContent = string.Empty;//日志信息
                                                if (hj212.CP.ContainsKey("i11001-Info".ToUpper()))
                                                {
                                                    logContent = Convert.ToString(hj212.CP["i11001-Info".ToUpper()]);//日志信息
                                                }

                                                var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                                if (hj212.CP.ContainsKey("DATATIME"))
                                                {
                                                    var DATATIMES = hj212.CP["DATATIME"];
                                                    if (DATATIMES != null && DATATIMES.Length >= 14)
                                                    {
                                                        DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                    }
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "氨氮检测仪上传日志信息";//日志来源
                                                //获取设备日志信息
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DATATIME, logContent, "氨氮检测仪上传日志信息", "表 C.40上传现场机信息(日志)"));
                                            }
                                        }
                                        //总磷 w21003
                                        if (AcceptInfo.Contains("w21011"))
                                        {
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21011")//化学需氧量
                                            {
                                                var logContent = string.Empty;//日志信息
                                                if (hj212.CP.ContainsKey("i11001-Info".ToUpper()))
                                                {
                                                    logContent = Convert.ToString(hj212.CP["i11001-Info".ToUpper()]);//日志信息
                                                }

                                                var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                                if (hj212.CP.ContainsKey("DATATIME"))
                                                {
                                                    var DATATIMES = hj212.CP["DATATIME"];
                                                    if (DATATIMES != null && DATATIMES.Length >= 14)
                                                    {
                                                        DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                    }
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "总磷检测仪上传日志信息";//日志来源
                                                //获取设备日志信息
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DATATIME, logContent, "总磷检测仪上传日志信息", "表 C.40上传现场机信息(日志)"));
                                            }
                                        }
                                        //化学需氧量 w01018
                                        //PolId=w01018 污染物编码
                                        //i11001-Info =//清洗管路// 日志内容
                                        //w21001 总氮
                                        if (AcceptInfo.Contains("w01018"))
                                        {
                                      
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W01018")//化学需氧量
                                            {
                                                var logContent = string.Empty;//日志信息
                                                if (hj212.CP.ContainsKey("i11001-Info".ToUpper()))
                                                {
                                                    logContent = Convert.ToString(hj212.CP["i11001-Info".ToUpper()]);//日志信息
                                                }

                                                var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                                if (hj212.CP.ContainsKey("DATATIME"))
                                                {
                                                    var DATATIMES = hj212.CP["DATATIME"];
                                                    if (DATATIMES != null && DATATIMES.Length >= 14)
                                                    {
                                                        DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                    }
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "化学需氧量检测仪上传日志信息";//日志来源
                                                //获取设备日志信息
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DATATIME, logContent, "化学需氧量检测仪上传日志信息", "表 C.40上传现场机信息(日志)"));
                                            }


                                        }
                                    }
                                    #endregion

                                    #region 获取现场时间校准 czb 20220801
                                    //QN=20160801085857223;ST=32;CN=1013;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& PolId = w01018 &&
                                    //获取设备校准时间
                                    if (CN == "1013")
                                    {

                                        //PolId=w01018 污染物编码
                                        //i11001-Info =//清洗管路// 日志内容
                                        //w21001 总氮
                                        if (AcceptInfo.Contains("w21001"))
                                        {

                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21001")//化学需氧量
                                            {
                                                var logContent = string.Empty;//日志信息
                                                if (hj212.CP.ContainsKey("i11001-Info".ToUpper()))
                                                {
                                                    logContent = Convert.ToString(hj212.CP["i11001-Info".ToUpper()]);//日志信息
                                                }

                                                var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                                if (hj212.CP.ContainsKey("DATATIME"))
                                                {
                                                    var DATATIMES = hj212.CP["DATATIME"];
                                                    if (DATATIMES != null && DATATIMES.Length >= 14)
                                                    {
                                                        DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                    }
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "总氮检测仪上传日志信息";//日志来源
                                                //获取设备日志信息
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DATATIME, logContent, "总氮检测仪上传日志信息", "表 C.40上传现场机信息(日志)"));
                                            }


                                        }
                                        //氨氮 w21003
                                        if (AcceptInfo.Contains("w21003"))
                                        {
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21003")//化学需氧量
                                            {
                                                var logContent = string.Empty;//日志信息
                                                if (hj212.CP.ContainsKey("i11001-Info".ToUpper()))
                                                {
                                                    logContent = Convert.ToString(hj212.CP["i11001-Info".ToUpper()]);//日志信息
                                                }

                                                var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                                if (hj212.CP.ContainsKey("DATATIME"))
                                                {
                                                    var DATATIMES = hj212.CP["DATATIME"];
                                                    if (DATATIMES != null && DATATIMES.Length >= 14)
                                                    {
                                                        DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                    }
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "氨氮检测仪上传日志信息";//日志来源
                                                //获取设备日志信息
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DATATIME, logContent, "氨氮检测仪上传日志信息", "表 C.40上传现场机信息(日志)"));
                                            }
                                        }
                                        //总磷 w21003
                                        if (AcceptInfo.Contains("w21011"))
                                        {
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21011")//化学需氧量
                                            {
                                                var logContent = string.Empty;//日志信息
                                                if (hj212.CP.ContainsKey("i11001-Info".ToUpper()))
                                                {
                                                    logContent = Convert.ToString(hj212.CP["i11001-Info".ToUpper()]);//日志信息
                                                }

                                                var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                                if (hj212.CP.ContainsKey("DATATIME"))
                                                {
                                                    var DATATIMES = hj212.CP["DATATIME"];
                                                    if (DATATIMES != null && DATATIMES.Length >= 14)
                                                    {
                                                        DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                    }
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "总磷检测仪上传日志信息";//日志来源
                                                //获取设备日志信息
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DATATIME, logContent, "总磷检测仪上传日志信息", "表 C.40上传现场机信息(日志)"));
                                            }
                                        }
                                        //化学需氧量 w01018
                                        //PolId=w01018 污染物编码
                                        //i11001-Info =//清洗管路// 日志内容
                                        //w21001 总氮
                                        if (AcceptInfo.Contains("w01018"))
                                        {

                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W01018")//化学需氧量
                                            {
                                                var logContent = string.Empty;//日志信息
                                                if (hj212.CP.ContainsKey("i11001-Info".ToUpper()))
                                                {
                                                    logContent = Convert.ToString(hj212.CP["i11001-Info".ToUpper()]);//日志信息
                                                }

                                                var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                                if (hj212.CP.ContainsKey("DATATIME"))
                                                {
                                                    var DATATIMES = hj212.CP["DATATIME"];
                                                    if (DATATIMES != null && DATATIMES.Length >= 14)
                                                    {
                                                        DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                    }
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "化学需氧量检测仪上传日志信息";//日志来源
                                                //获取设备日志信息
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DATATIME, logContent, "化学需氧量检测仪上传日志信息", "表 C.40上传现场机信息(日志)"));
                                            }


                                        }
                                    }
                                    #endregion
                                    #region 提取超标留样 czb 20220801
                                    //QN=20160801085857223;ST=32;CN=3015;PW=123456;MN=010000A8900016F000169DC0; Flag = 5; CP =&& DataTime = 20160801085857; VaseNo = 1 &&
                          
                                    if (CN == "3015")
                                    {

                                     

                                            var VaseNoVal = string.Empty;
                                            if (hj212.CP.ContainsKey("VaseNo".ToUpper()))
                                            {
                                                string VaseNo = "VaseNo".ToUpper(); //污染物编码
                                            VaseNoVal = hj212.CP[VaseNo];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                       
                                                var logContent = AcceptInfo;//日志信息
                                              

                                                var DATATIME = Convert.ToDateTime("1900-01-01");//w01018-SampleTime //cai采样时间
                                                if (hj212.CP.ContainsKey("DATATIME"))
                                                {
                                                    var DATATIMES = hj212.CP["DATATIME"];//留样时间
                                                    if (DATATIMES != null && DATATIMES.Length >= 14)
                                                    {
                                                        DATATIME = Convert.ToDateTime(DATATIMES.Substring(0, 4) + "-" + DATATIMES.Substring(4, 2) + "-" + DATATIMES.Substring(6, 2) + " " + DATATIMES.Substring(8, 2) + ":" + DATATIMES.Substring(10, 2) + ":" + DATATIMES.Substring(12, 2));
                                                    }
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "检测仪上传超标留样";//日志来源
                                                //获取设备日志信息
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType,[Remark1]) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DATATIME, logContent, "现场机超标留样", "表 C.34 超标留样", VaseNoVal));
                                         


                                    
                                      
                                      
                                      
                                    }
                                    #endregion
                                    #region 表 C.36 提取采样时间周期 czb 20220802
                                    //上传实时污染物数据
                                    if (CN == "3017")
                                    {

                                        //PolId=w01018 污染物编码
                                        //i11001-Info =//清洗管路// 日志内容
                                        //w21001 总氮
                                        if (AcceptInfo.Contains("w21001"))
                                        {

                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21001")//总氮
                                            {
                                                var CstartTime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("CstartTime".ToUpper()))
                                                {
                                                    CstartTime = Convert.ToString(hj212.CP["CstartTime".ToUpper()]);//采样开始时间
                                                }

                                                var CTime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("CTime".ToUpper()))
                                                {
                                                    CTime = Convert.ToString(hj212.CP["CTime".ToUpper()]);//采样周期
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "总氮检测仪提取采样时间周期";//日志来源 CstartTime+";"+ CTime
                                                //提取采样时间周期
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), AcceptInfo, CstartTime + ";" + CTime, "表 C.36 提取采样时间周期"));
                                            }


                                        }
                                        //氨氮 w21003
                                        if (AcceptInfo.Contains("w21003"))
                                        {
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21003")//氨氮
                                            {
                                                var CstartTime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("CstartTime".ToUpper()))
                                                {
                                                    CstartTime = Convert.ToString(hj212.CP["CstartTime".ToUpper()]);//采样开始时间
                                                }

                                                var CTime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("CTime".ToUpper()))
                                                {
                                                    CTime = Convert.ToString(hj212.CP["CTime".ToUpper()]);//采样周期
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "氨氮检测仪提取采样时间周期";//日志来源
                                                //提取采样时间周期
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), AcceptInfo, CstartTime + ";" + CTime, "表 C.36 提取采样时间周期"));
                                            }
                                        }
                                        //总磷 w21003
                                        if (AcceptInfo.Contains("w21011"))
                                        {
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21011")//总磷
                                            {
                                                var CstartTime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("CstartTime".ToUpper()))
                                                {
                                                    CstartTime = Convert.ToString(hj212.CP["CstartTime".ToUpper()]);//采样开始时间
                                                }

                                                var CTime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("CTime".ToUpper()))
                                                {
                                                    CTime = Convert.ToString(hj212.CP["CTime".ToUpper()]);//采样周期
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "总磷检测仪提取采样时间周期";//日志来源
                                                //提取采样时间周期
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), AcceptInfo, CstartTime + ";" + CTime, "表 C.36 提取采样时间周期"));
                                            }
                                        }
                                        //化学需氧量 w01018
                                        //PolId=w01018 污染物编码
                                        //i11001-Info =//清洗管路// 日志内容
                                    
                                        if (AcceptInfo.Contains("w01018"))
                                        {

                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W01018")//化学需氧量
                                            {
                                                var CstartTime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("CstartTime".ToUpper()))
                                                {
                                                    CstartTime = Convert.ToString(hj212.CP["CstartTime".ToUpper()]);//采样开始时间
                                                }

                                                var CTime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("CTime".ToUpper()))
                                                {
                                                    CTime = Convert.ToString(hj212.CP["CTime".ToUpper()]);//采样周期
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "化学需氧量检测仪提取采样时间周期";//日志来源
                                                //提取采样时间周期
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), AcceptInfo, CstartTime+";"+ CTime, "表 C.36 提取采样时间周期"));
                                            }


                                        }
                                    }
                                    #endregion

                                    #region 表 C.30 零点校准量程校准 czb 20220802
                                    //零点校准
                                    if (CN == "3011")
                                    {

                                       
                                    }
                                    #endregion

                                    #region 表 C.37 提取出样时间 czb 20220802                                  
                                    if (CN == "3018")
                                    {
                                        //QN=20160801085857223;ST=32;CN=3018;PW=123456;MN=010000A8900016F000169DC0; Flag = 4; CP =&& PolId = w01018; Stime = 40 &&
                                        //PolId=w01018 污染物编码
                                        //i11001-Info =//清洗管路// 日志内容
                                        //w21001 总氮
                                        if (AcceptInfo.Contains("w21001"))
                                        {
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21001")//总氮
                                            {
                                                

                                                var Stime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("Stime".ToUpper()))
                                                {
                                                    Stime = Convert.ToString(hj212.CP["Stime".ToUpper()]);//采样周期
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "总氮检测仪提取出样时间";//日志来源 CstartTime+";"+ CTime
                                                //提取采样时间周期
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), AcceptInfo,  Stime, "表 C.37 提取出样时间"));
                                            }
                                        }
                                        //氨氮 w21003
                                        if (AcceptInfo.Contains("w21003"))
                                        {
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];
                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21003")//总氮
                                            {


                                                var Stime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("Stime".ToUpper()))
                                                {
                                                    Stime = Convert.ToString(hj212.CP["Stime".ToUpper()]);//采样周期
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "氨氮检测仪提取出样时间";//日志来源 CstartTime+";"+ CTime
                                                //提取采样时间周期
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), AcceptInfo, Stime, "表 C.37 提取出样时间"));
                                            }
                                        }
                                        //总磷 w21003
                                        if (AcceptInfo.Contains("w21011"))
                                        {
                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];
                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W21011")//总氮
                                            {


                                                var Stime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("Stime".ToUpper()))
                                                {
                                                    Stime = Convert.ToString(hj212.CP["Stime".ToUpper()]);//采样周期
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "总磷检测仪提取出样时间";//日志来源 CstartTime+";"+ CTime
                                                //提取采样时间周期
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), AcceptInfo, Stime, "表 C.37 提取出样时间"));
                                            }
                                        }
                                        //化学需氧量 w01018
                                        //PolId=w01018 污染物编码
                                        //i11001-Info =//清洗管路// 日志内容

                                        if (AcceptInfo.Contains("w01018"))
                                        {

                                            var PolIdVal = string.Empty;
                                            if (hj212.CP.ContainsKey("PolId".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                                PolIdVal = hj212.CP[PolId];
                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            if (PolIdVal.ToUpper() == "W01018")//总氮
                                            {


                                                var Stime = string.Empty;//采样开始时间
                                                if (hj212.CP.ContainsKey("Stime".ToUpper()))
                                                {
                                                    Stime = Convert.ToString(hj212.CP["Stime".ToUpper()]);//采样周期
                                                }
                                                long logID = System.Math.Abs(snowflake.GetId());
                                                string ResourceLog = "化学需氧量检测仪提取出样时间";//日志来源 CstartTime+";"+ CTime
                                                //提取采样时间周期
                                                stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), AcceptInfo, Stime, "表 C.37 提取出样时间"));
                                            }


                                        }
                                    }
                                    #endregion
                                    #region 获取执行结果 czb 20220801
                                    //QN=20160801085857223;ST=91;CN=9012;PW=123456;MN=010000A8900016F000169DC0; Flag = 4; CP =&& ExeRtn = 1 &&

                                    if (CN == "9011")
                                    {

                                        //QnRtn=1 请求应答结果

                                        var QnRtnVal = string.Empty;
                                            if (hj212.CP.ContainsKey("QnRtn".ToUpper()))
                                            {
                                                string PolId = "PolId".ToUpper(); //污染物编码
                                            QnRtnVal = hj212.CP[PolId];


                                                //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                            }
                                            

                                        long logID = System.Math.Abs(snowflake.GetId());
                                        string ResourceLog = "现场机 返回请求应答 ";//日志来源
                                                                           //获取设备日志信息
                                        stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), QnRtnVal, "检测仪回应", "表 C.40上传现场机信息(日志)"));



                                    }

                                    if (CN == "9012")
                                    {

                                        //ExeRtn=1 请求执行结果

                                        var ExeRtnVal = string.Empty;
                                        if (hj212.CP.ContainsKey("ExeRtn".ToUpper()))
                                        {
                                            string ExeRtn = "ExeRtn".ToUpper(); //污染物编码
                                            ExeRtnVal = hj212.CP[ExeRtn];


                                            //SampleTime = Convert.ToDateTime(hj212.CP[w01018SampleTime]);//采集数据shijian时间
                                        }


                                        long logID = System.Math.Abs(snowflake.GetId());
                                        string ResourceLog = "现场机 返回执行结果";//日志来源
                                                                           //获取设备日志信息
                                        stringBuilder.Append(string.Format(@"INSERT INTO RunAllLogInfo ([ID] ,LogSource                                                
                                                               ,[ReceivingTime]
                                                               ,[LogContent]
                                                               ,[Remark],LogType) VALUES  ({0},'{1}','{2}','{3}','{4}','{5}')", logID, ResourceLog, DateTime.Now.ToString(), ExeRtnVal, "检测仪回应", "表 C.40上传现场机信息(日志)"));



                                    }
                                    #endregion
                                    if (stringBuilder.ToString().Length>0)
                                    {
                                        //执行数据库语句
                                        using (SqlCommand com = new SqlCommand(stringBuilder.ToString(), con))
                                        {
                                            try
                                            {
                                                con.Open();
                                                //   MessageBox.Show("成功连接数据库");
                                                int count = com.ExecuteNonQuery();
                                                logHelper.WriteLogInfo("成功执行{" + count + "}条记录", version);
                                                //  int x = (int)com.ExecuteScalar条记录();
                                                //  MessageBox.Show(string.Format("成功读取{0},条记录", x));
                                            }
                                            catch (Exception ex)
                                            {
                                                logHelper.WriteLogInfo(ex.Message, version);
                                                throw ex;

                                            }
                                            finally
                                            {
                                                con.Close();
                                                //  MessageBox.Show("成功关闭数据库连接", "提示信息", MessageBoxButtons.YesNoCancel);

                                            }

                                        }
                                    }
                                  
                                    richTextBox2.AppendText("服务端 " + GetCurrentTime() + "\r\n" + AcceptInfo + "\r\n" + "MN：" + MN + "\r\n" + "CN：" + CN);
                                    //this.richTextBox2.AppendText(AcceptInfo);


                                    //  this.richTextBox2.AppendText(unicode.GetString(readBytes));
                                }
                                else
                                {
                                    this.richTextBox2.AppendText(unicode.GetString(readBytes));
                                }

                                richTextBox2.SelectionStart = richTextBox2.TextLength;
                                label86.Text = Convert.ToString(All_R_Bytes);
                            }
                        )
                    );
                }
                else
                {
                    if (serialPort1.IsOpen)
                    {
                        byte[] ReDatas = new byte[serialPort1.BytesToRead];
                        serialPort1.Read(ReDatas, 0, ReDatas.Length);//读取数据
                                                                     //int SDataTemp = this.serialPort1.Read(ReDatas, 0, ReDatas.Length);
                                                                     //All_R_Bytes += SDataTemp;
                        this.AddData(ReDatas);//输出数据
                                              //this.richTextBox2.Invoke(new MethodInvoker(
                                              //    delegate {
                                              //        label86.Text = Convert.ToString(All_R_Bytes);
                                              //    }
                                              //    ));

                    }
                    else
                    {
                        MessageBox.Show("请先打开串口");
                    }
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);
                MessageBox.Show(ex.Message);
            }



            #endregion



        }


        //清空串口接收栏数据
        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = null;
            label86.Text = "0";
            label81.Text = "0";
        }
        /// <summary>
        /// 文本框只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //阻止从键盘输入键
            // e.Handled = true;

            //if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
            //{

            //    if ((e.KeyChar == (char)8)) { e.Handled = false; return; }
            //    else
            //    {
            //        int len = richTextBox1.Text.Length;
            //        if (len < 15)
            //        {
            //            if (len == 0 && e.KeyChar != '0')
            //            {
            //                e.Handled = false; return;
            //            }
            //            else if (len == 0)
            //            {
            //                MessageBox.Show("编号不能以0开头！"); return;
            //            }
            //            e.Handled = false; return;
            //        }
            //        else
            //        {
            //            MessageBox.Show("编号最多只能输入5位数字！");
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("编号只能输入数字！");
            //   // this.richTextBox1.Text = "";
            //}
        }


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data"></param>
        public void AddData(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.AppendFormat("{0:x2}" + " ", data[i]);
            }
            AddContent(sb.ToString().ToUpper());
            //if (radioButton1.Checked)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    for (int i = 0; i < data.Length; i++)
            //    {
            //        sb.AppendFormat("{0:x2}" + " ", data[i]);
            //    }
            //    AddContent(sb.ToString().ToUpper());
            //}
            //else if (radioButton4.Checked)
            //{
            //    AddContent(new ASCIIEncoding().GetString(data));
            //}
            //else if (radioButton3.Checked)
            //{
            //    AddContent(new UTF8Encoding().GetString(data));
            //}
            //else if (radioButton2.Checked)
            //{
            //    AddContent(new UnicodeEncoding().GetString(data));
            //}
            //else
            //{ }
            //this.richTextBox2.Invoke(new MethodInvoker(
            //    delegate {
            //        label86.Text = Convert.ToString(All_R_Bytes);
            //    }
            //    ));

            richTextBox2.Invoke(new MethodInvoker(delegate
            {
                label86.Text = (data.Length).ToString();
            }));
        }

        /// <summary>
        /// 输入到显示区域
        /// </summary>
        /// <param name="content"></param>
        private void AddContent(string content)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                //if (chkAutoLine.Checked && txtShowData.Text.Length > 0)
                //{
                //    txtShowData.AppendText("\r\n");
                //}
                richTextBox2.Text = "";
                if (richTextBox2.Text.Length > 0)
                {
                    richTextBox2.AppendText("\r\n");
                }
                var contentArray = content.Split(' ');
                //if (contentArray.Length<10)//烟感仪器
                //{

                //    if (contentArray != null)
                //    {
                //        if (contentArray[4] == "01")
                //        {
                //            content += "报警";
                //        }
                //        else
                //        {
                //            content += "正常";
                //        }
                //    }
                //}
                //else if (contentArray.Length==10)//温度湿度仪器
                //{
                //    if (contentArray != null)
                //    {
                //        var shidu = contentArray[3] + contentArray[4];
                //        var shiduWD =  To16Convert10(shidu);
                //        var wendu = contentArray[5] + contentArray[6];
                //        var tempWD = To16Convert10(wendu);
                //        double wenduValue = Math.Round(((double)tempWD / 10), 2);

                //        double shiduValue = Math.Round(((double)shiduWD / 10), 2);
                //        content +="温度"+ wenduValue.ToString();
                //        content +="湿度"+ shiduValue.ToString();
                //    }
                //}
                //  if (contentArray.Length == 8)//水浸传感器
                //{
                //    if (contentArray != null)
                //    {
                //        if (contentArray[4] == "01")
                //        {
                //            content += "浸水";
                //        }
                //        else
                //        {
                //            content += "正常";
                //        }
                //    }
                //}

                if (contentArray.Length >= 23)//仪表五参数 
                {
                    //：01 03 04 C2 8F 41 01 07 F0（01是设备地址，03是功能码，04是返回字节 个数，C2 8F 41 01是第一个因子值8.11，07 F0是校验码）
                    if (contentArray[0] == "01" && contentArray[1] == "03")
                    {
                        var pressureVal = contentArray[17] + contentArray[18] + contentArray[15] + contentArray[16];//光学溶解氧
                        var shiduPressureValue = Convert.ToDouble(get16tofloat(pressureVal));
                        if (shiduPressureValue < 0)
                        {
                            shiduPressureValue = 0;
                        }
                        var wenduVal = contentArray[5] + contentArray[6] + contentArray[3] + contentArray[4];//温度
                        var wenduValue = Convert.ToDouble(Math.Round(get16tofloat(wenduVal), 3));
                        if (wenduValue < 0)
                        {
                            wenduValue = 0;
                        }

                        var phVal = contentArray[9] + contentArray[10] + contentArray[7] + contentArray[8];//ph
                        var phValue = Convert.ToDouble(Math.Round(get16tofloat(phVal), 3));
                        if (phValue < 0)
                        {
                            phValue = 0;
                        }

                        var ddlVal = contentArray[13] + contentArray[14] + contentArray[11] + contentArray[12];//导电率
                        var ddlValue = Convert.ToDouble(Math.Round(get16tofloat(ddlVal), 3));
                        if (ddlValue < 0)
                        {
                            ddlValue = 0;
                        }
                        var zhuoduVal = contentArray[21] + contentArray[22] + contentArray[19] + contentArray[20];//浊度
                        var zhuoduValue = Convert.ToDouble(Math.Round(get16tofloat(zhuoduVal), 3));
                        if (zhuoduValue < 0)
                        {
                            zhuoduValue = 0;

                        }
                        content += "温度" + wenduValue.ToString() + "度" + "；PH" + phValue.ToString() + "" + "；导电率" + ddlValue.ToString() + "" + "；光学溶解氧" + shiduPressureValue.ToString() + "mg/L" + "；光学溶解氧" + zhuoduValue.ToString() + "NTU";
                    }
                }
                //if (contentArray.Length == 8)//压力变送器
                //{
                //    if (contentArray != null)
                //    {
                //           var pressureVal = contentArray[3] + contentArray[4];
                //           var shiduPressureValue =  To16Convert10(pressureVal);
                //           content += "压力" + shiduPressureValue.ToString() + "Kpa";
                //    }
                //}
                //else
                //{

                //}



                richTextBox2.AppendText(content);
            }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// czb 20220517
        /// 字符串转换16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private byte[] strToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Replace(" ", ""), 16);
            return returnBytes;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public bool SendData(byte[] data)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Write(data, 0, data.Length);//发送数据
                    //serialPort1.Write(richTextBox1.Text);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }





        /// <summary>
        /// 光标显示最后一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.SelectionStart = richTextBox2.TextLength;
            richTextBox2.ScrollToCaret();
        }
        /// <summary>
        /// 串口按钮初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInitialize_Click(object sender, EventArgs e)
        {

            version = this.txtVersion.Text;
        }
        /// <summary>
        /// 修改版号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateVersion_Click(object sender, EventArgs e)
        {
            var versionValue = this.txtVersion.Text;
            var isNum = Regex.IsMatch(versionValue, "^[0-9]*$");
            if (isNum)
            {
                this.txtVersion.Text = (Convert.ToInt32(versionValue) + 1).ToString();

            }
            else
            {
                MessageBox.Show("您输入的不是数字，不能自动生成");
            }
            version = this.txtVersion.Text;
        }

        #region 十六进制字符串转十进制
        /// <summary>
        /// 十六进制字符串转十进制
        /// </summary>
        /// <param name="str">十六进制字符</param>
        /// <returns></returns>
        private int To16Convert10(string str)
        {
            int res = 0;

            try
            {
                str = str.Trim().Replace(" ", "");//移除空字符
                //方法1
                res = int.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier);
                //  方法2
                //int r2 = System.Int32.Parse(str, System.Globalization.NumberStyles.HexNumber);
                //Console.WriteLine(r2);
                //  方法3
                //int r3 = Convert.ToInt32(str, 16);
                //Console.WriteLine(r3);

            }
            catch (Exception e)
            {
                res = 0;
            }

            return res;

        }
        /// <summary>
        /// 16 jin进制转浮点型
        /// </summary>
        /// <returns></returns>
        private float get16tofloat(string s)
        {
            float res = 0.00f;

            try
            {
                //   s = "40E51161";
                MatchCollection matches = Regex.Matches(s, @"[0-9A-Fa-f]{2}");
                byte[] bytes = new byte[matches.Count];
                for (int i = 0; i < bytes.Length; i++)
                    bytes[i] = byte.Parse(matches[i].Value, System.Globalization.NumberStyles.AllowHexSpecifier);
                res = BitConverter.ToSingle(bytes.Reverse().ToArray(), 0);

            }
            catch (Exception e)
            {
                res = 0.00f;
            }

            return res;
        }
        #endregion

        private void uiButton1_Click(object sender, EventArgs e)
        {
            //// 从这里开始复制
            //string KJLJ = Application.ExecutablePath;
            //if (!System.IO.File.Exists(KJLJ))//判断指定文件是否存在
            //    return;
            //string newKJLJ = KJLJ.Substring(KJLJ.LastIndexOf("\\") + 1);
            //RegistryKey Rkey =
            //    Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //if (Rkey == null)
            //    Rkey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            //Rkey.SetValue(newKJLJ, KJLJ);
            //MessageBox.Show("程序设置完毕,请重新启动计算机后即可生效！", "程序员的爬行温馨提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ////到这里结束，试一试，无需修改，即可，呵呵，不过你也要努力学习啊
            ///
            try
            {
                string regPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
                string path = Application.ExecutablePath.ToLower(); //将当前程序起动路径
                MessageBox.Show(path);
                string name = Application.ExecutablePath; //获得应用程序名称
                MessageBox.Show(name);
                //var mreg = Registry.CurrentUser;
                var regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regPath, true);
                if (regKey == null) regKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(regPath);
                regKey.SetValue(name, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        /// <summary>
        /// 16位crc校验码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CRC16(byte[] data)
        {
            ushort crc = 0xFFFF;
            int len = data.Length;
            for (int i = 0; i < len; i++)
            {
                crc = (ushort)((crc >> 8) ^ data[i]);
                for (int j = 0; j < 8; j++)
                    crc = (crc & 1) == 1 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
            }
            return string.Format("{0:X}", crc).PadLeft(4, '0');
        }

        /// <summary>
        /// Modbus CRC16计算
        /// </summary>
        /// <param name="hexCommand">十六进制字符串命令（如 "01 03 00 03 00 02"）</param>
        /// <returns>带CRC的完整命令字符串</returns>
        private string CalcCRC16(string hexCommand)
        {
            byte[] pBuf = HexStringToByteArray(hexCommand);
            uint i, j;
            uint crc16 = 0xFFFF;
            for (i = 0; i < pBuf.Length; i++)
            {
                crc16 ^= pBuf[i];
                for (j = 0; j < 8; j++)
                {
                    if ((crc16 & 0x0001) != 0)
                    {
                        crc16 >>= 1; // 右移一位
                        crc16 ^= 0xA001; // 异或多项式0xA001
                    }
                    else
                        crc16 >>= 1; // 右移一位
                }
            }
            string crcDefLow1 = Convert.ToString((byte)(crc16 & 0xFF), 16).PadLeft(2, '0');
            string crcDefHigh1 = Convert.ToString((byte)(crc16 >> 8), 16).PadLeft(2, '0');
            return hexCommand + " " + crcDefLow1 + " " + crcDefHigh1;
        }

        /// <summary>
        /// 十六进制字符串转字节数组（用于Modbus命令）
        /// </summary>
        /// <param name="hexString">十六进制字符串</param>
        /// <returns>字节数组</returns>
        private byte[] HexStringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "").Replace("-", "").ToUpper().Trim();
            if (hexString.Length % 2 != 0)
                hexString += "0";
            int bytesCount = hexString.Length / 2;
            byte[] bytes = new byte[bytesCount];
            for (int i = 0; i < bytesCount; i++)
            {
                string byteString = hexString.Substring(i * 2, 2);
                bytes[i] = Convert.ToByte(byteString, 16);
            }
            return bytes;
        }

        private void btnsent2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                }
                //if (!Information.IsNumeric(textBox1.Text) | !Information.IsNumeric(textBox2.Text) | !Information.IsNumeric(textBox3.Text) | !Information.IsNumeric(textBox4.Text) | !Information.IsNumeric(textBox5.Text) | !Information.IsNumeric(textBox6.Text) | !Information.IsNumeric(textBox7.Text) | !Information.IsNumeric(textBox8.Text))
                //      MessageBox.Show("请输入完整设置！");
                // else

                #region 方法1
                //if (!string.IsNullOrEmpty(richTextBox1.Text))
                //   {
                //       //Var_Atten_1 = Convert.ToString(richTextBox1.Text);

                //       //// Var_Atten_1 = Var_Atten_1.ToString("X6");
                //       //serialPort1.Write("\r");
                //       //serialPort1.Write(Var_Atten_1);
                //       //All_S_Bytes += Convert.ToString(Var_Atten_1).Length;
                //       //flag = 1;
                //   }
                //   else
                //   {
                //       MessageBox.Show("有数据为空！请完整填写数据");
                //   }


                /*****
                if (flag == 1 && serialPort1.IsOpen)
                {

                    // 十六进制重构 
                    HexSerialPort1(0XEE);  //帧头

                    HexSerialPort1(0X11);  //衰减控制标志

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_1)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_2)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_3)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_4)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_5)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_6)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_7)));

                    HexSerialPort1S(Convert.ToString((int)(4.0 * Var_Atten_8)));

                    HexSerialPort1(0XFF);  //帧尾

                  ***/


                //All_S_Bytes += 1;

                //label81.Text = Convert.ToString(All_S_Bytes);
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";
                //}
                #endregion



                #region 发送数据hex
                //byte[] data = new byte[1];    //发送数据字符数组，每次发送一个字节的数据
                //string strs = richTextBox1.Text.Replace(" ", "").Replace("\r\n", "");     //剔除字符串中的空格和回车
                ////如果字符串的个数为单数，在最后一位前补0
                //if (strs.Length % 2 == 1)
                //{
                //    strs = strs.Insert(strs.Length - 1, "0");
                //}
                ////将字符串中两个字符组成一个8进制数，循环次数为字符串长度的一半
                //for (int i = 0; i < strs.Length / 2; i++)
                //{
                //    //将两位字符转换成一位16进制数
                //    data[0] = Convert.ToByte(strs.Substring(i * 2, 2), 16);
                //    //将发送数据字符数组的数据发送
                //    serialPort1.Write(data, 0, 1);
                //}
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";
                #endregion


                #region 发送hex数据方法er
                byte[] sendData = null;
                //sendData = strToHexByte(richTextBox1.Text.Trim());
                if (rdoHex.Checked)
                {
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    sendData = strToHexByte(richtxt2.Text);
                    //  this.SendData(sendData);
                }
                else if (rdoTxtName.Checked)
                {
                    string strLength = (string)richtxt2.Text.Trim().Length.ToString();
                    if (strLength.Length == 1)
                    {
                        strLength = "000" + strLength;
                    }
                    else if (strLength.Length == 2)
                    {
                        strLength = "00" + strLength;
                    }
                    else if (strLength.Length == 3)
                    {
                        strLength = "0" + strLength;
                    }
                    else
                    {
                        strLength = strLength;
                    }
                    string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt2.Text.Trim()));//获取crc验证码

                    //MessageBox.Show(crc);
                    var sendStr = "##" + strLength + richtxt2.Text.Trim() + crc + "\r\n";
                    //MessageBox.Show(sendStr);
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                    //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                    logHelper.WriteLogInfo(sendStr, version);
                    sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                }
                //else if (rbtnSendUTF8.Checked)
                //{
                //    sendData = Encoding.UTF8.GetBytes(txtSendData.Text.Trim());
                //}
                //else if (rbtnSendUnicode.Checked)
                //{
                //    sendData = Encoding.Unicode.GetBytes(txtSendData.Text.Trim());
                //}
                else
                {
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                    sendData = strToHexByte(richtxt2.Text);
                }


                if (this.SendData(sendData))
                {
                    label81.Invoke(new MethodInvoker(delegate
                    {
                        label81.Text = (richtxt2.Text.Length).ToString();
                        //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    }));

                }
                else
                {

                }
                //label81.Invoke(new MethodInvoker(delegate
                //    {
                //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                //    }));
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                #endregion
            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnSend3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                }
              

            

 


                #region 发送hex数据方法er
                byte[] sendData = null;
                //sendData = strToHexByte(richTextBox1.Text.Trim());
                if (rdoHex.Checked)
                {
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    sendData = strToHexByte(richtxtbox3.Text);
                    //  this.SendData(sendData);
                }
                else if (rdoTxtName.Checked)
                {
                    string strLength = (string)richtxtbox3.Text.Trim().Length.ToString();
                    if (strLength.Length == 1)
                    {
                        strLength = "000" + strLength;
                    }
                    else if (strLength.Length == 2)
                    {
                        strLength = "00" + strLength;
                    }
                    else if (strLength.Length == 3)
                    {
                        strLength = "0" + strLength;
                    }
                    else
                    {
                        strLength = strLength;
                    }
                    string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxtbox3.Text.Trim()));//获取crc验证码

                    //MessageBox.Show(crc);
                    var sendStr = "##" + strLength + richtxtbox3.Text.Trim() + crc + "\r\n";
                    //MessageBox.Show(sendStr);
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                    //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                    logHelper.WriteLogInfo(sendStr, version);
                    sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                }
         
                else
                {
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                    sendData = strToHexByte(richtxtbox3.Text);
                }


                if (this.SendData(sendData))
                {
                    label81.Invoke(new MethodInvoker(delegate
                    {
                        label81.Text = (richtxtbox3.Text.Length).ToString();
                        //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    }));

                }
                else
                {

                }
                //label81.Invoke(new MethodInvoker(delegate
                //    {
                //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                //    }));
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                #endregion
            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                }







                #region 发送hex数据方法er
                byte[] sendData = null;
                //sendData = strToHexByte(richTextBox1.Text.Trim());
                if (rdoHex.Checked)
                {
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    sendData = strToHexByte(richtxt4.Text);
                    //  this.SendData(sendData);
                }
                else if (rdoTxtName.Checked)
                {
                    string strLength = (string)richtxt4.Text.Trim().Length.ToString();
                    if (strLength.Length == 1)
                    {
                        strLength = "000" + strLength;
                    }
                    else if (strLength.Length == 2)
                    {
                        strLength = "00" + strLength;
                    }
                    else if (strLength.Length == 3)
                    {
                        strLength = "0" + strLength;
                    }
                    else
                    {
                        strLength = strLength;
                    }
                    string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt4.Text.Trim()));//获取crc验证码

                    //MessageBox.Show(crc);
                    var sendStr = "##" + strLength + richtxt4.Text.Trim() + crc + "\r\n";
                    //MessageBox.Show(sendStr);
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                    //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                    logHelper.WriteLogInfo(sendStr, version);
                    sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                }

                else
                {
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                    sendData = strToHexByte(richtxt4.Text);
                }


                if (this.SendData(sendData))
                {
                    label81.Invoke(new MethodInvoker(delegate
                    {
                        label81.Text = (richtxt4.Text.Length).ToString();
                        //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    }));

                }
                else
                {

                }
                //label81.Invoke(new MethodInvoker(delegate
                //    {
                //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                //    }));
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                #endregion
            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                }







                #region 发送hex数据方法er
                byte[] sendData = null;
                //sendData = strToHexByte(richTextBox1.Text.Trim());
                if (rdoHex.Checked)
                {
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    sendData = strToHexByte(richtxt4.Text);
                    //  this.SendData(sendData);
                }
                else if (rdoTxtName.Checked)
                {
                    string strLength = (string)richtxt5.Text.Trim().Length.ToString();
                    if (strLength.Length == 1)
                    {
                        strLength = "000" + strLength;
                    }
                    else if (strLength.Length == 2)
                    {
                        strLength = "00" + strLength;
                    }
                    else if (strLength.Length == 3)
                    {
                        strLength = "0" + strLength;
                    }
                    else
                    {
                        strLength = strLength;
                    }
                    string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt5.Text.Trim()));//获取crc验证码

                    //MessageBox.Show(crc);
                    var sendStr = "##" + strLength + richtxt5.Text.Trim() + crc + "\r\n";
                    //MessageBox.Show(sendStr);
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                    //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                    logHelper.WriteLogInfo(sendStr, version);
                    sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                }

                else
                {
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                    sendData = strToHexByte(richtxt5.Text);
                }


                if (this.SendData(sendData))
                {
                    label81.Invoke(new MethodInvoker(delegate
                    {
                        label81.Text = (richtxt5.Text.Length).ToString();
                        //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    }));

                }
                else
                {

                }
                //label81.Invoke(new MethodInvoker(delegate
                //    {
                //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                //    }));
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                #endregion
            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsent6_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                }







                #region 发送hex数据方法er
                byte[] sendData = null;
                //sendData = strToHexByte(richTextBox1.Text.Trim());
                if (rdoHex.Checked)
                {
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    sendData = strToHexByte(richtxt6.Text);
                    //  this.SendData(sendData);
                }
                else if (rdoTxtName.Checked)
                {
                    string strLength = (string)richtxt6.Text.Trim().Length.ToString();
                    if (strLength.Length == 1)
                    {
                        strLength = "000" + strLength;
                    }
                    else if (strLength.Length == 2)
                    {
                        strLength = "00" + strLength;
                    }
                    else if (strLength.Length == 3)
                    {
                        strLength = "0" + strLength;
                    }
                    else
                    {
                        strLength = strLength;
                    }
                    string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt6.Text.Trim()));//获取crc验证码

                    //MessageBox.Show(crc);
                    var sendStr = "##" + strLength + richtxt6.Text.Trim() + crc + "\r\n";
                    //MessageBox.Show(sendStr);
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                    //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                    logHelper.WriteLogInfo(sendStr, version);
                    sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                }

                else
                {
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                    sendData = strToHexByte(richtxt6.Text);
                }


                if (this.SendData(sendData))
                {
                    label81.Invoke(new MethodInvoker(delegate
                    {
                        label81.Text = (richtxt6.Text.Length).ToString();
                        //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    }));

                }
                else
                {

                }
                //label81.Invoke(new MethodInvoker(delegate
                //    {
                //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                //    }));
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                #endregion
            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend7_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                }







                #region 发送hex数据方法er
                byte[] sendData = null;
                //sendData = strToHexByte(richTextBox1.Text.Trim());
                if (rdoHex.Checked)
                {
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    sendData = strToHexByte(richtxt6.Text);
                    //  this.SendData(sendData);
                }
                else if (rdoTxtName.Checked)
                {
                    string strLength = (string)richtxt7.Text.Trim().Length.ToString();
                    if (strLength.Length == 1)
                    {
                        strLength = "000" + strLength;
                    }
                    else if (strLength.Length == 2)
                    {
                        strLength = "00" + strLength;
                    }
                    else if (strLength.Length == 3)
                    {
                        strLength = "0" + strLength;
                    }
                    else
                    {
                        strLength = strLength;
                    }
                    string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt7.Text.Trim()));//获取crc验证码

                    //MessageBox.Show(crc);
                    var sendStr = "##" + strLength + richtxt7.Text.Trim() + crc + "\r\n";
                    //MessageBox.Show(sendStr);
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                    //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                    logHelper.WriteLogInfo(sendStr, version);
                    sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                }

                else
                {
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                    sendData = strToHexByte(richtxt7.Text);
                }


                if (this.SendData(sendData))
                {
                    label81.Invoke(new MethodInvoker(delegate
                    {
                        label81.Text = (richtxt7.Text.Length).ToString();
                        //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    }));

                }
                else
                {

                }
                //label81.Invoke(new MethodInvoker(delegate
                //    {
                //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                //    }));
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                #endregion
            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend10_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                }







                #region 发送hex数据方法er
                byte[] sendData = null;
                //sendData = strToHexByte(richTextBox1.Text.Trim());
                if (rdoHex.Checked)
                {
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    sendData = strToHexByte(richtxt10.Text);
                    //  this.SendData(sendData);
                }
                else if (rdoTxtName.Checked)
                {
                    string strLength = (string)richtxt10.Text.Trim().Length.ToString();
                    if (strLength.Length == 1)
                    {
                        strLength = "000" + strLength;
                    }
                    else if (strLength.Length == 2)
                    {
                        strLength = "00" + strLength;
                    }
                    else if (strLength.Length == 3)
                    {
                        strLength = "0" + strLength;
                    }
                    else
                    {
                        strLength = strLength;
                    }
                    string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt10.Text.Trim()));//获取crc验证码

                    //MessageBox.Show(crc);
                    var sendStr = "##" + strLength + richtxt10.Text.Trim() + crc + "\r\n";
                    //MessageBox.Show(sendStr);
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                    //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                    logHelper.WriteLogInfo(sendStr, version);
                    sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                }

                else
                {
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                    sendData = strToHexByte(richtxt10.Text);
                }


                if (this.SendData(sendData))
                {
                    label81.Invoke(new MethodInvoker(delegate
                    {
                        label81.Text = (richtxt10.Text.Length).ToString();
                        //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    }));

                }
                else
                {

                }
                //label81.Invoke(new MethodInvoker(delegate
                //    {
                //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                //    }));
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                #endregion
            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnSend9_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                }







                #region 发送hex数据方法er
                byte[] sendData = null;
                //sendData = strToHexByte(richTextBox1.Text.Trim());
                if (rdoHex.Checked)
                {
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    sendData = strToHexByte(richtxt9.Text);
                    //  this.SendData(sendData);
                }
                else if (rdoTxtName.Checked)
                {
                    string strLength = (string)richtxt9.Text.Trim().Length.ToString();
                    if (strLength.Length == 1)
                    {
                        strLength = "000" + strLength;
                    }
                    else if (strLength.Length == 2)
                    {
                        strLength = "00" + strLength;
                    }
                    else if (strLength.Length == 3)
                    {
                        strLength = "0" + strLength;
                    }
                    else
                    {
                        strLength = strLength;
                    }
                    string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt9.Text.Trim()));//获取crc验证码

                    //MessageBox.Show(crc);
                    var sendStr = "##" + strLength + richtxt9.Text.Trim() + crc + "\r\n";
                    //MessageBox.Show(sendStr);
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                    //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                    logHelper.WriteLogInfo(sendStr, version);
                    sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                }

                else
                {
                    //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                    sendData = strToHexByte(richtxt9.Text);
                }


                if (this.SendData(sendData))
                {
                    label81.Invoke(new MethodInvoker(delegate
                    {
                        label81.Text = (richtxt9.Text.Length).ToString();
                        //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    }));

                }
                else
                {

                }
                //label81.Invoke(new MethodInvoker(delegate
                //    {
                //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                //    }));
                //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                #endregion
            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend8_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt8.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt8.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt8.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt8.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt8.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt8.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsent11_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt11.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt11.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt11.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt11.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt11.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt8.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsent12_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt12.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt12.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt12.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt12.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt12.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt12.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend13_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt13.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt13.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt13.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt13.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt13.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt13.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend14_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt14.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt14.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt14.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt14.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt14.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt14.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend15_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt15.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt15.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt15.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt15.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt15.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt15.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend16_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt16.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt16.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt16.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt16.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt16.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt16.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend17_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt17.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt17.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt17.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt17.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt17.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt17.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }

        }

        private void btnsend18_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt18.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt18.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt18.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt18.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt18.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt18.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend19_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt19.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt19.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt19.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt19.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt19.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt19.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend20_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt20.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt20.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt20.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt20.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt20.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt20.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend21_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt21.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt21.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt21.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt21.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt21.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt21.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend22_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt22.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt22.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt22.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt22.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt22.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt22.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend23_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt23.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt23.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt23.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt23.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt23.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt23.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend24_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt24.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt24.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt24.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt24.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt24.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt24.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend25_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt25.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt25.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt25.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt25.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt25.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt25.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend26_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt26.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt26.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt26.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt26.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt26.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt26.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend27_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt27.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt27.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt27.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt27.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt27.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt27.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend28_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt28.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt28.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt28.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt28.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt28.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt28.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend29_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt29.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt29.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt29.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt29.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt29.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt29.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend30_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt30.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt30.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt30.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt30.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt30.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt30.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend31_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt31.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt31.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt31.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt31.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt31.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt31.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend32_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt32.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt32.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt32.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt32.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt32.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt32.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }

        }

        private void btnsend33_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt33.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt33.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt33.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt33.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt33.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt33.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend34_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt34.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt34.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt34.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt34.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt34.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt34.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend35_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt35.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt35.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt35.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt35.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt35.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt35.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsent36_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt36.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt36.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt36.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt36.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt36.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt36.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend37_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt37.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt37.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt37.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt37.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt37.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt37.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend38_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt38.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt38.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt38.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt38.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt38.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt38.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend39_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt39.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt39.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt39.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt39.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt39.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt39.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }

        private void btnsend40_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    logHelper.WriteLogInfo("请先打开串口！", version);
                    MessageBox.Show("请先打开串口！");
                }
                else
                {
                    #region 发送hex数据方法er
                    byte[] sendData = null;
                    //sendData = strToHexByte(richTextBox1.Text.Trim());
                    if (rdoHex.Checked)
                    {
                        //sendData = strToHexByte(richTextBox1.Text.Trim());
                        sendData = strToHexByte(richtxt40.Text);
                        //  this.SendData(sendData);
                    }
                    else if (rdoTxtName.Checked)
                    {
                        string strLength = (string)richtxt40.Text.Trim().Length.ToString();
                        if (strLength.Length == 1)
                        {
                            strLength = "000" + strLength;
                        }
                        else if (strLength.Length == 2)
                        {
                            strLength = "00" + strLength;
                        }
                        else if (strLength.Length == 3)
                        {
                            strLength = "0" + strLength;
                        }
                        else
                        {
                            strLength = strLength;
                        }
                        string crc = CRC16(System.Text.Encoding.UTF8.GetBytes(richtxt40.Text.Trim()));//获取crc验证码

                        //MessageBox.Show(crc);
                        var sendStr = "##" + strLength + richtxt40.Text.Trim() + crc + "\r\n";
                        //MessageBox.Show(sendStr);
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());
                        //sendData = Encoding.UTF8.GetBytes(richTextBox1.Text.Trim());//20220629 注释

                        logHelper.WriteLogInfo(sendStr, version);
                        sendData = Encoding.UTF8.GetBytes(sendStr);//20220629 注释
                    }

                    else
                    {
                        //sendData = Encoding.ASCII.GetBytes(richTextBox1.Text.Trim());

                        sendData = strToHexByte(richtxt40.Text);
                    }


                    if (this.SendData(sendData))
                    {
                        label81.Invoke(new MethodInvoker(delegate
                        {
                            label81.Text = (richtxt40.Text.Length).ToString();
                            //label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                        }));

                    }
                    else
                    {

                    }
                    //label81.Invoke(new MethodInvoker(delegate
                    //    {
                    //        label81.Text = (int.Parse(label81.Text) + richTextBox1.Text.Length).ToString();
                    //    }));
                    //richTextBox2.Text = richTextBox2.Text + "发送成功 " + DateTime.Now.ToString("HH:mm:ss") + "\r\n";//20220629 czb 注释

                    #endregion
                }

            }
            catch (Exception ex)
            {
                logHelper.WriteLogInfo(ex.Message, version);

            }
        }
        /// <summary>
        /// 环境hj212 协议
        /// czb 20220719
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public static string GetBackCmd(string Mode)
        {
            string strHead = "##";
            string strlen = "";
            string strMode = Mode;
            strlen = strMode.Length.ToString("0000");
            strMode = strHead + strlen + strMode + HJ212Mode.Crc_16(strMode) + "\r\n";
            return strMode;
        }
        /// <summary>
        /// 获取当前系统时间的方法
        /// </summary>
        /// <returns>当前时间</returns>
        private DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }
    }
}
