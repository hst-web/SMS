/*----------------------------------------------------------------
// 文件名：TeaCertificateService.cs
// 功能描述：学生服务
// 创建者：sysmenu
// 创建时间：2019-4-18
//----------------------------------------------------------------*/
using HST.Art.Core;
using System.Collections.Generic;
using HST.Art.Data;
using System;

namespace HST.Art.Service
{
    public class TeaCertificateService : ServiceBase, ITeaCertificateService
    {
        CertificateProvider _certificateProvider = new CertificateProvider();

        public TeaCertificate Get(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            TeaCertificate teaInfo = _certificateProvider.GetTea(id);
            return teaInfo;
        }

        public List<TeaCertificate> GetAll(FilterEntityModel filterModel = null)
        {
            if (filterModel != null) filterModel.FillWhereTbAsName(Constant.TEA_CERTIFICATE_AS_NAME);//筛选器添加表别名
            List<TeaCertificate> teaCertificateList = _certificateProvider.GetTeaAll(filterModel);
            return teaCertificateList;
        }

        public List<TeaCertificate> GetPage(FilterEntityModel filterModel, out int totalNum)
        {
            totalNum = 0;
            //参数验证
            if (filterModel == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            filterModel.FillWhereTbAsName(Constant.TEA_CERTIFICATE_AS_NAME);//筛选器添加表别名
            //获取数据
            List<TeaCertificate> teaCertificateList = _certificateProvider.GetTeaPage(filterModel, out totalNum);

            return teaCertificateList;
        }

        public bool Add(TeaCertificate teaInfo)
        {
            //参数验证
            if (teaInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _certificateProvider.AddTea(teaInfo);
        }

        public bool Delete(int id)
        {
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _certificateProvider.DeleteTea(id);
        }

        public bool LogicDelete(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _certificateProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "IsDeleted",
                Value = 1,
                TableName = "TeaCertificate"
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

            return _certificateProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "State",
                Value = (int)PublishState.Upper,
                TableName = "TeaCertificate"
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

            return _certificateProvider.Update(new FlagUpdHandle()
            {
                FieldType = FieldType.Int,
                Id = id,
                Key = "State",
                Value = (int)PublishState.Lower,
                TableName = "TeaCertificate"
            });
        }

        public bool Update(TeaCertificate teaInfo)
        {
            //参数验证
            if (teaInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _certificateProvider.UpdatTea(teaInfo);
        }

        public TeaCertificate GetByNumber(string number)
        {
            //参数验证
            if (string.IsNullOrEmpty(number))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            return _certificateProvider.GetTeaByNumber(number);
        }

        public bool Add(List<TeaCertificate> teaInfos, out List<TeaCertificate> failList)
        {
            if (teaInfos == null || teaInfos.Count <= 0)
            {
                failList = null;
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _certificateProvider.AddTea(teaInfos, out failList);
        }
    }
}
