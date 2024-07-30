#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
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

            //Client won't let you reincarnate if you're wearing a gender-locked item, but EquipmentTemplates also have to be cleaned.
            foreach (JobId job in Enum.GetValues(typeof(JobId)))
            {
                foreach (EquipType equipType in Enum.GetValues(typeof(EquipType)))
                {
                    foreach (EquipSlot equipSlot in Enumerable.Range(1, 15))
                    {
                        Item? item = pawn.EquipmentTemplate.GetEquipItem(job, equipType, (byte)equipSlot);
                        if (item is null) continue;
                        ClientItemInfo itemInfo = Server.ItemManager.LookupInfoByItem(Server, item);
                        if (itemInfo.Gender == Gender.Any) continue;
                        if ((pawn.EditInfo.Sex == 1 && itemInfo.Gender == Gender.Female)
                            || (pawn.EditInfo.Sex == 2 && itemInfo.Gender == Gender.Male))
                        {
                            pawn.EquipmentTemplate.SetEquipItem(null, job, equipType, (byte)equipSlot);
                            Server.Database.DeleteEquipItem(pawn.CommonId, job, equipType, (byte)equipSlot);
                        }
                    }
                }
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
