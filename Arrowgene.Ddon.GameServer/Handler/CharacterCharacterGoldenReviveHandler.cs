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

            bool walletUpdate = Server.WalletManager.RemoveFromWalletNtc(client, client.Character, WalletType.GoldenGemstones, 1); // TODO: Get price from settings.
            if (!walletUpdate)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
            }
            res.GP = Server.WalletManager.GetWalletAmount(client.Character, WalletType.GoldenGemstones); 

            return res;
        }
    }
}
