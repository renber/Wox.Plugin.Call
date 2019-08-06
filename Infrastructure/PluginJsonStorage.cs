using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Wox.Plugin.Call.Infrastructure.Helper;

namespace Wox.Plugin.Call.Infrastructure
{
    class PluginJsonStorage<T> : JsonStrorage<T> where T : new()
    {
        public PluginJsonStorage()
        {
            // C# releated, add python releated below
            var dataType = typeof(T);
            var assemblyName = typeof(T).Assembly.GetName().Name;
            DirectoryPath = Path.Combine(Constant.DataDirectory, DirectoryName, Constant.Plugins, assemblyName);
            Helper.ValidateDirectory(DirectoryPath);

            FilePath = Path.Combine(DirectoryPath, $"{dataType.Name}{FileSuffix}");
        }
    }
}
