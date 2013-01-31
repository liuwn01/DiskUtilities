using System;

namespace Defrag
{
    /// <summary>
    /// Input structure for use in MoveFile()
    /// </summary>
    internal struct MoveFileData
    {
        public IntPtr hFile;
        public Int64 StartingVcn;
        public Int64 StartingLcn;
        public Int32 ClusterCount;
    }
}
