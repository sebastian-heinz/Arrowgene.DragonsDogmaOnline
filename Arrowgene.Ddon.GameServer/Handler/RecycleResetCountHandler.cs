using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class RecycleResetCountHandler : GameRequestPacketHandler<C2SRecycleResetCountReq, S2CRecycleResetCountRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RecycleResetCountHandler));

        public RecycleResetCountHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CRecycleResetCountRes Handle(GameClient client, C2SRecycleResetCountReq request)
        {
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new();
            Server.Database.ExecuteInTransaction(connection =>
            {
                var updateGP = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.GoldenGemstones, Server.GameSettings.GameServerSettings.CraftItemRecycleResetGGCost, connectionIn: connection)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
                updateCharacterItemNtc.UpdateWalletList.Add(updateGP);
                Server.Database.UpsertRecycleEquipmentRecord(client.Character.CharacterId, 0, connectionIn: connection);
            });
            client.Send(updateCharacterItemNtc);

            return new();
        }
    }
}
