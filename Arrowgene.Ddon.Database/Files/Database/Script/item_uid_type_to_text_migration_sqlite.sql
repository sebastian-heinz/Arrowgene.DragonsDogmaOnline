PRAGMA foreign_keys=OFF;

CREATE TABLE "ddon_storage_item_new"
(
    "item_uid"     TEXT PRIMARY KEY NOT NULL,
    "character_id" INTEGER                NOT NULL,
    "storage_type" SMALLINT               NOT NULL,
    "slot_no"      SMALLINT               NOT NULL,
    "item_id"      INTEGER                NOT NULL,
    "item_num"     INTEGER                NOT NULL,
    "safety"       SMALLINT               NOT NULL,
    "color"        SMALLINT               NOT NULL,
    "plus_value"   SMALLINT               NOT NULL,
    "equip_points" INTEGER                NOT NULL,
    CONSTRAINT "uq_ddon_storage_item_character_id_storage_type_slot_no" UNIQUE ("character_id", "storage_type", "slot_no"),
    CONSTRAINT "fk_ddon_storage_item_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
INSERT INTO "ddon_storage_item_new" SELECT * FROM "ddon_storage_item";
DROP TABLE "ddon_storage_item";
ALTER TABLE "ddon_storage_item_new" RENAME TO "ddon_storage_item";

CREATE TABLE "ddon_equip_item_new"
(
    "item_uid"            TEXT NOT NULL,
    "character_common_id" INTEGER    NOT NULL,
    "job"                 SMALLINT   NOT NULL,
    "equip_type"          SMALLINT   NOT NULL,
    "equip_slot"          SMALLINT   NOT NULL,
    CONSTRAINT "pk_ddon_equip_item" PRIMARY KEY ("character_common_id", "job", "equip_type", "equip_slot"),
    CONSTRAINT "fk_ddon_equip_item_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);
INSERT INTO "ddon_equip_item_new" SELECT * FROM "ddon_equip_item";
DROP TABLE "ddon_equip_item";
ALTER TABLE "ddon_equip_item_new" RENAME TO "ddon_equip_item";

CREATE TABLE "ddon_equip_job_item_new"
(
    "item_uid"            TEXT NOT NULL,
    "character_common_id" INTEGER    NOT NULL,
    "job"                 SMALLINT   NOT NULL,
    "equip_slot"          SMALLINT   NOT NULL,
    CONSTRAINT "pk_ddon_equip_job_item" PRIMARY KEY ("character_common_id", "job", "equip_slot"),
    CONSTRAINT "fk_ddon_equip_job_item_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);
INSERT INTO "ddon_equip_job_item_new" SELECT * FROM "ddon_equip_job_item";
DROP TABLE "ddon_equip_job_item";
ALTER TABLE "ddon_equip_job_item_new" RENAME TO "ddon_equip_job_item";

CREATE TABLE "ddon_crests_new"
(
    "character_common_id" INTEGER    NOT NULL,
    "item_uid"            TEXT NOT NULL,
    "slot"                INTEGER    NOT NULL,
    "crest_id"            INTEGER    NOT NULL,
    "crest_amount"        INTEGER    NOT NULL,
    CONSTRAINT "fk_ddon_crests_item_uid" FOREIGN KEY ("item_uid") REFERENCES "ddon_storage_item" ("item_uid") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_crests_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);
INSERT INTO "ddon_crests_new" SELECT * FROM "ddon_crests";
DROP TABLE "ddon_crests";
ALTER TABLE "ddon_crests_new" RENAME TO "ddon_crests";

CREATE INDEX IF NOT EXISTS "idx_ddon_crests_item_uid" ON "ddon_crests" ("item_uid");
CREATE INDEX IF NOT EXISTS "idx_ddon_crests_character_item" ON "ddon_crests" ("character_common_id", "item_uid");

CREATE TABLE "ddon_equipment_limit_break_new"
(
    "character_id"     INTEGER    NOT NULL,
    "item_uid"         TEXT NOT NULL,
    "effect_1"         INTEGER    NOT NULL,
    "effect_2"         INTEGER    NOT NULL,
    "is_effect1_valid" BOOLEAN    NOT NULL,
    "is_effect2_valid" BOOLEAN    NOT NULL,
    CONSTRAINT "pk_ddon_equipment_limit_break" PRIMARY KEY ("character_id", "item_uid"),
    CONSTRAINT "fk_ddon_equipment_limit_break_item_uid" FOREIGN KEY ("item_uid") REFERENCES "ddon_storage_item" ("item_uid") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_equipment_limit_break_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
INSERT INTO "ddon_equipment_limit_break_new" SELECT * FROM "ddon_equipment_limit_break";
DROP TABLE "ddon_equipment_limit_break";
ALTER TABLE "ddon_equipment_limit_break_new" RENAME TO "ddon_equipment_limit_break";

CREATE INDEX IF NOT EXISTS "idx_ddon_equipment_limit_break_item_uid" ON "ddon_equipment_limit_break" ("item_uid");

PRAGMA foreign_keys=ON;
