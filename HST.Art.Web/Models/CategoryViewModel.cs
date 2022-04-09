using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HST.Art.Core;
using HST.Utillity;

namespace HST.Art.Web
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string CategoryName { get; set; }

        public int ParentId { get; set; }

        public string UserName { get; set; }

        public int State { get; set; }
        public string CreateTime { get; set; }
        public CategoryType CategoryType { get; set; }

        public string ParentName
        {
            get; set;
        }
    }
}