using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class GatheringSpotInfoAssetDeserializer : IAssetDeserializer<GatheringInfoAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(GatheringSpotInfoAssetDeserializer));

        public GatheringInfoAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            GatheringInfoAsset asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            foreach (var jStageId in document.RootElement.EnumerateObject())
            {
                var stageId = UInt32.Parse(jStageId.Name);
                if (!asset.GatheringInfoMap.ContainsKey(stageId))
                {
                    asset.GatheringInfoMap[stageId] = new Dictionary<(uint GroupNo, uint PosId), GatheringSpotInfo>();
                }

                foreach (var jGroup in jStageId.Value.EnumerateArray())
                {
                    var spotInfo = new GatheringSpotInfo()
                    {
                        GroupNo = jGroup.GetProperty("GroupNo").GetUInt32(),
                        PosId = jGroup.GetProperty("PosId").GetUInt32(),
                        GatheringType = (GatheringType) jGroup.GetProperty("GatheringType").GetUInt32(),
                        UintId = jGroup.GetProperty("UnitId").GetUInt32(),
                        Position = (
                            jGroup.GetProperty("Position").GetProperty("x").GetDouble(),
                            jGroup.GetProperty("Position").GetProperty("y").GetDouble(),
                            jGroup.GetProperty("Position").GetProperty("z").GetDouble()
                        )
                    };

                    asset.GatheringInfoMap[stageId][(spotInfo.GroupNo, spotInfo.PosId)] = spotInfo;
                }
            }

            return asset;
        }
    }
}
