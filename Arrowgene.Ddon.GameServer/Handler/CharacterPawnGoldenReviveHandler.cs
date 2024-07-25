using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterPawnGoldenReviveHandler : GameStructurePacketHandler<C2SCharacterPawnGoldenReviveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterPawnGoldenReviveHandler));


        public CharacterPawnGoldenReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterPawnGoldenReviveReq> req)
        {
            S2CCharacterPawnGoldenReviveRes res = new S2CCharacterPawnGoldenReviveRes(req.Structure);

            var amount = Server.WalletManager.GetWalletAmount(client.Character, WalletType.GoldenGemstones);
            res.GoldenGemstonePoint = amount - 1;
            Server.WalletManager.RemoveFromWallet(client.Character, WalletType.GoldenGemstones, 1);

            client.Send(res);
        }
    }
}
