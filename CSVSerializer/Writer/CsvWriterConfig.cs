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
            new Character("Comma", ",", CsvStrictness.Strict),
            new Character("Semicolon", ";", CsvStrictness.Strict),
            new Character("Double Quote", "\"", CsvStrictness.Strict),
            new Character("Line Break", "\r\n", CsvStrictness.Normal),
            new Character("New Line", "\n", CsvStrictness.Normal),
            new Character("Carriage Return", "\r", CsvStrictness.Normal),
            new Character("Tab", "\t", CsvStrictness.Normal),
            new Character("Line Seperator", ((char)0x2028).ToString(), CsvStrictness.Normal),
            new Character("Paragraph Seperator", ((char)0x2029).ToString(), CsvStrictness.Normal),
        };
    }

    /// <summary>
    /// Csv writing strictness.
    /// The more strict, the more formatting is lost in the output.
    /// </summary>
    public enum CsvStrictness
    {
        Strict = 1,
        Normal = 2,
        Loose = 3
    }
}
