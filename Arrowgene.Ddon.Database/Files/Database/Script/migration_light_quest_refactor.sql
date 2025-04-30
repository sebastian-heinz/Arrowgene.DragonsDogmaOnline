CREATE TABLE "ddon_light_quests"
(
    "variant_id"            INTEGER     NOT NULL,
    "quest_schedule_id"     INTEGER     NOT NULL,
    "quest_id"              INTEGER     NOT NULL,
    "target"                INTEGER     NOT NULL,
    "level"                 INTEGER     NOT NULL,
    "count"                 INTEGER     NOT NULL,
    "reward_xp"             INTEGER     NOT NULL,
    "reward_g"              INTEGER     NOT NULL,
    "reward_r"              INTEGER     NOT NULL,
    "reward_ap"             INTEGER     NOT NULL,
    "distribution_end"      DATETIME    NOT NULL,
    CONSTRAINT "pk_ddon_light_quests_variant_id" PRIMARY KEY ("variant_id")
);
INSERT INTO "ddon_schedule_next"(type, timestamp)
VALUES (6, 0);

DELETE FROM ddon_priority_quests;
DELETE FROM ddon_rank_record;
