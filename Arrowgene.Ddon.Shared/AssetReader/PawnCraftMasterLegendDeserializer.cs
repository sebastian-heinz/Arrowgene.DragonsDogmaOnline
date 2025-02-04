using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class PawnCraftMasterLegendDeserializer : IAssetDeserializer<List<CDataRegisteredLegendPawnInfo>>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(PawnCraftMasterLegendDeserializer));

        public List<CDataRegisteredLegendPawnInfo> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            List<CDataRegisteredLegendPawnInfo> asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var pawnElements = document.RootElement.EnumerateArray().ToList();
            foreach (var pawn in pawnElements)
            {
                var skillList = new List<CDataPawnCraftSkill>();
                foreach (var skill in pawn.GetProperty("PawnCraftSkillList").EnumerateArray())
                {
                    skillList.Add(new()
                    {
                        Level = skill.GetProperty("Level").GetUInt32(),
                        Type = (CraftSkillType)skill.GetProperty("Type").GetByte()
                    });
                }

                asset.Add(new CDataRegisteredLegendPawnInfo()
                {
                    PawnId = pawn.GetProperty("PawnId").GetUInt32(),
                    PointType = (WalletType)pawn.GetProperty("PointType").GetByte(),
                    RentalCost = pawn.GetProperty("RentalCost").GetUInt32(),
                    Unk3 = pawn.GetProperty("Unk3").GetUInt32(),
                    Name = pawn.GetProperty("Name").GetString(),
                    CraftRank = pawn.GetProperty("CraftRank").GetUInt32(),
                    PawnCraftSkillList = skillList
                });
            }

            return asset;
        }
    }
}
