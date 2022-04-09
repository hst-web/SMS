using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HST.Art.Web
{
    public class AccountViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "用户名")]
        public string UserName { get; set; } // user_name (length: 100)

        [Required(AllowEmptyStrings = false, ErrorMessage = "姓名不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "姓名不能包含空字符")]
        public string RealName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"^1[34578]\d{9}$", ErrorMessage = "手机号格式不正确")]
        [Remote("CheckPhone", "User", AdditionalFields = "Id", ErrorMessage = "手机号已存在")]
        public string Phone { get; set; } // phone

        [Remote("CheckEmail", "User", AdditionalFields = "Id", ErrorMessage = "邮箱已存在")]
        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; } // email (length: 100)

        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度{2}-{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不能为空")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z0-9-*/+.~!@#$%^&*()\S]{2,}$", ErrorMessage = "密码需含数字和字母，不能包含空字符")]
        [Display(Name = "密码")]
        public string Password { get; set; } // password (length: 50)
    
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        public bool IsSupAdmin { get; set; }
    }
}