using System;
using System.Collections.Generic;
using System.Text;

namespace Albian.Foundation
{
    public class PathService
    {
        /// <summary>
        /// 得到配置文件的物理路径
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetFullPath(string path)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            if (basePath.ToLower().IndexOf("\\bin") > 0)
            {
                basePath = basePath.Substring(0, basePath.ToLower().IndexOf("\\bin"));
                basePath = string.Format("{0}\\", basePath);
            }
            return string.Format("{0}\\{1}", basePath, path);
        }
    }
}
