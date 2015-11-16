using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVSerialization
{
    /// <summary>
    /// Holds data regarding your custom header text
    /// </summary>
    public class CustomHeader
    {
        public CustomHeader(string headerPropertyName, string headerOutputName)
        {
            HeaderPropertyName = headerPropertyName;
            HeaderOutputName = headerOutputName;
        }
        /// <summary>
        /// The name of your property as it appears in your class
        /// </summary>
        public string HeaderPropertyName { get; set; }
        /// <summary>
        /// The name you wish to see in the output
        /// </summary>
        public string HeaderOutputName { get; set; }
    }
}
