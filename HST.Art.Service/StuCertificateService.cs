/*----------------------------------------------------------------
// 文件名：StuCertificateService.cs
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
    public class StuCertificateService : ServiceBase, IStuCertificateService
    {
        CertificateProvider _certificateProvider = new CertificateProvider();

        public StuCertificate Get(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            StuCertificate stuInfo = _certificateProvider.GetStu(id);
            return stuInfo;
        }

        public List<StuCertificate> GetAll(FilterEntityModel filterModel = null)
        {
            if(filterModel!=null) filterModel.FillWhereTbAsName(Constant.STU_CERTIFICATE_AS_NAME);//筛选器添加表别名
            List<StuCertificate> stuCertificateList = _certificateProvider.GetStuAll(filterModel);
            return stuCertificateList;
        }

        public List<StuCertificate> GetPage(FilterEntityModel filterModel, out int totalNum)
        {
            totalNum = 0;
            //参数验证
            if (filterModel == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            filterModel.FillWhereTbAsName(Constant.STU_CERTIFICATE_AS_NAME);//筛选器添加表别名
            //获取数据
            List<StuCertificate> stuCertificateList = _certificateProvider.GetStuPage(filterModel, out totalNum);

            return stuCertificateList;
        }

        public bool Add(StuCertificate stuInfo)
        {
            //参数验证
            if (stuInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _certificateProvider.AddStu(stuInfo);
        }

        public bool Delete(int id)
        {
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _certificateProvider.DeleteStu(id);
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
                TableName = "StuCertificate"
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
                TableName = "StuCertificate"
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
                TableName = "StuCertificate"
            });
        }

        public bool Update(StuCertificate stuInfo)
        {
            //参数验证
            if (stuInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _certificateProvider.UpdatStu(stuInfo);
        }

        public StuCertificate GetByNumber(string number)
        {
            //参数验证
            if (string.IsNullOrEmpty(number))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            return _certificateProvider.GetStuByNumber(number);
        }

        public bool Add(List<StuCertificate> stuInfos, out List<StuCertificate> failList)
        {
            //参数验证
            if (stuInfos==null||stuInfos.Count<=0)
            {
                failList = null;
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _certificateProvider.AddStu(stuInfos, out failList);
        }
    }
}
