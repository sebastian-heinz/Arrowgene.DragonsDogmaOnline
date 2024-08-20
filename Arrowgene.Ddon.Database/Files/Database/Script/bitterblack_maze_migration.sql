ALTER TABLE "ddon_character" ADD "game_mode" INTEGER DEFAULT 1 NOT NULL;

CREATE TABLE "ddon_bbm_character_map" (
    "character_id"     INTEGER PRIMARY KEY NOT NULL,
    "bbm_character_id" INTEGER NOT NULL,
    CONSTRAINT fk_ddon_bbm_character_map_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_bbm_progress" (
    "character_id"     INTEGER PRIMARY KEY NOT NULL,
    "start_time"       INTEGER NOT NULL,
    "content_id"       INTEGER NOT NULL,
    "content_mode"     INTEGER NOT NULL,
    "tier"             INTEGER NOT NULL,
    "killed_death"     BOOLEAN NOT NULL,
    "last_ticket_time" INTEGER NOT NULL,
    CONSTRAINT fk_ddon_bbm_progress_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_bbm_rewards" (
    "character_id"  INTEGER PRIMARY KEY NOT NULL,
    "gold_marks"    INTEGER NOT NULL,
    "silver_marks"  INTEGER NOT NULL,
    "red_marks"     INTEGER NOT NULL,
    CONSTRAINT fk_ddon_bbm_rewards_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_bbm_content_treasure" (
    "character_id"  INTEGER PRIMARY KEY NOT NULL,
    "content_id"    INTEGER NOT NULL,
    "amount"        INTEGER NOT NULL,
    CONSTRAINT fk_ddon_bbm_content_treasure_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

INSERT INTO ddon_bbm_progress (character_id, start_time, content_id, content_mode, tier, killed_death, last_ticket_time)
SELECT character_id, 0, 0, 0, 0, 0, 0
FROM ddon_character;

INSERT INTO ddon_bbm_rewards (character_id, gold_marks, silver_marks, red_marks)
SELECT character_id, 0, 0, 0
FROM ddon_character;
