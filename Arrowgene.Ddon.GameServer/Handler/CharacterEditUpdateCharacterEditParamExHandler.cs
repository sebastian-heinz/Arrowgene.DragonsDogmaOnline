#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

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

            Server.WalletManager.RemoveFromWalletNtc(client, client.Character,
                packet.Structure.EditPrice.PointType, packet.Structure.EditPrice.Value);

            client.Character.EditInfo = packet.Structure.EditInfo;
            Server.Database.UpdateEditInfo(client.Character);
            
            if(packet.Structure.FirstName.Length > 0) {
                client.Character.FirstName = packet.Structure.FirstName;
                Server.Database.UpdateCharacterBaseInfo(client.Character);
            }

            //Client won't let you reincarnate if you're wearing a gender-locked item, but EquipmentTemplates also have to be cleaned.
            Server.EquipManager.CleanGenderedEquipTemplates(Server, client.Character);

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
