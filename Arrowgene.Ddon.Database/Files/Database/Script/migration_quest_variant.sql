
ALTER TABLE "ddon_quest_progress"
    ADD COLUMN "variant_quest_id" INTEGER NOT NULL DEFAULT 0;

ALTER TABLE "ddon_reward_box"
    ADD COLUMN "variant_quest_id" INTEGER NOT NULL DEFAULT 0;

UPDATE "ddon_quest_progress" SET "quest_id" = 20055001 WHERE "quest_id" = 2005501;
UPDATE "ddon_quest_progress" SET "variant_quest_id" = 102030 WHERE "quest_id" = 20055001;

UPDATE "ddon_priority_quests" SET "quest_id" = 20055001 WHERE "quest_id" = 2005501;

UPDATE "ddon_reward_box" SET "quest_id" = 20055001 WHERE "quest_id" = 2005501;

UPDATE "ddon_completed_quests" SET "quest_id" = 20055001 WHERE "quest_id" = 2005501;
