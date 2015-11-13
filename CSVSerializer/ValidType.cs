using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSVSerialization
{
    internal class ValidType
    {
        public ValidType(bool isCollection, PropertyInfo propertyInfo)
        {
            IsCollection = isCollection;
            PropertyInformation = propertyInfo;
        }

        public bool IsCollection { get; set; }
        public PropertyInfo PropertyInformation { get; set; }
    }
}
