CREATE TABLE "ddon_game_token_new"
(
    "account_id"   INTEGER PRIMARY KEY NOT NULL,
    "character_id" INTEGER             NOT NULL,
    "token"        TEXT                NOT NULL,
    "created"      DATETIME            NOT NULL,
    CONSTRAINT "uq_ddon_game_token_token" UNIQUE ("token"),
    CONSTRAINT "fk_ddon_game_token_account_id" FOREIGN KEY ("account_id") REFERENCES "account" ("id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_game_token_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id")
);
INSERT INTO "ddon_game_token_new" SELECT * FROM "ddon_game_token";
DROP TABLE "ddon_game_token";
ALTER TABLE "ddon_game_token_new" RENAME TO "ddon_game_token";


CREATE TABLE "ddon_contact_list_new"
(
    "id"                     INTEGER PRIMARY KEY AUTOINCREMENT,
    "requester_character_id" INTEGER  NOT NULL,
    "requested_character_id" INTEGER  NOT NULL,
    "status"                 SMALLINT NOT NULL,
    "type"                   SMALLINT NOT NULL,
    "requester_favorite"     BOOLEAN  NOT NULL,
    "requested_favorite"     BOOLEAN  NOT NULL,
    CONSTRAINT "fk_ddon_contact_list_requester_character_id" FOREIGN KEY ("requester_character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_contact_list_requested_character_id" FOREIGN KEY ("requested_character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE,
    CONSTRAINT "uq_ddon_contact_list_requester_character_id_requested_character_id" UNIQUE ("requester_character_id", "requested_character_id")
);
INSERT INTO "ddon_contact_list_new" SELECT * FROM "ddon_contact_list";
DROP TABLE "ddon_contact_list";
ALTER TABLE "ddon_contact_list_new" RENAME TO "ddon_contact_list";


CREATE TABLE "ddon_bazaar_exhibition_new"
(
    "bazaar_id"       INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "character_id"    INTEGER                           NOT NULL,
    "sequence"        INTEGER                           NOT NULL,
    "item_id"         INTEGER                           NOT NULL,
    "num"             INTEGER                           NOT NULL,
    "price"           INTEGER                           NOT NULL,
    "exhibition_time" DATETIME                          NOT NULL,
    "state"           SMALLINT                          NOT NULL,
    "proceeds"        INTEGER                           NOT NULL,
    "expire"          DATETIME                          NOT NULL,
    CONSTRAINT "fk_ddon_bazaar_exhibition_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id")
);
INSERT INTO "ddon_bazaar_exhibition_new" SELECT * FROM "ddon_bazaar_exhibition";
DROP TABLE "ddon_bazaar_exhibition";
ALTER TABLE "ddon_bazaar_exhibition_new" RENAME TO "ddon_bazaar_exhibition";

CREATE TABLE IF NOT EXISTS "ddon_pawn_craft_progress_new"
(
    "craft_character_id"     INTEGER NOT NULL,
    "craft_lead_pawn_id"     INTEGER NOT NULL,
    "craft_support_pawn_id1" INTEGER NOT NULL,
    "craft_support_pawn_id2" INTEGER NOT NULL,
    "craft_support_pawn_id3" INTEGER NOT NULL,

    "recipe_id"              INTEGER NOT NULL,
    "exp"                    INTEGER NOT NULL,
    "npc_action_id"          INTEGER NOT NULL,
    "item_id"                INTEGER NOT NULL,
    "unk0"                   INTEGER NOT NULL,
    "remain_time"            INTEGER NOT NULL,
    "exp_bonus"              BOOLEAN NOT NULL,
    "create_count"           INTEGER NOT NULL,

    "plus_value"             INTEGER NOT NULL,
    "great_success"          BOOLEAN NOT NULL,
    "bonus_exp"              INTEGER NOT NULL,
    "additional_quantity"    INTEGER NOT NULL,
    CONSTRAINT "fk_ddon_pawn_craft_progress_craft_character_id" FOREIGN KEY ("craft_character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_pawn_craft_progress_craft_lead_pawn_id" FOREIGN KEY ("craft_lead_pawn_id") REFERENCES "ddon_pawn" ("pawn_id") ON DELETE CASCADE
);
INSERT INTO "ddon_pawn_craft_progress_new" SELECT * FROM "ddon_pawn_craft_progress";
DROP TABLE "ddon_pawn_craft_progress";
ALTER TABLE "ddon_pawn_craft_progress_new" RENAME TO "ddon_pawn_craft_progress";
