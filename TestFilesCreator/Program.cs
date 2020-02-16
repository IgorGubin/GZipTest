using System;
using System.IO;
using System.Text;

namespace TestFilesCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateTestFiles(Path.GetFullPath(args[0]));

            Console.Read();
        }

        private static void CreateTestFiles(string dirPath)
        {
            var nulFilePath = Path.Combine(dirPath, "nul_1.txt");
            File.WriteAllText(nulFilePath, "");

            var stepContent = TxtContent();
            var smallFilePath = Path.Combine(dirPath, "sml_1.txt");
            File.WriteAllText(smallFilePath, stepContent);

            var max = 5l << 30;
            var bigFilePath = Path.Combine(dirPath, "big_1.txt");
            using (var fs = File.Open(bigFilePath, FileMode.Create, FileAccess.Write))
            using (TextWriter w = new StreamWriter(fs))
            {
                while (new FileInfo(bigFilePath).Length < max)
                    w.Write(stepContent);
            }

            var rnd = new Random(Guid.NewGuid().GetHashCode());
            var binBinFilePath = Path.Combine(dirPath, "big_1.bin");
            using (var fs = File.Open(binBinFilePath, FileMode.Create, FileAccess.Write))
            {
                while (new FileInfo(binBinFilePath).Length < max)
                    for (int i = 0; i < 1000; i++)
                        fs.WriteByte((byte)rnd.Next(0, 255));
            }

            max = 5l << 20;
            var mcdBinFilePath = Path.Combine(dirPath, "mcd_1.txt");
            using (var fs = File.Open(mcdBinFilePath, FileMode.Create, FileAccess.Write))
            using (TextWriter w = new StreamWriter(fs))
            {
                w.Write(new string('a', (int)max));
            }
        }

        private static string TxtContent()
        {
            var sb = new StringBuilder();

            var index = 1;
            foreach (var l in "abcdefghijklmnopqrstuvwxyz")
            {
                sb.Append($"{index++,2})");
                for (int i = 0; i < 10; i++)
                {
                    sb.Append($" {new string(l, i)}");
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }
    }
}
