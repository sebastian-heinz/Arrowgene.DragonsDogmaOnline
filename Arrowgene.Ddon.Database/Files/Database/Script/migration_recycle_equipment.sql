INSERT INTO ddon_schedule_next(type, timestamp) VALUES (24, 0);

CREATE TABLE "ddon_recycle_equipment"
(
    "character_id" INTEGER NOT NULL,
    "num_attempts" INTEGER NOT NULL,
    CONSTRAINT pk_ddon_recycle_equipment PRIMARY KEY ("character_id"),
    CONSTRAINT fk_ddon_recycle_equipment_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
