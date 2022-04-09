/*----------------------------------------------------------------

// 文件名：WorkOrderView.cs
// 功能描述：工单视图Model
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HST.Art.Core
{
    /// <summary>
    /// 公有工单视图
    /// </summary>
    public class PublicWorkOrderView : ModelBase
    {
        /// <summary>
        /// 服务空间
        /// </summary>
        public string ServiceSpaceID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(20, ErrorMessage = "名称长度不能超过20个字符")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "不能包含空字符")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(50, ErrorMessage = "长度不能超过50个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 筛选条件
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 显示字段
        /// </summary>
        public List<string> ShowFileds { get; set; }

        /// <summary>
        /// 启用标识
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 系统标识
        /// </summary>
        public bool IsSystemSetted { get; set; }

        /// <summary>
        /// 当前视图工单数量
        /// </summary>
        public int TotalCount { get; set; }

  
    }

    /// <summary>
    /// 私有视图
    /// </summary>
    public class PrivateWorkOrderView : ModelBase
    {
        /// <summary>
        /// 服务空间
        /// </summary>
        public string ServiceSpaceID { get; set; }

        /// <summary>
        /// 员工id
        /// </summary>
        public string StaffID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(20, ErrorMessage = "名称长度不能超过20个字符")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "不能包含空字符")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(50, ErrorMessage = "长度不能超过50个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 筛选条件
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// 显示字段
        /// </summary>
        public List<string> ShowFileds { get; set; }

        /// <summary>
        /// 启用标识
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 当前视图工单数量
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 工单个性化
    /// </summary>
    public class WorkOrderViewProfile
    {
        /// <summary>
        /// 公有视图id集合
        /// </summary>
        public List<string> PublicWorkOrderViewIDList { get; set; }

        /// <summary>
        /// 私有视图id集合
        /// </summary>
        public List<string> PrivateWorkOrderViewIDList { get; set; }
    }
}
