using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicQueryTest
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=false)]
    public sealed class QueryFieldAttribute:Attribute
    {
        public QueryFieldAttribute(bool required, object defaultvalue)
        {
            this.IsRequired = required;
            this.DefaultValue = defaultvalue;
        }

        public QueryFieldAttribute(bool required, object defaultvalue, string dataMethodName)
        {
            this.IsRequired = required;
            this.DataMethodName = dataMethodName;
            this.DefaultValue = defaultvalue;
        }

        public bool IsRequired { get; private set; }
        public string DataMethodName { get; private set; }
        public object DefaultValue { get; private set; }
    }
}
