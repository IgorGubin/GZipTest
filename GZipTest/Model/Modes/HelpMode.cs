using System;

using GZipTest.Model.Interfaces;
using GZipTest.Utilities;

namespace GZipTest.Model.Modes
{
    internal class HelpMode : IMode
    {
        private string _msg;
        public HelpMode(string msg = null) {
            _msg = msg;
        }

        public void Do()
        {
            Console.WriteLine(Properties.Resources.HelpHeader);
            Console.WriteLine();

            if (!_msg.IsNullOrEmptyOrwhitespace())
            { 
                Console.WriteLine(_msg);
                Console.WriteLine();
            }

            Console.WriteLine(Properties.Resources.Help);
        }
    }
}
