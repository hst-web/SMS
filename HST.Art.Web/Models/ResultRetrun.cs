using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZT.SMS.Web
{
    [Serializable]
    public partial class ResultRetrun
    {
        /// <summary>
        /// 返回错误信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 返回其他信息 例如里边可以放json串用来给前台传值
        /// </summary>
        public string other { get; set; }
        /// <summary>
        /// 是否成功，默认是false
        /// </summary>
        public bool isSuccess { get; set; }
        /// <summary>
        /// 数字标记 默认:0
        /// </summary>
        public int sign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResultRetrun()
        {
            isSuccess = false;
            sign = 0;
        }
    }

    public class ProgressResult
    {
        public ProgressResult() { }
        public ProgressResult(int totalCount)
        {
            this.totalCount = totalCount;
        }

        private int totalCount;
        private int count;

        public int SuccessCount { get; set; }
        /// <summary>
        /// 结束标记
        /// </summary>
        public bool EndTag { get; set; }
        public string Message { get; set; }
        public int Speed
        {
            get
            {
                if (count > 0)
                {
                    int curSpe = count * 100 / TotalCount;
                    if (curSpe >= 100)
                    {
                        EndTag = true;
                        return 100;
                    }

                    return curSpe;
                }

                return count;
            }
        }

        public int FailCount
        {
            get
            {
                return TotalCount - SuccessCount;
            }
        }

        public int Count
        {
            set
            {
                count = value;
            }
            get
            {
                return count;
            }
        }

        public int TotalCount
        {
            get
            {
                return totalCount;
            }
        }
    }
}