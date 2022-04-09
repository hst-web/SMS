/*----------------------------------------------------------------
// 文件名：DataEntity.cs
// 功能描述： 数据实体
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.Collections;

namespace HST.Art.Core
{
    /// <summary>
    /// 数据实体类
    /// </summary>
    public class DataEntity : ICollection
    {
        public DataEntity(string tableName)
        {
            _tableName = tableName;
        }

        public DataEntity()
        {
        }

        string _tableName;
        /// <summary>
        /// 数据库表名
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }

        /// <summary>
        /// 数据字段的集合
        /// </summary>
        ArrayList _list = new ArrayList();

        /// <summary>
        /// 数据字段类
        /// </summary>
        public class DataField
        {
            public DataField(string name, Type cType, object value)
            {
                Name = name;
                CType = cType;
                Value = value;
            }
            public DataField(string name, string value)
            {
                Name = name;
                CType = typeof(string);
                Value = value;
            }
            public DataField(string name, int value)
            {
                Name = name;
                CType = typeof(int);
                Value = value;
            }
            public DataField(string name, double value)
            {
                Name = name;
                CType = typeof(double);
                Value = value;
            }
            public DataField(string name, long value)
            {
                Name = name;
                CType = typeof(long);
                Value = value;
            }
            public DataField(string name, float value)
            {
                Name = name;
                CType = typeof(float);
                Value = value;
            }
            public DataField(string name, decimal value)
            {
                Name = name;
                CType = typeof(decimal);
                Value = value;
            }
            public string Name;
            public Type CType;
            public object Value;
        }

        /// <summary>
        /// 添加数据字段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cType"></param>
        /// <param name="value"></param>
        public void Add(string name, Type cType, object value)
        {
            DataField col = new DataField(name, cType, value);
            _list.Add(col);
        }
        public void Add(string name, string value)
        {
            DataField col = new DataField(name, value);
            _list.Add(col);
        }
        public void Add(string name, DateTime dt)
        {
            DataField col = new DataField(name, typeof(DateTime), dt);
            _list.Add(col);
        }
        public void Add(string name, int value)
        {
            DataField col = new DataField(name, value);
            _list.Add(col);
        }
        public void Add(string name, double value)
        {
            DataField col = new DataField(name, value);
            _list.Add(col);
        }
        public void Add(string name, decimal value)
        {
            DataField col = new DataField(name, value);
            _list.Add(col);
        }
        public void Add(string name, float value)
        {
            DataField col = new DataField(name, value);
            _list.Add(col);
        }
        public void Add(string name, long value)
        {
            DataField col = new DataField(name, value);
            _list.Add(col);
        }
        public void Add(string name, bool value)
        {
            DataField col = new DataField(name, typeof(bool), value);
            _list.Add(col);
        }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataField this[int index]
        {
            get { return _list[index] as DataField; }
        }
        public DataField this[string name]
        {
            get
            {
                foreach (DataField c in this)
                {
                    if (name == c.Name)
                        return c;
                }
                return null;
            }
        }

        /// <summary>
        /// 从目标数组的指定索引处开始将整个 System.Collections.ArrayList 复制到兼容的一维 System.Array。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            _list.CopyTo(array, index);
        }

        /// <summary>
        /// 数据字段的数量
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsSynchronized
        {
            get { return _list.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return _list.SyncRoot; }
        }

        /// <summary>
        /// 取得枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

}
