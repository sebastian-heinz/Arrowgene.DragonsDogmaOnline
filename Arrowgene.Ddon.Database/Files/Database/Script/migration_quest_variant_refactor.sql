ALTER TABLE "ddon_priority_quests"
    RENAME COLUMN "quest_id" to "quest_schedule_id";

ALTER TABLE "ddon_reward_box" 
    RENAME COLUMN "quest_id" to "quest_schedule_id";

ALTER TABLE "ddon_reward_box"
    DROP "variant_quest_id";

ALTER TABLE "ddon_quest_progress"
    RENAME COLUMN "quest_id" to "quest_schedule_id";

ALTER TABLE "ddon_quest_progress"
    DROP "variant_quest_id";
