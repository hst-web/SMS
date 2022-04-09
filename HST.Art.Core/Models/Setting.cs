/*----------------------------------------------------------------
// 文件名：Setting.cs
// 功能描述：设置信息表
// 创建者：sysmenu
// 创建时间：2019-4-14
//----------------------------------------------------------------*/
using System;
using HST.Utillity;
using Newtonsoft.Json;

namespace HST.Art.Core
{
    /// <summary>
    /// Setting
    /// </summary>
    [Serializable]
    public partial class Setting
    {
        public Setting()
        { }
        #region Model
        private int _id;
        private SettingType _type;
        private string _val;
        private DateTime _createdate = DateTime.Now;
        private bool _isenabled = false;
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
        public SettingType Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Val
        {
            set { _val = value; }
            get { return _val; }
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
        public bool IsEnabled
        {
            set { _isenabled = value; }
            get { return _isenabled; }
        }


        public Attestation AttestationVal
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_val))
                {
                    return JsonConvert.DeserializeObject<Attestation>(EncryptHelper.Decode(_val));
                }

                return null;
            }
        }
        #endregion Model
    }

    [Serializable]
    public class Attestation
    {
        public DateTime ExpireDate { get; set; }
        public bool IsInfinite { get; set; }
    }
}

