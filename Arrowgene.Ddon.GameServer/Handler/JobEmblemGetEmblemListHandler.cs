using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobEmblemGetEmblemListHandler : GameRequestPacketHandler<C2SJobEmblemGetEmblemListReq, S2CJobEmblemGetEmblemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobEmblemGetEmblemListHandler));


        public JobEmblemGetEmblemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobEmblemGetEmblemListRes Handle(GameClient client, C2SJobEmblemGetEmblemListReq request)
        {
            var result = new S2CJobEmblemGetEmblemListRes();

            foreach (var (jobId, emblemData) in client.Character.JobEmblems)
            {
                foreach (var uid in emblemData.UIDs)
                {
                    var item = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, uid).Item2.Item2;
                    result.JobEmblemList.Add(new CDataJobEmblem()
                    {
                        Job = jobId,
                        EmblemStatList = emblemData.GetEmblemStatParamList(),
                        EquipElementParamList = item.EquipElementParamList,
                    });

                    result.EmblemPointList.Add(new CDataJobEmblemPoints()
                    {
                        JobId = jobId,
                        Amount = Server.JobEmblemManager.GetAvailableEmblemPoints(emblemData),
                        MaxAmount = Server.JobEmblemManager.MaxEmblemPointsForLevel(emblemData),
                    });
                }
            }

            result.EmblemSettings.LevelingDataList = Server.GameSettings.EmblemSettings.LevelingData
                .Select(x => new CDataJobEmblemLevelData()
                {
                    Level = x.Level,
                    PPAmount = x.PPCost,
                    EPGain = x.EP,
                }).ToList();

            result.EmblemSettings.StatCostList = Server.GameSettings.EmblemSettings.StatUpgradeCost
                .Select(x => new CDataJobEmblemStatCostData()
                {
                    StatLevel = x.Level,
                    EPAmount = x.EPAmount,
                }).ToList();

            result.EmblemSettings.StatUpgradeDataList = Server.ScriptManager.JobEmblemStatModule.GetData();

            result.EmblemSettings.CrestSlotRestrictionList = Server.GameSettings.EmblemSettings.InheritanceUnlockLevels
                .OrderBy(x => x.UnlockLevel)
                .Select(x => new CDataJobEmblemSlotRestriction()
                {
                    LevelUnlocked = x.UnlockLevel,
                })
                .ToList();

            result.EmblemSettings.EmblemCrestInheritanceBaseChanceList = Server.GameSettings.EmblemSettings.InheritanceUnlockLevels
                .Select((x, index) => new CDataJobEmblemCrestInheritanceBaseChance()
                {
                    Slot = (byte)index,
                    BaseChanceAmount = x.BaseChance
                })
                .ToList();

            result.EmblemSettings.InheritanceIncreaseChanceItemList = Server.GameSettings.EmblemSettings.InheritanceChanceIncreaseItems
                .Select(x => new CDataJobEmblemInhertianceIncreaseChanceItem()
                {
                    ItemId = x.ItemId,
                    AmountConsumed = x.AmountConsumed,
                    PercentIncrease = x.PercentIncrease,
                }).ToList();


            result.EmblemSettings.EmblemPointResetGGCostList = new()
            {
                new()
                {
                    Unk0 = 0,
                    PointType = (byte) WalletType.GoldenGemstones,
                    Amount = Server.GameSettings.EmblemSettings.EmblemPointResetGGAmount,
                },
            };

            result.EmblemSettings.EmblemPointResetPPCostList = new()
            {
                new()
                {
                    Unk0 = 3,
                    PointType = 3, // This means PP some how
                    Amount = Server.GameSettings.EmblemSettings.EmblemPointResetPPAmount,
                }
            };

            result.EmblemSettings.InheritancePremiumCurrencyCost.Add(new CDataJobEmblemActionCostParam()
            {
                PointType = (byte) WalletType.GoldenGemstones,
                Amount = Server.GameSettings.EmblemSettings.EmblemInheritanceEquipLossGGCost
            });

            result.EmblemSettings.MaxEmblemLevel = Server.GameSettings.EmblemSettings.MaxEmblemLevel;
            result.EmblemSettings.MaxEmblemStatUpgrades = Server.GameSettings.EmblemSettings.MaxEmblemStatUpgrades;
            result.EmblemSettings.MaxInhertienceChanceIncrease = Server.GameSettings.EmblemSettings.MaxInheritanceChanceIncrease;

            result.EmblemSettings.InventoryFilter.Add(new CDataCommonU8(1)); // Common
            result.EmblemSettings.InventoryFilter.Add(new CDataCommonU8(2)); // High Grade Earrings
            result.EmblemSettings.InventoryFilter.Add(new CDataCommonU8(3)); // High Grade Bracelet
            result.EmblemSettings.InventoryFilter.Add(new CDataCommonU8(4)); // High Grade Rings

            return result;
        }
    }
}
