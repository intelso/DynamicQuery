using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQueryTest
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class QueryMethodAttribute: Attribute
    {
        public QueryMethodAttribute(bool isFixedQuery, string valueMember, string displayMember)
        {
            this.ValueMember = valueMember;
            this.DisplayMember = displayMember;
            this.IsFixedQuery = isFixedQuery;
                
        }

        public string ValueMember { get; private set; }
        public string DisplayMember { get; private set; }
        public bool IsFixedQuery { get; private set; }

        
    }
}
