using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_regedit
{
    class Program
    {
        public const string AppPaths = "Software\\深圳市致远速联信息技术有限公司";

        static void Main(string[] args)
        {
            RegistryKey currentKey = null;
            RegistryKey keys = Registry.LocalMachine.OpenSubKey(AppPaths, false);
            foreach (string item in keys.GetSubKeyNames())
            {
                currentKey = keys.OpenSubKey(item);
                var path = currentKey.GetValue("Path");
                var version = currentKey.GetValue("Version");
            }
        }
    }
}
