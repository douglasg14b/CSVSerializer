using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSVSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvUtilities.Writer;

namespace CSVSerialization.Tests
{
    [TestClass()]
    public class CSVSerializerTests
    {
        [TestMethod()]
        public void GetCSVString_PrimitiveTest()
        {
            TestData testData = new TestData();

            List<TestData.TestPrimitiveData> dataList = new List<TestData.TestPrimitiveData>()
            {
                new TestData.TestPrimitiveData()
            };

            CsvWriter<TestData.TestPrimitiveData> serializer = new CsvWriter<TestData.TestPrimitiveData>(new CsvWriterConfig(CsvStrictness.Loose));

            string output = serializer.GetCSVString(dataList);

            Assert.AreEqual(output, testData.expectedPrimitiveOutput);
        }

        [TestMethod()]
        public void GetCSVString_ComplexTest()
        {
            TestData testData = new TestData();

            List<TestData.TestComplexData> dataList = new List<TestData.TestComplexData>()
            {
                new TestData.TestComplexData()
            };

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>(new CsvWriterConfig(CsvStrictness.Loose));

            string output = serializer.GetCSVString(dataList);
            serializer.WriteCSV("TestOutput.csv", dataList);

            Assert.AreEqual(output, testData.expectedComplexOutput);
        }

        [TestMethod()]
        public void GetCSVString_ModifiedHeadersTest()
        {
            TestData testData = new TestData();

            List<TestData.TestComplexData> dataList = new List<TestData.TestComplexData>()
            {
                new TestData.TestComplexData()
            };

            List<string> columnNames = new List<string>()
            {
                "Test String",
                "Test Complex String"
            };

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>(new CsvWriterConfig(CsvStrictness.Loose));

            string output = serializer.GetCSVString(dataList, columnNames);
            string expectedOutput = "Test String,Test Complex String\nTest String...,\"Test \"\" Complex ;;;;; \n\"\"\"\"\"\"\"\" string\"\"\\\\  \b  \n  \"\n";

            Assert.AreEqual(output, expectedOutput);
        }

        [TestMethod()]
        public void GetCSVString_CustomHeadersTest()
        {
            TestData testData = new TestData();

            List<TestData.TestComplexData> dataList = new List<TestData.TestComplexData>()
            {
                new TestData.TestComplexData()
            };

            List<CustomHeader> customHeaders = new List<CustomHeader>()
            {
                new CustomHeader("TestString", "Test String Header"),
                new CustomHeader("ListIntTest", "List Int Test Header"),
                new CustomHeader("TestComplexString", "Test Complex String Header")
            };

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>(new CsvWriterConfig(CsvStrictness.Loose));

            string output = serializer.GetCSVString(dataList, customHeaders);
            string expectedOutput = "Test String Header,List Int Test Header,Test Complex String Header\nTest String...,\"1, 2, 3, 4, 5, 6, 7, 8, 9, 10\",\"Test \"\" Complex ;;;;; \n\"\"\"\"\"\"\"\" string\"\"\\\\  \b  \n  \"\n";

            Assert.AreEqual(output, expectedOutput);
        }

        [TestMethod()] //Tests including two non-supported data types through custom headers
        public void GetCSVString_CustomHeaders_IncludeTest()
        {
            TestData testData = new TestData();

            List<TestData.TestComplexData> dataList = new List<TestData.TestComplexData>()
            {
                new TestData.TestComplexData()
            };

            //Includes a non-supported data type and excludes a supported one
            List<CustomHeader> customHeaders = new List<CustomHeader>()
            {
                new CustomHeader("TestString", "Test String Header"),
                new CustomHeader("Structure", "Structure Header"), //Not supported
                new CustomHeader("ListIntTest", "List Int Test Header"),
                new CustomHeader("DateTimeTest", "Date Time Test Header"),
                new CustomHeader("TestComplexString", "Test Complex String Header")
            };

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>(new CsvWriterConfig(CsvStrictness.Loose));

            string output = serializer.GetCSVString(dataList, customHeaders);
            string expectedOutput = "Test String Header,Structure Header,List Int Test Header,Date Time Test Header,Test Complex String Header\nTest String...,CSVSerialization.Tests.TestData+TestStruct,\"1, 2, 3, 4, 5, 6, 7, 8, 9, 10\",11/22/2015 12:00:00 AM,\"Test \"\" Complex ;;;;; \n\"\"\"\"\"\"\"\" string\"\"\\\\  \b  \n  \"\n";

            Assert.AreEqual(output, expectedOutput);
        }

        [TestMethod()] //Tests custom headers excluding a supported type
        public void GetCSVString_CustomHeaders_ExcludeTest()
        {
            TestData testData = new TestData();

            List<TestData.TestComplexData> dataList = new List<TestData.TestComplexData>()
            {
                new TestData.TestComplexData()
            };

            List<CustomHeader> customHeaders = new List<CustomHeader>()
            {
                new CustomHeader("TestString", "Test String Header"),
                new CustomHeader("TestComplexString", "Test Complex String Header")
            };

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>(new CsvWriterConfig(CsvStrictness.Loose));

            string output = serializer.GetCSVString(dataList, customHeaders);
            string expectedOutput = "Test String Header,Test Complex String Header\nTest String...,\"Test \"\" Complex ;;;;; \n\"\"\"\"\"\"\"\" string\"\"\\\\  \b  \n  \"\n";

            Assert.AreEqual(output, expectedOutput);
        }
    }
}