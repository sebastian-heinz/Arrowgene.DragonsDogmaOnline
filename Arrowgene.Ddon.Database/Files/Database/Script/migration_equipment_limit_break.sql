CREATE TABLE "ddon_equipment_limit_break"
(
    "character_id" INTEGER NOT NULL,
    "item_uid"     VARCHAR(8) NOT NULL,
    "effect_1"     INTEGER NOT NULL,
    "effect_2"     INTEGER NOT NULL,
    "is_effect1_valid" BOOLEAN NOT NULL,
    "is_effect2_valid" BOOLEAN NOT NULL,
    CONSTRAINT pk_ddon_equipment_limit_break PRIMARY KEY ("character_id", "item_uid"),
    CONSTRAINT fk_ddon_equipment_limit_break_item_uid FOREIGN KEY ("item_uid") REFERENCES "ddon_storage_item" ("item_uid") ON DELETE CASCADE,
    CONSTRAINT fk_ddon_equipment_limit_break_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
