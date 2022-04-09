using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Core
{
    public interface IBaseService
    {
        bool Delete(int id);
        bool Publish(int id);
        bool Recovery(int id);
        bool LogicDelete(int id);
    }
}
