/*----------------------------------------------------------------

// 文件名：RegistryBaseKey.cs
// 功能描述：注册表基项域
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/


namespace HST.Utillity
{
    /// <summary>
    /// 注册表基项域
    /// </summary>
    public enum RegistryBaseKey
    {
        /// <summary>
        /// 对应于HKEY_CLASSES_ROOT主键
        /// </summary>
        ClassesRoot,

        /// <summary>
        /// 对应于HKEY_CURRENT_USER主键
        /// </summary>
        CurrentUser,

        /// <summary>
        /// 对应于 HKEY_LOCAL_MACHINE主键
        /// </summary>
        LocalMachine,

        /// <summary>
        /// 对应于 HKEY_USER主键
        /// </summary>
        Users,

        /// <summary>
        /// 对应于HEKY_CURRENT_CONFIG主键
        /// </summary>
        CurrentConfig
    }
}