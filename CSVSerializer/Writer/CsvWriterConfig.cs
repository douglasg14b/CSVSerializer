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

        public CsvWriterConfig(CsvStrictness strictness, bool parallel = true)
        {
            Strictness = strictness;
            Parallel = parallel;
        }

        public bool Parallel { get; set; } = true;
        public char EscapeChar = '"';
        public char NewLine = '\n';
        public char CarriageReturn = '\r';

        public CsvStrictness Strictness { get; private set; } = CsvStrictness.Normal;

        public List<Character> Characters { get; } = new List<Character>()
        {
            new Character("Comma", ",", CsvStrictness.Strict),
            new Character("Semicolon", ";", CsvStrictness.Strict),
            new Character("Double Quote", "\"", CsvStrictness.Strict),
            new Character("CR-LF Line Break", "\r\n", CsvStrictness.Normal),
            new Character("POSIX New Line", "\n", CsvStrictness.Normal),
            new Character("Carriage Return", "\r", CsvStrictness.Normal),
            new Character("Tab", "\t", CsvStrictness.Normal),
            new Character("Line Seperator", ((char)0x2028).ToString(), CsvStrictness.Normal),
            new Character("Paragraph Seperator", ((char)0x2029).ToString(), CsvStrictness.Normal),
        };

        public Dictionary<char, bool> GetFlaggedChars(CsvStrictness strictness)
        {
            Dictionary<char, bool> output = new Dictionary<char, bool>();
            for (int i = 0; i < Characters.Count; i++)
            {
                if (!Characters[i].IsChar)
                {
                    continue;
                }

                if(Characters[i].StrictnessToReplace == strictness)
                {
                    output.Add(Characters[i].CharValue, true);
                    continue;
                }
                output.Add(Characters[i].CharValue, false);
            }
            return output;
        }

        public Dictionary<string, string> GetCharsDictionary()
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            for (int i = 0; i < Characters.Count; i++)
            {

                output.Add(Characters[i].Name ,Characters[i].Value);
            }
            return output;
        }

        public HashSet<char> GetCharactersHash(CsvStrictness strictness)
        {
            HashSet<char> hash = new HashSet<char>();
            for(int i = 0; i < Characters.Count; i++)
            {
                if(Characters[i].StrictnessToReplace == strictness)
                {
                    hash.Add(Characters[i].CharValue);
                }
            }
            return hash;
        }
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
