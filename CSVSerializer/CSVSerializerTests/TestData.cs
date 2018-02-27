using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/**************************************************
    A known good dataset in which to test
 **************************************************/
namespace CSVSerialization.Tests
{
    public class TestData
    {

        public string expectedPrimitiveOutput = "TestBool,TestShort,TestUShort,TestInt,TestUInt,TestLong,TestULong,TestFloat,TestDouble\n\"True\",\"32767\",\"65535\",\"2147483647\",\"4294967295\",\"9223372036854775807\",\"18446744073709551615\",\"3.402823E+38\",\"1.79769313486232E+308\"\n";
        public string expectedComplexOutput = "TestString,TestComplexString,ListIntTest\n\"Test String...\",\"Word POSIX\n CR-LF\r\n Quote\"\"\",\"1,2,3,4,5,6,7,8,9,10\"\n";
        public string expectedModifiedHeadersOutput = "Test String,Test Complex String\n\"Test String...\",\"Word POSIX\n CR-LF\r\n Quote\"\"\"\n";
        public string expectedCustomHeaderOutput = "Test String Header,List Int Test Header,Test Complex String Header\n\"Test String...\",\"1,2,3,4,5,6,7,8,9,10\",\"Word POSIX\n CR-LF\r\n Quote\"\"\"\n";
        public string expectedComplexExcludeCustomHeadersOutput = "Test String Header,Test Complex String Header\n\"Test String...\",\"Word POSIX\n CR-LF\r\n Quote\"\"\"\n";
        public string expectedComplexInclusiveCustomHeadersOutput = "Test String Header,Structure Header,List Int Test Header,Date Time Test Header,Test Complex String Header\n\"Test String...\",\"CSVSerialization.Tests.TestData+TestStruct\",\"1,2,3,4,5,6,7,8,9,10\",\"11/22/2015 12:00:00 AM\",\"Word POSIX\n CR-LF\r\n Quote\"\"\"\n";
        //public string expectedComplexCustomHeadersOutput = "Test String Header,Structure Header,List Int Test Header,Date Time Test Header,Test Complex String Header\n\"Test String...\",\"CSVSerialization.Tests.TestData+TestStruct\",\"1,2,3,4,5,6,7,8,9,10\"\n";

        public class TestPrimitiveData
        {
            public bool TestBool { get; set; } = true;

            public short TestShort { get; set; } = short.MaxValue;
            public ushort TestUShort { get; set; } = ushort.MaxValue;
            public int TestInt { get; set; } = int.MaxValue;
            public uint TestUInt { get; set; } = uint.MaxValue;
            public long TestLong { get; set; } = long.MaxValue;
            public ulong TestULong { get; set; } = ulong.MaxValue;

            public float TestFloat { get; set; } = float.MaxValue;
            public double TestDouble { get; set; } = double.MaxValue;
            public decimal TestDecimal { get; set; } = decimal.MaxValue;
        }

        public class TestComplexData
        {
            public DateTime DateTimeTest { get; set; } = new DateTime(2015, 11, 22);
            public string TestString { get; set; } = "Test String...";
            public string TestComplexString { get; set; } = "Word POSIX\n CR-LF\r\n Quote\"";
            public TestStruct Structure { get; set; } = new TestStruct();
            public List<int> ListIntTest { get; set; } = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        public struct TestStruct
        {
            public short TestShort { get { return short.MaxValue; }}
        }
    }
}
