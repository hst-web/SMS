using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace HST.Utillity
{
    /// <summary>
    /// 客户端与服务端的通讯hub
    /// </summary>
    [HubName("messageHub")]
    public class MessageHub:Hub<IHubClient>
    {
        /// <summary>
        /// 发送消息（客户端可通过代理调用此函数，未使用）
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            Clients.All.recieveMessage(message);
        }
    }
}