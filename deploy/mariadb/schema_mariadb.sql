CREATE TABLE IF NOT EXISTS setting
(
    "key"   VARCHAR(32) NOT NULL,
    "value" TEXT        NOT NULL,
    PRIMARY KEY ("key")
);

CREATE TABLE IF NOT EXISTS account
(
    "id"                  INTEGER PRIMARY KEY AUTO_INCREMENT NOT NULL,
    "name"                TEXT                               NOT NULL,
    "normal_name"         TEXT                               NOT NULL,
    "hash"                TEXT                               NOT NULL,
    "mail"                TEXT                               NOT NULL,
    "mail_verified"       BOOLEAN                            NOT NULL,
    "mail_verified_at"    DATETIME DEFAULT NULL,
    "mail_token"          TEXT     DEFAULT NULL,
    "password_token"      TEXT     DEFAULT NULL,
    "login_token"         TEXT     DEFAULT NULL,
    "login_token_created" DATETIME DEFAULT NULL,
    "state"               INTEGER                            NOT NULL,
    "last_login"          DATETIME DEFAULT NULL,
    "created"             DATETIME                           NOT NULL,
    CONSTRAINT uq_account_name UNIQUE ("name"),
    CONSTRAINT uq_account_normal_name UNIQUE ("normal_name"),
    CONSTRAINT uq_account_login_token UNIQUE ("login_token"),
    CONSTRAINT uq_account_mail UNIQUE ("mail")
);

CREATE TABLE IF NOT EXISTS ddon_character_common
(
    "character_common_id" INTEGER PRIMARY KEY AUTO_INCREMENT NOT NULL,
    "job"                 SMALLINT                           NOT NULL,
    "hide_equip_head"     BOOLEAN                            NOT NULL,
    "hide_equip_lantern"  BOOLEAN                            NOT NULL
);

CREATE TABLE IF NOT EXISTS ddon_character
(
    "character_id"               INTEGER PRIMARY KEY AUTO_INCREMENT NOT NULL,
    "character_common_id"        INTEGER                            NOT NULL,
    "account_id"                 INTEGER                            NOT NULL,
    "version"                    INTEGER                            NOT NULL,
    "first_name"                 TEXT                               NOT NULL,
    "last_name"                  TEXT                               NOT NULL,
    "created"                    DATETIME                           NOT NULL,
    "my_pawn_slot_num"           SMALLINT                           NOT NULL,
    "rental_pawn_slot_num"       SMALLINT                           NOT NULL,
    "hide_equip_head_pawn"       BOOLEAN                            NOT NULL,
    "hide_equip_lantern_pawn"    BOOLEAN                            NOT NULL,
    "arisen_profile_share_range" SMALLINT                           NOT NULL,
    CONSTRAINT fk_character_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE,
    CONSTRAINT fk_character_account_id FOREIGN KEY ("account_id") REFERENCES account ("id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_pawn
(
    "pawn_id"             INTEGER PRIMARY KEY AUTO_INCREMENT NOT NULL,
    "character_common_id" INTEGER                            NOT NULL,
    "character_id"        INTEGER                            NOT NULL,
    "name"                TEXT                               NOT NULL,
    "hm_type"             SMALLINT                           NOT NULL,
    "pawn_type"           SMALLINT                           NOT NULL,
    CONSTRAINT fk_pawn_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE,
    CONSTRAINT fk_character_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_edit_info
(
    "character_common_id"         INTEGER PRIMARY KEY NOT NULL,
    "sex"                         SMALLINT            NOT NULL,
    "voice"                       SMALLINT            NOT NULL,
    "voice_pitch"                 SMALLINT            NOT NULL,
    "personality"                 SMALLINT            NOT NULL,
    "speech_freq"                 SMALLINT            NOT NULL,
    "body_type"                   SMALLINT            NOT NULL,
    "hair"                        SMALLINT            NOT NULL,
    "beard"                       SMALLINT            NOT NULL,
    "makeup"                      SMALLINT            NOT NULL,
    "scar"                        SMALLINT            NOT NULL,
    "eye_preset_no"               SMALLINT            NOT NULL,
    "nose_preset_no"              SMALLINT            NOT NULL,
    "mouth_preset_no"             SMALLINT            NOT NULL,
    "eyebrow_tex_no"              SMALLINT            NOT NULL,
    "color_skin"                  SMALLINT            NOT NULL,
    "color_hair"                  SMALLINT            NOT NULL,
    "color_beard"                 SMALLINT            NOT NULL,
    "color_eyebrow"               SMALLINT            NOT NULL,
    "color_r_eye"                 SMALLINT            NOT NULL,
    "color_l_eye"                 SMALLINT            NOT NULL,
    "color_makeup"                SMALLINT            NOT NULL,
    "sokutobu"                    SMALLINT            NOT NULL,
    "hitai"                       SMALLINT            NOT NULL,
    "mimi_jyouge"                 SMALLINT            NOT NULL,
    "kannkaku"                    SMALLINT            NOT NULL,
    "mabisasi_jyouge"             SMALLINT            NOT NULL,
    "hanakuchi_jyouge"            SMALLINT            NOT NULL,
    "ago_saki_haba"               SMALLINT            NOT NULL,
    "ago_zengo"                   SMALLINT            NOT NULL,
    "ago_saki_jyouge"             SMALLINT            NOT NULL,
    "hitomi_ookisa"               SMALLINT            NOT NULL,
    "me_ookisa"                   SMALLINT            NOT NULL,
    "me_kaiten"                   SMALLINT            NOT NULL,
    "mayu_kaiten"                 SMALLINT            NOT NULL,
    "mimi_ookisa"                 SMALLINT            NOT NULL,
    "mimi_muki"                   SMALLINT            NOT NULL,
    "elf_mimi"                    SMALLINT            NOT NULL,
    "miken_takasa"                SMALLINT            NOT NULL,
    "miken_haba"                  SMALLINT            NOT NULL,
    "hohobone_ryou"               SMALLINT            NOT NULL,
    "hohobone_jyouge"             SMALLINT            NOT NULL,
    "hohoniku"                    SMALLINT            NOT NULL,
    "erahone_jyouge"              SMALLINT            NOT NULL,
    "erahone_haba"                SMALLINT            NOT NULL,
    "hana_jyouge"                 SMALLINT            NOT NULL,
    "hana_haba"                   SMALLINT            NOT NULL,
    "hana_takasa"                 SMALLINT            NOT NULL,
    "hana_kakudo"                 SMALLINT            NOT NULL,
    "kuchi_haba"                  SMALLINT            NOT NULL,
    "kuchi_atsusa"                SMALLINT            NOT NULL,
    "eyebrow_uv_offset_x"         SMALLINT            NOT NULL,
    "eyebrow_uv_offset_y"         SMALLINT            NOT NULL,
    "wrinkle"                     SMALLINT            NOT NULL,
    "wrinkle_albedo_blend_rate"   SMALLINT            NOT NULL,
    "wrinkle_detail_normal_power" SMALLINT            NOT NULL,
    "muscle_albedo_blend_rate"    SMALLINT            NOT NULL,
    "muscle_detail_normal_power"  SMALLINT            NOT NULL,
    "height"                      SMALLINT            NOT NULL,
    "head_size"                   SMALLINT            NOT NULL,
    "neck_offset"                 SMALLINT            NOT NULL,
    "neck_scale"                  SMALLINT            NOT NULL,
    "upper_body_scale_x"          SMALLINT            NOT NULL,
    "belly_size"                  SMALLINT            NOT NULL,
    "teat_scale"                  SMALLINT            NOT NULL,
    "tekubi_size"                 SMALLINT            NOT NULL,
    "koshi_offset"                SMALLINT            NOT NULL,
    "koshi_size"                  SMALLINT            NOT NULL,
    "ankle_offset"                SMALLINT            NOT NULL,
    "fat"                         SMALLINT            NOT NULL,
    "muscle"                      SMALLINT            NOT NULL,
    "motion_filter"               SMALLINT            NOT NULL,
    CONSTRAINT fk_edit_info_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_status_info
(
    "character_common_id" INTEGER PRIMARY KEY NOT NULL,
    "hp"                  INTEGER             NOT NULL,
    "stamina"             INTEGER             NOT NULL,
    "revive_point"        SMALLINT            NOT NULL,
    "max_hp"              INTEGER             NOT NULL,
    "max_stamina"         INTEGER             NOT NULL,
    "white_hp"            INTEGER             NOT NULL,
    "gain_hp"             INTEGER             NOT NULL,
    "gain_stamina"        INTEGER             NOT NULL,
    "gain_attack"         INTEGER             NOT NULL,
    "gain_defense"        INTEGER             NOT NULL,
    "gain_magic_attack"   INTEGER             NOT NULL,
    "gain_magic_defense"  INTEGER             NOT NULL,
    CONSTRAINT fk_status_info_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_character_matching_profile
(
    "character_id"      INTEGER PRIMARY KEY NOT NULL,
    "entry_job"         SMALLINT            NOT NULL,
    "entry_job_level"   INTEGER             NOT NULL,
    "current_job"       SMALLINT            NOT NULL,
    "current_job_level" INTEGER             NOT NULL,
    objective_type1     INTEGER             NOT NULL,
    objective_type2     INTEGER             NOT NULL,
    "play_style"        INTEGER             NOT NULL,
    "comment"           TEXT                NOT NULL,
    "is_join_party"     BOOLEAN             NOT NULL,
    CONSTRAINT fk_matching_profile_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_character_arisen_profile
(
    "character_id"    INTEGER PRIMARY KEY NOT NULL,
    "background_id"   SMALLINT            NOT NULL,
    "title_uid"       INTEGER             NOT NULL,
    "title_index"     INTEGER             NOT NULL,
    "motion_id"       SMALLINT            NOT NULL,
    "motion_frame_no" INTEGER             NOT NULL,
    CONSTRAINT fk_arisen_profile_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_character_job_data
(
    "character_common_id"   INTEGER  NOT NULL,
    "job"                   SMALLINT NOT NULL,
    "exp"                   INTEGER  NOT NULL,
    "job_point"             INTEGER  NOT NULL,
    "lv"                    INTEGER  NOT NULL,
    "atk"                   SMALLINT NOT NULL,
    "def"                   SMALLINT NOT NULL,
    "m_atk"                 SMALLINT NOT NULL,
    "m_def"                 SMALLINT NOT NULL,
    "strength"              SMALLINT NOT NULL,
    "down_power"            SMALLINT NOT NULL,
    "shake_power"           SMALLINT NOT NULL,
    "stun_power"            SMALLINT NOT NULL,
    "consitution"           SMALLINT NOT NULL,
    "guts"                  SMALLINT NOT NULL,
    "fire_resist"           SMALLINT NOT NULL,
    "ice_resist"            SMALLINT NOT NULL,
    "thunder_resist"        SMALLINT NOT NULL,
    "holy_resist"           SMALLINT NOT NULL,
    "dark_resist"           SMALLINT NOT NULL,
    "spread_resist"         SMALLINT NOT NULL,
    "freeze_resist"         SMALLINT NOT NULL,
    "shock_resist"          SMALLINT NOT NULL,
    "absorb_resist"         SMALLINT NOT NULL,
    "dark_elm_resist"       SMALLINT NOT NULL,
    "poison_resist"         SMALLINT NOT NULL,
    "slow_resist"           SMALLINT NOT NULL,
    "sleep_resist"          SMALLINT NOT NULL,
    "stun_resist"           SMALLINT NOT NULL,
    "wet_resist"            SMALLINT NOT NULL,
    "oil_resist"            SMALLINT NOT NULL,
    "seal_resist"           SMALLINT NOT NULL,
    "curse_resist"          SMALLINT NOT NULL,
    "soft_resist"           SMALLINT NOT NULL,
    "stone_resist"          SMALLINT NOT NULL,
    "gold_resist"           SMALLINT NOT NULL,
    "fire_reduce_resist"    SMALLINT NOT NULL,
    "ice_reduce_resist"     SMALLINT NOT NULL,
    "thunder_reduce_resist" SMALLINT NOT NULL,
    "holy_reduce_resist"    SMALLINT NOT NULL,
    "dark_reduce_resist"    SMALLINT NOT NULL,
    "atk_down_resist"       SMALLINT NOT NULL,
    "def_down_resist"       SMALLINT NOT NULL,
    "m_atk_down_resist"     SMALLINT NOT NULL,
    "m_def_down_resist"     SMALLINT NOT NULL,
    CONSTRAINT pk_character_job_data PRIMARY KEY (character_common_id, job),
    CONSTRAINT fk_character_job_data_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_storage
(
    "character_id" INTEGER  NOT NULL,
    "storage_type" SMALLINT NOT NULL,
    "slot_max"     SMALLINT NOT NULL,
    "item_sort"    BLOB     NOT NULL,
    CONSTRAINT pk_ddon_storage PRIMARY KEY (character_id, storage_type),
    CONSTRAINT fk_storage_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_wallet_point
(
    "character_id" INTEGER  NOT NULL,
    "type"         SMALLINT NOT NULL,
    "value"        INTEGER  NOT NULL,
    CONSTRAINT pk_ddon_wallet_point PRIMARY KEY (character_id, type),
    CONSTRAINT fk_wallet_point_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_item
(
    -- See Item.cs, uid is at most of size 8.
    "uid"        VARCHAR(8) NOT NULL,
    "item_id"    INTEGER     NOT NULL,
    unk3         SMALLINT    NOT NULL,
    "color"      SMALLINT    NOT NULL,
    "plus_value" SMALLINT    NOT NULL,
    "equip_points" INTEGER   NOT NULL,
    PRIMARY KEY ("uid")
);

CREATE TABLE IF NOT EXISTS ddon_storage_item
(
    "item_uid"     VARCHAR(8) NOT NULL,
    "character_id" INTEGER     NOT NULL,
    "storage_type" SMALLINT    NOT NULL,
    "slot_no"      SMALLINT    NOT NULL,
    "item_num"     INTEGER     NOT NULL,
    CONSTRAINT pk_ddon_storage_item PRIMARY KEY (character_id, storage_type, slot_no),
    CONSTRAINT fk_storage_item_item_uid FOREIGN KEY ("item_uid") REFERENCES ddon_item ("uid") ON DELETE CASCADE,
    CONSTRAINT fk_storage_item_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_equip_item
(
    "item_uid"            VARCHAR(8) NOT NULL,
    "character_common_id" INTEGER     NOT NULL,
    "job"                 SMALLINT    NOT NULL,
    "equip_type"          SMALLINT    NOT NULL,
    "equip_slot"          SMALLINT    NOT NULL,
    CONSTRAINT pk_ddon_equip_item PRIMARY KEY (character_common_id, job, equip_type, equip_slot),
    CONSTRAINT fk_equip_item_item_uid FOREIGN KEY ("item_uid") REFERENCES ddon_item ("uid") ON DELETE CASCADE,
    CONSTRAINT fk_equip_item_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_equip_job_item
(
    "item_uid"            VARCHAR(8) NOT NULL,
    "character_common_id" INTEGER     NOT NULL,
    "job"                 SMALLINT    NOT NULL,
    "equip_slot"          SMALLINT    NOT NULL,
    CONSTRAINT pk_ddon_equip_job_item PRIMARY KEY (character_common_id, job, equip_slot),
    CONSTRAINT fk_equip_job_item_item_uid FOREIGN KEY ("item_uid") REFERENCES ddon_item ("uid") ON DELETE CASCADE,
    CONSTRAINT fk_equip_job_item_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_normal_skill_param
(
    "character_common_id" INTEGER  NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "skill_no"            INTEGER  NOT NULL,
    "index"               INTEGER  NOT NULL,
    "pre_skill_no"        INTEGER  NOT NULL,
    CONSTRAINT pk_ddon_normal_skill_param PRIMARY KEY (character_common_id, job, skill_no),
    CONSTRAINT fk_normal_skill_param_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_learned_custom_skill
(
    "character_common_id" INTEGER  NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "skill_id"            INTEGER  NOT NULL,
    "skill_lv"            SMALLINT NOT NULL,
    PRIMARY KEY (character_common_id, job, skill_id),
    CONSTRAINT fk_learned_custom_skill_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_equipped_custom_skill
(
    "character_common_id" INTEGER  NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "slot_no"             SMALLINT NOT NULL,
    "skill_id"            INTEGER  NOT NULL,
    CONSTRAINT pk_ddon_equipped_custom_skill PRIMARY KEY (character_common_id, job, slot_no),
    CONSTRAINT fk_equipped_custom_skill_character_common_id FOREIGN KEY (character_common_id, job, skill_id) REFERENCES ddon_learned_custom_skill (character_common_id, job, skill_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_learned_ability
(
    "character_common_id" INTEGER  NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "ability_id"          INTEGER  NOT NULL,
    "ability_lv"          SMALLINT NOT NULL,
    PRIMARY KEY (character_common_id, job, ability_id),
    CONSTRAINT fk_learned_ability_character_common_id FOREIGN KEY ("character_common_id") REFERENCES ddon_character_common ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_equipped_ability
(
    "character_common_id" INTEGER  NOT NULL,
    "equipped_to_job"     SMALLINT NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "slot_no"             SMALLINT NOT NULL,
    "ability_id"          INTEGER  NOT NULL,
    CONSTRAINT pk_ddon_equipped_ability PRIMARY KEY (character_common_id, equipped_to_job, slot_no),
    CONSTRAINT fk_equipped_ability_character_common_id FOREIGN KEY (character_common_id, job, ability_id) REFERENCES ddon_learned_ability (character_common_id, job, ability_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_shortcut
(
    "character_id" INTEGER  NOT NULL,
    "page_no"      INTEGER  NOT NULL,
    "button_no"    INTEGER  NOT NULL,
    "shortcut_id"  INTEGER  NOT NULL,
    u32_data       INTEGER  NOT NULL,
    f32_data       INTEGER  NOT NULL,
    "exex_type"    SMALLINT NOT NULL,
    CONSTRAINT pk_ddon_shortcut PRIMARY KEY (character_id, page_no, button_no),
    CONSTRAINT fk_shortcut_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_communication_shortcut
(
    "character_id" INTEGER  NOT NULL,
    "page_no"      INTEGER  NOT NULL,
    "button_no"    INTEGER  NOT NULL,
    "type"         SMALLINT NOT NULL,
    "category"     SMALLINT NOT NULL,
    "id"           INTEGER  NOT NULL,
    CONSTRAINT pk_ddon_communication_shortcut PRIMARY KEY (character_id, page_no, button_no),
    CONSTRAINT fk_communication_shortcut_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_pawn_reaction
(
    "pawn_id"       INTEGER  NOT NULL,
    "reaction_type" SMALLINT NOT NULL,
    "motion_no"     INTEGER  NOT NULL,
    CONSTRAINT pk_ddon_pawn_reaction PRIMARY KEY (pawn_id, reaction_type),
    CONSTRAINT fk_pawn_reaction_pawn_id FOREIGN KEY ("pawn_id") REFERENCES ddon_pawn ("pawn_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_sp_skill
(
    "pawn_id"     INTEGER  NOT NULL,
    "sp_skill_id" SMALLINT NOT NULL,
    "sp_skill_lv" SMALLINT NOT NULL,
    CONSTRAINT pk_ddon_sp_skill PRIMARY KEY ("pawn_id"),
    CONSTRAINT fk_sp_skill_pawn_id FOREIGN KEY ("pawn_id") REFERENCES ddon_pawn ("pawn_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_game_token
(
    "account_id"   INTEGER PRIMARY KEY NOT NULL,
    "character_id" INTEGER             NOT NULL,
    "token"        TEXT                NOT NULL,
    "created"      DATETIME            NOT NULL,
    CONSTRAINT uq_game_token_token UNIQUE ("token"),
    CONSTRAINT fk_game_token_account_id FOREIGN KEY ("account_id") REFERENCES account ("id"),
    CONSTRAINT fk_game_token_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id")
);

CREATE TABLE IF NOT EXISTS ddon_connection
(
    "server_id"  INTEGER  NOT NULL,
    "account_id" INTEGER  NOT NULL,
    "type"       INTEGER  NOT NULL,
    "created"    DATETIME NOT NULL,
    CONSTRAINT uq_connection_server_id_account_id UNIQUE (server_id, account_id),
    CONSTRAINT fk_connection_token_account_id FOREIGN KEY ("account_id") REFERENCES account ("id")
);
