using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentPartyMemberInfoHandler : GameRequestPacketHandler<C2SBattleContentPartyMemberInfoReq, S2CBattleContentPartyMemberInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentPartyMemberInfoHandler));

        public BattleContentPartyMemberInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentPartyMemberInfoRes Handle(GameClient client, C2SBattleContentPartyMemberInfoReq request)
        {
            var leader = client.Party.Leader?.Client.Character
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_LEADER_ABSENCE);

            bool contentSynced = true;
            foreach (var memberClient in client.Party.Clients)
            {
                var memberCharacter = memberClient.Character;
                contentSynced &= leader.BbmProgress.ContentId == memberCharacter.BbmProgress.ContentId;
            }

            if (!contentSynced)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CYCLE_CONTENTS_PLAY_START_INVALID_SITUATION_LEVEL, $"Not all players are synced");
            }

            if (client.Character.CharacterId != leader.CharacterId && client.Character.StageNo == leader.StageNo)
            {
                // Temp hack since member board doesn't work
                // Let the leader go first, then everyone else can join
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_MATCHING_PLAY_ENTRY_IS_NOT_READY, $"Leader didn't enter yet");
            }

            S2CBattleContentPartyMemberInfoRes res = new()
            {
                Unk0 =
                [
                    new CDataBattleContentUnk6
                    {
                        Unk0 = 2,
                        CurrentContentId = 0,
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
                            },
                        ],
                        Unk4 = true
                    }
                ],
                Unk1 = true
            };

            return res;
        }
    }
}
