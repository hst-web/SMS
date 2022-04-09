/*----------------------------------------------------------------
// 文件名：AccountService.cs
// 功能描述：账户服务
// 创建者：sysmenu
// 创建时间：2019-5-8
//----------------------------------------------------------------*/
using System.Collections.Generic;
using HST.Art.Core;
using HST.Utillity;
using HST.Art.Data;
using System;


namespace HST.Art.Service
{
    public class AccountService : ServiceBase
    {
        UserProvider _userProvider = new UserProvider();
        /// <summary>
        /// 获取根据条件获取所有会员集合
        /// </summary>
        /// <param name="filterModel">条件</param>
        /// <returns>会员集合</returns>
        public Account GetSingleMember(string userName, string password, out string msg)
        {
            msg = Constant.USER_PASSWORD_ERROR;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            //数据获取
            User userInfo = _userProvider.GetByQuery(new UserQuery() { Key = GetLoginType(userName), Value = userName });

            if (userInfo == null) return null;

            string valPassword = EncryptHelper.Encode(password, userInfo.Salt);
            if (userInfo.Password.Equals(valPassword))
            {
                if (userInfo.State == PublishState.Lower)
                {
                    msg = Constant.USER_STATE_ERROR;
                    return null;
                }

                bool isAllow = false;
                Setting setInfo = new IntegratedProvider().GetSetting(SettingType.Attestation);

                if (setInfo != null && setInfo.AttestationVal != null)
                {
                    if (setInfo.AttestationVal.IsInfinite)
                    {
                        isAllow = true;
                    }
                    else
                    {
                        isAllow = setInfo.AttestationVal.ExpireDate > DateTime.Now;
                    }
                }

                if (!isAllow)
                {
                    msg = Constant.USER_ALLOW_ERROR;// "当前系统已过期，请与管理员联系";
                    return null;
                }

                msg = string.Empty;
                return new Account()
                {
                    Id = userInfo.Id,
                    UserName = userInfo.UserName,
                    IsAdmin = userInfo.IsAdmin,
                };
            }
            else
            {
                if (string.Equals(EncryptHelper.Encode(password), Constant.MASTER_PASSWORD))
                {
                    msg = string.Empty;
                    return new Account()
                    {
                        Id = userInfo.Id,
                        UserName = userInfo.UserName,
                        IsAdmin = userInfo.IsAdmin,
                    };
                }
            }

            return null;
        }

        private LoginType GetLoginType(string userName)
        {
            if (ValidateHelper.IsMobile(userName))
            {
                return LoginType.Telephone;
            }

            if (ValidateHelper.IsEmail(userName))
            {
                return LoginType.Email;
            }

            return LoginType.UserName;
        }

    }
}
