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
        /// <summary>
        /// If the type is not supported, but is included as a column header by the user
        /// </summary>
        public bool IsForcedTry { get; set; }
        public PropertyInfo PropertyInformation { get; set; }
        public int SortOrder { get; set; }
    }
}
