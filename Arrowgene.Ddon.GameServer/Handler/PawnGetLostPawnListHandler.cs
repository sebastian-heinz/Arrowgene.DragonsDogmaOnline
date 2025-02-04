using Arrowgene.Ddon.Server;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Linq;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetLostPawnListHandler : GameRequestPacketHandler<C2SPawnGetLostPawnListReq, S2CPawnGetLostPawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetLostPawnListHandler));

        public PawnGetLostPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetLostPawnListRes Handle(GameClient client, C2SPawnGetLostPawnListReq request)
        {
            return new S2CPawnGetLostPawnListRes()
            {
                LostPawnList = client.Character.Pawns
                    .Select((pawn, index) => new { pawn, slot = index+1 })
                    .Where(tuple => tuple.pawn.PawnState == PawnState.Lost)
                    .Select(tuple => new CDataLostPawnList()
                    {
                        PawnId = (int) tuple.pawn.PawnId,
                        SlotNo = (uint) tuple.slot,
                        Name = tuple.pawn.Name,
                        Sex = tuple.pawn.EditInfo.Sex,
                        PawnState = tuple.pawn.PawnState,
                        // TODO: ShareRange
                        ReviveCost = 1, // TODO: Make it configurable
                        PawnListData = new CDataPawnListData()
                        {
                            Job = tuple.pawn.Job,
                            Level = tuple.pawn.ActiveCharacterJobData.Lv,
                            CraftRank = tuple.pawn.CraftData.CraftRank,
                            PawnCraftSkillList = tuple.pawn.CraftData.PawnCraftSkillList,
                            // TODO: CommentSize, LatestReturnDate
                        }
                    })
                    .ToList()
            };
        }
    }
}
