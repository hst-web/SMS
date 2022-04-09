using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HST.Art.Web
{
    public class ResourcePackageViewModel
    {
        public int Id { get; set; } // id (Primary key)

        [StringLength(20, ErrorMessage = "资料包名称长度不能超过20个字符")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "资料包名不能包含空字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "资料包名称不能为空")]
        [Remote("CheckPackgeName", "ResPackge", AdditionalFields = "Id", ErrorMessage = "资料包名称已存在")]
        public string Name { get; set; } // name (length: 100)
        public int State { get; set; } // state
        public int Sort { get; set; } // sort
        public string UpgradeTime { get; set; } // upgrade_time
        public string CreateDate { get; set; } // create_date
    }
}