/*----------------------------------------------------------------
// 文件名：OrganizationService.cs
// 功能描述：企业服务
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System.Collections.Generic;
using HST.Art.Core;
using HST.Art.Data;
using HST.Utillity;

namespace HST.Art.Service
{
    /// <summary>
    /// 企业服务
    /// </summary>
    public class OrganizationService : ServiceBase, IOrganizationService
    {
        OrganizationProvider _organizationProvider = new OrganizationProvider();

        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Organization Get(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            Organization orgInfo = _organizationProvider.Get(id);
            return orgInfo;
        }

        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public Organization GetByNumber(string number)
        {
            //参数验证
            if (string.IsNullOrEmpty(number))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            Organization orgInfo = _organizationProvider.GetByNumber(number);
            return orgInfo;
        }

        public Organization GetChacheData()
        {
            return CacheHelper.GetOrAddToCache<Organization>(Constant.ORG_CACHE_KEY, () => (GetByNumber(Constant.INIT_ORG_NUMBER)), 60);
        }

        /// <summary>
        /// 获取所有企业信息
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetAll()
        {
            List<Organization> orgrList = _organizationProvider.GetAll();
            return orgrList;
        }

        /// <summary>
        /// 更新企业信息
        /// </summary>
        /// <param name="orgInfo"></param>
        /// <returns></returns>
        public bool Update(Organization orgInfo)
        {
            //参数验证
            if (orgInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            CacheHelper.Remove(Constant.ORG_CACHE_KEY);

            return _organizationProvider.Update(orgInfo);
        }

        /// <summary>
        /// 删除企业信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            //参数验证
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _organizationProvider.Delete(id);
        }
    }
}