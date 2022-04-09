/*----------------------------------------------------------------
// 文件名：User.cs
// 功能描述：用户信息表
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace HST.Art.Core
{
    /// <summary>
    /// User
    /// </summary>
    [Serializable]
    public partial class User
    {
        public User()
        { }
        #region Model
        private int _id;
        private string _username;
        private string _password;
        private string _salt;
        private string _name;
        private string _email;
        private string _telephone;
        private string _headimg;
        private bool _isadmin = false;
        private PublishState _state;
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
        /// 用户名
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 盐
        /// </summary>
        public string Salt
        {
            set { _salt = value; }
            get { return _salt; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Telephone
        {
            set { _telephone = value; }
            get { return _telephone; }
        }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImg
        {
            set { _headimg = value; }
            get { return _headimg; }
        }
        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        public bool IsAdmin
        {
            set { _isadmin = value; }
            get { return _isadmin; }
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
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }

        /// <summary>
        /// 角色集合
        /// </summary>
        public List<int> RoleList { get; set; }
        #endregion Model

        public int UserId { get; set; }
        public string Author { get; set; }
    }
}

