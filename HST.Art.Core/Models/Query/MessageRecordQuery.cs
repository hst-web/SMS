using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.SMS.Core
{
   public class MessageRecordQuery
    {
        private int _pageIndex = 1;
        private int _pageSize = 10;
        public string MessageId { get; set; }
        public string CreateDate { get; set; }
        private int righting = -1;
      

        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }

            set
            {
                _pageIndex = value;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = value;
            }
        }

        public int SendState
        {
            get
            {
                return sendState;
            }

            set
            {
                sendState = value;
            }
        }

        /// <summary>
        /// 1 true,0 false
        /// </summary>
        public int Righting
        {
            get
            {
                return righting;
            }

            set
            {
                righting = value;
            }
        }

        private int sendState = -1;
    }
}
