CREATE TABLE "ddon_character_playpoint_data" (
	"character_id"	        INTEGER NOT NULL,
	"job"					SMALLINT NOT NULL,
	"play_point"			INTEGER NOT NULL DEFAULT 0,
	"exp_mode"				TINYINT NOT NULL DEFAULT 1,
	CONSTRAINT pk_character_playpoint PRIMARY KEY (character_id, job),
	CONSTRAINT fk_character_playpoint_character_id FOREIGN KEY("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);

INSERT INTO "ddon_character_playpoint_data" SELECT c.character_id, j.job, 0, 1 FROM "ddon_character" c INNER JOIN "ddon_character_job_data" j ON c.character_common_id = j.character_common_id;