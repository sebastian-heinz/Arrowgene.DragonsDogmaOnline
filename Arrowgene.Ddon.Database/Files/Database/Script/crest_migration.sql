CREATE TABLE ddon_crests (
	"character_common_id"	INTEGER NOT NULL,
	"item_uid"	VARCHAR(8) NOT NULL,
	"slot"	INTEGER NOT NULL,
	"crest_id"	INTEGER NOT NULL,
	"crest_amount"	INTEGER NOT NULL,
	FOREIGN KEY("item_uid") REFERENCES "ddon_storage_item"("item_uid") ON DELETE CASCADE,
	FOREIGN KEY("character_common_id") REFERENCES "ddon_character_common"("character_common_id") ON DELETE CASCADE
);