using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentCharacterInfoHandler : GameRequestPacketHandler<C2SBattleContentCharacterInfoReq, S2CBattleContentCharacterInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentCharacterInfoHandler));

        public BattleContentCharacterInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentCharacterInfoRes Handle(GameClient client, C2SBattleContentCharacterInfoReq request)
        {

            var result = new S2CBattleContentCharacterInfoRes()
            {
                SituationData = new CDataBattleContentSituationData
                {
                    GameMode = GameMode.BitterblackMaze,
                    StartTime = 1,
                    RewardReceived = false,
                    Unk3 = false,
                    RewardBonus = BattleContentRewardBonus.Normal,
                    ReportReset = 0,
                    ReportSearchResults = 0,
                    Unk7 = 2,
                    Unk8 = 2,
                    Unktime = 0,
                    ContentId = 2,
                    Unk11 = 2
                },
                AvailableRewardsList = [
                    new CDataBattleContentAvailableRewards
                    {
                        Id = 10,
                        Amount = 1
                    },
                    new CDataBattleContentAvailableRewards
                    {
                        Id = 2,
                        Amount = 1
                    },
                    new CDataBattleContentAvailableRewards
                    {
                        Id = 11,
                        Amount = 1
                    },
                    new CDataBattleContentAvailableRewards
                    {
                        Id = 3,
                        Amount = 1
                    },
                    new CDataBattleContentAvailableRewards
                    {
                        Id = 12,
                        Amount = 1
                    },
                    new CDataBattleContentAvailableRewards
                    {
                        Id = 4,
                        Amount = 1
                    },
                    new CDataBattleContentAvailableRewards
                    {
                        Id = 13,
                        Amount = 1
                    }
                ]
            };

            return result;
        }
    }
}
