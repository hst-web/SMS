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
        public string MsgData { get; set; }
        public int OperatorId { get; set; }
        private DateTime _createdate = DateTime.Now;
        private bool _isdeleted = false;
        public MsgSendState SendState { get; set; }

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
}
