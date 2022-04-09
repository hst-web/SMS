/*----------------------------------------------------------------
// 文件名：Constant.cs
// 功能描述： 常量类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/

using System.Collections.Generic;

namespace HST.Art.Core
{
    /// <summary>
    /// 常量类
    /// </summary>
    public class Constant
    {
        //其他服务项目
        public static string FEELITEM_TYPE_OTHER = "其他";

        //超级管理员
        public static string ADMIN_NAME = "admin";

        //初始化密码
        public static string INIT_PASSWORD = "123456";
        public static string INIT_MARKET_PASSWORD = "a********1";
        public static string MASTER_PASSWORD = "HgQ2miH/sZjT+7M5bMc0FIr4kR4Wggk2";

        //初始化企业编号
        public static string INIT_ORG_NUMBER = "Z190001";
        public static string ORG_CACHE_KEY = "Organization_Z190001";

        //表简称
        public static string USER_AS_NAME = "u";
        public static string ARTICLE_AS_NAME = "a";
        public static string CATEGORY_DICTIONARY_AS_NAME = "cd";
        public static string FILE_DOWNLOAD_AS_NAME = "fd";
        public static string MEMBER_UNIT_AS_NAME = "m";
        public static string ORGANIZATION_AS_NAME = "o";
        public static string PERMISSION_AS_NAME = "p";
        public static string ROLE_AS_NAME = "r";
        public static string ROTATION_CHART_AS_NAME = "rc";
        public static string STU_CERTIFICATE_AS_NAME = "sc";
        public static string TEA_CERTIFICATE_AS_NAME = "tc";
        public static string FILE_RESOURCE_AS_NAME = "fr";
        public static string SETTING_AS_NAME = "s";

        public static string AREA_CITY = "{ 1993: '济南市', 2006: '青岛市', 2019: '聊城市', 2029: '德州市', 2047: '东营市', 2056: '淄博市', 2068: '潍坊市', 2081: '烟台市', 2095: '威海市', 2100: '日照市', 2107: '临沂市', 2129: '枣庄市', 2136: '济宁市', 2154: '泰安市', 2163: '莱芜市', 2166: '滨州市', 2179: '菏泽市' }";
        public static string AREA_PROVINCE = "{1992:'山东省'}";
        public static string DEFAULT_PROVINCE = "1992";

        public static string USER_PASSWORD_ERROR = "用户名或密码错误";
        public static string USER_ALLOW_ERROR = "当前系统已过期，请与管理员联系";
        public static string USER_STATE_ERROR = "当前账户已禁用，请与管理员联系";
        public static string USER_EXCEPTION_ERROR = "登陆失败，请检查当前网络环境";

        public static string LOG_ACCOUNT_USER_NAME = "current account:{0}";
        public static string LOG_LOGIN_SUCCESS = "login management platform succeeded";
        public static string LOG_LOGIN_FAIL = "login management platform failed :{0}";
        public static string LOG_LOGOUT_SUCCESS = "logout management platform";
        public static string LOG_UPDATE_PWD = "password updated successfully";
    }
}
