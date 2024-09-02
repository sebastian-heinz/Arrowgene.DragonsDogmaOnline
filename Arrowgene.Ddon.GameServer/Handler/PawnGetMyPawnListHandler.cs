using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetMyPawnListHandler : StructurePacketHandler<GameClient, C2SPawnGetMyPawnListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetMyPawnListHandler));

        public PawnGetMyPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetMyPawnListReq> packet)
        {
            List<CDataPawnList> pawnList = new List<CDataPawnList>();

            uint index = 1;
            foreach (Pawn pawn in client.Character.Pawns)
            {
                CDataPawnList pawnListData = new CDataPawnList
                {
                    PawnId = (int)pawn.PawnId,
                    SlotNo = index++,
                    Name = pawn.Name,
                    Sex = pawn.EditInfo.Sex,
                    PawnState = pawn.State,
                    ShareRange = 1,
                    PawnListData = new CDataPawnListData
                    {
                        Job = pawn.Job,
                        Level = pawn.ActiveCharacterJobData.Lv,
                        CraftRank = pawn.CraftData.CraftRank,
                        PawnCraftSkillList = pawn.CraftData.PawnCraftSkillList,
                        // TODO: CommentSize, LatestReturnDate
                        CommentSize = 0,
                        LatestReturnDate = 0
                    },
                    // TODO: PawnState, ShareRange, Unk0, Unk1, Unk2
                    Unk0 = 6,
                    TrainingExp = 1422,
                    Unk2 = 0
                };
                pawnList.Add(pawnListData);
            }

            // TODO: PartnerInfo
            CDataPartnerPawnData partnerInfo = new CDataPartnerPawnData()
            {
                PawnId = client.Character.Pawns.FirstOrDefault()?.PawnId ?? 0,
                Likability = 1,
                Personality = 1
            };

            S2CPawnGetMypawnListRes res = new S2CPawnGetMypawnListRes();
            res.PawnList = pawnList;
            res.PartnerInfo = partnerInfo;

            client.Send(res);
        }
    }
}
