CREATE TABLE ddon_stamp_bonus
(
    "character_id"    		  INTEGER PRIMARY KEY NOT NULL,
    "last_stamp_time"      	  DATETIME  	NOT NULL,
    "consecutive_stamp"       INTEGER       NOT NULL,
    "total_stamp"             INTEGER       NOT NULL,
    CONSTRAINT fk_stamp_bonus_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

INSERT INTO "ddon_stamp_bonus" SELECT "character_id", "1900-01-01 00:00:00", 0, 0 FROM "ddon_character"