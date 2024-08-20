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
            var leader = client.Party.Leader.Client.Character;

            // Temp hack since member board doesn't work
            if (client.Character.CharacterId != leader.CharacterId)
            {
                return new S2CBattleContentPartyMemberInfoRes();
            }


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

            // This NTC will force the player into the dungeon and skip the board
            S2CBattleContentAreaChangeNtc ntc = new S2CBattleContentAreaChangeNtc()
            {
                // Unk0 = 2, // client.Character.NormalCharacterId,
                StageId = client.Character.NextBBMStageId,
                StartPos = 0,
                Unk4 = true,
            };
            client.Party.SendToAll(ntc);

            return new S2CBattleContentPartyMemberInfoRes();
        }
    }
}
