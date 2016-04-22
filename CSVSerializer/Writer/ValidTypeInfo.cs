using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsvUtilities.Writer
{
    /// <summary>
    /// Encases internal information regarding the type of a property
    /// </summary>
    internal class ValidTypeInfo
    {
        public ValidTypeInfo(bool isCollection, PropertyDescriptor propertyInfo)
        {
            IsCollection = isCollection;
            PropertyInformation = propertyInfo;
        }

        public bool IsCollection { get; set; }
        /// <summary>
        /// If the type is not supported, but is included as a column header by the user
        /// </summary>
        public bool IsForcedTry { get; set; }
        public PropertyDescriptor PropertyInformation { get; set; }
        public int SortOrder { get; set; }
    }
}
