using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

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
            // Seems only Unk0.Unk0 and WalletPoints does anything for BBM?

            var result = new S2CBattleContentResetInfoRes();
            result.ResetInfoList.Add(new CDataResetInfo()
            {
                Unk0 = new CDataResetInfoUnk0()
                {
                    Unk0 = 3, // Shows up in next packet Unk0
                },
                WalletPoints = new List<CDataWalletPoint>()
                {
                    new CDataWalletPoint()
                    {
                        Type = WalletType.BitterblackMazeResetTicket,
                        Value = 1,
                    },
                }
            });

            return result;
        }
    }
}
