﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZT.SMS.Web
{
    /// <summary>
    /// JqueryDataTable插件交互的DT格式的数据(DT参数区分大小写)
    /// </summary>
    public class SearchDataTable
    {
        /// <summary>
        /// 请求次数（前端==》后端）
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// 总记录数（前端《==后端）
        /// </summary>
        public int recordsTotal { get; set; }

        /// <summary>
        /// 过滤后的总记录数（前端《==后端）
        /// </summary>
        public int recordsFiltered { get; set; }

        /// <summary>
        /// 记录开始索引（前端==》后端）
        /// </summary>
        public int start { get; set; }

        /// <summary>
        /// PageIndex（前端==》后端）
        /// </summary>
        public int pageIndex { get; set; }

        /// <summary>
        /// PageSize（前端==》后端）
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// 集合分页数据（前端《==后端）
        /// </summary>
        public IList data { get; set; }
    }

    public class SearchViewModel
    {
        public string FilterKey { get; set; }
        public string FilterVal { get; set; }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string ReserveField { get; set; }
    }


    public class MsgViewQuery
    {
        public string MsgId { get; set; }
        public string CreateDate { get; set; }

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

        private int sendState = -1;


    }

    /// <summary>
    /// 筛选类别
    /// </summary>
    public enum SearchType
    {
        UnKnown,
        Name,
        Title,
        Type,
        State,
        Number,
        Area,
        Date
    }
}