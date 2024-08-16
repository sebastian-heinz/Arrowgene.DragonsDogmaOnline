using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterEditUpdatePawnEditParamHandler : GameRequestPacketHandler<C2SCharacterEditUpdatePawnEditParamReq, S2CCharacterEditUpdatePawnEditParamRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterEditUpdatePawnEditParamHandler));

        public CharacterEditUpdatePawnEditParamHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterEditUpdatePawnEditParamRes Handle(GameClient client, C2SCharacterEditUpdatePawnEditParamReq packet)
        {
            CharacterEditGetShopPriceHandler.CheckPrice(packet.UpdateType, packet.EditPrice.PointType, packet.EditPrice.Value);

            Server.WalletManager.RemoveFromWalletNtc(client, client.Character,
                packet.EditPrice.PointType, packet.EditPrice.Value);

            Pawn pawn = client.Character.PawnBySlotNo(packet.SlotNo);
            pawn.EditInfo = packet.EditInfo;
            Server.Database.UpdateEditInfo(pawn);
            foreach(Client other in Server.ClientLookup.GetAll()) {
                other.Send(new S2CCharacterEditUpdateEditParamNtc() {
                    CharacterId = pawn.CharacterId,
                    PawnId = pawn.PawnId,
                    EditInfo = pawn.EditInfo
                });
            }

            return new S2CCharacterEditUpdatePawnEditParamRes();
        }
    }
}
