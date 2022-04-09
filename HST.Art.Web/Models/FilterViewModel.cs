using HST.Art.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Web
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
