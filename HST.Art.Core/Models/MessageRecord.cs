using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.SMS.Core
{
    [Serializable]
    public class MessageRecord
    {
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string ToAddress { get; set; }
        public DateTime SendDate { get; set; }
        public MsgDataInfo MsgData { get; set; }
        public int OperatorId { get; set; }
        private DateTime _createdate = DateTime.Now;
        private bool _isdeleted = false;
        public MsgSendState SendState { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }
    }

    public class MsgDataInfo
    {
        public DateTime OrderDate { get; set; }

        public string OrderName { get; set; }

        public List<string> MsgNo
        {
            get
            {
                return msgNo;
            }

            set
            {
                msgNo = value;
            }
        }

        private List<string> msgNo = new List<string>();

    }
}
