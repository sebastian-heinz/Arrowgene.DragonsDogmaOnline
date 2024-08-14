#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditUpdatePawnEditParamExHandler : GameStructurePacketHandler<C2SCharacterEditUpdatePawnEditParamExReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditUpdatePawnEditParamExHandler));
        
        public CharacterEditUpdatePawnEditParamExHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterEditUpdatePawnEditParamExReq> packet)
        {
            // TODO: Substract GG
            Pawn pawn = client.Character.PawnBySlotNo(packet.Structure.SlotNo);
            pawn.EditInfo = packet.Structure.EditInfo;
            Server.Database.UpdateEditInfo(pawn);
            pawn.Name = packet.Structure.Name;
            Server.Database.UpdatePawnBaseInfo(pawn);

            //Weirdly enough, pawns can reincarnate while wearing gender-locked equipment just fine.
            List<(EquipType, EquipSlot)> forceRemovals = Server.EquipManager.CleanGenderedEquipTemplates(Server, pawn);
            if (forceRemovals.Any())
            {
                S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = ItemNoticeType.ChangePawnEquip
                };

                S2CEquipChangePawnEquipNtc updateEquipNtc = new S2CEquipChangePawnEquipNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    PawnId = pawn.PawnId,
                    EquipItemList = pawn.EquipmentTemplate.EquipmentAsCDataEquipItemInfo(pawn.Job, EquipType.Performance),
                    VisualEquipItemList = pawn.EquipmentTemplate.EquipmentAsCDataEquipItemInfo(pawn.Job, EquipType.Visual)
                };

                foreach ((EquipType, EquipSlot) force in forceRemovals)
                {
                    EquipType equipType = force.Item1;
                    EquipSlot slot = force.Item2;

                    Storage destinationStorage = client.Character.Storage.GetStorage(StorageType.ItemBagEquipment);
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

                client.Send(updateCharacterItemNtc);
                client.Send(updateEquipNtc);
            }

            client.Send(new S2CCharacterEditUpdatePawnEditParamExRes());
            foreach(Client other in Server.ClientLookup.GetAll()) {
                other.Send(new S2CCharacterEditUpdateEditParamExNtc() {
                    CharacterId = pawn.CharacterId,
                    PawnId = pawn.PawnId,
                    EditInfo = pawn.EditInfo,
                    Name = pawn.Name
                });
            }
        }
    }
}
