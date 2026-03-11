using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentResetInfoHandler : GameRequestPacketHandler<C2SBattleContentResetInfoReq, S2CBattleContentResetInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentResetInfoHandler));

        public BattleContentResetInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentResetInfoRes Handle(GameClient client, C2SBattleContentResetInfoReq request)
        {
            var result = new S2CBattleContentResetInfoRes();
            result.ResetInfoList.Add(new CDataResetInfo()
            {
                Unk0 = new CDataResetInfoUnk0()
                {
                    Index = 1
                },
                WalletPoints =
                [
                    new()
                    {
                        Type = WalletType.BitterblackMazeResetTicket,
                        Value = 1,
                    }
                ]
            });

            result.ResetInfoList.Add(new CDataResetInfo()
            {
                Unk0 = new CDataResetInfoUnk0()
                {
                    Index = 2, // Shows up in next packet Unk0
                    IsPremium1 = true,
                    IsPremium2 = true,
                },
                TrackUses = true,
                MaxUses = Server.GameSettings.GameServerSettings.BBMWeeklyGGResets,
                CurrentUses = Server.Database.SelectBBMGGReset(client.Character.CharacterId),
                WalletPoints =
                [
                    new()
                    {
                        Type = WalletType.GoldenGemstones,
                        Value = Server.GameSettings.GameServerSettings.BBMResetGGCost,
                    },
                ]
            });

            return result;
        }
    }
}
