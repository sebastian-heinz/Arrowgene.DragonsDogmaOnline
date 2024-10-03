#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditUpdatePawnEditParamExHandler : GameRequestPacketHandler<C2SCharacterEditUpdatePawnEditParamExReq, S2CCharacterEditUpdatePawnEditParamExRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditUpdatePawnEditParamExHandler));

        public CharacterEditUpdatePawnEditParamExHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterEditUpdatePawnEditParamExRes Handle(GameClient client, C2SCharacterEditUpdatePawnEditParamExReq packet)
        {
            CharacterEditGetShopPriceHandler.CheckPrice(packet.UpdateType, packet.EditPrice.PointType, packet.EditPrice.Value);

            Server.WalletManager.RemoveFromWalletNtc(client, client.Character,
                packet.EditPrice.PointType, packet.EditPrice.Value);

            Pawn pawn = client.Character.PawnBySlotNo(packet.SlotNo);
            pawn.EditInfo = packet.EditInfo;
            Server.Database.UpdateEditInfo(pawn);
            pawn.Name = packet.Name;
            Server.Database.UpdatePawnBaseInfo(pawn);

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.StorePostItemMail //Probably an abuse of this notice type.
            };

            List<(EquipType, EquipSlot)> forceRemovals = new List<(EquipType, EquipSlot)>();

            //Weirdly enough, pawns can reincarnate while wearing gender-locked equipment just fine.
            Server.Database.ExecuteInTransaction(connection =>
            {
                forceRemovals = Server.EquipManager.CleanGenderedEquipTemplates(Server, pawn, connection);
                if (forceRemovals.Any())
                {
                    foreach ((EquipType, EquipSlot) force in forceRemovals)
                    {
                        EquipType equipType = force.Item1;
                        EquipSlot slot = force.Item2;

                        Storage destinationStorage = client.Character.Storage.GetStorage(StorageType.ItemPost);
                        updateCharacterItemNtc.UpdateItemList.AddRange(Server.ItemManager.MoveItem(
                            Server,
                            client.Character,
                            pawn.Equipment.Storage,
                            pawn.Equipment.GetStorageSlot(equipType, (byte)slot),
                            1,
                            destinationStorage,
                            0
                        ));
                    }
                }
            });

            if (forceRemovals.Any())
            {
                client.Send(updateCharacterItemNtc);

                S2CEquipChangePawnEquipNtc updateEquipNtc = new S2CEquipChangePawnEquipNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    PawnId = pawn.PawnId,
                    EquipItemList = pawn.EquipmentTemplate.EquipmentAsCDataEquipItemInfo(pawn.Job, EquipType.Performance),
                    VisualEquipItemList = pawn.EquipmentTemplate.EquipmentAsCDataEquipItemInfo(pawn.Job, EquipType.Visual)
                };

                client.Send(updateEquipNtc);
            }

            client.Party.SendToAllExcept(new S2CCharacterEditUpdateEditParamExNtc()
            {
                CharacterId = pawn.CharacterId,
                PawnId = pawn.PawnId,
                EditInfo = pawn.EditInfo,
                Name = pawn.Name
            }, client);


            return new S2CCharacterEditUpdatePawnEditParamExRes();
        }
    }
}
