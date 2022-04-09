using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HST.Art.Core;
using HST.Utillity;

namespace HST.Art.Web
{
    public class RotationChartViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "链接地址不能为空")]
        [RegularExpression(@"^(?:([A-Za-z]+):)?(\/{0,3})([0-9.\-A-Za-z]+)(?::(\d+))?(?:\/([^?#]*))?(?:\?([^#]*))?(?:#(.*))?$", ErrorMessage = "链接地址格式不正确，请参照http://www.xxx.com格式填写。")]
        public string WebLink { get; set; }

        public RotationType RotationType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "请上传轮播图片")]
        public string HeadImg { get; set; }
        public string SmallHeadImg { get; set; }
        public string SortJson { get; set; }
    }
}