using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HST.Art.Web
{
    public class UserViewModel
    {
        public int Id { get; set; }        

        [StringLength(20, MinimumLength = 2, ErrorMessage = "用户名长度须{2}-{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "用户名不能包含空字符")]
        [Remote("CheckUserName", "User", AdditionalFields = "Id", ErrorMessage = "用户名称已存在")]
        public string UserName { get; set; } // user_name (length: 100)

        [Display(Name = "手机号")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"^1[34578]\d{9}$", ErrorMessage = "手机号格式不正确")]
        [Remote("CheckPhone", "User", AdditionalFields = "Id", ErrorMessage = "手机号已存在")]
        public string Phone { get; set; } // phone

        [Display(Name = "邮箱")]
        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "邮箱格式不正确")]
        [Remote("CheckEmail", "User", AdditionalFields = "Id", ErrorMessage = "邮箱已存在")]
        public string Email { get; set; } // email (length: 100)

        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度须{2}-{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不能为空")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z0-9-*/+.~!@#$%^&*()\S]{2,}$", ErrorMessage = "密码需含数字和字母，不能包含空字符")]
        public string Password { get; set; } // password (length: 50)

        [Required(AllowEmptyStrings = false, ErrorMessage = "姓名不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "姓名不能包含空字符")]
        public string RealName { get; set; }

        [Display(Name = "是否启用")]
        public int State { get; set; } // state

        [Display(Name = "创建时间")]
        public string CreateTime { get; set; }

        public bool IsSupAdmin { get; set; }

       
    }
}