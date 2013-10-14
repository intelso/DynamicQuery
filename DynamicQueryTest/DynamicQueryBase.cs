using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQueryTest
{
    public class DynamicQueryBase
    {
        internal virtual string GetLocalizedString(string key)
        {
            return String.Empty;
        }

        public Dictionary<PropertyInfo, bool> AssignedValues { get; set; }
    }
}
