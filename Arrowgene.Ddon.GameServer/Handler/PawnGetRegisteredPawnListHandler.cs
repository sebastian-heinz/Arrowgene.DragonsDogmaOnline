using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRegisteredPawnListHandler : GameRequestPacketHandler<C2SPawnGetRegisteredPawnListReq, S2CPawnGetRegisteredPawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRegisteredPawnListHandler));

        public PawnGetRegisteredPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetRegisteredPawnListRes Handle(GameClient client, C2SPawnGetRegisteredPawnListReq request)
        {
            var result = new S2CPawnGetRegisteredPawnListRes();
            
            List<(Pawn pawn, CDataCharacterSearchParam ownerName)> pawnInfo = new List<(Pawn pawn, CDataCharacterSearchParam ownerName)>();
            Server.Database.ExecuteInTransaction(connection =>
            {
                var pawnIds = Server.Database.SelectAllPlayerPawns(connection);
                foreach (var pawnId in pawnIds)
                {
                    var pawn = Server.Database.SelectPawn(connection, pawnId);
                    var ownerName = Server.Database.SelectCharacterNameById(connection, pawn.CharacterId);
                    pawnInfo.Add((pawn, ownerName));
                }
            });

            var searchParams = request.SearchParam;
            foreach (var (pawn, ownerName) in pawnInfo)
            {
                if (pawn.CharacterId == client.Character.CharacterId)
                {
                    continue;
                }

                if (searchParams.OwnerCharacterName.FirstName != "" || searchParams.OwnerCharacterName.LastName != "")
                {
                    var firstName = searchParams.OwnerCharacterName.FirstName;
                    var lastName = searchParams.OwnerCharacterName.LastName;

                    if (!ownerName.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (!ownerName.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                }

                if (searchParams.Sex != PawnSex.Any && searchParams.Sex != (PawnSex) pawn.EditInfo.Sex)
                {
                    continue;
                }

                if (searchParams.CharacterParam.VocationMin != 0)
                {
                    var characterParams = searchParams.CharacterParam;
                    if ((pawn.ActiveCharacterJobData.Lv < characterParams.VocationMin) || (pawn.ActiveCharacterJobData.Lv > characterParams.VocationMax))
                    {
                        continue;
                    }
                }

                if (searchParams.CharacterParam.ItemRankMin != 0)
                {
                    uint itemRank = Server.EquipManager.CalculateItemRank(Server, pawn);
                    if (itemRank < searchParams.CharacterParam.ItemRankMin || itemRank > searchParams.CharacterParam.ItemRankMax)
                    {
                        continue;
                    }
                }

                bool shouldSkip = false;
                foreach (var desiredSkillLevel in searchParams.CraftSkillList)
                {
                    var pawnSkill = pawn.CraftData.PawnCraftSkillList.Where(x => x.Type == desiredSkillLevel.Type).FirstOrDefault();
                    if (pawnSkill.Level < desiredSkillLevel.Level)
                    {
                        shouldSkip = true;
                        break;
                    }
                }

                if (shouldSkip)
                {
                    continue;
                }

                if (searchParams.DragonAbilitiesList.Count > 0)
                {
                    // Dragon Abilities
                    // TODO: Filter on dragon abilities
                }

                // If a job or jobs are selected, the Job Value is encoded as
                // 0b1111_1111_1111_1111_1111_nnnn_nnnn_nnn1
                // Were the value n = 0 meaning not set or n = 1 which means set
                // The n bits are the value (1 << JobId)
                // If the Value of Job is zero, than any job is valid search criteria
                if (searchParams.CharacterParam.Job != 0 && ((1 << (int) pawn.Job) & searchParams.CharacterParam.Job) == 0)
                {
                    continue;
                }

                result.RegisterdPawnList.Add(new CDataRegisterdPawnList()
                {
                    Name = pawn.Name,
                    PawnId = pawn.PawnId,
                    RentalCost = pawn.ActiveCharacterJobData.Lv * 10,
                    Sex = pawn.EditInfo.Sex,     
                    Updated = 0,
                    PawnListData = new CDataPawnListData()
                    {
                        Job = pawn.ActiveCharacterJobData.Job,
                        Level = pawn.ActiveCharacterJobData.Lv,
                        CraftRank = pawn.CraftData.CraftRank,
                    }
                });
            }

            return result;
        }
    }
}
