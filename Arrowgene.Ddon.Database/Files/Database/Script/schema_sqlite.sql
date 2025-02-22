/* Style hints:
   when a primary key is made up of a single column embed the primary key syntax in the column definition
   embed AUTOINCREMENT in column definition
   do not embed unique qualifier syntax, instead it should be part of a constraint definition
   unique, foreign and primary key constraints should always be explicitly named via CONSTRAINT
   
   naming convention for primary key constraints: pk_<full table name>
   naming convention for unique constraints: uq_<full table name>_<col 1>_<col 2>_...
   naming convention for foreign keys: fk_<full table name>_<foreign col>
   
   quote all column and table references
 */
CREATE TABLE IF NOT EXISTS "meta"
(
    "db_version" INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS "setting"
(
    "key"   VARCHAR(32) PRIMARY KEY NOT NULL,
    "value" TEXT                    NOT NULL
);

CREATE TABLE IF NOT EXISTS "account"
(
    "id"                  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "name"                TEXT                              NOT NULL,
    "normal_name"         TEXT                              NOT NULL,
    "hash"                TEXT                              NOT NULL,
    "mail"                TEXT                              NOT NULL,
    "mail_verified"       BOOLEAN                           NOT NULL,
    "mail_verified_at"    DATETIME DEFAULT NULL,
    "mail_token"          TEXT     DEFAULT NULL,
    "password_token"      TEXT     DEFAULT NULL,
    "login_token"         TEXT     DEFAULT NULL,
    "login_token_created" DATETIME DEFAULT NULL,
    "state"               INTEGER                           NOT NULL,
    "last_login"          DATETIME DEFAULT NULL,
    "created"             DATETIME                          NOT NULL,
    CONSTRAINT "uq_account_name" UNIQUE ("name"),
    CONSTRAINT "uq_account_normal_name" UNIQUE ("normal_name"),
    CONSTRAINT "uq_account_login_token" UNIQUE ("login_token"),
    CONSTRAINT "uq_account_mail" UNIQUE ("mail")
);

CREATE TABLE IF NOT EXISTS "ddon_character_common"
(
    "character_common_id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "job"                 SMALLINT                          NOT NULL,
    "hide_equip_head"     BOOLEAN                           NOT NULL,
    "hide_equip_lantern"  BOOLEAN                           NOT NULL
);

CREATE TABLE IF NOT EXISTS "ddon_character"
(
    "character_id"               INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "character_common_id"        INTEGER                           NOT NULL,
    "account_id"                 INTEGER                           NOT NULL,
    "version"                    INTEGER                           NOT NULL,
    "first_name"                 TEXT                              NOT NULL,
    "last_name"                  TEXT                              NOT NULL,
    "created"                    DATETIME                          NOT NULL,
    "my_pawn_slot_num"           SMALLINT                          NOT NULL,
    "rental_pawn_slot_num"       SMALLINT                          NOT NULL,
    "hide_equip_head_pawn"       BOOLEAN                           NOT NULL,
    "hide_equip_lantern_pawn"    BOOLEAN                           NOT NULL,
    "arisen_profile_share_range" SMALLINT                          NOT NULL,
    "fav_warp_slot_num"          INTEGER                           NOT NULL,
    "max_bazaar_exhibits"        INTEGER                           NOT NULL,
    "game_mode"                  INTEGER                           NOT NULL,
    CONSTRAINT "fk_ddon_character_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_character_account_id" FOREIGN KEY ("account_id") REFERENCES "account" ("id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_pawn"
(
    "pawn_id"                     INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "character_common_id"         INTEGER                           NOT NULL,
    "character_id"                INTEGER                           NOT NULL,
    "name"                        TEXT                              NOT NULL,
    "hm_type"                     SMALLINT                          NOT NULL,
    "pawn_type"                   SMALLINT                          NOT NULL,
    "pawn_state"                  SMALLINT                          NOT NULL,
    "training_points"             INTEGER                           NOT NULL,
    "available_training"          INTEGER                           NOT NULL,
    "craft_rank"                  INTEGER                           NOT NULL,
    "craft_rank_limit"            INTEGER                           NOT NULL,
    "craft_exp"                   INTEGER                           NOT NULL,
    "craft_points"                INTEGER                           NOT NULL,
    "production_speed_level"      INTEGER                           NOT NULL,
    "equipment_enhancement_level" INTEGER                           NOT NULL,
    "equipment_quality_level"     INTEGER                           NOT NULL,
    "consumable_quantity_level"   INTEGER                           NOT NULL,
    "cost_performance_level"      INTEGER                           NOT NULL,
    "is_official_pawn"            BOOLEAN                           NOT NULL,
    CONSTRAINT "fk_ddon_pawn_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_pawn_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_pawn_training_status"
(
    "pawn_id"         INTEGER  NOT NULL,
    "job"             SMALLINT NOT NULL,
    "training_status" BLOB     NOT NULL,
    CONSTRAINT "pk_ddon_pawn_training_status" PRIMARY KEY ("pawn_id", "job"),
    CONSTRAINT "fk_ddon_pawn_training_status_pawn_id" FOREIGN KEY ("pawn_id") REFERENCES "ddon_pawn" ("pawn_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_edit_info"
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
    CONSTRAINT "fk_ddon_edit_info_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_status_info"
(
    "character_common_id" INTEGER PRIMARY KEY NOT NULL,
    "revive_point"        SMALLINT            NOT NULL,
    "hp"                  INTEGER             NOT NULL,
    "white_hp"            INTEGER             NOT NULL,
    CONSTRAINT "fk_ddon_status_info_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_character_matching_profile"
(
    "character_id"      INTEGER PRIMARY KEY NOT NULL,
    "entry_job"         SMALLINT            NOT NULL,
    "entry_job_level"   INTEGER             NOT NULL,
    "current_job"       SMALLINT            NOT NULL,
    "current_job_level" INTEGER             NOT NULL,
    "objective_type1"   INTEGER             NOT NULL,
    "objective_type2"   INTEGER             NOT NULL,
    "play_style"        INTEGER             NOT NULL,
    "comment"           TEXT                NOT NULL,
    "is_join_party"     BOOLEAN             NOT NULL,
    CONSTRAINT "fk_ddon_character_matching_profile_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_character_arisen_profile"
(
    "character_id"    INTEGER PRIMARY KEY NOT NULL,
    "background_id"   SMALLINT            NOT NULL,
    "title_uid"       INTEGER             NOT NULL,
    "title_index"     INTEGER             NOT NULL,
    "motion_id"       SMALLINT            NOT NULL,
    "motion_frame_no" INTEGER             NOT NULL,
    CONSTRAINT "fk_ddon_character_arisen_profile_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_character_job_data"
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
    CONSTRAINT "pk_ddon_character_job_data" PRIMARY KEY ("character_common_id", "job"),
    CONSTRAINT "fk_ddon_character_job_data_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_storage
(
    "character_id" INTEGER  NOT NULL,
    "storage_type" SMALLINT NOT NULL,
    "slot_max"     SMALLINT NOT NULL,
    "item_sort"    BLOB     NOT NULL,
    CONSTRAINT "pk_ddon_storage" PRIMARY KEY ("character_id", "storage_type"),
    CONSTRAINT "fk_ddon_storage_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_wallet_point
(
    "character_id" INTEGER  NOT NULL,
    "type"         SMALLINT NOT NULL,
    "value"        INTEGER  NOT NULL,
    CONSTRAINT "pk_ddon_wallet_point" PRIMARY KEY ("character_id", "type"),
    CONSTRAINT "fk_ddon_wallet_point_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_storage_item"
(
    "item_uid"     VARCHAR(8) PRIMARY KEY NOT NULL,
    "character_id" INTEGER                NOT NULL,
    "storage_type" SMALLINT               NOT NULL,
    "slot_no"      SMALLINT               NOT NULL,
    "item_id"      INTEGER                NOT NULL,
    "item_num"     INTEGER                NOT NULL,
    "safety"       SMALLINT               NOT NULL,
    "color"        SMALLINT               NOT NULL,
    "plus_value"   SMALLINT               NOT NULL,
    "equip_points" INTEGER                NOT NULL,
    CONSTRAINT "uq_ddon_storage_item_character_id_storage_type_slot_no" UNIQUE ("character_id", "storage_type", "slot_no"),
    CONSTRAINT "fk_ddon_storage_item_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

-- CREATE TABLE IF NOT EXISTS ddon_additional_status
-- (
--     "item_uid"          VARCHAR(8) NOT NULL,
--     "character_id"      INTEGER NOT NULL,
--     "is_add_stat1"      TINYINT NOT NULL,
--     "is_add_stat2"      TINYINT NOT NULL,
--     "additional_status1" SMALLINT NOT NULL,
--     "additional_status2" SMALLINT NOT NULL,
--     CONSTRAINT pk_ddon_additional_status PRIMARY KEY ("item_uid"),
--     CONSTRAINT fk_additional_status_item_uid FOREIGN KEY ("item_uid") REFERENCES ddon_storage_item ("item_uid") ON DELETE CASCADE,
--     CONSTRAINT fk_additional_status_character_id FOREIGN KEY ("character_id") REFERENCES ddon_character ("character_id") ON DELETE CASCADE
-- );
-- Put in comments because it seems this might be apart of a larger system. TODO: Revisit this when we start messing around with Craig's crafting.


CREATE TABLE IF NOT EXISTS "ddon_equip_item"
(
    "item_uid"            VARCHAR(8) NOT NULL,
    "character_common_id" INTEGER    NOT NULL,
    "job"                 SMALLINT   NOT NULL,
    "equip_type"          SMALLINT   NOT NULL,
    "equip_slot"          SMALLINT   NOT NULL,
    CONSTRAINT "pk_ddon_equip_item" PRIMARY KEY ("character_common_id", "job", "equip_type", "equip_slot"),
    CONSTRAINT "fk_ddon_equip_item_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_equip_job_item"
(
    "item_uid"            VARCHAR(8) NOT NULL,
    "character_common_id" INTEGER    NOT NULL,
    "job"                 SMALLINT   NOT NULL,
    "equip_slot"          SMALLINT   NOT NULL,
    CONSTRAINT "pk_ddon_equip_job_item" PRIMARY KEY ("character_common_id", "job", "equip_slot"),
    CONSTRAINT "fk_ddon_equip_job_item_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_normal_skill_param"
(
    "character_common_id" INTEGER  NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "skill_no"            INTEGER  NOT NULL,
    "index"               INTEGER  NOT NULL,
    "pre_skill_no"        INTEGER  NOT NULL,
    CONSTRAINT "pk_ddon_normal_skill_param" PRIMARY KEY ("character_common_id", "job", "skill_no"),
    CONSTRAINT "fk_ddon_normal_skill_param_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_learned_custom_skill"
(
    "character_common_id" INTEGER  NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "skill_id"            INTEGER  NOT NULL,
    "skill_lv"            SMALLINT NOT NULL,
    CONSTRAINT "pk_ddon_learned_custom_skill" PRIMARY KEY ("character_common_id", "job", "skill_id"),
    CONSTRAINT "fk_ddon_learned_custom_skill_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_equipped_custom_skill"
(
    "character_common_id" INTEGER  NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "slot_no"             SMALLINT NOT NULL,
    "skill_id"            INTEGER  NOT NULL,
    CONSTRAINT "pk_ddon_equipped_custom_skill" PRIMARY KEY ("character_common_id", "job", "slot_no"),
    CONSTRAINT "fk_ddon_equipped_custom_skill_character_common_id" FOREIGN KEY ("character_common_id", "job", "skill_id") REFERENCES "ddon_learned_custom_skill" ("character_common_id", "job", "skill_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_learned_ability"
(
    "character_common_id" INTEGER  NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "ability_id"          INTEGER  NOT NULL,
    "ability_lv"          SMALLINT NOT NULL,
    CONSTRAINT "pk_ddon_learned_ability" PRIMARY KEY ("character_common_id", "job", "ability_id"),
    CONSTRAINT "fk_ddon_learned_ability_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_equipped_ability"
(
    "character_common_id" INTEGER  NOT NULL,
    "equipped_to_job"     SMALLINT NOT NULL,
    "job"                 SMALLINT NOT NULL,
    "slot_no"             SMALLINT NOT NULL,
    "ability_id"          INTEGER  NOT NULL,
    CONSTRAINT "pk_ddon_equipped_ability" PRIMARY KEY ("character_common_id", "equipped_to_job", "slot_no"),
    CONSTRAINT "fk_ddon_equipped_ability_character_common_id" FOREIGN KEY ("character_common_id", "job", "ability_id") REFERENCES "ddon_learned_ability" ("character_common_id", "job", "ability_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_shortcut"
(
    "character_id" INTEGER  NOT NULL,
    "page_no"      INTEGER  NOT NULL,
    "button_no"    INTEGER  NOT NULL,
    "shortcut_id"  INTEGER  NOT NULL,
    u32_data       INTEGER  NOT NULL,
    f32_data       INTEGER  NOT NULL,
    "exex_type"    SMALLINT NOT NULL,
    CONSTRAINT "pk_ddon_shortcut" PRIMARY KEY ("character_id", "page_no", "button_no"),
    CONSTRAINT "fk_ddon_shortcut_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_communication_shortcut"
(
    "character_id" INTEGER  NOT NULL,
    "page_no"      INTEGER  NOT NULL,
    "button_no"    INTEGER  NOT NULL,
    "type"         SMALLINT NOT NULL,
    "category"     SMALLINT NOT NULL,
    "id"           INTEGER  NOT NULL,
    CONSTRAINT "pk_ddon_communication_shortcut" PRIMARY KEY ("character_id", "page_no", "button_no"),
    CONSTRAINT "fk_ddon_communication_shortcut_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_pawn_reaction"
(
    "pawn_id"       INTEGER  NOT NULL,
    "reaction_type" SMALLINT NOT NULL,
    "motion_no"     INTEGER  NOT NULL,
    CONSTRAINT "pk_ddon_pawn_reaction" PRIMARY KEY ("pawn_id", "reaction_type"),
    CONSTRAINT "fk_ddon_pawn_reaction_pawn_id" FOREIGN KEY ("pawn_id") REFERENCES "ddon_pawn" ("pawn_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_sp_skill"
(
    "pawn_id"     INTEGER  NOT NULL,
    "job"         SMALLINT NOT NULL,
    "sp_skill_id" SMALLINT NOT NULL,
    "sp_skill_lv" SMALLINT NOT NULL,
    CONSTRAINT "pk_ddon_sp_skill" PRIMARY KEY ("pawn_id", "job", "sp_skill_id"),
    CONSTRAINT "fk_ddon_sp_skill_pawn_id" FOREIGN KEY ("pawn_id") REFERENCES "ddon_pawn" ("pawn_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_released_warp_point"
(
    "character_id"     INTEGER NOT NULL,
    "warp_point_id"    INTEGER NOT NULL,
    "favorite_slot_no" INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_released_warp_point" PRIMARY KEY ("character_id", "warp_point_id"),
    CONSTRAINT "fk_ddon_released_warp_point_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_game_token"
(
    "account_id"   INTEGER PRIMARY KEY NOT NULL,
    "character_id" INTEGER             NOT NULL,
    "token"        TEXT                NOT NULL,
    "created"      DATETIME            NOT NULL,
    CONSTRAINT "uq_ddon_game_token_token" UNIQUE ("token"),
    CONSTRAINT "fk_ddon_game_token_account_id" FOREIGN KEY ("account_id") REFERENCES "account" ("id") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_game_token_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id")
);

CREATE TABLE IF NOT EXISTS "ddon_connection"
(
    "server_id"  INTEGER  NOT NULL,
    "account_id" INTEGER  NOT NULL,
    "type"       INTEGER  NOT NULL,
    "created"    DATETIME NOT NULL,
    CONSTRAINT "uq_ddon_connection_server_id_account_id" UNIQUE ("server_id", "account_id"),
    CONSTRAINT "fk_ddon_connection_token_account_id" FOREIGN KEY ("account_id") REFERENCES "account" ("id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_dragon_force_augmentation"
(
    "character_common_id" INTEGER NOT NULL,
    "element_id"          INTEGER NOT NULL,
    "page_no"             INTEGER NOT NULL,
    "group_no"            INTEGER NOT NULL,
    "index_no"            INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_dragon_force_augmentation" PRIMARY KEY ("character_common_id", "element_id"),
    CONSTRAINT "fk_ddon_dragon_force_augmentation_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_orb_gain_extend_param"
(
    "character_common_id" INTEGER PRIMARY KEY NOT NULL,
    "hp_max"              INTEGER             NOT NULL,
    "stamina_max"         INTEGER             NOT NULL,
    "physical_attack"     INTEGER             NOT NULL,
    "physical_defence"    INTEGER             NOT NULL,
    "magic_attack"        INTEGER             NOT NULL,
    "magic_defence"       INTEGER             NOT NULL,
    "ability_cost"        INTEGER             NOT NULL,
    "jewelry_slot"        INTEGER             NOT NULL,
    "use_item_slot"       INTEGER             NOT NULL,
    "material_item_slot"  INTEGER             NOT NULL,
    "equip_item_slot"     INTEGER             NOT NULL,
    "main_pawn_slot"      INTEGER             NOT NULL,
    "support_pawn_slot"   INTEGER             NOT NULL,
    CONSTRAINT "fk_ddon_orb_gain_extend_param_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_unlocked_secret_ability"
(
    "character_common_id" INTEGER NOT NULL,
    "ability_id"          INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_unlocked_secret_ability" PRIMARY KEY ("character_common_id", "ability_id"),
    CONSTRAINT "fk_unlocked_secret_ability_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_contact_list"
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

CREATE TABLE IF NOT EXISTS "ddon_bazaar_exhibition"
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
    CONSTRAINT "fk_ddon_bazaar_exhibition_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_reward_box"
(
    "uniq_reward_id"       INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "character_common_id"  INTEGER                           NOT NULL,
    "quest_schedule_id"    INTEGER                           NOT NULL,
    "num_random_rewards"   INTEGER                           NOT NULL,
    "random_reward0_index" INTEGER                           NOT NULL,
    "random_reward1_index" INTEGER                           NOT NULL,
    "random_reward2_index" INTEGER                           NOT NULL,
    "random_reward3_index" INTEGER                           NOT NULL,
    CONSTRAINT "fk_ddon_reward_box_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_quest_progress"
(
    "character_common_id" INTEGER NOT NULL,
    "quest_type"          INTEGER NOT NULL,
    "quest_schedule_id"   INTEGER NOT NULL,
    "step"                INTEGER NOT NULL,
    CONSTRAINT "fk_ddon_quest_progress_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_completed_quests"
(
    "character_common_id" INTEGER NOT NULL,
    "quest_type"          INTEGER NOT NULL,
    "quest_id"            INTEGER NOT NULL,
    "clear_count"         INTEGER NOT NULL DEFAULT 0,
    CONSTRAINT "fk_ddon_completed_quests_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_priority_quests"
(
    "character_common_id" INTEGER NOT NULL,
    "quest_schedule_id"   INTEGER NOT NULL,
    CONSTRAINT "fk_ddon_priority_quests_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_system_mail"
(
    "message_id"    INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "character_id"  INTEGER                           NOT NULL,
    "message_state" INTEGER                           NOT NULL,
    "sender_name"   VARCHAR(256)                      NOT NULL DEFAULT '',
    "message_title" VARCHAR(256)                      NOT NULL DEFAULT '',
    "message_body"  VARCHAR(2048)                     NOT NULL DEFAULT '',
    "send_date"     INTEGER                           NOT NULL DEFAULT 0,
    CONSTRAINT "fk_ddon_system_mail_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_system_mail_attachment"
(
    "attachment_id"   INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    "message_id"      INTEGER                           NOT NULL,
    "attachment_type" INTEGER                           NOT NULL,
    "is_received"     BOOLEAN                           NOT NULL DEFAULT FALSE,
    "param0"          VARCHAR(256)                      NOT NULL DEFAULT '',
    "param1"          INTEGER                           NOT NULL DEFAULT 0,
    "param2"          INTEGER                           NOT NULL DEFAULT 0,
    "param3"          INTEGER                           NOT NULL DEFAULT 0,
    CONSTRAINT "fk_ddon_system_mail_attachment_message_id" FOREIGN KEY ("message_id") REFERENCES "ddon_system_mail" ("message_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_character_playpoint_data
(
    "character_id" INTEGER  NOT NULL,
    "job"          SMALLINT NOT NULL,
    "play_point"   INTEGER  NOT NULL DEFAULT 0,
    "exp_mode"     TINYINT  NOT NULL DEFAULT 1,
    CONSTRAINT "pk_ddon_character_playpoint_data" PRIMARY KEY ("character_id", "job"),
    CONSTRAINT "fk_ddon_character_playpoint_data_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_stamp_bonus"
(
    "character_id"      INTEGER PRIMARY KEY NOT NULL,
    "last_stamp_time"   DATETIME            NOT NULL,
    "consecutive_stamp" INTEGER             NOT NULL,
    "total_stamp"       INTEGER             NOT NULL,
    CONSTRAINT "fk_ddon_stamp_bonus_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_crests"
(
    "character_common_id" INTEGER    NOT NULL,
    "item_uid"            VARCHAR(8) NOT NULL,
    "slot"                INTEGER    NOT NULL,
    "crest_id"            INTEGER    NOT NULL,
    "crest_amount"        INTEGER    NOT NULL,
    CONSTRAINT "fk_ddon_crests_item_uid" FOREIGN KEY ("item_uid") REFERENCES "ddon_storage_item" ("item_uid") ON DELETE CASCADE,
    CONSTRAINT "fk_ddon_crests_character_common_id" FOREIGN KEY ("character_common_id") REFERENCES "ddon_character_common" ("character_common_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_preset_ability"
(
    "character_id" INTEGER NOT NULL,
    "preset_no"    INTEGER NOT NULL,
    "preset_name"  TEXT,
    "ability_1"    INTEGER,
    "ability_2"    INTEGER,
    "ability_3"    INTEGER,
    "ability_4"    INTEGER,
    "ability_5"    INTEGER,
    "ability_6"    INTEGER,
    "ability_7"    INTEGER,
    "ability_8"    INTEGER,
    "ability_9"    INTEGER,
    "ability_10"   INTEGER,
    CONSTRAINT "pk_ddon_preset_ability" PRIMARY KEY ("character_id", "preset_no"),
    CONSTRAINT "fk_ddon_preset_ability_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_pawn_craft_progress"
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

CREATE TABLE IF NOT EXISTS "ddon_binary_data"
(
    "character_id" INTEGER PRIMARY KEY NOT NULL,
    "binary_data"  BLOB                NOT NULL,
    CONSTRAINT "fk_ddon_binary_data_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_bbm_character_map"
(
    "character_id"     INTEGER PRIMARY KEY NOT NULL,
    "bbm_character_id" INTEGER             NOT NULL,
    CONSTRAINT "fk_ddon_bbm_character_map_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_bbm_progress"
(
    "character_id"     INTEGER PRIMARY KEY NOT NULL,
    "start_time"       INTEGER             NOT NULL,
    "content_id"       INTEGER             NOT NULL,
    "content_mode"     INTEGER             NOT NULL,
    "tier"             INTEGER             NOT NULL,
    "killed_death"     BOOLEAN             NOT NULL,
    "last_ticket_time" INTEGER             NOT NULL,
    CONSTRAINT "fk_ddon_bbm_progress_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_bbm_rewards"
(
    "character_id" INTEGER PRIMARY KEY NOT NULL,
    "gold_marks"   INTEGER             NOT NULL,
    "silver_marks" INTEGER             NOT NULL,
    "red_marks"    INTEGER             NOT NULL,
    CONSTRAINT "fk_ddon_bbm_rewards_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_bbm_content_treasure"
(
    "character_id" INTEGER PRIMARY KEY NOT NULL,
    "content_id"   INTEGER             NOT NULL,
    "amount"       INTEGER             NOT NULL,
    CONSTRAINT "fk_ddon_bbm_content_treasure_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character" ("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_clan_param"
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

CREATE TABLE IF NOT EXISTS "ddon_clan_membership"
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

CREATE TABLE IF NOT EXISTS "ddon_clan_shop_purchases"
(
    "clan_id"               INTEGER     NOT NULL,
    "lineup_id"             INTEGER     NOT NULL,
    CONSTRAINT "pk_ddon_clan_shop_purchases" PRIMARY KEY ("clan_id", "lineup_id"),
    CONSTRAINT "fl_ddon_clan_shop_purchases_clan_id" FOREIGN KEY ("clan_id") REFERENCES "ddon_clan_param" ("clan_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_clan_base_customization"
(
    "clan_id"               INTEGER     NOT NULL,
    "type"                  INTEGER     NOT NULL,
    "furniture_id"          INTEGER     NOT NULL,
    CONSTRAINT "pk_ddon_clan_base_customization" PRIMARY KEY ("clan_id", "type"),
    CONSTRAINT "fl_ddon_clan_base_customization_clan_id" FOREIGN KEY ("clan_id") REFERENCES "ddon_clan_param" ("clan_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_epitaph_road_unlocks" (
	"character_id"	INTEGER NOT NULL,
	"epitaph_id"	INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_epitaph_road_unlocks" PRIMARY KEY ("character_id", "epitaph_id"),
	CONSTRAINT "fk_ddon_epitaph_road_unlocks_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_epitaph_claimed_weekly_rewards" (
	"character_id"	INTEGER NOT NULL,
	"epitaph_id"	INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_epitaph_claimed_weekly_rewards" PRIMARY KEY ("character_id", "epitaph_id"),
	CONSTRAINT "fk_ddon_epitaph_claimed_weekly_rewards_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_schedule_next" (
	"type"	INTEGER NOT NULL,
	"timestamp"	BIGINT NOT NULL,
	PRIMARY KEY("type")
);
INSERT INTO ddon_schedule_next(type, timestamp) VALUES (19, 0);

CREATE TABLE IF NOT EXISTS "ddon_area_rank" (
    "character_id"      INTEGER NOT NULL,
    "area_id"           INTEGER NOT NULL,
    "rank"              INTEGER NOT NULL,
    "point"             INTEGER NOT NULL,
    "week_point"        INTEGER NOT NULL,
    "last_week_point"   INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_area_rank" PRIMARY KEY ("character_id", "area_id"),
	CONSTRAINT "fk_ddon_area_rank_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "ddon_area_rank_supply" (
    "character_id"      INTEGER NOT NULL,
    "area_id"           INTEGER NOT NULL,
    "index"             INTEGER NOT NULL,
    "item_id"           INTEGER NOT NULL,
    "num"               INTEGER NOT NULL,
    CONSTRAINT "pk_ddon_area_rank_supply" PRIMARY KEY ("character_id", "area_id", "index"),
	CONSTRAINT "fk_ddon_area_rank_supply_character_id" FOREIGN KEY ("character_id") REFERENCES "ddon_character"("character_id") ON DELETE CASCADE
);

INSERT INTO "ddon_schedule_next" ("type", "timestamp") VALUES (4, 0);
