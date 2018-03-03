using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvUtilities.Writer;

namespace CsvUtilitiesExample
{
    class Program
    {
        static void Main(string[] args)
        {

            PerformanceTest();
            Console.ReadLine();
        }

        public static void PerformanceTest()
        {
            
            /*//prevent the JIT Compiler from optimizing Fkt calls away
            long seed = Environment.TickCount;

            //use the second Core/Processor for the test
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);

            //prevent "Normal" Processes from interrupting Threads
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            //prevent "Normal" Threads from interrupting this thread
            Thread.CurrentThread.Priority = ThreadPriority.Highest;*/
            

            int iterations = 100;
            int size = 10000;
            Stopwatch stopwatch = new Stopwatch();
            List<ExampleClass> testData = new List<ExampleClass>(10000);
            for (int i = 0; i < size; i++)
            {
                testData.Add(new ExampleClass());
            }

            CsvWriter<ExampleClass> writer = new CsvWriter<ExampleClass>();
            writer.GetCSVString(testData);

            for (int i = 0; i < iterations; i++)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(i);

                stopwatch.Start();
                writer.GetCSVString(testData);
                stopwatch.Stop();
            }
            stopwatch.Stop();
            Console.Clear();

            TimeSpan time = stopwatch.Elapsed;
            double perLooop = time.TotalMilliseconds / iterations;
            Console.WriteLine($"Iterations: {iterations}");
            Console.WriteLine($"Total ms: {time.TotalMilliseconds}");
            Console.WriteLine($"ms per loop: {time.TotalMilliseconds / iterations}");

        }
    }

    public class ExampleClass
    {
        public string FirstName { get; set; } = "John";
        public string LastName { get; set; } = "Doe";
        public int Age { get; set; } = 43;
        public DateTime Created = new DateTime(2016, 4, 22);
        public List<string> Words { get; set; } = new List<string>()
        {
            "Jeez",
            "Womble",
            "Black",
            "Zip",
            "Empire"
        };
    }
}
