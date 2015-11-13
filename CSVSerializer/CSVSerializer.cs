using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSVSerializer
{
    public class CSVSerializer<T>
    {
        /// <summary>
        /// Returns a collection of strings, each a CSV row
        /// </summary>
        /// <param name="input"> Input collection of objects to format</param>
        /// <returns></returns>
        public List<string> GetCSV(ICollection<T> input)
        {
            List<List<string>> dataStrings = GetCSVDataStrings(input);
            List<string> output = new List<string>();

            foreach (List<string> row in dataStrings)
            {
                output.Add(FormatCSVRow(row));
            }
            return output;
        }

        /// <summary>
        /// Returns a collection of strings, each a CSV row. Only returns the collumns you specificed
        /// </summary>
        /// <param name="input"> Input collection of objects to format</param>
        /// <param name="collumns"> List of Column names that matches the names of your properties. Is not case or white space sensative</param>
        /// <returns></returns>
        public List<string> GetCSV(ICollection<T> input, ICollection<string> collumns)
        {
            List<List<string>> dataStrings = GetCSVDataStrings(input, collumns);
            List<string> output = new List<string>();

            foreach (List<string> row in dataStrings)
            {
                output.Add(FormatCSVRow(row));
            }
            return output;
        }

        #region Property Retrieval
        //Retrieves a 2-Dimensional array of strigns that represent the input classes
        private List<List<string>> GetCSVDataStrings(ICollection<T> input, ICollection<string> headers = null)
        {
            List<List<string>> output = new List<List<string>>();
            List<ValidType> properties = FilterProperties(new List<PropertyInfo>(typeof(T).GetProperties()), headers);
            if (properties.Count != 0)
            {
                if (headers == null)
                    output.Add(GetHeaders(properties));
                else
                    output.Add(headers.ToList());

                foreach (T item in input)
                {
                    output.Add(GetDataRowAsStrings(item, properties));
                }
                return output;
            }
            else
                throw new ArgumentException("There was no valid input to format as a CSV");

        }

        //Converts each type T into a list of  based on it's data
        private List<string> GetDataRowAsStrings(T input, List<ValidType> properties)
        {
            List<string> output = new List<string>();
            foreach (ValidType property in properties)
            {
                if (!property.IsCollection)
                {
                    output.Add(MakeStringSafe(CleanString(property.PropertyInformation.GetValue(input).ToString())));
                }
                else
                {
                    output.Add(MakeStringSafe(CleanString(FormatMultiItemCSVCell(GetStringDataFromGenericCollection(property.PropertyInformation, input)))));
                }
            }
            return output;
        }

        //Filters out non accpted Types
        private List<ValidType> FilterProperties(List<PropertyInfo> properties, ICollection<string> columnNames = null)
        {
            List<ValidType> output = new List<ValidType>();
            List<string> cleanNames = new List<string>();
            if (columnNames != null)
                cleanNames = CleanStringOfCaseAndSpace(columnNames).ToList();

            foreach (PropertyInfo property in properties)
            {
                ValidType validType;
                if (columnNames != null) //If there are specific collumn names only add properties that match those names
                {
                    if (PropertyNameExistsInArray(property, cleanNames))
                    {
                        if (CheckForValidProperty(property, out validType))
                            output.Add(validType);
                    }
                }
                else
                {
                    if (CheckForValidProperty(property, out validType))
                        output.Add(validType);
                }
            }

            return output;
        }

        //Checks if the property is of an acceptable type and outputs a ValidType
        private bool CheckForValidProperty(PropertyInfo property, out ValidType validType)
        {
            if (property.PropertyType.IsGenericType)
            {
                Type interfac1e = property.PropertyType.GetInterface(typeof(ICollection<>).Name);
                if (interfac1e != null)
                {
                    if (interfac1e.Name == typeof(ICollection<>).Name)
                    {
                        if (property.PropertyType.GenericTypeArguments[0].IsPrimitive || property.PropertyType.GenericTypeArguments[0] == typeof(string))
                        {
                            validType = new ValidType(true, property);
                            return true;
                        }
                    }
                }
            }
            else if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
            {
                validType = new ValidType(false, property);
                return true;
            }
            validType = null;
            return false;
        }

        //Checks if the property exists in the array, ignores case and whitespaces.
        private bool PropertyNameExistsInArray(PropertyInfo property, ICollection<string> columnNames)
        {
            foreach (string name in columnNames)
            {
                if (string.Compare(property.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
            }
            return false;
        }

        //Cleans a list of strigns of cases and spaces. Used to lower extra load of doing it on the fly for each comparison
        private ICollection<string> CleanStringOfCaseAndSpace(ICollection<string> names)
        {
            List<string> output = new List<string>();
            foreach (string name in names)
            {
                output.Add(name.ToLower().Replace(" ", ""));
            }
            return output;
        }

        //Gets the string headers for each applicable type in T
        private List<string> GetHeaders(List<ValidType> properties)
        {
            List<string> propertyStrings = new List<string>();
            foreach (ValidType validType in properties)
            {
                propertyStrings.Add(validType.PropertyInformation.Name);
            }
            return propertyStrings;
        }

        #endregion

        //Takes a list of strings and formats them in a CSV style
        private string FormatCSVRow(List<string> strings)
        {
            string formatString = "{0}";
            string outputString = "";
            for (int i = 0; i < strings.Count; i++)
            {
                if (i == strings.Count - 1)
                {
                    outputString += string.Format(formatString, strings[i]);
                }
                else
                {
                    outputString += string.Format(formatString + ",", strings[i]);
                }
            }
            return outputString;
        }

        private List<string> GetStringDataFromGenericCollection(PropertyInfo info, T item)
        {
            List<string> output = new List<string>();
            IEnumerable collectionObject = (IEnumerable)info.GetValue(item);
            if (collectionObject != null)
            {
                output = collectionObject.Cast<object>().Select(e => e.ToString()).ToList();
            }
            return output;
        }

        //Takes a list of strings a puts them into a single string that be a single CSV item
        private string FormatMultiItemCSVCell(ICollection<string> input)
        {
            string output = "";
            for (int i = 0; i < input.Count; i++)
            {
                if (i == input.Count - 1)
                {
                    output += input.ElementAt(i);
                }
                else
                {
                    output += input.ElementAt(i) + ", ";
                }
            }
            return output;
        }

        //Cleans a string of any new lines or line breaks
        private string CleanString(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return input.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(lineSeparator, string.Empty).Replace(paragraphSeparator, string.Empty);
        }

        //Encases any comma containing strings in quotes
        private string MakeStringSafe(string input)
        {
            if (input.Contains(","))
                return "\"" + input + "\"";
            return input;

        }
    }
}

