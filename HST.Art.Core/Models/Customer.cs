/*----------------------------------------------------------------

// 文件名：Customer.cs
// 功能描述：客户Model
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/

using System.ComponentModel.DataAnnotations;

namespace HST.Art.Core
{
    /// <summary>
    /// 客户Model
    /// </summary>
    public class Customer : ModelBase
    {
        /// <summary>
        /// 服务空间id
        /// </summary>
        public string ServiceSpaceID { get; set; }

        /// <summary>
        /// 服务网点Id
        /// </summary>
        public string ServiceNetworkID { get; set; }

        /// <summary>
        /// 客户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string CompanyName { get; set; }
     
        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 标签集合
        /// </summary>
        public string LabelIDs { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 最近服务
        /// </summary>
        public string RecentService { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
       
    }

    /// <summary>
    /// 客户标签
    /// </summary>
    public class CustomerLabel : ModelBase
    {
        /// <summary>
        /// 服务空间id
        /// </summary>
        public string ServiceSpaceID { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        [Required(ErrorMessage = "标签名称不能为空")]
        [StringLength(10, ErrorMessage = "长度不能超过10个字")]
        public string Name { get; set; }
    }
}
