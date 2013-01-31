using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Defrag
{
    public class Defrag
    {
        #region Const

        private const uint FileShareRead = 0x00000001;
        private const uint FileShareWrite = 0x00000002;
        private const uint OpenExisting = 3;

        private const uint GenericRead = (0x80000000);
        private const uint GenericWrite = (0x40000000);

        private const uint FileReadAttributes = (0x0080);
        private const uint FileWriteAttributes = 0x0100;

        #endregion

        #region WinAPI import

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode,
                                                IntPtr lpSecurityAttributes,
                                                uint dwCreationDisposition, uint dwFlagsAndAttributes,
                                                IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer,
                                                   uint nInBufferSize,
                                                   [Out] IntPtr lpOutBuffer, uint nOutBufferSize,
                                                   ref uint lpBytesReturned, IntPtr lpOverlapped);

        #endregion

        #region Private methods

        private IntPtr OpenVolume(string deviceName)
        {
            IntPtr hDevice = CreateFile(@"\\.\" + deviceName, GenericRead | GenericWrite, FileShareWrite,
                                        IntPtr.Zero,
                                        OpenExisting, 0, IntPtr.Zero);

            if ((int)hDevice == -1)
            {
                throw new Exception(Marshal.GetLastWin32Error().ToString(CultureInfo.InvariantCulture));
            }

            return hDevice;
        }

        private IntPtr OpenFile(string path)
        {
            IntPtr hFile = CreateFile(path, FileReadAttributes | FileWriteAttributes,
                                      FileShareRead | FileShareWrite, IntPtr.Zero,
                                      OpenExisting, 0, IntPtr.Zero);

            if ((int)hFile == -1)
            {
                throw new Exception(Marshal.GetLastWin32Error().ToString(CultureInfo.InvariantCulture));
            }

            return hFile;
        }

        private int FindFreeSpace(BitArray volumeMap, int requiredSpace, int startIndex = 0)
        {
            int index = startIndex;
            int totalFreeSeq = 0;
            while (index < volumeMap.Length)
            {
                if (totalFreeSeq == requiredSpace)
                    return index - requiredSpace;

                if (!volumeMap.Get(index))
                {
                    totalFreeSeq++;
                }
                else
                {
                    totalFreeSeq = 0;
                }

                ++index;
            }

            return -1;
        }

        private int FindRandomFreeCluster(BitArray volumeMap)
        {
            Random rand = new Random();

            while (true)
            {
                int currentCluster = rand.Next(volumeMap.Length);
                if (!volumeMap.Get(currentCluster))
                {
                    return currentCluster;
                }
            }
        }

        #endregion

        /// <summary>
        /// Get volume map for a device.
        /// </summary>
        /// <param name="deviceName">Example: C:</param>
        /// <returns>BitArray for each cluster.</returns>
        public BitArray GetVolumeMap(string deviceName)
        {
            const long i64 = 0;
            IntPtr pAlloc = IntPtr.Zero;
            IntPtr hDevice = IntPtr.Zero;

            try
            {
                hDevice = OpenVolume(deviceName);

                GCHandle handle = GCHandle.Alloc(i64, GCHandleType.Pinned);
                IntPtr p = handle.AddrOfPinnedObject();

                // 64 megs == 67108864 bytes == 536870912 bits == cluster count
                // Max volume size: 2 TB
                const uint q = 1024 * 1024 * 64;

                uint size = 0;
                pAlloc = Marshal.AllocHGlobal((int) q);
                IntPtr pDest = pAlloc;

                bool fResult = DeviceIoControl(
                    hDevice,
                    FsConst.FsctlGetVolumeBitmap,
                    p,
                    (uint) Marshal.SizeOf(i64),
                    pDest,
                    q,
                    ref size,
                    IntPtr.Zero);

                if (!fResult)
                {
                    throw new Exception(Marshal.GetLastWin32Error().ToString(CultureInfo.InvariantCulture));
                }
                handle.Free();

                /*
                 * we've got this structure now
                    struct 
                    {
                        LARGE_INTEGER StartingLcn;
                        LARGE_INTEGER BitmapSize;
                        BYTE Buffer[1];
                    } VOLUME_BITMAP_BUFFER, *PVOLUME_BITMAP_BUFFER;
                */
                Int64 startingLcn = (Int64) Marshal.PtrToStructure(pDest, typeof (Int64));
                if (startingLcn != 0)
                    throw new Exception("GetVolumeMap. Destination is not NULL");

                pDest = (IntPtr) ((Int64) pDest + 8);
                Int64 bitmapSize = (Int64) Marshal.PtrToStructure(pDest, typeof (Int64));

                Int32 byteSize = (int) (bitmapSize/8);
                byteSize++; // round up - even with no remainder

                IntPtr bitmapBegin = (IntPtr) ((Int64) pDest + 8);
                byte[] byteArr = new byte[byteSize];
                Marshal.Copy(bitmapBegin, byteArr, 0, byteSize);

                /* truncate to precise cluster count */
                BitArray retVal = new BitArray(byteArr) { Length = (int)bitmapSize };

                return retVal;
            }
            finally
            {
                CloseHandle(hDevice);
                Marshal.FreeHGlobal(pAlloc);
            }
        }

        /// <summary>
        /// Returns a 2 * number of extents array where every [vcn; lcn]
        /// </summary>
        /// <param name="path">File to get its map.</param>
        /// <returns>An array of [virtual cluster, physical cluster]</returns>
        public long[,] GetFileMap(string path)
        {
            const long i64 = 0;
            IntPtr hFile = IntPtr.Zero;
            IntPtr pAlloc = IntPtr.Zero;

            try
            {
                hFile = OpenFile(path);

                GCHandle handle = GCHandle.Alloc(i64, GCHandleType.Pinned);
                IntPtr p = handle.AddrOfPinnedObject();

                const uint q = 1024 * 1024 * 64;

                uint size = 0;
                pAlloc = Marshal.AllocHGlobal((int) q);
                IntPtr pDest = pAlloc;
                bool fResult = DeviceIoControl(
                    hFile,
                    FsConst.FsctlGetRetrievalPointers,
                    p,
                    (uint) Marshal.SizeOf(i64),
                    pDest,
                    q,
                    ref size,
                    IntPtr.Zero);

                if (!fResult)
                {
                    throw new Exception(Marshal.GetLastWin32Error().ToString(CultureInfo.InvariantCulture));
                }

                handle.Free();

                /*
                 * now we've got:
                    struct RETRIEVAL_POINTERS_BUFFER
                    {  
                        DWORD ExtentCount;  
                        LARGE_INTEGER StartingVcn;  
                        struct
                        {
                            LARGE_INTEGER NextVcn;
                            LARGE_INTEGER Lcn;
                        } Extents[1];
                    } RETRIEVAL_POINTERS_BUFFER, *PRETRIEVAL_POINTERS_BUFFER;
                 */

                Int32 extentCount = (Int32) Marshal.PtrToStructure(pDest, typeof (Int32));
                pDest = (IntPtr) ((Int64) pDest + 4);

                Int64 startingVcn = (Int64) Marshal.PtrToStructure(pDest, typeof (Int64));
                if (startingVcn != 0)
                    throw new Exception("GetFileMap. Destination is not NULL");

                pDest = (IntPtr) ((Int64) pDest + 8);

                // now pDest points at an array of pairs of Int64s.
                Array retVal = Array.CreateInstance(typeof (Int64), new int[] {extentCount, 2});

                for (int i = 0; i < extentCount; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Int64 v = (Int64) Marshal.PtrToStructure(pDest, typeof (Int64));
                        retVal.SetValue(v, new int[] {i, j});
                        pDest = (IntPtr) ((Int64) pDest + 8);
                    }
                }

                /* get normal ints as longs */
                long[,] retArray = new long[retVal.Length / 2, 2];
                for (int i = 0; i < retVal.Length; i++)
                {
                    retArray[i / 2, i % 2] = (long) retVal.GetValue(i / 2, i % 2) >> 32;
                }

                return retArray;
            }
            finally
            {
                CloseHandle(hFile);
                hFile = IntPtr.Zero;

                Marshal.FreeHGlobal(pAlloc);
                pAlloc = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Moves file part using virtual cluster, logical cluster on disk and amount of clusters
        /// </summary>
        /// <param name="path">FileName</param>
        /// <param name="vcn">Virtual cluster</param>
        /// <param name="lcn">Cluster on disk</param>
        /// <param name="count">Amount of clusters</param>
        public void MoveFile(string path, Int64 vcn, Int64 lcn, Int32 count)
        {
            string deviceName = path.Substring(0, 2);

            IntPtr hVol = IntPtr.Zero;
            IntPtr hFile = IntPtr.Zero;
            try
            {
                hVol = OpenVolume(deviceName);

                hFile = OpenFile(path);

                MoveFileData mfd = new MoveFileData
                                       {
                                           hFile = hFile,
                                           StartingVcn = vcn,
                                           StartingLcn = lcn,
                                           ClusterCount = count
                                       };

                GCHandle handle = GCHandle.Alloc(mfd, GCHandleType.Pinned);
                IntPtr p = handle.AddrOfPinnedObject();
                uint bufSize = (uint) Marshal.SizeOf(mfd);
                uint size = 0;

                bool fResult = DeviceIoControl(
                    hVol,
                    FsConst.FsctlMoveFile,
                    p,
                    bufSize,
                    IntPtr.Zero, // no output data from this FSCTL
                    0,
                    ref size,
                    IntPtr.Zero);

                handle.Free();

                if (!fResult)
                {
                    throw new DefragAccessDeniedException(Marshal.GetLastWin32Error().ToString(CultureInfo.InvariantCulture));
                }
            }
            finally
            {
                CloseHandle(hVol);
                CloseHandle(hFile);
            }
        }

        /// <summary>
        /// Defragments file if it's necessary
        /// </summary>
        /// <param name="path">Path to file</param>
        public void DefragmentFile(string path)
        {
            /* is defragmented? */
            string deviceName = path.Substring(0, 2);
            long[,] fileMap = GetFileMap(path);
            if (fileMap.Length == 2)
                return;

            /* find some free space */
            int clustersAmount = (int)(fileMap[fileMap.Length / 2 - 1, 0]);
            BitArray volumeMap = GetVolumeMap(deviceName);
            int freeSpaceIndex = FindFreeSpace(volumeMap, clustersAmount);
            if (freeSpaceIndex < 0)
                throw new Exception("Not enough free space");

            /* move first part */
            try
            {
                MoveFile(path, 0, freeSpaceIndex, (int)(fileMap[0, 0]));
                freeSpaceIndex += (int)(fileMap[0, 0]);
            }
            catch (DefragAccessDeniedException dex)
            {
                freeSpaceIndex = FindFreeSpace(volumeMap, clustersAmount);
                if (freeSpaceIndex < 0)
                    throw new Exception("Not enough free space");

                throw new Exception(dex.Message);
            }

            /* other parts */
            while (fileMap.Length > 2)
            {
                try
                {
                    MoveFile(path, fileMap[0, 0], freeSpaceIndex, (int)(fileMap[1, 0] - fileMap[0, 0]));
                    freeSpaceIndex += (int)(fileMap[1, 0] - fileMap[0, 0]);
                }
                catch (DefragAccessDeniedException dex)
                {
                    freeSpaceIndex = FindFreeSpace(volumeMap, (int)(fileMap[1, 0] - fileMap[0, 0]), freeSpaceIndex);
                    if (freeSpaceIndex < 0)
                        throw new Exception("Not enough free space");

                    throw new Exception(dex.Message);
                }

                fileMap = GetFileMap(path);
            }
        }

        /// <summary>
        /// Fragments file. Should be used for debug or for demo
        /// </summary>
        /// <param name="path">Path to file</param>
        public void FragmentFile(string path)
        {
            Random rand = new Random();
            var fileMap = GetFileMap(path);
            int totalClusters = (int) fileMap[fileMap.Length / 2 - 1, 0];

            /* less than or equal to 20 fragments */
            for (int i = 0; i < 20; i++)
            {
                string deviceName = path.Substring(0, 2);
                BitArray volumeMap = GetVolumeMap(deviceName);

                MoveFile(path, rand.Next(totalClusters), FindRandomFreeCluster(volumeMap), 1);
            }
        }
    }
}
