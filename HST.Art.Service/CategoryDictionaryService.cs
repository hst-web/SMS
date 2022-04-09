/*----------------------------------------------------------------
// 文件名：CategoryDictionaryService.cs
// 功能描述：轮播服务
// 创建者：sysmenu
// 创建时间：2019-4-18
//----------------------------------------------------------------*/
using HST.Art.Core;
using System.Collections.Generic;
using System.Linq;
using HST.Art.Data;
using System;

namespace HST.Art.Service
{
    public class CategoryDictionaryService : ServiceBase, ICategoryDictionaryService
    {
        CategoryDictionaryProvider _categoryDictionaryProvider = new CategoryDictionaryProvider();

        public CategoryDictionary Get(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            CategoryDictionary categoryInfo = _categoryDictionaryProvider.Get(id);
            return categoryInfo;
        }

        public List<CategoryDictionary> GetAll(CategoryType categoryType)
        {
            FilterEntityModel filterModel = new FilterEntityModel();
            filterModel.SortDict = new KeyValuePair<string, SortType>("id", SortType.Asc);

            if (categoryType != CategoryType.UnKnown)
            {
                filterModel.KeyValueList = new List<KeyValueObj>()
                {
                    new KeyValueObj() {Key="Type",Value=(int)categoryType,FieldType= FieldType.Int,TbAsName=Constant.CATEGORY_DICTIONARY_AS_NAME }
                };
            }

            List<CategoryDictionary> categoryList = _categoryDictionaryProvider.GetAll(filterModel);
            return categoryList;
        }

        public List<CategoryDictionary> GetAll(List<CategoryType> categoryTypes)
        {
            FilterEntityModel filterModel = new FilterEntityModel();
            filterModel.SortDict = new KeyValuePair<string, SortType>("id", SortType.Asc);
            if (categoryTypes != null && categoryTypes.Count > 0)
            {
                filterModel.FilterType = FilterType.In;
                filterModel.KeyValueList = new List<KeyValueObj>();
                List<int> categorys = categoryTypes.Select(g => (int)g).ToList();

                filterModel.KeyValueList.Add(new KeyValueObj() { Key = "Type", Value = categorys, FieldType = FieldType.Int, TbAsName = Constant.CATEGORY_DICTIONARY_AS_NAME });
            }

            List<CategoryDictionary> categoryList = _categoryDictionaryProvider.GetAll(filterModel);
            return categoryList;
        }

        public bool Add(CategoryDictionary categoryInfo)
        {
            //参数验证
            if (categoryInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _categoryDictionaryProvider.Add(categoryInfo);
        }

        public bool Delete(int id)
        {
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _categoryDictionaryProvider.Delete(id);
        }

        public bool LogicDelete(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _categoryDictionaryProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "IsDeleted",
                Value = 1,
                TableName = "CategoryDictionary"
            });
        }

        public bool Publish(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _categoryDictionaryProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "State",
                Value = (int)PublishState.Upper,
                TableName = "CategoryDictionary"
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

            return _categoryDictionaryProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "State",
                Value = (int)PublishState.Lower,
                TableName = "CategoryDictionary"
            });
        }

        public bool Update(CategoryDictionary categoryInfo)
        {
            //参数验证
            if (categoryInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _categoryDictionaryProvider.Update(categoryInfo);
        }

        public List<int> GetCategorysByPartentId(int partentId)
        {
            if (partentId<=0)
            {
                return null;
            }

            return _categoryDictionaryProvider.GetCategorysByPartentId(partentId);
        }
    }
}
