using System;
using cn.jpush.api.common;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using cn.jpush.api.common.resp;
using cn.jpush.api;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HST.Utillity
{
    /// <summary>
    /// app消息助手
    /// </summary>
    public class AppMessageHelper
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="appKey">appkey</param>
        /// <param name="masterSecret">主密钥</param>
        public static void sendToAll(string msg, string appKey = "9b725db3484eceb1abab684a", string masterSecret = "7a6081367b0d0dda83a8b969")
        {
            JPushClient client = new JPushClient(appKey, masterSecret);
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();
            pushPayload.audience = Audience.all();
            pushPayload.message = Message.content(msg);
            try
            {
                var result = client.SendPush(pushPayload);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 发送通知（s_tag 定位）
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="appKey">appkey</param>
        /// <param name="masterSecret">主密钥</param>
        public static void sendNotificToUsers(string msg, List<string> userId, string appKey = "9b725db3484eceb1abab684a", string masterSecret = "7a6081367b0d0dda83a8b969")
        {
            JPushClient client = new JPushClient(appKey, masterSecret);
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_tag(userId.ToArray());
            pushPayload.notification = new Notification().setAlert(msg).setIos(new IosNotification().autoBadge().setSound("happy").AddExtra("form", "JPush"));

            try
            {
                var result = client.SendPush(pushPayload);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="extra">推送实体</param>
        /// <param name="userDics">用户集合</param>
        public static void sendMsgToUsers(PushMsgModel extra, List<PushStaff> userDics)
        {
            sendIosNotificToUsers(extra, userDics);
            sendAndroidMsgToUsers(extra, userDics);
        }

        /// <summary>
        /// 发送通知（s_tag 定位,ios）
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="appKey">appkey</param>
        /// <param name="masterSecret">主密钥</param>
        private static void sendIosNotificToUsers(PushMsgModel extra, List<PushStaff> userDics, string appKey = "9b725db3484eceb1abab684a", string masterSecret = "7a6081367b0d0dda83a8b969")
        {
            if (userDics.Count <= 0)
            {
                return;
            }

            string extraData =JsonConvert.SerializeObject(extra);
            JPushClient client = new JPushClient(appKey, masterSecret);
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.ios();

            foreach (PushStaff item in userDics)
            {
                pushPayload.audience = Audience.s_alias(item.UserName);
                pushPayload.options.apns_production = true;
                if (item.IsReceivePush)
                {
                    if (item.IsPushVoice)
                        pushPayload.notification = new Notification().setAlert(extra.MessageTitle).setIos(new IosNotification().autoBadge().setSound("newworkorder.wav").AddExtra("extra", extraData));
                    else
                        pushPayload.notification = new Notification().setAlert(extra.MessageTitle).setIos(new IosNotification().autoBadge().setSound("happy").AddExtra("extra", extraData));

                    try
                    {
                        var result = client.SendPush(pushPayload);
                    }
                    catch
                    {

                    }
                }           
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="appKey">appkey</param>
        /// <param name="masterSecret">主密钥</param>
        private static void sendAndroidMsgToUsers(PushMsgModel msg, List<PushStaff> userDics, string appKey = "9b725db3484eceb1abab684a", string masterSecret = "7a6081367b0d0dda83a8b969")
        {
            JPushClient client = new JPushClient(appKey, masterSecret);
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();

            foreach (PushStaff item in userDics)
            {
                pushPayload.audience = Audience.s_alias(item.UserName);
                pushPayload.options.time_to_live = 30;

                if (item.IsPushVoice)
                {
                    msg.IsPushVoice = true;
                    pushPayload.message = Message.content(JsonConvert.SerializeObject(msg));
                }
                else
                {
                    msg.IsPushVoice = false;
                    pushPayload.message = Message.content(JsonConvert.SerializeObject(msg));
                }
                  
                try
                {
                    var result = client.SendPush(pushPayload);
                }
                catch
                {

                }
            }
        }
    }

    /// <summary>
    /// 推送消息model
    /// </summary>
    public class PushMsgModel
    {
        public string ServiceSpaceId { get; set; }
        public string WorkOrderId { get; set; }
        public string MessageTitle { get; set; }
        public bool IsPushVoice { get; set; }
    }

    public class PushStaff
    {
        public string UserName { get; set; }
        public bool IsPushVoice { get; set; }
        public bool IsReceivePush { get; set; }
    }
}