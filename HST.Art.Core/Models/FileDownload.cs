/*----------------------------------------------------------------
// 文件名：FileDownload.cs
// 功能描述：文件下载
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
using System;
namespace HST.Art.Core
{
    /// <summary>
    /// FileDownload
    /// </summary>
    [Serializable]
    public partial class FileDownload
    {
        public FileDownload()
        { }
        #region Model
        private int _id;
        private int _userid;
        private string _name;
        private string _title;
        private int _category;
        private FileFormat _type;
        private string _src;
        private PublishState _state;
        private string _description;
        private string _headimg;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
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
        public int Category
        {
            set { _category = value; }
            get { return _category; }
        }
        /// <summary>
        /// 文件类型(ppt、excel、word、txt等)
        /// </summary>
        public FileFormat Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Src
        {
            set { _src = value; }
            get { return _src; }
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
        public string Description
        {
            set { _description = value; }
            get { return _description; }
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

        public string CategoryName { get; set; }
        public string UserName { get; set; }
        public string Synopsis { get; set; }
        #endregion Model

    }
}

