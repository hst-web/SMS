using Microsoft.AspNet.SignalR;
using System;

namespace HST.Utillity
{
    /// <summary>
    /// web消息助手
    /// </summary>
    public class WebMessageHelper
    {
        private readonly static Lazy<WebMessageHelper> _instance = new Lazy<WebMessageHelper>(() => new WebMessageHelper());

        public static WebMessageHelper Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        private readonly IHubContext<IHubClient> _hubContext;
        private WebMessageHelper()
        {
            //获取指定hub上下文对象
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<IHubClient>("messageHub");
        }
        
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        public void sendToAll(string msg)
        {
            var message = new { Content = msg };
            _hubContext.Clients.All.recieveMessage(message);//调用客户端函数（用于客户端接收消息）
        }
        
    }
}