using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HST.Art.Core
{
    public class FlagUpdHandle
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public FieldType FieldType { get; set; }
    }
}
