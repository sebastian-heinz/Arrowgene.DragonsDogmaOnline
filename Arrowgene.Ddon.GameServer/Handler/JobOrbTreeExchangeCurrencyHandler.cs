using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobOrbTreeExchangeCurrencyHandler : GameRequestPacketHandler<C2SJobOrbTreeExchangeCurrencyReq, S2CJobOrbTreeExchangeCurrencyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobOrbTreeExchangeCurrencyHandler));

        public JobOrbTreeExchangeCurrencyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobOrbTreeExchangeCurrencyRes Handle(GameClient client, C2SJobOrbTreeExchangeCurrencyReq request)
        {
            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.WalletManager.RemoveFromWallet(client.Character, WalletType.HighOrbs, request.ExchangeAmount / Server.GameSettings.GameServerSettings.HighOrbConversionRate, connection);
                Server.WalletManager.AddToWallet(client.Character, WalletType.BloodOrbs, request.ExchangeAmount, connectionIn: connection);
            });

            return new()
            {
                WalletType0 = (uint)WalletType.BloodOrbs,
                AmountType0 = Server.WalletManager.GetWalletAmount(client.Character, WalletType.BloodOrbs),
                WalletType1 = (uint)WalletType.HighOrbs,
                AmountType1 = Server.WalletManager.GetWalletAmount(client.Character, WalletType.HighOrbs),
            };
        }
    }
}
