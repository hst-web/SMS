/*----------------------------------------------------------------
// 文件名：KeyValueObj.cs
// 功能描述： 键值对类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/

using System.Collections.Generic;

namespace HST.Art.Core
{
    /// <summary>
    /// key-value对象
    /// </summary>
    public class KeyValueObj
    {
        private string _key = "";
        public string Key
        {
            get
            {
                return _key;
            }

            set
            {
                _key = value;
            }
        }

        private object _value = "";
        public object Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        private string _tbAsName = "";

        public bool IsList
        {
            get
            {
                return (_value != null && (_value.GetType().IsArray || _value.GetType() == typeof(List<string>) || _value.GetType() == typeof(List<int>)));
            }
        }

        private FieldType fieldType = FieldType.String;

        /// <summary>
        /// 例外字段
        /// </summary>
        public bool Exception { set; get; }

        public FieldType FieldType
        {
            get
            {
                return fieldType;
            }

            set
            {
                fieldType = value;
            }
        }

        /// <summary>
        /// 表简称
        /// </summary>
        public string TbAsName
        {
            get
            {
                if (!string.IsNullOrEmpty(_tbAsName) && !_tbAsName.EndsWith("."))
                {
                    _tbAsName = _tbAsName + ".";
                }
                return _tbAsName;
            }

            set
            {
                _tbAsName = value;
            }
        }
    }
}
