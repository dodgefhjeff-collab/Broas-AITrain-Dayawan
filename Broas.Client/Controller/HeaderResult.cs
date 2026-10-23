
namespace WaterStation.Client.Controller
{
    public class HeaderResult<T>
    {
        public HeaderResult()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            IsSucceed = false;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public virtual bool IsSucceed { get; set; }
        /// <summary>
        /// 信息通知
        /// </summary>
        public virtual string Message { get; set; } = string.Empty;
        /// <summary>
        /// 返回的类型
        /// </summary>
        public virtual T Result { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public virtual string StatusCode { get; set; } = string.Empty;

        /// <summary>
        /// 分页总条数
        /// </summary>
        public int Total { get; set; }
    }
}
