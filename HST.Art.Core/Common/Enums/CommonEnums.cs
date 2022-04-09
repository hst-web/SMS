using System.ComponentModel;

namespace HST.Art.Core
{
    public class CommonEnums
    {
    }

    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCode
    {
        //未知
        UnKnown = 0,

        //不允许修改
        NotUpdate = 1,

        //错误的请求
        ErrorReqrest = 400,

        //未授权
        Unauthorized = 401,

        //文件不存在
        NoFile = 404,

        //服务器错误
        ServerError = 500,

        //参数为空
        ParameterNull = 501
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Female = 0,

        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Male = 1
    }

    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileType
    {
        //未知
        UnKnown = 0,
        //文件夹
        Folder = 1,
        //文档
        Document = 2,
        //图片
        Picture = 3,
        //视频
        Video = 4
    }

    public enum FileFormat
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnow = 0,
        /// <summary>
        /// word文档
        /// </summary>
        Word = 1,
        /// <summary>
        /// ppt文档
        /// </summary>
        PPT = 2,
        /// <summary>
        /// pdf文档
        /// </summary>
        PDF = 3,
        /// <summary>
        /// excel表格
        /// </summary>
        XLSX = 4,
        /// <summary>
        /// txt文本文档
        /// </summary>
        TXT = 5,
        /// <summary>
        /// 视频
        /// </summary>
        Video = 6,
        /// <summary>
        /// 音频
        /// </summary>
        Audio = 7,
        Img = 8
    }

    /// <summary>
    /// 统计类型
    /// </summary>
    public enum StatisticType
    {
        //未知
        UnKnown = 0,
        //按日期
        Date = 1,
        //按周
        Week = 2,
        //按月
        Month = 3,
        //按年
        Year = 4,
        //按小时
        Hour = 5,
        //按分钟
        Minute = 6
    }

    public enum PublishState
    {
        /// <summary>
        /// 下架
        /// </summary>
        Lower,
        /// <summary>
        /// 上架
        /// </summary>
        Upper
    }

    public enum LoginType
    {
        [Description("用户名")]
        UserName,
        [Description("手机号")]
        Telephone,
        [Description("邮箱")]
        Email
    }

    public enum FieldType
    {
        String,
        Int,
        Date
    }

    public enum FilterType
    {
        And,
        Or,
        In,
        Like,
        Neq,
        Between
    }

    public enum SortType
    {
        Asc,
        Desc
    }


    public enum SectionType
    {
        UnKnown,
        /// <summary>
        /// 行业资讯
        /// </summary>
        [Description("行业资讯")]
        Industry,
        /// <summary>
        /// 协会活动
        /// </summary>
        [Description("协会活动")]
        Association,
        /// <summary>
        /// 考级中心
        /// </summary>
        [Description("考级中心")]
        Examination,
        /// <summary>
        /// 社会公益
        /// </summary>
        [Description("社会公益")]
        Social
    }

    public enum LevelType
    {
        UnKnown,
        /// <summary>
        /// 普通教师
        /// </summary>
        [Description("普通教师")]
        General,
        /// <summary>
        /// 高级教师
        /// </summary>
        [Description("高级教师")]
        Senior,
        /// <summary>
        /// 特级教师
        /// </summary>
        [Description("特级教师")]
        Super,
        /// <summary>
        /// 金牌教师
        /// </summary>
        [Description("金牌教师")]
        Gold
    }

    public enum CategoryType
    {
        UnKnown,
        /// <summary>
        /// 行业资讯
        /// </summary>
        [Description("行业资讯")]
        Industry,
        /// <summary>
        /// 协会活动
        /// </summary>
        [Description("协会活动")]
        Association,
        /// <summary>
        /// 考级中心
        /// </summary>
        [Description("考级中心")]
        Examination,
        /// <summary>
        /// 社会公益
        /// </summary>
        [Description("社会公益")]
        Social,
        /// <summary>
        /// 会员类别
        /// </summary>
        [Description("协会会员")]
        Member,
        /// <summary>
        /// 下载类别
        /// </summary>
        [Description("下载类别")]
        Download
    }

    /// <summary>
    /// 证书类型
    /// </summary>
    public enum CertificateType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown,
        /// <summary>
        /// 获奖
        /// </summary>
        Prize,
        /// <summary>
        /// 培训
        /// </summary>
        Train
    }

    public enum RotationType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown = 0,
        /// <summary>
        /// 首页Banner
        /// </summary>
        Banner = 1,
        /// <summary>
        /// 首页logo
        /// </summary>
        Logo = 2
    }

    public enum SettingType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown = 0,
        /// <summary>
        /// 认证
        /// </summary>
        Attestation,
        /// <summary>
        /// 轮播
        /// </summary>
        Rotation,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }

    /// <summary>
    /// 日志来源
    /// </summary>
    public enum LogSource
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown = 0,
        /// <summary>
        /// 管理端
        /// </summary>
        Admin = 1,

        /// <summary>
        /// 前端
        /// </summary>
        Web = 2
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UnKnown = 0,
        /// <summary>
        /// 操作日志
        /// </summary>
        Operation = 1,

        /// <summary>
        /// 错误日志
        /// </summary>
        Error = 2
    }
}
