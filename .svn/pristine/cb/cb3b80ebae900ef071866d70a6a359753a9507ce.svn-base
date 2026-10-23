using System;
using System.Collections.Generic;

namespace WaterStation.Client.Model.Commom
{
    public class PageDto<T>
    {
        public PageDto()
        {
            IsSucceed = false;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public virtual bool IsSucceed { get; set; }

        /// <summary>
        /// 返回的类型
        /// </summary>
        public virtual List<T> Result { get; set; }

        /// <summary>
        /// 分页总条数
        /// </summary>
        public int Total { get; set; }
    }
}
