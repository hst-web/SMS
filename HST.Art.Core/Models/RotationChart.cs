/*----------------------------------------------------------------
// 文件名：RotationChart.cs
// 功能描述：轮播图信息
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace HST.Art.Core
{
    /// <summary>
    /// RotationChart
    /// </summary>
    [Serializable]
    public partial class RotationChart
    {
        public RotationChart()
        { }
        #region Model
        private int _id;
        private string _imgsrc;
        private string _weblink;
        private PublishState _state = 0;
        private RotationType _type;
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
        public string ImgSrc
        {
            set { _imgsrc = value; }
            get { return _imgsrc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WebLink
        {
            set { _weblink = value; }
            get { return _weblink; }
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
        /// 1:banner
        /// 2:logo
        /// </summary>
        public RotationType Type
        {
            set { _type = value; }
            get { return _type; }
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
        #endregion Model
    }

    public class RotationSort
    {
        private List<int> _sortList = new List<int>();
        public RotationType RotationType { get; set; }
        public List<int> SortList
        {
            get
            {
                return _sortList;
            }

            set
            {
                _sortList = value;
            }
        }
    }
}

