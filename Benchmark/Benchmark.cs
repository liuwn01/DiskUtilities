using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Benchmark
{
    public class Benchmark
    {
        #region Const

        private const FileOptions FileFlagNoBuffering = (FileOptions) 0x20000000;

        #endregion

        #region Fields

        private readonly List<string> _createdFiles;
        private readonly Stopwatch _watch;

        public int AmountOfRepeats { get; set; }
        public string LocalDisk { get; set; }
        public FileSize FileSize { get; set; }
        public BenchmarkWay WayToGo { get; set; }

        #endregion

        #region Constructor

        public Benchmark(string localDisk)
        {
            _watch = new Stopwatch();
            _createdFiles = new List<string>();
            LocalDisk = localDisk;
        }

        #endregion

        #region Private methods

        private void Clear()
        {
            foreach (string createdFile in _createdFiles)
            {
                File.Delete(createdFile);
            }

            _createdFiles.Clear();
        }

        private string GetFileName(string disk)
        {
            while (true)
            {
                string randomName = Guid.NewGuid().ToString();
                if (!File.Exists(disk + randomName))
                    return randomName;
            }
        }

        private TestResult SequentialTest()
        {
            var result = new TestResult();
            var fileSize = (int) FileSize;

            byte[] testFile = new byte[fileSize].FillWithRandomValues();
            /* Create file */
            string randomFileName = GetFileName(LocalDisk);
            _createdFiles.Add(LocalDisk + randomFileName);

            /* Write test */
            _watch.Reset();
            _watch.Start();
            using (var fStream = new FileStream(LocalDisk + randomFileName, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, FileOptions.WriteThrough | FileFlagNoBuffering))
            {
                fStream.Write(testFile, 0, fileSize);
            }
            _watch.Stop();
            result.WriteSpeedMBps = (double) fileSize/_watch.ElapsedMilliseconds / 1024 / 1024 * 1000;

            /* Read test */
            _watch.Reset();
            _watch.Start();
            using (var fStream = new FileStream(LocalDisk + randomFileName, FileMode.Open, FileAccess.Read, FileShare.None, 1024 * 1024, FileOptions.WriteThrough | FileFlagNoBuffering))
            {
                fStream.Read(testFile, 0, fileSize);
            }
            _watch.Stop();
            result.ReadSpeedMBps = (double) fileSize / _watch.ElapsedMilliseconds / 1024/ 1024 * 1000;

            /* Delete created file*/
            Clear();

            return result;
        }

        private TestResult BlockTest(BenchmarkWay wayToGo)
        {
            var result = new TestResult();
            var fileSize = (int) FileSize;
            var blockSize = (int) wayToGo;

            byte[] testFile = new byte[blockSize].FillWithRandomValues();
            var iterAmount = (int) Math.Ceiling((double) fileSize / blockSize);
            string randomFileName = GetFileName(LocalDisk);
            _createdFiles.Add(LocalDisk + randomFileName);

            /* Write test */
            _watch.Reset();
            _watch.Start();
            using (var fStream = new FileStream(LocalDisk + randomFileName, FileMode.Create, FileAccess.Write, FileShare.None, blockSize, FileOptions.WriteThrough | FileFlagNoBuffering))
            {
                while (iterAmount > 0)
                {
                    fStream.Write(testFile, 0, blockSize);

                    --iterAmount;
                }
            }
            _watch.Stop();
            result.WriteSpeedMBps = (double) fileSize / _watch.ElapsedMilliseconds / 1024 / 1024 * 1000;

            /* Read test */
            iterAmount = (int) Math.Ceiling((double) fileSize / blockSize);
            _watch.Reset();
            _watch.Start();
            using (var fStream = new FileStream(LocalDisk + randomFileName, FileMode.Open, FileAccess.Read, FileShare.None, blockSize, FileOptions.WriteThrough | FileFlagNoBuffering))
            {
                while (iterAmount > 0)
                {
                    fStream.Read(testFile, 0, blockSize);

                    --iterAmount;
                }
            }
            _watch.Stop();
            result.ReadSpeedMBps = (double) fileSize / _watch.ElapsedMilliseconds / 1024 / 1024 * 1000;

            /* Delete created file*/
            Clear();

            return result;
        }

        #endregion

        public TestResult Run()
        {
            var results = new List<TestResult>();

            int amntOfRpts = AmountOfRepeats;
            while (amntOfRpts > 0)
            {
                switch (WayToGo)
                {
                    case BenchmarkWay.Sequential:
                        {
                            results.Add(SequentialTest());
                        }
                        break;
                    case BenchmarkWay._512K:
                        {
                            results.Add(BlockTest(BenchmarkWay._512K));
                        }
                        break;
                    case BenchmarkWay._4K:
                        {
                            results.Add(BlockTest(BenchmarkWay._4K));
                        }
                        break;
                    default:
                        throw new Exception("unknown Way To Go");
                }
                --amntOfRpts;
            }

            return new TestResult(results.Select(p => p.ReadSpeedMBps).Average(),
                                  results.Select(p => p.WriteSpeedMBps).Average());
        }
    }
}