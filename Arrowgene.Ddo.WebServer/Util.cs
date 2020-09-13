using System;
using System.IO;
using System.Reflection;

namespace Arrowgene.Ddo.WebServer
{
    internal static class Util
    {
        /// <summary>
        ///     The directory of the executing assembly.
        ///     This might not be the location where the .dll files are located.
        /// </summary>
        public static string ExecutingDirectory()
        {
            var path = Assembly.GetEntryAssembly().CodeBase;
            var uri = new Uri(path);
            var directory = Path.GetDirectoryName(uri.LocalPath);
            return directory;
        }
    }
}
