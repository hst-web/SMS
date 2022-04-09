using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HST.Art.Web
{
    public class LoginViewModel
    {
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0}长度{2}-{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不能为空")]
        [Display(Name = "账号")]
        public string Account { get; set; }

        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0}长度{2}-{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不能为空")]
        [Display(Name = "密码")]
        public string Password { get; set; }


        //[StringLength(4, MinimumLength = 4, ErrorMessage = "{0}长度{2}个字符")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "验证码不能为空")]
        //[Display(Name = "验证码")]
        //public string ValidCode { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class LogViewModel
    {
        /// <summary>
        /// 查询日期
        /// </summary>
        public string SearchDate { get; set; }

        /// <summary>
        /// 下载链接
        /// </summary>
        public string DownloadUrl { get; set; }

        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get; set; }

    }
}