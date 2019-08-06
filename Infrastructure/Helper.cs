using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wox.Plugin.Call.Infrastructure
{
    static class Helper
    {
        /// <summary>
        /// http://www.yinwang.org/blog-cn/2015/11/21/programming-philosophy
        /// </summary>
        public static T NonNull<T>(this T obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return obj;
            }
        }

        public static void ValidateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static class Constant
        {
            public const string Wox = "Wox";
            public const string Plugins = "Plugins";
            public static readonly string DataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Wox);
        }
    }
}
