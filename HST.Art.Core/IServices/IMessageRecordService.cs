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
        bool Update(List<MessageRecord> messageRecordInfos, out List<MessageRecord> failList);
        bool Add(MessageRecord messageRecordInfo);
        bool Add(List<MessageRecord> messageRecordInfos, out List<MessageRecord> failList);
        bool Send(MessageRecord sendMsg);
    }
}
