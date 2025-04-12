CREATE TABLE "ddon_achievement"
(
    "character_id"      INTEGER NOT NULL,
    "achievement_id"    INTEGER NOT NULL,
    "date"              DATETIME NOT NULL,
    CONSTRAINT pk_ddon_achievement PRIMARY KEY ("character_id", "achievement_id"),
    CONSTRAINT fk_ddon_achievement_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_achievement_progress"
(
    "character_id"      INTEGER NOT NULL,
    "achievement_type"  INTEGER NOT NULL,
    "achievement_param" INTEGER NOT NULL,
    "progress"          INTEGER NOT NULL,
    CONSTRAINT pk_ddon_achievement_progress PRIMARY KEY ("character_id", "achievement_type", "achievement_param"),
    CONSTRAINT fk_ddon_achievement_progress_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_achievement_unique_crafts"
(
    "character_id"      INTEGER NOT NULL,
    "item_id"           INTEGER NOT NULL,
    "craft_type"        TINYINT NOT NULL,
    CONSTRAINT pk_ddon_achievement_unique_crafts PRIMARY KEY ("character_id", "item_id"),
    CONSTRAINT fk_ddon_achievement_unique_crafts_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_unlocked_items"
(
    "character_id"      INTEGER NOT NULL,
    "category"          INTEGER NOT NULL,
    "item_id"           INTEGER NOT NULL,
    CONSTRAINT pk_ddon_unlocked_items PRIMARY KEY ("character_id", "category", "item_id"),
    CONSTRAINT fk_ddon_unlocked_items_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_myroom_customization"
(
    "character_id"      INTEGER NOT NULL,
    "layout_id"         TINYINT NOT NULL,
    "item_id"           INTEGER NOT NULL,
    CONSTRAINT pk_ddon_myroom_customization PRIMARY KEY ("character_id", "layout_id"),
    CONSTRAINT fk_ddon_myroom_customization_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
