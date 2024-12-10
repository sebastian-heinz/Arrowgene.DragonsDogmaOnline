CREATE TABLE ddon_epitaph_road_unlocks (
	"character_id"	INTEGER NOT NULL,
	"epitaph_id"	INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_epitaph_road_unlocks" PRIMARY KEY ("character_id", "epitaph_id"),
	CONSTRAINT "fk_ddon_epitaph_road_unlocks_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);

CREATE TABLE ddon_epitaph_claimed_weekly_rewards (
	"character_id"	INTEGER NOT NULL,
	"epitaph_id"	INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_epitaph_claimed_weekly_rewards" PRIMARY KEY ("character_id", "epitaph_id"),
	CONSTRAINT "fk_ddon_epitaph_claimed_weekly_rewards_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);
