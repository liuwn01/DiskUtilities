namespace Defrag
{
    /// <summary>
    /// Constants from WinIOCtl.h
    /// </summary>
    internal class FsConst
    {
        const uint FileDeviceFileSystem = 0x00000009;

        const uint MethodNeither = 3;
        const uint MethodBuffered = 0;

        const uint FileAnyAccess = 0;
        const uint FileSpecialAccess = FileAnyAccess;

        public static uint FsctlGetVolumeBitmap = CtlCode(FileDeviceFileSystem, 27, MethodNeither, FileAnyAccess);
        public static uint FsctlGetRetrievalPointers = CtlCode(FileDeviceFileSystem, 28, MethodNeither, FileAnyAccess);
        public static uint FsctlMoveFile = CtlCode(FileDeviceFileSystem, 29, MethodBuffered, FileSpecialAccess);

        static uint CtlCode(uint deviceType, uint function, uint method, uint access)
        {
            return ((deviceType) << 16) | ((access) << 14) | ((function) << 2) | (method);
        }
    }
}
