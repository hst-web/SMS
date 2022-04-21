/*----------------------------------------------------------------
// 文件名：MessageRecordService.cs
// 功能描述：会员单位服务
// 创建者：sysmenu
// 创建时间：2019-4-18
//----------------------------------------------------------------*/
using ZT.SMS.Core;
using System.Collections.Generic;
using System.Linq;
using ZT.SMS.Data;
using System;
using System.Text.RegularExpressions;
using ZT.Utillity;
using System.Web.Configuration;
using System.Web;
using System.Threading;

namespace ZT.SMS.Service
{
    public class MessageRecordService : ServiceBase, IMessageRecordService
    {
        MessageRecordProvider _MessageRecordProvider = new MessageRecordProvider();
        IntegratedProvider _interProvider = new IntegratedProvider();
        private static readonly object objLock = new object();
        private static string signId = WebConfigurationManager.AppSettings["SignId"].ToString();
        private static string templateId = WebConfigurationManager.AppSettings["TemplateId"].ToString();
        //signId, string templateId
        public MessageRecord Get(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            MessageRecord MessageRecordInfo = _MessageRecordProvider.Get(id);
            return MessageRecordInfo;
        }

        public List<MessageRecord> GetAll(MessageRecordQuery filterModel = null)
        {
            List<MessageRecord> MessageRecordList = _MessageRecordProvider.GetAll(filterModel);
            return MessageRecordList;
        }

        public List<MessageRecord> GetPage(MessageRecordQuery filterModel, out int totalNum)
        {
            totalNum = 0;
            //参数验证
            if (filterModel == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //获取数据
            List<MessageRecord> MessageRecordList = _MessageRecordProvider.GetPage(filterModel, out totalNum);

            return MessageRecordList;
        }

        public bool Add(MessageRecord MessageRecordInfo)
        {
            //参数验证
            if (MessageRecordInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _MessageRecordProvider.Add(MessageRecordInfo);
        }

        #region 状态操作
        public bool Delete(int id)
        {
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _MessageRecordProvider.Delete(id);
        }

        public bool LogicDelete(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _MessageRecordProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "IsDeleted",
                Value = 1,
                TableName = "MessageRecord"
            });
        }

        public bool Publish(int id)
        {
            //参数验证 
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _MessageRecordProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "State",
                Value = (int)MsgSendState.SendSuccess,
                TableName = "MessageRecord"
            });
        }

        public bool Recovery(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _MessageRecordProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "State",
                Value = (int)MsgSendState.Unsent,
                TableName = "MessageRecord"
            });
        }

        #endregion


        public bool Update(MessageRecord MessageRecordInfo)
        {
            //参数验证
            if (MessageRecordInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }
            return _MessageRecordProvider.Update(MessageRecordInfo);
        }

        public MessageRecord GetByMsgId(string msgId)
        {
            //参数验证
            if (string.IsNullOrEmpty(msgId))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            MessageRecordQuery query = new MessageRecordQuery();
            query.MessageId = msgId;
            List<MessageRecord> MessageRecordList = _MessageRecordProvider.GetAll(query);
            if (MessageRecordList != null && MessageRecordList.Count > 0)
            {
                return MessageRecordList.FirstOrDefault();
            }

            return null;
        }

        public void Add(List<MessageRecord> MessageRecordInfos, out List<MessageRecord> failList)
        {
            if (MessageRecordInfos == null || MessageRecordInfos.Count <= 0)
            {
                failList = null;
                ErrorMsg = ErrorCode.ParameterNull;
                return;
            }

            _MessageRecordProvider.Add(MessageRecordInfos, out failList);
        }

        public List<string> GetAllMessageId()
        {
            return _MessageRecordProvider.GetAllMessageId();
        }

        public void Update(List<MessageRecord> messageRecordInfos, out List<MessageRecord> failList)
        {
            if (messageRecordInfos == null || messageRecordInfos.Count <= 0)
            {
                failList = null;
                ErrorMsg = ErrorCode.ParameterNull;
                return;
            }

            _MessageRecordProvider.Update(messageRecordInfos, out failList);
        }

        public bool Send(List<MessageRecord> sendMsgList, bool needCheck = true)
        {
            PreconditionUtil.checkArgument(CollectionUtils.IsNotEmpty(sendMsgList), "参数校验失败");

            lock (objLock)
            {
                FGSMSResponse response = new FGSMSResponse();
                if (needCheck)
                {
                    List<MessageRecord> msgLogList = JsonUtils.Deserialize<List<MessageRecord>>(sendMsgList.Serialize());
                    CheckSendRecord(ref sendMsgList);
                    if (CollectionUtils.IsEmpty(sendMsgList))
                    {
                        // 记录日志
                        response.msg = "消息已经发送成功，无需重复发送";
                        AddLog(msgLogList, response);
                        return true;
                    }
                }

                // 后续跟进业务调整
                string content = string.Format("{0}||{1}||{2}", sendMsgList.First().MsgData.OrderDate.GetDateTimeFormats('D')[0].ToString(), string.Join("、", sendMsgList.Select(g => g.MsgData.OrderName)), string.Join("、", sendMsgList.Select(g => g.MessageId)));
                response = FGSMSHelper.TemplateSMS(signId, templateId, content, sendMsgList.First().ToAddress);
                Logger.Info(typeof(MessageRecordService), "message body=" + string.Format("request:{0},response:{1}", sendMsgList.First().ToAddress + "," + content, response.Serialize()));


                if (response.code == 0)
                {
                    sendMsgList.ForEach(g => { g.SendState = MsgSendState.SendSuccess; g.MsgData.MsgNo.Add(response.msg_no); });
                    _MessageRecordProvider.Send(sendMsgList);
                }
                else
                {
                    sendMsgList.ForEach(g =>
                    {
                        g.SendState = MsgSendState.SendFailed;
                        if (!string.IsNullOrEmpty(response.msg_no))
                            g.MsgData.MsgNo.Add(response.msg_no);
                    });
                    _MessageRecordProvider.Send(sendMsgList);

                }
                // 记录日志
                AddLog(sendMsgList, response);

                // 余额不足结束流程
                if (response.code == 15)
                {
                    throw new BizException(response.code.ToString(), response.errorMsg);
                }

                return response.success;
            }
        }

        private void CheckSendRecord(ref List<MessageRecord> sendMsgList)
        {
            List<int> ids = sendMsgList.Select(g => g.Id).ToList();
            sendMsgList.Clear();
            sendMsgList.AddRange(_MessageRecordProvider.Get(ids).FindAll(s => s.SendState == MsgSendState.Unsent || s.SendState == MsgSendState.SendFailed));
        }

        private void AddLog(List<MessageRecord> sendMsgs, FGSMSResponse response)
        {
            SystemLog sLog = new SystemLog();
            sLog.ActionName = "SendSMS";
            sLog.ControllerName = "MessageRecordService.Send";
            sLog.ResultLog = JsonUtils.Serialize(response);
            sLog.Source = LogSource.Admin;
            sLog.Type = response.success ? LogType.Info : LogType.Error;
            sLog.UserAgent = string.Join(",", sendMsgs.Select(g => g.MessageId));
            sLog.UserId = sendMsgs.First().OperatorId;
            sLog.ReqParameter = sendMsgs.Serialize();
            _interProvider.AddLog(sLog);
        }

        public int Count(MsgSendState state)
        {
            return _MessageRecordProvider.GetCount(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Key = "State",
                Value = (int)state,
                TableName = "MessageRecord"
            });
        }

        public MessageStatisticsInfo Statistics()
        {
            return _MessageRecordProvider.Statistics();
        }

        /// <summary>
        /// 加载报表
        /// </summary>
        public void LoadReport()
        {
            while (true)
            {
                FGSMSReportResponse response = FGSMSHelper.GetSMSReport();
                Logger.Info(typeof(MessageRecordService), "GetSMSReport=" + JsonUtils.Serialize(response));
                if (!response.success)
                {
                    SystemLog sLog = new SystemLog();
                    sLog.ActionName = "SMSReport";
                    sLog.ControllerName = "MessageRecordService.AddReport";
                    sLog.ResultLog = JsonUtils.Serialize(response);
                    sLog.Source = LogSource.Admin;
                    sLog.Type = LogType.Error;
                    _interProvider.AddLog(sLog);
                    break;
                }

                if (CollectionUtils.IsEmpty(response.data)) continue;
                _MessageRecordProvider.AddReport(response.data.Select(g => new MessageReport()
                {
                    MsgNo = g.msg_no,
                    Mobile = g.mobile,
                    ResultCode = g.status,
                    SendState = g.success ? MsgSendState.SendSuccess : MsgSendState.ReceiveFailed
                }).ToList());
                //需要间隔5s
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// 冲正
        /// </summary>
        public void Righting()
        {
            LoadReport();
            RightingHandle();
            Logger.Info(typeof(MessageRecordService), "Righting End");
        }

        private void RightingHandle()
        {
            List<MessageReport> reportList = _MessageRecordProvider.GetAllReport();
            if (CollectionUtils.IsEmpty(reportList))
            {
                return;
            }

            List<MessageRecord> messageList = _MessageRecordProvider.GetAll(new MessageRecordQuery() { Righting = 0 });
            if (CollectionUtils.IsEmpty(messageList))
            {
                return;
            }

            List<MessageReport> reportEndList = new List<MessageReport>();
            List<MessageRecord> messageEndList = new List<MessageRecord>();
            List<MessageRecord> faildList = null;
            foreach (MessageReport item in reportList)
            {
                List<MessageRecord> messageItemList = messageList.FindAll(g => g.MsgData.MsgNo.Contains(item.MsgNo));
                if (CollectionUtils.IsNotEmpty(messageItemList))
                {
                    messageItemList.ForEach(g => { g.SendState = item.SendState; g.IsRighting = true; });
                    messageEndList.AddRange(messageItemList);
                    item.IsRighting = true;
                    reportEndList.Add(item);
                }
            }

            _MessageRecordProvider.UpdateReport(reportEndList);
            _MessageRecordProvider.Update(messageEndList, out faildList);
        }


    }
}
