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
    public class CharacterEditUpdateCharacterEditParamExHandler : GameStructurePacketHandler<C2SCharacterEditUpdateCharacterEditParamExReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditUpdateCharacterEditParamExHandler));
        
        public CharacterEditUpdateCharacterEditParamExHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterEditUpdateCharacterEditParamExReq> packet)
        {
            // TODO: Substract GG
            client.Character.EditInfo = packet.Structure.EditInfo;
            Server.Database.UpdateEditInfo(client.Character);
            
            if(packet.Structure.FirstName.Length > 0) {
                client.Character.FirstName = packet.Structure.FirstName;
                Server.Database.UpdateCharacterBaseInfo(client.Character);
            }

            //Client won't let you reincarnate if you're wearing a gender-locked item, but EquipmentTemplates also have to be cleaned.
            foreach (JobId job in Enum.GetValues(typeof(JobId)))
            {
                foreach (EquipType equipType in Enum.GetValues(typeof(EquipType)))
                {
                    foreach (EquipSlot equipSlot in Enumerable.Range(1, 15))
                    {
                        Item? item = client.Character.EquipmentTemplate.GetEquipItem(job, equipType, (byte)equipSlot);
                        if (item is null) continue;
                        ClientItemInfo itemInfo = Server.ItemManager.LookupInfoByItem(Server, item);
                        if (itemInfo.Gender == Gender.Any) continue;
                        if ((client.Character.EditInfo.Sex == 1 && itemInfo.Gender == Gender.Female)
                            || (client.Character.EditInfo.Sex == 2 && itemInfo.Gender == Gender.Male))
                        {
                            client.Character.EquipmentTemplate.SetEquipItem(null, job, equipType, (byte)equipSlot);
                            Server.Database.DeleteEquipItem(client.Character.CommonId, job, equipType, (byte)equipSlot);
                        }
                    }
                }
            }

            client.Send(new S2CCharacterEditUpdateCharacterEditParamExRes());
            foreach(Client other in Server.ClientLookup.GetAll()) {
                other.Send(new S2CCharacterEditUpdateEditParamExNtc() {
                    CharacterId = client.Character.CharacterId,
                    PawnId = 0,
                    EditInfo = client.Character.EditInfo,
                    Name = client.Character.FirstName
                });
            }
        }
    }
}
