namespace Benchmark
{
    public class TestResult
    {
        #region Fields

        public double ReadSpeedMBps { get; set; }
        public double WriteSpeedMBps { get; set; }

        #endregion

        #region Constructors

        public TestResult() {  }

        public TestResult(double readSpeedMBps, double writeSpeedMBps)
        {
            ReadSpeedMBps = readSpeedMBps;
            WriteSpeedMBps = writeSpeedMBps;
        }

        #endregion
    }
}
