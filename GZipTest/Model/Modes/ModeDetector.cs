using System.Text.RegularExpressions;

using GZipTest.Model.Interfaces;

namespace GZipTest.Model.Modes
{
    internal class ModeDetector
    {
        public IMode GetMode(string[] args)
        {
            IMode res;
            if (args == null || args.Length == 0 || (args.Length == 1 && Regex.IsMatch(args[0], @"[\/|\-{1,2}](\?|h)", RegexOptions.IgnoreCase)))
            {
                res = new HelpMode();
            }
            else if (args.Length < 2 || args.Length > 3)
            {
                res = new HelpMode(Properties.Resources.ErrInvalidArgs);
            }
            else
            {
                var srcFilePath = args[1];
                var resFilePath = args.Length == 3 ? args[2] : null;

                switch (args[0].Trim())
                {
                    case "compress":
                        res = new CompressMode(srcFilePath, resFilePath);
                        break;
                    case "decompress":
                        res = new DecompressMode(srcFilePath, resFilePath);
                        break;
                    default:
                        res = new HelpMode(Properties.Resources.ErrInvalidArgs);
                        break;
                }
            }
            return res;
        }
    }
}
