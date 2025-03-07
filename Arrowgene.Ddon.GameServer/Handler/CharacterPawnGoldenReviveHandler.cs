using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterPawnGoldenReviveHandler : GameRequestPacketHandler<C2SCharacterPawnGoldenReviveReq, S2CCharacterPawnGoldenReviveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterPawnGoldenReviveHandler));


        public CharacterPawnGoldenReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterPawnGoldenReviveRes Handle(GameClient client, C2SCharacterPawnGoldenReviveReq req)
        {
            S2CCharacterPawnGoldenReviveRes res = new()
            {
                PawnId = req.PawnId,
            };

            bool walletUpdate = Server.WalletManager.RemoveFromWalletNtc(client, client.Character, WalletType.GoldenGemstones, 1); // TODO: Get price from settings.
            if (!walletUpdate)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
            }
            res.GoldenGemstonePoint = Server.WalletManager.GetWalletAmount(client.Character, WalletType.GoldenGemstones);

            return res;
        }
    }
}
