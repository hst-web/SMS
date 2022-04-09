using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Core
{
    public interface IOrganizationService
    {    
        Organization Get(int id);
        Organization GetByNumber(string number);
        List<Organization> GetAll();
        Organization GetChacheData();
        bool Update(Organization orgInfo);
        bool Delete(int id);
    }
}
