using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HST.Art.Core;
using HST.Utillity;

namespace HST.Art.Web
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [StringLength(50, ErrorMessage = "文章标题不可超过{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "文章标题不能为空")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "请上传文章头图")]
        public string HeadImg { get; set; }
        public string Description { get; set; }

        public SectionType Section { get; set; }
        public string SectionName
        {
            get
            {
                return Section.GetDescription();
            }
        }
        public int State { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "所属类别不能为空")]
        public int Category { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "所属类别不能为空")]
        public int ParCategory { get; set; }
        public string UserName { get; set; }

        public string SmallHeadImg { get; set; }

        public string CreateTime { get; set; }

        public string CategoryName
        {
            get; set;
        }
        public string ParCategoryName { get; set; }

        public string PublishDate { get; set; }
    }

    public class StatisticViewModel
    {
        public SectionType Section { get; set; }
        public string Name
        {
            get
            {
                return Section.GetDescription();
            }
        }
        public int Count { get; set; }
        public string Percent { get; set; }
    }
}