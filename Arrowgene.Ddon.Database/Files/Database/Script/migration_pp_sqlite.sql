CREATE TABLE "ddon_character_playpoint_data" (
	"character_common_id"	INTEGER NOT NULL,
	"job"					SMALLINT NOT NULL,
	"play_point"			INTEGER NOT NULL DEFAULT 0,
	"exp_mode"				TINYINT NOT NULL DEFAULT 1,
	CONSTRAINT pk_character_playpoint PRIMARY KEY (character_common_id, job),
	CONSTRAINT fk_character_playpoint_character_common_id FOREIGN KEY("character_common_id") REFERENCES "ddon_character_common"("character_common_id") ON DELETE CASCADE
);

INSERT INTO "ddon_character_playpoint_data" SELECT "character_common_id", "job", 0, 1 FROM "ddon_character_job_data" jobdata WHERE jobdata."character_common_id" IN (SELECT chardata."character_common_id" from "ddon_character" chardata);