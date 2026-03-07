using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentPhaseEntryBeginRecruitmentHandler : GameRequestPacketHandler<C2SBattleContentPhaseEntryBeginRecruitmentReq,
        S2CBattleContentPhaseEntryBeginRecruitmentRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentPhaseEntryBeginRecruitmentHandler));

        public BattleContentPhaseEntryBeginRecruitmentHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentPhaseEntryBeginRecruitmentRes Handle(GameClient client, C2SBattleContentPhaseEntryBeginRecruitmentReq request)
        {
            // TODO: The request has lots of data.. 

            var leader = client.Party.Leader?.Client.Character
                         ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_LEADER_ABSENCE);

            // This NTC will force the player into the dungeon and skip the board
            S2CBattleContentAreaChangeNtc areaChangeNtc = new S2CBattleContentAreaChangeNtc
            {
                Unk0 = request.Unk3,
                Unk1 = request.UnknownString,
                StageId = leader.NextBBMStageId,
                StartPos = 0,
                Unk4 = request.Unk6,
                Unk5 = [new CDataBattleContentUnk5
                    {
                        Unk0 = 0,
                        Unk1 = 0
                    }
                ],
            };
            client.Send(areaChangeNtc);

            S2CBattleContentPhaseEntryBeginRecruitmentRes res = new();
            return res;
        }
    }
}
