using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterGoldenReviveHandler : GameStructurePacketHandler<C2SCharacterCharacterGoldenReviveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterGoldenReviveHandler));

        public CharacterCharacterGoldenReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterGoldenReviveReq> packet)
        {
            S2CCharacterCharacterGoldenReviveRes res = new S2CCharacterCharacterGoldenReviveRes();

            var amount = Server.WalletManager.GetWalletAmount(client.Character, WalletType.GoldenGemstones);
            res.GP = amount - 1;
            Server.WalletManager.RemoveFromWallet(client.Character, WalletType.GoldenGemstones, 1);

            client.Send(res);
        }
    }
}
