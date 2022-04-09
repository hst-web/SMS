using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Canve.ESH.Utillity
{
    public class NurseSchedule: DynamicObject
    {
        public string EmpName { get; set; }
        public string TotalHour { get; set; }
        public string TotalWork { get; set; }

        Dictionary<string, object> Properties = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!Properties.Keys.Contains(binder.Name))
            {
                Properties.Add(binder.Name, value.ToString());
            }
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return Properties.TryGetValue(binder.Name, out result);
        }
    }
}
