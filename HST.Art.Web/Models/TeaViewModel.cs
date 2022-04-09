using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HST.Art.Core;
using HST.Utillity;

namespace HST.Art.Web
{
    public class TeaViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "{0}长度{2}-{1}个字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "教师名不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "教师名不能包含空字符")]
        public string TeacherName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "证书编号不能为空")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "证书编号不能包含空字符")]
        [Remote("CheckTeaNumber", "Teacher", AdditionalFields = "Id", ErrorMessage = "证书编号已存在")]
        public string Number { get; set; }

        public Gender Gender { get; set; }
        public CertificateType Category { get; set; }
        public LevelType Level { get; set; }

        public int Province { get; set; }

        public int City { get; set; }

        public string Area { get; set; }

        public string UserName { get; set; }

        public int State { get; set; }
        public string HeadImg { get; set; }
        public string CreateTime { get; set; }

        public string CategoryName
        {
            get
            {
                switch (Category)
                {
                    case CertificateType.Prize:
                        return "获奖证书";
                    case CertificateType.Train:
                        return "师资认证";
                }

                return string.Empty;
            }
        }

        public string LevelName
        {
            get
            {
                return Level.GetDescription();
            }
        }

        public string GenderName
        {
            get
            {
                return Gender.GetDescription();
            }
        }

    }
}