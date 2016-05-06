using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvUtilities.Reader
{
    public class CsvReader
    {

        #region Constructors

        public CsvReader(bool hasHeaders)
            : this(hasHeaders, DefaultDelimiter, DefaultQuote, DefaultEscape)
        {
        }

        private CsvReader(bool hasHeaders, char delimiter, char quote, char escape)
        {
            _hasHeaders = hasHeaders;
            _delimiter = delimiter;
            _quote = quote;
            _escape = escape;
            _nextFieldStart = 0;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Defines the default delimiter character separating each field.
        /// </summary>
        public const char DefaultDelimiter = ',';

        /// <summary>
        /// Defines the default quote character wrapping every field.
        /// </summary>
        public const char DefaultQuote = '"';

        /// <summary>
        /// Defines the default escape character letting insert quotation characters inside a quoted field.
        /// </summary>
        public const char DefaultEscape = '"';

        #endregion

        #region Settings

        /// <summary>
        /// Indicates if the CSV data's first row are headers
        /// </summary>
        private bool _hasHeaders;

        /// <summary>
        /// Contains the delimiter for the Csv
        /// </summary>
        private char _delimiter;

        /// <summary>
        /// Contains the quote char for the Csv
        /// </summary>
        private char _quote;

        /// <summary>
        /// Contaisn the escape character for the csv
        /// </summary>
        private char _escape;

        #endregion

        #region State

        /// <summary>
        /// A char array containing the CSV data
        /// </summary>
        private char[] _csvData;

        /// <summary>
        /// Contains the length of the csvData
        /// </summary>
        private long _csvDataLength;

        /// <summary>
        /// The current index of the parser
        /// </summary>
        private long _workingIndex = 0;

        /// <summary>
        /// Contains the next fields starting index
        /// </summary>
        private long _nextFieldStart; //TODO initialize to zero in constructor

        /// <summary>
        /// Contains the index of the next field of that row
        /// </summary>
        private long _nextFieldIndex;
        /// <summary>
        /// Contains an array of field values for the current line
        /// Null indicattes that no fields have been parsed for that row
        /// </summary>
        private string[] _fields;

        /// <summary>
        /// A list of the rows parsed by the reader
        /// </summary>
        private List<string[]> rows;

        /// <summary>
        /// Contains the field count of each row
        /// </summary>
        private long _fieldCount;

        /// <summary>
        /// Indicates if the first line has been read
        /// </summary>
        private bool _firstLineRead = false;

        /// <summary>
        /// Indicates if the last read operation reached an EOL character
        /// </summary>
        private bool _eol;

        #endregion



        public List<List<string>> ReadCsv()
        {       
            string path = @"C:\Users\Douglasg14b\Documents\Visual Studio 2015\Projects\CSVSerializer Tester\TestRead.csv";
            string csvText = System.IO.File.ReadAllText(path);

            List<List<string>> parsedData = new List<List<string>>();
            List<string> row = new List<string>();

            int currentCommaCount = 0;
            int lastColumnIndex = 0;
            for (int i = 0; i < csvText.Length; i++)
            {
                if(csvText[i] == ',')
                {
                    if(currentCommaCount == 0)
                    {
                        row.Add(csvText.Substring(lastColumnIndex, i));
                        lastColumnIndex = i;
                    }
                }
                else if(csvText[i] == '\r' || csvText[i] == '\n')
                {

                }
            }

            return parsedData;
        }

        public void ReadCsv2()
        {
            string path = @"C:\Users\Douglasg14b\Documents\Visual Studio 2015\Projects\CSVSerializer Tester\TestRead.csv";
            string csvText = System.IO.File.ReadAllText(path);
            _csvData = csvText.ToCharArray();
            _csvDataLength = _csvData.LongLength;
            ParseToEnd();
        }

        #region Character Checks

        /// <summary>
        /// Parses a new line delimiter.
        /// </summary>
        /// <param name="position">The starting position of the parsing. Will contain the resulting end position.</param>
        /// <returns></returns>
        /// </exception>
        private bool ParseNewLine(ref int position)
        {
            char c = _csvData[position];

            // Treat \r as new line only if it's not the delimiter
            if (c == '\r' && _delimiter != '\r')
            {
                position++;

                // Skip following \n (if there is one)
                if (position < _csvDataLength)
                {
                    if (_csvData[position] == '\n')
                        position++;
                }
                return true;
            }
            else if (c == '\n')
            {
                position++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the character at the specified position is a new line delimiter.
        /// </summary>
        /// <param name="pos">The position of the character to verify.</param>
        /// <returns>
        /// 	<see langword="true"/> if the character at the specified position is a new line delimiter; otherwise, <see langword="false"/>.
        /// </returns>
        private bool IsNewLine(int pos)
        {
            char c = _csvData[pos];

            if (c == '\n')
                return true;
            else if (c == '\r' && _delimiter != '\r')
                return true;
            else
                return false;
        }

        #endregion
        /// <summary>
        /// Reads the CSV from the beginning to the end
        /// </summary>
        private void ParseToEnd()
        {
            while (ParseNextLine()) ;
        }

        /// <summary>
        /// Parses a single line
        /// </summary>
        /// <returns></returns>
        private bool ParseNextLine()
        {
            if(!_firstLineRead)
            {
                int fieldCount = 0;
                while (ParseField(fieldCount))
                {
                    fieldCount++;
                }

                _fieldCount = fieldCount;
            }
            else
            {

            }

            return true;
        }

        private bool ParseField(int field)
        {
            //Index of the current field start
            long index = field;
            while(index < field + 1)
            {
                string value = String.Empty;
                if(_csvData[_nextFieldStart] != _quote)
                {
                    //Unquoted field
                    long start = _nextFieldStart;
                    long pos = _nextFieldStart;

                    while (pos < _csvDataLength)
                    {
                        char c = _csvData[pos];

                        //If the character is a column delimiter, set the next field start and break out of loop
                        if (c == _delimiter)
                        {
                            _nextFieldStart = pos + 1;
                            break;
                        }
                        else if(c == '\r' || c == '\n') //If char is a new line or carriage return, you have reached the end of the column
                        {
                            _nextFieldStart = pos;
                            _eol = true;
                            break;
                        }
                        else
                        {
                            pos++;
                        }
                    }

                }
                else
                {
                    //Quoted Field
                }
                index++;
            }
            return true;
        }
    }
}
