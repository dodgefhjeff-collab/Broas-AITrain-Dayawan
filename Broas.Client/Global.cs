using System;
using System.Collections.Generic;
using System.IO.Ports;
using NLog;
using WaterStation.Client.Model.UserDto;

namespace WaterStation.Client
{
    public static class Global
    {
        /// <summary>
        /// 全局日志记录器（用于程序级别的统一日志，如全局异常等）
        /// </summary>
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static UserInfoDto userInfoDto = null;

        public static List<FunctionInfo> functionInfoDtos = null;

        public static List<RoleFunctionInfo> roleFunctionInfoDtos =new();

        public static bool GetBitStatue(int num, int pos)
        {
            if ((num & (1 << pos)) > 0)
                return true;
            else
                return false;
        }

        public static int ReStart { get; set; } = 1;

        public static List<SerialPort> SerialPortList = null;
        public static decimal RecentValZD = 0;
        public static decimal RecentValAD = 0;
        public static decimal RecentValCOD = 0;
        public static decimal RecentValZL = 0;
        public static decimal RecentValXS = 0;
        public static decimal RecentValZhuodu = 0;
        public static decimal YBRecentValZD = 0;
        public static decimal YBRecentValAD = 0;
        public static decimal YBRecentValCOD = 0;
        public static decimal YBRecentValZL = 0;
        public static decimal YBRecentValXS = 0;
        public static decimal YBRecentValZhuodu = 0;
        public static decimal YSRecentValZD = 0;
        public static decimal YSRecentValAD = 0;
        public static decimal YSRecentValCOD = 0;
        public static decimal YSRecentValZL = 0;
        public static decimal YSRecentValXS = 0;
        public static decimal YSRecentValZhuodu = 0;
        public static string RecentValTimeZD = DateTime.Now.ToString("G");
        public static string RecentValTimeAD = DateTime.Now.ToString("G");
        public static string RecentValTimeCOD = DateTime.Now.ToString("G");
        public static string RecentValTimeZL = DateTime.Now.ToString("G");
        public static string RecentValTimeXS = DateTime.Now.ToString("G");
        private static readonly object _syncRoot = new();
        public static string EQNames = string.Empty;
        private static string _isAIAlarm = "";
        public static string IsAIAlarm
        {
            get
            {
                lock (_syncRoot)
                {
                    return _isAIAlarm;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _isAIAlarm = value;
                }
            }
        }

        /// <summary>
        /// AI 传感器串口通信异常（超时/发送失败/串口未开；不含 AI 业务报警）
        /// </summary>
        public static bool IsAICommError { get; set; }

        /// <summary>
        /// 仪表仪器串口通信异常（设备时钟心跳停滞、串口未开；不含仪表业务报警）
        /// </summary>
        public static bool IsYBCommError { get; set; }

        /// <summary>
        /// 仪表设备时钟（用于通信心跳检测）
        /// </summary>
        public static string YBDeviceTimeZD { get; set; } = string.Empty;
        public static string YBDeviceTimeAD { get; set; } = string.Empty;
        public static string YBDeviceTimeCOD { get; set; } = string.Empty;
        public static string YBDeviceTimeZL { get; set; } = string.Empty;

        /// <summary>
        /// 最近一次成功收到 AI 有效数据的时间
        /// </summary>
        public static DateTime LastAIReceiveTime { get; set; } = DateTime.Now;
    }
}
