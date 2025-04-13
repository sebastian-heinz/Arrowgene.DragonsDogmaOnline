using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetMyPawnListHandler : GameRequestPacketHandler<C2SPawnGetMyPawnListReq, S2CPawnGetMyPawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetMyPawnListHandler));

        public PawnGetMyPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetMyPawnListRes Handle(GameClient client, C2SPawnGetMyPawnListReq request)
        {
            S2CPawnGetMyPawnListRes res = new S2CPawnGetMyPawnListRes();

            uint index = 1;
            foreach (Pawn pawn in client.Character.Pawns)
            {
                CDataPawnList pawnListData = new CDataPawnList
                {
                    PawnId = (int)pawn.PawnId,
                    SlotNo = index++,
                    Name = pawn.Name,
                    Sex = pawn.EditInfo.Sex,
                    PawnState = pawn.PawnState,
                    PawnListData = new CDataPawnListData()
                    {
                        Job = pawn.Job,
                        Level = pawn.ActiveCharacterJobData.Lv,
                        CraftRank = pawn.CraftData.CraftRank,
                        PawnCraftSkillList = pawn.CraftData.PawnCraftSkillList,
                        // TODO: CommentSize, LatestReturnDate
                    },
                    // TODO: ShareRange, Unk0, Unk1, Unk2
                };
                res.PawnList.Add(pawnListData);
            }

            var partnerPawn = client.Character.Pawns.Where(x => x.PawnId == client.Character.PartnerPawnId).FirstOrDefault();
            if (partnerPawn != null)
            {
                var partnerPawnData = Server.PartnerPawnManager.GetPartnerPawnData(client) ?? new PartnerPawnData();
                res.PartnerInfo = partnerPawnData.ToCDataPartnerPawnData(partnerPawn);
            }

            return res;
        }
    }
}
