using System;

namespace WaterStation.Client.Model.RunLogDto
{
    /// <summary>
    /// 
    /// 20220801
    /// 所有运行日志信息
    /// </summary>
    public class RunAllLogInfoDto
    {
        #region 定义为单利
        // 定义一个静态变量来保存类的实例
        private static RunAllLogInfoDto uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        public RunAllLogInfoDto()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static RunAllLogInfoDto GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new RunAllLogInfoDto();
                    }
                }
            }
            return uniqueInstance;
        }
        #endregion

        #region  所有运行日志信息

        /// <summary>
        /// 主键ID
        /// </summary>

        public long ID { get; set; }
      
        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime ReceivingTime { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public string Creator { get; set; }


        /// <summary>
        /// 启用状态
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Addtime { get; set; }
        /// <summary>
        /// 添加人
        /// </summary>
        public string AddUser { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }


        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Remark { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType { get; set; }
        #endregion

    }
}
