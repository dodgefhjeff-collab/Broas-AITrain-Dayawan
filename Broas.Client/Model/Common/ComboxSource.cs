using System.Collections.Generic;

namespace WaterStation.Client.Model.Commom
{
    public class ComboxSource
    {
        public string DisplayMember { get; set; }

        public int ValueMember { get; set; }

        /// 货位任务状态 0任务空闲 1 入库锁定  2 出库锁定
        public static List<ComboxSource> AllotSateSources = new()
        {
            new ComboxSource{DisplayMember ="请选择",ValueMember =-1  },
            new ComboxSource{DisplayMember ="任务空闲",ValueMember =0  },
            new ComboxSource{DisplayMember ="入库锁定",ValueMember =1  },
            new ComboxSource{DisplayMember ="出库锁定",ValueMember =2  },
            new ComboxSource{DisplayMember ="取空异常",ValueMember =3  },
            new ComboxSource{DisplayMember ="占位异常",ValueMember =4  },
        };

        /// <summary>
        /// 设备列表功能
        ///  20220825
        /// 状态
        /// </summary>
        public static List<ComboxSource> QualityControlSources = new()
        {
            new ComboxSource{DisplayMember ="请选择",ValueMember =-1  },
            new ComboxSource{DisplayMember ="开启",ValueMember =0  },
            new ComboxSource{DisplayMember ="关闭",ValueMember =1  },
            new ComboxSource{DisplayMember ="异常",ValueMember =2  }
        };

        /// <summary>
        /// 波特率
        ///  20220901
        /// 状态
        /// </summary>
        public static List<ComboxSource> GetBaudRate = new()
        {
            new ComboxSource{DisplayMember ="请选择",ValueMember =-1  },
            new ComboxSource{DisplayMember ="1200",ValueMember =1200  },
            new ComboxSource{DisplayMember ="2400",ValueMember =2400  },
            new ComboxSource{DisplayMember ="4800",ValueMember =4800  },
           new ComboxSource{DisplayMember ="9600",ValueMember =9600  },
            new ComboxSource{DisplayMember ="14400",ValueMember =14400  },
            new ComboxSource{DisplayMember ="19200",ValueMember =19200  },
            new ComboxSource{DisplayMember ="38400",ValueMember =38400  },
            new ComboxSource{DisplayMember ="56000",ValueMember =56000  },
            new ComboxSource{DisplayMember ="57600",ValueMember =57600  },
            new ComboxSource{DisplayMember ="115200",ValueMember =115200  },
            new ComboxSource{DisplayMember ="128000",ValueMember =128000  },
            new ComboxSource{DisplayMember ="230400",ValueMember =230400  }
        };
        /// <summary>
        /// 数据位
        ///  20220901
        /// 状态
        /// </summary>
        public static List<ComboxSource> GetDataBit = new()
        {
            new ComboxSource{DisplayMember ="请选择",ValueMember =-1  },
            new ComboxSource{DisplayMember ="5",ValueMember =5  },
            new ComboxSource{DisplayMember ="6",ValueMember =6  },
            new ComboxSource{DisplayMember ="7",ValueMember =7  },
           new ComboxSource{DisplayMember ="8",ValueMember =8  }
        };

        /// <summary>
        /// 停止位
        ///  20220901
        /// 状态
        /// </summary>
        public static List<ComboxSource> GetStopBit = new()
        {
            new ComboxSource{DisplayMember ="请选择",ValueMember =-1  },
            new ComboxSource{DisplayMember ="1",ValueMember =1  },
            new ComboxSource{DisplayMember ="1.5",ValueMember =(int)1.5  },
            new ComboxSource{DisplayMember ="2",ValueMember =2 }
        };

        /// <summary>
        /// 校验位
        ///  20220901
        /// 状态
        /// </summary>
        public static List<ComboxSource> GetCheckBit = new()
        {
            new ComboxSource{DisplayMember ="请选择",ValueMember =-1  },
            new ComboxSource{DisplayMember ="None",ValueMember =0  },
            new ComboxSource{DisplayMember ="One",ValueMember =1 },
            new ComboxSource{DisplayMember ="Two",ValueMember =2  },
           new ComboxSource{DisplayMember ="OnePointFive",ValueMember =3  }
        };
    }
}
