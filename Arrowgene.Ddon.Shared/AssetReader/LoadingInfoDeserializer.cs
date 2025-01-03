using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class LoadingInfoDeserializer : IAssetDeserializer<List<CDataLoadingInfoSchedule>>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(LoadingInfoDeserializer));

        public List<CDataLoadingInfoSchedule> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            List<CDataLoadingInfoSchedule> asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var loadingInfos = document.RootElement.EnumerateArray().ToList();
            foreach (var info in loadingInfos)
            {
                DateTimeOffset beginTime = DateTimeOffset.MinValue;
                if (info.TryGetProperty("begin_date_time", out JsonElement jBegin))
                {
                    beginTime = info.GetProperty("begin_date_time").GetDateTimeOffset();
                }

                DateTimeOffset endTime = DateTimeOffset.MaxValue;
                if (info.TryGetProperty("end_date_time", out JsonElement jEnd))
                {
                    endTime = info.GetProperty("end_date_time").GetDateTimeOffset();
                }

                asset.Add(new CDataLoadingInfoSchedule()
                {
                    Text = info.GetProperty("text").ToString(),
                    ImageId = info.GetProperty("image_id").GetUInt32(),
                    Priority = info.GetProperty("priority").GetUInt32(),
                    BeginDateTime = beginTime,
                    EndDateTime = endTime,
                });
            }

            return asset;
        }
    }
}
