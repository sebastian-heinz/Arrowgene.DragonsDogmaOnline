CREATE TABLE "ddon_dispel_seals"
(
    "character_id"      INTEGER NOT NULL,
    "seal_index"        INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_dispel_seals" PRIMARY KEY ("character_id", "seal_index"),
    CONSTRAINT "fk_ddon_dispel_seals_character_id" FOREIGN KEY ("character_id") references "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_bbm_reset_gg"
(
    "character_id"  INTEGER NOT NULL,
    "reset_count"   INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_bbm_reset_gg_ticket" PRIMARY KEY ("character_id"),
    CONSTRAINT "fk_ddon_bbm_reset_gg_ticket_character_id" FOREIGN KEY ("character_id") references "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_bbm_reset_ticket"
(
    "character_id"              INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_bbm_reset_ticket" PRIMARY KEY ("character_id"),
    CONSTRAINT "fk_ddon_bbm_reset_ticket_character_id" FOREIGN KEY ("character_id") references "ddon_character" ("character_id") ON DELETE CASCADE
);

INSERT INTO "ddon_schedule_next"(type, timestamp)
VALUES (20, 0);

DROP TABLE "ddon_bbm_rewards";
CREATE TABLE "ddon_bbm_rewards"
(
    "character_id" INTEGER      NOT NULL,
    "gold_marks"   INTEGER      NOT NULL,
    "silver_marks" INTEGER      NOT NULL,
    "red_marks"    INTEGER      NOT NULL,
    "stage_id"     INTEGER      NOT NULL,
    CONSTRAINT "pk_ddon_bbm_rewards" PRIMARY KEY ("character_id", "stage_id"),
    CONSTRAINT "fk_ddon_bbm_rewards_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

DROP TABLE "ddon_bbm_content_treasure";
CREATE TABLE "ddon_bbm_content_treasure"
(
    "character_id"  INTEGER  NOT NULL,
    "stage_id"      INTEGER  NOT NULL,
    "group_id"      INTEGER  NOT NULL,
    "index"         INTEGER  NOT NULL,
    CONSTRAINT "pk_ddon_bbm_content_treasure" PRIMARY KEY ("character_id", "stage_id", "group_id", "index"),
    CONSTRAINT "fk_ddon_bbm_content_treasure_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

