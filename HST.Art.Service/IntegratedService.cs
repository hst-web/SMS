/*----------------------------------------------------------------
// 文件名：SettingService.cs
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
    public class IntegratedService : ServiceBase, IIntegratedService
    {
        IntegratedProvider _integratedProvider = new IntegratedProvider();

        public Setting GetSetting(SettingType setType)
        {
            return _integratedProvider.GetSetting(setType);
        }

        public bool UpdateSetting(Setting setInfo)
        {
            //参数验证
            if (setInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _integratedProvider.UpdateSetting(setInfo);
        }

        public bool DeleteSetting(int id)
        {
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }

            return _integratedProvider.DeleteSetting(id);
        }

        public SystemLog GetLog(int id)
        {
            if (id < 1)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return null;
            }

            return _integratedProvider.GetLog(id);
        }

        public List<SystemLog> GetLogPage(LogQuery query, out int totalNum)
        {
            totalNum = 0;
            return _integratedProvider.GetLogPage(query.Equals(null) ? new LogQuery() : query, out totalNum);
        }

        public bool AddLog(SystemLog logInfo)
        {
            //参数验证
            if (logInfo == null)
            {
                ErrorMsg = ErrorCode.ParameterNull;
                return false;
            }
            return _integratedProvider.AddLog(logInfo);
        }
    }
}
