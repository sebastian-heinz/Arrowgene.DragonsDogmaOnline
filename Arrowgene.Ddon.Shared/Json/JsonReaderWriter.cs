using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Json
{
    public class JsonReaderWriter<T> : IAssetDeserializer<T>
    {
        public List<T> ReadPath(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
        
        // TODO: WritePath
    }
}