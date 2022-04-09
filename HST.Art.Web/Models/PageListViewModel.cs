using System;
using System.Collections.Generic;

namespace HST.Art.Web
{
    /// <summary>
    /// 分页视图模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageListViewModel<T>:List<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">集合</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalItemCount">总记录数</param>
        public PageListViewModel(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            if (items != null)
                AddRange(items);
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;

        }
        
        /// <summary>
        /// 附加记录
        /// </summary>
        public int ExtraCount { get; set; }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// 总页面数
        /// </summary>
        public int TotalPageCount { get { return (int)Math.Ceiling(TotalItemCount / (double)PageSize); } }

        /// <summary>
        /// 开始记录索引
        /// </summary>
        public int StartRecordIndex { get { return (CurrentPageIndex - 1) * PageSize + 1; } }

        /// <summary>
        /// 结束记录索引
        /// </summary>
        public int EndRecordIndex { get { return TotalItemCount > CurrentPageIndex * PageSize ? CurrentPageIndex * PageSize : TotalItemCount; } }
    }
}
