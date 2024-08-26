using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetOfficialPawnListHandler : GameRequestPacketHandler<C2SPawnGetOfficialPawnListReq, S2CPawnGetOfficialPawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetOfficialPawnListHandler));

        public PawnGetOfficialPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetOfficialPawnListRes Handle(GameClient client, C2SPawnGetOfficialPawnListReq request)
        {
            var results = new S2CPawnGetOfficialPawnListRes();
            var officialPawnIds = Server.Database.SelectOfficialPawns();
            foreach (var pawnId in officialPawnIds)
            {
                var pawn = Server.Database.SelectPawn(pawnId);
                results.OfficialPawnList.Add(new CDataRegisterdPawnList()
                {
                    Name = pawn.Name,
                    RentalCost = client.Character.ActiveCharacterJobData.Lv * 10,
                    Sex = pawn.EditInfo.Sex,
                    Updated = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    PawnId = pawn.PawnId,
                    PawnListData = new CDataPawnListData()
                    {
                        Job = pawn.Job,
                        Level = client.Character.ActiveCharacterJobData.Lv,
                        PawnCraftSkillList = pawn.CraftData.PawnCraftSkillList,
                    }
                });
            }
            return results;
        }
    }
}
