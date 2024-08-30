ALTER TABLE "ddon_quest_progress"
    ADD COLUMN "variant_quest_id" INTEGER NOT NULL DEFAULT 0;

ALTER TABLE "ddon_reward_box"
    ADD COLUMN "variant_quest_id" INTEGER NOT NULL DEFAULT 0;
