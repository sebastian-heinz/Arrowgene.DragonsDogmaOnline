using FluentMigrator;
using FluentMigrator.Expressions;
using YamlDotNet.Core.Tokens;

namespace Arrowgene.Ddon.Database.Migrations
{
    [Migration(00000001)]
    public class DatabaseCreate : Migration
    {
        public override void Up()
        {
            Create.Table("setting")
                .WithColumn("key").AsString(32).NotNullable()
                    .PrimaryKey()
                .WithColumn("value").AsString().NotNullable();

            Create.Table("account")
                .WithColumn("id").AsInt32().NotNullable()
                    .Identity()
                    .PrimaryKey()
                .WithColumn("name").AsString().NotNullable()
                    .Unique()
                .WithColumn("normal_name").AsString().NotNullable()
                    .Unique()
                .WithColumn("hash").AsString().NotNullable()
                .WithColumn("mail").AsString().NotNullable()
                    .Unique()
                .WithColumn("mail_verified").AsBoolean().NotNullable()
                .WithColumn("mail_verified_at").AsDateTime().NotNullable()
                .WithColumn("mail_token").AsString().Nullable()
                .WithColumn("password_token").AsString().Nullable()
                .WithColumn("login_token").AsString().Nullable().Unique()
                .WithColumn("login_token_created").AsDateTime().Nullable()
                .WithColumn("state").AsInt32().NotNullable()
                .WithColumn("last_login").AsDateTime().Nullable()
                .WithColumn("created").AsDateTime().NotNullable();

            Create.Table("ddon_character_common")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .Identity()
                    .PrimaryKey()
                .WithColumn("job").AsInt16().NotNullable()
                .WithColumn("hide_equip_head").AsBoolean().NotNullable()
                .WithColumn("hide_equip_lantern").AsBoolean().NotNullable();

            Create.Table("ddon_character")
                .WithColumn("character_id").AsInt32().NotNullable()
                    .Identity()
                    .PrimaryKey()
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("account_id").AsInt32().NotNullable()
                    .ForeignKey("account", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("version").AsInt32().NotNullable()
                .WithColumn("first_name").AsString().NotNullable()
                .WithColumn("last_name").AsString().NotNullable()
                .WithColumn("created").AsDateTime().NotNullable()
                .WithColumn("my_pawn_slot_num").AsInt16().NotNullable()
                .WithColumn("rental_pawn_slot_num").AsInt16().NotNullable()
                .WithColumn("hide_equip_head_pawn").AsBoolean().NotNullable()
                .WithColumn("hide_equip_lantern_pawn").AsBoolean().NotNullable()
                .WithColumn("arisen_profile_share_range").AsInt16().NotNullable()
                .WithColumn("fav_warp_slot_num").AsInt32().NotNullable()
                .WithColumn("max_bazaar_exhibits").AsInt32().NotNullable();

            Create.Table("ddon_pawn")
                .WithColumn("pawn_id").AsInt32().NotNullable()
                    .Identity()
                    .PrimaryKey()
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("character_id").AsInt32().NotNullable()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("hm_type").AsInt16().NotNullable()
                .WithColumn("pawn_type").AsInt16().NotNullable()
                .WithColumn("training_points").AsInt32().NotNullable()
                .WithColumn("available_training").AsInt32().NotNullable();

            Create.Table("ddon_pawn_training_status")
                .WithColumn("pawn_id").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("job").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("training_status").AsBinary().NotNullable()
                    .ForeignKey("ddon_pawn", "pawn_id").OnDelete(System.Data.Rule.Cascade);

            Create.Table("ddon_edit_info")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("sex").AsInt16().NotNullable()
                .WithColumn("voice").AsInt16().NotNullable()
                .WithColumn("voice_pitch").AsInt16().NotNullable()
                .WithColumn("personality").AsInt16().NotNullable()
                .WithColumn("speech_freq").AsInt16().NotNullable()
                .WithColumn("body_type").AsInt16().NotNullable()
                .WithColumn("hair").AsInt16().NotNullable()
                .WithColumn("beard").AsInt16().NotNullable()
                .WithColumn("makeup").AsInt16().NotNullable()
                .WithColumn("scar").AsInt16().NotNullable()
                .WithColumn("eye_preset_no").AsInt16().NotNullable()
                .WithColumn("nose_preset_no").AsInt16().NotNullable()
                .WithColumn("mouth_preset_no").AsInt16().NotNullable()
                .WithColumn("eyebrow_tex_no").AsInt16().NotNullable()
                .WithColumn("color_skin").AsInt16().NotNullable()
                .WithColumn("color_hair").AsInt16().NotNullable()
                .WithColumn("color_beard").AsInt16().NotNullable()
                .WithColumn("color_eyebrow").AsInt16().NotNullable()
                .WithColumn("color_r_eye").AsInt16().NotNullable()
                .WithColumn("color_l_eye").AsInt16().NotNullable()
                .WithColumn("color_makeup").AsInt16().NotNullable()
                .WithColumn("sokutobu").AsInt16().NotNullable()
                .WithColumn("hitai").AsInt16().NotNullable()
                .WithColumn("mimi_jyouge").AsInt16().NotNullable()
                .WithColumn("kannkaku").AsInt16().NotNullable()
                .WithColumn("mabisasi_jyouge").AsInt16().NotNullable()
                .WithColumn("hanakuchi_jyouge").AsInt16().NotNullable()
                .WithColumn("ago_saki_haba").AsInt16().NotNullable()
                .WithColumn("ago_zengo").AsInt16().NotNullable()
                .WithColumn("ago_saki_jyouge").AsInt16().NotNullable()
                .WithColumn("hitomi_ookisa").AsInt16().NotNullable()
                .WithColumn("me_ookisa").AsInt16().NotNullable()
                .WithColumn("me_kaiten").AsInt16().NotNullable()
                .WithColumn("mayu_kaiten").AsInt16().NotNullable()
                .WithColumn("mimi_ookisa").AsInt16().NotNullable()
                .WithColumn("mimi_muki").AsInt16().NotNullable()
                .WithColumn("elf_mimi").AsInt16().NotNullable()
                .WithColumn("miken_takasa").AsInt16().NotNullable()
                .WithColumn("miken_haba").AsInt16().NotNullable()
                .WithColumn("hohobone_ryou").AsInt16().NotNullable()
                .WithColumn("hohobone_jyouge").AsInt16().NotNullable()
                .WithColumn("hohoniku").AsInt16().NotNullable()
                .WithColumn("erahone_jyouge").AsInt16().NotNullable()
                .WithColumn("erahone_haba").AsInt16().NotNullable()
                .WithColumn("hana_jyouge").AsInt16().NotNullable()
                .WithColumn("hana_haba").AsInt16().NotNullable()
                .WithColumn("hana_takasa").AsInt16().NotNullable()
                .WithColumn("hana_kakudo").AsInt16().NotNullable()
                .WithColumn("kuchi_haba").AsInt16().NotNullable()
                .WithColumn("kuchi_atsusa").AsInt16().NotNullable()
                .WithColumn("eyebrow_uv_offset_x").AsInt16().NotNullable()
                .WithColumn("eyebrow_uv_offset_y").AsInt16().NotNullable()
                .WithColumn("wrinkle").AsInt16().NotNullable()
                .WithColumn("wrinkle_albedo_blend_rate").AsInt16().NotNullable()
                .WithColumn("wrinkle_detail_normal_power").AsInt16().NotNullable()
                .WithColumn("muscle_albedo_blend_rate").AsInt16().NotNullable()
                .WithColumn("muscle_detail_normal_power").AsInt16().NotNullable()
                .WithColumn("height").AsInt16().NotNullable()
                .WithColumn("head_size").AsInt16().NotNullable()
                .WithColumn("neck_offset").AsInt16().NotNullable()
                .WithColumn("neck_scale").AsInt16().NotNullable()
                .WithColumn("upper_body_scale_x").AsInt16().NotNullable()
                .WithColumn("belly_size").AsInt16().NotNullable()
                .WithColumn("teat_scale").AsInt16().NotNullable()
                .WithColumn("tekubi_size").AsInt16().NotNullable()
                .WithColumn("koshi_offset").AsInt16().NotNullable()
                .WithColumn("koshi_size").AsInt16().NotNullable()
                .WithColumn("ankle_offset").AsInt16().NotNullable()
                .WithColumn("fat").AsInt16().NotNullable()
                .WithColumn("muscle").AsInt16().NotNullable()
                .WithColumn("motion_filter").AsInt16().NotNullable();

            Create.Table("ddon_status_info")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("revive_point").AsInt16().NotNullable()
                .WithColumn("hp").AsInt32().NotNullable()
                .WithColumn("white_hp").AsInt32().NotNullable();

            Create.Table("ddon_character_matching_profile")
                .WithColumn("character_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("entry_job").AsInt16().NotNullable()
                .WithColumn("entry_job_level").AsInt32().NotNullable()
                .WithColumn("current_job").AsInt16().NotNullable()
                .WithColumn("current_job_level").AsInt32().NotNullable()
                .WithColumn("objective_type1").AsInt32().NotNullable()
                .WithColumn("objective_type2").AsInt32().NotNullable()
                .WithColumn("play_style").AsInt32().NotNullable()
                .WithColumn("comment").AsString().NotNullable()
                .WithColumn("is_join_party").AsBoolean().NotNullable();

            Create.Table("ddon_character_arisen_profile")
                .WithColumn("character_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("background_id").AsInt16().NotNullable()
                .WithColumn("title_uid").AsInt32().NotNullable()
                .WithColumn("title_index").AsInt32().NotNullable()
                .WithColumn("motion_id").AsInt16().NotNullable()
                .WithColumn("motion_frame_no").AsInt32().NotNullable();

            Create.Table("ddon_character_job_data")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("job").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("exp").AsInt32().NotNullable()
                .WithColumn("job_point").AsInt32().NotNullable()
                .WithColumn("lv").AsInt32().NotNullable()
                .WithColumn("atk").AsInt16().NotNullable()
                .WithColumn("def").AsInt16().NotNullable()
                .WithColumn("m_atk").AsInt16().NotNullable()
                .WithColumn("m_def").AsInt16().NotNullable()
                .WithColumn("strength").AsInt16().NotNullable()
                .WithColumn("down_power").AsInt16().NotNullable()
                .WithColumn("shake_power").AsInt16().NotNullable()
                .WithColumn("stun_power").AsInt16().NotNullable()
                .WithColumn("consitution").AsInt16().NotNullable()
                .WithColumn("guts").AsInt16().NotNullable()
                .WithColumn("fire_resist").AsInt16().NotNullable()
                .WithColumn("ice_resist").AsInt16().NotNullable()
                .WithColumn("thunder_resist").AsInt16().NotNullable()
                .WithColumn("holy_resist").AsInt16().NotNullable()
                .WithColumn("dark_resist").AsInt16().NotNullable()
                .WithColumn("spread_resist").AsInt16().NotNullable()
                .WithColumn("freeze_resist").AsInt16().NotNullable()
                .WithColumn("shock_resist").AsInt16().NotNullable()
                .WithColumn("absorb_resist").AsInt16().NotNullable()
                .WithColumn("dark_elm_resist").AsInt16().NotNullable()
                .WithColumn("poison_resist").AsInt16().NotNullable()
                .WithColumn("slow_resist").AsInt16().NotNullable()
                .WithColumn("sleep_resist").AsInt16().NotNullable()
                .WithColumn("stun_resist").AsInt16().NotNullable()
                .WithColumn("wet_resist").AsInt16().NotNullable()
                .WithColumn("oil_resist").AsInt16().NotNullable()
                .WithColumn("seal_resist").AsInt16().NotNullable()
                .WithColumn("curse_resist").AsInt16().NotNullable()
                .WithColumn("soft_resist").AsInt16().NotNullable()
                .WithColumn("stone_resist").AsInt16().NotNullable()
                .WithColumn("gold_resist").AsInt16().NotNullable()
                .WithColumn("fire_reduce_resist").AsInt16().NotNullable()
                .WithColumn("ice_reduce_resist").AsInt16().NotNullable()
                .WithColumn("thunder_reduce_resist").AsInt16().NotNullable()
                .WithColumn("holy_reduce_resist").AsInt16().NotNullable()
                .WithColumn("dark_reduce_resist").AsInt16().NotNullable()
                .WithColumn("atk_down_resist").AsInt16().NotNullable()
                .WithColumn("def_down_resist").AsInt16().NotNullable()
                .WithColumn("m_atk_down_resist").AsInt16().NotNullable()
                .WithColumn("m_def_down_resist").AsInt16().NotNullable();

            Create.Table("ddon_storage")
                .WithColumn("character_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("storage_type").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("slot_max").AsInt16().NotNullable()
                .WithColumn("item_sort").AsBinary().NotNullable();

            Create.Table("ddon_wallet_point")
                .WithColumn("character_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("type").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("value").AsInt32().NotNullable();

            Create.Table("ddon_item")
                .WithColumn("uid").AsString(8).NotNullable()
                    .PrimaryKey()
                .WithColumn("item_id").AsInt32().NotNullable()
                .WithColumn("unk3").AsInt16().NotNullable()
                .WithColumn("color").AsInt16().NotNullable()
                .WithColumn("plus_value").AsInt16().NotNullable();

            Create.Table("ddon_storage_item")
                .WithColumn("item_uid").AsString(8).NotNullable()
                    .ForeignKey("ddon_item", "uid").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("character_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("storage_type").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("slot_no").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("item_num").AsInt32().NotNullable();

            Create.Table("ddon_equip_item")
                .WithColumn("item_uid").AsString(8).NotNullable()
                    .ForeignKey("ddon_item", "uid").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("job").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("equip_type").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("equip_slot").AsInt16().NotNullable()
                    .PrimaryKey();

            Create.Table("ddon_equip_job_item")
                .WithColumn("item_uid").AsString(8).NotNullable()
                    .ForeignKey("ddon_item", "uid").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("job").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("equip_slot").AsInt16().NotNullable()
                    .PrimaryKey();

            Create.Table("ddon_normal_skill_param")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("job").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("skill_no").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("index").AsInt32().NotNullable()
                .WithColumn("pre_skill_no").AsInt32();

            Create.Table("ddon_learned_custom_skill")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("job").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("skill_id").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("skill_lv").AsInt32().NotNullable();

            Create.Table("ddon_equipped_custom_skill")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("job").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("slot_no").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("skill_id").AsInt32().NotNullable();

            Create.Table("ddon_learned_ability")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("job").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("ability_id").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("ability_lv").AsInt16().NotNullable();

            Create.Table("ddon_equipped_ability")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("equipped_to_job").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("job").AsInt16().NotNullable()
                .WithColumn("slot_no").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("ability_id").AsInt32().NotNullable()
                    .PrimaryKey();

            Create.Table("ddon_shortcut")
                .WithColumn("character_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("page_no").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("button_no").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("shortcut_id").AsInt32().NotNullable()
                .WithColumn("u32_data").AsInt32().NotNullable()
                .WithColumn("f32_data").AsInt32().NotNullable()
                .WithColumn("exex_type").AsInt16().NotNullable();

            Create.Table("ddon_communication_shortcut")
                .WithColumn("character_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("page_no").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("button_no").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("type").AsInt16().NotNullable()
                .WithColumn("category").AsInt16().NotNullable()
                .WithColumn("id").AsInt32().NotNullable();

            Create.Table("ddon_pawn_reaction")
                .WithColumn("pawn_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_pawn", "pawn_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("reaction_type").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("motion_no").AsInt32().NotNullable();

            Create.Table("ddon_sp_skill")
                .WithColumn("pawn_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_pawn", "pawn_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("job").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("sp_skill_id").AsInt16().NotNullable()
                    .PrimaryKey()
                .WithColumn("sp_skill_lv").AsInt16().NotNullable();

            Create.Table("ddon_released_warp_point")
                .WithColumn("character_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("warp_point_id").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("favorite_slot_no").AsInt32().NotNullable();

            Create.Table("ddon_game_token")
                .WithColumn("account_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("account", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("character_id").AsInt32().NotNullable()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("token").AsString().NotNullable()
                    .Unique()
                .WithColumn("created").AsDateTime().NotNullable();

            Create.Table("ddon_connection")
                .WithColumn("server_id").AsInt32().NotNullable()
                    // .Unique()
                .WithColumn("account_id").AsInt32().NotNullable()
                    // .Unique()
                    .ForeignKey("account", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("type").AsInt32().NotNullable()
                .WithColumn("created").AsDateTime().NotNullable();

            Create.Table("ddon_dragon_force_augmentation")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("element_id").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("page_no").AsInt32().NotNullable()
                .WithColumn("group_no").AsInt32().NotNullable()
                .WithColumn("index_no").AsInt32().NotNullable();

            Create.Table("ddon_orb_gain_extend_param")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("hp_max").AsInt32().NotNullable()
                .WithColumn("stamina_max").AsInt32().NotNullable()
                .WithColumn("physical_attack").AsInt32().NotNullable()
                .WithColumn("physical_defence").AsInt32().NotNullable()
                .WithColumn("magic_attack").AsInt32().NotNullable()
                .WithColumn("magic_defence").AsInt32().NotNullable()
                .WithColumn("ability_cost").AsInt32().NotNullable()
                .WithColumn("jewelry_slot").AsInt32().NotNullable()
                .WithColumn("use_item_slot").AsInt32().NotNullable()
                .WithColumn("material_item_slot").AsInt32().NotNullable()
                .WithColumn("equip_item_slot").AsInt32().NotNullable()
                .WithColumn("main_pawn_slot").AsInt32().NotNullable()
                .WithColumn("support_pawn_slot").AsInt32().NotNullable();

            Create.Table("ddon_unlocked_secret_ability")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("ability_id").AsInt32().NotNullable()
                    .PrimaryKey();

            Create.Table("ddon_contact_list")
                .WithColumn("id").AsInt32().NotNullable()
                    .Identity()
                    .PrimaryKey()
                .WithColumn("requester_character_id").AsInt32().NotNullable()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("requested_character_id").AsInt32().NotNullable()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("status").AsInt16().NotNullable()
                .WithColumn("type").AsInt16().NotNullable()
                .WithColumn("requester_favorite").AsBoolean().NotNullable()
                .WithColumn("requested_favorite").AsBoolean().NotNullable();

            Create.Table("ddon_bazaar_exhibition")
                .WithColumn("bazaar_id").AsInt32().NotNullable()
                    .Identity()
                    .PrimaryKey()
                .WithColumn("character_id").AsInt32().NotNullable()
                    .ForeignKey("ddon_character", "character_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("sequence").AsInt32().NotNullable()
                .WithColumn("item_id").AsInt32().NotNullable()
                .WithColumn("num").AsInt32().NotNullable()
                .WithColumn("price").AsInt32().NotNullable()
                .WithColumn("exhibition_time").AsDateTime().NotNullable()
                .WithColumn("state").AsInt16().NotNullable()
                .WithColumn("proceeds").AsInt32().NotNullable()
                .WithColumn("expire").AsDateTime().NotNullable();

            Create.Table("ddon_reward_box")
                .WithColumn("uniq_reward_id").AsInt32().NotNullable()
                    .Identity()
                    .PrimaryKey()
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("quest_id").AsInt32().NotNullable()
                .WithColumn("num_random_rewards").AsInt32().NotNullable()
                .WithColumn("random_reward0_index").AsInt32().NotNullable()
                .WithColumn("random_reward1_index").AsInt32().NotNullable()
                .WithColumn("random_reward2_index").AsInt32().NotNullable()
                .WithColumn("random_reward3_index").AsInt32().NotNullable();

            Create.Table("ddon_quest_progress")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("quest_id").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("quest_type").AsInt32().NotNullable()
                .WithColumn("step").AsInt32().NotNullable();

            Create.Table("ddon_completed_quests")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("quest_id").AsInt32().NotNullable()
                    .PrimaryKey()
                .WithColumn("quest_type").AsInt32().NotNullable()
                .WithColumn("clear_count").AsInt32().NotNullable();

            Create.Table("ddon_priority_quests")
                .WithColumn("character_common_id").AsInt32().NotNullable()
                    .PrimaryKey()
                    .ForeignKey("ddon_character_common", "character_common_id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("quest_id").AsInt32().NotNullable()
                    .PrimaryKey();
        }

        public override void Down()
        {
            Delete.Table("setting");
            Delete.Table("account");
            Delete.Table("ddon_character_common");
            Delete.Table("ddon_character");
            Delete.Table("ddon_pawn");
            Delete.Table("ddon_pawn_training_status");
            Delete.Table("ddon_edit_info");
            Delete.Table("ddon_status_info");
            Delete.Table("ddon_character_matching_profile");
            Delete.Table("ddon_character_arisen_profile");
            Delete.Table("ddon_character_job_data");
            Delete.Table("ddon_storage");
            Delete.Table("ddon_wallet_point");
            Delete.Table("ddon_item");
            Delete.Table("ddon_storage_item");
            Delete.Table("ddon_equip_item");
            Delete.Table("ddon_equip_job_item");
            Delete.Table("ddon_normal_skill_param");
            Delete.Table("ddon_learned_custom_skill");
            Delete.Table("ddon_equipped_custom_skill");
            Delete.Table("ddon_learned_ability");
            Delete.Table("ddon_equipped_ability");
            Delete.Table("ddon_shortcut");
            Delete.Table("ddon_communication_shortcut");
            Delete.Table("ddon_pawn_reaction");
            Delete.Table("ddon_sp_skill");
            Delete.Table("ddon_released_warp_point");
            Delete.Table("ddon_game_token");
            Delete.Table("ddon_connection");
            Delete.Table("ddon_dragon_force_augmentation");
            Delete.Table("ddon_orb_gain_extend_param");
            Delete.Table("ddon_unlocked_secret_ability");
            Delete.Table("ddon_contact_list");
            Delete.Table("ddon_bazaar_exhibition");
            Delete.Table("ddon_reward_box");
            Delete.Table("ddon_quest_progress");
            Delete.Table("ddon_completed_quests");
            Delete.Table("ddon_priority_quests");
        }
    }
}
