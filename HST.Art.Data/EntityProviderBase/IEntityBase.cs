using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HST.Art.Core;

namespace HST.Art.Data
{
    public interface IEntityBase
    {
        bool Update(FlagUpdHandle flagUpdHandle);
    }
}
