CREATE SCHEMA IF NOT EXISTS public;

CREATE TABLE IF NOT EXISTS setting
(
    key   TEXT NOT NULL,
    value TEXT NOT NULL,
    PRIMARY KEY (key)
);

CREATE TABLE IF NOT EXISTS account
(
    id                  SERIAL PRIMARY KEY          NOT NULL,
    name                TEXT                        NOT NULL,
    normal_name         TEXT                        NOT NULL,
    hash                TEXT                        NOT NULL,
    mail                TEXT                        NOT NULL,
    mail_verified       BOOLEAN                     NOT NULL,
    mail_verified_at    TIMESTAMP WITHOUT TIME ZONE DEFAULT NULL,
    mail_token          TEXT                        DEFAULT NULL,
    password_token      TEXT                        DEFAULT NULL,
    login_token         TEXT                        DEFAULT NULL,
    login_token_created TIMESTAMP WITHOUT TIME ZONE DEFAULT NULL,
    state               INTEGER                     NOT NULL,
    last_login          TIMESTAMP WITHOUT TIME ZONE DEFAULT NULL,
    created             TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT uq_account_name UNIQUE (name),
    CONSTRAINT uq_account_normal_name UNIQUE (normal_name),
    CONSTRAINT uq_account_login_token UNIQUE (login_token),
    CONSTRAINT uq_account_mail UNIQUE (mail)
);

CREATE TABLE IF NOT EXISTS ddon_character_common
(
    character_common_id SERIAL PRIMARY KEY NOT NULL,
    job                 INTEGER            NOT NULL,
    hide_equip_head     BOOLEAN            NOT NULL,
    hide_equip_lantern  BOOLEAN            NOT NULL,
    jewelry_slot_num    INTEGER            NOT NULL
);


CREATE TABLE IF NOT EXISTS ddon_character
(
    character_id               SERIAL PRIMARY KEY          NOT NULL,
    character_common_id        INTEGER                     NOT NULL,
    account_id                 INTEGER                     NOT NULL,
    version                    INTEGER                     NOT NULL,
    first_name                 TEXT                        NOT NULL,
    last_name                  TEXT                        NOT NULL,
    created                    TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    my_pawn_slot_num           INTEGER                     NOT NULL,
    rental_pawn_slot_num       INTEGER                     NOT NULL,
    hide_equip_head_pawn       BOOLEAN                     NOT NULL,
    hide_equip_lantern_pawn    BOOLEAN                     NOT NULL,
    arisen_profile_share_range INTEGER                     NOT NULL,
    CONSTRAINT fk_character_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE,
    CONSTRAINT fk_character_account_id FOREIGN KEY (account_id) REFERENCES account (id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_pawn
(
    pawn_id             SERIAL PRIMARY KEY NOT NULL,
    character_common_id INTEGER            NOT NULL,
    character_id        INTEGER            NOT NULL,
    name                TEXT               NOT NULL,
    hm_type             INTEGER            NOT NULL,
    pawn_type           INTEGER            NOT NULL,
    CONSTRAINT fk_character_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE,
    CONSTRAINT fk_character_character_id FOREIGN KEY (character_id) REFERENCES ddon_character (character_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_edit_info
(
    character_common_id         INTEGER PRIMARY KEY NOT NULL,
    sex                         INTEGER             NOT NULL,
    voice                       INTEGER             NOT NULL,
    voice_pitch                 INTEGER             NOT NULL,
    personality                 INTEGER             NOT NULL,
    speech_freq                 INTEGER             NOT NULL,
    body_type                   INTEGER             NOT NULL,
    hair                        INTEGER             NOT NULL,
    beard                       INTEGER             NOT NULL,
    makeup                      INTEGER             NOT NULL,
    scar                        INTEGER             NOT NULL,
    eye_preset_no               INTEGER             NOT NULL,
    nose_preset_no              INTEGER             NOT NULL,
    mouth_preset_no             INTEGER             NOT NULL,
    eyebrow_tex_no              INTEGER             NOT NULL,
    color_skin                  INTEGER             NOT NULL,
    color_hair                  INTEGER             NOT NULL,
    color_beard                 INTEGER             NOT NULL,
    color_eyebrow               INTEGER             NOT NULL,
    color_r_eye                 INTEGER             NOT NULL,
    color_l_eye                 INTEGER             NOT NULL,
    color_makeup                INTEGER             NOT NULL,
    sokutobu                    INTEGER             NOT NULL,
    hitai                       INTEGER             NOT NULL,
    mimi_jyouge                 INTEGER             NOT NULL,
    kannkaku                    INTEGER             NOT NULL,
    mabisasi_jyouge             INTEGER             NOT NULL,
    hanakuchi_jyouge            INTEGER             NOT NULL,
    ago_saki_haba               INTEGER             NOT NULL,
    ago_zengo                   INTEGER             NOT NULL,
    ago_saki_jyouge             INTEGER             NOT NULL,
    hitomi_ookisa               INTEGER             NOT NULL,
    me_ookisa                   INTEGER             NOT NULL,
    me_kaiten                   INTEGER             NOT NULL,
    mayu_kaiten                 INTEGER             NOT NULL,
    mimi_ookisa                 INTEGER             NOT NULL,
    mimi_muki                   INTEGER             NOT NULL,
    elf_mimi                    INTEGER             NOT NULL,
    miken_takasa                INTEGER             NOT NULL,
    miken_haba                  INTEGER             NOT NULL,
    hohobone_ryou               INTEGER             NOT NULL,
    hohobone_jyouge             INTEGER             NOT NULL,
    hohoniku                    INTEGER             NOT NULL,
    erahone_jyouge              INTEGER             NOT NULL,
    erahone_haba                INTEGER             NOT NULL,
    hana_jyouge                 INTEGER             NOT NULL,
    hana_haba                   INTEGER             NOT NULL,
    hana_takasa                 INTEGER             NOT NULL,
    hana_kakudo                 INTEGER             NOT NULL,
    kuchi_haba                  INTEGER             NOT NULL,
    kuchi_atsusa                INTEGER             NOT NULL,
    eyebrow_uv_offset_x         INTEGER             NOT NULL,
    eyebrow_uv_offset_y         INTEGER             NOT NULL,
    wrinkle                     INTEGER             NOT NULL,
    wrinkle_albedo_blend_rate   INTEGER             NOT NULL,
    wrinkle_detail_normal_power INTEGER             NOT NULL,
    muscle_albedo_blend_rate    INTEGER             NOT NULL,
    muscle_detail_normal_power  INTEGER             NOT NULL,
    height                      INTEGER             NOT NULL,
    head_size                   INTEGER             NOT NULL,
    neck_offset                 INTEGER             NOT NULL,
    neck_scale                  INTEGER             NOT NULL,
    upper_body_scale_x          INTEGER             NOT NULL,
    belly_size                  INTEGER             NOT NULL,
    teat_scale                  INTEGER             NOT NULL,
    tekubi_size                 INTEGER             NOT NULL,
    koshi_offset                INTEGER             NOT NULL,
    koshi_size                  INTEGER             NOT NULL,
    ankle_offset                INTEGER             NOT NULL,
    fat                         INTEGER             NOT NULL,
    muscle                      INTEGER             NOT NULL,
    motion_filter               INTEGER             NOT NULL,
    CONSTRAINT fk_edit_info_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_status_info
(
    character_common_id INTEGER PRIMARY KEY NOT NULL,
    hp                  INTEGER             NOT NULL,
    stamina             INTEGER             NOT NULL,
    revive_point        INTEGER             NOT NULL,
    max_hp              INTEGER             NOT NULL,
    max_stamina         INTEGER             NOT NULL,
    white_hp            INTEGER             NOT NULL,
    gain_hp             INTEGER             NOT NULL,
    gain_stamina        INTEGER             NOT NULL,
    gain_attack         INTEGER             NOT NULL,
    gain_defense        INTEGER             NOT NULL,
    gain_magic_attack   INTEGER             NOT NULL,
    gain_magic_defense  INTEGER             NOT NULL,
    CONSTRAINT fk_status_info_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_character_matching_profile
(
    character_id      INTEGER PRIMARY KEY NOT NULL,
    entry_job         INTEGER             NOT NULL,
    entry_job_level   INTEGER             NOT NULL,
    current_job       INTEGER             NOT NULL,
    current_job_level INTEGER             NOT NULL,
    objective_type1   INTEGER             NOT NULL,
    objective_type2   INTEGER             NOT NULL,
    play_style        INTEGER             NOT NULL,
    comment           TEXT                NOT NULL,
    is_join_party     BOOLEAN             NOT NULL,
    CONSTRAINT fk_matching_profile_character_id FOREIGN KEY (character_id) REFERENCES ddon_character (character_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_character_arisen_profile
(
    character_id    INTEGER PRIMARY KEY NOT NULL,
    background_id   INTEGER             NOT NULL,
    title_uid       INTEGER             NOT NULL,
    title_index     INTEGER             NOT NULL,
    motion_id       INTEGER             NOT NULL,
    motion_frame_no INTEGER             NOT NULL,
    CONSTRAINT fk_arisen_profile_character_id FOREIGN KEY (character_id) REFERENCES ddon_character (character_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_character_job_data
(
    character_common_id   INTEGER NOT NULL,
    job                   INTEGER NOT NULL,
    exp                   INTEGER NOT NULL,
    job_point             INTEGER NOT NULL,
    lv                    INTEGER NOT NULL,
    atk                   INTEGER NOT NULL,
    def                   INTEGER NOT NULL,
    m_atk                 INTEGER NOT NULL,
    m_def                 INTEGER NOT NULL,
    strength              INTEGER NOT NULL,
    down_power            INTEGER NOT NULL,
    shake_power           INTEGER NOT NULL,
    stun_power            INTEGER NOT NULL,
    consitution           INTEGER NOT NULL,
    guts                  INTEGER NOT NULL,
    fire_resist           INTEGER NOT NULL,
    ice_resist            INTEGER NOT NULL,
    thunder_resist        INTEGER NOT NULL,
    holy_resist           INTEGER NOT NULL,
    dark_resist           INTEGER NOT NULL,
    spread_resist         INTEGER NOT NULL,
    freeze_resist         INTEGER NOT NULL,
    shock_resist          INTEGER NOT NULL,
    absorb_resist         INTEGER NOT NULL,
    dark_elm_resist       INTEGER NOT NULL,
    poison_resist         INTEGER NOT NULL,
    slow_resist           INTEGER NOT NULL,
    sleep_resist          INTEGER NOT NULL,
    stun_resist           INTEGER NOT NULL,
    wet_resist            INTEGER NOT NULL,
    oil_resist            INTEGER NOT NULL,
    seal_resist           INTEGER NOT NULL,
    curse_resist          INTEGER NOT NULL,
    soft_resist           INTEGER NOT NULL,
    stone_resist          INTEGER NOT NULL,
    gold_resist           INTEGER NOT NULL,
    fire_reduce_resist    INTEGER NOT NULL,
    ice_reduce_resist     INTEGER NOT NULL,
    thunder_reduce_resist INTEGER NOT NULL,
    holy_reduce_resist    INTEGER NOT NULL,
    dark_reduce_resist    INTEGER NOT NULL,
    atk_down_resist       INTEGER NOT NULL,
    def_down_resist       INTEGER NOT NULL,
    m_atk_down_resist     INTEGER NOT NULL,
    m_def_down_resist     INTEGER NOT NULL,
    CONSTRAINT pk_character_job_data PRIMARY KEY (character_common_id, job),
    CONSTRAINT fk_character_job_data_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_storage
(
    character_id INTEGER NOT NULL,
    storage_type INTEGER NOT NULL,
    slot_max     INTEGER NOT NULL,
    item_sort    BYTEA   NOT NULL,
    PRIMARY KEY (character_id, storage_type),
    CONSTRAINT fk_storage_character_id FOREIGN KEY (character_id) REFERENCES ddon_character (character_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_wallet_point
(
    character_id INTEGER NOT NULL,
    type         INTEGER NOT NULL,
    value        INTEGER NOT NULL,
    PRIMARY KEY (character_id, type),
    CONSTRAINT fk_wallet_point_character_id FOREIGN KEY (character_id) REFERENCES ddon_character (character_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_item
(
    uid        TEXT    NOT NULL,
    item_id    INTEGER NOT NULL,
    unk3       INTEGER NOT NULL,
    color      INTEGER NOT NULL,
    plus_value INTEGER NOT NULL,
    PRIMARY KEY (uid)
);

CREATE TABLE IF NOT EXISTS ddon_storage_item
(
    item_uid     TEXT    NOT NULL,
    character_id INTEGER NOT NULL,
    storage_type INTEGER NOT NULL,
    slot_no      INTEGER NOT NULL,
    item_num     INTEGER NOT NULL,
    PRIMARY KEY (character_id, storage_type, slot_no),
    CONSTRAINT fk_storage_item_item_uid FOREIGN KEY (item_uid) REFERENCES ddon_item (uid) ON DELETE CASCADE,
    CONSTRAINT fk_storage_item_character_id FOREIGN KEY (character_id) REFERENCES ddon_character (character_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_equip_item
(
    item_uid            TEXT    NOT NULL,
    character_common_id INTEGER NOT NULL,
    job                 INTEGER NOT NULL,
    equip_type          INTEGER NOT NULL,
    equip_slot          INTEGER NOT NULL,
    PRIMARY KEY (character_common_id, job, equip_type, equip_slot),
    CONSTRAINT fk_equip_item_item_uid FOREIGN KEY (item_uid) REFERENCES ddon_item (uid) ON DELETE CASCADE,
    CONSTRAINT fk_equip_item_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_equip_job_item
(
    item_uid            TEXT    NOT NULL,
    character_common_id INTEGER NOT NULL,
    job                 INTEGER NOT NULL,
    equip_slot          INTEGER NOT NULL,
    PRIMARY KEY (character_common_id, job, equip_slot),
    CONSTRAINT fk_equip_job_item_item_uid FOREIGN KEY (item_uid) REFERENCES ddon_item (uid) ON DELETE CASCADE,
    CONSTRAINT fk_equip_job_item_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_normal_skill_param
(
    character_common_id INTEGER NOT NULL,
    job                 INTEGER NOT NULL,
    skill_no            INTEGER NOT NULL,
    index               INTEGER NOT NULL,
    pre_skill_no        INTEGER NOT NULL,
    PRIMARY KEY (character_common_id, job, skill_no),
    CONSTRAINT fk_normal_skill_param_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_learned_custom_skill
(
    character_common_id INTEGER NOT NULL,
    job                 INTEGER NOT NULL,
    skill_id            INTEGER NOT NULL,
    skill_lv            INTEGER NOT NULL,
    PRIMARY KEY (character_common_id, job, skill_id),
    CONSTRAINT fk_learned_custom_skill_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_equipped_custom_skill
(
    character_common_id INTEGER NOT NULL,
    job                 INTEGER NOT NULL,
    slot_no             INTEGER NOT NULL,
    skill_id            INTEGER NOT NULL,
    PRIMARY KEY (character_common_id, job, slot_no),
    CONSTRAINT fk_equipped_custom_skill_character_common_id FOREIGN KEY (character_common_id, job, skill_id) REFERENCES ddon_learned_custom_skill (character_common_id, job, skill_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_learned_ability
(
    character_common_id INTEGER NOT NULL,
    job                 INTEGER NOT NULL,
    ability_id          INTEGER NOT NULL,
    ability_lv          INTEGER NOT NULL,
    PRIMARY KEY (character_common_id, job, ability_id),
    CONSTRAINT fk_learned_ability_character_common_id FOREIGN KEY (character_common_id) REFERENCES ddon_character_common (character_common_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_equipped_ability
(
    character_common_id INTEGER NOT NULL,
    equipped_to_job     INTEGER NOT NULL,
    job                 INTEGER NOT NULL,
    slot_no             INTEGER NOT NULL,
    ability_id          INTEGER NOT NULL,
    PRIMARY KEY (character_common_id, equipped_to_job, slot_no),
    CONSTRAINT fk_equipped_ability_character_common_id FOREIGN KEY (character_common_id, job, ability_id) REFERENCES ddon_learned_ability (character_common_id, job, ability_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_shortcut
(
    character_id INTEGER NOT NULL,
    page_no      INTEGER NOT NULL,
    button_no    INTEGER NOT NULL,
    shortcut_id  INTEGER NOT NULL,
    u32_data     INTEGER NOT NULL,
    f32_data     INTEGER NOT NULL,
    exex_type    INTEGER NOT NULL,
    PRIMARY KEY (character_id, page_no, button_no),
    CONSTRAINT fk_shortcut_character_id FOREIGN KEY (character_id) REFERENCES ddon_character (character_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_communication_shortcut
(
    character_id INTEGER NOT NULL,
    page_no      INTEGER NOT NULL,
    button_no    INTEGER NOT NULL,
    type         INTEGER NOT NULL,
    category     INTEGER NOT NULL,
    id           INTEGER NOT NULL,
    PRIMARY KEY (character_id, page_no, button_no),
    CONSTRAINT fk_communication_shortcut_character_id FOREIGN KEY (character_id) REFERENCES ddon_character (character_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_pawn_reaction
(
    pawn_id       INTEGER NOT NULL,
    reaction_type INTEGER NOT NULL,
    motion_no     INTEGER NOT NULL,
    PRIMARY KEY (pawn_id, reaction_type),
    CONSTRAINT fk_pawn_reaction_pawn_id FOREIGN KEY (pawn_id) REFERENCES ddon_pawn (pawn_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_sp_skill
(
    pawn_id     INTEGER NOT NULL,
    sp_skill_id INTEGER NOT NULL,
    sp_skill_lv INTEGER NOT NULL,
    PRIMARY KEY (pawn_id),
    CONSTRAINT fk_sp_skill_pawn_id FOREIGN KEY (pawn_id) REFERENCES ddon_pawn (pawn_id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ddon_game_token
(
    account_id   INTEGER PRIMARY KEY         NOT NULL,
    character_id INTEGER                     NOT NULL,
    token        TEXT                        NOT NULL,
    created      TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT uq_game_token_token UNIQUE (token),
    CONSTRAINT fk_game_token_account_id FOREIGN KEY (account_id) REFERENCES account (id),
    CONSTRAINT fk_game_token_character_id FOREIGN KEY (character_id) REFERENCES ddon_character (character_id)
);

CREATE TABLE IF NOT EXISTS ddon_connection
(
    server_id  INTEGER                     NOT NULL,
    account_id INTEGER                     NOT NULL,
    type       INTEGER                     NOT NULL,
    created    TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT uq_connection_server_id_account_id UNIQUE (server_id, account_id),
    CONSTRAINT fk_game_token_account_id FOREIGN KEY (account_id) REFERENCES account (id)
);