using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetMyPawnListHandler : GameRequestPacketHandler<C2SPawnGetMyPawnListReq, S2CPawnGetMypawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetMyPawnListHandler));

        public PawnGetMyPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetMypawnListRes Handle(GameClient client, C2SPawnGetMyPawnListReq request)
        {
            S2CPawnGetMypawnListRes res = new S2CPawnGetMypawnListRes();

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
                var partnerData = Server.Database.GetPartnerPawnRecord(client.Character.CharacterId, client.Character.PartnerPawnId);
                res.PartnerInfo =  (partnerData != null) ? partnerData.ToCDataPartnerPawnData(partnerPawn) : new CDataPartnerPawnData();
            }

            return res;
        }
    }
}
