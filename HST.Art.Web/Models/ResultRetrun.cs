using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HST.Art.Web
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
}