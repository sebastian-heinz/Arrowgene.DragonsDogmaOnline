CREATE TABLE IF NOT EXISTS "account_ip_ban"
(
    "addr"                TEXT      PRIMARY KEY NOT NULL,
    "date"                DATETIME              NOT NULL DEFAULT (CURRENT_TIMESTAMP)
);
CREATE INDEX IF NOT EXISTS "idx_account_ip_ban_addr" ON "account_ip_ban" ("addr");

CREATE TABLE IF NOT EXISTS "ddon_mail"
(
    "message_id"    INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "character_id"  INTEGER                           NOT NULL,
    "message_state" INTEGER                           NOT NULL,
    "sender_id"     INTEGER                           NOT NULL,
    "message_title" VARCHAR(256)                      NOT NULL DEFAULT '',
    "message_body"  VARCHAR(2048)                     NOT NULL DEFAULT '',
    "send_date"     INTEGER                           NOT NULL DEFAULT 0,
    CONSTRAINT "fk_ddon_mail_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS "idx_ddon_mail_character_id" ON "ddon_mail" ("character_id");

CREATE TABLE "ddon_group_chat_groups"
(
    "group_id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "group_name" STRING NOT NULL,
    "group_desc" STRING NOT NULL,
    "prevent_deletion" BOOLEAN NOT NULL
);

CREATE TABLE "ddon_group_chat"
(
    "character_id" INTEGER NOT NULL,
    "group_id"     BIGINT NOT NULL,
    CONSTRAINT "pk_ddon_group_chat" PRIMARY KEY ("character_id")
    CONSTRAINT "fk_ddon_group_chat_character_id" FOREIGN KEY ("character_id") references "ddon_character" ("character_id") ON DELETE CASCADE
    CONSTRAINT "fk_ddon_group_chat_group_id" FOREIGN KEY ("group_id") references "ddon_group_chat_groups" ("group_id") ON DELETE CASCADE
);

INSERT INTO "ddon_schedule_next"(type, timestamp)
VALUES (26, 0);

CREATE TABLE "ddon_black_list"
(
    "character_id" INTEGER NOT NULL,
    "target_id" INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_black_list" PRIMARY KEY ("character_id", "target_id")
    CONSTRAINT "fk_ddon_black_list_character_id" FOREIGN KEY ("character_id") references "ddon_character" ("character_id") ON DELETE CASCADE
    CONSTRAINT "fk_ddon_black_list_target_id" FOREIGN KEY ("target_id") references "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_communication_message_set"
(
    "character_id"  INTEGER NOT NULL,
    "set_no"        INTEGER NOT NULL,
    "set_name"      TEXT    NOT NULL,
    CONSTRAINT "pk_ddon_communication_message_set" PRIMARY KEY ("character_id", "set_no")
    CONSTRAINT "fk_ddon_communication_message_set_character_id" FOREIGN KEY ("character_id") references "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE "ddon_communication_message"
(
    "character_id"  INTEGER NOT NULL,
    "set_no"        INTEGER NOT NULL,
    "message_no"    INTEGER NOT NULL,
    "message"       TEXT NOT NULL,
    "emotion"       INTEGER NOT NULL,
    "emotochat"     BOOLEAN NOT NULL,
    CONSTRAINT "pk_ddon_communication_message" PRIMARY KEY ("character_id", "set_no", "message_no")
    CONSTRAINT "fk_ddon_communication_message_character_id_set_no" FOREIGN KEY ("character_id", "set_no") references "ddon_communication_message_set" ("character_id", "set_no") ON DELETE CASCADE
);

ALTER TABLE ddon_stamp_bonus DROP COLUMN last_stamp_time;
ALTER TABLE ddon_stamp_bonus ADD COLUMN can_stamp BOOLEAN NOT NULL DEFAULT TRUE;
INSERT INTO "ddon_schedule_next"(type, timestamp) VALUES (2, 0);
