using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HST.Art.Core;
using HST.Utillity;

namespace HST.Art.Web
{
    public class DownloadViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string FileName { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "{0}长度{2}-{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "下载标题不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "下载标题不能包含空字符")]
        public string FileTitle { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "所属类别不能为空")]
        public int Category { get; set; }

        public FileFormat FileType
        {

            get;set;
        }

       
        [Required(AllowEmptyStrings = false, ErrorMessage = "请上传附件文件")]
        public string Src { get; set; }

        public string UserName { get; set; }

        public int State { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "请上传文件头图")]
        public string FileImg { get; set; }

        public string SmallFileImg { get; set; }
        public string Description { get; set; }
        public string CreateTime { get; set; }

        public string Extension { get; set; }
        public string CategoryName
        {
            get; set;
        }
        public string Synopsis { get; set; }
    }
}