using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WaterStation.Client;
using WaterStation.Client.Controller;
using WaterStation.Client.Model.RunLogDto;
using NLog;

namespace WaterStation.Client.Model.Commom
{
    /// <summary>
    /// 日志相关
    /// </summary>
    public class logHelper
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region 字段属性
        private static string encryptKey = "1234";//配置文件加密的密匙20220917
        private LogType _logType;

        private string _basePath;

        private int _fileSize = 10 * 1024 * 1024; //日志分隔文件大小

        private LogStream _currentStream = new ();

        private string _dateFormat = "yyyyMMdd"; //日志文件名日期格式化

        private string _rootFolder = "Log"; //日志文件夹名称

        private Mutex _mutex;

        #endregion

        #region LogWriter
        public logHelper(LogType logType)
        {
            _logType = logType;
            _mutex = new Mutex(false, "Mutex.LogWriter." + logType.ToString() + ".7693FFAD38004F6B8FD31F6A8B4CE2BD");

            Init();
        }
        #endregion

        #region Init
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            //初始化 _basePath
            InitBasePath();

            //创建目录
            CreateLogDir();

            //更新日志写入流
            UpdateCurrentStream();
        }
        #endregion

        #region 初始化 _basePath
        /// <summary>
        /// 初始化 _basePath
        /// </summary>
        private void InitBasePath()
        {
            UriBuilder uri = new(Assembly.GetExecutingAssembly().Location);
            _basePath = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
        }
        #endregion

        #region 初始化 _currentArchiveIndex
        /// <summary>
        /// 初始化 _currentArchiveIndex
        /// </summary>
        private void InitCurrentArchiveIndex()
        {
            Regex regex = new(_currentStream.CurrentDateStr + "_*(\\d*).txt");
            string[] fileArr = Directory.GetFiles(_currentStream.CurrentLogFileDir, _currentStream.CurrentDateStr + "*");
            foreach (string file in fileArr)
            {
                Match match = regex.Match(file);
                if (match.Success)
                {
                    string str = match.Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        int temp = Convert.ToInt32(str);
                        if (temp > _currentStream.CurrentArchiveIndex)
                            _currentStream.CurrentArchiveIndex = temp;
                    }
                    else
                        _currentStream.CurrentArchiveIndex = 0;
                }
            }
        }
        #endregion

        #region 初始化 _currentFileSize
        /// <summary>
        /// 初始化 _currentFileSize
        /// </summary>
        private void InitCurrentFileSize()
        {
            FileInfo fileInfo = new (_currentStream.CurrentLogFilePath);
            _currentStream.CurrentFileSize = fileInfo.Length;
        }
        #endregion

        #region CreateLogDir()
        /// <summary>
        /// 创建日志目录
        /// </summary>
        private void CreateLogDir()
        {
            string logDir = Path.Combine(_basePath, _rootFolder + "\\" + _logType.ToString());
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }
        #endregion

        #region CreateStream
        /// <summary>
        /// 创建日志写入流
        /// </summary>
        private void CreateStream()
        {
            _currentStream.CurrentFileStream = new (_currentStream.CurrentLogFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            _currentStream.CurrentStreamWriter = new (_currentStream.CurrentFileStream, Encoding.UTF8);
        }
        #endregion

        #region CloseStream
        /// <summary>
        /// 关闭日志写入流
        /// </summary>
        private void CloseStream()
        {
            _currentStream.CurrentStreamWriter?.Close();
            _currentStream.CurrentFileStream?.Close();
        }
        #endregion

        #region 拼接日志内容
        /// <summary>
        /// 拼接日志内容
        /// </summary>
        private static string CreateLogString(LogType logType, string log)
        {
            return string.Format(@"{0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ("[" + logType.ToString() + "]").PadRight(7, ' '), log);
        }
        #endregion

        #region 写文件
        /// <summary>
        /// 写文件
        /// </summary>
        private void WriteFile(string log)
        {
            try
            {
                _mutex.WaitOne();

                //判断是否更新Stream
                string dateStr = DateTime.Now.ToString(_dateFormat);
                if (_currentStream.CurrentDateStr != dateStr)
                {
                    _currentStream.CurrentDateStr = dateStr;
                    UpdateCurrentStream();
                }

                //判断是否创建Archive
                int byteCount = Encoding.UTF8.GetByteCount(log);
                _currentStream.CurrentFileSize += byteCount;
                if (_currentStream.CurrentFileSize >= _fileSize)
                {
                    _currentStream.CurrentFileSize = 0;
                    CreateArchive();
                }

                //日志内容写入文件
                _currentStream.CurrentFileStream.Seek(0, SeekOrigin.End);
                _currentStream.CurrentStreamWriter.WriteLine(log);
                _currentStream.CurrentStreamWriter.Flush();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
        #endregion

        #region CreateArchive
        /// <summary>
        /// 创建日志存档
        /// </summary>
        private void CreateArchive()
        {
            string fileName = Path.GetFileNameWithoutExtension(_currentStream.CurrentLogFilePath);

            CloseStream(); //关闭日志写入流
            File.Move(_currentStream.CurrentLogFilePath, Path.Combine(_currentStream.CurrentLogFileDir, fileName + "_" + (++_currentStream.CurrentArchiveIndex) + ".txt")); //存档
            CreateStream(); //创建日志写入流
        }
        #endregion

        #region UpdateCurrentStream
        /// <summary>
        /// 更新日志写入流
        /// </summary>
        private void UpdateCurrentStream()
        {
            try
            {
                //关闭日志写入流
                CloseStream();

                //创建新的日志路径
                _currentStream.CurrentDateStr = DateTime.Now.ToString(_dateFormat);
                _currentStream.CurrentLogFileDir = Path.Combine(_basePath, _rootFolder + "\\" + _logType.ToString());
                _currentStream.CurrentLogFilePath = Path.Combine(_currentStream.CurrentLogFileDir, _currentStream.CurrentDateStr + ".txt");

                //创建日志写入流
                CreateStream();

                //初始化 _currentArchiveIndex
                InitCurrentArchiveIndex();

                //初始化 _currentFileSize
                InitCurrentFileSize();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        #endregion

        #region 写日志
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log">日志内容</param>
        public void WriteLog(string log)
        {
            try
            {
                log = CreateLogString(_logType, log);
                WriteFile(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        #endregion

        /// <summary>
        /// 吸光度日志子目录名（位于程序启动目录之下）
        /// 完整路径：{StartupPath}/{AbsorbanceLogFolder}/{yyyyMMdd}/rizhi吸光度.log
        /// </summary>
        public const string AbsorbanceLogFolder = "吸光度";

        /// <summary>
        /// 获取指定时间对应的吸光度日志路径（新目录结构）
        /// </summary>
        public static string GetAbsorbanceLogPath(DateTime time)
        {
            string root = System.Windows.Forms.Application.StartupPath;
            string folder = Path.Combine(root, AbsorbanceLogFolder, time.ToString("yyyyMMdd"));
            return Path.Combine(folder, "rizhi吸光度.log");
        }

        /// <summary>
        /// 解析吸光度日志读取路径：优先新目录，不存在则回退旧目录
        /// 旧目录：{StartupPath}/{yyyyMMdd}/rizhi吸光度.log
        /// </summary>
        public static string ResolveAbsorbanceLogPath(DateTime time)
        {
            string newPath = GetAbsorbanceLogPath(time);
            if (File.Exists(newPath))
                return newPath;

            string root = System.Windows.Forms.Application.StartupPath;
            string oldFolder = Path.Combine(root, time.ToString("yyyyMMdd"));
            return Path.Combine(oldFolder, "rizhi吸光度.log");
        }

        public static void WriteLogInfo(string strLog, string fileInfo)
        {
            var path = System.Windows.Forms.Application.StartupPath;
            string sFilePath = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
            //string sFileName = "rizhi" + DateTime.Now.ToString("yyyyMMdd-HH-mm-ss") + ".log";
            string sFileName = "rizhi" + fileInfo + ".log";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
            if (!Directory.Exists(sFilePath))//验证路径是否存在
                Directory.CreateDirectory(sFilePath);
                //不存在则创建
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
                using (FileStream fs1 = new FileStream(sFileName, FileMode.Append, FileAccess.Write))
                {
                    sw = new StreamWriter(fs1);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   ---   " + strLog);
                    sw.Close();
                    sw.Dispose();
                }
                //  fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            else
                using (FileStream fs1 = new FileStream(sFileName, FileMode.Create, FileAccess.Write))
                {
                    sw = new StreamWriter(fs1);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   ---   " + strLog);
                    sw.Close();
                    sw.Dispose();
                }
                //fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            //sw = new StreamWriter(fs);
            //sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   ---   " + strLog);
            //sw.Close();
            //sw.Dispose();
            //fs.Close();
            //fs.Dispose();
        }
        #region 加密解密
        public static string Encrypt(string str)
        {
            DESCryptoServiceProvider descsp = new();
            byte[] key = Encoding.Unicode.GetBytes(encryptKey);//密匙
            byte[] data = Encoding.Unicode.GetBytes(str);
            MemoryStream MStream = new ();
            CryptoStream CStream = new (MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);
            CStream.FlushFinalBlock();
            return Convert.ToBase64String(MStream.ToArray());
        }
        /// <summary>
        /// 解密字符串信息
        ///  
        /// 20220828
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decrypt(string str, string encryptKey1)
        {
            DESCryptoServiceProvider descsp = new ();
            byte[] key = Encoding.Unicode.GetBytes(encryptKey1);
            byte[] data = Convert.FromBase64String(str);
            MemoryStream MStream = new ();
            CryptoStream CStram = new (MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStram.Write(data, 0, data.Length);
            CStram.FlushFinalBlock();
            return Encoding.Unicode.GetString(MStream.ToArray());
        }
        #endregion

        #region 日志写入db
        /// <summary>
        /// 写运行/操作日志到数据库（流程、报警、操作等统一用此方法）
        /// </summary>
        /// <param name="logType">日志类型，如 "流程"、"报警"、"操作"</param>
        /// <param name="content">日志内容</param>
        public static void WriteLogDB(string logType, string content)
        {
            try
            {
                var now = DateTime.Now;
                var dto = new RunAllLogInfoDto
                {
                    LogType = logType,
                    LogContent = content ?? "",
                    ReceivingTime = now,
                    Addtime = now,
                    UpdateTime = now
                };
                if (string.Equals(logType, "操作", StringComparison.Ordinal))
                    dto.Creator = Global.userInfoDto?.UserName ?? Global.userInfoDto?.FullName ?? "";
                AddRunAllLogInfo(dto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "WriteLogDB 写库异常 LogType={0}", logType);
            }
        }

        /// <summary>
        /// 增加日志信息
        /// 写入数据库
        /// </summary>
        /// <param name="runAllLogInfo"></param>
        public static async void AddRunAllLogInfo(RunAllLogInfoDto runAllLogInfo)
        {
            try
            {
                var headerResult = await DataAPI.AddRunlogAsync(runAllLogInfo);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "添加db日志时发生异常");
            }
        }
        #endregion
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        Debug,

        Info,

        Error
    }
    internal class LogStream
    {
        public FileStream CurrentFileStream { get; set; }

        public StreamWriter CurrentStreamWriter { get; set; }

        public int CurrentArchiveIndex { get; set; }

        public long CurrentFileSize { get; set; }

        public string CurrentDateStr { get; set; }

        public string CurrentLogFilePath { get; set; }

        public string CurrentLogFileDir { get; set; }
    }
}
