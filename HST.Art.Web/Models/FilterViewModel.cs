using ZT.SMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.SMS.Web
{
    /// <summary>
    /// 筛选条件类
    /// </summary>
    public class FilterViewModel
    {
        public CategoryType CategoryType { get; set; }
        public bool IsParent { get; set; }
    }
}
