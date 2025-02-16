using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonDeliverItemForExHandler : GameRequestPacketHandler<C2SSeasonDungeonDeliverItemForExReq, S2CSeasonDungeonDeliverItemForExRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonDeliverItemForExHandler));

        public SeasonDungeonDeliverItemForExHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonDeliverItemForExRes Handle(GameClient client, C2SSeasonDungeonDeliverItemForExReq request)
        {
            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.ConsumeBag
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var item in request.ItemList)
                {
                    var itemUpdate = Server.ItemManager.ConsumeItemByUId(Server, client.Character, StorageType.ItemBagMaterial, item.ItemUId, item.Num, connection);
                    ntc.UpdateItemList.Add(itemUpdate);
                }
            });

            if ((EpitaphId.GetKind(request.EpitaphId) == EpitaphIdKind.Path) ||
                (EpitaphId.GetKind(request.EpitaphId) == EpitaphIdKind.Trial && EpitaphId.GetIndex(request.EpitaphId) == 0) ||
                (EpitaphId.GetKind(request.EpitaphId) == EpitaphIdKind.Barrier))
            {
                client.Character.EpitaphRoadState.UnlockedContent.Add(request.EpitaphId);
                Server.Database.InsertEpitaphRoadUnlock(client.Character.CharacterId, request.EpitaphId);

                if (EpitaphId.GetKind(request.EpitaphId) == EpitaphIdKind.Barrier)
                {
                    var barrier = Server.EpitaphRoadManager.GetBarrier(request.EpitaphId);
                    foreach (var sectionId in barrier.DependentSectionIds)
                    {
                        if (client.Character.EpitaphRoadState.UnlockedContent.Contains(sectionId))
                        {
                            continue;
                        }

                        var section = Server.EpitaphRoadManager.GetEpitahObject<EpitaphSection>(sectionId);

                        bool shouldUnlockSection = true;
                        foreach (var dependencyId in section.BarrierDependencies)
                        {
                            shouldUnlockSection &= client.Character.EpitaphRoadState.UnlockedContent.Contains(dependencyId);
                            if (!shouldUnlockSection)
                            {
                                break;
                            }
                        }

                        if (shouldUnlockSection)
                        {
                            client.Character.EpitaphRoadState.UnlockedContent.Add(section.EpitaphId);
                            Server.Database.InsertEpitaphRoadUnlock(client.Character.CharacterId, section.EpitaphId);
                        }
                    }
                }
            }

            if (EpitaphId.GetKind(request.EpitaphId) == EpitaphIdKind.Trial && EpitaphId.GetIndex(request.EpitaphId) == 0)
            {
                var trial = Server.EpitaphRoadManager.GetTrial(request.EpitaphId);
                client.Party.SendToAll(new S2CSeasonDungeonSetOmStateNtc()
                {
                    LayoutId = trial.OmLayoutId.ToCDataStageLayoutId(),
                    State = SoulOrdealOmState.TrialAvailable,
                });
            }
            else if (EpitaphId.GetKind(request.EpitaphId) == EpitaphIdKind.Barrier)
            {
                var barrier = Server.EpitaphRoadManager.GetBarrier(request.EpitaphId);
                client.Send(new S2CSeasonDungeonSetOmStateNtc()
                {
                    LayoutId = barrier.StageId.ToCDataStageLayoutId(),
                    State = SoulOrdealOmState.AreaUnlocked
                });
            }

            if (ntc.UpdateItemList.Count > 0 || ntc.UpdateWalletList.Count > 0)
            {
                client.Send(ntc);
            }

            return new S2CSeasonDungeonDeliverItemForExRes();
        }
    }
}
