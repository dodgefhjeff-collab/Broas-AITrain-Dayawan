using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Threading;
using NLog;

namespace WaterStation.Client
{
    public static class ShowTip
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // 用于节流控制的字典,记录最后一次显示时间
        private static readonly ConcurrentDictionary<string, DateTime> lastShowTime = new();

        // P1：同一消息节流间隔(毫秒)，加严以降低 GDI 压力
        private const int ThrottleInterval = 1500;

        // P1：与 UIMessageTip.MaxConcurrentTips 对齐
        private static readonly SemaphoreSlim tipSemaphore = new(UIMessageTip.MaxConcurrentTips, UIMessageTip.MaxConcurrentTips);

        #region 消息提示
        /// <summary>
        /// 显示出错消息(带节流和并发控制)
        /// </summary>
        /// <param name="text">消息文本</param>
        /// <param name="delay">消息停留时长(ms)。默认1秒</param>
        /// <param name="floating">是否漂浮</param>
        public static void ShowErrorTip(string text, int delay = 1000, bool floating = true)
        {
            try
            {
                // 节流检查:同一消息在短时间内不重复显示
                string key = $"Error_{text}";
                DateTime now = DateTime.Now;

                if (lastShowTime.TryGetValue(key, out DateTime lastTime))
                {
                    if ((now - lastTime).TotalMilliseconds < ThrottleInterval)
                    {
                        // 记录到日志但不显示UI
                        logger.Warn($"错误提示被节流: {text}");
                        return;
                    }
                }

                lastShowTime[key] = now;

                // 尝试获取信号量,如果无法获取则降级为日志记录
                if (!tipSemaphore.Wait(0))
                {
                    logger.Error($"提示窗口数量已达上限,降级为日志记录: {text}");
                    return;
                }

                try
                {
                    UIMessageTip.ShowError(text, delay, floating);
                }
                finally
                {
                    // 延迟释放信号量,确保提示窗口有时间显示
                    _ = System.Threading.Tasks.Task.Delay(delay + 100).ContinueWith(_ =>
                    {
                        try { tipSemaphore.Release(); }
                        catch (SemaphoreFullException) { }
                    });
                }
            }
            catch (OutOfMemoryException ex)
            {
                // 内存不足时降级为日志记录
                logger.Fatal(ex, $"内存不足,无法显示错误提示: {text}");
            }
            catch (Exception ex)
            {
                // 其他异常也记录日志,避免二次异常
                logger.Error(ex, $"显示错误提示时发生异常: {text}");
            }
        }

        /// <summary>
        /// 显示警告消息(带节流和并发控制)
        /// </summary>
        /// <param name="text">消息文本</param>
        /// <param name="delay">消息停留时长(ms)。默认1秒</param>
        /// <param name="floating">是否漂浮</param>
        public static void ShowWarningTip(string text, int delay = 1000, bool floating = true)
        {
            try
            {
                string key = $"Warning_{text}";
                DateTime now = DateTime.Now;

                if (lastShowTime.TryGetValue(key, out DateTime lastTime))
                {
                    if ((now - lastTime).TotalMilliseconds < ThrottleInterval)
                    {
                        logger.Warn($"警告提示被节流: {text}");
                        return;
                    }
                }

                lastShowTime[key] = now;

                if (!tipSemaphore.Wait(0))
                {
                    logger.Warn($"提示窗口数量已达上限,降级为日志记录: {text}");
                    return;
                }

                try
                {
                    UIMessageTip.ShowWarning(text, delay, floating);
                }
                finally
                {
                    _ = System.Threading.Tasks.Task.Delay(delay + 100).ContinueWith(_ =>
                    {
                        try { tipSemaphore.Release(); }
                        catch (SemaphoreFullException) { }
                    });
                }
            }
            catch (OutOfMemoryException ex)
            {
                logger.Fatal(ex, $"内存不足,无法显示警告提示: {text}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"显示警告提示时发生异常: {text}");
            }
        }

        /// <summary>
        /// 显示成功消息(带节流和并发控制)
        /// </summary>
        /// <param name="text">消息文本</param>
        /// <param name="delay">消息停留时长(ms)。默认1秒</param>
        /// <param name="floating">是否漂浮</param>
        public static void ShowSuccessTip(string text, int delay = 1000, bool floating = true)
        {
            try
            {
                // P1：成功消息同样节流，避免刷屏占满 GDI
                string key = $"Ok_{text}";
                DateTime now = DateTime.Now;

                if (lastShowTime.TryGetValue(key, out DateTime lastTime))
                {
                    if ((now - lastTime).TotalMilliseconds < ThrottleInterval)
                    {
                        logger.Info($"成功提示被节流: {text}");
                        return;
                    }
                }

                lastShowTime[key] = now;

                if (!tipSemaphore.Wait(0))
                {
                    logger.Info($"提示窗口数量已达上限,跳过成功提示: {text}");
                    return;
                }

                try
                {
                    UIMessageTip.ShowOk(text, delay, floating);
                }
                finally
                {
                    _ = System.Threading.Tasks.Task.Delay(delay + 100).ContinueWith(_ =>
                    {
                        try { tipSemaphore.Release(); }
                        catch (SemaphoreFullException) { }
                    });
                }
            }
            catch (OutOfMemoryException ex)
            {
                logger.Fatal(ex, $"内存不足,无法显示成功提示: {text}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"显示成功提示时发生异常: {text}");
            }
        }

        /// <summary>
        /// 显示确认对话框
        /// </summary>
        public static bool ShowAskDialog(string title, string msg, UIStyle style = UIStyle.Blue, bool showMask = true)
        {
            try
            {
                return UIMessageDialog.ShowMessageDialog(msg, title, true, style, showMask);
            }
            catch (OutOfMemoryException ex)
            {
                logger.Fatal(ex, $"内存不足,无法显示确认对话框: {title} - {msg}");
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"显示确认对话框时发生异常: {title} - {msg}");
                return false;
            }
        }

        /// <summary>
        /// 清理过期的节流记录(建议定期调用)
        /// </summary>
        public static void CleanupThrottleCache()
        {
            try
            {
                DateTime threshold = DateTime.Now.AddMinutes(-5);
                var expiredKeys = new System.Collections.Generic.List<string>();

                foreach (var kvp in lastShowTime)
                {
                    if (kvp.Value < threshold)
                    {
                        expiredKeys.Add(kvp.Key);
                    }
                }

                foreach (var key in expiredKeys)
                {
                    lastShowTime.TryRemove(key, out _);
                }

                logger.Debug($"清理了 {expiredKeys.Count} 条过期的节流记录");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "清理节流缓存时发生异常");
            }
        }
        #endregion
    }
}
