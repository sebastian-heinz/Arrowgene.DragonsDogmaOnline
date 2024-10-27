CREATE TABLE "ddon_equip_preset"
(
    "character_common_id" INTEGER NOT NULL,
    "preset_no"           INTEGER NOT NULL,
    "job_id"              INTEGER NOT NULL,
    "preset_name"         TEXT    NOT NULL,
    CONSTRAINT "pk_ddon_equip_preset" PRIMARY KEY ("character_common_id", "job_id", "preset_no"),  
    CONSTRAINT "fk_ddon_equip_preset_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_equip_preset_template"
(
    "character_common_id" INTEGER NOT NULL,
    "preset_no"           INTEGER NOT NULL,
    "job_id"              INTEGER NOT NULL,
    "slot_no"             INTEGER NOT NULL,
    "item_uid"            VARCHAR(8) NOT NULL,
    CONSTRAINT "pk_ddon_equip_preset_template" PRIMARY KEY ("character_common_id", "job_id", "preset_no", "slot_no"),    
    CONSTRAINT "fk_ddon_equip_preset_template_character_common_id" FOREIGN KEY ("character_common_id", "job_id", "preset_no") REFERENCES "ddon_equip_preset" ("character_common_id", "job_id", "preset_no") ON DELETE CASCADE
);
