using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentInstantClearInfoHandler : GameRequestPacketHandler<C2SBattleContentInstantClearInfoReq, S2CBattleContentInstantClearInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentInstantClearInfoHandler));

        public BattleContentInstantClearInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentInstantClearInfoRes Handle(GameClient client, C2SBattleContentInstantClearInfoReq request)
        {
            S2CBattleContentInstantClearInfoRes res = new S2CBattleContentInstantClearInfoRes
            {
                Unk0 =
                [
                    new CDataBattleContentUnk4
                    {
                        Unk0 = 2,
                        Unk1 = 0,
                        UnknownString = "Bitterblack Maze Rotunda: Netherworld 1",
                        WalletPoints =
                        [
                            new CDataWalletPoint
                            {
                                Type = WalletType.Gold,
                                Value = 123
                            }
                        ],
                        Unk3 =
                        [
                            new CDataBattleContentUnk5
                            {
                                Unk0 = 2,
                                Unk1 = 2
                            }
                        ]
                    }
                ]
            };

            return res;
        }
    }
}
