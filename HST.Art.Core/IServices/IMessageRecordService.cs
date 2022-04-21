using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.SMS.Core
{
    public interface IMessageRecordService : IBaseService
    {
        List<MessageRecord> GetPage(MessageRecordQuery recordQuery, out int totalNum);
        List<MessageRecord> GetAll(MessageRecordQuery recordQuery);
        MessageRecord Get(int id);
        MessageRecord GetByMsgId(string msgId);
        List<string> GetAllMessageId();
        bool Update(MessageRecord messageRecordInfo);
        void Update(List<MessageRecord> messageRecordInfos, out List<MessageRecord> failList);
        bool Add(MessageRecord messageRecordInfo);
        void Add(List<MessageRecord> messageRecordInfos, out List<MessageRecord> failList);
        bool Send(List<MessageRecord> sendMsgs, bool needCheck = true);
        int Count(MsgSendState state);
        MessageStatisticsInfo Statistics();
        void Righting();
        void LoadReport();
    }
}
