using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditUpdateCharacterEditParamHandler : GameStructurePacketHandler<C2SCharacterEditUpdateCharacterEditParamReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditUpdateCharacterEditParamHandler));

        public CharacterEditUpdateCharacterEditParamHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterEditUpdateCharacterEditParamReq> packet)
        {
            Server.WalletManager.RemoveFromWalletNtc(client, client.Character,
                            packet.Structure.EditPrice.PointType, packet.Structure.EditPrice.Value);

            client.Character.EditInfo = packet.Structure.EditInfo;
            Server.Database.UpdateEditInfo(client.Character);
            client.Send(new S2CCharacterEditUpdateCharacterEditParamRes());
            foreach(Client other in Server.ClientLookup.GetAll()) {
                other.Send(new S2CCharacterEditUpdateEditParamNtc() {
                    CharacterId = client.Character.CharacterId,
                    PawnId = 0,
                    EditInfo = client.Character.EditInfo
                });
            }
        }
    }
}
