namespace HST.Utillity
{
    /// <summary>
    /// 接口：客户端js函数的原型，后台不要实现
    /// </summary>
    public interface IHubClient
    {
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="msg">消息</param>
        void recieveMessage(object msg);
    }
}
