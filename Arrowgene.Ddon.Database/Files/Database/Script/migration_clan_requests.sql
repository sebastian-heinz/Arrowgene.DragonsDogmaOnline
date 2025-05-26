CREATE TABLE "ddon_clan_requests"
(
    "requester_character_id"    INTEGER PRIMARY KEY NOT NULL,
    "clan_id"                   INTEGER  NOT NULL,
    "created"                   DATETIME NOT NULL,
    "approved"                  SMALLINT NOT NULL,
    CONSTRAINT "fk_ddon_clan_requests_clan_id" FOREIGN KEY ("clan_id") REFERENCES "ddon_clan_param" ("clan_id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_clan_requests_requester_character_id" FOREIGN KEY ("requester_character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

INSERT INTO "ddon_schedule_next" ("type", "timestamp")
VALUES (25, 0);
