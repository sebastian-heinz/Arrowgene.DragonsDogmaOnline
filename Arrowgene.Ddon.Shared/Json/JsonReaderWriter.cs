using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.Json
{
    public class JsonReaderWriter<T> : IAssetDeserializer<T>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(JsonReaderWriter<T>));
        public T ReadPath(string path)
        {
            Logger.Info($"Reading {path}");
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json);
        }
        
        // TODO: WritePath
    }
}