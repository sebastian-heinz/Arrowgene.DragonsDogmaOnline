CREATE TABLE ddon_pawn_favorites (
	"character_id"	INTEGER NOT NULL,
	"pawn_id"	    INTEGER NOT NULL,
    CONSTRAINT pk_ddon_pawn_favorites PRIMARY KEY ("character_id", "pawn_id"),
    CONSTRAINT fk_ddon_pawn_favorites_pawn_id FOREIGN KEY ("pawn_id") REFERENCES "ddon_pawn" ("pawn_id") ON DELETE CASCADE,
    CONSTRAINT fk_ddon_pawn_favorites_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
