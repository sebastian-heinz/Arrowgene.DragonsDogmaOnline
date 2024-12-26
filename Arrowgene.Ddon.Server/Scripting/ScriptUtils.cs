using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class ScriptUtils
    {
        public static ScriptModule FindModule(string path, Dictionary<string, ScriptModule> modules)
        {
            string[] directories = Path.GetDirectoryName(path).Split(Path.DirectorySeparatorChar);

            foreach (var directory in directories.Reverse())
            {
                if (modules.ContainsKey(directory))
                {
                    return modules[directory];
                }
            }

            return null;
        }
    }
}
