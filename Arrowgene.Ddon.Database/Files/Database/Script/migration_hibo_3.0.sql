CREATE TABLE ddon_skill_augmentation_released_elements_copy (
    "character_id"	INTEGER NOT NULL,
    "orb_tree_type" INTEGER NOT NULL,
    "job_id"	    INTEGER NOT NULL,
    "release_id"	INTEGER NOT NULL,
    CONSTRAINT pk_ddon_skill_augmentation_released_elements PRIMARY KEY ("character_id", "orb_tree_type", "job_id", "release_id"),
    CONSTRAINT fk_ddon_skill_augmentation_released_elements_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

INSERT INTO ddon_skill_augmentation_released_elements_copy (character_id, orb_tree_type, job_id, release_id)
SELECT character_id, 1, job_id, release_id
FROM ddon_skill_augmentation_released_elements;

DROP TABLE ddon_skill_augmentation_released_elements;

ALTER TABLE ddon_skill_augmentation_released_elements_copy RENAME TO ddon_skill_augmentation_released_elements;
