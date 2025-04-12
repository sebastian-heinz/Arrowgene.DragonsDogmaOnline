ALTER TABLE "ddon_character"
    ADD COLUMN "partner_pawn_id" INTEGER NOT NULL DEFAULT 0;

CREATE TABLE "ddon_partner_pawn"
(
    "character_id" INTEGER NOT NULL,
    "pawn_id" INTEGER NOT NULL,
    "num_gifts" INTEGER NOT NULL,
    "num_crafts" INTEGER NOT NULL,
    "num_adventures" INTEGER NOT NULL,
    CONSTRAINT pk_ddon_partner_pawn PRIMARY KEY ("character_id", "pawn_id"),
    CONSTRAINT fk_ddonpartner_pawn_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_partner_pawn_last_affection_increase"
(
    "character_id" INTEGER NOT NULL,
    "pawn_id" INTEGER NOT NULL,
    "action" INTEGER NOT NULL,
    CONSTRAINT pk_ddon_partner_pawn_last_affection_increase PRIMARY KEY ("character_id", "pawn_id", "action"),
    CONSTRAINT fk_ddon_partner_pawn_affection_increase_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
INSERT INTO ddon_schedule_next(type, timestamp) VALUES (13, 0);

CREATE TABLE "ddon_partner_pawn_pending_rewards"
(
    "character_id" INTEGER NOT NULL,
    "pawn_id" INTEGER NOT NULL,
    "reward_level" INTEGER NOT NULL,
    CONSTRAINT pk_ddon_partner_pawn_pending_rewards PRIMARY KEY ("character_id", "pawn_id"),
    CONSTRAINT fk_ddon_partner_pawn_pending_rewards_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
