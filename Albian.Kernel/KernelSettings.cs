using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albian.Kernel
{
    public class KernelSettings
    {
        private static string _appid = string.Empty;
        public static string AppId
        {
            get { return _appid; }
            set { _appid = value; }
        }

        private static string _appName = string.Empty;
        public static string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        private static string _key = string.Empty;
        public static string Key
        {
            get { return _key; }
            set { _key = value; }
        }
    }
}
