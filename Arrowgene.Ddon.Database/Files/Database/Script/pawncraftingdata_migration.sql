ALTER TABLE "ddon_pawn"
    ADD COLUMN "craft_rank" INTEGER NOT NULL DEFAULT 1;
ALTER TABLE "ddon_pawn"
    ADD COLUMN "craft_rank_limit" INTEGER NOT NULL DEFAULT 8;
ALTER TABLE "ddon_pawn"
    ADD COLUMN "craft_exp" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "ddon_pawn"
    ADD COLUMN "craft_points" INTEGER NOT NULL DEFAULT 0;

ALTER TABLE "ddon_pawn"
    ADD COLUMN "production_speed_level" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "ddon_pawn"
    ADD COLUMN "equipment_enhancement_level" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "ddon_pawn"
    ADD COLUMN "equipment_quality_level" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "ddon_pawn"
    ADD COLUMN "consumable_quantity_level" INTEGER NOT NULL DEFAULT 0;
ALTER TABLE "ddon_pawn"
    ADD COLUMN "cost_performance_level" INTEGER NOT NULL DEFAULT 0;

CREATE TABLE IF NOT EXISTS "ddon_pawn_craft_progress"
(
    "craft_character_id"     INTEGER NOT NULL,
    "craft_lead_pawn_id"     INTEGER NOT NULL,
    "craft_support_pawn_id1" INTEGER NOT NULL,
    "craft_support_pawn_id2" INTEGER NOT NULL,
    "craft_support_pawn_id3" INTEGER NOT NULL,

    "recipe_id"              INTEGER NOT NULL,
    "exp"                    INTEGER NOT NULL,
    "npc_action_id"          INTEGER NOT NULL,
    "item_id"                INTEGER NOT NULL,
    "unk0"                   INTEGER NOT NULL,
    "remain_time"            INTEGER NOT NULL,
    "exp_bonus"              BOOLEAN NOT NULL,
    "create_count"           INTEGER NOT NULL,

    "plus_value"             INTEGER NOT NULL,
    "great_success"          BOOLEAN NOT NULL,
    "bonus_exp"              INTEGER NOT NULL,
    "additional_quantity"    INTEGER NOT NULL,
    FOREIGN KEY ("craft_character_id") REFERENCES "ddon_character" ("character_id"),
    FOREIGN KEY ("craft_lead_pawn_id") REFERENCES "ddon_pawn" ("pawn_id")
);
