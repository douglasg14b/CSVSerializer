using CSVSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CsvUtilities.Writer
{
    public class CsvWriter<T>
    {
        public CsvWriter()
        {
            config = new CsvWriterConfig();
        }

        public CsvWriter(CsvWriterConfig config)
        {
            config = this.config;
        }

        CsvWriterConfig config;
        /// <summary>
        /// Formats and Writes a CSV to your path
        /// </summary>
        /// <param name="path">path and name of your file</param>
        /// <param name="input">Input collection of objects to format</param>
        public void WriteCSV(string path, ICollection<T> input)
        {
            string CSVString;
            CSVString = GetCSVString(input);

            StreamWriter writer = new StreamWriter(path);
            writer.Write(CSVString);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Formats and Writes a CSV to your path
        /// </summary>
        /// <param name="path">path and name of your file</param>
        /// <param name="input">Input collection of objects to format</param>
        /// <param name="collumnNames">Collection of Column names that matches the names of your properties. Is not case or white space sensative</param>
        public void WriteCSV(string path, ICollection<T> input, ICollection<string> columnNames)
        {
            string CSVString;
            CSVString = GetCSVString(input, columnNames);

            StreamWriter writer = new StreamWriter(path);
            writer.Write(CSVString);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Formats and Writes a CSV to your path
        /// </summary>
        /// <param name="path">path and name of your file</param>
        /// <param name="input">Input collection of objects to format</param>
        /// <param name="columnNames">Collection of CustomHeaders that specify what you want your headers to be named</param>
        public void WriteCSV(string path, ICollection<T> input, ICollection<CustomHeader> columnNames)
        {
            string CSVString;
            CSVString = GetCSVString(input, columnNames);

            StreamWriter writer = new StreamWriter(path);
            writer.Write(CSVString);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Returns a collection of strings, each a CSV row
        /// </summary>
        /// <param name="input"> Input collection of objects to format</param>
        /// <returns></returns>
        public List<string> GetCSVRows(ICollection<T> input)
        {
            List<List<string>> dataStrings = GetCSVDataStrings(input);
            List<string> output = new List<string>();

            foreach (List<string> row in dataStrings)
            {
                output.Add(FormatCSVRow(row, false));
            }
            return output;
        }

        /// <summary>
        /// Returns a collection of strings, each a CSV row. Only returns the collumns you specificed
        /// </summary>
        /// <param name="input"> Input collection of objects to serialize</param>
        /// <param name="columnNames"> Collection of Column names that matches the names of your properties. Is not case or white space sensative</param>
        /// <returns></returns>
        public List<string> GetCSVRows(ICollection<T> input, ICollection<string> columnNames)
        {
            List<List<string>> dataStrings = GetCSVDataStrings(input, columnNames);
            List<string> output = new List<string>();

            foreach (List<string> row in dataStrings)
            {
                output.Add(FormatCSVRow(row, false));
            }
            return output;
        }

        /// <summary>
        /// Returns a collection of strings, each a CSV row. Only returns the collumns you specificed
        /// </summary>
        /// <param name="input">Input collection of objects to serialize</param>
        /// <param name="columnNames">Collection of CustomHeaders that specify what you want your headers to be named</param>
        /// <returns></returns>
        public List<string> GetCSVRows(ICollection<T> input, ICollection<CustomHeader> columnNames)
        {
            List<List<string>> dataStrings = GetCSVDataStrings(input, columnNames);
            List<string> output = new List<string>();

            foreach (List<string> row in dataStrings)
            {
                output.Add(FormatCSVRow(row, false));
            }
            return output;
        }

        /// <summary>
        /// Returns a single string formatted as a CSV based on your input objects
        /// </summary>
        /// <param name="input">Input collection of objects to serialize</param>
        /// <returns></returns>
        public string GetCSVString(ICollection<T> input)
        {
            List<List<string>> dataStrings = GetCSVDataStrings(input);
            StringBuilder builder = new StringBuilder();
            foreach (List<string> row in dataStrings)
            {
                builder.Append(FormatCSVRow(row, true));        
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns a single string formatted as a CSV based on your input objects
        /// </summary>
        /// <param name="input">Input collection of objects to serialize</param>
        /// <param name="collumnNames">Collection of Column names that matches the names of your properties. Is not case or white space sensative</param>
        /// <returns></returns>
        public string GetCSVString(ICollection<T> input, ICollection<string> columnNames)
        {
            List<List<string>> dataStrings = GetCSVDataStrings(input, columnNames);

            StringBuilder builder = new StringBuilder();
            foreach (List<string> row in dataStrings)
            {
                builder.Append(FormatCSVRow(row, true));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns a single string formatted as a CSV based on your input objects
        /// </summary>
        /// <param name="input">Input collection of objects to serialize</param>
        /// <param name="columnNames">Collection of CustomHeaders that specify what you want your headers to be named</param>
        /// <returns></returns>
        public string GetCSVString(ICollection<T> input, ICollection<CustomHeader> columnNames)
        {
            List<List<string>> dataStrings = GetCSVDataStrings(input, columnNames);

            StringBuilder builder = new StringBuilder();
            foreach (List<string> row in dataStrings)
            {
                builder.Append(FormatCSVRow(row, true));
            }
            return builder.ToString();
        }

        #region Property Retrieval
        //Retrieves a 2-Dimensional array of strigns that represent the input classes
        private List<List<string>> GetCSVDataStrings(ICollection<T> input, ICollection<string> headers = null)
        {
            List<List<string>> output = new List<List<string>>();
            List<string> validHeaders = new List<string>();
            List<ValidTypeInfo> properties;
            if (headers != null)
            {
                List<string> cleanHeaders = CleanStringOfCaseAndSpace(headers).ToList();
                properties = SortProperties(FilterProperties(TypeDescriptor.GetProperties(typeof(T)), cleanHeaders), cleanHeaders);
                validHeaders = FilterColumnHeaders(properties, cleanHeaders, headers);
            }
            else
            {
                properties = FilterProperties(TypeDescriptor.GetProperties(typeof(T)));
            }

            if (properties.Count != 0)
            {
                if (headers == null)
                {
                    output.Add(GetHeaders(properties));
                }
                else
                {
                    output.Add(validHeaders);
                }

                foreach (T item in input)
                {
                    output.Add(GetDataRowAsStrings(item, properties));
                }
                return output;
            }
            else
            {
                throw new ArgumentException("There was no valid input to format as a CSV");
            }
        }

        //Retrieves a 2-dimensional array of strings that represents the input classes, rpeserving the header names specified by the user
        private List<List<string>> GetCSVDataStrings(ICollection<T> input, ICollection<CustomHeader> headers)
        {
            List<List<string>> output = new List<List<string>>();

            List<ValidTypeInfo> properties;
            List<string> cleanHeaders = new List<string>();
            List<string> customHeaders = new List<string>();
            foreach (CustomHeader header in headers)
            {
                cleanHeaders.Add(CleanStringOfCaseAndSpace(header.HeaderPropertyName));
                customHeaders.Add(MakeStringSafe(CleanString(header.HeaderOutputName)));
            }

            //Retrieves and sorts properties that have column names, if the property doesn't have a column name it's filtered out
            properties = SortProperties(FilterProperties(TypeDescriptor.GetProperties(typeof(T)), cleanHeaders), cleanHeaders);

            //Filters out headers that don't have matching properties
            customHeaders = FilterColumnHeaders(properties, headers);

            if (properties.Count != 0)
            {
                output.Add(customHeaders);
                foreach (T item in input)
                {
                    output.Add(GetDataRowAsStrings(item, properties));
                }
                return output;
            }
            else
            {
                throw new ArgumentException("There was no valid input to format as a CSV");
            }



        }


        //Converts each type T into a list of  based on it's data
        private List<string> GetDataRowAsStrings(T input, List<ValidTypeInfo> properties)
        {
            List<string> output = new List<string>();
            foreach (ValidTypeInfo property in properties)
            {
                if (!property.IsCollection)
                {
                    if (property.PropertyInformation.GetValue(input) != null)
                    {
                        output.Add(MakeStringSafe(CleanString(property.PropertyInformation.GetValue(input).ToString())));
                    }
                    else
                    {
                        output.Add("");
                    }
                }
                else
                {
                    if (property.PropertyInformation.GetValue(input) != null)
                    {
                        string workingString = FormatMultiItemCSVCell(GetStringDataFromGenericCollection(property.PropertyInformation, input));
                        output.Add(MakeStringSafe(CleanString(workingString)));
                    }
                    else
                    {
                        output.Add("");
                    }
                }
            }
            return output;
        }

        //Filters out non accpted Types
        private List<ValidTypeInfo> FilterProperties(PropertyDescriptorCollection properties, ICollection<string> cleanColumnNames = null)
        {
            List<ValidTypeInfo> output = new List<ValidTypeInfo>();

            foreach(PropertyDescriptor property in properties)
            {
                ValidTypeInfo validType;
                if (cleanColumnNames != null) //If there are specific collumn names only add properties that match those names
                {
                    if (PropertyNameExistsInArray(property, cleanColumnNames))
                    {
                        if (CheckForValidProperty(property, out validType))
                        {
                            validType.IsForcedTry = false;
                            output.Add(validType);
                        }
                        else
                        {
                            validType.IsForcedTry = true;
                            output.Add(validType);
                        }
                    }
                }
                else
                {
                    if (CheckForValidProperty(property, out validType))
                    {
                        output.Add(validType);
                    }
                }
            }

            return output;
        }

        //Sorts the properties by the collumn name orders
        private List<ValidTypeInfo> SortProperties(List<ValidTypeInfo> properties, ICollection<string> cleanColumnNames)
        {
            foreach (ValidTypeInfo property in properties)
            {
                for (int i = 0; i < cleanColumnNames.Count; i++)
                {
                    if (string.Compare(property.PropertyInformation.Name, cleanColumnNames.ElementAt(i), StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        property.SortOrder = i;
                    }
                }
            }
            List<ValidTypeInfo> output = properties.OrderBy(o => o.SortOrder).ToList();
            return output;
        }

        private List<string> FilterColumnHeaders(List<ValidTypeInfo> properties, ICollection<string> cleanCustomHeaders, ICollection<string> customHeaders)
        {
            List<string> validcolumnNames = new List<string>();
            foreach (ValidTypeInfo property in properties)
            {
                for (int i = 0; i < customHeaders.Count; i++)
                {
                    if (string.Compare(property.PropertyInformation.Name, cleanCustomHeaders.ElementAt(i), StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        validcolumnNames.Add(customHeaders.ElementAt(i));
                    }
                }
            }
            return validcolumnNames;
        }

        private List<string> FilterColumnHeaders(List<ValidTypeInfo> properties, ICollection<CustomHeader> cleanCustomHeaders)
        {
            List<string> validcolumnNames = new List<string>();
            foreach (ValidTypeInfo property in properties)
            {
                foreach (CustomHeader header in cleanCustomHeaders)
                {
                    if (string.Compare(property.PropertyInformation.Name, header.HeaderPropertyName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        validcolumnNames.Add(header.HeaderOutputName);
                    }
                }
            }
            return validcolumnNames;
        }

        //Checks if the property is of an acceptable type and outputs a ValidTypeInfo. Only gets sent properties that exist in the collumn names
        private bool CheckForValidProperty(PropertyDescriptor property, out ValidTypeInfo validType)
        {
            if (property.PropertyType.IsGenericType)
            {
                Type interfaceType = property.PropertyType.GetInterface(typeof(ICollection<>).Name);
                if (interfaceType != null)
                {
                    if (interfaceType.Name == typeof(ICollection<>).Name)
                    {
                        if (property.PropertyType.GenericTypeArguments[0].IsPrimitive || property.PropertyType.GenericTypeArguments[0] == typeof(string))
                        {
                            validType = new ValidTypeInfo(true, property);
                            return true;
                        }
                    }
                }
            }
            else if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
            {
                validType = new ValidTypeInfo(false, property);
                return true;
            }

            validType = new ValidTypeInfo(false, property);
            return false;
        }

        //Checks if the property exists in the array, ignores case and whitespaces.
        private bool PropertyNameExistsInArray(PropertyDescriptor property, ICollection<string> columnNames)
        {
            foreach (string name in columnNames)
            {
                if (string.Compare(property.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
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

        private string CleanStringOfCaseAndSpace(string input)
        {
            return input.ToLower().Replace(" ", "");
        }

        //Gets the string headers for each applicable type in T
        private List<string> GetHeaders(List<ValidTypeInfo> properties)
        {
            List<string> propertyStrings = new List<string>();
            foreach (ValidTypeInfo validType in properties)
            {
                propertyStrings.Add(validType.PropertyInformation.Name);
            }
            return propertyStrings;
        }

        #endregion

        //Takes a list of strings and formats them in a CSV style as a row
        private string FormatCSVRow(List<string> strings, bool lineBreaks)
        {
            string output = string.Join(",", strings);
            if (lineBreaks)
            {
                output += "\n";
            }

            return output;
        }

        private List<string> GetStringDataFromGenericCollection(PropertyDescriptor info, T item)
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
            /*foreach(Character character in config.Characters)
            {
                if(config.Strictness <= character.StrictnessToReplace)
                {

                }
                else
                {

                }
            }*/
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }

            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return input.Replace("\r\n", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace("\t", string.Empty)
                        .Replace(lineSeparator, string.Empty)
                        .Replace(paragraphSeparator, string.Empty);
        }

        private string ReplaceCharacter(string input, string character)
        {
            return input.Replace(character, string.Empty);
        }

        private string EscapeCharacter(string input, string character)
        {
            if(!input.Contains(character))
            {
                return input;
            }

            string output = input;
            int i = 0;
            while((i = output.IndexOf(character, i)) != -1)
            {
                output = output.Insert(i, "\"");
                i += character.Length + 2;
                if(i - 1 >= output.Length)
                {
                    break;
                }
            }

            return output;
        }

        //Encases any comma containing strings in quotes and puts a quote infront of any in-string quote
        private string MakeStringSafe(string input)
        {
            bool containsCommasOrSemicolins = input.Contains(",") || input.Contains(";");
            bool containsQuotes = input.Contains("\"");
            string output = input;

            if (containsQuotes)
            {
                int i = 0;
                while ((i = output.IndexOf('"', i)) != -1)
                {
                    output = output.Insert(i, "\"");
                    i += 2;
                    if (i - 1 >= output.Length)
                    {
                        break;
                    }
                }

                return "\"" + output + "\"";
            }
            else if (containsCommasOrSemicolins)
            {
                return "\"" + output + "\"";
            }


            return output;
        }
    }
}

