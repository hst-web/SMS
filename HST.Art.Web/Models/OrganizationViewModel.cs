using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HST.Art.Core;
using HST.Utillity;

namespace HST.Art.Web
{
    public class OrganizationViewModel
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "{0}长度{2}-{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "企业名称不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "企业名称不能包含空字符")]
        public string OrgName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", ErrorMessage = "请输入正确的Email格式！")]
        [StringLength(50, ErrorMessage = "邮箱长度不能超过50个字符")]
        public string Email { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [RegularExpression(@"(^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$)|(^((\(\d{3}\))|(\d{3}\-))?([1][3-8]\d{9})$)", ErrorMessage = "电话格式不正确")]
        public string Telephone { get; set; }

        [StringLength(200, ErrorMessage = "长度不能超过200个字符")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "地址不能包含空字符")]
        public string Address { get; set; }

        public string Blog { get; set; }

        public string WeChat { get; set; }

        [StringLength(600, ErrorMessage = "长度不能超过600个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "企业简介不能为空")]
        public string Description { get; set; }

        public string Logo { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 组织架构
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }

        public string SmallHeadImg { get; set; }

        public string CreateTime { get; set; }

    }


}