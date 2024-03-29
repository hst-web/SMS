﻿using ZT.SMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZT.SMS.Web
{
    public class SidebarViewModel
    {
        /// <summary>
        /// 最新信息
        /// </summary>
        public List<WebNewsViewModel> NewestList { get; set; }

        public CategoryType SectionType { get; set; }
    }

    public class HeaderViewModel
    {
        public CategoryType SectionType { get; set; }
        public List<CategoryDictionary> CategoryList { get; set; }
    }

}