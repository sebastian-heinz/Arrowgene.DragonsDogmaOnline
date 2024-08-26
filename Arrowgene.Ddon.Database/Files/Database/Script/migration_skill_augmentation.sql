CREATE TABLE ddon_skill_augmentation_released_elements (
	"character_id"	INTEGER NOT NULL,
	"job_id"	    INTEGER NOT NULL,
	"release_id"	INTEGER NOT NULL,
    CONSTRAINT pk_ddon_skill_augmentation_released_elements PRIMARY KEY ("character_id", "job_id", "release_id"),
    CONSTRAINT fk_ddon_skill_augmentation_released_elements_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
