using System;

using GZipTest.Model.Modes;
using GZipTest.Utilities;

namespace GZipTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            _utility.ExitIfError(() => {
                new ModeDetector()
                    .GetMode(args)
                    .Do();
            }).Invoke();

            Environment.Exit(0);
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            ((Exception)e.ExceptionObject).LogExeption(true);

            Environment.Exit(1);
        }
    }
}
