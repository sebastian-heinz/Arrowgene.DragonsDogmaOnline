PRAGMA foreign_keys=OFF;

CREATE TABLE "ddon_edit_info_new"
(
    "character_common_id"         INTEGER PRIMARY KEY NOT NULL,
    "sex"                         INTEGER            NOT NULL,
    "voice"                       INTEGER            NOT NULL,
    "voice_pitch"                 INTEGER            NOT NULL,
    "personality"                 INTEGER            NOT NULL,
    "speech_freq"                 INTEGER            NOT NULL,
    "body_type"                   INTEGER            NOT NULL,
    "hair"                        INTEGER            NOT NULL,
    "beard"                       INTEGER            NOT NULL,
    "makeup"                      INTEGER            NOT NULL,
    "scar"                        INTEGER            NOT NULL,
    "eye_preset_no"               INTEGER            NOT NULL,
    "nose_preset_no"              INTEGER            NOT NULL,
    "mouth_preset_no"             INTEGER            NOT NULL,
    "eyebrow_tex_no"              INTEGER            NOT NULL,
    "color_skin"                  INTEGER            NOT NULL,
    "color_hair"                  INTEGER            NOT NULL,
    "color_beard"                 INTEGER            NOT NULL,
    "color_eyebrow"               INTEGER            NOT NULL,
    "color_r_eye"                 INTEGER            NOT NULL,
    "color_l_eye"                 INTEGER            NOT NULL,
    "color_makeup"                INTEGER            NOT NULL,
    "sokutobu"                    INTEGER            NOT NULL,
    "hitai"                       INTEGER            NOT NULL,
    "mimi_jyouge"                 INTEGER            NOT NULL,
    "kannkaku"                    INTEGER            NOT NULL,
    "mabisasi_jyouge"             INTEGER            NOT NULL,
    "hanakuchi_jyouge"            INTEGER            NOT NULL,
    "ago_saki_haba"               INTEGER            NOT NULL,
    "ago_zengo"                   INTEGER            NOT NULL,
    "ago_saki_jyouge"             INTEGER            NOT NULL,
    "hitomi_ookisa"               INTEGER            NOT NULL,
    "me_ookisa"                   INTEGER            NOT NULL,
    "me_kaiten"                   INTEGER            NOT NULL,
    "mayu_kaiten"                 INTEGER            NOT NULL,
    "mimi_ookisa"                 INTEGER            NOT NULL,
    "mimi_muki"                   INTEGER            NOT NULL,
    "elf_mimi"                    INTEGER            NOT NULL,
    "miken_takasa"                INTEGER            NOT NULL,
    "miken_haba"                  INTEGER            NOT NULL,
    "hohobone_ryou"               INTEGER            NOT NULL,
    "hohobone_jyouge"             INTEGER            NOT NULL,
    "hohoniku"                    INTEGER            NOT NULL,
    "erahone_jyouge"              INTEGER            NOT NULL,
    "erahone_haba"                INTEGER            NOT NULL,
    "hana_jyouge"                 INTEGER            NOT NULL,
    "hana_haba"                   INTEGER            NOT NULL,
    "hana_takasa"                 INTEGER            NOT NULL,
    "hana_kakudo"                 INTEGER            NOT NULL,
    "kuchi_haba"                  INTEGER            NOT NULL,
    "kuchi_atsusa"                INTEGER            NOT NULL,
    "eyebrow_uv_offset_x"         INTEGER            NOT NULL,
    "eyebrow_uv_offset_y"         INTEGER            NOT NULL,
    "wrinkle"                     INTEGER            NOT NULL,
    "wrinkle_albedo_blend_rate"   INTEGER            NOT NULL,
    "wrinkle_detail_normal_power" INTEGER            NOT NULL,
    "muscle_albedo_blend_rate"    INTEGER            NOT NULL,
    "muscle_detail_normal_power"  INTEGER            NOT NULL,
    "height"                      INTEGER            NOT NULL,
    "head_size"                   INTEGER            NOT NULL,
    "neck_offset"                 INTEGER            NOT NULL,
    "neck_scale"                  INTEGER            NOT NULL,
    "upper_body_scale_x"          INTEGER            NOT NULL,
    "belly_size"                  INTEGER            NOT NULL,
    "teat_scale"                  INTEGER            NOT NULL,
    "tekubi_size"                 INTEGER            NOT NULL,
    "koshi_offset"                INTEGER            NOT NULL,
    "koshi_size"                  INTEGER            NOT NULL,
    "ankle_offset"                INTEGER            NOT NULL,
    "fat"                         INTEGER            NOT NULL,
    "muscle"                      INTEGER            NOT NULL,
    "motion_filter"               INTEGER            NOT NULL,
    CONSTRAINT "fk_ddon_edit_info_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);
INSERT INTO "ddon_edit_info_new" SELECT * FROM "ddon_edit_info";
DROP TABLE "ddon_edit_info";
ALTER TABLE "ddon_edit_info_new" RENAME TO "ddon_edit_info";

CREATE TABLE "ddon_priority_quests_new"
(
    "character_common_id" INTEGER NOT NULL,
    "quest_schedule_id"   INTEGER NOT NULL,
    CONSTRAINT "fk_ddon_priority_quests_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE,
    CONSTRAINT "uq_character_common_id_quest_schedule_id" UNIQUE ("character_common_id", "quest_schedule_id")
);
INSERT INTO "ddon_priority_quests_new" SELECT * FROM "ddon_priority_quests";
DROP TABLE "ddon_priority_quests";
ALTER TABLE "ddon_priority_quests_new" RENAME TO "ddon_priority_quests";

CREATE INDEX IF NOT EXISTS "idx_ddon_character_account_game_mode" ON "ddon_character" ("account_id", "game_mode");
CREATE INDEX IF NOT EXISTS "idx_ddon_pawn_character_id" ON "ddon_pawn" ("character_id");
CREATE INDEX IF NOT EXISTS "idx_ddon_quest_progress_character_common_id" ON "ddon_quest_progress" ("character_common_id");
CREATE INDEX IF NOT EXISTS "idx_completed_quests_character_common_id_type" ON "ddon_completed_quests" ("character_common_id", "quest_type");
CREATE INDEX IF NOT EXISTS "idx_ddon_system_mail_character_id" ON "ddon_system_mail" ("character_id");
CREATE INDEX IF NOT EXISTS "idx_ddon_crests_character_item" ON "ddon_crests" ("character_common_id", "item_uid");
