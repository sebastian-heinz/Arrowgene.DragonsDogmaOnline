using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class RecycleGetInfoHandler : GameRequestPacketHandler<C2SRecycleGetInfoReq, S2CRecycleGetInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RecycleGetInfoHandler));

        public RecycleGetInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CRecycleGetInfoRes Handle(GameClient client, C2SRecycleGetInfoReq request)
        {
            var attempts = Server.Database.GetRecycleEquipmentAttempts(client.Character.CharacterId);
            return new()
            {
                MaxAttempts = Server.GameSettings.GameServerSettings.CraftItemRecycleMax,
                AttemptsTaken = attempts,
                ResetCostList = new()
                {
                    new CDataRecycleWalletCost()
                    {
                        WalletType = WalletType.GoldenGemstones,
                        Amount = Server.GameSettings.GameServerSettings.CraftItemRecycleResetGGCost
                    }
                }
            };
        }
    }
}
