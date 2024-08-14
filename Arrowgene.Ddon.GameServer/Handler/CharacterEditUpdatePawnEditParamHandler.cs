using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditUpdatePawnEditParamHandler : GameStructurePacketHandler<C2SCharacterEditUpdatePawnEditParamReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditUpdatePawnEditParamHandler));

        public CharacterEditUpdatePawnEditParamHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterEditUpdatePawnEditParamReq> packet)
        {
            Server.WalletManager.RemoveFromWalletNtc(client, client.Character,
                packet.Structure.EditPrice.PointType, packet.Structure.EditPrice.Value);

            Pawn pawn = client.Character.PawnBySlotNo(packet.Structure.SlotNo);
            pawn.EditInfo = packet.Structure.EditInfo;
            Server.Database.UpdateEditInfo(pawn);
            client.Send(new S2CCharacterEditUpdatePawnEditParamRes());
            foreach(Client other in Server.ClientLookup.GetAll()) {
                other.Send(new S2CCharacterEditUpdateEditParamNtc() {
                    CharacterId = pawn.CharacterId,
                    PawnId = pawn.PawnId,
                    EditInfo = pawn.EditInfo
                });
            }
        }
    }
}
