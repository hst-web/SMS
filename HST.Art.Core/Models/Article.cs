/*----------------------------------------------------------------
// 文件名：Article.cs
// 功能描述：文章Model
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
using System;
using HST.Utillity;
using System.IO;

namespace HST.Art.Core
{
    /// <summary>
    /// Article
    /// </summary>
    [Serializable]
    public partial class Article
    {
        public Article()
        { }
        #region Model
        private int _id;
        private int _userid;
        private string _title;
        private string _headimg;
        private string _content;
        private string _author;
        private SectionType _section;
        private PublishState _state = 0;
        private int _parcategory;
        private int _category;
        private DateTime _updatedate;
        private DateTime _createdate = DateTime.Now;
        private bool _isdeleted = false;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HeadImg
        {
            set { _headimg = value; }
            get { return _headimg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Author
        {
            set { _author = value; }
            get { return _author; }
        }
        /// <summary>
        /// 所属模块
        /// </summary>
        public SectionType Section
        {
            set { _section = value; }
            get { return _section; }
        }
        /// <summary>
        /// 0:下架
        /// 1:上架
        /// </summary>
        public PublishState State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ParCategory
        {
            set { _parcategory = value; }
            get { return _parcategory; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Category
        {
            set { _category = value; }
            get { return _category; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            set { _updatedate = value; }
            get { return _updatedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }

        public string UserName { get; set; }
        public string CategoryName { get; set; }
        public string ParCategoryName { get; set; }
        #endregion Model
        /// <summary>
        /// 简介
        /// </summary>
        public string Synopsis { get; set; }

        public DateTime PublishDate { get; set; }
        public string SmallImg
        {
            get
            {
                if (string.IsNullOrEmpty(HeadImg)) { return string.Empty; }
                string fileName = Path.GetFileName(HeadImg);
                string thumbFileName = "small_" + fileName;
                return HeadImg.Replace(fileName, thumbFileName);
            }
        }
    }

    public class ArticleStatistic
    {
        public SectionType SectionType { get; set; }
        public int SectionCount { get; set; }
        public string SectionName
        {
            get
            {
                return SectionType.GetDescription();
            }
        }
    }
}

