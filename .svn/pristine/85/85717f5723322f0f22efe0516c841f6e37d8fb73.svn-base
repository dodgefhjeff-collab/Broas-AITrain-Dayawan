using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WaterStation.Client;

namespace Broas.Client
{
    static class Program
    {
        /// <summary>
        /// 单实例互斥体名称，用于防止程序多开
        /// </summary>
        private const string SingleInstanceMutexName = "Global\\Broas.Client.SingleInstance";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 全局异常处理：捕获 UI 线程、非 UI 线程及 Task 中未处理的异常，避免直接导致进程退出
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            bool createdNew;
            using (var mutex = new Mutex(true, SingleInstanceMutexName, out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("程序已在运行中，请勿重复启动。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                while (Global.ReStart == 1)
                {
                    Global.ReStart = 0;
                    BroasClientNew loginBroas = new();
                    loginBroas.ShowDialog();
                }
            }
        }

        /// <summary>
        /// UI 线程未处理异常
        /// </summary>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleGlobalException(e.Exception, "UI 线程异常");
        }

        /// <summary>
        /// 非 UI 线程 / 后台线程未处理异常
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            HandleGlobalException(ex, "非 UI 线程异常");
        }

        /// <summary>
        /// Task 中未观察到的异常
        /// </summary>
        private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleGlobalException(e.Exception, "任务调度异常");
            e.SetObserved();
        }

        /// <summary>
        /// 全局异常统一处理：记录日志并给出友好提示
        /// </summary>
        private static void HandleGlobalException(Exception? ex, string category)
        {
            if (ex == null)
            {
                return;
            }

            try
            {
                // 记录到全局日志（如果有专用日志类，可替换为你的日志实现）
                try
                {
                    // 写入 NLog 全局日志（FATAL 级别，表示致命错误）
                    Global.Logger?.Fatal($"{category}: {ex}");
                }
                catch
                {
                    // 忽略日志自身失败，避免二次异常
                }

            }
            catch
            {
                // 最终兜底：任何在全局异常处理中的异常都直接忽略，避免递归崩溃
            }
        }
    }
}
