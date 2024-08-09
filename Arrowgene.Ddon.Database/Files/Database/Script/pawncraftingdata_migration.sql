ALTER TABLE "ddon_pawn"
    ADD COLUMN "craft_rank" INTEGER NOT NULL DEFAULT 1;
ALTER TABLE "ddon_pawn"
    ADD COLUMN "craft_rank_limit" INTEGER NOT NULL DEFAULT 71;
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
