using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetFavoritePawnListHandler : GameRequestPacketHandler<C2SPawnGetFavoritePawnListReq, S2CPawnGetFavoritePawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetFavoritePawnListHandler));


        public PawnGetFavoritePawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetFavoritePawnListRes Handle(GameClient client, C2SPawnGetFavoritePawnListReq request)
        {
            var result = new S2CPawnGetFavoritePawnListRes();
            Server.Database.ExecuteInTransaction(connection =>
            {
                var pawnFavorites = Server.Database.GetPawnFavorites(client.Character.CharacterId, connection);
                foreach (var pawnId in pawnFavorites)
                {
                    var pawn = Server.Database.SelectPawn(connection, pawnId);
                    if (!client.Character.RentedPawns.Where(x => x.PawnId == pawnId).Any())
                    {
                        result.FavoritePawnList.Add(new CDataRegisterdPawnList()
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
                }
            });
            return result;
        }
    }
}
