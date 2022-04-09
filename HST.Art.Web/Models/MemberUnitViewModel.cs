using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HST.Art.Core;
using HST.Utillity;

namespace HST.Art.Web
{
    public class MemberUnitViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "{0}长度{2}-{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "会员单位名称不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "会员单位名称不能包含空字符")]
        public string MemberUnitName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "所属类别不能为空")]
        public int Category { get; set; }
        public int Star { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "会员单位编号不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "会员单位编号不能包含空字符")]
        [Remote("CheckNumber", "MemberUnit", AdditionalFields = "Id", ErrorMessage = "会员单位编号已存在")]
        public string Number { get; set; }
        public string UserName { get; set; }
        public int State { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "请上传会员单位头图")]
        public string HeadImg { get; set; }

        public string SmallHeadImg { get; set; }
        public string Description { get; set; }
        public string CreateTime { get; set; }
        public string Synopsis { get; set; }
        public string CategoryName
        {
            get; set;
        }

        public int Province { get; set; }

        public int City { get; set; }

        public string Area { get; set; }

        public string StarName
        {
            get
            {
                return Star + " 星";
            }
        }
    }
}