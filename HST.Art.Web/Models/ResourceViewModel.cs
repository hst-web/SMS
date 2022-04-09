using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HST.Art.Core;

namespace HST.Art.Web
{
    public class ResourceViewModel
    {
        public int Id { get; set; } // id (Primary key)

        [StringLength(20, ErrorMessage = "资料名称长度不能超过20个字符")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "资料名不能包含空字符")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "资料名称不能为空")]
        [Remote("CheckResName", "Resource", AdditionalFields = "Id,PackageId,Format", ErrorMessage = "资料名称已存在")]
        public string Title { get; set; } // title (length: 100)
        public int State { get; set; } // state
        public int PackageId { get; set; } // package_id

        public string PackageName { get; set; }
        public int Format { get; set; } // format
        public int FileId { get; set; } // file_id
        public string FileName { get; set; }

        public string FileUrl { get; set; }
        public string MemberTypeName { get; set; }
        public int Original { get; set; } // original
        public int? PvSum { get; set; } // pv_sum
        public int? DownloadSum { get; set; } // download_sum
        [StringLength(200, ErrorMessage = "资料简介不能超过200个字符")]
        public string Description { get; set; } // description (length: 100)
        public string ShowTimeStart { get; set; } // show_time_start
        public string ShowTimeEnd { get; set; } // show_time_end
        public string UpgradeTime { get; set; } // upgrade_time
        public string CreateDate { get; set; } // create_date

        public string ShowTimeRange
        {
            get
            {
                if (!string.IsNullOrEmpty(ShowTimeStart)&&!string.IsNullOrEmpty(ShowTimeEnd))
                    return ShowTimeStart+"至"+ShowTimeEnd;
                else
                    return "一直展示";
            }
        }

        public string FormatStr
        {
            get
            {
                switch ((FileFormat)Format)
                {
                    case FileFormat.Word:
                        return "WORD文档";
                    case FileFormat.PPT:
                        return "PPT文档";
                    case FileFormat.PDF:
                        return "PDF文档";
                    case FileFormat.Video:
                        return "视频";
                    case FileFormat.Audio:
                        return "音频";
                    default:
                        return "未知";
                }
            }
        }

        public List<int> MemberTypeIds { get; set; }

       // public List<MemberTypeViewModel> MemberTypes { get; set; }

       // public ResourceFile ResFile { get; set; }
    }
}