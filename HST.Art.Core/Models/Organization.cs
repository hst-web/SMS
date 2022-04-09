/*----------------------------------------------------------------
// 文件名：Organization.cs
// 功能描述：企业组织信息
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
using System;
namespace HST.Art.Core
{
    /// <summary>
    /// Organization
    /// </summary>
    [Serializable]
    public partial class Organization
    {
        public Organization()
        { }
        #region Model
        private int _id;
        private string _name;
        private string _logo;
        private string _telephone;
        private string _email;
        private string _wechat;
        private string _blog;
        private string _description;
        private string _framework;
        private DateTime _createdate = DateTime.Now;
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
        public string Logo
        {
            set { _logo = value; }
            get { return _logo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Telephone
        {
            set { _telephone = value; }
            get { return _telephone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WeChat
        {
            set { _wechat = value; }
            get { return _wechat; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Blog
        {
            set { _blog = value; }
            get { return _blog; }
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
        public string Framework
        {
            set { _framework = value; }
            get { return _framework; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }

        public string Number { get; set; }
        public string Detail { get; set; }
        public string Address { get; set; }
        #endregion Model

    }
}

