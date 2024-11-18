CREATE TABLE "ddon_clan_param"
(
    "clan_id"               INTEGER PRIMARY KEY AUTOINCREMENT,
    "clan_level"            INTEGER             NOT NULL,
    "member_num"            INTEGER             NOT NULL,
    "master_id"             INTEGER             NOT NULL,
    "system_restriction"    BOOLEAN             NOT NULL,
    "is_base_release"       BOOLEAN             NOT NULL,
    "can_base_release"      BOOLEAN             NOT NULL,
    "total_clan_point"      INTEGER             NOT NULL,
    "money_clan_point"      INTEGER             NOT NULL,
    "name"                  TEXT                NOT NULL,
    "short_name"            TEXT                NOT NULL,
    "emblem_mark_type"      SMALLINT            NOT NULL,
    "emblem_base_type"      SMALLINT            NOT NULL,
    "emblem_main_color"     SMALLINT            NOT NULL,
    "emblem_sub_color"      SMALLINT            NOT NULL,
    "motto"                 INTEGER             NOT NULL,
    "active_days"           INTEGER             NOT NULL,
    "active_time"           INTEGER             NOT NULL,
    "characteristic"        INTEGER             NOT NULL,
    "is_publish"            BOOLEAN             NOT NULL,
    "comment"               TEXT                NOT NULL,
    "board_message"         TEXT                NOT NULL,
    "created"               DATETIME            NOT NULL
);

CREATE TABLE "ddon_clan_membership"
(
    "character_id"          INTEGER     NOT NULL,
    "clan_id"               INTEGER     NOT NULL,
    "rank"                  INTEGER     NOT NULL,
    "permission"            INTEGER     NOT NULL,
    "created"               DATETIME    NOT NULL,
    CONSTRAINT "pk_ddon_clan_membership" PRIMARY KEY ("character_id", "clan_id"),
    CONSTRAINT "fk_ddon_clan_membership_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_clan_membership_clan_id" FOREIGN KEY ("clan_id") REFERENCES "ddon_clan_param" ("clan_id") ON DELETE CASCADE
);
