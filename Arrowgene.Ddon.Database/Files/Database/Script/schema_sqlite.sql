CREATE TABLE IF NOT EXISTS `setting`
(
    `key`   TEXT NOT NULL,
    `value` TEXT NOT NULL,
    PRIMARY KEY (`key`)
);

CREATE TABLE IF NOT EXISTS `account`
(
    `id`                  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `name`                TEXT                              NOT NULL,
    `normal_name`         TEXT                              NOT NULL,
    `hash`                TEXT                              NOT NULL,
    `mail`                TEXT                              NOT NULL,
    `mail_verified`       INTEGER                           NOT NULL,
    `mail_verified_at`    DATETIME DEFAULT NULL,
    `mail_token`          TEXT     DEFAULT NULL,
    `password_token`      TEXT     DEFAULT NULL,
    `login_token`         TEXT     DEFAULT NULL,
    `login_token_created` DATETIME DEFAULT NULL,
    `state`               INTEGER                           NOT NULL,
    `last_login`          DATETIME DEFAULT NULL,
    `created`             DATETIME                          NOT NULL,
    CONSTRAINT `uq_account_name` UNIQUE (`name`),
    CONSTRAINT `uq_account_normal_name` UNIQUE (`normal_name`),
    CONSTRAINT `uq_account_login_token` UNIQUE (`login_token`),
    CONSTRAINT `uq_account_mail` UNIQUE (`mail`)
);

CREATE TABLE IF NOT EXISTS `ddon_character_common`
(
    `character_common_id`        INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `job`                        TINYINT                           NOT NULL,
    `hide_equip_head`            BIT                               NOT NULL,
    `hide_equip_lantern`         BIT                               NOT NULL,
    `jewelry_slot_num`           TINYINT                           NOT NULL
);

CREATE TABLE IF NOT EXISTS `ddon_character`
(
    `character_id`               INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `character_common_id`        INTEGER                           NOT NULL,
    `account_id`                 INTEGER                           NOT NULL,
    `version`                    INTEGER                           NOT NULL,
    `first_name`                 TEXT                              NOT NULL,
    `last_name`                  TEXT                              NOT NULL,
    `created`                    DATETIME                          NOT NULL,
    `my_pawn_slot_num`           TINYINT                           NOT NULL,
    `rental_pawn_slot_num`       TINYINT                           NOT NULL,
    `hide_equip_head_pawn`       BIT                               NOT NULL,
    `hide_equip_lantern_pawn`    BIT                               NOT NULL,
    `arisen_profile_share_range` TINYINT                           NOT NULL,
    CONSTRAINT `fk_character_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE,
    CONSTRAINT `fk_character_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_pawn`
(
    `pawn_id`                    INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    `character_common_id`        INTEGER                           NOT NULL,
    `character_id`               INTEGER                           NOT NULL,
    `name`                       TEXT                              NOT NULL,
    `hm_type`                    TINYINT                           NOT NULL,
    `pawn_type`                  TINYINT                           NOT NULL,
    CONSTRAINT `fk_character_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE,
    CONSTRAINT `fk_character_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddon_character` (`character_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_edit_info`
(
    `character_common_id`                INTEGER PRIMARY KEY NOT NULL,
    `sex`                         BIT                 NOT NULL,
    `voice`                       TINYINT             NOT NULL,
    `voice_pitch`                 SMALLINT            NOT NULL,
    `personality`                 TINYINT             NOT NULL,
    `speech_freq`                 TINYINT             NOT NULL,
    `body_type`                   TINYINT             NOT NULL,
    `hair`                        TINYINT             NOT NULL,
    `beard`                       TINYINT             NOT NULL,
    `makeup`                      TINYINT             NOT NULL,
    `scar`                        TINYINT             NOT NULL,
    `eye_preset_no`               TINYINT             NOT NULL,
    `nose_preset_no`              TINYINT             NOT NULL,
    `mouth_preset_no`             TINYINT             NOT NULL,
    `eyebrow_tex_no`              TINYINT             NOT NULL,
    `color_skin`                  TINYINT             NOT NULL,
    `color_hair`                  TINYINT             NOT NULL,
    `color_beard`                 TINYINT             NOT NULL,
    `color_eyebrow`               TINYINT             NOT NULL,
    `color_r_eye`                 TINYINT             NOT NULL,
    `color_l_eye`                 TINYINT             NOT NULL,
    `color_makeup`                TINYINT             NOT NULL,
    `sokutobu`                    SMALLINT            NOT NULL,
    `hitai`                       SMALLINT            NOT NULL,
    `mimi_jyouge`                 SMALLINT            NOT NULL,
    `kannkaku`                    SMALLINT            NOT NULL,
    `mabisasi_jyouge`             SMALLINT            NOT NULL,
    `hanakuchi_jyouge`            SMALLINT            NOT NULL,
    `ago_saki_haba`               SMALLINT            NOT NULL,
    `ago_zengo`                   SMALLINT            NOT NULL,
    `ago_saki_jyouge`             SMALLINT            NOT NULL,
    `hitomi_ookisa`               SMALLINT            NOT NULL,
    `me_ookisa`                   SMALLINT            NOT NULL,
    `me_kaiten`                   SMALLINT            NOT NULL,
    `mayu_kaiten`                 SMALLINT            NOT NULL,
    `mimi_ookisa`                 SMALLINT            NOT NULL,
    `mimi_muki`                   SMALLINT            NOT NULL,
    `elf_mimi`                    SMALLINT            NOT NULL,
    `miken_takasa`                SMALLINT            NOT NULL,
    `miken_haba`                  SMALLINT            NOT NULL,
    `hohobone_ryou`               SMALLINT            NOT NULL,
    `hohobone_jyouge`             SMALLINT            NOT NULL,
    `hohoniku`                    SMALLINT            NOT NULL,
    `erahone_jyouge`              SMALLINT            NOT NULL,
    `erahone_haba`                SMALLINT            NOT NULL,
    `hana_jyouge`                 SMALLINT            NOT NULL,
    `hana_haba`                   SMALLINT            NOT NULL,
    `hana_takasa`                 SMALLINT            NOT NULL,
    `hana_kakudo`                 SMALLINT            NOT NULL,
    `kuchi_haba`                  SMALLINT            NOT NULL,
    `kuchi_atsusa`                SMALLINT            NOT NULL,
    `eyebrow_uv_offset_x`         SMALLINT            NOT NULL,
    `eyebrow_uv_offset_y`         SMALLINT            NOT NULL,
    `wrinkle`                     SMALLINT            NOT NULL,
    `wrinkle_albedo_blend_rate`   SMALLINT            NOT NULL,
    `wrinkle_detail_normal_power` SMALLINT            NOT NULL,
    `muscle_albedo_blend_rate`    SMALLINT            NOT NULL,
    `muscle_detail_normal_power`  SMALLINT            NOT NULL,
    `height`                      SMALLINT            NOT NULL,
    `head_size`                   SMALLINT            NOT NULL,
    `neck_offset`                 SMALLINT            NOT NULL,
    `neck_scale`                  SMALLINT            NOT NULL,
    `upper_body_scale_x`          SMALLINT            NOT NULL,
    `belly_size`                  SMALLINT            NOT NULL,
    `teat_scale`                  SMALLINT            NOT NULL,
    `tekubi_size`                 SMALLINT            NOT NULL,
    `koshi_offset`                SMALLINT            NOT NULL,
    `koshi_size`                  SMALLINT            NOT NULL,
    `ankle_offset`                SMALLINT            NOT NULL,
    `fat`                         SMALLINT            NOT NULL,
    `muscle`                      SMALLINT            NOT NULL,
    `motion_filter`               SMALLINT            NOT NULL,
    CONSTRAINT `fk_edit_info_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_status_info`
(
    `character_common_id`       INTEGER PRIMARY KEY NOT NULL,
    `hp`                 INT                 NOT NULL,
    `stamina`            INT                 NOT NULL,
    `revive_point`       TINYINT             NOT NULL,
    `max_hp`             INT                 NOT NULL,
    `max_stamina`        INT                 NOT NULL,
    `white_hp`           INT                 NOT NULL,
    `gain_hp`            INT                 NOT NULL,
    `gain_stamina`       INT                 NOT NULL,
    `gain_attack`        INT                 NOT NULL,
    `gain_defense`       INT                 NOT NULL,
    `gain_magic_attack`  INT                 NOT NULL,
    `gain_magic_defense` INT                 NOT NULL,
    CONSTRAINT `fk_status_info_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_character_matching_profile`
(
    `character_id`      INTEGER PRIMARY KEY NOT NULL,
    `entry_job`         TINYINT             NOT NULL,
    `entry_job_level`   INT                 NOT NULL,
    `current_job`       TINYINT             NOT NULL,
    `current_job_level` INT                 NOT NULL,
    `objective_type1`   INT                 NOT NULL,
    `objective_type2`   INT                 NOT NULL,
    `play_style`        INT                 NOT NULL,
    `comment`           TEXT                NOT NULL,
    `is_join_party`     TINYINT             NOT NULL,
    CONSTRAINT `fk_matching_profile_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddon_character` (`character_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_character_arisen_profile`
(
    `character_id`    INTEGER PRIMARY KEY NOT NULL,
    `background_id`   TINYINT             NOT NULL,
    `title_uid`       INT                 NOT NULL,
    `title_index`     INT                 NOT NULL,
    `motion_id`       SMALLINT            NOT NULL,
    `motion_frame_no` INT                 NOT NULL,
    CONSTRAINT `fk_arisen_profile_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddon_character` (`character_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_character_job_data`
(
    `character_common_id`   INTEGER  NOT NULL,
    `job`                   TINYINT  NOT NULL,
    `exp`                   INT      NOT NULL,
    `job_point`             INT      NOT NULL,
    `lv`                    INT      NOT NULL,
    `atk`                   SMALLINT NOT NULL,
    `def`                   SMALLINT NOT NULL,
    `m_atk`                 SMALLINT NOT NULL,
    `m_def`                 SMALLINT NOT NULL,
    `strength`              SMALLINT NOT NULL,
    `down_power`            SMALLINT NOT NULL,
    `shake_power`           SMALLINT NOT NULL,
    `stun_power`            SMALLINT NOT NULL,
    `consitution`           SMALLINT NOT NULL,
    `guts`                  SMALLINT NOT NULL,
    `fire_resist`           TINYINT  NOT NULL,
    `ice_resist`            TINYINT  NOT NULL,
    `thunder_resist`        TINYINT  NOT NULL,
    `holy_resist`           TINYINT  NOT NULL,
    `dark_resist`           TINYINT  NOT NULL,
    `spread_resist`         TINYINT  NOT NULL,
    `freeze_resist`         TINYINT  NOT NULL,
    `shock_resist`          TINYINT  NOT NULL,
    `absorb_resist`         TINYINT  NOT NULL,
    `dark_elm_resist`       TINYINT  NOT NULL,
    `poison_resist`         TINYINT  NOT NULL,
    `slow_resist`           TINYINT  NOT NULL,
    `sleep_resist`          TINYINT  NOT NULL,
    `stun_resist`           TINYINT  NOT NULL,
    `wet_resist`            TINYINT  NOT NULL,
    `oil_resist`            TINYINT  NOT NULL,
    `seal_resist`           TINYINT  NOT NULL,
    `curse_resist`          TINYINT  NOT NULL,
    `soft_resist`           TINYINT  NOT NULL,
    `stone_resist`          TINYINT  NOT NULL,
    `gold_resist`           TINYINT  NOT NULL,
    `fire_reduce_resist`    TINYINT  NOT NULL,
    `ice_reduce_resist`     TINYINT  NOT NULL,
    `thunder_reduce_resist` TINYINT  NOT NULL,
    `holy_reduce_resist`    TINYINT  NOT NULL,
    `dark_reduce_resist`    TINYINT  NOT NULL,
    `atk_down_resist`       TINYINT  NOT NULL,
    `def_down_resist`       TINYINT  NOT NULL,
    `m_atk_down_resist`     TINYINT  NOT NULL,
    `m_def_down_resist`     TINYINT  NOT NULL,
    PRIMARY KEY (`character_common_id`, `job`),
    CONSTRAINT `fk_character_job_data_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_storage`
(
    `character_id`  INTEGER             NOT NULL,
    `storage_type`  TINYINT             NOT NULL,
    `slot_max`      SMALLINT            NOT NULL,
    `item_sort`     BLOB                NOT NULL,
    PRIMARY KEY (`character_id`, `storage_type`),
    CONSTRAINT `fk_storage_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddon_character` (`character_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_wallet_point`
(
    `character_id`  INTEGER             NOT NULL,
    `type`          TINYINT             NOT NULL,
    `value`         INTEGER             NOT NULL,
    PRIMARY KEY (`character_id`, `type`),
    CONSTRAINT `fk_wallet_point_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddon_character` (`character_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_item`
(
    `uid`          TEXT             NOT NULL,
    `item_id`      INT              NOT NULL,
    `unk3`         TINYINT          NOT NULL,
    `color`        TINYINT          NOT NULL,
    `plus_value`   TINYINT          NOT NULL,
    PRIMARY KEY (`uid`)
);

CREATE TABLE IF NOT EXISTS `ddon_storage_item`
(
    `item_uid`     TEXT             NOT NULL,
    `character_id` INTEGER          NOT NULL,
    `storage_type` TINYINT          NOT NULL,
    `slot_no`      SMALLINT         NOT NULL,
    `item_num`     INT              NOT NULL,
    PRIMARY KEY (`character_id`, `storage_type`, `slot_no`),
    CONSTRAINT `fk_storage_item_item_uid` FOREIGN KEY (`item_uid`) REFERENCES `ddon_item` (`uid`) ON DELETE CASCADE,
    CONSTRAINT `fk_storage_item_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddon_character` (`character_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_equip_item`
(
    `item_uid`            TEXT             NOT NULL,
    `character_common_id` INTEGER          NOT NULL,
    `job`                 TINYINT          NOT NULL,
    `equip_type`          TINYINT          NOT NULL,
    `equip_slot`          SMALLINT         NOT NULL,
    PRIMARY KEY (`character_common_id`, `job`, `equip_type`, `equip_slot`),
    CONSTRAINT `fk_equip_item_item_uid` FOREIGN KEY (`item_uid`) REFERENCES `ddon_item` (`uid`) ON DELETE CASCADE,
    CONSTRAINT `fk_equip_item_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_equip_job_item`
(
    `character_common_id`  INTEGER NOT NULL,
    `job`           TINYINT NOT NULL,
    `job_item_id`   INT     NOT NULL,
    `equip_slot_no` TINYINT NOT NULL,
    PRIMARY KEY (`character_common_id`, `job`, `equip_slot_no`),
    CONSTRAINT `fk_equip_job_item_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_normal_skill_param`
(
    `character_common_id` INTEGER NOT NULL,
    `job`          TINYINT NOT NULL,
    `skill_no`     INT     NOT NULL,
    `index`        INT     NOT NULL,
    `pre_skill_no` INT     NOT NULL,
    PRIMARY KEY (`character_common_id`, `job`, `skill_no`),
    CONSTRAINT `fk_normal_skill_param_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_learned_custom_skill`
(
    `character_common_id`   INTEGER NOT NULL,
    `job`            TINYINT NOT NULL,
    `skill_id` INT     NOT NULL,
    `skill_lv` TINYINT NOT NULL,
    PRIMARY KEY (`character_common_id`, `job`, `skill_id`),
    CONSTRAINT `fk_learned_custom_skill_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_equipped_custom_skill`
(
    `character_common_id`   INTEGER NOT NULL,
    `job`            TINYINT NOT NULL,
    `slot_no`        TINYINT NOT NULL,
    `skill_id` INT     NOT NULL,
    PRIMARY KEY (`character_common_id`, `job`, `slot_no`),
    CONSTRAINT `fk_equipped_custom_skill_character_common_id` FOREIGN KEY (`character_common_id`, `job`, `skill_id`) REFERENCES `ddon_learned_custom_skill` (`character_common_id`, `job`, `skill_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_learned_ability`
(
    `character_common_id`    INTEGER NOT NULL,
    `job`             TINYINT NOT NULL,
    `ability_id` INT     NOT NULL,
    `ability_lv` TINYINT NOT NULL,
    PRIMARY KEY (`character_common_id`, `job`, `ability_id`),
    CONSTRAINT `fk_learned_ability_character_common_id` FOREIGN KEY (`character_common_id`) REFERENCES `ddon_character_common` (`character_common_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_equipped_ability`
(
    `character_common_id`    INTEGER NOT NULL,
    `equipped_to_job` TINYINT NOT NULL,
    `job`             TINYINT NOT NULL,
    `slot_no`         TINYINT NOT NULL,
    `ability_id` INT     NOT NULL,
    PRIMARY KEY (`character_common_id`, `equipped_to_job`, `slot_no`),
    CONSTRAINT `fk_equipped_ability_character_common_id` FOREIGN KEY (`character_common_id`, `job`, `ability_id`) REFERENCES `ddon_learned_ability` (`character_common_id`, `job`, `ability_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_shortcut`
(
    `character_id`    INTEGER NOT NULL,
    `page_no` INTEGER NOT NULL,
    `button_no` INTEGER NOT NULL,
    `shortcut_id` INTEGER NOT NULL,
    `u32_data` INTEGER NOT NULL,
    `f32_data` INTEGER NOT NULL,
    `exex_type` TINYINT NOT NULL,
    PRIMARY KEY (`character_id`, `page_no`, `button_no`),
    CONSTRAINT `fk_shortcut_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddon_character` (`character_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_communication_shortcut`
(
    `character_id`    INTEGER NOT NULL,
    `page_no` INTEGER NOT NULL,
    `button_no` INTEGER NOT NULL,
    `type` TINYINT NOT NULL,
    `category` TINYINT NOT NULL,
    `id` INTEGER NOT NULL,
    PRIMARY KEY (`character_id`, `page_no`, `button_no`),
    CONSTRAINT `fk_communication_shortcut_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddon_character` (`character_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_pawn_reaction`
(
    `pawn_id`       INTEGER NOT NULL,
    `reaction_type` TINYINT NOT NULL,
    `motion_no`     INTEGER NOT NULL,
    PRIMARY KEY (`pawn_id`,`reaction_type`),
    CONSTRAINT `fk_pawn_reaction_pawn_id` FOREIGN KEY (`pawn_id`) REFERENCES `ddon_pawn` (`pawn_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_sp_skill`
(
    `pawn_id`       INTEGER NOT NULL,
    `sp_skill_id`   TINYINT NOT NULL,
    `sp_skill_lv`   TINYINT NOT NULL,
    PRIMARY KEY (`pawn_id`),
    CONSTRAINT `fk_sp_skill_pawn_id` FOREIGN KEY (`pawn_id`) REFERENCES `ddon_pawn` (`pawn_id`) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ddon_game_token`
(
    `account_id`   INTEGER PRIMARY KEY NOT NULL,
    `character_id` INTEGER             NOT NULL,
    `token`        TEXT                NOT NULL,
    `created`      DATETIME            NOT NULL,
    CONSTRAINT `uq_game_token_token` UNIQUE (`token`),
    CONSTRAINT `fk_game_token_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
    CONSTRAINT `fk_game_token_character_id` FOREIGN KEY (`character_id`) REFERENCES `ddon_character` (`character_id`)
);

CREATE TABLE IF NOT EXISTS `ddon_connection`
(
    `server_id`  INTEGER  NOT NULL,
    `account_id` INTEGER  NOT NULL,
    `type`       INTEGER  NOT NULL,
    `created`    DATETIME NOT NULL,
    CONSTRAINT `uq_connection_server_id_account_id` UNIQUE (`server_id`, `account_id`),
    CONSTRAINT `fk_game_token_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
);