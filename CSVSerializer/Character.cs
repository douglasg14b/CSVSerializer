using CsvUtilities.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVSerializer
{
    //Holds information regarding a character and if it needs to be replaced or not
    public class Character
    {
        public Character(string name, string value, CsvStrictness replaceStrictness, bool needsCharacterEscaping, bool needsColumnEscaping)
        {
            Name = name;
            Value = value;
            StrictnessToReplace = replaceStrictness;
            NeedsCharacterEscaping = needsCharacterEscaping;
            NeedsColumnEscaping = needsColumnEscaping;
        }

        public string Name { get; }
        public string Value { get; }
        public CsvStrictness StrictnessToReplace { get; }
        public bool NeedsCharacterEscaping { get; }
        public bool NeedsColumnEscaping { get; }
    }
}
