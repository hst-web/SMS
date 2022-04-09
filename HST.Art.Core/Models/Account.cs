/*----------------------------------------------------------------
// 文件名：Account.cs
// 功能描述：Account
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
namespace HST.Art.Core
{
    /// <summary>
    /// 账号Model
    /// </summary>
    public class Account
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否为组织管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 是否自动登录
        /// </summary>
        public bool IsAutoLogin { get; set; }
    }
}
