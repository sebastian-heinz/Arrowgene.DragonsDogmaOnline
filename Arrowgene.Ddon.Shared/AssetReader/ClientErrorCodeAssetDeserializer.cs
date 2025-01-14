using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class ClientErrorCodeAssetDeserializer : IAssetDeserializer<Dictionary<ErrorCode, ClientErrorCode>>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CostExpScalingAssetDeserializer));

        public Dictionary<ErrorCode, ClientErrorCode> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            Dictionary<ErrorCode, ClientErrorCode> asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var errorCodeElements = document.RootElement.EnumerateArray().ToList();
            foreach (var element in errorCodeElements)
            {
                if (!element.TryGetProperty("code", out JsonElement jCode)
                    || !Enum.TryParse(jCode.GetString(), true, out ErrorCode errorCode))
                {
                    continue;
                }

                Dictionary<string, string> messages = new();
                foreach (var property in element.EnumerateObject())
                {
                    if (property.NameEquals("code"))
                    {
                        continue;
                    }
                    messages.Add(property.Name.ToString(), property.Value.ToString());
                }

                asset[errorCode] = new ClientErrorCode()
                {
                    Code = errorCode,
                    Message = messages
                };
            }

            return asset;
        }
    }
}
