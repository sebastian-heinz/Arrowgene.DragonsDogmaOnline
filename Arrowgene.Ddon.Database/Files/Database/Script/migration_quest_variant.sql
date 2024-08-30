DELETE FROM "ddon_quest_progress" WHERE "quest_id" = 2005501;

DELETE FROM "ddon_priority_quests" WHERE "quest_id" = 2005501;

DELETE FROM "ddon_reward_box" WHERE "quest_id" = 2005501;

DELETE FROM "ddon_completed_quests" WHERE "quest_id" = 2005501;

ALTER TABLE "ddon_quest_progress"
    ADD COLUMN "variant_quest_id" INTEGER NOT NULL DEFAULT 0;

ALTER TABLE "ddon_reward_box"
    ADD COLUMN "variant_quest_id" INTEGER NOT NULL DEFAULT 0;
