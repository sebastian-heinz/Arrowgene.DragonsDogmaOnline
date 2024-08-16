CREATE TABLE "ddon_binary_data"
(
    "character_id"      INTEGER  NOT NULL,
    "binary_data"       BLOB     NOT NULL,
    CONSTRAINT pk_binary_data PRIMARY KEY (character_id),
    CONSTRAINT fk_binary_character_id FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);