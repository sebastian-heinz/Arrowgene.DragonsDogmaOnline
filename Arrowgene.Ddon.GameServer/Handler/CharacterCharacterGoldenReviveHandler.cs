using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterGoldenReviveHandler : GameRequestPacketHandler<C2SCharacterCharacterGoldenReviveReq, S2CCharacterCharacterGoldenReviveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterGoldenReviveHandler));

        public CharacterCharacterGoldenReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterCharacterGoldenReviveRes Handle(GameClient client, C2SCharacterCharacterGoldenReviveReq packet)
        {
            S2CCharacterCharacterGoldenReviveRes res = new S2CCharacterCharacterGoldenReviveRes();

            var amount = Server.WalletManager.GetWalletAmount(client.Character, WalletType.GoldenGemstones);
            res.GP = amount - 1;
            Server.WalletManager.RemoveFromWalletNtc(client, client.Character, WalletType.GoldenGemstones, 1);

            return res;
        }
    }
}
