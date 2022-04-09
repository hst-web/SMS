using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Core
{
    public interface IMemberUnitService : IBaseService
    {
        List<MemberUnit> GetPage(FilterEntityModel filterModel, out int totalNum);
        List<MemberUnit> GetAll(FilterEntityModel filterModel);
        MemberUnit Get(int id);
        MemberUnit GetByNumber(string number);
        bool Update(MemberUnit memberUnitInfo);
        bool Add(MemberUnit memberUnitInfo);
        bool Add(List<MemberUnit> memberUnitInfos, out List<MemberUnit> failList);
    }
}
