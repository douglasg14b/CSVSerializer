using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSVSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvUtilities.Writer;
using System.Diagnostics;
using System.Threading;

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

            CsvWriter<TestData.TestPrimitiveData> serializer = new CsvWriter<TestData.TestPrimitiveData>();

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

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>();

            string output = serializer.GetCSVString(dataList);
            //serializer.WriteCSV("TestOutput.csv", dataList);

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

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>();

            string output = serializer.GetCSVString(dataList, columnNames);

            Assert.AreEqual(output, testData.expectedModifiedHeadersOutput);
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

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>();

            string output = serializer.GetCSVString(dataList, customHeaders);

            Assert.AreEqual(output, testData.expectedCustomHeaderOutput);
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

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>();

            string output = serializer.GetCSVString(dataList, customHeaders);

            Assert.AreEqual(output, testData.expectedComplexInclusiveCustomHeadersOutput);
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

            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>();

            string output = serializer.GetCSVString(dataList, customHeaders);

            Assert.AreEqual(output, testData.expectedComplexExcludeCustomHeadersOutput);
        }

        [TestMethod]
        public void GetCSVString_PerformanceTest()
        {
            //prevent the JIT Compiler from optimizing Fkt calls away
            long seed = Environment.TickCount;

            //use the second Core/Processor for the test
            //Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);

            //prevent "Normal" Processes from interrupting Threads
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            //prevent "Normal" Threads from interrupting this thread
            //Thread.CurrentThread.Priority = ThreadPriority.Highest;

            int items = 10000;
            int iterations = 100;

            Stopwatch stopwatch = new Stopwatch();
            CsvWriter<TestData.TestComplexData> serializer = new CsvWriter<TestData.TestComplexData>();

            TestData testData = new TestData();
            List<TestData.TestComplexData> dataList = new List<TestData.TestComplexData>();
            for(int i = 0; i < items; i++)
            {
                dataList.Add(new TestData.TestComplexData());
            }

            serializer.GetCSVString(dataList); //Warmup
            stopwatch.Start();
            for(int i = 0; i < iterations; i++)
            {
                serializer.GetCSVString(dataList);
            }
            stopwatch.Stop();
            double ms = stopwatch.Elapsed.TotalMilliseconds;
            double msPerIteration = ms / iterations;
            var test = "";

        }
    }
}