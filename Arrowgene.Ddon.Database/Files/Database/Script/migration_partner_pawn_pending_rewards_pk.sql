CREATE TABLE "ddon_partner_pawn_pending_rewards_new"
(
    "character_id" INTEGER NOT NULL,
    "pawn_id"      INTEGER NOT NULL,
    "reward_level" INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_partner_pawn_pending_rewards" PRIMARY KEY ("character_id", "pawn_id", "reward_level"),
    CONSTRAINT "fk_ddon_partner_pawn_pending_rewards_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_partner_pawn_pending_rewards_pawn_id" FOREIGN KEY ("pawn_id") REFERENCES "ddon_pawn" ("pawn_id") ON DELETE CASCADE
);

INSERT INTO "ddon_partner_pawn_pending_rewards_new" ("character_id", "pawn_id", "reward_level")
SELECT "character_id", "pawn_id", "reward_level" FROM "ddon_partner_pawn_pending_rewards";

DROP TABLE "ddon_partner_pawn_pending_rewards";

ALTER TABLE "ddon_partner_pawn_pending_rewards_new" RENAME TO "ddon_partner_pawn_pending_rewards";