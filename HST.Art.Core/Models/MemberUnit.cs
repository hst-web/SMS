/*----------------------------------------------------------------
// 文件名：MemberUnit.cs
// 功能描述：会员单位
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
using System;
namespace HST.Art.Core
{
    /// <summary>
    /// MemberUnit
    /// </summary>
    [Serializable]
    public partial class MemberUnit
    {
        public MemberUnit()
        { }
        #region Model
        private int _id;
        private string _name;
        private string _headimg;
        private int _star = 1;
        private string _number;
        private PublishState _state;
        private int _category;
        private string _description;
        private string _province;
        private string _city;
        private string _county;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
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
        public int Star
        {
            set { _star = value; }
            get { return _star; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Number
        {
            set { _number = value; }
            get { return _number; }
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
        public int Category
        {
            set { _category = value; }
            get { return _category; }
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
        public string Province
        {
            set { _province = value; }
            get { return _province; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string City
        {
            set { _city = value; }
            get { return _city; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string County
        {
            set { _county = value; }
            get { return _county; }
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
        #endregion Model


        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Synopsis { get; set; }
    }
}

