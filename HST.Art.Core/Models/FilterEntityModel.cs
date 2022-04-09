using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HST.Art.Core
{
    public class FilterEntityModel
    {
        private FilterType _filterType;
        private string _where = "";
        private string _orderBy = "";
        private string _asOrderBy = "";
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private SortType _defaultSort;
        private Dictionary<string, object> _sqlParList = new Dictionary<string, object>();
        private string _sortTbAsName;
        public List<KeyValueObj> KeyValueReserves { get; set; }
        public List<KeyValueObj> KeyValueList { get; set; }
        public KeyValuePair<string, SortType> SortDict { get; set; }
        public KeyValuePair<string, SortType> ThenDict { get; set; }

        /// <summary>
        /// 筛选器
        /// </summary>
        public FilterType FilterType
        {
            get
            {
                return _filterType;
            }

            set
            {
                _filterType = value;
            }
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }

            set
            {
                _pageIndex = value;
            }
        }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = value;
            }
        }
        public Dictionary<string, object> SqlParList
        {
            get
            {
                return _sqlParList;
            }
        }

        public bool IsHaveSort
        {

            get
            {
                return !string.IsNullOrEmpty(SortDict.Key);
            }
        }

        /// <summary>
        /// Where 条件
        /// </summary>
        public string Where
        {
            get
            {
                StringBuilder sBuilder = new StringBuilder();
                if (KeyValueReserves != null)
                {
                    sBuilder.Append(GetReserveWhereStr(KeyValueReserves));
                }

                if (KeyValueList != null && KeyValueList.Count > 0)
                {
                    foreach (KeyValueObj item in KeyValueList)
                    {
                        ConvertKeyValueObj(item);
                        sBuilder.Append(GetWhereItemStr(item));
                    }
                }

                _where = sBuilder.ToString();
                return _where;
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy
        {
            get
            {
                if (!string.IsNullOrEmpty(SortDict.Key))
                {
                    _orderBy = string.Format(" order by {0} {1}", SortDict.Key, SortDict.Value.ToString());
                    if (!string.IsNullOrEmpty(ThenDict.Key))
                    {
                        _orderBy += string.Format(" ,{0} {1}", ThenDict.Key, ThenDict.Value.ToString());
                    }
                }
                else
                {
                    _orderBy = string.Format(" order by Id {0} ", _defaultSort);
                }

                return _orderBy;
            }

        }

        public string AsOrderBy
        {
            get
            {
                if (!string.IsNullOrEmpty(SortDict.Key) && !string.IsNullOrEmpty(_sortTbAsName))
                {
                    _asOrderBy = string.Format(" order by {2}{0} {1}", SortDict.Key, SortDict.Value.ToString(), _sortTbAsName);
                    if (!string.IsNullOrEmpty(ThenDict.Key))
                    {
                        _asOrderBy += string.Format(" ,{2}{0} {1}", ThenDict.Key, ThenDict.Value.ToString(), _sortTbAsName);
                    }
                }
                else
                {
                    _asOrderBy = string.Format(" order by {0}Id {1} ", _sortTbAsName, _defaultSort);
                }

                return _asOrderBy;
            }

        }

        public string SortTbAsName
        {
            set
            {
                _sortTbAsName = !string.IsNullOrWhiteSpace(value) ? value + "." : value;
            }
        }

        public SortType DefaultSort
        {
            set
            {
                _defaultSort = value;
            }
        }

        private void ConvertKeyValueObj(KeyValueObj item)
        {
            if (item == null) return;
            if (item.IsList && _filterType != FilterType.In)
                throw new Exception("In-filter type is required when a object value is a collection");

            if (item.IsList && item.FieldType == FieldType.String)
            {
                List<string> objList = item.Value as List<string>;
                item.Value = objList.Select(g => g = string.Format("'{0}'", g)).ToList();
            }
        }

        private string GetWhereItemStr(KeyValueObj item)
        {
            StringBuilder sBuilder = new StringBuilder();

            switch (_filterType)
            {
                case FilterType.And:
                    sBuilder.Append(string.Format(" and {2}{0}=@{1} ", item.Key, item.Key, item.TbAsName));
                    FileSqlDic(string.Format("@{0}", item.Key), item.Value);
                    break;
                case FilterType.Or:
                    sBuilder.Append(string.Format(" or {2}{0}=@{1} ", item.Key, item.Key, item.TbAsName));
                    FileSqlDic(string.Format("@{0}", item.Key), item.Value);
                    break;
                case FilterType.In:
                    if (item.IsList)
                    {
                        if (item.FieldType == FieldType.String)
                        {
                            sBuilder.Append(string.Format(" and {2}{0} in ({1}) ", item.Key, string.Join(",", (List<string>)item.Value), item.TbAsName));
                        }
                        else if (item.FieldType == FieldType.Int)
                        {
                            sBuilder.Append(string.Format(" and {2}{0} in ({1}) ", item.Key, string.Join(",", (List<int>)item.Value), item.TbAsName));
                        }
                    }
                    else
                    {
                        sBuilder.Append(string.Format(" and {2}{0}=@{1} ", item.Key, item.Key, item.TbAsName));
                        FileSqlDic(string.Format("@{0}", item.Key), item.Value);
                    }
                    break;
                case FilterType.Like:
                    sBuilder.Append(string.Format(" and {2}{0} like '%{1}%' ", item.Key, item.Value.ToString(), item.TbAsName));
                    break;
                case FilterType.Neq:
                    sBuilder.Append(string.Format(" and {2}{0} <> @{1} ", item.Key, item.Key, item.TbAsName));
                    FileSqlDic(string.Format("@{0}", item.Key), item.Value);
                    break;
                case FilterType.Between:
                    if (item.FieldType == FieldType.Date)
                    {
                        sBuilder.Append(string.Format(" and {2}{0} between {1} ", item.Key, string.Format("'{0}' and '{1}'", Convert.ToDateTime(item.Value).ToShortDateString(), Convert.ToDateTime(item.Value).AddDays(1).ToShortDateString()), item.TbAsName));
                    }
                    else
                    {
                        sBuilder.Append(string.Format(" and {2}{0}=@{1} ", item.Key, item.Key, item.TbAsName));
                        FileSqlDic(string.Format("@{0}", item.Key), item.Value);
                    }
                    break;
                default:
                    break;
            }

            return sBuilder.ToString();
        }

        private string GetReserveWhereStr(List<KeyValueObj> items)
        {
            StringBuilder sBuilder = new StringBuilder();
            foreach (KeyValueObj item in items)
            {
                sBuilder.Append(string.Format(" and {2}{0}=@{1} ", item.Key, item.Key, item.TbAsName));
                FileSqlDic(string.Format("@{0}", item.Key), item.Value);
            }

            return sBuilder.ToString();
        }

        private void FileSqlDic(string key, object value)
        {
            if (_sqlParList.Where(g => g.Key == key).Count() > 0) return;
            _sqlParList.Add(key, value);
        }

        public void FillWhereTbAsName(string tbAsName)
        {
            if (string.IsNullOrWhiteSpace(tbAsName)) return;
            if (KeyValueReserves != null)
            {
                //KeyValueReserve.ForEach.TbAsName = tbAsName;
                KeyValueReserves.ForEach(g => g.TbAsName = tbAsName);
            }

            if (KeyValueList == null || KeyValueList.Count <= 0) return;
            foreach (KeyValueObj item in KeyValueList)
            {
                item.TbAsName = tbAsName;
            }
        }
    }
}
