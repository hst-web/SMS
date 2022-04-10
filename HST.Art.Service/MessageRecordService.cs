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

namespace ZT.SMS.Service
{
    public class MessageRecordService : ServiceBase, IMessageRecordService
    {
        MessageRecordProvider _MessageRecordProvider = new MessageRecordProvider();

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

        public bool Add(List<MessageRecord> MessageRecordInfos, out List<MessageRecord> failList)
        {
            if (MessageRecordInfos == null || MessageRecordInfos.Count <= 0)
            {
                failList = null;
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _MessageRecordProvider.Add(MessageRecordInfos, out failList);
        }

        public List<string> GetAllMessageId()
        {
            return _MessageRecordProvider.GetAllMessageId();
        }

        public bool Update(List<MessageRecord> messageRecordInfos, out List<MessageRecord> failList)
        {
            if (messageRecordInfos == null || messageRecordInfos.Count <= 0)
            {
                failList = null;
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _MessageRecordProvider.Update(messageRecordInfos, out failList);
        }
    }
}
