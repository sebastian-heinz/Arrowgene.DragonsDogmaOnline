CREATE TABLE "ddon_area_rank" (
    "character_id"      INTEGER NOT NULL,
    "area_id"           INTEGER NOT NULL,
    "rank"              INTEGER NOT NULL,
    "point"             INTEGER NOT NULL,
    "week_point"        INTEGER NOT NULL,
    "last_week_point"   INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_area_rank" PRIMARY KEY ("character_id", "area_id"),
	CONSTRAINT "fk_ddon_area_rank_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_area_rank_supply" (
    "character_id"      INTEGER NOT NULL,
    "area_id"           INTEGER NOT NULL,
    "index"             INTEGER NOT NULL,
    "item_id"           INTEGER NOT NULL,
    "num"               INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_area_rank_supply" PRIMARY KEY ("character_id", "area_id", "index"),
	CONSTRAINT "fk_ddon_area_rank_supply_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);

CREATE TABLE "temp_area_rank"
(    
    "character_id"      INTEGER NOT NULL,
    "area_id"           INTEGER NOT NULL,
    "rank"              INTEGER NOT NULL,
    "point"             INTEGER NOT NULL,
    "week_point"        INTEGER NOT NULL,
    "last_week_point"   INTEGER NOT NULL
);

INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 1, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 2, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 3, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 4, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 5, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 6, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 7, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 8, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 9, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 10, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 11, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 12, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 13, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 14, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 15, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 16, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 17, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 18, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 19, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 20, 0, 0, 0, 0 FROM "ddon_character";
INSERT INTO "temp_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") SELECT "character_id", 21, 0, 0, 0, 0 FROM "ddon_character";

INSERT INTO "ddon_area_rank" ("character_id", "area_id", "rank", "point", "week_point", "last_week_point") 
    SELECT * FROM "temp_area_rank"
    ORDER BY "character_id", "area_id";

DROP TABLE "temp_area_rank";

INSERT INTO "ddon_schedule_next" ("type", "timestamp") VALUES (4, 0);
