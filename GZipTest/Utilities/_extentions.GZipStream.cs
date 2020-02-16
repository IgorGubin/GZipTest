using System;
using System.IO;
using System.IO.Compression;

namespace GZipTest.Utilities
{
    /// <summary>
    /// GZipStream class extentions.
    /// </summary>
    /// <remarks>
    /// https://www.ietf.org/rfc/rfc1952.txt
    /// https://www.ietf.org/rfc/rfc1951.txt
    /// </remarks>
    internal static partial class _extentions
    {
        public static byte[] GZipCompress(this byte[] buffer)
        {
            byte[] res;
            using (var compressedMomoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(compressedMomoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(buffer, 0, buffer.Length);
                }
                res = compressedMomoryStream.ToArray();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return res;
        }

        public static byte[] GZipDecompress(this byte[] buffer)
        {
            byte[] res = null;

            using (var decompressedMemoryStream = new MemoryStream())
            {
                using (var compressedMomoryStream = new MemoryStream(buffer))
                {
                    using (var gzipStream = new GZipStream(compressedMomoryStream, CompressionMode.Decompress))
                    {
                        gzipStream.CopyTo(decompressedMemoryStream);
                    }
                }
                res = decompressedMemoryStream.ToArray();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return res;
        }
    }
}
