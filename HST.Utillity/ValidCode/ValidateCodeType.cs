/*----------------------------------------------------------------

// 文件名：ValidateCodeType.cs
// 功能描述： 验证码类型
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/


namespace HST.Utillity
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum ValidateCodeType
    {
        /// <summary>
        /// 纯数值
        /// </summary>
        Number,

        /// <summary>
        /// 数值与字母的组合
        /// </summary>
        NumberAndLetter,

        /// <summary>
        /// 汉字
        /// </summary>
        Hanzi
    }
}