/*----------------------------------------------------------------
// 文件名：RotationChartService.cs
// 功能描述：轮播服务
// 创建者：sysmenu
// 创建时间：2019-4-18
//----------------------------------------------------------------*/
using HST.Art.Core;
using System.Collections.Generic;
using System.Linq;
using HST.Art.Data;
using System;
using Newtonsoft.Json;

namespace HST.Art.Service
{
    public class RotationChartService : ServiceBase, IRotationChartService
    {
        RotationChartProvider _rotationChartProvider = new RotationChartProvider();
        IntegratedProvider _intergratedProvider = new IntegratedProvider();

        public RotationChart Get(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            RotationChart rotationChartInfo = _rotationChartProvider.Get(id);
            return rotationChartInfo;
        }

        public List<RotationChart> GetAll(RotationType rotationType)
        {
            FilterEntityModel filterModel = new FilterEntityModel();
            filterModel.SortDict = new KeyValuePair<string, SortType>("Id", SortType.Asc);

            if (rotationType != RotationType.UnKnown)
            {
                filterModel.KeyValueList = new List<KeyValueObj>()
                {
                    new KeyValueObj() {Key="Type",Value=(int)rotationType,FieldType= FieldType.Int }
                };
            }

            List<int> sortList = GetSortList(rotationType);
            List<RotationChart> rotationChartList = _rotationChartProvider.GetAll(filterModel);

            if (rotationChartList != null && rotationChartList.Count > 0 && sortList.Count > 0)
            {
                rotationChartList = rotationChartList.OrderByDescending(x => Enumerable.Reverse(sortList.Select(g => g)).ToList().IndexOf(x.Id)).ToList();
            }

            return rotationChartList;
        }

        private List<int> GetSortList(RotationType type)
        {
            List<RotationSort> sortList = GetRotationSorts();
            List<int> sorts = new List<int>();

            if (sortList != null)
            {
                RotationSort sortModel = sortList.Find(g => g.RotationType == type);

                if (sortModel != null)
                {
                    sorts = sortModel.SortList;
                }
            }

            return sorts;
        }

        public bool Add(RotationChart rotationChartInfo)
        {
            //参数验证
            if (rotationChartInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            bool isSuccess = _rotationChartProvider.Add(rotationChartInfo);

            if (isSuccess)
            {
                try
                {
                    List<RotationSort> sortList = GetRotationSorts();
                    if (sortList.Find(g => g.RotationType == rotationChartInfo.Type) != null)
                    {
                        sortList.Find(g => g.RotationType == rotationChartInfo.Type).SortList.Add(rotationChartInfo.Id);
                    }
                    else
                    {
                        sortList.Add(new RotationSort() { RotationType = rotationChartInfo.Type, SortList = new List<int>() { rotationChartInfo.Id } });
                    }

                    UpdateRotationSort(sortList);
                }
                catch
                {

                }
            }

            return isSuccess;
        }

        public bool Delete(int id)
        {
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            bool isSuccess = _rotationChartProvider.Delete(id);
            if (isSuccess)
            {
                DeleteSort(id);
            }

            return isSuccess;
        }

        private void DeleteSort(int id)
        {
            List<RotationSort> sortList = GetRotationSorts();
            if (sortList.Count <= 0)
            {
                return;
            }

            try
            {
                RotationSort bannerSort = sortList.Find(g => g.RotationType == RotationType.Banner) ?? new RotationSort();
                RotationSort logoSort = sortList.Find(g => g.RotationType == RotationType.Logo) ?? new RotationSort();

                if (bannerSort.SortList.Count > 0)
                {
                    bannerSort.SortList.RemoveAll(g => g.Equals(id));
                }
                if (logoSort.SortList.Count > 0)
                {
                    logoSort.SortList.RemoveAll(g => g.Equals(id));
                }

                UpdateRotationSort(new List<RotationSort>() { bannerSort, logoSort });
            }
            catch
            {

            }

        }

        public bool LogicDelete(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            bool isSuccess = _rotationChartProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "IsDeleted",
                Value = 1,
                TableName = "RotationChart"
            });

            if (isSuccess)
            {
                DeleteSort(id);
            }

            return isSuccess;
        }

        public bool Publish(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _rotationChartProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "State",
                Value = (int)PublishState.Upper,
                TableName = "RotationChart"
            });
        }

        public bool Recovery(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _rotationChartProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "State",
                Value = (int)PublishState.Lower,
                TableName = "RotationChart"
            });
        }

        public bool Update(RotationChart rotationChartInfo)
        {
            //参数验证
            if (rotationChartInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _rotationChartProvider.Update(rotationChartInfo);
        }

        public bool Update(List<RotationChart> rotations)
        {
            if (rotations == null || rotations.Count <= 0)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _rotationChartProvider.Update(rotations);
        }

        public List<RotationSort> GetRotationSorts()
        {
            List<RotationSort> rotationSortList = new List<RotationSort>();
            Setting setInfo = _intergratedProvider.GetSetting(SettingType.Rotation);
            if (setInfo != null && !string.IsNullOrEmpty(setInfo.Val))
            {
                rotationSortList = JsonConvert.DeserializeObject<List<RotationSort>>(setInfo.Val);
            }
            return rotationSortList;
        }

        public bool UpdateRotationSort(List<RotationSort> sortList)
        {
            if (sortList == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            string sortJson = JsonConvert.SerializeObject(sortList);
            return _intergratedProvider.UpdateSettingByType(new Setting() { Val = sortJson, Type = SettingType.Rotation });
        }
    }
}
