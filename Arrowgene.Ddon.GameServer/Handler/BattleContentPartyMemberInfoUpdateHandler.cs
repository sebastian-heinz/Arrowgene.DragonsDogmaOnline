using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentPartyMemberInfoUpdateHandler : GameRequestPacketHandler<C2SBattleContentPartyMemberInfoUpdateReq, S2CBattleContentPartyMemberInfoUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentPartyMemberInfoUpdateHandler));

        public BattleContentPartyMemberInfoUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentPartyMemberInfoUpdateRes Handle(GameClient client, C2SBattleContentPartyMemberInfoUpdateReq request)
        {
            S2CBattleContentPartyMemberInfoUpdateNtc ntc = new S2CBattleContentPartyMemberInfoUpdateNtc()
            {
                CharacterId = client.Character.CharacterId,
                Location = 0,
                BattleContentSituationData = new CDataBattleContentSituationData
                {
                    GameMode = GameMode.BitterblackMaze,
                    StartTime = 1,
                    RewardReceived = false,
                    Unk3 = false,
                    RewardBonus = BattleContentRewardBonus.Normal,
                    ReportReset = 0,
                    ReportSearchResults = 0,
                    Unk7 = 0,
                    Unk8 = 0,
                    Unktime = 0,
                    ContentId = 2,
                    Unk11 = 0
                },
                BattleContentAvailableRewardsList =
                [
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
                ],
                Status = true
            };
            client.Send(ntc);

            return new S2CBattleContentPartyMemberInfoUpdateRes();
        }
    }
}
