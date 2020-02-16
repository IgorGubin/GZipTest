using System;
using System.Threading;

namespace GZipTest.Utilities
{
    internal static partial class _utility
    {
        public static ThreadStart ExitIfError(Action a)
        {
            var res = new ThreadStart(() => {
                try
                {
                    a.Invoke();
                }
                catch (Exception ex)
                {
                    ex.LogExeption();
                    Environment.Exit(1);
                }
            });
            return res;
        }

        public static void OutProgress(double percent, string info = null)
        {
            Console.Write($"\r{percent:0.00}% {info}");
        }
    }
}
