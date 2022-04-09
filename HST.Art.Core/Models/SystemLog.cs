/*----------------------------------------------------------------
// 文件名：SystemLog.cs
// 功能描述：日志操作
// 创建者：sysmenu
// 创建时间：2020-01-14
//----------------------------------------------------------------*/
using System;
using HST.Utillity;
using Newtonsoft.Json;

namespace HST.Art.Core
{
    /// <summary>
    /// Setting
    /// </summary>
    [Serializable]
    public partial class SystemLog
    {
        public SystemLog()
        { }
        #region Model
        public int Id { get; set; }
        public int UserId { get; set; }
        public LogSource Source { get; set; }
        public LogType Type { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ClientIp { get; set; }
        public string UserAgent { get; set; }
        public string ResultLog { get; set; }
        public string ReqParameter { get; set; }
        public DateTime CreateDate { get; set; }
        #endregion Model
    }

    [Serializable]
    public class LogQuery
    {
        public LogSource Source { get; set; }
        public LogType Type { get; set; }
        public string StartDate { get; set; }
        private string _endDate;
        public string EndDate
        {
            set
            {
                value = _endDate;
            }

            get
            {
                if (!string.IsNullOrEmpty(_endDate))
                {
                    return Convert.ToDateTime(_endDate).AddDays(1).ToShortDateString();
                }

                return _endDate;
            }
        }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

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

        private int _pageIndex = 1;
        private int _pageSize = 20;
    }
}

