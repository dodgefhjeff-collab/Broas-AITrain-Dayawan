using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterStation.Client.Model.Commom
{
    /// <summary>
    /// 仪表时间校准纯逻辑（可单测）：因子过滤、每天一次判定、指令正文。
    /// </summary>
    public static class YBTimeCalibrationLogic
    {
        public static readonly (string Station, string Name, string Factor)[] Analyzers =
        {
            ("01", "总氮", "ZD"),
            ("02", "氨氮", "AD"),
            ("03", "COD", "COD"),
            ("04", "总磷", "ZL")
        };

        /// <summary>
        /// 因子是否在设备名配置中。
        /// </summary>
        public static bool IsFactorEnabled(string factorKey, string eqNames)
        {
            if (string.IsNullOrEmpty(eqNames))
                return false;

            string name = factorKey switch
            {
                "ZD" => "总氮",
                "AD" => "氨氮",
                "ZL" => "总磷",
                "COD" => "COD",
                _ => null
            };
            return name != null && eqNames.Contains(name);
        }

        /// <summary>
        /// 返回当前配置下应下发时间校准的仪表列表。
        /// </summary>
        public static IReadOnlyList<(string Station, string Name, string Factor)> GetEnabledAnalyzers(string eqNames)
        {
            return Analyzers
                .Where(x => IsFactorEnabled(x.Factor, eqNames))
                .ToList();
        }

        /// <summary>
        /// 当天尚未尝试过则返回 true（不保证下发成功）。
        /// </summary>
        public static bool ShouldAttemptToday(DateTime lastAttemptDate, DateTime now)
        {
            return lastAttemptDate.Date != now.Date;
        }

        /// <summary>
        /// 组装时间校准指令正文（不含 CRC）。
        /// </summary>
        public static string BuildCommandBody(string station, DateTime now)
        {
            return string.Format("{0} 10 12 00 00 04 08 00 0d {1}",
                station, now.ToString("yyMMddHHmmss").Trim());
        }

        /// <summary>
        /// 操作日志文案。
        /// </summary>
        public static string BuildLogText(string analyzerName, bool isScheduled)
        {
            return isScheduled ? $"{analyzerName}时间校准（定时）" : $"{analyzerName}时间校准";
        }
    }
}
