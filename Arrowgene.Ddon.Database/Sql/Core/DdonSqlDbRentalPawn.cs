using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public partial class DdonSqlDb : SqlDb
    {
        private static readonly string[] RentalPawnFields =
        [
            "hiring_character_id", "pawn_id", "data", "data_size", "adventure_count", "craft_count", "kill_count",
        ];

        private readonly string SqlInsertRentalPawn =
            $"INSERT INTO \"ddon_rental_pawn\" ({BuildQueryField(RentalPawnFields)}) VALUES ({BuildQueryInsert(RentalPawnFields)}) ON CONFLICT DO NOTHING;";

        private readonly string SqlUpdateRentalPawn =
            """UPDATE "ddon_rental_pawn" SET "adventure_count" = @adventure_count, "craft_count" = @craft_count, "kill_count" = @kill_count WHERE "hiring_character_id" = @hiring_character_id AND "pawn_id" = @pawn_id;""";

        private readonly string SqlDeleteRentalPawn =
            "DELETE FROM \"ddon_rental_pawn\" WHERE \"hiring_character_id\"=@hiring_character_id AND \"pawn_id\"=@pawn_id;";

        private readonly string SqlSelectRentalPawns =
            $"SELECT {BuildQueryField(RentalPawnFields)} FROM \"ddon_rental_pawn\" WHERE \"hiring_character_id\" = @hiring_character_id;";

        private readonly string SqlSelectCDataCommunityCharacterBaseInfo = """
            SELECT 
                "ddon_character"."character_id", 
                "ddon_character"."first_name", 
                "ddon_character"."last_name", 
                "ddon_clan_param"."short_name"
            FROM "ddon_character"
            LEFT OUTER JOIN "ddon_clan_membership" on "ddon_character"."character_id" = "ddon_clan_membership"."character_id"
            LEFT OUTER JOIN "ddon_clan_param" on "ddon_clan_param"."clan_id" = "ddon_clan_membership"."clan_id"
            WHERE "ddon_character"."character_id" = @character_id;
            """;

        private static readonly string[] RentalPawnFeedbackFields =
        [
            "hiring_character_id", "pawn_id", "hire_date", "return_date", "adventure_count", "craft_count", "kill_count", 
            "appearance_score", "appearance_comment", "combat_score", "combat_comment", "craft_score", "craft_comment"
        ];

        private readonly string SqlInsertRentalPawnFeedback =
            $"""INSERT INTO "ddon_rental_pawn_feedback" ({BuildQueryField(RentalPawnFeedbackFields)}) VALUES ({BuildQueryInsert(RentalPawnFeedbackFields)});""";

        private readonly string SqlSelectRentalPawnHistory =
            $"""
            SELECT 
                {BuildQueryField("ddon_rental_pawn_feedback", RentalPawnFeedbackFields)},
                "ddon_character"."first_name" as "debtor_first_name", 
                "ddon_character"."last_name" as "debtor_last_name", 
                "ddon_clan_param"."short_name" as "debtor_clan_name"
            FROM "ddon_rental_pawn_feedback"
            LEFT OUTER JOIN "ddon_character" on "ddon_character"."character_id" = "ddon_rental_pawn_feedback"."hiring_character_id"
            LEFT OUTER JOIN "ddon_clan_membership" on "ddon_rental_pawn_feedback"."hiring_character_id" = "ddon_clan_membership"."character_id"
            LEFT OUTER JOIN "ddon_clan_param" on "ddon_clan_param"."clan_id" = "ddon_clan_membership"."clan_id"
            WHERE "ddon_rental_pawn_feedback"."pawn_id" = @pawn_id
            ORDER BY "ddon_rental_pawn_feedback"."return_date" DESC
            LIMIT 100;
            """;

        private readonly string SqlSelectAveragePawnFeedback =
            $"""
            SELECT
                COUNT("ddon_rental_pawn_feedback"."return_date") as "rental_count",
                SUM("ddon_rental_pawn_feedback"."kill_count") as "kill_count",
                SUM("ddon_rental_pawn_feedback"."craft_count") as "craft_count",
                AVG("ddon_rental_pawn_feedback"."appearance_score") as "average_appearance",
            	STRING_AGG(CAST("ddon_rental_pawn_feedback"."appearance_comment" AS TEXT), '') as "appearance_comment",
                AVG("ddon_rental_pawn_feedback"."combat_score") as "average_combat",
            	STRING_AGG(CAST("ddon_rental_pawn_feedback"."combat_comment" AS TEXT), '') as "combat_comment",
                AVG("ddon_rental_pawn_feedback"."craft_score") as "average_craft",
            	STRING_AGG(CAST("ddon_rental_pawn_feedback"."craft_comment" AS TEXT), '') as "craft_comment"
                FROM "ddon_rental_pawn_feedback"
            WHERE "ddon_rental_pawn_feedback"."pawn_id" = @pawn_id
            GROUP BY "ddon_rental_pawn_feedback"."pawn_id";
            """;

        public override List<RentalPawn> SelectRentalPawns(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                List<RentalPawn> rentalPawns = new();
                ExecuteReader(connection,
                    SqlSelectRentalPawns,
                    command => { AddParameter(command, "@hiring_character_id", characterId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            byte adventureCount = GetByte(reader, "adventure_count");
                            byte craftCount = GetByte(reader, "craft_count");
                            uint killCount = GetUInt32(reader, "kill_count");

                            int dataSize = GetInt32(reader, "data_size");
                            byte[] data = GetBytes(reader, "data", dataSize);

                            var jsonString = Util.DecompressJSON(data);
                            var rentalRecord = JsonSerializer.Deserialize<RentalPawnRecord>(jsonString);
                            var rentalPawn = rentalRecord.ToRentalPawn(characterId, adventureCount, craftCount, killCount);
                            rentalPawns.Add(rentalPawn);
                        }
                    });
                return rentalPawns;
            });
        }

        public override bool InsertRentalPawn(uint characterId, RentalPawnRecord record, byte adventureCount, byte craftCount, DbConnection? connectionIn = null)
        {
            string jsonString = JsonSerializer.Serialize(record);
            byte[] data = Util.CompressJSON(jsonString);
            int dataSize = data.Length;

            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(
                connection,
                SqlInsertRentalPawn,
                command =>
                {
                    AddParameter(command, "@hiring_character_id", characterId);
                    AddParameter(command, "@pawn_id", record.PawnId);
                    AddParameter(command, "@data", data);
                    AddParameter(command, "@data_size", dataSize);
                    AddParameter(command, "@adventure_count", adventureCount);
                    AddParameter(command, "@craft_count", craftCount);
                    AddParameter(command, "@pawn_state", (byte)PawnState.None);
                    AddParameter(command, "@kill_count", 0);
                }
                ) == 1;
            });
        }

        public override bool UpdateRentalPawn(uint characterId, RentalPawn pawn, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(
                connection,
                SqlUpdateRentalPawn,
                command =>
                {
                    AddParameter(command, "@hiring_character_id", characterId);
                    AddParameter(command, "@pawn_id", pawn.PawnId);
                    AddParameter(command, "@adventure_count", pawn.AdventureCount);
                    AddParameter(command, "@craft_count", pawn.CraftCount);
                    AddParameter(command, "@pawn_state", (byte)pawn.PawnState);
                    AddParameter(command, "@kill_count", pawn.KillCount);
                }
                ) == 1;
            });
        }

        public override bool DeleteRentalPawn(uint characterId, uint pawnId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(
                connection,
                SqlDeleteRentalPawn,
                command =>
                {
                    AddParameter(command, "@hiring_character_id", characterId);
                    AddParameter(command, "@pawn_id", pawnId);
                }
                ) == 1;
            });
        }

        public override CDataCommunityCharacterBaseInfo SelectCommunityCharacterBaseInfo(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                CDataCommunityCharacterBaseInfo data = new();
                ExecuteReader(
                    connection,
                    SqlSelectCDataCommunityCharacterBaseInfo,
                    command => { AddParameter(command, "@character_id", characterId); },
                    reader =>
                    {
                        if (reader.Read())
                        {
                            data.CharacterId = GetUInt32(reader, "character_id");
                            data.CharacterName = new CDataCharacterName
                            {
                                FirstName = GetString(reader, "first_name"),
                                LastName = GetString(reader, "last_name")
                            };
                            data.ClanName = GetStringNullable(reader, "short_name") ?? string.Empty;
                        }
                    }
                );
                return data;
            });
        }

        public override bool InsertRentalPawnFeedback(uint characterId, RentalPawn pawn, List<CDataPawnFeedback> pawnFeedbacks, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(
                connection,
                SqlInsertRentalPawnFeedback,
                command =>
                {
                    AddParameter(command, "@hiring_character_id", characterId);
                    AddParameter(command, "@pawn_id", pawn.PawnId);
                    AddParameter(command, "@hire_date", pawn.HireDate);
                    AddParameter(command, "@return_date", DateTime.UtcNow);
                    AddParameter(command, "@adventure_count", pawn.MaxAdventureCount - pawn.AdventureCount);
                    AddParameter(command, "@craft_count", pawn.MaxCraftCount - pawn.CraftCount);
                    AddParameter(command, "@kill_count", pawn.KillCount);
                    AddParameter(command, "@appearance_score", pawnFeedbacks.Where(x => x.Type == 0).FirstOrDefault()?.Value, System.Data.DbType.Byte);
                    AddParameter(command, "@appearance_comment", pawnFeedbacks.Where(x => x.Type == 0).FirstOrDefault()?.CommentNo, System.Data.DbType.Byte);
                    AddParameter(command, "@combat_score", pawnFeedbacks.Where(x => x.Type == 1).FirstOrDefault()?.Value, System.Data.DbType.Byte);
                    AddParameter(command, "@combat_comment", pawnFeedbacks.Where(x => x.Type == 1).FirstOrDefault()?.CommentNo, System.Data.DbType.Byte);
                    AddParameter(command, "@craft_score", pawnFeedbacks.Where(x => x.Type == 2).FirstOrDefault()?.Value, System.Data.DbType.Byte);
                    AddParameter(command, "@craft_comment", pawnFeedbacks.Where(x => x.Type == 2).FirstOrDefault()?.CommentNo, System.Data.DbType.Byte);
                }
                ) == 1;
            });
        }

        public override List<CDataPawnHistory> SelectPawnHistory(uint pawnId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                List<CDataPawnHistory> historyList = [];
                ExecuteReader(connection,
                    SqlSelectRentalPawnHistory,
                    command => { AddParameter(command, "@pawn_id", pawnId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            CDataPawnHistory history = new()
                            {
                                PawnId = pawnId,
                                DebtorBaseInfo = new()
                                {
                                    CharacterId = GetUInt32(reader, "hiring_character_id"),
                                    CharacterName = new()
                                    {
                                        FirstName = GetStringNullable(reader, "debtor_first_name") ?? string.Empty,
                                        LastName = GetStringNullable(reader, "debtor_last_name") ?? string.Empty,
                                    },
                                    ClanName = GetStringNullable(reader, "debtor_clan_name") ?? string.Empty,
                                },
                                ReturnDate = GetDateTime(reader, "return_date"),
                                AdventureTime = GetDateTime(reader, "return_date") - GetDateTime(reader, "hire_date"), // Maybe?
                                AdventureCount = GetByte(reader, "adventure_count"),
                                CraftCount = GetByte(reader, "craft_count"),
                                KillEnemyNum = GetUInt32(reader, "kill_count"),
                            };

                            List<CDataPawnFeedback> feedback = new();
                            if (!reader.IsDBNull(reader.GetOrdinal("appearance_score")))
                            {
                                feedback.Add(new()
                                {
                                    Type = 0,
                                    Value = GetByte(reader, "appearance_score"),
                                    CommentNo = GetByte(reader, "appearance_comment")
                                });
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("combat_score")))
                            {
                                feedback.Add(new()
                                {
                                    Type = 1,
                                    Value = GetByte(reader, "combat_score"),
                                    CommentNo = GetByte(reader, "combat_comment")
                                });
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("craft_score")))
                            {
                                feedback.Add(new()
                                {
                                    Type = 2,
                                    Value = GetByte(reader, "craft_score"),
                                    CommentNo = GetByte(reader, "craft_comment")
                                });
                            }
                            history.PawnFeedback = feedback.OrderByDescending(x => x.Value).FirstOrDefault(new CDataPawnFeedback());

                            historyList.Add(history);
                        }
                    });
                return historyList;
            });
        }
    
        public override CDataPawnTotalScore SelectPawnTotalScore(uint pawnId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                CDataPawnTotalScore data = new();
                ExecuteReader(
                    connection,
                    SqlSelectAveragePawnFeedback,
                    command => { AddParameter(command, "@pawn_id", pawnId); },
                    reader =>
                    {
                        if (reader.Read())
                        {
                            data.RentalCount = GetUInt32(reader, "rental_count");
                            data.BattleCount = GetUInt32(reader, "kill_count");
                            data.CraftCount = GetUInt32(reader, "craft_count");

                            // These string/char/byte shenanigans is a janky way of accessing the statistical mode of the comments.
                            // Postgres has a MODE() aggregate, but SQLite does not.
                            // Maybe this is overengineered.
                            if (!reader.IsDBNull(reader.GetOrdinal("average_appearance")))
                            {
                                data.AveragePawnFeedbackList.Add(new()
                                {
                                    Type = 0,
                                    Value = (byte)Math.Round(GetFloat(reader, "average_appearance")),
                                    CommentNo = (byte)((byte)GetString(reader, "appearance_comment").Mode() - 0x30)
                                });
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("average_combat")))
                            {
                                data.AveragePawnFeedbackList.Add(new()
                                {
                                    Type = 1,
                                    Value = (byte)Math.Round(GetFloat(reader, "average_combat")),
                                    CommentNo = (byte)((byte)GetString(reader, "combat_comment").Mode() - 0x30)
                                });
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("average_craft")))
                            {
                                data.AveragePawnFeedbackList.Add(new()
                                {
                                    Type = 2,
                                    Value = (byte)Math.Round(GetFloat(reader, "average_craft")),
                                    CommentNo = (byte)((byte)GetString(reader, "craft_comment").Mode() - 0x30)
                                });
                            }
                        }
                    }
                );
                return data;
            });
        }
    }
}
