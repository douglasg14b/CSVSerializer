using CSVSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvUtilities.Writer
{
    public class CsvWriterConfig
    {
        public CsvWriterConfig(){}

        public CsvWriterConfig(CsvStrictness strictness)
        {
            Strictness = strictness;
        }

        public CsvStrictness Strictness { get; private set; } = CsvStrictness.Normal;

        public List<Character> Characters { get; } = new List<Character>()
        {
            new Character("Comma", ",", CsvStrictness.Strict, false, true),
            new Character("Semicolon", ";", CsvStrictness.Strict, false, true),
            new Character("Double Quote", "\"", CsvStrictness.Very_Strict, true, true),
            new Character("Line Break", "\r\n", CsvStrictness.Normal, false, true),
            new Character("New Line", "\n", CsvStrictness.Normal, false, true),
            new Character("Carriage Return", "\r", CsvStrictness.Normal, false, true),
            new Character("Tab", "\t", CsvStrictness.Normal, false, true),
            new Character("Line Seperator", ((char)0x2028).ToString(), CsvStrictness.Normal, false, true),
            new Character("Paragraph Seperator", ((char)0x2029).ToString(), CsvStrictness.Normal, false, true)
        };
    }

    /// <summary>
    /// Csv writing strictness.
    /// Determines the level of formatting safety the writer uses.
    /// The stricter, the more formatting is removed.
    /// </summary>
    public enum CsvStrictness
    {
        /// <summary>
        /// Removes any characters poor CSV parsers may incorrectly parse
        /// </summary>
        Very_Strict = 0,
        /// <summary>
        /// Removes any characters poor CSV parsers may incorrectly parse except double quotes
        /// </summary>
        Strict = 1,
        /// <summary>
        /// Removes line breaks and similar formatting some parsers may incorrectly parse
        /// </summary>
        Normal = 2,
        /// <summary>
        /// Exact adherence to RFC 4180
        /// </summary>
        Loose = 3
    }
}
