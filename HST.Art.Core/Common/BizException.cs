using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.SMS.Core
{
    public class BizException : Exception
    {
        private string errorCode;
        private int bizCode;
        public BizException(string errorCode, string message)
          : base(message)
        {
            this.errorCode = errorCode;
        }


        public BizException(string errorCode, string message, Exception innerException)
          : base(message, innerException)
        {
            this.errorCode = errorCode;
        }

        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
        }

        public int BizCode
        {
            get
            {
                return bizCode;
            }
        }
    }
}
