using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benchmark;

namespace Tests
{
    [TestClass]
    public class BenchmarkTests
    {
        [TestMethod]
        public void RunSeqTest()
        {
            Benchmark.Benchmark b = new Benchmark.Benchmark("D:\\");
            b.FileSize = FileSize._10MB;
            b.AmountOfRepeats = 1;
            b.WayToGo = BenchmarkWay.Sequential;

            TestResult result = b.Run();
            Console.WriteLine("Sequential, 10MB:");
            Console.WriteLine("Read: {0} MB/s, Write: {1} MB/s", result.ReadSpeedMBps, result.WriteSpeedMBps);

            b.FileSize = FileSize._100MB;
            result = b.Run();
            Console.WriteLine("Sequential, 100MB:");
            Console.WriteLine("Read: {0} MB/s, Write: {1} MB/s", result.ReadSpeedMBps, result.WriteSpeedMBps);
        }

        [TestMethod]
        public void Run512KTest()
        {
            Benchmark.Benchmark b = new Benchmark.Benchmark("D:\\");
            b.FileSize = FileSize._10MB;
            b.AmountOfRepeats = 1;
            b.WayToGo = BenchmarkWay._512K;

            TestResult result = b.Run();
            Console.WriteLine("512K, 10MB:");
            Console.WriteLine("Read: {0} MB/s, Write: {1} MB/s", result.ReadSpeedMBps, result.WriteSpeedMBps);

            b.FileSize = FileSize._100MB;
            result = b.Run();
            Console.WriteLine("512K, 100MB:");
            Console.WriteLine("Read: {0} MB/s, Write: {1} MB/s", result.ReadSpeedMBps, result.WriteSpeedMBps);
        }

        [TestMethod]
        public void Run4KTest()
        {
            Benchmark.Benchmark b = new Benchmark.Benchmark("D:\\");
            b.FileSize = FileSize._10MB;
            b.AmountOfRepeats = 1;
            b.WayToGo = BenchmarkWay._4K;

            TestResult result = b.Run();
            Console.WriteLine("4K, 10MB:");
            Console.WriteLine("Read: {0} MB/s, Write: {1} MB/s", result.ReadSpeedMBps, result.WriteSpeedMBps);
        }
    }
}
