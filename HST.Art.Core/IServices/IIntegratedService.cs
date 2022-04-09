using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Core
{
    public interface IIntegratedService
    {
        Setting GetSetting(SettingType setType);
        bool UpdateSetting(Setting setInfo);
        bool DeleteSetting(int id);
        SystemLog GetLog(int id);
        List<SystemLog> GetLogPage(LogQuery query, out int totalNum);
        bool AddLog(SystemLog logInfo);
    }
}
