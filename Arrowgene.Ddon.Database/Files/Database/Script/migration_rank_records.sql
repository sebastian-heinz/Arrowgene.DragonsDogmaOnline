CREATE TABLE "ddon_rank_record"
(
    "record_id"           INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "character_id"        INTEGER                           NOT NULL,
    "quest_id"            INTEGER                           NOT NULL,
    "score"               INTEGER                           NOT NULL,
    "date"                DATETIME                          NOT NULL,
    CONSTRAINT "fk_ddon_rank_record_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);
INSERT INTO ddon_schedule_next(type, timestamp) VALUES (23, 0);
