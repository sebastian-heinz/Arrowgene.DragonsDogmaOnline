using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public class RpcUnwrappedObject
    {
        public RpcInternalCommand Command { get; set; }
        public ushort Origin { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonConverter(typeof(DataJsonConverter))]
        public string Data { get; set; }
        public T GetData<T>()
        {
            return JsonSerializer.Deserialize<T>(Data);
        }

        // Hack to deserialize nested objects.
        internal class DataJsonConverter : JsonConverter<string>
        {
            public override string Read(
                ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using (var jsonDoc = JsonDocument.ParseValue(ref reader))
                {
                    return jsonDoc.RootElement.GetRawText();
                }
            }

            public override void Write(
                Utf8JsonWriter writer, string value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
