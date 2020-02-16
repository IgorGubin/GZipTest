using System;
using System.IO;

using NLog;

namespace GZipTest.Utilities
{
    internal static partial class _extentions
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void LogExeption(this Exception ex, bool isFatal = false)
        {
            var exType = isFatal ? LogLevel.Fatal : LogLevel.Error;
            switch (ex)
            {
            case OutOfMemoryException e1:
                Logger.Log(exType, Properties.Resources.ErrOutOfMemory, e1);
                break;
            case FileNotFoundException e2:
                Logger.Log(exType, Properties.Resources.ErrNotFoundFileOrDirectory, e2);
                break;
            case DirectoryNotFoundException e3:
                Logger.Log(exType, Properties.Resources.ErrNotFoundFileOrDirectory, e3);
                break;
            case DriveNotFoundException e4:
                Logger.Log(exType, Properties.Resources.ErrDriveNotFound, e4);
                break;
            case FileLoadException e5:
                Logger.Log(exType, Properties.Resources.ErrFileLoad, e5);
                break;
            case EndOfStreamException e6:
                Logger.Log(exType, Properties.Resources.ErrEndOfStream, e6);
                break;
            case PathTooLongException e7:
                Logger.Log(exType, Properties.Resources.ErrPathTooLong, e7);
                break;
            case UnauthorizedAccessException e8:
                Logger.Log(exType, Properties.Resources.ErrUnauthorizedAccess, e8);
                break;
            case IOException e9:
                var hresult = e9.HResult & 0x0000FFFF;
                switch (hresult)
                {
                    case 0x20:
                        Logger.Log(exType, Properties.Resources.ErrSharingViolation, e9);
                        break;
                    case 0x87:
                    case 0x50:
                        Logger.Log(exType, Properties.Resources.ErrFileOrDictionaryAlreadyExists, e9);
                        break;
                    case 0x27:
                    case 0x70:
                        Logger.Log(exType, Properties.Resources.ErrNotEnoughDiskSpace, e9);
                        break;
                    default:
                        Logger.Log(exType, string.Format(Properties.Resources.TmplateErrIOHresult, hresult), e9);
                        break;
                }
                break;
            default:
                Logger.Log(exType, ex);
                break;
            }
        }
    }
}
