using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HST.Art.Web
{
    public class ReturnPageResultIList<T>
    {
        public ReturnPageResultIList()
        { }

        public ReturnPageResultIList(IList<T> DataList, int totalRecord, int pageSize = 10)
        {
            DataT = DataList;
            totalRecords = totalRecord;
            PageSize = pageSize;
        }

        public int totalPages
        {
            get
            {
                if (totalRecords > 0)
                {
                    return (int)Math.Ceiling(Convert.ToDouble(totalRecords) / PageSize);
                }
                return 0;
            }
        }

        public int totalRecords { get; set; }
        public int PageSize { get; set; }

        public IList<T> DataT { get; set; }
    }
}