using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobOrbTreeGetCurrencyExchangeHandler : GameRequestPacketHandler<C2SJobOrbTreeGetCurrencyExchangeReq, S2CJobOrbTreeGetCurrencyExchangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobOrbTreeGetCurrencyExchangeHandler));

        public JobOrbTreeGetCurrencyExchangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobOrbTreeGetCurrencyExchangeRes Handle(GameClient client, C2SJobOrbTreeGetCurrencyExchangeReq request)
        {
            return new S2CJobOrbTreeGetCurrencyExchangeRes()
            {
                CurrencyExchangeList =
                [
                    new()
                    {
                        Unk0 = request.Unk0,
                        Unk4 = 1337,
                        WalletTypeFrom = (uint) WalletType.HighOrbs,
                        WalletTypeTo = (uint) WalletType.BloodOrbs,
                        ConversionRate = Server.GameSettings.GameServerSettings.HighOrbConversionRate
                    }
                ],
                EnableOrbExchange = Server.GameSettings.GameServerSettings.EnableHighOrbConversion
            };
        }
    }
}
