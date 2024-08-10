CREATE TABLE "ddon_preset_ability" (
    "character_id"	INTEGER NOT NULL,
    "preset_no"	    INTEGER NOT NULL,
    "preset_name"	TEXT,
    "ability_1"	    INTEGER,
    "ability_2"	    INTEGER,
    "ability_3"	    INTEGER,
    "ability_4"	    INTEGER,
    "ability_5"	    INTEGER,
    "ability_6"	    INTEGER,
    "ability_7"	    INTEGER,
    "ability_8"	    INTEGER,
    "ability_9" 	INTEGER,
    "ability_10"	INTEGER,
    CONSTRAINT "fk_preset_ability" FOREIGN KEY("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
    CONSTRAINT "pk_preset_ability" PRIMARY KEY("character_id","preset_no")
);

DELETE FROM "ddon_equipped_ability";