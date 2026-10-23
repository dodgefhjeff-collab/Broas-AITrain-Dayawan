using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;

namespace TipGdiStressTest
{
    /// <summary>
    /// Tip/GDI 压测窗体：验证 UIMessageTip 在高频调用下不闪退，且 GDI 对象可回落。
    /// </summary>
    public sealed class MainForm : Form
    {
        private const int GR_GDIOBJECTS = 0;

        private readonly Label _lblGdi;
        private readonly Label _lblStatus;
        private readonly NumericUpDown _numBurst;
        private readonly NumericUpDown _numInterval;
        private readonly TextBox _txtLog;
        private readonly Button _btnBurst;
        private readonly Button _btnLoop;
        private readonly Button _btnStop;
        private readonly System.Windows.Forms.Timer _gdiTimer;

        private CancellationTokenSource _loopCts;
        private int _shown;
        private int _peakGdi;
        private int _baseGdi;

        public MainForm()
        {
            Text = "UIMessageTip GDI 压测工具";
            Width = 720;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;

            var tip = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 48,
                Padding = new Padding(8),
                Text = "验收：1) 压测中进程不退出  2) GDI 峰值后能回落接近基线  3) 超并发时 Tip 可被跳过但不崩溃"
            };

            _lblGdi = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 28,
                Padding = new Padding(8, 4, 8, 0),
                Font = new Font(Font.FontFamily, 11f, FontStyle.Bold),
                Text = "GDI: -"
            };

            _lblStatus = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 24,
                Padding = new Padding(8, 0, 8, 0),
                Text = "状态: 空闲"
            };

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(8, 4, 8, 4),
                WrapContents = false
            };

            panel.Controls.Add(new Label { Text = "突发次数", AutoSize = true, Margin = new Padding(0, 8, 4, 0) });
            _numBurst = new NumericUpDown { Minimum = 1, Maximum = 5000, Value = 200, Width = 80 };
            panel.Controls.Add(_numBurst);

            panel.Controls.Add(new Label { Text = "循环间隔ms", AutoSize = true, Margin = new Padding(12, 8, 4, 0) });
            _numInterval = new NumericUpDown { Minimum = 0, Maximum = 5000, Value = 20, Width = 80 };
            panel.Controls.Add(_numInterval);

            _btnBurst = new Button { Text = "突发压测", Width = 100, Height = 30 };
            _btnBurst.Click += async (_, __) => await RunBurstAsync();
            panel.Controls.Add(_btnBurst);

            _btnLoop = new Button { Text = "连续压测", Width = 100, Height = 30 };
            _btnLoop.Click += async (_, __) => await RunLoopAsync();
            panel.Controls.Add(_btnLoop);

            _btnStop = new Button { Text = "停止", Width = 80, Height = 30, Enabled = false };
            _btnStop.Click += (_, __) =>
            {
                _loopCts?.Cancel();
                AppendLog("已请求停止");
            };
            panel.Controls.Add(_btnStop);

            _txtLog = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Consolas", 9f)
            };

            Controls.Add(_txtLog);
            Controls.Add(panel);
            Controls.Add(_lblStatus);
            Controls.Add(_lblGdi);
            Controls.Add(tip);

            _gdiTimer = new System.Windows.Forms.Timer { Interval = 500 };
            _gdiTimer.Tick += (_, __) => RefreshGdi();
            _gdiTimer.Start();

            Load += (_, __) =>
            {
                _baseGdi = GetGdiCount();
                _peakGdi = _baseGdi;
                AppendLog($"基线 GDI={_baseGdi}，MaxConcurrentTips={UIMessageTip.MaxConcurrentTips}");
                RefreshGdi();
            };

            FormClosing += (_, __) =>
            {
                _loopCts?.Cancel();
                _gdiTimer.Stop();
            };
        }

        private async Task RunBurstAsync()
        {
            SetBusy(true);
            _shown = 0;
            var count = (int)_numBurst.Value;
            var interval = (int)_numInterval.Value;
            _lblStatus.Text = $"状态: 突发压测 {count} 次…";
            AppendLog($"开始突发压测 count={count}, interval={interval}ms");

            try
            {
                for (var i = 0; i < count; i++)
                {
                    FireOneTip(i);
                    if (interval > 0)
                    {
                        await Task.Delay(interval);
                    }
                    else if (i % 20 == 0)
                    {
                        await Task.Yield();
                    }
                }

                // 等待 Tip 显示周期结束，观察 GDI 回落
                AppendLog("突发结束，等待 3 秒观察 GDI 回落…");
                await Task.Delay(3000);
                var after = GetGdiCount();
                AppendLog($"结果: 发起={count}, 当前GDI={after}, 峰值GDI={_peakGdi}, 基线={_baseGdi}");
                AppendLog(Judge(after) ? "【通过】进程存活且 GDI 未失控上涨" : "【关注】GDI 仍明显高于基线，请再等几秒或检查泄漏");
                _lblStatus.Text = "状态: 突发压测完成（进程未退出即 P0 通过）";
            }
            catch (Exception ex)
            {
                AppendLog("异常: " + ex);
                _lblStatus.Text = "状态: 异常（见日志）";
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async Task RunLoopAsync()
        {
            _loopCts?.Cancel();
            _loopCts = new CancellationTokenSource();
            var token = _loopCts.Token;
            SetBusy(true);
            _shown = 0;
            var interval = (int)_numInterval.Value;
            _lblStatus.Text = "状态: 连续压测中…";
            AppendLog($"开始连续压测 interval={interval}ms，点「停止」结束");

            try
            {
                var i = 0;
                while (!token.IsCancellationRequested)
                {
                    FireOneTip(i++);
                    await Task.Delay(Math.Max(1, interval), token);
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                AppendLog("异常: " + ex);
            }

            AppendLog("连续压测停止，等待 3 秒观察 GDI 回落…");
            await Task.Delay(3000);
            // 已泄漏的句柄无法靠 GC 收回；此处仅促使托管包装回收，便于观察修复后是否回落
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            await Task.Delay(500);
            var after = GetGdiCount();
            AppendLog($"结果: 发起≈{_shown}, 当前GDI={after}, 峰值GDI={_peakGdi}, 基线={_baseGdi}");
            AppendLog(Judge(after) ? "【通过】进程存活且 GDI 可回落" : "【关注】GDI 仍偏高，继续观察是否缓慢回落");
            _lblStatus.Text = "状态: 已停止";
            SetBusy(false);
        }

        private void FireOneTip(int index)
        {
            _shown++;
            // 三种类型轮换，模拟业务刷屏
            switch (index % 3)
            {
                case 0:
                    UIMessageTip.ShowError($"压测错误 #{index} " + DateTime.Now.ToString("HH:mm:ss.fff"), 800, true);
                    break;
                case 1:
                    UIMessageTip.ShowWarning($"压测警告 #{index}", 800, true);
                    break;
                default:
                    UIMessageTip.ShowOk($"压测成功 #{index}", 800, true);
                    break;
            }
        }

        private bool Judge(int currentGdi)
        {
            // 停后应回到接近基线；允许少量波动（字体/缓存等）
            if (_peakGdi >= 9000)
            {
                return false;
            }

            // 相对基线超过 +80 视为未回落（此前 +1500 过宽，591 会被误判通过）
            return currentGdi <= _baseGdi + 80;
        }

        private void SetBusy(bool busy)
        {
            _btnBurst.Enabled = !busy;
            _btnLoop.Enabled = !busy;
            _btnStop.Enabled = busy;
            _numBurst.Enabled = !busy;
            _numInterval.Enabled = !busy;
        }

        private void RefreshGdi()
        {
            var gdi = GetGdiCount();
            if (gdi > _peakGdi)
            {
                _peakGdi = gdi;
            }

            _lblGdi.Text = $"GDI对象: {gdi}    峰值: {_peakGdi}    基线: {_baseGdi}    已发起Tip: {_shown}";
            _lblGdi.ForeColor = gdi > 8000 ? Color.Red : (gdi > 5000 ? Color.DarkOrange : Color.DarkGreen);
        }

        private void AppendLog(string message)
        {
            var line = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
            if (_txtLog.InvokeRequired)
            {
                _txtLog.BeginInvoke(new Action(() =>
                {
                    _txtLog.AppendText(line);
                }));
            }
            else
            {
                _txtLog.AppendText(line);
            }

            Debug.WriteLine(message);
        }

        private static int GetGdiCount()
        {
            return GetGuiResources(Process.GetCurrentProcess().Handle, GR_GDIOBJECTS);
        }

        [DllImport("user32.dll")]
        private static extern int GetGuiResources(IntPtr hProcess, int uiFlags);
    }
}
