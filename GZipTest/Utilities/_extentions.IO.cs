using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GZipTest.Utilities
{
    internal static partial class _extentions
    {
        public static string GetFreeFilePath(this string filePath)
        {
            var dirPath = Path.GetDirectoryName(filePath);
            var searchPattern = Path.GetFileNameWithoutExtension(filePath) + "*" + Path.GetExtension(filePath);

            string res = null;
            var indexes = Directory.GetFiles(dirPath, searchPattern).Select(f => f.GetIndex()).OrderBy(i => i).Distinct().ToList();
            if (indexes.Count == 0)
            {
                res = filePath;
            }
            else if (indexes.Count == 1 && indexes[0] == 0)
            {
                res = filePath.GetFilePathWithIndex(1);
            }
            else
            {
                var resIndex = 0;
                var maxIndex = indexes[indexes.Count - 1];
                for (int i = 1; i < maxIndex; i++)
                {
                    if (!indexes.Contains(i))
                    {
                        resIndex = i;
                        break;
                    }
                }

                if (resIndex == 0)
                {
                    resIndex = maxIndex + 1;
                }

                res = filePath.GetFilePathWithIndex(resIndex);
            }
            return res;
        }

        public static int GetIndex(this string filePath)
        {
            var res = 0;
            Match m;
            if ((m = Regex.Match(filePath, @"\(\s*(?<index>\d+)\s*\).\S+$")).Success)
            {
                res = int.TryParse(m.Groups["index"].Value, out int tmp) ? tmp : 0;
            }
            return res;
        }

        public static string GetFilePathWithIndex(this string filePath, int index)
        {
            var ext = Path.GetExtension(filePath);
            var res = Regex.Replace(filePath, $@"(\(\s*\d+\s*\))?{ext}$", $"({index}){ext}");
            return res;
        }
    }
}
