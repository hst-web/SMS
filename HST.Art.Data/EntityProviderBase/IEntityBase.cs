using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZT.SMS.Core;

namespace ZT.SMS.Data
{
    public interface IEntityBase
    {
        bool Update(FlagUpdHandle flagUpdHandle);
    }
}
