using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentContentEntryHandler : GameRequestPacketHandler<C2SBattleContentContentEntryReq, S2CBattleContentContentEntryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentContentEntryHandler));

        public BattleContentContentEntryHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentContentEntryRes Handle(GameClient client, C2SBattleContentContentEntryReq request)
        {
            var progress = client.Character.BbmProgress;

            if (progress.StartTime == 0)
            {
                progress.StartTime = (ulong) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                Server.Database.UpdateBBMProgress(client.Character.CharacterId, progress);
            }

            var contentStatus = BitterblackMazeManager.GetUpdatedContentStatus(Server, client.Character);
            S2CBattleContentContentEntryNtc ntc = new S2CBattleContentContentEntryNtc()
            {
                GameMode = GameMode.BitterblackMaze,
            };
            ntc.BattleContentStatusList.Add(contentStatus);
            client.Send(ntc);

            S2CBattleContentProgressNtc ntc2 = new S2CBattleContentProgressNtc();
            ntc2.BattleContentStatusList.Add(contentStatus);
            client.Send(ntc2);

            return new S2CBattleContentContentEntryRes();
        }
    }
}
