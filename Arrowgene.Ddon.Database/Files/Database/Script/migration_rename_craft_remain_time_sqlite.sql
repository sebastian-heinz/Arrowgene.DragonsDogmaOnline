ALTER TABLE "ddon_pawn_craft_progress" RENAME COLUMN "remain_time" TO "finish_at";
INSERT INTO "ddon_schedule_next" ("type", "timestamp") VALUES (25, 0);