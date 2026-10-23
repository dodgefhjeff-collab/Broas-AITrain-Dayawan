using System;
using System.Linq;
using WaterStation.Client.Model.Commom;

namespace YBTimeCalibrationTest
{
    /// <summary>
    /// 仪表时间校准逻辑单测（不连串口）。
    /// </summary>
    internal static class Program
    {
        private static int _passed;
        private static int _failed;

        private static int Main()
        {
            Console.WriteLine("=== 仪表时间校准逻辑测试 ===");
            Console.WriteLine();

            TestShouldAttemptToday();
            TestFactorFilter();
            TestCommandBody();
            TestLogText();
            TestDailyFlowNoInstrument();
            TestDailyFlowOnlyConfigured();

            Console.WriteLine();
            Console.WriteLine($"结果: 通过 {_passed}, 失败 {_failed}");
            return _failed == 0 ? 0 : 1;
        }

        private static void TestShouldAttemptToday()
        {
            var today = new DateTime(2026, 8, 13);
            var yesterday = today.AddDays(-1);

            AssertTrue("从未校准过 → 应执行", YBTimeCalibrationLogic.ShouldAttemptToday(DateTime.MinValue, today));
            AssertTrue("昨天校准过 → 今天应执行", YBTimeCalibrationLogic.ShouldAttemptToday(yesterday, today));
            AssertFalse("今天已尝试 → 不再执行", YBTimeCalibrationLogic.ShouldAttemptToday(today, today));
            AssertFalse("今天任意时刻已尝试 → 不再执行", YBTimeCalibrationLogic.ShouldAttemptToday(today.AddHours(10), today.AddHours(15)));
        }

        private static void TestFactorFilter()
        {
            AssertEqual("空配置 → 0 台", 0, YBTimeCalibrationLogic.GetEnabledAnalyzers(null).Count);
            AssertEqual("空字符串 → 0 台", 0, YBTimeCalibrationLogic.GetEnabledAnalyzers("").Count);
            AssertEqual("无仪表名 → 0 台", 0, YBTimeCalibrationLogic.GetEnabledAnalyzers("五参,流量").Count);

            var onlyAd = YBTimeCalibrationLogic.GetEnabledAnalyzers("氨氮");
            AssertEqual("仅氨氮 → 1 台", 1, onlyAd.Count);
            AssertEqual("仅氨氮站号", "02", onlyAd[0].Station);
            AssertEqual("仅氨氮名称", "氨氮", onlyAd[0].Name);

            var all = YBTimeCalibrationLogic.GetEnabledAnalyzers("总氮,氨氮,COD,总磷");
            AssertEqual("四因子全开 → 4 台", 4, all.Count);
            AssertTrue("含总氮", all.Any(x => x.Factor == "ZD"));
            AssertTrue("含氨氮", all.Any(x => x.Factor == "AD"));
            AssertTrue("含COD", all.Any(x => x.Factor == "COD"));
            AssertTrue("含总磷", all.Any(x => x.Factor == "ZL"));

            AssertFalse("ZD 未配置", YBTimeCalibrationLogic.IsFactorEnabled("ZD", "氨氮"));
            AssertTrue("AD 已配置", YBTimeCalibrationLogic.IsFactorEnabled("AD", "氨氮"));
        }

        private static void TestCommandBody()
        {
            var now = new DateTime(2026, 8, 13, 16, 45, 30);
            string cmd = YBTimeCalibrationLogic.BuildCommandBody("02", now);
            AssertEqual("氨氮指令正文", "02 10 12 00 00 04 08 00 0d 260813164530", cmd);

            string cmdZd = YBTimeCalibrationLogic.BuildCommandBody("01", now);
            AssertEqual("总氮站号", "01 10 12 00 00 04 08 00 0d 260813164530", cmdZd);
        }

        private static void TestLogText()
        {
            AssertEqual("手动日志", "氨氮时间校准", YBTimeCalibrationLogic.BuildLogText("氨氮", false));
            AssertEqual("定时日志", "氨氮时间校准（定时）", YBTimeCalibrationLogic.BuildLogText("氨氮", true));
        }

        /// <summary>
        /// 模拟：无配置仪表 → 不发指令，但当天标记已处理。
        /// </summary>
        private static void TestDailyFlowNoInstrument()
        {
            var today = new DateTime(2026, 8, 13);
            DateTime last = DateTime.MinValue;
            int sendCount = 0;

            if (YBTimeCalibrationLogic.ShouldAttemptToday(last, today))
            {
                var enabled = YBTimeCalibrationLogic.GetEnabledAnalyzers("");
                if (enabled.Count == 0)
                {
                    last = today.Date;
                }
                else
                {
                    sendCount = enabled.Count;
                    last = today.Date;
                }
            }

            AssertEqual("无仪表不发送", 0, sendCount);
            AssertFalse("无仪表也标记当天完成", YBTimeCalibrationLogic.ShouldAttemptToday(last, today));
        }

        /// <summary>
        /// 模拟：仅配置氨氮 → 只发 1 台；当天第二次不再发。
        /// </summary>
        private static void TestDailyFlowOnlyConfigured()
        {
            var today = new DateTime(2026, 8, 13, 9, 0, 0);
            DateTime last = DateTime.MinValue;
            int sendCount = 0;

            void TryOnce()
            {
                if (!YBTimeCalibrationLogic.ShouldAttemptToday(last, today))
                    return;

                var enabled = YBTimeCalibrationLogic.GetEnabledAnalyzers("氨氮");
                if (enabled.Count == 0)
                {
                    last = today.Date;
                    return;
                }

                foreach (var item in enabled)
                {
                    _ = YBTimeCalibrationLogic.BuildCommandBody(item.Station, today);
                    sendCount++;
                }

                last = today.Date;
            }

            TryOnce();
            AssertEqual("首次只发氨氮 1 台", 1, sendCount);

            int beforeSecond = sendCount;
            TryOnce();
            AssertEqual("当天第二次不增加发送", beforeSecond, sendCount);
        }

        private static void AssertTrue(string name, bool actual)
        {
            if (actual)
            {
                Pass(name);
            }
            else
            {
                Fail(name, "期望 true，实际 false");
            }
        }

        private static void AssertFalse(string name, bool actual)
        {
            if (!actual)
            {
                Pass(name);
            }
            else
            {
                Fail(name, "期望 false，实际 true");
            }
        }

        private static void AssertEqual(string name, object expected, object actual)
        {
            if (Equals(expected, actual))
            {
                Pass(name);
            }
            else
            {
                Fail(name, $"期望 [{expected}]，实际 [{actual}]");
            }
        }

        private static void Pass(string name)
        {
            _passed++;
            Console.WriteLine($"[PASS] {name}");
        }

        private static void Fail(string name, string detail)
        {
            _failed++;
            Console.WriteLine($"[FAIL] {name} — {detail}");
        }
    }
}
