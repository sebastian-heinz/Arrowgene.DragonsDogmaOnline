CREATE TABLE ddon_storage_item_temp
(
    "item_uid"     VARCHAR(8) NOT NULL PRIMARY KEY,
    "character_id" INTEGER    NOT NULL,
    "storage_type" SMALLINT   NOT NULL,
    "slot_no"      SMALLINT   NOT NULL,
    "item_id"      INTEGER    NOT NULL,
    "item_num"     INTEGER    NOT NULL,
    "unk3"         SMALLINT   NOT NULL,
    "color"        SMALLINT   NOT NULL,
    "plus_value"   SMALLINT   NOT NULL,
    CONSTRAINT pk_ddon_storage_item UNIQUE (character_id, storage_type, slot_no),
    CONSTRAINT fk_storage_item_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

CREATE TABLE ddon_equip_item_temp
(
    "item_uid"            VARCHAR(8) NOT NULL,
    "character_common_id" INTEGER    NOT NULL,
    "job"                 SMALLINT   NOT NULL,
    "equip_type"          SMALLINT   NOT NULL,
    "equip_slot"          SMALLINT   NOT NULL,
    CONSTRAINT pk_ddon_equip_item PRIMARY KEY (character_common_id, job, equip_type, equip_slot),
    CONSTRAINT fk_equip_item_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE ddon_equip_job_item_temp
(
    "item_uid"            VARCHAR(8) NOT NULL,
    "character_common_id" INTEGER    NOT NULL,
    "job"                 SMALLINT   NOT NULL,
    "equip_slot"          SMALLINT   NOT NULL,
    CONSTRAINT pk_ddon_equip_job_item PRIMARY KEY (character_common_id, job, equip_slot),
    CONSTRAINT fk_equip_job_item_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);