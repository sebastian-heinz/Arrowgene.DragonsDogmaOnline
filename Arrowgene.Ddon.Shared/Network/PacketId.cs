using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Network
{
    public readonly struct PacketId
    {
        private static void AddPacketIdEntry(Dictionary<int, PacketId> packetIds, PacketId packetId)
        {
            packetIds.Add(packetId.GetHashCode(), packetId);
        }

        private static int GetHashCode(byte groupId, ushort handlerId, byte handlerSubId)
        {
            return HashCode.Combine(groupId, handlerId, handlerSubId);
        }

        public static PacketId GetPacketId(ServerType serverType, byte groupId, ushort handlerId, byte handlerSubId)
        {
            switch (serverType)
            {
                case ServerType.Game: return GetGamePacketId(groupId, handlerId, handlerSubId);
                case ServerType.Login: return GetLoginPacketId(groupId, handlerId, handlerSubId);
            }

            return new PacketId(groupId, handlerId, handlerSubId, "Unknown");
        }

        public static PacketId GetLoginPacketId(byte groupId, ushort handlerId, byte handlerSubId)
        {
            int hashCode = GetHashCode(groupId, handlerId, handlerSubId);
            if (LoginPacketIds.ContainsKey(hashCode))
            {
                return LoginPacketIds[hashCode];
            }

            return new PacketId(groupId, handlerId, handlerSubId, "Unknown");
        }

        public static PacketId GetGamePacketId(byte groupId, ushort handlerId, byte handlerSubId)
        {
            int hashCode = GetHashCode(groupId, handlerId, handlerSubId);
            if (GamePacketIds.ContainsKey(hashCode))
            {
                return GamePacketIds[hashCode];
            }

            return new PacketId(groupId, handlerId, handlerSubId, "Unknown");
        }

        public readonly byte GroupId;
        public readonly ushort HandlerId;
        public readonly byte HandlerSubId;
        public readonly string Name;
        public readonly string OriginalName;

        public PacketId(byte groupId, ushort handlerId, byte handlerSubId, string name, string originalName = null)
        {
            GroupId = groupId;
            HandlerId = handlerId;
            HandlerSubId = handlerSubId;
            Name = name;
            if (originalName != null)
            {
                OriginalName = originalName;
            }
            else
            {
                OriginalName = name;
            }
        }

        public bool Equals(PacketId other)
        {
            return GroupId == other.GroupId && HandlerId == other.HandlerId && HandlerSubId == other.HandlerSubId;
        }

        public override bool Equals(object obj)
        {
            return obj is PacketId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return GetHashCode(GroupId, HandlerId, HandlerSubId);
        }
        
        public static bool operator ==(PacketId a, PacketId b) 
        {
            return a.Equals(b);
        }

        public static bool operator !=(PacketId a, PacketId b) 
        {
            return !a.Equals(b);
        }

        public static readonly PacketId UNKNOWN = new PacketId(0, 0, 0, "UNKNOWN");

        #region LoginPacketIds

        public static readonly PacketId C2L_PING_REQ = new PacketId(0, 0, 1, "C2L_PING_REQ");
        public static readonly PacketId L2C_PING_RES = new PacketId(0, 0, 2, "L2C_PING_RES");
        public static readonly PacketId C2L_LOGIN_REQ = new PacketId(0, 1, 1, "C2L_LOGIN_REQ");
        public static readonly PacketId L2C_LOGIN_RES = new PacketId(0, 1, 2, "L2C_LOGIN_RES");
        public static readonly PacketId C2L_LOGOUT_REQ = new PacketId(0, 2, 1, "C2L_LOGOUT_REQ");
        public static readonly PacketId L2C_LOGOUT_RES = new PacketId(0, 2, 2, "L2C_LOGOUT_RES");
        public static readonly PacketId L2C_EJECTION_NTC = new PacketId(0, 3, 16, "L2C_EJECTION_NTC");

        public static readonly PacketId C2L_CLIENT_CHALLENGE_REQ = new PacketId(1, 0, 1, "C2L_CLIENT_CHALLENGE_REQ");
        public static readonly PacketId L2C_CLIENT_CHALLENGE_RES = new PacketId(1, 0, 2, "L2C_CLIENT_CHALLENGE_RES");
        public static readonly PacketId L2C_LOGIN_SERVER_CERT_NOTICE = new PacketId(1, 1, 16, "L2C_LOGIN_SERVER_CERT_NOTICE");

        public static readonly PacketId C2L_GET_GAME_SERVER_LIST_RES = new PacketId(2, 0, 1, "C2L_GET_GAME_SERVER_LIST_RES");
        public static readonly PacketId L2C_GET_GAME_SERVER_LIST_RES = new PacketId(2, 0, 2, "L2C_GET_GAME_SERVER_LIST_RES");
        public static readonly PacketId C2L_GET_GAME_SESSION_KEY_REQ = new PacketId(2, 1, 1, "C2L_GET_GAME_SESSION_KEY_REQ");
        public static readonly PacketId L2C_GET_GAME_SESSION_KEY_RES = new PacketId(2, 1, 2, "L2C_GET_GAME_SESSION_KEY_RES");
        public static readonly PacketId C2L_GET_LOGIN_SETTING_REQ = new PacketId(2, 2, 1, "C2L_GET_LOGIN_SETTING_REQ");
        public static readonly PacketId L2C_GET_LOGIN_SETTING_RES = new PacketId(2, 2, 2, "L2C_GET_LOGIN_SETTING_RES");
        public static readonly PacketId L2C_NEXT_CONNECT_SERVER_NTC = new PacketId(2, 3, 16, "L2C_NEXT_CONNECT_SERVER_NTC");

        public static readonly PacketId C2L_GET_ERROR_MESSAGE_LIST_REQ = new PacketId(3, 0, 1, "C2L_GET_ERROR_MESSAGE_LIST_REQ");
        public static readonly PacketId L2C_GET_ERROR_MESSAGE_LIST_RES = new PacketId(3, 0, 2, "L2C_GET_ERROR_MESSAGE_LIST_RES");
        public static readonly PacketId L2C_GET_ERROR_MESSAGE_LIST_NTC = new PacketId(3, 0, 16, "L2C_GET_ERROR_MESSAGE_LIST_NTC");

        public static readonly PacketId C2L_GP_COURSE_GET_INFO_REQ = new PacketId(4, 0, 1, "C2L_GP_COURSE_GET_INFO_REQ");
        public static readonly PacketId L2C_GP_COURSE_GET_INFO_RES = new PacketId(4, 0, 2, "L2C_GP_COURSE_GET_INFO_RES");

        public static readonly PacketId C2L_GET_CHARACTER_LIST_REQ = new PacketId(5, 0, 1, "C2L_GET_CHARACTER_LIST_REQ");
        public static readonly PacketId L2C_GET_CHARACTER_LIST_RES = new PacketId(5, 0, 2, "L2C_GET_CHARACTER_LIST_RES");
        public static readonly PacketId C2L_DECIDE_CHARACTER_ID_REQ = new PacketId(5, 1, 1, "C2L_DECIDE_CHARACTER_ID_REQ");
        public static readonly PacketId L2C_DECIDE_CHARACTER_ID_RES = new PacketId(5, 1, 2, "L2C_DECIDE_CHARACTER_ID_RES");
        public static readonly PacketId C2L_DECIDE_CANCEL_CHARACTER_REQ = new PacketId(5, 2, 1, "C2L_DECIDE_CANCEL_CHARACTER_REQ");
        public static readonly PacketId L2C_DECIDE_CANCEL_CHARACTER_RES = new PacketId(5, 2, 2, "L2C_DECIDE_CANCEL_CHARACTER_RES");
        public static readonly PacketId C2L_CREATE_CHARACTER_DATA_REQ = new PacketId(5, 3, 1, "C2L_CREATE_CHARACTER_DATA_REQ");
        public static readonly PacketId L2C_CREATE_CHARACTER_DATA_RES = new PacketId(5, 3, 2, "L2C_CREATE_CHARACTER_DATA_RES");
        public static readonly PacketId L2C_CREATE_CHARACTER_DATA_NTC = new PacketId(5, 3, 16, "L2C_CREATE_CHARACTER_DATA_NTC");
        public static readonly PacketId C2L_DELETE_CHARACTER_INFO_REQ = new PacketId(5, 4, 1, "C2L_DELETE_CHARACTER_INFO_REQ");
        public static readonly PacketId L2C_DELETE_CHARACTER_INFO_RES = new PacketId(5, 4, 2, "L2C_DELETE_CHARACTER_INFO_RES");
        public static readonly PacketId L2C_LOGIN_WAIT_NUM_NTC = new PacketId(5, 5, 16, "L2C_LOGIN_WAIT_NUM_NTC");


        #endregion

        #region LoginPacketIdsInit

        private static Dictionary<int, PacketId> InitializeLoginPacketIds()
        {
            Dictionary<int, PacketId> packetIds = new Dictionary<int, PacketId>();
            AddPacketIdEntry(packetIds, C2L_PING_REQ);
            AddPacketIdEntry(packetIds, L2C_PING_RES);
            AddPacketIdEntry(packetIds, C2L_LOGIN_REQ);
            AddPacketIdEntry(packetIds, L2C_LOGIN_RES);
            AddPacketIdEntry(packetIds, C2L_LOGOUT_REQ);
            AddPacketIdEntry(packetIds, L2C_LOGOUT_RES);
            AddPacketIdEntry(packetIds, L2C_EJECTION_NTC);
            AddPacketIdEntry(packetIds, C2L_CLIENT_CHALLENGE_REQ);
            AddPacketIdEntry(packetIds, L2C_CLIENT_CHALLENGE_RES);
            AddPacketIdEntry(packetIds, L2C_LOGIN_SERVER_CERT_NOTICE);
            AddPacketIdEntry(packetIds, C2L_GET_GAME_SERVER_LIST_RES);
            AddPacketIdEntry(packetIds, L2C_GET_GAME_SERVER_LIST_RES);
            AddPacketIdEntry(packetIds, C2L_GET_GAME_SESSION_KEY_REQ);
            AddPacketIdEntry(packetIds, L2C_GET_GAME_SESSION_KEY_RES);
            AddPacketIdEntry(packetIds, C2L_GET_LOGIN_SETTING_REQ);
            AddPacketIdEntry(packetIds, L2C_GET_LOGIN_SETTING_RES);
            AddPacketIdEntry(packetIds, L2C_NEXT_CONNECT_SERVER_NTC);
            AddPacketIdEntry(packetIds, C2L_GET_ERROR_MESSAGE_LIST_REQ);
            AddPacketIdEntry(packetIds, L2C_GET_ERROR_MESSAGE_LIST_RES);
            AddPacketIdEntry(packetIds, L2C_GET_ERROR_MESSAGE_LIST_NTC);
            AddPacketIdEntry(packetIds, C2L_GP_COURSE_GET_INFO_REQ);
            AddPacketIdEntry(packetIds, L2C_GP_COURSE_GET_INFO_RES);
            AddPacketIdEntry(packetIds, C2L_GET_CHARACTER_LIST_REQ);
            AddPacketIdEntry(packetIds, L2C_GET_CHARACTER_LIST_RES);
            AddPacketIdEntry(packetIds, C2L_DECIDE_CHARACTER_ID_REQ);
            AddPacketIdEntry(packetIds, L2C_DECIDE_CHARACTER_ID_RES);
            AddPacketIdEntry(packetIds, C2L_DECIDE_CANCEL_CHARACTER_REQ);
            AddPacketIdEntry(packetIds, L2C_DECIDE_CANCEL_CHARACTER_RES);
            AddPacketIdEntry(packetIds, C2L_CREATE_CHARACTER_DATA_REQ);
            AddPacketIdEntry(packetIds, L2C_CREATE_CHARACTER_DATA_RES);
            AddPacketIdEntry(packetIds, L2C_CREATE_CHARACTER_DATA_NTC);
            AddPacketIdEntry(packetIds, C2L_DELETE_CHARACTER_INFO_REQ);
            AddPacketIdEntry(packetIds, L2C_DELETE_CHARACTER_INFO_RES);
            AddPacketIdEntry(packetIds, L2C_LOGIN_WAIT_NUM_NTC);
            return packetIds;
        }

        #endregion

        #region GamePacketIds

// Group: 0 - (CONNECTION)
        public static readonly PacketId C2S_CONNECTION_PING_REQ = new PacketId(0, 0, 1, "C2S_CONNECTION_PING_REQ");
        public static readonly PacketId S2C_CONNECTION_PING_RES = new PacketId(0, 0, 2, "S2C_CONNECTION_PING_RES"); // PING に
        public static readonly PacketId C2S_CONNECTION_LOGIN_REQ = new PacketId(0, 1, 1, "C2S_CONNECTION_LOGIN_REQ");
        public static readonly PacketId S2C_CONNECTION_LOGIN_RES = new PacketId(0, 1, 2, "S2C_CONNECTION_LOGIN_RES"); // ログインに
        public static readonly PacketId C2S_CONNECTION_LOGOUT_REQ = new PacketId(0, 2, 1, "C2S_CONNECTION_LOGOUT_REQ");
        public static readonly PacketId S2C_CONNECTION_LOGOUT_RES = new PacketId(0, 2, 2, "S2C_CONNECTION_LOGOUT_RES"); // ログアウトに
        public static readonly PacketId C2S_CONNECTION_GET_LOGIN_ANNOUNCEMENT_REQ = new PacketId(0, 3, 1, "C2S_CONNECTION_GET_LOGIN_ANNOUNCEMENT_REQ");
        public static readonly PacketId S2C_CONNECTION_GET_LOGIN_ANNOUNCEMENT_RES = new PacketId(0, 3, 2, "S2C_CONNECTION_GET_LOGIN_ANNOUNCEMENT_RES"); // ログインお知らせメッセージの取得に
        public static readonly PacketId S2C_CONNECTION_0_4_16_NTC = new PacketId(0, 4, 16, "S2C_CONNECTION_0_4_16_NTC");
        public static readonly PacketId C2S_CONNECTION_MOVE_IN_SERVER_REQ = new PacketId(0, 5, 1, "C2S_CONNECTION_MOVE_IN_SERVER_REQ");
        public static readonly PacketId S2C_CONNECTION_MOVE_IN_SERVER_RES = new PacketId(0, 5, 2, "S2C_CONNECTION_MOVE_IN_SERVER_RES"); // サーバー移動（入る）に
        public static readonly PacketId C2S_CONNECTION_MOVE_OUT_SERVER_REQ = new PacketId(0, 6, 1, "C2S_CONNECTION_MOVE_OUT_SERVER_REQ");
        public static readonly PacketId S2C_CONNECTION_MOVE_OUT_SERVER_RES = new PacketId(0, 6, 2, "S2C_CONNECTION_MOVE_OUT_SERVER_RES"); // サーバー移動（出る）に
        public static readonly PacketId C2S_CONNECTION_RESERVE_SERVER_REQ = new PacketId(0, 7, 1, "C2S_CONNECTION_RESERVE_SERVER_REQ");
        public static readonly PacketId S2C_CONNECTION_RESERVE_SERVER_RES = new PacketId(0, 7, 2, "S2C_CONNECTION_RESERVE_SERVER_RES"); // 他サーバーのロビーの部屋予約に
        public static readonly PacketId S2C_CONNECTION_0_9_16_NTC = new PacketId(0, 9, 16, "S2C_CONNECTION_0_9_16_NTC");
        public static readonly PacketId S2C_CONNECTION_0_10_16_NTC = new PacketId(0, 10, 16, "S2C_CONNECTION_0_10_16_NTC");
        public static readonly PacketId S2C_CONNECTION_0_11_16_NTC = new PacketId(0, 11, 16, "S2C_CONNECTION_0_11_16_NTC");
        public static readonly PacketId S2C_CONNECTION_0_12_16_NTC = new PacketId(0, 12, 16, "S2C_CONNECTION_0_12_16_NTC");

// Group: 1 - (SERVER)
        public static readonly PacketId C2S_SERVER_GET_SERVER_LIST_REQ = new PacketId(1, 0, 1, "C2S_SERVER_GET_SERVER_LIST_REQ");
        public static readonly PacketId S2C_SERVER_GET_SERVER_LIST_RES = new PacketId(1, 0, 2, "S2C_SERVER_GET_SERVER_LIST_RES"); // サーバーリスト取得に
        public static readonly PacketId C2S_SERVER_GET_GAME_SETTING_REQ = new PacketId(1, 1, 1, "C2S_SERVER_GET_GAME_SETTING_REQ");
        public static readonly PacketId S2C_SERVER_GET_GAME_SETTING_RES = new PacketId(1, 1, 2, "S2C_SERVER_GET_GAME_SETTING_RES"); // ゲーム設定の取得に
        public static readonly PacketId C2S_SERVER_GET_WORLD_INFO_REQ = new PacketId(1, 2, 1, "C2S_SERVER_GET_WORLD_INFO_REQ");
        public static readonly PacketId S2C_SERVER_GET_WORLD_INFO_RES = new PacketId(1, 2, 2, "S2C_SERVER_GET_WORLD_INFO_RES"); // ワールド情報の取得に
        public static readonly PacketId C2S_SERVER_GET_REAL_TIME_REQ = new PacketId(1, 4, 1, "C2S_SERVER_GET_REAL_TIME_REQ");
        public static readonly PacketId S2C_SERVER_GET_REAL_TIME_RES = new PacketId(1, 4, 2, "S2C_SERVER_GET_REAL_TIME_RES"); // サーバー時間の取得に
        public static readonly PacketId C2S_SERVER_REPORT_SITE_GET_ADDRESS_REQ = new PacketId(1, 5, 1, "C2S_SERVER_REPORT_SITE_GET_ADDRESS_REQ");
        public static readonly PacketId S2C_SERVER_REPORT_SITE_GET_ADDRESS_RES = new PacketId(1, 5, 2, "S2C_SERVER_REPORT_SITE_GET_ADDRESS_RES"); // 報告サイトURL取得に
        public static readonly PacketId C2S_SERVER_1_6_1_REQ = new PacketId(1, 6, 1, "C2S_SERVER_1_6_1_REQ");
        public static readonly PacketId S2C_SERVER_1_6_2_RES = new PacketId(1, 6, 2, "S2C_SERVER_1_6_2_RES");
        public static readonly PacketId C2S_SERVER_WEATHER_FORECAST_GET_REQ = new PacketId(1, 7, 1, "C2S_SERVER_WEATHER_FORECAST_GET_REQ");
        public static readonly PacketId S2C_SERVER_WEATHER_FORECAST_GET_RES = new PacketId(1, 7, 2, "S2C_SERVER_WEATHER_FORECAST_GET_RES"); // 天気予報取得に
        public static readonly PacketId C2S_SERVER_GAME_TIME_GET_BASEINFO_REQ = new PacketId(1, 8, 1, "C2S_SERVER_GAME_TIME_GET_BASEINFO_REQ");
        public static readonly PacketId S2C_SERVER_GAME_TIME_GET_BASEINFO_RES = new PacketId(1, 8, 2, "S2C_SERVER_GAME_TIME_GET_BASEINFO_RES"); // ゲーム内時間、天候、月齢の基本情報取得の結果
        public static readonly PacketId C2S_SERVER_GET_SCREEN_SHOT_CATEGORY_REQ = new PacketId(1, 9, 1, "C2S_SERVER_GET_SCREEN_SHOT_CATEGORY_REQ");
        public static readonly PacketId S2C_SERVER_GET_SCREEN_SHOT_CATEGORY_RES = new PacketId(1, 9, 2, "S2C_SERVER_GET_SCREEN_SHOT_CATEGORY_RES"); // スクリーンショットカテゴリの取得
        public static readonly PacketId S2C_SERVER_1_10_16_NTC = new PacketId(1, 10, 16, "S2C_SERVER_1_10_16_NTC");
        public static readonly PacketId S2C_SERVER_1_11_16_NTC = new PacketId(1, 11, 16, "S2C_SERVER_1_11_16_NTC");
        public static readonly PacketId S2C_SERVER_1_12_16_NTC = new PacketId(1, 12, 16, "S2C_SERVER_1_12_16_NTC");
        public static readonly PacketId S2C_SERVER_1_13_16_NTC = new PacketId(1, 13, 16, "S2C_SERVER_1_13_16_NTC");
        public static readonly PacketId S2C_SERVER_1_14_16_NTC = new PacketId(1, 14, 16, "S2C_SERVER_1_14_16_NTC");

// Group: 2 - (CHARACTER)
        public static readonly PacketId C2S_CHARACTER_DECIDE_CHARACTER_ID_REQ = new PacketId(2, 0, 1, "C2S_CHARACTER_DECIDE_CHARACTER_ID_REQ");
        public static readonly PacketId S2C_CHARACTER_DECIDE_CHARACTER_ID_RES = new PacketId(2, 0, 2, "S2C_CHARACTER_DECIDE_CHARACTER_ID_RES"); // キャラクター確定に
        public static readonly PacketId S2C_CHARACTER_2_1_1 = new PacketId(2, 1, 1, "S2C_CHARACTER_2_1_1");
        public static readonly PacketId C2S_CHARACTER_CHARACTER_SEARCH_REQ = new PacketId(2, 2, 1, "C2S_CHARACTER_CHARACTER_SEARCH_REQ");
        public static readonly PacketId S2C_CHARACTER_CHARACTER_SEARCH_RES = new PacketId(2, 2, 2, "S2C_CHARACTER_CHARACTER_SEARCH_RES"); // キャラクター検索に
        public static readonly PacketId S2C_CHARACTER_2_3_16_NTC = new PacketId(2, 3, 16, "S2C_CHARACTER_2_3_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_4_16_NTC = new PacketId(2, 4, 16, "S2C_CHARACTER_2_4_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_5_16_NTC = new PacketId(2, 5, 16, "S2C_CHARACTER_2_5_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_6_16_NTC = new PacketId(2, 6, 16, "S2C_CHARACTER_2_6_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_7_16_NTC = new PacketId(2, 7, 16, "S2C_CHARACTER_2_7_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_8_16_NTC = new PacketId(2, 8, 16, "S2C_CHARACTER_2_8_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_9_16_NTC = new PacketId(2, 9, 16, "S2C_CHARACTER_2_9_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_10_16_NTC = new PacketId(2, 10, 16, "S2C_CHARACTER_2_10_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_11_16_NTC = new PacketId(2, 11, 16, "S2C_CHARACTER_2_11_16_NTC");
        public static readonly PacketId C2S_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_REQ = new PacketId(2, 12, 1, "C2S_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_REQ");
        public static readonly PacketId S2C_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_RES = new PacketId(2, 12, 2, "S2C_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_RES"); // コミュニティキャラクターステータス取得要求に
        public static readonly PacketId C2S_CHARACTER_CHARACTER_POINT_REVIVE_REQ = new PacketId(2, 16, 1, "C2S_CHARACTER_CHARACTER_POINT_REVIVE_REQ");
        public static readonly PacketId S2C_CHARACTER_CHARACTER_POINT_REVIVE_RES = new PacketId(2, 16, 2, "S2C_CHARACTER_CHARACTER_POINT_REVIVE_RES"); // キャラクタ復活(復活力消費)に
        public static readonly PacketId C2S_CHARACTER_CHARACTER_GOLDEN_REVIVE_REQ = new PacketId(2, 17, 1, "C2S_CHARACTER_CHARACTER_GOLDEN_REVIVE_REQ");
        public static readonly PacketId S2C_CHARACTER_CHARACTER_GOLDEN_REVIVE_RES = new PacketId(2, 17, 2, "S2C_CHARACTER_CHARACTER_GOLDEN_REVIVE_RES"); // キャラクタ復活(黄金石消費)に
        public static readonly PacketId C2S_CHARACTER_CHARACTER_PENALTY_REVIVE_REQ = new PacketId(2, 18, 1, "C2S_CHARACTER_CHARACTER_PENALTY_REVIVE_REQ");
        public static readonly PacketId S2C_CHARACTER_CHARACTER_PENALTY_REVIVE_RES = new PacketId(2, 18, 2, "S2C_CHARACTER_CHARACTER_PENALTY_REVIVE_RES"); // キャラクタ復活(デスペナ付与)に
        public static readonly PacketId C2S_CHARACTER_PAWN_POINT_REVIVE_REQ = new PacketId(2, 22, 1, "C2S_CHARACTER_PAWN_POINT_REVIVE_REQ");
        public static readonly PacketId S2C_CHARACTER_PAWN_POINT_REVIVE_RES = new PacketId(2, 22, 2, "S2C_CHARACTER_PAWN_POINT_REVIVE_RES"); // ポーン復活(復活力消費)に
        public static readonly PacketId C2S_CHARACTER_PAWN_GOLDEN_REVIVE_REQ = new PacketId(2, 23, 1, "C2S_CHARACTER_PAWN_GOLDEN_REVIVE_REQ");
        public static readonly PacketId S2C_CHARACTER_PAWN_GOLDEN_REVIVE_RES = new PacketId(2, 23, 2, "S2C_CHARACTER_PAWN_GOLDEN_REVIVE_RES"); // ポーン復活(黄金石消費)に
        public static readonly PacketId C2S_CHARACTER_GET_REVIVE_CHARGEABLE_TIME_REQ = new PacketId(2, 24, 1, "C2S_CHARACTER_GET_REVIVE_CHARGEABLE_TIME_REQ");
        public static readonly PacketId S2C_CHARACTER_GET_REVIVE_CHARGEABLE_TIME_RES = new PacketId(2, 24, 2, "S2C_CHARACTER_GET_REVIVE_CHARGEABLE_TIME_RES"); // 復活力チャージ可能時間取得に
        public static readonly PacketId C2S_CHARACTER_CHARGE_REVIVE_POINT_REQ = new PacketId(2, 25, 1, "C2S_CHARACTER_CHARGE_REVIVE_POINT_REQ");
        public static readonly PacketId S2C_CHARACTER_CHARGE_REVIVE_POINT_RES = new PacketId(2, 25, 2, "S2C_CHARACTER_CHARGE_REVIVE_POINT_RES"); // 復活力チャージに
        public static readonly PacketId C2S_CHARACTER_GET_REVIVE_POINT_REQ = new PacketId(2, 26, 1, "C2S_CHARACTER_GET_REVIVE_POINT_REQ");
        public static readonly PacketId S2C_CHARACTER_GET_REVIVE_POINT_RES = new PacketId(2, 26, 2, "S2C_CHARACTER_GET_REVIVE_POINT_RES"); // 復活力取得に
        public static readonly PacketId S2C_CHARACTER_2_27_16_NTC = new PacketId(2, 27, 16, "S2C_CHARACTER_2_27_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_28_16_NTC = new PacketId(2, 28, 16, "S2C_CHARACTER_2_28_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_29_16_NTC = new PacketId(2, 29, 16, "S2C_CHARACTER_2_29_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_30_16_NTC = new PacketId(2, 30, 16, "S2C_CHARACTER_2_30_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_31_16_NTC = new PacketId(2, 31, 16, "S2C_CHARACTER_2_31_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_32_16_NTC = new PacketId(2, 32, 16, "S2C_CHARACTER_2_32_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_33_16_NTC = new PacketId(2, 33, 16, "S2C_CHARACTER_2_33_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_34_16_NTC = new PacketId(2, 34, 16, "S2C_CHARACTER_2_34_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_35_16_NTC = new PacketId(2, 35, 16, "S2C_CHARACTER_2_35_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_36_16_NTC = new PacketId(2, 36, 16, "S2C_CHARACTER_2_36_16_NTC");
        public static readonly PacketId S2C_CHARACTER_2_37_16_NTC = new PacketId(2, 37, 16, "S2C_CHARACTER_2_37_16_NTC");
        public static readonly PacketId C2S_CHARACTER_SET_ONLINE_STATUS_REQ = new PacketId(2, 38, 1, "C2S_CHARACTER_SET_ONLINE_STATUS_REQ");
        public static readonly PacketId S2C_CHARACTER_SET_ONLINE_STATUS_RES = new PacketId(2, 38, 2, "S2C_CHARACTER_SET_ONLINE_STATUS_RES"); // オンラインステータス設定に
        public static readonly PacketId C2S_CHARACTER_SWITCH_GAME_MODE_REQ = new PacketId(2, 39, 1, "C2S_CHARACTER_SWITCH_GAME_MODE_REQ");
        public static readonly PacketId S2C_CHARACTER_SWITCH_GAME_MODE_RES = new PacketId(2, 39, 2, "S2C_CHARACTER_SWITCH_GAME_MODE_RES"); // ゲームモードのスイッチ要求
        public static readonly PacketId S2C_CHARACTER_2_39_16_NTC = new PacketId(2, 39, 16, "S2C_CHARACTER_2_39_16_NTC");
        public static readonly PacketId C2S_CHARACTER_CREATE_MODE_CHARACTER_EDIT_PARAM_REQ = new PacketId(2, 40, 1, "C2S_CHARACTER_CREATE_MODE_CHARACTER_EDIT_PARAM_REQ");
        public static readonly PacketId S2C_CHARACTER_CREATE_MODE_CHARACTER_EDIT_PARAM_RES = new PacketId(2, 40, 2, "S2C_CHARACTER_CREATE_MODE_CHARACTER_EDIT_PARAM_RES"); // モードキャラ作成

// Group: 3 - (LOBBY)
        public static readonly PacketId C2S_LOBBY_LOBBY_JOIN_REQ = new PacketId(3, 0, 1, "C2S_LOBBY_LOBBY_JOIN_REQ");
        public static readonly PacketId S2C_LOBBY_LOBBY_JOIN_RES = new PacketId(3, 0, 2, "S2C_LOBBY_LOBBY_JOIN_RES"); // ロビーの入室に
        public static readonly PacketId C2S_LOBBY_LOBBY_LEAVE_REQ = new PacketId(3, 1, 1, "C2S_LOBBY_LOBBY_LEAVE_REQ");
        public static readonly PacketId S2C_LOBBY_LOBBY_LEAVE_RES = new PacketId(3, 1, 2, "S2C_LOBBY_LOBBY_LEAVE_RES"); // ロビーの退室に
        public static readonly PacketId C2S_LOBBY_LOBBY_CHAT_MSG_REQ = new PacketId(3, 2, 1, "C2S_LOBBY_LOBBY_CHAT_MSG_REQ");
        public static readonly PacketId S2C_LOBBY_LOBBY_CHAT_MSG_RES = new PacketId(3, 2, 2, "S2C_LOBBY_LOBBY_CHAT_MSG_RES");
        public static readonly PacketId S2C_LOBBY_LOBBY_CHAT_MSG_NTC = new PacketId(3, 2, 16, "S2C_LOBBY_LOBBY_CHAT_MSG_NTC", "S2C_LOBBY_3_2_16_NTC");
        public static readonly PacketId S2C_LOBBY_3_4_16_NTC = new PacketId(3, 4, 16, "S2C_LOBBY_3_4_16_NTC");

// Group: 4 - (CHAT)
        public static readonly PacketId C2S_CHAT_SEND_TELL_MSG_REQ = new PacketId(4, 0, 1, "C2S_CHAT_SEND_TELL_MSG_REQ");
        public static readonly PacketId S2C_CHAT_SEND_TELL_MSG_RES = new PacketId(4, 0, 2, "S2C_CHAT_SEND_TELL_MSG_RES"); // 個人チャット発言に
        public static readonly PacketId S2C_CHAT_4_0_16_NTC = new PacketId(4, 0, 16, "S2C_CHAT_4_0_16_NTC");

// Group: 5 - (USER)
        public static readonly PacketId C2S_USER_LIST_GET_USER_LIST_REQ = new PacketId(5, 0, 1, "C2S_USER_LIST_GET_USER_LIST_REQ");
        public static readonly PacketId S2C_USER_LIST_GET_USER_LIST_RES = new PacketId(5, 0, 2, "S2C_USER_LIST_GET_USER_LIST_RES"); // ユーザーリストの取得に
        public static readonly PacketId C2S_USER_LIST_USER_LIST_MAX_NUM_REQ = new PacketId(5, 1, 1, "C2S_USER_LIST_USER_LIST_MAX_NUM_REQ");
        public static readonly PacketId S2C_USER_LIST_USER_LIST_MAX_NUM_RES = new PacketId(5, 1, 2, "S2C_USER_LIST_USER_LIST_MAX_NUM_RES"); // ユーザーリストの最大人数指定に
        public static readonly PacketId S2C_USER_5_2_16_NTC = new PacketId(5, 2, 16, "S2C_USER_5_2_16_NTC");
        public static readonly PacketId S2C_USER_5_3_16_NTC = new PacketId(5, 3, 16, "S2C_USER_5_3_16_NTC");

// Group: 6 - (PARTY)
        public static readonly PacketId C2S_PARTY_PARTY_CREATE_REQ = new PacketId(6, 0, 1, "C2S_PARTY_PARTY_CREATE_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_CREATE_RES = new PacketId(6, 0, 2, "S2C_PARTY_PARTY_CREATE_RES"); // パーティ作成に
        public static readonly PacketId C2S_PARTY_PARTY_INVITE_REQ = new PacketId(6, 1, 1, "C2S_PARTY_PARTY_INVITE_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_INVITE_RES = new PacketId(6, 1, 2, "S2C_PARTY_PARTY_INVITE_RES"); // パーティ要請（要請側）に
        public static readonly PacketId S2C_PARTY_6_1_16_NTC = new PacketId(6, 1, 16, "S2C_PARTY_6_1_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_INVITE_CHARACTER_REQ = new PacketId(6, 2, 1, "C2S_PARTY_PARTY_INVITE_CHARACTER_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_INVITE_CHARACTER_RES = new PacketId(6, 2, 2, "S2C_PARTY_PARTY_INVITE_CHARACTER_RES"); // パーティ要請（キャラ）（要請側）に
        public static readonly PacketId C2S_PARTY_PARTY_INVITE_CANCEL_REQ = new PacketId(6, 3, 1, "C2S_PARTY_PARTY_INVITE_CANCEL_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_INVITE_CANCEL_RES = new PacketId(6, 3, 2, "S2C_PARTY_PARTY_INVITE_CANCEL_RES"); // パーティ要請キャンセル（要請側）に
        public static readonly PacketId S2C_PARTY_6_3_16_NTC = new PacketId(6, 3, 16, "S2C_PARTY_6_3_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_INVITE_REFUSE_REQ = new PacketId(6, 4, 1, "C2S_PARTY_PARTY_INVITE_REFUSE_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_INVITE_REFUSE_RES = new PacketId(6, 4, 2, "S2C_PARTY_PARTY_INVITE_REFUSE_RES"); // パーティ要請拒否（要請受ける側）に
        public static readonly PacketId C2S_PARTY_PARTY_INVITE_PREPARE_ACCEPT_REQ = new PacketId(6, 5, 1, "C2S_PARTY_PARTY_INVITE_PREPARE_ACCEPT_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_INVITE_PREPARE_ACCEPT_RES = new PacketId(6, 5, 2, "S2C_PARTY_PARTY_INVITE_PREPARE_ACCEPT_RES"); // パーティ要請受諾（要請受ける側）に
        public static readonly PacketId S2C_PARTY_6_5_16_NTC = new PacketId(6, 5, 16, "S2C_PARTY_6_5_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_INVITE_ENTRY_REQ = new PacketId(6, 6, 1, "C2S_PARTY_PARTY_INVITE_ENTRY_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_INVITE_ENTRY_RES = new PacketId(6, 6, 2, "S2C_PARTY_PARTY_INVITE_ENTRY_RES"); // 要請受諾エントリーに
        public static readonly PacketId S2C_PARTY_6_6_16_NTC = new PacketId(6, 6, 16, "S2C_PARTY_6_6_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_INVITE_ENTRY_CANCEL_REQ = new PacketId(6, 7, 1, "C2S_PARTY_PARTY_INVITE_ENTRY_CANCEL_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_INVITE_ENTRY_CANCEL_RES = new PacketId(6, 7, 2, "S2C_PARTY_PARTY_INVITE_ENTRY_CANCEL_RES"); // 要請受諾エントリーキャンセルに
        public static readonly PacketId S2C_PARTY_6_7_16_NTC = new PacketId(6, 7, 16, "S2C_PARTY_6_7_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_JOIN_REQ = new PacketId(6, 8, 1, "C2S_PARTY_PARTY_JOIN_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_JOIN_RES = new PacketId(6, 8, 2, "S2C_PARTY_PARTY_JOIN_RES"); // パーティ参加に
        public static readonly PacketId S2C_PARTY_6_8_16_NTC = new PacketId(6, 8, 16, "S2C_PARTY_6_8_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_GET_CONTENT_NUMBER_REQ = new PacketId(6, 9, 1, "C2S_PARTY_PARTY_GET_CONTENT_NUMBER_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_GET_CONTENT_NUMBER_RES = new PacketId(6, 9, 2, "S2C_PARTY_PARTY_GET_CONTENT_NUMBER_RES"); // パーティコンテンツ情報取得に
        public static readonly PacketId C2S_PARTY_6_10_1_REQ = new PacketId(6, 10, 1, "C2S_PARTY_6_10_1_REQ");
        public static readonly PacketId S2C_PARTY_6_10_2_RES = new PacketId(6, 10, 2, "S2C_PARTY_6_10_2_RES");
        public static readonly PacketId S2C_PARTY_6_10_16_NTC = new PacketId(6, 10, 16, "S2C_PARTY_6_10_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_MEMBER_KICK_REQ = new PacketId(6, 11, 1, "C2S_PARTY_PARTY_MEMBER_KICK_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_MEMBER_KICK_RES = new PacketId(6, 11, 2, "S2C_PARTY_PARTY_MEMBER_KICK_RES"); // パーティメンバーキックに
        public static readonly PacketId S2C_PARTY_6_11_16_NTC = new PacketId(6, 11, 16, "S2C_PARTY_6_11_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_BREAKUP_REQ = new PacketId(6, 12, 1, "C2S_PARTY_PARTY_BREAKUP_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_BREAKUP_RES = new PacketId(6, 12, 2, "S2C_PARTY_PARTY_BREAKUP_RES"); // パーティ解散に
        public static readonly PacketId S2C_PARTY_6_12_16_NTC = new PacketId(6, 12, 16, "S2C_PARTY_6_12_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_CHANGE_LEADER_REQ = new PacketId(6, 13, 1, "C2S_PARTY_PARTY_CHANGE_LEADER_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_CHANGE_LEADER_RES = new PacketId(6, 13, 2, "S2C_PARTY_PARTY_CHANGE_LEADER_RES"); // パーティリーダー変更に
        public static readonly PacketId S2C_PARTY_6_13_16_NTC = new PacketId(6, 13, 16, "S2C_PARTY_6_13_16_NTC");
        public static readonly PacketId C2S_PARTY_PARTY_SEARCH_REQ = new PacketId(6, 14, 1, "C2S_PARTY_PARTY_SEARCH_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_SEARCH_RES = new PacketId(6, 14, 2, "S2C_PARTY_PARTY_SEARCH_RES"); // パーティ検索に
        public static readonly PacketId C2S_PARTY_PARTY_MEMBER_SET_VALUE_REQ = new PacketId(6, 15, 1, "C2S_PARTY_PARTY_MEMBER_SET_VALUE_REQ");
        public static readonly PacketId S2C_PARTY_PARTY_MEMBER_SET_VALUE_RES = new PacketId(6, 15, 2, "S2C_PARTY_PARTY_MEMBER_SET_VALUE_RES"); // パーティメンバー汎用フラグ操作に
        public static readonly PacketId S2C_PARTY_6_15_16_NTC = new PacketId(6, 15, 16, "S2C_PARTY_6_15_16_NTC");
        public static readonly PacketId S2C_PARTY_6_16_16_NTC = new PacketId(6, 16, 16, "S2C_PARTY_6_16_16_NTC");
        public static readonly PacketId S2C_PARTY_6_17_16_NTC = new PacketId(6, 17, 16, "S2C_PARTY_6_17_16_NTC");
        public static readonly PacketId S2C_PARTY_6_18_16_NTC = new PacketId(6, 18, 16, "S2C_PARTY_6_18_16_NTC");
        public static readonly PacketId S2C_PARTY_6_19_16_NTC = new PacketId(6, 19, 16, "S2C_PARTY_6_19_16_NTC");
        public static readonly PacketId S2C_PARTY_6_20_16_NTC = new PacketId(6, 20, 16, "S2C_PARTY_6_20_16_NTC");
        public static readonly PacketId S2C_PARTY_6_21_16_NTC = new PacketId(6, 21, 16, "S2C_PARTY_6_21_16_NTC");
        public static readonly PacketId S2C_PARTY_6_22_16_NTC = new PacketId(6, 22, 16, "S2C_PARTY_6_22_16_NTC");
        public static readonly PacketId S2C_PARTY_6_24_16_NTC = new PacketId(6, 24, 16, "S2C_PARTY_6_24_16_NTC");
        public static readonly PacketId S2C_PARTY_6_26_16_NTC = new PacketId(6, 26, 16, "S2C_PARTY_6_26_16_NTC");

// Group: 7 - (QUICK)
        public static readonly PacketId C2S_QUICK_PARTY_REGISTER_REQ = new PacketId(7, 0, 1, "C2S_QUICK_PARTY_REGISTER_REQ");
        public static readonly PacketId S2C_QUICK_PARTY_REGISTER_RES = new PacketId(7, 0, 2, "S2C_QUICK_PARTY_REGISTER_RES"); // クイックパーティ登録要求に
        public static readonly PacketId S2C_QUICK_7_0_16_NTC = new PacketId(7, 0, 16, "S2C_QUICK_7_0_16_NTC");
        public static readonly PacketId C2S_QUICK_PARTY_REGISTER_QUEST_REQ = new PacketId(7, 1, 1, "C2S_QUICK_PARTY_REGISTER_QUEST_REQ");
        public static readonly PacketId S2C_QUICK_PARTY_REGISTER_QUEST_RES = new PacketId(7, 1, 2, "S2C_QUICK_PARTY_REGISTER_QUEST_RES"); // クイックパーティクエスト用登録要求に
        public static readonly PacketId S2C_QUICK_7_1_16_NTC = new PacketId(7, 1, 16, "S2C_QUICK_7_1_16_NTC");
        public static readonly PacketId C2S_QUICK_PARTY_ENTRY_REQ = new PacketId(7, 3, 1, "C2S_QUICK_PARTY_ENTRY_REQ");
        public static readonly PacketId S2C_QUICK_PARTY_ENTRY_RES = new PacketId(7, 3, 2, "S2C_QUICK_PARTY_ENTRY_RES"); // クイックパーティエントリー要求に
        public static readonly PacketId S2C_QUICK_7_3_16_NTC = new PacketId(7, 3, 16, "S2C_QUICK_7_3_16_NTC");
        public static readonly PacketId S2C_QUICK_7_5_16_NTC = new PacketId(7, 5, 16, "S2C_QUICK_7_5_16_NTC");
        public static readonly PacketId S2C_QUICK_7_6_16_NTC = new PacketId(7, 6, 16, "S2C_QUICK_7_6_16_NTC");
        public static readonly PacketId S2C_QUICK_7_7_16_NTC = new PacketId(7, 7, 16, "S2C_QUICK_7_7_16_NTC");

// Group: 8 - (PAWN)
        public static readonly PacketId C2S_PAWN_CREATE_MYPAWN_REQ = new PacketId(8, 0, 1, "C2S_PAWN_CREATE_MYPAWN_REQ");
        public static readonly PacketId S2C_PAWN_CREATE_MYPAWN_RES = new PacketId(8, 0, 2, "S2C_PAWN_CREATE_MYPAWN_RES"); // マイポーン作成に
        public static readonly PacketId C2S_PAWN_DELETE_MYPAWN_REQ = new PacketId(8, 1, 1, "C2S_PAWN_DELETE_MYPAWN_REQ");
        public static readonly PacketId S2C_PAWN_DELETE_MYPAWN_RES = new PacketId(8, 1, 2, "S2C_PAWN_DELETE_MYPAWN_RES"); // マイポーン削除に
        public static readonly PacketId C2S_PAWN_GET_MYPAWN_LIST_REQ = new PacketId(8, 2, 1, "C2S_PAWN_GET_MYPAWN_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_MYPAWN_LIST_RES = new PacketId(8, 2, 2, "S2C_PAWN_GET_MYPAWN_LIST_RES"); // マイポーンリスト取得に
        public static readonly PacketId C2S_PAWN_GET_MYPAWN_DATA_REQ = new PacketId(8, 3, 1, "C2S_PAWN_GET_MYPAWN_DATA_REQ");
        public static readonly PacketId S2C_PAWN_GET_MYPAWN_DATA_RES = new PacketId(8, 3, 2, "S2C_PAWN_GET_MYPAWN_DATA_RES"); // マイポーンデータ取得に
        public static readonly PacketId C2S_PAWN_GET_REGISTERED_PAWN_LIST_REQ = new PacketId(8, 4, 1, "C2S_PAWN_GET_REGISTERED_PAWN_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_REGISTERED_PAWN_LIST_RES = new PacketId(8, 4, 2, "S2C_PAWN_GET_REGISTERED_PAWN_LIST_RES"); // 登録されてるポーンリスト取得に
        public static readonly PacketId C2S_PAWN_GET_REGISTERED_PAWN_LIST_BY_CHARACTER_REQ = new PacketId(8, 5, 1, "C2S_PAWN_GET_REGISTERED_PAWN_LIST_BY_CHARACTER_REQ");
        public static readonly PacketId S2C_PAWN_GET_REGISTERED_PAWN_LIST_BY_CHARACTER_RES = new PacketId(8, 5, 2, "S2C_PAWN_GET_REGISTERED_PAWN_LIST_BY_CHARACTER_RES"); // 登録されてるポーンリスト取得に
        public static readonly PacketId C2S_PAWN_GET_REGISTERED_PAWN_DATA_REQ = new PacketId(8, 6, 1, "C2S_PAWN_GET_REGISTERED_PAWN_DATA_REQ");
        public static readonly PacketId S2C_PAWN_GET_REGISTERED_PAWN_DATA_RES = new PacketId(8, 6, 2, "S2C_PAWN_GET_REGISTERED_PAWN_DATA_RES"); // 登録されてるポーンデータ取得に
        public static readonly PacketId C2S_PAWN_RENT_REGISTERED_PAWN_REQ = new PacketId(8, 7, 1, "C2S_PAWN_RENT_REGISTERED_PAWN_REQ");
        public static readonly PacketId S2C_PAWN_RENT_REGISTERED_PAWN_RES = new PacketId(8, 7, 2, "S2C_PAWN_RENT_REGISTERED_PAWN_RES"); // ポーンレンタル要求に
        public static readonly PacketId C2S_PAWN_GET_RENTED_PAWN_LIST_REQ = new PacketId(8, 8, 1, "C2S_PAWN_GET_RENTED_PAWN_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_RENTED_PAWN_LIST_RES = new PacketId(8, 8, 2, "S2C_PAWN_GET_RENTED_PAWN_LIST_RES"); // レンタルポーンリスト取得に
        public static readonly PacketId C2S_PAWN_GET_RENTED_PAWN_DATA_REQ = new PacketId(8, 9, 1, "C2S_PAWN_GET_RENTED_PAWN_DATA_REQ");
        public static readonly PacketId S2C_PAWN_GET_RENTED_PAWN_DATA_RES = new PacketId(8, 9, 2, "S2C_PAWN_GET_RENTED_PAWN_DATA_RES"); // レンタルポーンデータ取得に
        public static readonly PacketId C2S_PAWN_RETURN_RENTED_PAWN_REQ = new PacketId(8, 10, 1, "C2S_PAWN_RETURN_RENTED_PAWN_REQ");
        public static readonly PacketId S2C_PAWN_RETURN_RENTED_PAWN_RES = new PacketId(8, 10, 2, "S2C_PAWN_RETURN_RENTED_PAWN_RES"); // ポーン返却に
        public static readonly PacketId C2S_PAWN_GET_PARTY_PAWN_DATA_REQ = new PacketId(8, 11, 1, "C2S_PAWN_GET_PARTY_PAWN_DATA_REQ");
        public static readonly PacketId S2C_PAWN_GET_PARTY_PAWN_DATA_RES = new PacketId(8, 11, 2, "S2C_PAWN_GET_PARTY_PAWN_DATA_RES"); // パーティ内のポーンデータ取得に
        public static readonly PacketId C2S_PAWN_JOIN_PARTY_MYPAWN_REQ = new PacketId(8, 12, 1, "C2S_PAWN_JOIN_PARTY_MYPAWN_REQ");
        public static readonly PacketId S2C_PAWN_JOIN_PARTY_MYPAWN_RES = new PacketId(8, 12, 2, "S2C_PAWN_JOIN_PARTY_MYPAWN_RES"); // マイポーンのパーティー参加に
        public static readonly PacketId S2C_PAWN_8_12_16_NTC = new PacketId(8, 12, 16, "S2C_PAWN_8_12_16_NTC");
        public static readonly PacketId C2S_PAWN_JOIN_PARTY_RENTED_PAWN_REQ = new PacketId(8, 13, 1, "C2S_PAWN_JOIN_PARTY_RENTED_PAWN_REQ");
        public static readonly PacketId S2C_PAWN_JOIN_PARTY_RENTED_PAWN_RES = new PacketId(8, 13, 2, "S2C_PAWN_JOIN_PARTY_RENTED_PAWN_RES"); // レンタルポーンのパーティー参加に
        public static readonly PacketId C2S_PAWN_GET_FAVORITE_PAWN_LIST_REQ = new PacketId(8, 14, 1, "C2S_PAWN_GET_FAVORITE_PAWN_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_FAVORITE_PAWN_LIST_RES = new PacketId(8, 14, 2, "S2C_PAWN_GET_FAVORITE_PAWN_LIST_RES"); // お気に入りポーンリスト取得に
        public static readonly PacketId C2S_PAWN_SET_FAVORITE_PAWN_REQ = new PacketId(8, 15, 1, "C2S_PAWN_SET_FAVORITE_PAWN_REQ");
        public static readonly PacketId S2C_PAWN_SET_FAVORITE_PAWN_RES = new PacketId(8, 15, 2, "S2C_PAWN_SET_FAVORITE_PAWN_RES"); // お気に入りポーン登録に
        public static readonly PacketId C2S_PAWN_DELETE_FAVORITE_PAWN_REQ = new PacketId(8, 16, 1, "C2S_PAWN_DELETE_FAVORITE_PAWN_REQ");
        public static readonly PacketId S2C_PAWN_DELETE_FAVORITE_PAWN_RES = new PacketId(8, 16, 2, "S2C_PAWN_DELETE_FAVORITE_PAWN_RES"); // お気に入りポーン抹消に
        public static readonly PacketId C2S_PAWN_GET_OFFICIAL_PAWN_LIST_REQ = new PacketId(8, 17, 1, "C2S_PAWN_GET_OFFICIAL_PAWN_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_OFFICIAL_PAWN_LIST_RES = new PacketId(8, 17, 2, "S2C_PAWN_GET_OFFICIAL_PAWN_LIST_RES"); // 公式ポーンリスト取得に
        public static readonly PacketId C2S_PAWN_GET_LEGEND_PAWN_LIST_REQ = new PacketId(8, 18, 1, "C2S_PAWN_GET_LEGEND_PAWN_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_LEGEND_PAWN_LIST_RES = new PacketId(8, 18, 2, "S2C_PAWN_GET_LEGEND_PAWN_LIST_RES"); // レジェンドポーンリスト取得に
        public static readonly PacketId C2S_PAWN_PAWN_LOST_REQ = new PacketId(8, 19, 1, "C2S_PAWN_PAWN_LOST_REQ");
        public static readonly PacketId S2C_PAWN_PAWN_LOST_RES = new PacketId(8, 19, 2, "S2C_PAWN_PAWN_LOST_RES"); // ポーンロストに
        public static readonly PacketId S2C_PAWN_8_19_16_NTC = new PacketId(8, 19, 16, "S2C_PAWN_8_19_16_NTC");
        public static readonly PacketId C2S_PAWN_GET_LOST_PAWN_LIST_REQ = new PacketId(8, 20, 1, "C2S_PAWN_GET_LOST_PAWN_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_LOST_PAWN_LIST_RES = new PacketId(8, 20, 2, "S2C_PAWN_GET_LOST_PAWN_LIST_RES"); // ロストポーンポーンリスト取得に
        public static readonly PacketId C2S_PAWN_LOST_PAWN_REVIVE_REQ = new PacketId(8, 21, 1, "C2S_PAWN_LOST_PAWN_REVIVE_REQ");
        public static readonly PacketId S2C_PAWN_LOST_PAWN_REVIVE_RES = new PacketId(8, 21, 2, "S2C_PAWN_LOST_PAWN_REVIVE_RES"); // ロストポーン復活に
        public static readonly PacketId C2S_PAWN_LOST_PAWN_POINT_REVIVE_REQ = new PacketId(8, 22, 1, "C2S_PAWN_LOST_PAWN_POINT_REVIVE_REQ");
        public static readonly PacketId S2C_PAWN_LOST_PAWN_POINT_REVIVE_RES = new PacketId(8, 22, 2, "S2C_PAWN_LOST_PAWN_POINT_REVIVE_RES"); // ロストポーン復活(復活力消費)に
        public static readonly PacketId C2S_PAWN_LOST_PAWN_GOLDEN_REVIVE_REQ = new PacketId(8, 23, 1, "C2S_PAWN_LOST_PAWN_GOLDEN_REVIVE_REQ");
        public static readonly PacketId S2C_PAWN_LOST_PAWN_GOLDEN_REVIVE_RES = new PacketId(8, 23, 2, "S2C_PAWN_LOST_PAWN_GOLDEN_REVIVE_RES"); // ロストポーン復活(黄金石消費)に
        public static readonly PacketId C2S_PAWN_LOST_PAWN_WALLET_REVIVE_REQ = new PacketId(8, 24, 1, "C2S_PAWN_LOST_PAWN_WALLET_REVIVE_REQ");
        public static readonly PacketId S2C_PAWN_LOST_PAWN_WALLET_REVIVE_RES = new PacketId(8, 24, 2, "S2C_PAWN_LOST_PAWN_WALLET_REVIVE_RES"); // ロストポーン復活(通貨消費)に
        public static readonly PacketId C2S_PAWN_8_25_1_REQ = new PacketId(8, 25, 1, "C2S_PAWN_8_25_1_REQ");
        public static readonly PacketId S2C_PAWN_8_25_2_RES = new PacketId(8, 25, 2, "S2C_PAWN_8_25_2_RES");
        public static readonly PacketId S2C_PAWN_8_25_16_NTC = new PacketId(8, 25, 16, "S2C_PAWN_8_25_16_NTC");
        public static readonly PacketId C2S_PAWN_UPDATE_PAWN_REACTION_LIST_REQ = new PacketId(8, 26, 1, "C2S_PAWN_UPDATE_PAWN_REACTION_LIST_REQ");
        public static readonly PacketId S2C_PAWN_UPDATE_PAWN_REACTION_LIST_RES = new PacketId(8, 26, 2, "S2C_PAWN_UPDATE_PAWN_REACTION_LIST_RES"); // ポーンリアクションリスト更新に
        public static readonly PacketId S2C_PAWN_8_26_16_NTC = new PacketId(8, 26, 16, "S2C_PAWN_8_26_16_NTC");
        public static readonly PacketId C2S_PAWN_UPDATE_PAWN_SHARE_RANGE_REQ = new PacketId(8, 27, 1, "C2S_PAWN_UPDATE_PAWN_SHARE_RANGE_REQ");
        public static readonly PacketId S2C_PAWN_UPDATE_PAWN_SHARE_RANGE_RES = new PacketId(8, 27, 2, "S2C_PAWN_UPDATE_PAWN_SHARE_RANGE_RES"); // ポーン公開設定更新に
        public static readonly PacketId C2S_PAWN_GET_PAWN_HISTORY_LIST_REQ = new PacketId(8, 28, 1, "C2S_PAWN_GET_PAWN_HISTORY_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_PAWN_HISTORY_LIST_RES = new PacketId(8, 28, 2, "S2C_PAWN_GET_PAWN_HISTORY_LIST_RES"); // ポーンレンタル履歴リスト取得に
        public static readonly PacketId C2S_PAWN_GET_PAWN_TOTAL_SCORE_REQ = new PacketId(8, 29, 1, "C2S_PAWN_GET_PAWN_TOTAL_SCORE_REQ");
        public static readonly PacketId S2C_PAWN_GET_PAWN_TOTAL_SCORE_RES = new PacketId(8, 29, 2, "S2C_PAWN_GET_PAWN_TOTAL_SCORE_RES"); // ポーン総合評価取得に
        public static readonly PacketId C2S_PAWN_GET_NORA_PAWN_LIST_REQ = new PacketId(8, 30, 1, "C2S_PAWN_GET_NORA_PAWN_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_NORA_PAWN_LIST_RES = new PacketId(8, 30, 2, "S2C_PAWN_GET_NORA_PAWN_LIST_RES"); // 野良ポーンリスト取得に
        public static readonly PacketId C2S_PAWN_GET_FREE_RENTAL_PAWN_LIST_REQ = new PacketId(8, 31, 1, "C2S_PAWN_GET_FREE_RENTAL_PAWN_LIST_REQ");
        public static readonly PacketId S2C_PAWN_GET_FREE_RENTAL_PAWN_LIST_RES = new PacketId(8, 31, 2, "S2C_PAWN_GET_FREE_RENTAL_PAWN_LIST_RES"); // 購入済みポーンリスト取得に
        public static readonly PacketId C2S_PAWN_GET_NORA_PAWN_DATA_REQ = new PacketId(8, 32, 1, "C2S_PAWN_GET_NORA_PAWN_DATA_REQ");
        public static readonly PacketId S2C_PAWN_GET_NORA_PAWN_DATA_RES = new PacketId(8, 32, 2, "S2C_PAWN_GET_NORA_PAWN_DATA_RES"); // 野良ポーンデータ取得に
        public static readonly PacketId S2C_PAWN_8_33_16_NTC = new PacketId(8, 33, 16, "S2C_PAWN_8_33_16_NTC");
        public static readonly PacketId S2C_PAWN_8_34_16_NTC = new PacketId(8, 34, 16, "S2C_PAWN_8_34_16_NTC");
        public static readonly PacketId S2C_PAWN_8_35_16_NTC = new PacketId(8, 35, 16, "S2C_PAWN_8_35_16_NTC");
        public static readonly PacketId S2C_PAWN_8_36_16_NTC = new PacketId(8, 36, 16, "S2C_PAWN_8_36_16_NTC");
        public static readonly PacketId S2C_PAWN_8_37_16_NTC = new PacketId(8, 37, 16, "S2C_PAWN_8_37_16_NTC");
        public static readonly PacketId S2C_PAWN_8_38_16_NTC = new PacketId(8, 38, 16, "S2C_PAWN_8_38_16_NTC");
        public static readonly PacketId S2C_PAWN_8_39_16_NTC = new PacketId(8, 39, 16, "S2C_PAWN_8_39_16_NTC");
        public static readonly PacketId S2C_PAWN_8_40_16_NTC = new PacketId(8, 40, 16, "S2C_PAWN_8_40_16_NTC");
        public static readonly PacketId S2C_PAWN_8_41_16_NTC = new PacketId(8, 41, 16, "S2C_PAWN_8_41_16_NTC");
        public static readonly PacketId C2S_PAWN_EXTRA_JOIN_PARTY_REQ = new PacketId(8, 42, 1, "C2S_PAWN_EXTRA_JOIN_PARTY_REQ");
        public static readonly PacketId S2C_PAWN_EXTRA_JOIN_PARTY_RES = new PacketId(8, 42, 2, "S2C_PAWN_EXTRA_JOIN_PARTY_RES"); // 特殊ポーンの一括レンタル
        public static readonly PacketId C2S_PAWN_EXTRA_LEAVE_PARTY_REQ = new PacketId(8, 43, 1, "C2S_PAWN_EXTRA_LEAVE_PARTY_REQ");
        public static readonly PacketId S2C_PAWN_EXTRA_LEAVE_PARTY_RES = new PacketId(8, 43, 2, "S2C_PAWN_EXTRA_LEAVE_PARTY_RES"); // 特殊ポーンの一括離脱

// Group: 9 - (BINARY)
        public static readonly PacketId C2S_BINARY_SAVE_GET_CHARACTER_BIN_SAVEDATA_REQ = new PacketId(9, 0, 1, "C2S_BINARY_SAVE_GET_CHARACTER_BIN_SAVEDATA_REQ");
        public static readonly PacketId S2C_BINARY_SAVE_GET_CHARACTER_BIN_SAVEDATA_RES = new PacketId(9, 0, 2, "S2C_BINARY_SAVE_GET_CHARACTER_BIN_SAVEDATA_RES"); // キャラクターバイナリ情報取得に
        public static readonly PacketId C2S_BINARY_SAVE_SET_CHARACTER_BIN_SAVEDATA_REQ = new PacketId(9, 1, 1, "C2S_BINARY_SAVE_SET_CHARACTER_BIN_SAVEDATA_REQ");
        public static readonly PacketId S2C_BINARY_SAVE_SET_CHARACTER_BIN_SAVEDATA_RES = new PacketId(9, 1, 2, "S2C_BINARY_SAVE_SET_CHARACTER_BIN_SAVEDATA_RES"); // キャラクターバイナリ情報セットに

// Group: 10 - (ITEM)
        public static readonly PacketId C2S_ITEM_USE_BAG_ITEM_REQ = new PacketId(10, 0, 1, "C2S_ITEM_USE_BAG_ITEM_REQ");
        public static readonly PacketId S2C_ITEM_USE_BAG_ITEM_RES = new PacketId(10, 0, 2, "S2C_ITEM_USE_BAG_ITEM_RES"); // 所持アイテム使用に
        public static readonly PacketId S2C_ITEM_10_0_16_NTC = new PacketId(10, 0, 16, "S2C_ITEM_10_0_16_NTC");
        public static readonly PacketId C2S_ITEM_USE_JOB_ITEMS_REQ = new PacketId(10, 1, 1, "C2S_ITEM_USE_JOB_ITEMS_REQ");
        public static readonly PacketId S2C_ITEM_USE_JOB_ITEMS_RES = new PacketId(10, 1, 2, "S2C_ITEM_USE_JOB_ITEMS_RES"); // ジョブ専用アイテム使用(複数スロット)に
        public static readonly PacketId C2S_ITEM_LOAD_STORAGE_ITEM_REQ = new PacketId(10, 2, 1, "C2S_ITEM_LOAD_STORAGE_ITEM_REQ");
        public static readonly PacketId S2C_ITEM_LOAD_STORAGE_ITEM_RES = new PacketId(10, 2, 2, "S2C_ITEM_LOAD_STORAGE_ITEM_RES"); // 指定倉庫のキャッシュリクエストに
        public static readonly PacketId C2S_ITEM_GET_STORAGE_ITEM_LIST_REQ = new PacketId(10, 3, 1, "C2S_ITEM_GET_STORAGE_ITEM_LIST_REQ");
        public static readonly PacketId S2C_ITEM_GET_STORAGE_ITEM_LIST_RES = new PacketId(10, 3, 2, "S2C_ITEM_GET_STORAGE_ITEM_LIST_RES"); // 倉庫アイテムリスト取得に
        public static readonly PacketId C2S_ITEM_GET_ITEM_STORE_LIST_REQ = new PacketId(10, 4, 1, "C2S_ITEM_GET_ITEM_STORE_LIST_REQ");
        public static readonly PacketId S2C_ITEM_GET_ITEM_STORE_LIST_RES = new PacketId(10, 4, 2, "S2C_ITEM_GET_ITEM_STORE_LIST_RES"); // 指定アイテム所持状態の取得に
        public static readonly PacketId C2S_ITEM_CONSUME_STORAGE_ITEM_REQ = new PacketId(10, 5, 1, "C2S_ITEM_CONSUME_STORAGE_ITEM_REQ");
        public static readonly PacketId S2C_ITEM_CONSUME_STORAGE_ITEM_RES = new PacketId(10, 5, 2, "S2C_ITEM_CONSUME_STORAGE_ITEM_RES"); // 倉庫アイテム消費に
        public static readonly PacketId C2S_ITEM_GET_POST_ITEM_LIST_REQ = new PacketId(10, 6, 1, "C2S_ITEM_GET_POST_ITEM_LIST_REQ");
        public static readonly PacketId S2C_ITEM_GET_POST_ITEM_LIST_RES = new PacketId(10, 6, 2, "S2C_ITEM_GET_POST_ITEM_LIST_RES"); // 宅配アイテムリスト取得に
        public static readonly PacketId C2S_ITEM_MOVE_ITEM_REQ = new PacketId(10, 7, 1, "C2S_ITEM_MOVE_ITEM_REQ");
        public static readonly PacketId S2C_ITEM_MOVE_ITEM_RES = new PacketId(10, 7, 2, "S2C_ITEM_MOVE_ITEM_RES"); // アイテム移動に
        public static readonly PacketId C2S_ITEM_SELL_ITEM_REQ = new PacketId(10, 8, 1, "C2S_ITEM_SELL_ITEM_REQ");
        public static readonly PacketId S2C_ITEM_SELL_ITEM_RES = new PacketId(10, 8, 2, "S2C_ITEM_SELL_ITEM_RES"); // アイテム売却に
        public static readonly PacketId C2S_ITEM_GET_ITEM_STORAGE_INFO_REQ = new PacketId(10, 9, 1, "C2S_ITEM_GET_ITEM_STORAGE_INFO_REQ");
        public static readonly PacketId S2C_ITEM_GET_ITEM_STORAGE_INFO_RES = new PacketId(10, 9, 2, "S2C_ITEM_GET_ITEM_STORAGE_INFO_RES"); // アイテムストレージ情報取得に
        public static readonly PacketId S2C_ITEM_10_10_16_NTC = new PacketId(10, 10, 16, "S2C_ITEM_10_10_16_NTC");
        public static readonly PacketId S2C_ITEM_10_11_16_NTC = new PacketId(10, 11, 16, "S2C_ITEM_10_11_16_NTC");
        public static readonly PacketId S2C_ITEM_10_12_16_NTC = new PacketId(10, 12, 16, "S2C_ITEM_10_12_16_NTC");
        public static readonly PacketId S2C_ITEM_10_13_16_NTC = new PacketId(10, 13, 16, "S2C_ITEM_10_13_16_NTC");
        public static readonly PacketId S2C_ITEM_10_14_16_NTC = new PacketId(10, 14, 16, "S2C_ITEM_10_14_16_NTC");
        public static readonly PacketId C2S_ITEM_GET_PAY_COST_REQ = new PacketId(10, 15, 1, "C2S_ITEM_GET_PAY_COST_REQ");
        public static readonly PacketId S2C_ITEM_GET_PAY_COST_RES = new PacketId(10, 15, 2, "S2C_ITEM_GET_PAY_COST_RES"); // 代価
        public static readonly PacketId C2S_ITEM_GET_VALUABLE_ITEM_LIST_REQ = new PacketId(10, 16, 1, "C2S_ITEM_GET_VALUABLE_ITEM_LIST_REQ");
        public static readonly PacketId S2C_ITEM_GET_VALUABLE_ITEM_LIST_RES = new PacketId(10, 16, 2, "S2C_ITEM_GET_VALUABLE_ITEM_LIST_RES"); // 落し物リスト取得
        public static readonly PacketId C2S_ITEM_RECOVERY_VALUABLE_ITEM_REQ = new PacketId(10, 17, 1, "C2S_ITEM_RECOVERY_VALUABLE_ITEM_REQ");
        public static readonly PacketId S2C_ITEM_RECOVERY_VALUABLE_ITEM_RES = new PacketId(10, 17, 2, "S2C_ITEM_RECOVERY_VALUABLE_ITEM_RES"); // 落し物の再取得
        public static readonly PacketId C2S_ITEM_GET_ITEM_STORAGE_CUSTOM_REQ = new PacketId(10, 18, 1, "C2S_ITEM_GET_ITEM_STORAGE_CUSTOM_REQ");
        public static readonly PacketId S2C_ITEM_GET_ITEM_STORAGE_CUSTOM_RES = new PacketId(10, 18, 2, "S2C_ITEM_GET_ITEM_STORAGE_CUSTOM_RES"); // アイテムストレージカスタム設定取得
        public static readonly PacketId C2S_ITEM_UPDATE_ITEM_STORAGE_CUSTOM_REQ = new PacketId(10, 19, 1, "C2S_ITEM_UPDATE_ITEM_STORAGE_CUSTOM_REQ");
        public static readonly PacketId S2C_ITEM_UPDATE_ITEM_STORAGE_CUSTOM_RES = new PacketId(10, 19, 2, "S2C_ITEM_UPDATE_ITEM_STORAGE_CUSTOM_RES"); // アイテムストレージカスタム設定更新
        public static readonly PacketId C2S_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_REQ = new PacketId(10, 20, 1, "C2S_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_REQ");
        public static readonly PacketId S2C_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_RES = new PacketId(10, 20, 2, "S2C_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_RES"); // EQUIP_RARE_TYPE対象のアイテムリストの取得
        public static readonly PacketId C2S_ITEM_CHANGE_ATTR_DISCARD_REQ = new PacketId(10, 21, 1, "C2S_ITEM_CHANGE_ATTR_DISCARD_REQ");
        public static readonly PacketId S2C_ITEM_CHANGE_ATTR_DISCARD_RES = new PacketId(10, 21, 2, "S2C_ITEM_CHANGE_ATTR_DISCARD_RES"); // アイテム破棄売却フラグ変更完了
        public static readonly PacketId C2S_ITEM_GET_DEFAULT_STORAGE_EMPTY_SLOT_NUM_REQ = new PacketId(10, 22, 1, "C2S_ITEM_GET_DEFAULT_STORAGE_EMPTY_SLOT_NUM_REQ");
        public static readonly PacketId S2C_ITEM_GET_DEFAULT_STORAGE_EMPTY_SLOT_NUM_RES = new PacketId(10, 22, 2, "S2C_ITEM_GET_DEFAULT_STORAGE_EMPTY_SLOT_NUM_RES"); // 指定ストレージ（デフォルト：レスタニア側）の空き枠数取得に
        public static readonly PacketId C2S_ITEM_GET_SPECIFIED_HAVING_ITEM_LIST_REQ = new PacketId(10, 23, 1, "C2S_ITEM_GET_SPECIFIED_HAVING_ITEM_LIST_REQ");
        public static readonly PacketId S2C_ITEM_GET_SPECIFIED_HAVING_ITEM_LIST_RES = new PacketId(10, 23, 2, "S2C_ITEM_GET_SPECIFIED_HAVING_ITEM_LIST_RES"); // 指定アイテムIDの所持品取得に
        public static readonly PacketId C2S_ITEM_EMBODY_ITEMS_REQ = new PacketId(10, 24, 1, "C2S_ITEM_EMBODY_ITEMS_REQ");
        public static readonly PacketId S2C_ITEM_EMBODY_ITEMS_RES = new PacketId(10, 24, 2, "S2C_ITEM_EMBODY_ITEMS_RES"); // 具現化に
        public static readonly PacketId C2S_ITEM_GET_EMBODY_PAY_COST_REQ = new PacketId(10, 25, 1, "C2S_ITEM_GET_EMBODY_PAY_COST_REQ");
        public static readonly PacketId S2C_ITEM_GET_EMBODY_PAY_COST_RES = new PacketId(10, 25, 2, "S2C_ITEM_GET_EMBODY_PAY_COST_RES"); // 具現化コストの取得に

// Group: 11 - (QUEST)
        public static readonly PacketId C2S_QUEST_GET_LIGHT_QUEST_LIST_REQ = new PacketId(11, 0, 1, "C2S_QUEST_GET_LIGHT_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_LIGHT_QUEST_LIST_RES = new PacketId(11, 0, 2, "S2C_QUEST_GET_LIGHT_QUEST_LIST_RES"); // ライトクエストリストの取得に
        public static readonly PacketId C2S_QUEST_GET_SET_QUEST_LIST_REQ = new PacketId(11, 1, 1, "C2S_QUEST_GET_SET_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_SET_QUEST_LIST_RES = new PacketId(11, 1, 2, "S2C_QUEST_GET_SET_QUEST_LIST_RES"); // セットクエストリストの取得に
        public static readonly PacketId S2C_QUEST_11_1_16_NTC = new PacketId(11, 1, 16, "S2C_QUEST_11_1_16_NTC");
        public static readonly PacketId C2S_QUEST_GET_MAIN_QUEST_LIST_REQ = new PacketId(11, 2, 1, "C2S_QUEST_GET_MAIN_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_MAIN_QUEST_LIST_RES = new PacketId(11, 2, 2, "S2C_QUEST_GET_MAIN_QUEST_LIST_RES"); // メインクエストリストの取得に
        public static readonly PacketId S2C_QUEST_11_2_16_NTC = new PacketId(11, 2, 16, "S2C_QUEST_11_2_16_NTC");
        public static readonly PacketId C2S_QUEST_GET_TUTORIAL_QUEST_LIST_REQ = new PacketId(11, 3, 1, "C2S_QUEST_GET_TUTORIAL_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_TUTORIAL_QUEST_LIST_RES = new PacketId(11, 3, 2, "S2C_QUEST_GET_TUTORIAL_QUEST_LIST_RES"); // チュートリアルクエストリストに
        public static readonly PacketId C2S_QUEST_GET_LOT_QUEST_LIST_REQ = new PacketId(11, 4, 1, "C2S_QUEST_GET_LOT_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_LOT_QUEST_LIST_RES = new PacketId(11, 4, 2, "S2C_QUEST_GET_LOT_QUEST_LIST_RES"); // 抽選クエストリストの取得に
        public static readonly PacketId C2S_QUEST_GET_PACKAGE_QUEST_LIST_REQ = new PacketId(11, 5, 1, "C2S_QUEST_GET_PACKAGE_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_PACKAGE_QUEST_LIST_RES = new PacketId(11, 5, 2, "S2C_QUEST_GET_PACKAGE_QUEST_LIST_RES"); // パッケージクエストリストの取得に
        public static readonly PacketId C2S_QUEST_GET_MOB_HUNT_QUEST_LIST_REQ = new PacketId(11, 6, 1, "C2S_QUEST_GET_MOB_HUNT_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_MOB_HUNT_QUEST_LIST_RES = new PacketId(11, 6, 2, "S2C_QUEST_GET_MOB_HUNT_QUEST_LIST_RES"); // モブハントクエストリストの取得に
        public static readonly PacketId S2C_QUEST_11_7_16_NTC = new PacketId(11, 7, 16, "S2C_QUEST_11_7_16_NTC");
        public static readonly PacketId C2S_QUEST_GET_TIME_LIMITED_QUEST_LIST_REQ = new PacketId(11, 8, 1, "C2S_QUEST_GET_TIME_LIMITED_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_TIME_LIMITED_QUEST_LIST_RES = new PacketId(11, 8, 2, "S2C_QUEST_GET_TIME_LIMITED_QUEST_LIST_RES"); // 期間限定クエストリストの取得に
        public static readonly PacketId C2S_QUEST_GET_WORLD_MANAGE_QUEST_LIST_REQ = new PacketId(11, 9, 1, "C2S_QUEST_GET_WORLD_MANAGE_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_WORLD_MANAGE_QUEST_LIST_RES = new PacketId(11, 9, 2, "S2C_QUEST_GET_WORLD_MANAGE_QUEST_LIST_RES"); // 世界管理クエストリストの取得に
        public static readonly PacketId S2C_QUEST_11_9_16_NTC = new PacketId(11, 9, 16, "S2C_QUEST_11_9_16_NTC");
        public static readonly PacketId C2S_QUEST_GET_END_CONTENTS_GROUP_REQ = new PacketId(11, 10, 1, "C2S_QUEST_GET_END_CONTENTS_GROUP_REQ");
        public static readonly PacketId S2C_QUEST_GET_END_CONTENTS_GROUP_RES = new PacketId(11, 10, 2, "S2C_QUEST_GET_END_CONTENTS_GROUP_RES"); // エンドコンテンツクエストリストの取得に
        public static readonly PacketId C2S_QUEST_GET_QUEST_SCHEDULE_INFO_REQ = new PacketId(11, 11, 1, "C2S_QUEST_GET_QUEST_SCHEDULE_INFO_REQ");
        public static readonly PacketId S2C_QUEST_GET_QUEST_SCHEDULE_INFO_RES = new PacketId(11, 11, 2, "S2C_QUEST_GET_QUEST_SCHEDULE_INFO_RES"); // スケジューラIDからクエストIDの取得
        public static readonly PacketId C2S_QUEST_GET_MAIN_QUEST_COMPLETE_INFO_REQ = new PacketId(11, 12, 1, "C2S_QUEST_GET_MAIN_QUEST_COMPLETE_INFO_REQ");
        public static readonly PacketId S2C_QUEST_GET_MAIN_QUEST_COMPLETE_INFO_RES = new PacketId(11, 12, 2, "S2C_QUEST_GET_MAIN_QUEST_COMPLETE_INFO_RES"); // メインクエストクリア情報の取得に
        public static readonly PacketId C2S_QUEST_11_13_1_REQ = new PacketId(11, 13, 1, "C2S_QUEST_11_13_1_REQ");
        public static readonly PacketId S2C_QUEST_11_13_2_RES = new PacketId(11, 13, 2, "S2C_QUEST_11_13_2_RES");
        public static readonly PacketId C2S_QUEST_GET_SET_QUEST_INFO_LIST_REQ = new PacketId(11, 14, 1, "C2S_QUEST_GET_SET_QUEST_INFO_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_SET_QUEST_INFO_LIST_RES = new PacketId(11, 14, 2, "S2C_QUEST_GET_SET_QUEST_INFO_LIST_RES"); // 新聞用セットクエスト情報リストの取得に
        public static readonly PacketId C2S_QUEST_GET_CYCLE_CONTENTS_NEWS_LIST_REQ = new PacketId(11, 15, 1, "C2S_QUEST_GET_CYCLE_CONTENTS_NEWS_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_CYCLE_CONTENTS_NEWS_LIST_RES = new PacketId(11, 15, 2, "S2C_QUEST_GET_CYCLE_CONTENTS_NEWS_LIST_RES"); // 新聞用循環コンテンツ情報リストの取得に
        public static readonly PacketId C2S_QUEST_GET_PACKAGE_QUEST_INFO_REQ = new PacketId(11, 16, 1, "C2S_QUEST_GET_PACKAGE_QUEST_INFO_REQ");
        public static readonly PacketId S2C_QUEST_GET_PACKAGE_QUEST_INFO_RES = new PacketId(11, 16, 2, "S2C_QUEST_GET_PACKAGE_QUEST_INFO_RES"); // UI用パッケージクエスト情報リストの取得に
        public static readonly PacketId C2S_QUEST_GET_PACKAGE_QUEST_INFO_DETAIL_REQ = new PacketId(11, 17, 1, "C2S_QUEST_GET_PACKAGE_QUEST_INFO_DETAIL_REQ");
        public static readonly PacketId S2C_QUEST_GET_PACKAGE_QUEST_INFO_DETAIL_RES = new PacketId(11, 17, 2, "S2C_QUEST_GET_PACKAGE_QUEST_INFO_DETAIL_RES"); // UI用パッケージクエスト詳細情報の取得に
        public static readonly PacketId C2S_QUEST_ADD_PACKAGE_QUEST_POINT_REQ = new PacketId(11, 18, 1, "C2S_QUEST_ADD_PACKAGE_QUEST_POINT_REQ");
        public static readonly PacketId S2C_QUEST_ADD_PACKAGE_QUEST_POINT_RES = new PacketId(11, 18, 2, "S2C_QUEST_ADD_PACKAGE_QUEST_POINT_RES"); // パッケージクエストポイントの増加に
        public static readonly PacketId C2S_QUEST_GET_AREA_BONUS_LIST_REQ = new PacketId(11, 19, 1, "C2S_QUEST_GET_AREA_BONUS_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_AREA_BONUS_LIST_RES = new PacketId(11, 19, 2, "S2C_QUEST_GET_AREA_BONUS_LIST_RES"); // 新聞用エリアボーナスリスト取得に
        public static readonly PacketId C2S_QUEST_QUEST_ORDER_REQ = new PacketId(11, 20, 1, "C2S_QUEST_QUEST_ORDER_REQ");
        public static readonly PacketId S2C_QUEST_QUEST_ORDER_RES = new PacketId(11, 20, 2, "S2C_QUEST_QUEST_ORDER_RES"); // クエスト受注に
        public static readonly PacketId S2C_QUEST_11_20_16_NTC = new PacketId(11, 20, 16, "S2C_QUEST_11_20_16_NTC");
        public static readonly PacketId C2S_QUEST_QUEST_PROGRESS_REQ = new PacketId(11, 21, 1, "C2S_QUEST_QUEST_PROGRESS_REQ");
        public static readonly PacketId S2C_QUEST_QUEST_PROGRESS_RES = new PacketId(11, 21, 2, "S2C_QUEST_QUEST_PROGRESS_RES"); // クエスト進行に
        public static readonly PacketId S2C_QUEST_11_21_16_NTC = new PacketId(11, 21, 16, "S2C_QUEST_11_21_16_NTC");
        public static readonly PacketId C2S_QUEST_LEADER_QUEST_PROGRESS_REQUEST_REQ = new PacketId(11, 22, 1, "C2S_QUEST_LEADER_QUEST_PROGRESS_REQUEST_REQ");
        public static readonly PacketId S2C_QUEST_LEADER_QUEST_PROGRESS_REQUEST_RES = new PacketId(11, 22, 2, "S2C_QUEST_LEADER_QUEST_PROGRESS_REQUEST_RES"); // リーダーへのクエスト進行要求に
        public static readonly PacketId S2C_QUEST_11_22_16_NTC = new PacketId(11, 22, 16, "S2C_QUEST_11_22_16_NTC");
        public static readonly PacketId C2S_QUEST_LIGHT_QUEST_GP_COMPLETE_REQ = new PacketId(11, 23, 1, "C2S_QUEST_LIGHT_QUEST_GP_COMPLETE_REQ");
        public static readonly PacketId S2C_QUEST_LIGHT_QUEST_GP_COMPLETE_RES = new PacketId(11, 23, 2, "S2C_QUEST_LIGHT_QUEST_GP_COMPLETE_RES"); // ボードクエスト課金即時クリア応答
        public static readonly PacketId C2S_QUEST_CHECK_QUEST_DISTRIBUTION_REQ = new PacketId(11, 24, 1, "C2S_QUEST_CHECK_QUEST_DISTRIBUTION_REQ");
        public static readonly PacketId S2C_QUEST_CHECK_QUEST_DISTRIBUTION_RES = new PacketId(11, 24, 2, "S2C_QUEST_CHECK_QUEST_DISTRIBUTION_RES"); // クエスト配信チェックに
        public static readonly PacketId C2S_QUEST_QUEST_CANCEL_REQ = new PacketId(11, 25, 1, "C2S_QUEST_QUEST_CANCEL_REQ");
        public static readonly PacketId S2C_QUEST_QUEST_CANCEL_RES = new PacketId(11, 25, 2, "S2C_QUEST_QUEST_CANCEL_RES"); // クエストキャンセルに
        public static readonly PacketId S2C_QUEST_11_25_16_NTC = new PacketId(11, 25, 16, "S2C_QUEST_11_25_16_NTC");
        public static readonly PacketId S2C_QUEST_11_26_16_NTC = new PacketId(11, 26, 16, "S2C_QUEST_11_26_16_NTC");
        public static readonly PacketId S2C_QUEST_11_27_16_NTC = new PacketId(11, 27, 16, "S2C_QUEST_11_27_16_NTC");
        public static readonly PacketId C2S_QUEST_END_DISTRIBUTION_QUEST_CANCEL_REQ = new PacketId(11, 28, 1, "C2S_QUEST_END_DISTRIBUTION_QUEST_CANCEL_REQ");
        public static readonly PacketId S2C_QUEST_END_DISTRIBUTION_QUEST_CANCEL_RES = new PacketId(11, 28, 2, "S2C_QUEST_END_DISTRIBUTION_QUEST_CANCEL_RES"); // 配信終了クエストキャンセルに
        public static readonly PacketId C2S_QUEST_QUEST_COMPLETE_FLAG_CLEAR_REQ = new PacketId(11, 29, 1, "C2S_QUEST_QUEST_COMPLETE_FLAG_CLEAR_REQ");
        public static readonly PacketId S2C_QUEST_QUEST_COMPLETE_FLAG_CLEAR_RES = new PacketId(11, 29, 2, "S2C_QUEST_QUEST_COMPLETE_FLAG_CLEAR_RES"); // クエストクリアフラグクリアに
        public static readonly PacketId C2S_QUEST_GET_QUEST_DETAIL_LIST_REQ = new PacketId(11, 30, 1, "C2S_QUEST_GET_QUEST_DETAIL_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_QUEST_DETAIL_LIST_RES = new PacketId(11, 30, 2, "S2C_QUEST_GET_QUEST_DETAIL_LIST_RES"); // クエスト詳細情報の取得に
        public static readonly PacketId C2S_QUEST_GET_QUEST_COMPLETE_LIST_REQ = new PacketId(11, 31, 1, "C2S_QUEST_GET_QUEST_COMPLETE_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_QUEST_COMPLETE_LIST_RES = new PacketId(11, 31, 2, "S2C_QUEST_GET_QUEST_COMPLETE_LIST_RES"); // クエストクリアリストの取得に
        public static readonly PacketId C2S_QUEST_SET_PRIORITY_QUEST_REQ = new PacketId(11, 32, 1, "C2S_QUEST_SET_PRIORITY_QUEST_REQ");
        public static readonly PacketId S2C_QUEST_SET_PRIORITY_QUEST_RES = new PacketId(11, 32, 2, "S2C_QUEST_SET_PRIORITY_QUEST_RES"); // 優先クエストの設定に
        public static readonly PacketId S2C_QUEST_11_32_16_NTC = new PacketId(11, 32, 16, "S2C_QUEST_11_32_16_NTC");
        public static readonly PacketId C2S_QUEST_CANCEL_PRIORITY_QUEST_REQ = new PacketId(11, 33, 1, "C2S_QUEST_CANCEL_PRIORITY_QUEST_REQ");
        public static readonly PacketId S2C_QUEST_CANCEL_PRIORITY_QUEST_RES = new PacketId(11, 33, 2, "S2C_QUEST_CANCEL_PRIORITY_QUEST_RES"); // 優先クエストの解除に
        public static readonly PacketId C2S_QUEST_GET_PRIORITY_QUEST_REQ = new PacketId(11, 34, 1, "C2S_QUEST_GET_PRIORITY_QUEST_REQ");
        public static readonly PacketId S2C_QUEST_GET_PRIORITY_QUEST_RES = new PacketId(11, 34, 2, "S2C_QUEST_GET_PRIORITY_QUEST_RES"); // 優先クエストの取得に
        public static readonly PacketId C2S_QUEST_GET_SET_QUEST_OPEN_DATE_LIST_REQ = new PacketId(11, 35, 1, "C2S_QUEST_GET_SET_QUEST_OPEN_DATE_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_SET_QUEST_OPEN_DATE_LIST_RES = new PacketId(11, 35, 2, "S2C_QUEST_GET_SET_QUEST_OPEN_DATE_LIST_RES"); // セットクエストの公開日時リストの取得に
        public static readonly PacketId C2S_QUEST_GET_QUEST_LAYOUT_FLAG_REQ = new PacketId(11, 36, 1, "C2S_QUEST_GET_QUEST_LAYOUT_FLAG_REQ");
        public static readonly PacketId S2C_QUEST_GET_QUEST_LAYOUT_FLAG_RES = new PacketId(11, 36, 2, "S2C_QUEST_GET_QUEST_LAYOUT_FLAG_RES"); // クエストレイアウトフラグの取得に
        public static readonly PacketId C2S_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_REQ = new PacketId(11, 37, 1, "C2S_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_RES = new PacketId(11, 37, 2, "S2C_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_RES"); // 循環コンテンツ状態リストの取得に
        public static readonly PacketId C2S_QUEST_GET_CYCLE_CONTENTS_SITUATION_INFO_LIST_REQ = new PacketId(11, 38, 1, "C2S_QUEST_GET_CYCLE_CONTENTS_SITUATION_INFO_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_CYCLE_CONTENTS_SITUATION_INFO_LIST_RES = new PacketId(11, 38, 2, "S2C_QUEST_GET_CYCLE_CONTENTS_SITUATION_INFO_LIST_RES"); // 循環コンテンツ本編情報リストの取得に
        public static readonly PacketId C2S_QUEST_PLAY_ENTRY_REQ = new PacketId(11, 39, 1, "C2S_QUEST_PLAY_ENTRY_REQ");
        public static readonly PacketId S2C_QUEST_PLAY_ENTRY_RES = new PacketId(11, 39, 2, "S2C_QUEST_PLAY_ENTRY_RES"); // プレイエントリーに
        public static readonly PacketId S2C_QUEST_11_39_16_NTC = new PacketId(11, 39, 16, "S2C_QUEST_11_39_16_NTC");
        public static readonly PacketId C2S_QUEST_11_40_1_REQ = new PacketId(11, 40, 1, "C2S_QUEST_11_40_1_REQ");
        public static readonly PacketId S2C_QUEST_11_40_2_RES = new PacketId(11, 40, 2, "S2C_QUEST_11_40_2_RES");
        public static readonly PacketId S2C_QUEST_11_40_16_NTC = new PacketId(11, 40, 16, "S2C_QUEST_11_40_16_NTC");
        public static readonly PacketId C2S_QUEST_PLAY_START_REQ = new PacketId(11, 41, 1, "C2S_QUEST_PLAY_START_REQ");
        public static readonly PacketId S2C_QUEST_PLAY_START_RES = new PacketId(11, 41, 2, "S2C_QUEST_PLAY_START_RES"); // プレイスタートに
        public static readonly PacketId C2S_QUEST_PLAY_START_TIMER_REQ = new PacketId(11, 42, 1, "C2S_QUEST_PLAY_START_TIMER_REQ");
        public static readonly PacketId S2C_QUEST_PLAY_START_TIMER_RES = new PacketId(11, 42, 2, "S2C_QUEST_PLAY_START_TIMER_RES"); // クエスト時間計測開始に
        public static readonly PacketId S2C_QUEST_11_42_16_NTC = new PacketId(11, 42, 16, "S2C_QUEST_11_42_16_NTC");
        public static readonly PacketId C2S_QUEST_PLAY_INTERRUPT_REQ = new PacketId(11, 43, 1, "C2S_QUEST_PLAY_INTERRUPT_REQ");
        public static readonly PacketId S2C_QUEST_PLAY_INTERRUPT_RES = new PacketId(11, 43, 2, "S2C_QUEST_PLAY_INTERRUPT_RES"); // コンテンツプレイ中断要求に
        public static readonly PacketId S2C_QUEST_11_43_16_NTC = new PacketId(11, 43, 16, "S2C_QUEST_11_43_16_NTC");
        public static readonly PacketId C2S_QUEST_PLAY_INTERRUPT_ANSWER_REQ = new PacketId(11, 44, 1, "C2S_QUEST_PLAY_INTERRUPT_ANSWER_REQ");
        public static readonly PacketId S2C_QUEST_PLAY_INTERRUPT_ANSWER_RES = new PacketId(11, 44, 2, "S2C_QUEST_PLAY_INTERRUPT_ANSWER_RES"); // コンテンツプレイ中断応答に
        public static readonly PacketId C2S_QUEST_PLAY_END_REQ = new PacketId(11, 45, 1, "C2S_QUEST_PLAY_END_REQ");
        public static readonly PacketId S2C_QUEST_PLAY_END_RES = new PacketId(11, 45, 2, "S2C_QUEST_PLAY_END_RES"); // エンドコンテンツ終了に
        public static readonly PacketId S2C_QUEST_11_45_16_NTC = new PacketId(11, 45, 16, "S2C_QUEST_11_45_16_NTC");
        public static readonly PacketId C2S_QUEST_CYCLE_CONTENTS_PLAY_START_REQ = new PacketId(11, 46, 1, "C2S_QUEST_CYCLE_CONTENTS_PLAY_START_REQ");
        public static readonly PacketId S2C_QUEST_CYCLE_CONTENTS_PLAY_START_RES = new PacketId(11, 46, 2, "S2C_QUEST_CYCLE_CONTENTS_PLAY_START_RES"); // 循環コンテンツプレイスタートに
        public static readonly PacketId C2S_QUEST_CYCLE_CONTENTS_PLAY_END_REQ = new PacketId(11, 47, 1, "C2S_QUEST_CYCLE_CONTENTS_PLAY_END_REQ");
        public static readonly PacketId S2C_QUEST_CYCLE_CONTENTS_PLAY_END_RES = new PacketId(11, 47, 2, "S2C_QUEST_CYCLE_CONTENTS_PLAY_END_RES"); // 循環コンテンツ終了に
        public static readonly PacketId S2C_QUEST_11_47_16_NTC = new PacketId(11, 47, 16, "S2C_QUEST_11_47_16_NTC");
        public static readonly PacketId C2S_QUEST_GET_CYCLE_CONTENTS_POINT_LIST_REQ = new PacketId(11, 48, 1, "C2S_QUEST_GET_CYCLE_CONTENTS_POINT_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_CYCLE_CONTENTS_POINT_LIST_RES = new PacketId(11, 48, 2, "S2C_QUEST_GET_CYCLE_CONTENTS_POINT_LIST_RES"); // 循環コンテンツポイントリスト取得に
        public static readonly PacketId C2S_QUEST_GET_CYCLE_CONTENTS_NOW_POINT_LIST_REQ = new PacketId(11, 49, 1, "C2S_QUEST_GET_CYCLE_CONTENTS_NOW_POINT_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_CYCLE_CONTENTS_NOW_POINT_LIST_RES = new PacketId(11, 49, 2, "S2C_QUEST_GET_CYCLE_CONTENTS_NOW_POINT_LIST_RES"); // 循環コンテンツ現在ポイントリスト取得に
        public static readonly PacketId C2S_QUEST_GET_CYCLE_CONTENTS_BORDER_REWARD_LIST_REQ = new PacketId(11, 50, 1, "C2S_QUEST_GET_CYCLE_CONTENTS_BORDER_REWARD_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_CYCLE_CONTENTS_BORDER_REWARD_LIST_RES = new PacketId(11, 50, 2, "S2C_QUEST_GET_CYCLE_CONTENTS_BORDER_REWARD_LIST_RES"); // 循環コンテンツボーダー報酬リスト取得に
        public static readonly PacketId C2S_QUEST_GET_CYCLE_CONTENTS_RANKING_REWARD_LIST_REQ = new PacketId(11, 51, 1, "C2S_QUEST_GET_CYCLE_CONTENTS_RANKING_REWARD_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_CYCLE_CONTENTS_RANKING_REWARD_LIST_RES = new PacketId(11, 51, 2, "S2C_QUEST_GET_CYCLE_CONTENTS_RANKING_REWARD_LIST_RES"); // 循環コンテンツランキング報酬リスト取得に
        public static readonly PacketId C2S_QUEST_GET_CYCLE_CONTENTS_REWARD_REQ = new PacketId(11, 52, 1, "C2S_QUEST_GET_CYCLE_CONTENTS_REWARD_REQ");
        public static readonly PacketId S2C_QUEST_GET_CYCLE_CONTENTS_REWARD_RES = new PacketId(11, 52, 2, "S2C_QUEST_GET_CYCLE_CONTENTS_REWARD_RES"); // 循環コンテンツ報酬受け取りに
        public static readonly PacketId C2S_QUEST_11_53_1_REQ = new PacketId(11, 53, 1, "C2S_QUEST_11_53_1_REQ");
        public static readonly PacketId S2C_QUEST_11_53_2_RES = new PacketId(11, 53, 2, "S2C_QUEST_11_53_2_RES");
        public static readonly PacketId C2S_QUEST_11_54_1_REQ = new PacketId(11, 54, 1, "C2S_QUEST_11_54_1_REQ");
        public static readonly PacketId S2C_QUEST_11_54_2_RES = new PacketId(11, 54, 2, "S2C_QUEST_11_54_2_RES");
        public static readonly PacketId C2S_QUEST_GP_CYCLE_CONTENTS_ROULETTE_REQ = new PacketId(11, 55, 1, "C2S_QUEST_GP_CYCLE_CONTENTS_ROULETTE_REQ");
        public static readonly PacketId S2C_QUEST_GP_CYCLE_CONTENTS_ROULETTE_RES = new PacketId(11, 55, 2, "S2C_QUEST_GP_CYCLE_CONTENTS_ROULETTE_RES"); // 循環コンテンツルーレット課金応答
        public static readonly PacketId C2S_QUEST_GET_AREA_INFO_LIST_REQ = new PacketId(11, 56, 1, "C2S_QUEST_GET_AREA_INFO_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_AREA_INFO_LIST_RES = new PacketId(11, 56, 2, "S2C_QUEST_GET_AREA_INFO_LIST_RES"); // 新聞用エリア情報リストの取得に
        public static readonly PacketId C2S_QUEST_GET_QUEST_PARTY_BONUS_LIST_REQ = new PacketId(11, 57, 1, "C2S_QUEST_GET_QUEST_PARTY_BONUS_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_QUEST_PARTY_BONUS_LIST_RES = new PacketId(11, 57, 2, "S2C_QUEST_GET_QUEST_PARTY_BONUS_LIST_RES"); // パーティボーナス対象クエストリスト取得に
        public static readonly PacketId C2S_QUEST_SEND_LEADER_QUEST_ORDER_CONDITION_INFO_REQ = new PacketId(11, 58, 1, "C2S_QUEST_SEND_LEADER_QUEST_ORDER_CONDITION_INFO_REQ");
        public static readonly PacketId S2C_QUEST_SEND_LEADER_QUEST_ORDER_CONDITION_INFO_RES = new PacketId(11, 58, 2, "S2C_QUEST_SEND_LEADER_QUEST_ORDER_CONDITION_INFO_RES"); // リーダの受注条件送信に
        public static readonly PacketId S2C_QUEST_11_58_16_NTC = new PacketId(11, 58, 16, "S2C_QUEST_11_58_16_NTC");
        public static readonly PacketId C2S_QUEST_SEND_LEADER_WAIT_ORDER_QUEST_LIST_REQ = new PacketId(11, 59, 1, "C2S_QUEST_SEND_LEADER_WAIT_ORDER_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_SEND_LEADER_WAIT_ORDER_QUEST_LIST_RES = new PacketId(11, 59, 2, "S2C_QUEST_SEND_LEADER_WAIT_ORDER_QUEST_LIST_RES"); // リーダの受注待ちクエスト送信に
        public static readonly PacketId S2C_QUEST_11_59_16_NTC = new PacketId(11, 59, 16, "S2C_QUEST_11_59_16_NTC");
        public static readonly PacketId C2S_QUEST_QUEST_LOG_INFO_REQ = new PacketId(11, 61, 1, "C2S_QUEST_QUEST_LOG_INFO_REQ");
        public static readonly PacketId S2C_QUEST_QUEST_LOG_INFO_RES = new PacketId(11, 61, 2, "S2C_QUEST_QUEST_LOG_INFO_RES"); // クエストログ通知に
        public static readonly PacketId C2S_QUEST_GET_REWARD_BOX_LIST_REQ = new PacketId(11, 62, 1, "C2S_QUEST_GET_REWARD_BOX_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_REWARD_BOX_LIST_RES = new PacketId(11, 62, 2, "S2C_QUEST_GET_REWARD_BOX_LIST_RES"); // 報酬ボックスリスト取得に
        public static readonly PacketId C2S_QUEST_GET_REWARD_BOX_LIST_NUM_REQ = new PacketId(11, 63, 1, "C2S_QUEST_GET_REWARD_BOX_LIST_NUM_REQ");
        public static readonly PacketId S2C_QUEST_GET_REWARD_BOX_LIST_NUM_RES = new PacketId(11, 63, 2, "S2C_QUEST_GET_REWARD_BOX_LIST_NUM_RES"); // 報酬ボックスリスト数の取得に
        public static readonly PacketId C2S_QUEST_GET_REWARD_BOX_ITEM_REQ = new PacketId(11, 64, 1, "C2S_QUEST_GET_REWARD_BOX_ITEM_REQ");
        public static readonly PacketId S2C_QUEST_GET_REWARD_BOX_ITEM_RES = new PacketId(11, 64, 2, "S2C_QUEST_GET_REWARD_BOX_ITEM_RES"); // 報酬ボックスアイテムの受け取りに
        public static readonly PacketId C2S_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_REQ = new PacketId(11, 65, 1, "C2S_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_RES = new PacketId(11, 65, 2, "S2C_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_RES"); // 循環コンテンツ報酬リスト取得に
        public static readonly PacketId C2S_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_NUM_REQ = new PacketId(11, 66, 1, "C2S_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_NUM_REQ");
        public static readonly PacketId S2C_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_NUM_RES = new PacketId(11, 66, 2, "S2C_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_NUM_RES"); // 循環コンテンツ報酬リスト数の取得に
        public static readonly PacketId C2S_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_ITEM_REQ = new PacketId(11, 67, 1, "C2S_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_ITEM_REQ");
        public static readonly PacketId S2C_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_ITEM_RES = new PacketId(11, 67, 2, "S2C_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_ITEM_RES"); // 循環コンテンツ報酬アイテムの受け取りに
        public static readonly PacketId S2C_QUEST_11_68_16_NTC = new PacketId(11, 68, 16, "S2C_QUEST_11_68_16_NTC");
        public static readonly PacketId C2S_QUEST_DELIVER_ITEM_REQ = new PacketId(11, 69, 1, "C2S_QUEST_DELIVER_ITEM_REQ");
        public static readonly PacketId S2C_QUEST_DELIVER_ITEM_RES = new PacketId(11, 69, 2, "S2C_QUEST_DELIVER_ITEM_RES"); // 納品に
        public static readonly PacketId S2C_QUEST_11_69_16_NTC = new PacketId(11, 69, 16, "S2C_QUEST_11_69_16_NTC");
        public static readonly PacketId C2S_QUEST_DECIDE_DELIVERY_ITEM_REQ = new PacketId(11, 70, 1, "C2S_QUEST_DECIDE_DELIVERY_ITEM_REQ");
        public static readonly PacketId S2C_QUEST_DECIDE_DELIVERY_ITEM_RES = new PacketId(11, 70, 2, "S2C_QUEST_DECIDE_DELIVERY_ITEM_RES"); // 納品決定に
        public static readonly PacketId S2C_QUEST_11_70_16_NTC = new PacketId(11, 70, 16, "S2C_QUEST_11_70_16_NTC");
        public static readonly PacketId C2S_QUEST_GET_PARTY_QUEST_PROGRESS_INFO_REQ = new PacketId(11, 71, 1, "C2S_QUEST_GET_PARTY_QUEST_PROGRESS_INFO_REQ");
        public static readonly PacketId S2C_QUEST_GET_PARTY_QUEST_PROGRESS_INFO_RES = new PacketId(11, 71, 2, "S2C_QUEST_GET_PARTY_QUEST_PROGRESS_INFO_RES"); // パーティークエスト進行情報取得に
        public static readonly PacketId C2S_QUEST_DEBUG_MAIN_QUEST_JUMP_REQ = new PacketId(11, 73, 1, "C2S_QUEST_DEBUG_MAIN_QUEST_JUMP_REQ");
        public static readonly PacketId S2C_QUEST_DEBUG_MAIN_QUEST_JUMP_RES = new PacketId(11, 73, 2, "S2C_QUEST_DEBUG_MAIN_QUEST_JUMP_RES"); // (デバッグ用)メインクエストジャンプに
        public static readonly PacketId S2C_QUEST_11_73_16_NTC = new PacketId(11, 73, 16, "S2C_QUEST_11_73_16_NTC");
        public static readonly PacketId C2S_QUEST_DEBUG_QUEST_RESET_REQ = new PacketId(11, 74, 1, "C2S_QUEST_DEBUG_QUEST_RESET_REQ");
        public static readonly PacketId S2C_QUEST_DEBUG_QUEST_RESET_RES = new PacketId(11, 74, 2, "S2C_QUEST_DEBUG_QUEST_RESET_RES"); // (デバッグ用)クエストリセットに
        public static readonly PacketId C2S_QUEST_DEBUG_QUEST_RESET_ALL_REQ = new PacketId(11, 75, 1, "C2S_QUEST_DEBUG_QUEST_RESET_ALL_REQ");
        public static readonly PacketId S2C_QUEST_DEBUG_QUEST_RESET_ALL_RES = new PacketId(11, 75, 2, "S2C_QUEST_DEBUG_QUEST_RESET_ALL_RES"); // (デバッグ用)クエスト全リセットに
        public static readonly PacketId C2S_QUEST_DEBUG_CYCLE_CONTENTS_POINT_UPLOAD_REQ = new PacketId(11, 76, 1, "C2S_QUEST_DEBUG_CYCLE_CONTENTS_POINT_UPLOAD_REQ");
        public static readonly PacketId S2C_QUEST_DEBUG_CYCLE_CONTENTS_POINT_UPLOAD_RES = new PacketId(11, 76, 2, "S2C_QUEST_DEBUG_CYCLE_CONTENTS_POINT_UPLOAD_RES"); // (デバッグ用)循環コンテンツ成果ポイントのアップロードに
        public static readonly PacketId C2S_QUEST_11_77_1_REQ = new PacketId(11, 77, 1, "C2S_QUEST_11_77_1_REQ");
        public static readonly PacketId S2C_QUEST_11_77_2_RES = new PacketId(11, 77, 2, "S2C_QUEST_11_77_2_RES");
        public static readonly PacketId C2S_QUEST_11_78_1_REQ = new PacketId(11, 78, 1, "C2S_QUEST_11_78_1_REQ");
        public static readonly PacketId S2C_QUEST_11_78_2_RES = new PacketId(11, 78, 2, "S2C_QUEST_11_78_2_RES");
        public static readonly PacketId C2S_QUEST_11_79_1_REQ = new PacketId(11, 79, 1, "C2S_QUEST_11_79_1_REQ");
        public static readonly PacketId S2C_QUEST_11_79_2_RES = new PacketId(11, 79, 2, "S2C_QUEST_11_79_2_RES");
        public static readonly PacketId C2S_QUEST_DEBUG_ENEMY_SET_PRESET_FIX_REQ = new PacketId(11, 80, 1, "C2S_QUEST_DEBUG_ENEMY_SET_PRESET_FIX_REQ");
        public static readonly PacketId S2C_QUEST_DEBUG_ENEMY_SET_PRESET_FIX_RES = new PacketId(11, 80, 2, "S2C_QUEST_DEBUG_ENEMY_SET_PRESET_FIX_RES"); // (デバッグ用)敵セットプリセット固定に
        public static readonly PacketId S2C_QUEST_11_82_16_NTC = new PacketId(11, 82, 16, "S2C_QUEST_11_82_16_NTC");
        public static readonly PacketId S2C_QUEST_11_83_16_NTC = new PacketId(11, 83, 16, "S2C_QUEST_11_83_16_NTC");
        public static readonly PacketId S2C_QUEST_11_84_16_NTC = new PacketId(11, 84, 16, "S2C_QUEST_11_84_16_NTC");
        public static readonly PacketId S2C_QUEST_11_85_16_NTC = new PacketId(11, 85, 16, "S2C_QUEST_11_85_16_NTC");
        public static readonly PacketId S2C_QUEST_11_86_16_NTC = new PacketId(11, 86, 16, "S2C_QUEST_11_86_16_NTC");
        public static readonly PacketId S2C_QUEST_11_87_16_NTC = new PacketId(11, 87, 16, "S2C_QUEST_11_87_16_NTC");
        public static readonly PacketId S2C_QUEST_11_88_16_NTC = new PacketId(11, 88, 16, "S2C_QUEST_11_88_16_NTC");
        public static readonly PacketId S2C_QUEST_11_89_16_NTC = new PacketId(11, 89, 16, "S2C_QUEST_11_89_16_NTC");
        public static readonly PacketId S2C_QUEST_11_90_16_NTC = new PacketId(11, 90, 16, "S2C_QUEST_11_90_16_NTC");
        public static readonly PacketId S2C_QUEST_11_91_16_NTC = new PacketId(11, 91, 16, "S2C_QUEST_11_91_16_NTC");
        public static readonly PacketId S2C_QUEST_11_92_16_NTC = new PacketId(11, 92, 16, "S2C_QUEST_11_92_16_NTC");
        public static readonly PacketId S2C_QUEST_11_93_16_NTC = new PacketId(11, 93, 16, "S2C_QUEST_11_93_16_NTC");
        public static readonly PacketId S2C_QUEST_11_94_16_NTC = new PacketId(11, 94, 16, "S2C_QUEST_11_94_16_NTC");
        public static readonly PacketId S2C_QUEST_11_95_16_NTC = new PacketId(11, 95, 16, "S2C_QUEST_11_95_16_NTC");
        public static readonly PacketId S2C_QUEST_11_96_16_NTC = new PacketId(11, 96, 16, "S2C_QUEST_11_96_16_NTC");
        public static readonly PacketId S2C_QUEST_11_97_16_NTC = new PacketId(11, 97, 16, "S2C_QUEST_11_97_16_NTC");
        public static readonly PacketId S2C_QUEST_11_98_16_NTC = new PacketId(11, 98, 16, "S2C_QUEST_11_98_16_NTC");
        public static readonly PacketId S2C_QUEST_11_99_16_NTC = new PacketId(11, 99, 16, "S2C_QUEST_11_99_16_NTC");
        public static readonly PacketId S2C_QUEST_11_100_16_NTC = new PacketId(11, 100, 16, "S2C_QUEST_11_100_16_NTC");
        public static readonly PacketId S2C_QUEST_11_101_16_NTC = new PacketId(11, 101, 16, "S2C_QUEST_11_101_16_NTC");
        public static readonly PacketId S2C_QUEST_11_102_16_NTC = new PacketId(11, 102, 16, "S2C_QUEST_11_102_16_NTC");
        public static readonly PacketId S2C_QUEST_11_103_16_NTC = new PacketId(11, 103, 16, "S2C_QUEST_11_103_16_NTC");
        public static readonly PacketId S2C_QUEST_11_104_16_NTC = new PacketId(11, 104, 16, "S2C_QUEST_11_104_16_NTC");
        public static readonly PacketId S2C_QUEST_11_105_16_NTC = new PacketId(11, 105, 16, "S2C_QUEST_11_105_16_NTC");
        public static readonly PacketId S2C_QUEST_11_106_16_NTC = new PacketId(11, 106, 16, "S2C_QUEST_11_106_16_NTC");
        public static readonly PacketId S2C_QUEST_11_107_16_NTC = new PacketId(11, 107, 16, "S2C_QUEST_11_107_16_NTC");
        public static readonly PacketId S2C_QUEST_11_108_16_NTC = new PacketId(11, 108, 16, "S2C_QUEST_11_108_16_NTC");
        public static readonly PacketId S2C_QUEST_11_109_16_NTC = new PacketId(11, 109, 16, "S2C_QUEST_11_109_16_NTC");
        public static readonly PacketId S2C_QUEST_11_110_16_NTC = new PacketId(11, 110, 16, "S2C_QUEST_11_110_16_NTC");
        public static readonly PacketId S2C_QUEST_11_111_16_NTC = new PacketId(11, 111, 16, "S2C_QUEST_11_111_16_NTC");
        public static readonly PacketId S2C_QUEST_11_112_16_NTC = new PacketId(11, 112, 16, "S2C_QUEST_11_112_16_NTC");
        public static readonly PacketId S2C_QUEST_11_113_16_NTC = new PacketId(11, 113, 16, "S2C_QUEST_11_113_16_NTC");
        public static readonly PacketId S2C_QUEST_11_114_16_NTC = new PacketId(11, 114, 16, "S2C_QUEST_11_114_16_NTC");
        public static readonly PacketId S2C_QUEST_11_115_16_NTC = new PacketId(11, 115, 16, "S2C_QUEST_11_115_16_NTC");
        public static readonly PacketId S2C_QUEST_11_116_16_NTC = new PacketId(11, 116, 16, "S2C_QUEST_11_116_16_NTC");
        public static readonly PacketId S2C_QUEST_11_117_16_NTC = new PacketId(11, 117, 16, "S2C_QUEST_11_117_16_NTC");
        public static readonly PacketId S2C_QUEST_11_118_16_NTC = new PacketId(11, 118, 16, "S2C_QUEST_11_118_16_NTC");
        public static readonly PacketId S2C_QUEST_11_119_16_NTC = new PacketId(11, 119, 16, "S2C_QUEST_11_119_16_NTC");
        public static readonly PacketId S2C_QUEST_11_120_16_NTC = new PacketId(11, 120, 16, "S2C_QUEST_11_120_16_NTC");
        public static readonly PacketId C2S_QUEST_GET_LEVEL_BONUS_LIST_REQ = new PacketId(11, 121, 1, "C2S_QUEST_GET_LEVEL_BONUS_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_LEVEL_BONUS_LIST_RES = new PacketId(11, 121, 2, "S2C_QUEST_GET_LEVEL_BONUS_LIST_RES"); // レベルボーナス情報リストの取得に
        public static readonly PacketId C2S_QUEST_GET_ADVENTURE_GUIDE_QUEST_LIST_REQ = new PacketId(11, 122, 1, "C2S_QUEST_GET_ADVENTURE_GUIDE_QUEST_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_ADVENTURE_GUIDE_QUEST_LIST_RES = new PacketId(11, 122, 2, "S2C_QUEST_GET_ADVENTURE_GUIDE_QUEST_LIST_RES"); // 冒険ガイドクエストの取得に
        public static readonly PacketId C2S_QUEST_GET_ADVENTURE_GUIDE_QUEST_NOTICE_REQ = new PacketId(11, 123, 1, "C2S_QUEST_GET_ADVENTURE_GUIDE_QUEST_NOTICE_REQ");
        public static readonly PacketId S2C_QUEST_GET_ADVENTURE_GUIDE_QUEST_NOTICE_RES = new PacketId(11, 123, 2, "S2C_QUEST_GET_ADVENTURE_GUIDE_QUEST_NOTICE_RES"); // 冒険ガイドクエスト通知の取得に
        public static readonly PacketId C2S_QUEST_SET_NAVIGATION_QUEST_REQ = new PacketId(11, 124, 1, "C2S_QUEST_SET_NAVIGATION_QUEST_REQ");
        public static readonly PacketId S2C_QUEST_SET_NAVIGATION_QUEST_RES = new PacketId(11, 124, 2, "S2C_QUEST_SET_NAVIGATION_QUEST_RES"); // ナビゲーションクエストセットに
        public static readonly PacketId S2C_QUEST_11_124_16_NTC = new PacketId(11, 124, 16, "S2C_QUEST_11_124_16_NTC");
        public static readonly PacketId C2S_QUEST_CANCEL_NAVIGATION_QUEST_REQ = new PacketId(11, 125, 1, "C2S_QUEST_CANCEL_NAVIGATION_QUEST_REQ");
        public static readonly PacketId S2C_QUEST_CANCEL_NAVIGATION_QUEST_RES = new PacketId(11, 125, 2, "S2C_QUEST_CANCEL_NAVIGATION_QUEST_RES"); // ナビゲーションクエストのキャンセルに
        public static readonly PacketId S2C_QUEST_11_125_16_NTC = new PacketId(11, 125, 16, "S2C_QUEST_11_125_16_NTC");
        public static readonly PacketId S2C_QUEST_11_126_16_NTC = new PacketId(11, 126, 16, "S2C_QUEST_11_126_16_NTC");
        public static readonly PacketId C2S_QUEST_GET_END_CONTENTS_RECRUIT_LIST_REQ = new PacketId(11, 127, 1, "C2S_QUEST_GET_END_CONTENTS_RECRUIT_LIST_REQ");
        public static readonly PacketId S2C_QUEST_GET_END_CONTENTS_RECRUIT_LIST_RES = new PacketId(11, 127, 2, "S2C_QUEST_GET_END_CONTENTS_RECRUIT_LIST_RES"); // エンドコンテンツ募集リストの取得に

// Group: 12 - (STAGE)
        public static readonly PacketId C2S_STAGE_GET_STAGE_LIST_REQ = new PacketId(12, 0, 1, "C2S_STAGE_GET_STAGE_LIST_REQ");
        public static readonly PacketId S2C_STAGE_GET_STAGE_LIST_RES = new PacketId(12, 0, 2, "S2C_STAGE_GET_STAGE_LIST_RES"); // ステージリスト取得に
        public static readonly PacketId C2S_STAGE_AREA_CHANGE_REQ = new PacketId(12, 1, 1, "C2S_STAGE_AREA_CHANGE_REQ");
        public static readonly PacketId S2C_STAGE_AREA_CHANGE_RES = new PacketId(12, 1, 2, "S2C_STAGE_AREA_CHANGE_RES"); // エリアチェンジリクエストに
        public static readonly PacketId C2S_STAGE_GET_SP_AREA_CHANGE_ID_FROM_NPC_ID_REQ = new PacketId(12, 2, 1, "C2S_STAGE_GET_SP_AREA_CHANGE_ID_FROM_NPC_ID_REQ");
        public static readonly PacketId S2C_STAGE_GET_SP_AREA_CHANGE_ID_FROM_NPC_ID_RES = new PacketId(12, 2, 2, "S2C_STAGE_GET_SP_AREA_CHANGE_ID_FROM_NPC_ID_RES"); // 特殊ステージ移動ID取得に
        public static readonly PacketId C2S_STAGE_GET_SP_AREA_CHANGE_LIST_FROM_OM_REQ = new PacketId(12, 3, 1, "C2S_STAGE_GET_SP_AREA_CHANGE_LIST_FROM_OM_REQ");
        public static readonly PacketId S2C_STAGE_GET_SP_AREA_CHANGE_LIST_FROM_OM_RES = new PacketId(12, 3, 2, "S2C_STAGE_GET_SP_AREA_CHANGE_LIST_FROM_OM_RES"); // 特殊ステージ移動リスト取得に
        public static readonly PacketId C2S_STAGE_GET_SP_AREA_CHANGE_INFO_REQ = new PacketId(12, 4, 1, "C2S_STAGE_GET_SP_AREA_CHANGE_INFO_REQ");
        public static readonly PacketId S2C_STAGE_GET_SP_AREA_CHANGE_INFO_RES = new PacketId(12, 4, 2, "S2C_STAGE_GET_SP_AREA_CHANGE_INFO_RES"); // 特殊ステージ移動情報取得に
        public static readonly PacketId C2S_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_REQ = new PacketId(12, 5, 1, "C2S_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_REQ");
        public static readonly PacketId S2C_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_RES = new PacketId(12, 5, 2, "S2C_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_RES"); // 一斉ステージ移動待ち受け開始に
        public static readonly PacketId C2S_STAGE_UNISON_AREA_CHANGE_GET_RECRUITMENT_STATE_REQ = new PacketId(12, 6, 1, "C2S_STAGE_UNISON_AREA_CHANGE_GET_RECRUITMENT_STATE_REQ");
        public static readonly PacketId S2C_STAGE_UNISON_AREA_CHANGE_GET_RECRUITMENT_STATE_RES = new PacketId(12, 6, 2, "S2C_STAGE_UNISON_AREA_CHANGE_GET_RECRUITMENT_STATE_RES"); // 一斉ステージ移動待ち受け状態取得に
        public static readonly PacketId C2S_STAGE_UNISON_AREA_CHANGE_READY_REQ = new PacketId(12, 7, 1, "C2S_STAGE_UNISON_AREA_CHANGE_READY_REQ");
        public static readonly PacketId S2C_STAGE_UNISON_AREA_CHANGE_READY_RES = new PacketId(12, 7, 2, "S2C_STAGE_UNISON_AREA_CHANGE_READY_RES"); // 一斉ステージ移動準備に
        public static readonly PacketId C2S_STAGE_UNISON_AREA_CHANGE_READY_CANCEL_REQ = new PacketId(12, 8, 1, "C2S_STAGE_UNISON_AREA_CHANGE_READY_CANCEL_REQ");
        public static readonly PacketId S2C_STAGE_UNISON_AREA_CHANGE_READY_CANCEL_RES = new PacketId(12, 8, 2, "S2C_STAGE_UNISON_AREA_CHANGE_READY_CANCEL_RES"); // 一斉ステージ移動準備キャンセルに
        public static readonly PacketId S2C_STAGE_12_9_16_NTC = new PacketId(12, 9, 16, "S2C_STAGE_12_9_16_NTC");
        public static readonly PacketId S2C_STAGE_12_10_16_NTC = new PacketId(12, 10, 16, "S2C_STAGE_12_10_16_NTC");
        public static readonly PacketId C2S_STAGE_GET_TICKET_DUNGEON_CATEGORY_LIST_REQ = new PacketId(12, 11, 1, "C2S_STAGE_GET_TICKET_DUNGEON_CATEGORY_LIST_REQ");
        public static readonly PacketId S2C_STAGE_GET_TICKET_DUNGEON_CATEGORY_LIST_RES = new PacketId(12, 11, 2, "S2C_STAGE_GET_TICKET_DUNGEON_CATEGORY_LIST_RES"); // チケットダンジョンカテゴリ取得に
        public static readonly PacketId C2S_STAGE_GET_TICKET_DUNGEON_INFO_LIST_REQ = new PacketId(12, 12, 1, "C2S_STAGE_GET_TICKET_DUNGEON_INFO_LIST_REQ");
        public static readonly PacketId S2C_STAGE_GET_TICKET_DUNGEON_INFO_LIST_RES = new PacketId(12, 12, 2, "S2C_STAGE_GET_TICKET_DUNGEON_INFO_LIST_RES"); // チケットダンジョン情報取得に
        public static readonly PacketId C2S_STAGE_IS_EXIST_APP_CHARACTER_REQ = new PacketId(12, 13, 1, "C2S_STAGE_IS_EXIST_APP_CHARACTER_REQ");
        public static readonly PacketId S2C_STAGE_IS_EXIST_APP_CHARACTER_RES = new PacketId(12, 13, 2, "S2C_STAGE_IS_EXIST_APP_CHARACTER_RES"); // DebugText

// Group: 13 - (INSTANCE)
        public static readonly PacketId C2S_INSTANCE_GET_ENEMY_SET_LIST_REQ = new PacketId(13, 0, 1, "C2S_INSTANCE_GET_ENEMY_SET_LIST_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_ENEMY_SET_LIST_RES = new PacketId(13, 0, 2, "S2C_INSTANCE_GET_ENEMY_SET_LIST_RES"); // 敵セットリスト取得に
        public static readonly PacketId C2S_INSTANCE_ENEMY_KILL_REQ = new PacketId(13, 3, 1, "C2S_INSTANCE_ENEMY_KILL_REQ");
        public static readonly PacketId S2C_INSTANCE_ENEMY_KILL_RES = new PacketId(13, 3, 2, "S2C_INSTANCE_ENEMY_KILL_RES"); // 敵死亡リクエスト結果に
        public static readonly PacketId C2S_INSTANCE_GET_ITEM_SET_LIST_REQ = new PacketId(13, 5, 1, "C2S_INSTANCE_GET_ITEM_SET_LIST_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_ITEM_SET_LIST_RES = new PacketId(13, 5, 2, "S2C_INSTANCE_GET_ITEM_SET_LIST_RES"); // アイテムセットリスト取得に
        public static readonly PacketId C2S_INSTANCE_GET_GATHERING_ITEM_LIST_REQ = new PacketId(13, 6, 1, "C2S_INSTANCE_GET_GATHERING_ITEM_LIST_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_GATHERING_ITEM_LIST_RES = new PacketId(13, 6, 2, "S2C_INSTANCE_GET_GATHERING_ITEM_LIST_RES"); // 採取アイテムリスト取得に
        public static readonly PacketId C2S_INSTANCE_GET_GATHERING_ITEM_REQ = new PacketId(13, 8, 1, "C2S_INSTANCE_GET_GATHERING_ITEM_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_GATHERING_ITEM_RES = new PacketId(13, 8, 2, "S2C_INSTANCE_GET_GATHERING_ITEM_RES"); // 採取アイテム取得に
        public static readonly PacketId C2S_INSTANCE_GET_DROP_ITEM_SET_LIST_REQ = new PacketId(13, 9, 1, "C2S_INSTANCE_GET_DROP_ITEM_SET_LIST_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_DROP_ITEM_SET_LIST_RES = new PacketId(13, 9, 2, "S2C_INSTANCE_GET_DROP_ITEM_SET_LIST_RES"); // ドロップアイテム配置リストを取得
        public static readonly PacketId C2S_INSTANCE_GET_DROP_ITEM_LIST_REQ = new PacketId(13, 10, 1, "C2S_INSTANCE_GET_DROP_ITEM_LIST_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_DROP_ITEM_LIST_RES = new PacketId(13, 10, 2, "S2C_INSTANCE_GET_DROP_ITEM_LIST_RES"); // ドロップアイテムリストを取得
        public static readonly PacketId C2S_INSTANCE_GET_DROP_ITEM_REQ = new PacketId(13, 11, 1, "C2S_INSTANCE_GET_DROP_ITEM_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_DROP_ITEM_RES = new PacketId(13, 11, 2, "S2C_INSTANCE_GET_DROP_ITEM_RES"); // ドロップアイテムを取得
        public static readonly PacketId C2S_INSTANCE_13_13_1_REQ = new PacketId(13, 13, 1, "C2S_INSTANCE_13_13_1_REQ");
        public static readonly PacketId S2C_INSTANCE_13_13_2_RES = new PacketId(13, 13, 2, "S2C_INSTANCE_13_13_2_RES");
        public static readonly PacketId C2S_INSTANCE_TRANING_ROOM_GET_ENEMY_LIST_REQ = new PacketId(13, 14, 1, "C2S_INSTANCE_TRANING_ROOM_GET_ENEMY_LIST_REQ");
        public static readonly PacketId S2C_INSTANCE_TRANING_ROOM_GET_ENEMY_LIST_RES = new PacketId(13, 14, 2, "S2C_INSTANCE_TRANING_ROOM_GET_ENEMY_LIST_RES"); // トレーニング部屋敵リスト取得に
        public static readonly PacketId C2S_INSTANCE_TRANING_ROOM_SET_ENEMY_REQ = new PacketId(13, 15, 1, "C2S_INSTANCE_TRANING_ROOM_SET_ENEMY_REQ");
        public static readonly PacketId S2C_INSTANCE_TRANING_ROOM_SET_ENEMY_RES = new PacketId(13, 15, 2, "S2C_INSTANCE_TRANING_ROOM_SET_ENEMY_RES"); // トレーニング部屋敵セット要求に
        public static readonly PacketId C2S_INSTANCE_TREASURE_POINT_GET_CATEGORY_LIST_REQ = new PacketId(13, 16, 1, "C2S_INSTANCE_TREASURE_POINT_GET_CATEGORY_LIST_REQ");
        public static readonly PacketId S2C_INSTANCE_TREASURE_POINT_GET_CATEGORY_LIST_RES = new PacketId(13, 16, 2, "S2C_INSTANCE_TREASURE_POINT_GET_CATEGORY_LIST_RES"); // お宝ポイント　カテゴリリスト取得に
        public static readonly PacketId C2S_INSTANCE_TREASURE_POINT_GET_LIST_REQ = new PacketId(13, 17, 1, "C2S_INSTANCE_TREASURE_POINT_GET_LIST_REQ");
        public static readonly PacketId S2C_INSTANCE_TREASURE_POINT_GET_LIST_RES = new PacketId(13, 17, 2, "S2C_INSTANCE_TREASURE_POINT_GET_LIST_RES"); // お宝ポイント　ポイントリスト取得
        public static readonly PacketId C2S_INSTANCE_TREASURE_POINT_GET_INFO_REQ = new PacketId(13, 18, 1, "C2S_INSTANCE_TREASURE_POINT_GET_INFO_REQ");
        public static readonly PacketId S2C_INSTANCE_TREASURE_POINT_GET_INFO_RES = new PacketId(13, 18, 2, "S2C_INSTANCE_TREASURE_POINT_GET_INFO_RES"); // お宝ポイント　ポイント情報取得
        public static readonly PacketId C2S_INSTANCE_SET_OM_INSTANT_KEY_VALUE_REQ = new PacketId(13, 20, 1, "C2S_INSTANCE_SET_OM_INSTANT_KEY_VALUE_REQ");
        public static readonly PacketId S2C_INSTANCE_SET_OM_INSTANT_KEY_VALUE_RES = new PacketId(13, 20, 2, "S2C_INSTANCE_SET_OM_INSTANT_KEY_VALUE_RES"); // OMインスタンスエリア共有メモリ保存に
        public static readonly PacketId S2C_INSTANCE_13_20_16_NTC = new PacketId(13, 20, 16, "S2C_INSTANCE_13_20_16_NTC");
        public static readonly PacketId C2S_INSTANCE_GET_OM_INSTANT_KEY_VALUE_REQ = new PacketId(13, 21, 1, "C2S_INSTANCE_GET_OM_INSTANT_KEY_VALUE_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_OM_INSTANT_KEY_VALUE_RES = new PacketId(13, 21, 2, "S2C_INSTANCE_GET_OM_INSTANT_KEY_VALUE_RES"); // OMインスタンスエリア共有メモリ取得に
        public static readonly PacketId C2S_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_REQ = new PacketId(13, 22, 1, "C2S_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_RES = new PacketId(13, 22, 2, "S2C_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_RES"); // OMインスタンスエリア共有メモリ全取得に
        public static readonly PacketId C2S_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_REQ = new PacketId(13, 23, 1, "C2S_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_REQ");
        public static readonly PacketId S2C_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_RES = new PacketId(13, 23, 2, "S2C_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_RES"); // OMインスタンスエリア共有メモリ交換に
        public static readonly PacketId S2C_INSTANCE_13_23_16_NTC = new PacketId(13, 23, 16, "S2C_INSTANCE_13_23_16_NTC");
        public static readonly PacketId C2S_INSTANCE_SET_INSTANT_KEY_VALUE_UL_REQ = new PacketId(13, 24, 1, "C2S_INSTANCE_SET_INSTANT_KEY_VALUE_UL_REQ");
        public static readonly PacketId S2C_INSTANCE_SET_INSTANT_KEY_VALUE_UL_RES = new PacketId(13, 24, 2, "S2C_INSTANCE_SET_INSTANT_KEY_VALUE_UL_RES"); // インスタンスエリア共有メモリ保存(u32)に
        public static readonly PacketId C2S_INSTANCE_GET_INSTANT_KEY_VALUE_UL_REQ = new PacketId(13, 25, 1, "C2S_INSTANCE_GET_INSTANT_KEY_VALUE_UL_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_INSTANT_KEY_VALUE_UL_RES = new PacketId(13, 25, 2, "S2C_INSTANCE_GET_INSTANT_KEY_VALUE_UL_RES"); // インスタンスエリア共有メモリ取得(u32)に
        public static readonly PacketId C2S_INSTANCE_SET_INSTANT_KEY_VALUE_STR_REQ = new PacketId(13, 26, 1, "C2S_INSTANCE_SET_INSTANT_KEY_VALUE_STR_REQ");
        public static readonly PacketId S2C_INSTANCE_SET_INSTANT_KEY_VALUE_STR_RES = new PacketId(13, 26, 2, "S2C_INSTANCE_SET_INSTANT_KEY_VALUE_STR_RES"); // インスタンスエリア共有メモリ保存(MtString)に
        public static readonly PacketId C2S_INSTANCE_GET_INSTANT_KEY_VALUE_STR_REQ = new PacketId(13, 27, 1, "C2S_INSTANCE_GET_INSTANT_KEY_VALUE_STR_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_INSTANT_KEY_VALUE_STR_RES = new PacketId(13, 27, 2, "S2C_INSTANCE_GET_INSTANT_KEY_VALUE_STR_RES"); // インスタンスエリア共有メモリ取得(MtString)に
        public static readonly PacketId S2C_INSTANCE_13_29_16_NTC = new PacketId(13, 29, 16, "S2C_INSTANCE_13_29_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_30_16_NTC = new PacketId(13, 30, 16, "S2C_INSTANCE_13_30_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_31_16_NTC = new PacketId(13, 31, 16, "S2C_INSTANCE_13_31_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_32_16_NTC = new PacketId(13, 32, 16, "S2C_INSTANCE_13_32_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_33_16_NTC = new PacketId(13, 33, 16, "S2C_INSTANCE_13_33_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_34_16_NTC = new PacketId(13, 34, 16, "S2C_INSTANCE_13_34_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_35_16_NTC = new PacketId(13, 35, 16, "S2C_INSTANCE_13_35_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_36_16_NTC = new PacketId(13, 36, 16, "S2C_INSTANCE_13_36_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_37_16_NTC = new PacketId(13, 37, 16, "S2C_INSTANCE_13_37_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_38_16_NTC = new PacketId(13, 38, 16, "S2C_INSTANCE_13_38_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_39_16_NTC = new PacketId(13, 39, 16, "S2C_INSTANCE_13_39_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_40_16_NTC = new PacketId(13, 40, 16, "S2C_INSTANCE_13_40_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_41_16_NTC = new PacketId(13, 41, 16, "S2C_INSTANCE_13_41_16_NTC");
        public static readonly PacketId S2C_INSTANCE_13_42_16_NTC = new PacketId(13, 42, 16, "S2C_INSTANCE_13_42_16_NTC");
        public static readonly PacketId C2S_INSTANCE_GET_EX_OM_INFO_REQ = new PacketId(13, 43, 1, "C2S_INSTANCE_GET_EX_OM_INFO_REQ");
        public static readonly PacketId S2C_INSTANCE_GET_EX_OM_INFO_RES = new PacketId(13, 43, 2, "S2C_INSTANCE_GET_EX_OM_INFO_RES"); // 拡張OM情報取得に

// Group: 14 - (WARP)
        public static readonly PacketId C2S_WARP_RELEASE_WARP_POINT_REQ = new PacketId(14, 0, 1, "C2S_WARP_RELEASE_WARP_POINT_REQ");
        public static readonly PacketId S2C_WARP_RELEASE_WARP_POINT_RES = new PacketId(14, 0, 2, "S2C_WARP_RELEASE_WARP_POINT_RES"); // ワープポイント解放要求に
        public static readonly PacketId S2C_WARP_14_0_16_NTC = new PacketId(14, 0, 16, "S2C_WARP_14_0_16_NTC");
        public static readonly PacketId C2S_WARP_GET_RELEASE_WARP_POINT_LIST_REQ = new PacketId(14, 1, 1, "C2S_WARP_GET_RELEASE_WARP_POINT_LIST_REQ");
        public static readonly PacketId S2C_WARP_GET_RELEASE_WARP_POINT_LIST_RES = new PacketId(14, 1, 2, "S2C_WARP_GET_RELEASE_WARP_POINT_LIST_RES"); // 解放済みワープポイントリストを取得に
        public static readonly PacketId C2S_WARP_GET_WARP_POINT_LIST_REQ = new PacketId(14, 2, 1, "C2S_WARP_GET_WARP_POINT_LIST_REQ");
        public static readonly PacketId S2C_WARP_GET_WARP_POINT_LIST_RES = new PacketId(14, 2, 2, "S2C_WARP_GET_WARP_POINT_LIST_RES"); // ワープポイントリストを取得に
        public static readonly PacketId C2S_WARP_WARP_REQ = new PacketId(14, 3, 1, "C2S_WARP_WARP_REQ");
        public static readonly PacketId S2C_WARP_WARP_RES = new PacketId(14, 3, 2, "S2C_WARP_WARP_RES"); // ワープリクエストに
        public static readonly PacketId C2S_WARP_GET_FAVORITE_WARP_POINT_LIST_REQ = new PacketId(14, 4, 1, "C2S_WARP_GET_FAVORITE_WARP_POINT_LIST_REQ");
        public static readonly PacketId S2C_WARP_GET_FAVORITE_WARP_POINT_LIST_RES = new PacketId(14, 4, 2, "S2C_WARP_GET_FAVORITE_WARP_POINT_LIST_RES"); // お気に入りワープポイントリストを取得に
        public static readonly PacketId C2S_WARP_FAVORITE_WARP_REQ = new PacketId(14, 5, 1, "C2S_WARP_FAVORITE_WARP_REQ");
        public static readonly PacketId S2C_WARP_FAVORITE_WARP_RES = new PacketId(14, 5, 2, "S2C_WARP_FAVORITE_WARP_RES"); // お気に入りワープリクエストに
        public static readonly PacketId C2S_WARP_REGISTER_FAVORITE_WARP_REQ = new PacketId(14, 6, 1, "C2S_WARP_REGISTER_FAVORITE_WARP_REQ");
        public static readonly PacketId S2C_WARP_REGISTER_FAVORITE_WARP_RES = new PacketId(14, 6, 2, "S2C_WARP_REGISTER_FAVORITE_WARP_RES"); // お気に入りワープ登録に
        public static readonly PacketId C2S_WARP_PARTY_WARP_REQ = new PacketId(14, 7, 1, "C2S_WARP_PARTY_WARP_REQ");
        public static readonly PacketId S2C_WARP_PARTY_WARP_RES = new PacketId(14, 7, 2, "S2C_WARP_PARTY_WARP_RES"); // 同行ワープに
        public static readonly PacketId C2S_WARP_GET_AREA_WARP_POINT_LIST_REQ = new PacketId(14, 8, 1, "C2S_WARP_GET_AREA_WARP_POINT_LIST_REQ");
        public static readonly PacketId S2C_WARP_GET_AREA_WARP_POINT_LIST_RES = new PacketId(14, 8, 2, "S2C_WARP_GET_AREA_WARP_POINT_LIST_RES"); // エリア指定ワープ情報取得に
        public static readonly PacketId C2S_WARP_AREA_WARP_REQ = new PacketId(14, 9, 1, "C2S_WARP_AREA_WARP_REQ");
        public static readonly PacketId S2C_WARP_AREA_WARP_RES = new PacketId(14, 9, 2, "S2C_WARP_AREA_WARP_RES"); // 現在のエリアからのワープリクエストに
        public static readonly PacketId C2S_WARP_GET_RETURN_LOCATION_REQ = new PacketId(14, 12, 1, "C2S_WARP_GET_RETURN_LOCATION_REQ");
        public static readonly PacketId S2C_WARP_GET_RETURN_LOCATION_RES = new PacketId(14, 12, 2, "S2C_WARP_GET_RETURN_LOCATION_RES"); // 復帰位置の取得に
        public static readonly PacketId S2C_WARP_14_13_16_NTC = new PacketId(14, 13, 16, "S2C_WARP_14_13_16_NTC");
        public static readonly PacketId C2S_WARP_GET_START_POINT_LIST_REQ = new PacketId(14, 14, 1, "C2S_WARP_GET_START_POINT_LIST_REQ");
        public static readonly PacketId S2C_WARP_GET_START_POINT_LIST_RES = new PacketId(14, 14, 2, "S2C_WARP_GET_START_POINT_LIST_RES"); // ゲーム開始ポイントの取得に

// Group: 15
        public static readonly PacketId S2C_15_65535_255 = new PacketId(15, 65535, 255, "S2C_15_65535_255");

// Group: 16 - (FRIEND)
        public static readonly PacketId C2S_FRIEND_GET_FRIEND_LIST_REQ = new PacketId(16, 0, 1, "C2S_FRIEND_GET_FRIEND_LIST_REQ");
        public static readonly PacketId S2C_FRIEND_GET_FRIEND_LIST_RES = new PacketId(16, 0, 2, "S2C_FRIEND_GET_FRIEND_LIST_RES"); // フレンドリスト取得に
        public static readonly PacketId C2S_FRIEND_APPLY_FRIEND_REQ = new PacketId(16, 1, 1, "C2S_FRIEND_APPLY_FRIEND_REQ");
        public static readonly PacketId S2C_FRIEND_APPLY_FRIEND_RES = new PacketId(16, 1, 2, "S2C_FRIEND_APPLY_FRIEND_RES"); // フレンド申請に
        public static readonly PacketId S2C_FRIEND_16_1_16_NTC = new PacketId(16, 1, 16, "S2C_FRIEND_16_1_16_NTC");
        public static readonly PacketId C2S_FRIEND_APPROVE_FRIEND_REQ = new PacketId(16, 2, 1, "C2S_FRIEND_APPROVE_FRIEND_REQ");
        public static readonly PacketId S2C_FRIEND_APPROVE_FRIEND_RES = new PacketId(16, 2, 2, "S2C_FRIEND_APPROVE_FRIEND_RES"); // フレンド承認に
        public static readonly PacketId S2C_FRIEND_16_2_16_NTC = new PacketId(16, 2, 16, "S2C_FRIEND_16_2_16_NTC");
        public static readonly PacketId C2S_FRIEND_REMOVE_FRIEND_REQ = new PacketId(16, 3, 1, "C2S_FRIEND_REMOVE_FRIEND_REQ");
        public static readonly PacketId S2C_FRIEND_REMOVE_FRIEND_RES = new PacketId(16, 3, 2, "S2C_FRIEND_REMOVE_FRIEND_RES"); // フレンド解除に
        public static readonly PacketId S2C_FRIEND_16_3_16_NTC = new PacketId(16, 3, 16, "S2C_FRIEND_16_3_16_NTC");
        public static readonly PacketId C2S_FRIEND_REGISTER_FAVORITE_FRIEND_REQ = new PacketId(16, 4, 1, "C2S_FRIEND_REGISTER_FAVORITE_FRIEND_REQ");
        public static readonly PacketId S2C_FRIEND_REGISTER_FAVORITE_FRIEND_RES = new PacketId(16, 4, 2, "S2C_FRIEND_REGISTER_FAVORITE_FRIEND_RES"); // お気に入りフレンド設定に
        public static readonly PacketId C2S_FRIEND_CANCEL_FRIEND_APPLICATION_REQ = new PacketId(16, 5, 1, "C2S_FRIEND_CANCEL_FRIEND_APPLICATION_REQ");
        public static readonly PacketId S2C_FRIEND_CANCEL_FRIEND_APPLICATION_RES = new PacketId(16, 5, 2, "S2C_FRIEND_CANCEL_FRIEND_APPLICATION_RES"); // フレンド申請キャンセルに
        public static readonly PacketId S2C_FRIEND_16_5_16_NTC = new PacketId(16, 5, 16, "S2C_FRIEND_16_5_16_NTC");
        public static readonly PacketId C2S_FRIEND_GET_RECENT_CHARACTER_LIST_REQ = new PacketId(16, 6, 1, "C2S_FRIEND_GET_RECENT_CHARACTER_LIST_REQ");
        public static readonly PacketId S2C_FRIEND_GET_RECENT_CHARACTER_LIST_RES = new PacketId(16, 6, 2, "S2C_FRIEND_GET_RECENT_CHARACTER_LIST_RES"); // 最近遊んだプレイヤー取得に

// Group: 17 - (BLACK)
        public static readonly PacketId C2S_BLACK_LIST_GET_BLACK_LIST_REQ = new PacketId(17, 0, 1, "C2S_BLACK_LIST_GET_BLACK_LIST_REQ");
        public static readonly PacketId S2C_BLACK_LIST_GET_BLACK_LIST_RES = new PacketId(17, 0, 2, "S2C_BLACK_LIST_GET_BLACK_LIST_RES"); // ブラックリスト取得に
        public static readonly PacketId C2S_BLACK_LIST_ADD_BLACK_LIST_REQ = new PacketId(17, 1, 1, "C2S_BLACK_LIST_ADD_BLACK_LIST_REQ");
        public static readonly PacketId S2C_BLACK_LIST_ADD_BLACK_LIST_RES = new PacketId(17, 1, 2, "S2C_BLACK_LIST_ADD_BLACK_LIST_RES"); // ブラックリスト追加に
        public static readonly PacketId C2S_BLACK_LIST_REMOVE_BLACK_LIST_REQ = new PacketId(17, 2, 1, "C2S_BLACK_LIST_REMOVE_BLACK_LIST_REQ");
        public static readonly PacketId S2C_BLACK_LIST_REMOVE_BLACK_LIST_RES = new PacketId(17, 2, 2, "S2C_BLACK_LIST_REMOVE_BLACK_LIST_RES"); // ブラックリスト削除に

// Group: 18 - (GROUP)
        public static readonly PacketId C2S_GROUP_CHAT_GROUP_CHAT_GET_MEMBER_LIST_REQ = new PacketId(18, 0, 1, "C2S_GROUP_CHAT_GROUP_CHAT_GET_MEMBER_LIST_REQ");
        public static readonly PacketId S2C_GROUP_CHAT_GROUP_CHAT_GET_MEMBER_LIST_RES = new PacketId(18, 0, 2, "S2C_GROUP_CHAT_GROUP_CHAT_GET_MEMBER_LIST_RES"); // グループチャットメンバーリスト取得に
        public static readonly PacketId C2S_GROUP_CHAT_GROUP_CHAT_INVITE_CHARACTER_REQ = new PacketId(18, 1, 1, "C2S_GROUP_CHAT_GROUP_CHAT_INVITE_CHARACTER_REQ");
        public static readonly PacketId S2C_GROUP_CHAT_GROUP_CHAT_INVITE_CHARACTER_RES = new PacketId(18, 1, 2, "S2C_GROUP_CHAT_GROUP_CHAT_INVITE_CHARACTER_RES"); // グループチャット招待に
        public static readonly PacketId S2C_GROUP_18_1_16_NTC = new PacketId(18, 1, 16, "S2C_GROUP_18_1_16_NTC");
        public static readonly PacketId C2S_GROUP_CHAT_GROUP_CHAT_LEAVE_CHARACTER_REQ = new PacketId(18, 2, 1, "C2S_GROUP_CHAT_GROUP_CHAT_LEAVE_CHARACTER_REQ");
        public static readonly PacketId S2C_GROUP_CHAT_GROUP_CHAT_LEAVE_CHARACTER_RES = new PacketId(18, 2, 2, "S2C_GROUP_CHAT_GROUP_CHAT_LEAVE_CHARACTER_RES"); // グループチャット退室に
        public static readonly PacketId S2C_GROUP_18_2_16_NTC = new PacketId(18, 2, 16, "S2C_GROUP_18_2_16_NTC");
        public static readonly PacketId C2S_GROUP_CHAT_GROUP_CHAT_KICK_CHARACTER_REQ = new PacketId(18, 3, 1, "C2S_GROUP_CHAT_GROUP_CHAT_KICK_CHARACTER_REQ");
        public static readonly PacketId S2C_GROUP_CHAT_GROUP_CHAT_KICK_CHARACTER_RES = new PacketId(18, 3, 2, "S2C_GROUP_CHAT_GROUP_CHAT_KICK_CHARACTER_RES"); // グループチャットキックに
        public static readonly PacketId S2C_GROUP_18_3_16_NTC = new PacketId(18, 3, 16, "S2C_GROUP_18_3_16_NTC");
        public static readonly PacketId S2C_GROUP_18_4_16_NTC = new PacketId(18, 4, 16, "S2C_GROUP_18_4_16_NTC");

// Group: 19 - (SKILL)
        public static readonly PacketId C2S_SKILL_GET_ACQUIRABLE_NORMAL_SKILL_LIST_REQ = new PacketId(19, 0, 1, "C2S_SKILL_GET_ACQUIRABLE_NORMAL_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_ACQUIRABLE_NORMAL_SKILL_LIST_RES = new PacketId(19, 0, 2, "S2C_SKILL_GET_ACQUIRABLE_NORMAL_SKILL_LIST_RES"); // 習得可能ノーマルスキルリスト取得に
        public static readonly PacketId C2S_SKILL_GET_ACQUIRABLE_SKILL_LIST_REQ = new PacketId(19, 1, 1, "C2S_SKILL_GET_ACQUIRABLE_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_ACQUIRABLE_SKILL_LIST_RES = new PacketId(19, 1, 2, "S2C_SKILL_GET_ACQUIRABLE_SKILL_LIST_RES"); // 習得可能スキルリスト取得に
        public static readonly PacketId C2S_SKILL_GET_ACQUIRABLE_ABILITY_LIST_REQ = new PacketId(19, 2, 1, "C2S_SKILL_GET_ACQUIRABLE_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_ACQUIRABLE_ABILITY_LIST_RES = new PacketId(19, 2, 2, "S2C_SKILL_GET_ACQUIRABLE_ABILITY_LIST_RES"); // 習得可能アビリティリスト取得に
        public static readonly PacketId C2S_SKILL_LEARN_NORMAL_SKILL_REQ = new PacketId(19, 3, 1, "C2S_SKILL_LEARN_NORMAL_SKILL_REQ");
        public static readonly PacketId S2C_SKILL_LEARN_NORMAL_SKILL_RES = new PacketId(19, 3, 2, "S2C_SKILL_LEARN_NORMAL_SKILL_RES"); // ノーマルスキル習得に
        public static readonly PacketId C2S_SKILL_LEARN_SKILL_REQ = new PacketId(19, 4, 1, "C2S_SKILL_LEARN_SKILL_REQ");
        public static readonly PacketId S2C_SKILL_LEARN_SKILL_RES = new PacketId(19, 4, 2, "S2C_SKILL_LEARN_SKILL_RES"); // スキル習得に
        public static readonly PacketId C2S_SKILL_LEARN_ABILITY_REQ = new PacketId(19, 5, 1, "C2S_SKILL_LEARN_ABILITY_REQ");
        public static readonly PacketId S2C_SKILL_LEARN_ABILITY_RES = new PacketId(19, 5, 2, "S2C_SKILL_LEARN_ABILITY_RES"); // アビリティ習得に
        public static readonly PacketId C2S_SKILL_LEARN_PAWN_NORMAL_SKILL_REQ = new PacketId(19, 6, 1, "C2S_SKILL_LEARN_PAWN_NORMAL_SKILL_REQ");
        public static readonly PacketId S2C_SKILL_LEARN_PAWN_NORMAL_SKILL_RES = new PacketId(19, 6, 2, "S2C_SKILL_LEARN_PAWN_NORMAL_SKILL_RES"); // ポーンノーマルスキル習得に
        public static readonly PacketId C2S_SKILL_LEARN_PAWN_SKILL_REQ = new PacketId(19, 7, 1, "C2S_SKILL_LEARN_PAWN_SKILL_REQ");
        public static readonly PacketId S2C_SKILL_LEARN_PAWN_SKILL_RES = new PacketId(19, 7, 2, "S2C_SKILL_LEARN_PAWN_SKILL_RES"); // ポーンカスタムスキル習得に
        public static readonly PacketId C2S_SKILL_LEARN_PAWN_ABILITY_REQ = new PacketId(19, 8, 1, "C2S_SKILL_LEARN_PAWN_ABILITY_REQ");
        public static readonly PacketId S2C_SKILL_LEARN_PAWN_ABILITY_RES = new PacketId(19, 8, 2, "S2C_SKILL_LEARN_PAWN_ABILITY_RES"); // ポーンアビリティ習得に
        public static readonly PacketId C2S_SKILL_GET_LEARNED_NORMAL_SKILL_LIST_REQ = new PacketId(19, 9, 1, "C2S_SKILL_GET_LEARNED_NORMAL_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_LEARNED_NORMAL_SKILL_LIST_RES = new PacketId(19, 9, 2, "S2C_SKILL_GET_LEARNED_NORMAL_SKILL_LIST_RES"); // 習得済ノーマルスキルリストの取得に
        public static readonly PacketId C2S_SKILL_GET_LEARNED_SKILL_LIST_REQ = new PacketId(19, 10, 1, "C2S_SKILL_GET_LEARNED_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_LEARNED_SKILL_LIST_RES = new PacketId(19, 10, 2, "S2C_SKILL_GET_LEARNED_SKILL_LIST_RES"); // 習得済スキルリストの取得に
        public static readonly PacketId C2S_SKILL_GET_LEARNED_ABILITY_LIST_REQ = new PacketId(19, 11, 1, "C2S_SKILL_GET_LEARNED_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_LEARNED_ABILITY_LIST_RES = new PacketId(19, 11, 2, "S2C_SKILL_GET_LEARNED_ABILITY_LIST_RES"); // 習得済アビリティリストの取得に
        public static readonly PacketId C2S_SKILL_GET_PAWN_LEARNED_NORMAL_SKILL_LIST_REQ = new PacketId(19, 12, 1, "C2S_SKILL_GET_PAWN_LEARNED_NORMAL_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_PAWN_LEARNED_NORMAL_SKILL_LIST_RES = new PacketId(19, 12, 2, "S2C_SKILL_GET_PAWN_LEARNED_NORMAL_SKILL_LIST_RES"); // ポーン習得済ノーマルスキルリストの取得に
        public static readonly PacketId C2S_SKILL_GET_PAWN_LEARNED_SKILL_LIST_REQ = new PacketId(19, 13, 1, "C2S_SKILL_GET_PAWN_LEARNED_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_PAWN_LEARNED_SKILL_LIST_RES = new PacketId(19, 13, 2, "S2C_SKILL_GET_PAWN_LEARNED_SKILL_LIST_RES"); // ポーン習得済スキルリストの取得に
        public static readonly PacketId C2S_SKILL_GET_PAWN_LEARNED_ABILITY_LIST_REQ = new PacketId(19, 14, 1, "C2S_SKILL_GET_PAWN_LEARNED_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_PAWN_LEARNED_ABILITY_LIST_RES = new PacketId(19, 14, 2, "S2C_SKILL_GET_PAWN_LEARNED_ABILITY_LIST_RES"); // ポーン習得済アビリティリストの取得に
        public static readonly PacketId C2S_SKILL_SET_SKILL_REQ = new PacketId(19, 15, 1, "C2S_SKILL_SET_SKILL_REQ");
        public static readonly PacketId S2C_SKILL_SET_SKILL_RES = new PacketId(19, 15, 2, "S2C_SKILL_SET_SKILL_RES"); // スキル装備に
        public static readonly PacketId C2S_SKILL_CHANGE_EX_SKILL_REQ = new PacketId(19, 16, 1, "C2S_SKILL_CHANGE_EX_SKILL_REQ");
        public static readonly PacketId S2C_SKILL_CHANGE_EX_SKILL_RES = new PacketId(19, 16, 2, "S2C_SKILL_CHANGE_EX_SKILL_RES"); // EXスキル切り替えに
        public static readonly PacketId C2S_SKILL_SET_ABILITY_REQ = new PacketId(19, 17, 1, "C2S_SKILL_SET_ABILITY_REQ");
        public static readonly PacketId S2C_SKILL_SET_ABILITY_RES = new PacketId(19, 17, 2, "S2C_SKILL_SET_ABILITY_RES"); // アビリティ装備に
        public static readonly PacketId C2S_SKILL_SET_PAWN_SKILL_REQ = new PacketId(19, 18, 1, "C2S_SKILL_SET_PAWN_SKILL_REQ");
        public static readonly PacketId S2C_SKILL_SET_PAWN_SKILL_RES = new PacketId(19, 18, 2, "S2C_SKILL_SET_PAWN_SKILL_RES"); // ポーンカスタムスキル装備に
        public static readonly PacketId C2S_SKILL_SET_PAWN_ABILITY_REQ = new PacketId(19, 19, 1, "C2S_SKILL_SET_PAWN_ABILITY_REQ");
        public static readonly PacketId S2C_SKILL_SET_PAWN_ABILITY_RES = new PacketId(19, 19, 2, "S2C_SKILL_SET_PAWN_ABILITY_RES"); // ポーンアビリティ装備に
        public static readonly PacketId C2S_SKILL_SET_OFF_SKILL_REQ = new PacketId(19, 20, 1, "C2S_SKILL_SET_OFF_SKILL_REQ");
        public static readonly PacketId S2C_SKILL_SET_OFF_SKILL_RES = new PacketId(19, 20, 2, "S2C_SKILL_SET_OFF_SKILL_RES"); // スキル解除に
        public static readonly PacketId C2S_SKILL_SET_OFF_ABILITY_REQ = new PacketId(19, 21, 1, "C2S_SKILL_SET_OFF_ABILITY_REQ");
        public static readonly PacketId S2C_SKILL_SET_OFF_ABILITY_RES = new PacketId(19, 21, 2, "S2C_SKILL_SET_OFF_ABILITY_RES"); // アビリティ解除に
        public static readonly PacketId C2S_SKILL_SET_OFF_PAWN_SKILL_REQ = new PacketId(19, 22, 1, "C2S_SKILL_SET_OFF_PAWN_SKILL_REQ");
        public static readonly PacketId S2C_SKILL_SET_OFF_PAWN_SKILL_RES = new PacketId(19, 22, 2, "S2C_SKILL_SET_OFF_PAWN_SKILL_RES"); // ポーンカスタムスキル解除に
        public static readonly PacketId C2S_SKILL_SET_OFF_PAWN_ABILITY_REQ = new PacketId(19, 23, 1, "C2S_SKILL_SET_OFF_PAWN_ABILITY_REQ");
        public static readonly PacketId S2C_SKILL_SET_OFF_PAWN_ABILITY_RES = new PacketId(19, 23, 2, "S2C_SKILL_SET_OFF_PAWN_ABILITY_RES"); // ポーンアビリティ解除に
        public static readonly PacketId C2S_SKILL_GET_SET_SKILL_LIST_REQ = new PacketId(19, 24, 1, "C2S_SKILL_GET_SET_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_SET_SKILL_LIST_RES = new PacketId(19, 24, 2, "S2C_SKILL_GET_SET_SKILL_LIST_RES"); // 装備中スキルリストの取得に
        public static readonly PacketId C2S_SKILL_GET_SET_ABILITY_LIST_REQ = new PacketId(19, 25, 1, "C2S_SKILL_GET_SET_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_SET_ABILITY_LIST_RES = new PacketId(19, 25, 2, "S2C_SKILL_GET_SET_ABILITY_LIST_RES"); // 装備中アビリティリストの取得に
        public static readonly PacketId C2S_SKILL_GET_PAWN_SET_SKILL_LIST_REQ = new PacketId(19, 26, 1, "C2S_SKILL_GET_PAWN_SET_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_PAWN_SET_SKILL_LIST_RES = new PacketId(19, 26, 2, "S2C_SKILL_GET_PAWN_SET_SKILL_LIST_RES"); // ポーン装備中スキルリストの取得に
        public static readonly PacketId C2S_SKILL_GET_PAWN_SET_ABILITY_LIST_REQ = new PacketId(19, 27, 1, "C2S_SKILL_GET_PAWN_SET_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_PAWN_SET_ABILITY_LIST_RES = new PacketId(19, 27, 2, "S2C_SKILL_GET_PAWN_SET_ABILITY_LIST_RES"); // ポーン装備中アビリティリストの取得に
        public static readonly PacketId C2S_SKILL_GET_CURRENT_SET_SKILL_LIST_REQ = new PacketId(19, 28, 1, "C2S_SKILL_GET_CURRENT_SET_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_CURRENT_SET_SKILL_LIST_RES = new PacketId(19, 28, 2, "S2C_SKILL_GET_CURRENT_SET_SKILL_LIST_RES"); // 装備中スキルリスト取得に
        public static readonly PacketId C2S_SKILL_GET_CURRENT_SET_ABILITY_LIST_REQ = new PacketId(19, 29, 1, "C2S_SKILL_GET_CURRENT_SET_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_CURRENT_SET_ABILITY_LIST_RES = new PacketId(19, 29, 2, "S2C_SKILL_GET_CURRENT_SET_ABILITY_LIST_RES"); // 装備中アビリティリスト取得に
        public static readonly PacketId C2S_SKILL_19_30_1_REQ = new PacketId(19, 30, 1, "C2S_SKILL_19_30_1_REQ");
        public static readonly PacketId S2C_SKILL_19_30_2_RES = new PacketId(19, 30, 2, "S2C_SKILL_19_30_2_RES");
        public static readonly PacketId C2S_SKILL_19_31_1_REQ = new PacketId(19, 31, 1, "C2S_SKILL_19_31_1_REQ");
        public static readonly PacketId S2C_SKILL_19_31_2_RES = new PacketId(19, 31, 2, "S2C_SKILL_19_31_2_RES");
        public static readonly PacketId C2S_SKILL_GET_RELEASE_SKILL_LIST_REQ = new PacketId(19, 32, 1, "C2S_SKILL_GET_RELEASE_SKILL_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_RELEASE_SKILL_LIST_RES = new PacketId(19, 32, 2, "S2C_SKILL_GET_RELEASE_SKILL_LIST_RES"); // 解放済みスキルリストの取得に
        public static readonly PacketId C2S_SKILL_GET_RELEASE_ABILITY_LIST_REQ = new PacketId(19, 33, 1, "C2S_SKILL_GET_RELEASE_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_RELEASE_ABILITY_LIST_RES = new PacketId(19, 33, 2, "S2C_SKILL_GET_RELEASE_ABILITY_LIST_RES"); // 解放済みアビリティリストの取得に
        public static readonly PacketId C2S_SKILL_REGISTER_PRESET_ABILITY_REQ = new PacketId(19, 34, 1, "C2S_SKILL_REGISTER_PRESET_ABILITY_REQ");
        public static readonly PacketId S2C_SKILL_REGISTER_PRESET_ABILITY_RES = new PacketId(19, 34, 2, "S2C_SKILL_REGISTER_PRESET_ABILITY_RES"); // プリセットアビリティの登録に
        public static readonly PacketId C2S_SKILL_SET_PRESET_ABILITY_NAME_REQ = new PacketId(19, 35, 1, "C2S_SKILL_SET_PRESET_ABILITY_NAME_REQ");
        public static readonly PacketId S2C_SKILL_SET_PRESET_ABILITY_NAME_RES = new PacketId(19, 35, 2, "S2C_SKILL_SET_PRESET_ABILITY_NAME_RES"); // プリセットアビリティ名の設定に
        public static readonly PacketId C2S_SKILL_GET_PRESET_ABILITY_LIST_REQ = new PacketId(19, 36, 1, "C2S_SKILL_GET_PRESET_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_GET_PRESET_ABILITY_LIST_RES = new PacketId(19, 36, 2, "S2C_SKILL_GET_PRESET_ABILITY_LIST_RES"); // プリセットアビリティリストの取得に
        public static readonly PacketId C2S_SKILL_SET_PRESET_ABILITY_LIST_REQ = new PacketId(19, 37, 1, "C2S_SKILL_SET_PRESET_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_SET_PRESET_ABILITY_LIST_RES = new PacketId(19, 37, 2, "S2C_SKILL_SET_PRESET_ABILITY_LIST_RES"); // プリセットアビリティ装備に
        public static readonly PacketId C2S_SKILL_SET_PAWN_PRESET_ABILITY_LIST_REQ = new PacketId(19, 38, 1, "C2S_SKILL_SET_PAWN_PRESET_ABILITY_LIST_REQ");
        public static readonly PacketId S2C_SKILL_SET_PAWN_PRESET_ABILITY_LIST_RES = new PacketId(19, 38, 2, "S2C_SKILL_SET_PAWN_PRESET_ABILITY_LIST_RES"); // ポーンプリセットアビリティ装備に
        public static readonly PacketId C2S_SKILL_GET_ABILITY_COST_REQ = new PacketId(19, 39, 1, "C2S_SKILL_GET_ABILITY_COST_REQ");
        public static readonly PacketId S2C_SKILL_GET_ABILITY_COST_RES = new PacketId(19, 39, 2, "S2C_SKILL_GET_ABILITY_COST_RES"); // アビリティセット用コストの取得に
        public static readonly PacketId C2S_SKILL_GET_PAWN_ABILITY_COST_REQ = new PacketId(19, 40, 1, "C2S_SKILL_GET_PAWN_ABILITY_COST_REQ");
        public static readonly PacketId S2C_SKILL_GET_PAWN_ABILITY_COST_RES = new PacketId(19, 40, 2, "S2C_SKILL_GET_PAWN_ABILITY_COST_RES"); // ポーンアビリティセット用コストの取得に
        public static readonly PacketId S2C_SKILL_19_41_16_NTC = new PacketId(19, 41, 16, "S2C_SKILL_19_41_16_NTC");
        public static readonly PacketId S2C_SKILL_19_42_16_NTC = new PacketId(19, 42, 16, "S2C_SKILL_19_42_16_NTC");
        public static readonly PacketId S2C_SKILL_19_43_16_NTC = new PacketId(19, 43, 16, "S2C_SKILL_19_43_16_NTC");
        public static readonly PacketId S2C_SKILL_19_44_16_NTC = new PacketId(19, 44, 16, "S2C_SKILL_19_44_16_NTC");
        public static readonly PacketId S2C_SKILL_19_45_16_NTC = new PacketId(19, 45, 16, "S2C_SKILL_19_45_16_NTC");
        public static readonly PacketId S2C_SKILL_19_46_16_NTC = new PacketId(19, 46, 16, "S2C_SKILL_19_46_16_NTC");
        public static readonly PacketId S2C_SKILL_19_47_16_NTC = new PacketId(19, 47, 16, "S2C_SKILL_19_47_16_NTC");
        public static readonly PacketId S2C_SKILL_19_48_16_NTC = new PacketId(19, 48, 16, "S2C_SKILL_19_48_16_NTC");
        public static readonly PacketId S2C_SKILL_19_49_16_NTC = new PacketId(19, 49, 16, "S2C_SKILL_19_49_16_NTC");
        public static readonly PacketId S2C_SKILL_19_50_16_NTC = new PacketId(19, 50, 16, "S2C_SKILL_19_50_16_NTC");

// Group: 20 - (SHOP)
        public static readonly PacketId C2S_SHOP_GET_SHOP_GOODS_LIST_REQ = new PacketId(20, 0, 1, "C2S_SHOP_GET_SHOP_GOODS_LIST_REQ");
        public static readonly PacketId S2C_SHOP_GET_SHOP_GOODS_LIST_RES = new PacketId(20, 0, 2, "S2C_SHOP_GET_SHOP_GOODS_LIST_RES"); // 商品リスト取得に
        public static readonly PacketId C2S_SHOP_BUY_SHOP_GOODS_REQ = new PacketId(20, 1, 1, "C2S_SHOP_BUY_SHOP_GOODS_REQ");
        public static readonly PacketId S2C_SHOP_BUY_SHOP_GOODS_RES = new PacketId(20, 1, 2, "S2C_SHOP_BUY_SHOP_GOODS_RES"); // 商品購入に

// Group: 21 - (INN)
        public static readonly PacketId C2S_INN_GET_STAY_PRICE_REQ = new PacketId(21, 0, 1, "C2S_INN_GET_STAY_PRICE_REQ");
        public static readonly PacketId S2C_INN_GET_STAY_PRICE_RES = new PacketId(21, 0, 2, "S2C_INN_GET_STAY_PRICE_RES"); // 宿泊費の取得に
        public static readonly PacketId C2S_INN_STAY_INN_REQ = new PacketId(21, 1, 1, "C2S_INN_STAY_INN_REQ");
        public static readonly PacketId S2C_INN_STAY_INN_RES = new PacketId(21, 1, 2, "S2C_INN_STAY_INN_RES"); // 宿泊に
        public static readonly PacketId C2S_INN_GET_PENALTY_HEAL_STAY_PRICE_REQ = new PacketId(21, 5, 1, "C2S_INN_GET_PENALTY_HEAL_STAY_PRICE_REQ");
        public static readonly PacketId S2C_INN_GET_PENALTY_HEAL_STAY_PRICE_RES = new PacketId(21, 5, 2, "S2C_INN_GET_PENALTY_HEAL_STAY_PRICE_RES"); // 弱化回復宿泊費の取得に
        public static readonly PacketId C2S_INN_STAY_PENALTY_HEAL_INN_REQ = new PacketId(21, 6, 1, "C2S_INN_STAY_PENALTY_HEAL_INN_REQ");
        public static readonly PacketId S2C_INN_STAY_PENALTY_HEAL_INN_RES = new PacketId(21, 6, 2, "S2C_INN_STAY_PENALTY_HEAL_INN_RES"); // 弱化回復宿泊に

// Group: 22 - (POTION)
        public static readonly PacketId C2S_POTION_JOB_ELEMENT_CHECK_REQ = new PacketId(22, 0, 1, "C2S_POTION_JOB_ELEMENT_CHECK_REQ");
        public static readonly PacketId S2C_POTION_JOB_ELEMENT_CHECK_RES = new PacketId(22, 0, 2, "S2C_POTION_JOB_ELEMENT_CHECK_RES"); // ジョブ要素解放チェックに
        public static readonly PacketId C2S_POTION_JOB_ELEMENT_RELEASE_REQ = new PacketId(22, 1, 1, "C2S_POTION_JOB_ELEMENT_RELEASE_REQ");
        public static readonly PacketId S2C_POTION_JOB_ELEMENT_RELEASE_RES = new PacketId(22, 1, 2, "S2C_POTION_JOB_ELEMENT_RELEASE_RES"); // ジョブ要素解放に
        public static readonly PacketId C2S_POTION_ORB_ELEMENT_CHECK_REQ = new PacketId(22, 2, 1, "C2S_POTION_ORB_ELEMENT_CHECK_REQ");
        public static readonly PacketId S2C_POTION_ORB_ELEMENT_CHECK_RES = new PacketId(22, 2, 2, "S2C_POTION_ORB_ELEMENT_CHECK_RES"); // 竜力の継承解放チェックに
        public static readonly PacketId C2S_POTION_ORB_ELEMENT_RELEASE_REQ = new PacketId(22, 3, 1, "C2S_POTION_ORB_ELEMENT_RELEASE_REQ");
        public static readonly PacketId S2C_POTION_ORB_ELEMENT_RELEASE_RES = new PacketId(22, 3, 2, "S2C_POTION_ORB_ELEMENT_RELEASE_RES"); // 竜力の継承解放に
        public static readonly PacketId C2S_POTION_ADVENTURE_UTILITY_CHECK_REQ = new PacketId(22, 4, 1, "C2S_POTION_ADVENTURE_UTILITY_CHECK_REQ");
        public static readonly PacketId S2C_POTION_ADVENTURE_UTILITY_CHECK_RES = new PacketId(22, 4, 2, "S2C_POTION_ADVENTURE_UTILITY_CHECK_RES"); // 冒険補助要素解放チェックに
        public static readonly PacketId C2S_POTION_ADVENTURE_UTILITY_RELEASE_REQ = new PacketId(22, 5, 1, "C2S_POTION_ADVENTURE_UTILITY_RELEASE_REQ");
        public static readonly PacketId S2C_POTION_ADVENTURE_UTILITY_RELEASE_RES = new PacketId(22, 5, 2, "S2C_POTION_ADVENTURE_UTILITY_RELEASE_RES"); // 冒険補助要素解放に
        public static readonly PacketId S2C_POTION_22_6_16_NTC = new PacketId(22, 6, 16, "S2C_POTION_22_6_16_NTC");
        public static readonly PacketId S2C_POTION_22_7_16_NTC = new PacketId(22, 7, 16, "S2C_POTION_22_7_16_NTC");
        public static readonly PacketId C2S_POTION_ELEMENT_GROUP_RELEASE_REQ = new PacketId(22, 8, 1, "C2S_POTION_ELEMENT_GROUP_RELEASE_REQ");
        public static readonly PacketId S2C_POTION_ELEMENT_GROUP_RELEASE_RES = new PacketId(22, 8, 2, "S2C_POTION_ELEMENT_GROUP_RELEASE_RES"); // ポーショングループ要素解放に
        public static readonly PacketId C2S_POTION_ELEMENT_GROUP_CHECK_REQ = new PacketId(22, 9, 1, "C2S_POTION_ELEMENT_GROUP_CHECK_REQ");
        public static readonly PacketId S2C_POTION_ELEMENT_GROUP_CHECK_RES = new PacketId(22, 9, 2, "S2C_POTION_ELEMENT_GROUP_CHECK_RES"); // ポーショングループ要素解放による成長要素の有無チェックに

// Group: 23 - (AREA)
        public static readonly PacketId C2S_AREA_GET_AREA_MASTER_INFO_REQ = new PacketId(23, 0, 1, "C2S_AREA_GET_AREA_MASTER_INFO_REQ");
        public static readonly PacketId S2C_AREA_GET_AREA_MASTER_INFO_RES = new PacketId(23, 0, 2, "S2C_AREA_GET_AREA_MASTER_INFO_RES"); // エリア情報取得に
        public static readonly PacketId C2S_AREA_23_1_1_REQ = new PacketId(23, 1, 1, "C2S_AREA_23_1_1_REQ");
        public static readonly PacketId S2C_AREA_23_1_2_RES = new PacketId(23, 1, 2, "S2C_AREA_23_1_2_RES");
        public static readonly PacketId C2S_AREA_AREA_RANK_UP_REQ = new PacketId(23, 2, 1, "C2S_AREA_AREA_RANK_UP_REQ");
        public static readonly PacketId S2C_AREA_AREA_RANK_UP_RES = new PacketId(23, 2, 2, "S2C_AREA_AREA_RANK_UP_RES"); // エリアランクアップに
        public static readonly PacketId C2S_AREA_GET_AREA_SUPPLY_INFO_REQ = new PacketId(23, 3, 1, "C2S_AREA_GET_AREA_SUPPLY_INFO_REQ");
        public static readonly PacketId S2C_AREA_GET_AREA_SUPPLY_INFO_RES = new PacketId(23, 3, 2, "S2C_AREA_GET_AREA_SUPPLY_INFO_RES"); // 支給品リスト取得に
        public static readonly PacketId C2S_AREA_GET_AREA_SUPPLY_REQ = new PacketId(23, 4, 1, "C2S_AREA_GET_AREA_SUPPLY_REQ");
        public static readonly PacketId S2C_AREA_GET_AREA_SUPPLY_RES = new PacketId(23, 4, 2, "S2C_AREA_GET_AREA_SUPPLY_RES"); // 支給品受け取りに
        public static readonly PacketId C2S_AREA_GET_AREA_POINT_DEBUG_REQ = new PacketId(23, 5, 1, "C2S_AREA_GET_AREA_POINT_DEBUG_REQ");
        public static readonly PacketId S2C_AREA_GET_AREA_POINT_DEBUG_RES = new PacketId(23, 5, 2, "S2C_AREA_GET_AREA_POINT_DEBUG_RES"); // エリアポイント加算（デバッグ用）に
        public static readonly PacketId C2S_AREA_GET_AREA_RELEASE_LIST_REQ = new PacketId(23, 7, 1, "C2S_AREA_GET_AREA_RELEASE_LIST_REQ");
        public static readonly PacketId S2C_AREA_GET_AREA_RELEASE_LIST_RES = new PacketId(23, 7, 2, "S2C_AREA_GET_AREA_RELEASE_LIST_RES"); // 全エリア解放リスト取得に
        public static readonly PacketId C2S_AREA_GET_LEADER_AREA_RELEASE_LIST_REQ = new PacketId(23, 8, 1, "C2S_AREA_GET_LEADER_AREA_RELEASE_LIST_REQ");
        public static readonly PacketId S2C_AREA_GET_LEADER_AREA_RELEASE_LIST_RES = new PacketId(23, 8, 2, "S2C_AREA_GET_LEADER_AREA_RELEASE_LIST_RES"); // 全エリア解放リスト取得（PTリーダー依存）に
        public static readonly PacketId C2S_AREA_GET_AREA_QUEST_HINT_LIST_REQ = new PacketId(23, 9, 1, "C2S_AREA_GET_AREA_QUEST_HINT_LIST_REQ");
        public static readonly PacketId S2C_AREA_GET_AREA_QUEST_HINT_LIST_RES = new PacketId(23, 9, 2, "S2C_AREA_GET_AREA_QUEST_HINT_LIST_RES"); // クエスト情報リスト取得に
        public static readonly PacketId C2S_AREA_BUY_AREA_QUEST_HINT_REQ = new PacketId(23, 10, 1, "C2S_AREA_BUY_AREA_QUEST_HINT_REQ");
        public static readonly PacketId S2C_AREA_BUY_AREA_QUEST_HINT_RES = new PacketId(23, 10, 2, "S2C_AREA_BUY_AREA_QUEST_HINT_RES"); // エリア内クエスト情報購入に
        public static readonly PacketId C2S_AREA_GET_SPOT_INFO_LIST_REQ = new PacketId(23, 11, 1, "C2S_AREA_GET_SPOT_INFO_LIST_REQ");
        public static readonly PacketId S2C_AREA_GET_SPOT_INFO_LIST_RES = new PacketId(23, 11, 2, "S2C_AREA_GET_SPOT_INFO_LIST_RES"); // スポット情報リスト取得に
        public static readonly PacketId C2S_AREA_GET_AREA_BASE_INFO_LIST_REQ = new PacketId(23, 12, 1, "C2S_AREA_GET_AREA_BASE_INFO_LIST_REQ");
        public static readonly PacketId S2C_AREA_GET_AREA_BASE_INFO_LIST_RES = new PacketId(23, 12, 2, "S2C_AREA_GET_AREA_BASE_INFO_LIST_RES"); // エリアポイントリスト取得に
        public static readonly PacketId S2C_AREA_23_13_16_NTC = new PacketId(23, 13, 16, "S2C_AREA_23_13_16_NTC");
        public static readonly PacketId S2C_AREA_23_14_16_NTC = new PacketId(23, 14, 16, "S2C_AREA_23_14_16_NTC");
        public static readonly PacketId C2S_AREA_23_15_1_REQ = new PacketId(23, 15, 1, "C2S_AREA_23_15_1_REQ");
        public static readonly PacketId S2C_AREA_23_15_2_RES = new PacketId(23, 15, 2, "S2C_AREA_23_15_2_RES");
        public static readonly PacketId S2C_AREA_23_16_16_NTC = new PacketId(23, 16, 16, "S2C_AREA_23_16_16_NTC");

// Group: 24 - (JOB)
        public static readonly PacketId C2S_JOB_MASTER_GET_JOB_MASTER_ORDER_PROGRESS_REQ = new PacketId(24, 0, 1, "C2S_JOB_MASTER_GET_JOB_MASTER_ORDER_PROGRESS_REQ");
        public static readonly PacketId S2C_JOB_MASTER_GET_JOB_MASTER_ORDER_PROGRESS_RES = new PacketId(24, 0, 2, "S2C_JOB_MASTER_GET_JOB_MASTER_ORDER_PROGRESS_RES"); // ジョブマスター課題進捗取得に
        public static readonly PacketId C2S_JOB_MASTER_REPORT_JOB_ORDER_PROGRESS_REQ = new PacketId(24, 1, 1, "C2S_JOB_MASTER_REPORT_JOB_ORDER_PROGRESS_REQ");
        public static readonly PacketId S2C_JOB_MASTER_REPORT_JOB_ORDER_PROGRESS_RES = new PacketId(24, 1, 2, "S2C_JOB_MASTER_REPORT_JOB_ORDER_PROGRESS_RES"); // ジョブマスター課題報告に
        public static readonly PacketId C2S_JOB_MASTER_ACTIVATE_JOB_ORDER_REQ = new PacketId(24, 2, 1, "C2S_JOB_MASTER_ACTIVATE_JOB_ORDER_REQ");
        public static readonly PacketId S2C_JOB_MASTER_ACTIVATE_JOB_ORDER_RES = new PacketId(24, 2, 2, "S2C_JOB_MASTER_ACTIVATE_JOB_ORDER_RES"); // ジョブマスター課題のアクティブ化に
        public static readonly PacketId C2S_JOB_24_3_1_REQ = new PacketId(24, 3, 1, "C2S_JOB_24_3_1_REQ");
        public static readonly PacketId S2C_JOB_24_3_2_RES = new PacketId(24, 3, 2, "S2C_JOB_24_3_2_RES");
        public static readonly PacketId S2C_JOB_24_4_16_NTC = new PacketId(24, 4, 16, "S2C_JOB_24_4_16_NTC");
        public static readonly PacketId S2C_JOB_24_5_16_NTC = new PacketId(24, 5, 16, "S2C_JOB_24_5_16_NTC");
        public static readonly PacketId S2C_JOB_24_6_16_NTC = new PacketId(24, 6, 16, "S2C_JOB_24_6_16_NTC");

// Group: 25 - (ORB)
        public static readonly PacketId C2S_ORB_DEVOTE_GET_ALL_ORB_ELEMENT_LIST_REQ = new PacketId(25, 0, 1, "C2S_ORB_DEVOTE_GET_ALL_ORB_ELEMENT_LIST_REQ");
        public static readonly PacketId S2C_ORB_DEVOTE_GET_ALL_ORB_ELEMENT_LIST_RES = new PacketId(25, 0, 2, "S2C_ORB_DEVOTE_GET_ALL_ORB_ELEMENT_LIST_RES"); // オーブ成長要素リスト取得に
        public static readonly PacketId C2S_ORB_DEVOTE_GET_RELEASE_ORB_ELEMENT_LIST_REQ = new PacketId(25, 1, 1, "C2S_ORB_DEVOTE_GET_RELEASE_ORB_ELEMENT_LIST_REQ");
        public static readonly PacketId S2C_ORB_DEVOTE_GET_RELEASE_ORB_ELEMENT_LIST_RES = new PacketId(25, 1, 2, "S2C_ORB_DEVOTE_GET_RELEASE_ORB_ELEMENT_LIST_RES"); // 解放済みオーブ成長要素リスト取得に
        public static readonly PacketId C2S_ORB_DEVOTE_RELEASE_ORB_ELEMENT_REQ = new PacketId(25, 2, 1, "C2S_ORB_DEVOTE_RELEASE_ORB_ELEMENT_REQ");
        public static readonly PacketId S2C_ORB_DEVOTE_RELEASE_ORB_ELEMENT_RES = new PacketId(25, 2, 2, "S2C_ORB_DEVOTE_RELEASE_ORB_ELEMENT_RES"); // オーブ成長要素解放に
        public static readonly PacketId C2S_ORB_DEVOTE_GET_PAWN_RELEASE_ORB_ELEMENT_LIST_REQ = new PacketId(25, 3, 1, "C2S_ORB_DEVOTE_GET_PAWN_RELEASE_ORB_ELEMENT_LIST_REQ");
        public static readonly PacketId S2C_ORB_DEVOTE_GET_PAWN_RELEASE_ORB_ELEMENT_LIST_RES = new PacketId(25, 3, 2, "S2C_ORB_DEVOTE_GET_PAWN_RELEASE_ORB_ELEMENT_LIST_RES"); // ポーン用オーブ成長要素リスト取得に
        public static readonly PacketId C2S_ORB_DEVOTE_RELEASE_PAWN_ORB_ELEMENT_REQ = new PacketId(25, 4, 1, "C2S_ORB_DEVOTE_RELEASE_PAWN_ORB_ELEMENT_REQ");
        public static readonly PacketId S2C_ORB_DEVOTE_RELEASE_PAWN_ORB_ELEMENT_RES = new PacketId(25, 4, 2, "S2C_ORB_DEVOTE_RELEASE_PAWN_ORB_ELEMENT_RES"); // ポーン用オーブ捧げに
        public static readonly PacketId C2S_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_REQ = new PacketId(25, 5, 1, "C2S_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_REQ");
        public static readonly PacketId S2C_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_RES = new PacketId(25, 5, 2, "S2C_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_RES"); // オーブ成長による拡張パラメータ取得に
        public static readonly PacketId S2C_ORB_25_6_16_NTC = new PacketId(25, 6, 16, "S2C_ORB_25_6_16_NTC");

// Group: 26 - (PROFILE)
        public static readonly PacketId C2S_PROFILE_GET_CHARACTER_PROFILE_REQ = new PacketId(26, 0, 1, "C2S_PROFILE_GET_CHARACTER_PROFILE_REQ");
        public static readonly PacketId S2C_PROFILE_GET_CHARACTER_PROFILE_RES = new PacketId(26, 0, 2, "S2C_PROFILE_GET_CHARACTER_PROFILE_RES"); // 他人のキャラクター情報取得に
        public static readonly PacketId C2S_PROFILE_GET_MY_CHARACTER_PROFILE_REQ = new PacketId(26, 1, 1, "C2S_PROFILE_GET_MY_CHARACTER_PROFILE_REQ");
        public static readonly PacketId S2C_PROFILE_GET_MY_CHARACTER_PROFILE_RES = new PacketId(26, 1, 2, "S2C_PROFILE_GET_MY_CHARACTER_PROFILE_RES"); // 自分のキャラクター情報取得に
        public static readonly PacketId C2S_PROFILE_GET_AVAILABLE_BACKGROUND_LIST_REQ = new PacketId(26, 2, 1, "C2S_PROFILE_GET_AVAILABLE_BACKGROUND_LIST_REQ");
        public static readonly PacketId S2C_PROFILE_GET_AVAILABLE_BACKGROUND_LIST_RES = new PacketId(26, 2, 2, "S2C_PROFILE_GET_AVAILABLE_BACKGROUND_LIST_RES"); // 設定可能な背景画像リスト取得に
        public static readonly PacketId C2S_PROFILE_SET_ARISEN_PROFILE_REQ = new PacketId(26, 3, 1, "C2S_PROFILE_SET_ARISEN_PROFILE_REQ");
        public static readonly PacketId S2C_PROFILE_SET_ARISEN_PROFILE_RES = new PacketId(26, 3, 2, "S2C_PROFILE_SET_ARISEN_PROFILE_RES"); // 覚者カードカスタマイズ情報設定に
        public static readonly PacketId C2S_PROFILE_26_4_1_REQ = new PacketId(26, 4, 1, "C2S_PROFILE_26_4_1_REQ");
        public static readonly PacketId S2C_PROFILE_26_4_2_RES = new PacketId(26, 4, 2, "S2C_PROFILE_26_4_2_RES");
        public static readonly PacketId C2S_PROFILE_SET_PAWN_PROFILE_REQ = new PacketId(26, 5, 1, "C2S_PROFILE_SET_PAWN_PROFILE_REQ");
        public static readonly PacketId S2C_PROFILE_SET_PAWN_PROFILE_RES = new PacketId(26, 5, 2, "S2C_PROFILE_SET_PAWN_PROFILE_RES"); // ポーンカードカスタマイズ情報設定に
        public static readonly PacketId C2S_PROFILE_SET_PAWN_PROFILE_COMMENT_REQ = new PacketId(26, 6, 1, "C2S_PROFILE_SET_PAWN_PROFILE_COMMENT_REQ");
        public static readonly PacketId S2C_PROFILE_SET_PAWN_PROFILE_COMMENT_RES = new PacketId(26, 6, 2, "S2C_PROFILE_SET_PAWN_PROFILE_COMMENT_RES"); // ポーン紹介文設定に
        public static readonly PacketId C2S_PROFILE_UPDATE_ARISEN_PROFILE_SHARE_RANGE_REQ = new PacketId(26, 7, 1, "C2S_PROFILE_UPDATE_ARISEN_PROFILE_SHARE_RANGE_REQ");
        public static readonly PacketId S2C_PROFILE_UPDATE_ARISEN_PROFILE_SHARE_RANGE_RES = new PacketId(26, 7, 2, "S2C_PROFILE_UPDATE_ARISEN_PROFILE_SHARE_RANGE_RES"); // 覚者/ポーンカード公開範囲設定に
        public static readonly PacketId C2S_PROFILE_SET_OBJECTIVE_REQ = new PacketId(26, 8, 1, "C2S_PROFILE_SET_OBJECTIVE_REQ");
        public static readonly PacketId S2C_PROFILE_SET_OBJECTIVE_RES = new PacketId(26, 8, 2, "S2C_PROFILE_SET_OBJECTIVE_RES"); // 目的設定に
        public static readonly PacketId C2S_PROFILE_SET_MATCHING_PROFILE_REQ = new PacketId(26, 10, 1, "C2S_PROFILE_SET_MATCHING_PROFILE_REQ");
        public static readonly PacketId S2C_PROFILE_SET_MATCHING_PROFILE_RES = new PacketId(26, 10, 2, "S2C_PROFILE_SET_MATCHING_PROFILE_RES"); // マッチングプロフィール設定に
        public static readonly PacketId C2S_PROFILE_GET_MATCHING_PROFILE_REQ = new PacketId(26, 11, 1, "C2S_PROFILE_GET_MATCHING_PROFILE_REQ");
        public static readonly PacketId S2C_PROFILE_GET_MATCHING_PROFILE_RES = new PacketId(26, 11, 2, "S2C_PROFILE_GET_MATCHING_PROFILE_RES"); // マッチングプロフィール取得に
        public static readonly PacketId C2S_PROFILE_SET_SHORTCUT_LIST_REQ = new PacketId(26, 12, 1, "C2S_PROFILE_SET_SHORTCUT_LIST_REQ");
        public static readonly PacketId S2C_PROFILE_SET_SHORTCUT_LIST_RES = new PacketId(26, 12, 2, "S2C_PROFILE_SET_SHORTCUT_LIST_RES"); // ショートカットリスト設定に
        public static readonly PacketId C2S_PROFILE_GET_SHORTCUT_LIST_REQ = new PacketId(26, 13, 1, "C2S_PROFILE_GET_SHORTCUT_LIST_REQ");
        public static readonly PacketId S2C_PROFILE_GET_SHORTCUT_LIST_RES = new PacketId(26, 13, 2, "S2C_PROFILE_GET_SHORTCUT_LIST_RES"); // ショートカットリスト取得に
        public static readonly PacketId C2S_PROFILE_SET_COMMUNICATION_SHORTCUT_LIST_REQ = new PacketId(26, 14, 1, "C2S_PROFILE_SET_COMMUNICATION_SHORTCUT_LIST_REQ");
        public static readonly PacketId S2C_PROFILE_SET_COMMUNICATION_SHORTCUT_LIST_RES = new PacketId(26, 14, 2, "S2C_PROFILE_SET_COMMUNICATION_SHORTCUT_LIST_RES"); // コミュニケーションショートカットリスト設定に
        public static readonly PacketId C2S_PROFILE_GET_COMMUNICATION_SHORTCUT_LIST_REQ = new PacketId(26, 15, 1, "C2S_PROFILE_GET_COMMUNICATION_SHORTCUT_LIST_REQ");
        public static readonly PacketId S2C_PROFILE_GET_COMMUNICATION_SHORTCUT_LIST_RES = new PacketId(26, 15, 2, "S2C_PROFILE_GET_COMMUNICATION_SHORTCUT_LIST_RES"); // コミュニケーションショートカットリスト取得に
        public static readonly PacketId C2S_PROFILE_SET_MESSAGE_SET_REQ = new PacketId(26, 16, 1, "C2S_PROFILE_SET_MESSAGE_SET_REQ");
        public static readonly PacketId S2C_PROFILE_SET_MESSAGE_SET_RES = new PacketId(26, 16, 2, "S2C_PROFILE_SET_MESSAGE_SET_RES"); // メッセージセット設定に
        public static readonly PacketId C2S_PROFILE_GET_MESSAGE_SET_REQ = new PacketId(26, 17, 1, "C2S_PROFILE_GET_MESSAGE_SET_REQ");
        public static readonly PacketId S2C_PROFILE_GET_MESSAGE_SET_RES = new PacketId(26, 17, 2, "S2C_PROFILE_GET_MESSAGE_SET_RES"); // メッセージセット取得に
        public static readonly PacketId S2C_PROFILE_26_18_16_NTC = new PacketId(26, 18, 16, "S2C_PROFILE_26_18_16_NTC");

// Group: 27 - (ACHIEVEMENT)
        public static readonly PacketId C2S_ACHIEVEMENT_ACHIEVEMENT_GET_PROGRESS_LIST_REQ = new PacketId(27, 0, 1, "C2S_ACHIEVEMENT_ACHIEVEMENT_GET_PROGRESS_LIST_REQ");
        public static readonly PacketId S2C_ACHIEVEMENT_ACHIEVEMENT_GET_PROGRESS_LIST_RES = new PacketId(27, 0, 2, "S2C_ACHIEVEMENT_ACHIEVEMENT_GET_PROGRESS_LIST_RES"); // アチーブメント進捗リストの取得に
        public static readonly PacketId C2S_ACHIEVEMENT_ACHIEVEMENT_GET_REWARD_LIST_REQ = new PacketId(27, 1, 1, "C2S_ACHIEVEMENT_ACHIEVEMENT_GET_REWARD_LIST_REQ");
        public static readonly PacketId S2C_ACHIEVEMENT_ACHIEVEMENT_GET_REWARD_LIST_RES = new PacketId(27, 1, 2, "S2C_ACHIEVEMENT_ACHIEVEMENT_GET_REWARD_LIST_RES"); // アチーブメント報酬リストの取得に
        public static readonly PacketId C2S_ACHIEVEMENT_ACHIEVEMENT_GET_FURNITURE_REWARD_LIST_REQ = new PacketId(27, 2, 1, "C2S_ACHIEVEMENT_ACHIEVEMENT_GET_FURNITURE_REWARD_LIST_REQ");
        public static readonly PacketId S2C_ACHIEVEMENT_ACHIEVEMENT_GET_FURNITURE_REWARD_LIST_RES = new PacketId(27, 2, 2, "S2C_ACHIEVEMENT_ACHIEVEMENT_GET_FURNITURE_REWARD_LIST_RES"); // 家具アチーブメント報酬リスト取得に
        public static readonly PacketId C2S_ACHIEVEMENT_ACHIEVEMENT_GET_RECEIVABLE_REWARD_LIST_REQ = new PacketId(27, 3, 1, "C2S_ACHIEVEMENT_ACHIEVEMENT_GET_RECEIVABLE_REWARD_LIST_REQ");
        public static readonly PacketId S2C_ACHIEVEMENT_ACHIEVEMENT_GET_RECEIVABLE_REWARD_LIST_RES = new PacketId(27, 3, 2, "S2C_ACHIEVEMENT_ACHIEVEMENT_GET_RECEIVABLE_REWARD_LIST_RES"); // 受け取り可能なアチーブメント報酬リスト取得に
        public static readonly PacketId C2S_ACHIEVEMENT_ACHIEVEMENT_REWARD_RECEIVE_REQ = new PacketId(27, 4, 1, "C2S_ACHIEVEMENT_ACHIEVEMENT_REWARD_RECEIVE_REQ");
        public static readonly PacketId S2C_ACHIEVEMENT_ACHIEVEMENT_REWARD_RECEIVE_RES = new PacketId(27, 4, 2, "S2C_ACHIEVEMENT_ACHIEVEMENT_REWARD_RECEIVE_RES"); // アチーブメント報酬の受け取りに
        public static readonly PacketId C2S_ACHIEVEMENT_27_5_1_REQ = new PacketId(27, 5, 1, "C2S_ACHIEVEMENT_27_5_1_REQ");
        public static readonly PacketId S2C_ACHIEVEMENT_27_5_2_RES = new PacketId(27, 5, 2, "S2C_ACHIEVEMENT_27_5_2_RES");
        public static readonly PacketId C2S_ACHIEVEMENT_27_6_1_REQ = new PacketId(27, 6, 1, "C2S_ACHIEVEMENT_27_6_1_REQ");
        public static readonly PacketId S2C_ACHIEVEMENT_27_6_2_RES = new PacketId(27, 6, 2, "S2C_ACHIEVEMENT_27_6_2_RES");
        public static readonly PacketId S2C_ACHIEVEMENT_27_7_16_NTC = new PacketId(27, 7, 16, "S2C_ACHIEVEMENT_27_7_16_NTC");
        public static readonly PacketId S2C_ACHIEVEMENT_27_8_16_NTC = new PacketId(27, 8, 16, "S2C_ACHIEVEMENT_27_8_16_NTC");
        public static readonly PacketId S2C_ACHIEVEMENT_27_9_16_NTC = new PacketId(27, 9, 16, "S2C_ACHIEVEMENT_27_9_16_NTC");
        public static readonly PacketId C2S_ACHIEVEMENT_ACHIEVEMENT_GET_CATEGORY_PROGRESS_LIST_REQ = new PacketId(27, 10, 1, "C2S_ACHIEVEMENT_ACHIEVEMENT_GET_CATEGORY_PROGRESS_LIST_REQ");
        public static readonly PacketId S2C_ACHIEVEMENT_ACHIEVEMENT_GET_CATEGORY_PROGRESS_LIST_RES = new PacketId(27, 10, 2, "S2C_ACHIEVEMENT_ACHIEVEMENT_GET_CATEGORY_PROGRESS_LIST_RES"); // 指定カテゴリのアチーブメント進捗リストの取得に

// Group: 28 - (GP)
        public static readonly PacketId C2S_GP_GET_GP_REQ = new PacketId(28, 0, 1, "C2S_GP_GET_GP_REQ");
        public static readonly PacketId S2C_GP_GET_GP_RES = new PacketId(28, 0, 2, "S2C_GP_GET_GP_RES"); // GP取得要求に
        public static readonly PacketId C2S_GP_GET_GP_DETAIL_REQ = new PacketId(28, 1, 1, "C2S_GP_GET_GP_DETAIL_REQ");
        public static readonly PacketId S2C_GP_GET_GP_DETAIL_RES = new PacketId(28, 1, 2, "S2C_GP_GET_GP_DETAIL_RES"); // GP詳細取得
        public static readonly PacketId C2S_GP_28_2_1_REQ = new PacketId(28, 2, 1, "C2S_GP_28_2_1_REQ");
        public static readonly PacketId S2C_GP_28_2_2_RES = new PacketId(28, 2, 2, "S2C_GP_28_2_2_RES");
        public static readonly PacketId C2S_GP_GET_CAP_REQ = new PacketId(28, 3, 1, "C2S_GP_GET_CAP_REQ");
        public static readonly PacketId S2C_GP_GET_CAP_RES = new PacketId(28, 3, 2, "S2C_GP_GET_CAP_RES"); // CAP取得要求に
        public static readonly PacketId C2S_GP_GET_CAP_TO_GP_CHANGE_LIST_REQ = new PacketId(28, 4, 1, "C2S_GP_GET_CAP_TO_GP_CHANGE_LIST_REQ");
        public static readonly PacketId S2C_GP_GET_CAP_TO_GP_CHANGE_LIST_RES = new PacketId(28, 4, 2, "S2C_GP_GET_CAP_TO_GP_CHANGE_LIST_RES"); // CAPからGPへの変換テーブル取得要求に
        public static readonly PacketId C2S_GP_CHANGE_CAP_TO_GP_REQ = new PacketId(28, 5, 1, "C2S_GP_CHANGE_CAP_TO_GP_REQ");
        public static readonly PacketId S2C_GP_CHANGE_CAP_TO_GP_RES = new PacketId(28, 5, 2, "S2C_GP_CHANGE_CAP_TO_GP_RES"); // CAPからGP変換要求に
        public static readonly PacketId C2S_GP_COG_GET_ID_REQ = new PacketId(28, 6, 1, "C2S_GP_COG_GET_ID_REQ");
        public static readonly PacketId S2C_GP_COG_GET_ID_RES = new PacketId(28, 6, 2, "S2C_GP_COG_GET_ID_RES"); // COG ID取得要求に
        public static readonly PacketId C2S_GP_GP_SHOP_DISPLAY_GET_TYPE_REQ = new PacketId(28, 7, 1, "C2S_GP_GP_SHOP_DISPLAY_GET_TYPE_REQ");
        public static readonly PacketId S2C_GP_GP_SHOP_DISPLAY_GET_TYPE_RES = new PacketId(28, 7, 2, "S2C_GP_GP_SHOP_DISPLAY_GET_TYPE_RES"); // 黄金石ショップメニュー取得に
        public static readonly PacketId C2S_GP_GP_SHOP_DISPLAY_GET_LINEUP_REQ = new PacketId(28, 8, 1, "C2S_GP_GP_SHOP_DISPLAY_GET_LINEUP_REQ");
        public static readonly PacketId S2C_GP_GP_SHOP_DISPLAY_GET_LINEUP_RES = new PacketId(28, 8, 2, "S2C_GP_GP_SHOP_DISPLAY_GET_LINEUP_RES"); // 黄金石ショップ商品リスト取得に
        public static readonly PacketId C2S_GP_GP_SHOP_DISPLAY_BUY_REQ = new PacketId(28, 9, 1, "C2S_GP_GP_SHOP_DISPLAY_BUY_REQ");
        public static readonly PacketId S2C_GP_GP_SHOP_DISPLAY_BUY_RES = new PacketId(28, 9, 2, "S2C_GP_GP_SHOP_DISPLAY_BUY_RES"); // 黄金石ショップ購入後の結果取得に
        public static readonly PacketId C2S_GP_GP_SHOP_GET_COURSE_LINEUP_REQ = new PacketId(28, 10, 1, "C2S_GP_GP_SHOP_GET_COURSE_LINEUP_REQ");
        public static readonly PacketId S2C_GP_GP_SHOP_GET_COURSE_LINEUP_RES = new PacketId(28, 10, 2, "S2C_GP_GP_SHOP_GET_COURSE_LINEUP_RES"); // 黄金石ショップ：コースラインナップ取得要求に
        public static readonly PacketId C2S_GP_GP_SHOP_GET_ITEM_LINEUP_REQ = new PacketId(28, 11, 1, "C2S_GP_GP_SHOP_GET_ITEM_LINEUP_REQ");
        public static readonly PacketId S2C_GP_GP_SHOP_GET_ITEM_LINEUP_RES = new PacketId(28, 11, 2, "S2C_GP_GP_SHOP_GET_ITEM_LINEUP_RES"); // 黄金石ショップ：アイテムラインナップ取得要求に
        public static readonly PacketId C2S_GP_GP_SHOP_GET_PAWN_LINEUP_REQ = new PacketId(28, 12, 1, "C2S_GP_GP_SHOP_GET_PAWN_LINEUP_REQ");
        public static readonly PacketId S2C_GP_GP_SHOP_GET_PAWN_LINEUP_RES = new PacketId(28, 12, 2, "S2C_GP_GP_SHOP_GET_PAWN_LINEUP_RES"); // 黄金石ショップ：レジェンドポーンラインナップ取得要求に
        public static readonly PacketId C2S_GP_28_13_1_REQ = new PacketId(28, 13, 1, "C2S_GP_28_13_1_REQ");
        public static readonly PacketId S2C_GP_28_13_2_RES = new PacketId(28, 13, 2, "S2C_GP_28_13_2_RES");
        public static readonly PacketId C2S_GP_28_14_1_REQ = new PacketId(28, 14, 1, "C2S_GP_28_14_1_REQ");
        public static readonly PacketId S2C_GP_28_14_2_RES = new PacketId(28, 14, 2, "S2C_GP_28_14_2_RES");
        public static readonly PacketId C2S_GP_GP_SHOP_GET_BUY_HISTORY_REQ = new PacketId(28, 15, 1, "C2S_GP_GP_SHOP_GET_BUY_HISTORY_REQ");
        public static readonly PacketId S2C_GP_GP_SHOP_GET_BUY_HISTORY_RES = new PacketId(28, 15, 2, "S2C_GP_GP_SHOP_GET_BUY_HISTORY_RES"); // 黄金石ショップ：購入履歴取得要求に
        public static readonly PacketId C2S_GP_GP_SHOP_GET_CAP_CHARGE_URL_REQ = new PacketId(28, 16, 1, "C2S_GP_GP_SHOP_GET_CAP_CHARGE_URL_REQ");
        public static readonly PacketId S2C_GP_GP_SHOP_GET_CAP_CHARGE_URL_RES = new PacketId(28, 16, 2, "S2C_GP_GP_SHOP_GET_CAP_CHARGE_URL_RES"); // CAPチャージURL取得要求に
        public static readonly PacketId C2S_GP_GP_COURSE_GET_AVAILABLE_LIST_REQ = new PacketId(28, 17, 1, "C2S_GP_GP_COURSE_GET_AVAILABLE_LIST_REQ");
        public static readonly PacketId S2C_GP_GP_COURSE_GET_AVAILABLE_LIST_RES = new PacketId(28, 17, 2, "S2C_GP_GP_COURSE_GET_AVAILABLE_LIST_RES"); // 黄金石ショップ：所持リスト取得要求に
        public static readonly PacketId C2S_GP_GP_COURSE_GET_VALID_LIST_REQ = new PacketId(28, 18, 1, "C2S_GP_GP_COURSE_GET_VALID_LIST_REQ");
        public static readonly PacketId S2C_GP_GP_COURSE_GET_VALID_LIST_RES = new PacketId(28, 18, 2, "S2C_GP_GP_COURSE_GET_VALID_LIST_RES"); // 黄金石ショップ：使用中リスト取得要求に
        public static readonly PacketId C2S_GP_GP_COURSE_USE_FROM_AVAILABLE_REQ = new PacketId(28, 19, 1, "C2S_GP_GP_COURSE_USE_FROM_AVAILABLE_REQ");
        public static readonly PacketId S2C_GP_GP_COURSE_USE_FROM_AVAILABLE_RES = new PacketId(28, 19, 2, "S2C_GP_GP_COURSE_USE_FROM_AVAILABLE_RES"); // 黄金石ショップ：所持使用要求に
        public static readonly PacketId C2S_GP_GP_COURSE_GET_VERSION_REQ = new PacketId(28, 20, 1, "C2S_GP_GP_COURSE_GET_VERSION_REQ");
        public static readonly PacketId S2C_GP_GP_COURSE_GET_VERSION_RES = new PacketId(28, 20, 2, "S2C_GP_GP_COURSE_GET_VERSION_RES"); // 課金コースバージョン取得に
        public static readonly PacketId C2S_GP_28_21_1_REQ = new PacketId(28, 21, 1, "C2S_GP_28_21_1_REQ");
        public static readonly PacketId S2C_GP_28_21_2_RES = new PacketId(28, 21, 2, "S2C_GP_28_21_2_RES");
        public static readonly PacketId C2S_GP_GP_EDIT_GET_VOICE_LIST_REQ = new PacketId(28, 22, 1, "C2S_GP_GP_EDIT_GET_VOICE_LIST_REQ");
        public static readonly PacketId S2C_GP_GP_EDIT_GET_VOICE_LIST_RES = new PacketId(28, 22, 2, "S2C_GP_GP_EDIT_GET_VOICE_LIST_RES"); // 選択可能ボイスリスト取得に
        public static readonly PacketId C2S_GP_GP_SHOP_CAN_BUY_REQ = new PacketId(28, 23, 1, "C2S_GP_GP_SHOP_CAN_BUY_REQ");
        public static readonly PacketId S2C_GP_GP_SHOP_CAN_BUY_RES = new PacketId(28, 23, 2, "S2C_GP_GP_SHOP_CAN_BUY_RES"); // 課金商品購入可能状態の取得に
        public static readonly PacketId S2C_GP_28_24_16_NTC = new PacketId(28, 24, 16, "S2C_GP_28_24_16_NTC");
        public static readonly PacketId S2C_GP_28_25_16_NTC = new PacketId(28, 25, 16, "S2C_GP_28_25_16_NTC");
        public static readonly PacketId S2C_GP_28_26_16_NTC = new PacketId(28, 26, 16, "S2C_GP_28_26_16_NTC");
        public static readonly PacketId S2C_GP_28_27_16_NTC = new PacketId(28, 27, 16, "S2C_GP_28_27_16_NTC");
        public static readonly PacketId S2C_GP_28_28_16_NTC = new PacketId(28, 28, 16, "S2C_GP_28_28_16_NTC");
        public static readonly PacketId S2C_GP_28_29_16_NTC = new PacketId(28, 29, 16, "S2C_GP_28_29_16_NTC");
        public static readonly PacketId C2S_GP_GP_COURSE_EFFECT_MISMATCH_REQ = new PacketId(28, 30, 1, "C2S_GP_GP_COURSE_EFFECT_MISMATCH_REQ");
        public static readonly PacketId S2C_GP_GP_COURSE_EFFECT_MISMATCH_RES = new PacketId(28, 30, 2, "S2C_GP_GP_COURSE_EFFECT_MISMATCH_RES"); // 課金効能異常検知リクエスト完了
        public static readonly PacketId C2S_GP_GP_COURSE_INFO_RELOAD_REQ = new PacketId(28, 31, 1, "C2S_GP_GP_COURSE_INFO_RELOAD_REQ");
        public static readonly PacketId S2C_GP_GP_COURSE_INFO_RELOAD_RES = new PacketId(28, 31, 2, "S2C_GP_GP_COURSE_INFO_RELOAD_RES"); // 課金効能リスト取得要求完了
        public static readonly PacketId C2S_GP_GP_COURSE_EFFECT_RELOAD_REQ = new PacketId(28, 32, 1, "C2S_GP_GP_COURSE_EFFECT_RELOAD_REQ");
        public static readonly PacketId S2C_GP_GP_COURSE_EFFECT_RELOAD_RES = new PacketId(28, 32, 2, "S2C_GP_GP_COURSE_EFFECT_RELOAD_RES"); // 課金効能リロード要求完了
        public static readonly PacketId C2S_GP_GET_VALID_CHAT_COM_GROUP_REQ = new PacketId(28, 33, 1, "C2S_GP_GET_VALID_CHAT_COM_GROUP_REQ");
        public static readonly PacketId S2C_GP_GET_VALID_CHAT_COM_GROUP_RES = new PacketId(28, 33, 2, "S2C_GP_GET_VALID_CHAT_COM_GROUP_RES"); // 購入済み定型文のグループIDリスト取得
        public static readonly PacketId C2S_GP_GET_UPDATE_APP_COURSE_BONUS_FLAG_REQ = new PacketId(28, 34, 1, "C2S_GP_GET_UPDATE_APP_COURSE_BONUS_FLAG_REQ");
        public static readonly PacketId S2C_GP_GET_UPDATE_APP_COURSE_BONUS_FLAG_RES = new PacketId(28, 34, 2, "S2C_GP_GET_UPDATE_APP_COURSE_BONUS_FLAG_RES"); // アプリコースボーナスの更新が必要か
        public static readonly PacketId C2S_GP_UPDATE_APP_COURSE_BONUS_REQ = new PacketId(28, 35, 1, "C2S_GP_UPDATE_APP_COURSE_BONUS_REQ");
        public static readonly PacketId S2C_GP_UPDATE_APP_COURSE_BONUS_RES = new PacketId(28, 35, 2, "S2C_GP_UPDATE_APP_COURSE_BONUS_RES"); // アプリコースボーナスの更新

// Group: 29 - (EQUIP)
        public static readonly PacketId C2S_EQUIP_GET_CHARACTER_EQUIP_LIST_REQ = new PacketId(29, 0, 1, "C2S_EQUIP_GET_CHARACTER_EQUIP_LIST_REQ");
        public static readonly PacketId S2C_EQUIP_GET_CHARACTER_EQUIP_LIST_RES = new PacketId(29, 0, 2, "S2C_EQUIP_GET_CHARACTER_EQUIP_LIST_RES"); // 装備リスト取得に
        public static readonly PacketId C2S_EQUIP_CHANGE_CHARACTER_EQUIP_REQ = new PacketId(29, 1, 1, "C2S_EQUIP_CHANGE_CHARACTER_EQUIP_REQ");
        public static readonly PacketId S2C_EQUIP_CHANGE_CHARACTER_EQUIP_RES = new PacketId(29, 1, 2, "S2C_EQUIP_CHANGE_CHARACTER_EQUIP_RES"); // 装備変更に
        public static readonly PacketId S2C_EQUIP_29_1_16_NTC = new PacketId(29, 1, 16, "S2C_EQUIP_29_1_16_NTC");
        public static readonly PacketId C2S_EQUIP_CHANGE_CHARACTER_STORAGE_EQUIP_REQ = new PacketId(29, 2, 1, "C2S_EQUIP_CHANGE_CHARACTER_STORAGE_EQUIP_REQ");
        public static readonly PacketId S2C_EQUIP_CHANGE_CHARACTER_STORAGE_EQUIP_RES = new PacketId(29, 2, 2, "S2C_EQUIP_CHANGE_CHARACTER_STORAGE_EQUIP_RES"); // 倉庫装備変更に
        public static readonly PacketId C2S_EQUIP_GET_PAWN_EQUIP_LIST_REQ = new PacketId(29, 3, 1, "C2S_EQUIP_GET_PAWN_EQUIP_LIST_REQ");
        public static readonly PacketId S2C_EQUIP_GET_PAWN_EQUIP_LIST_RES = new PacketId(29, 3, 2, "S2C_EQUIP_GET_PAWN_EQUIP_LIST_RES"); // ポーン装備リスト取得に
        public static readonly PacketId C2S_EQUIP_CHANGE_PAWN_EQUIP_REQ = new PacketId(29, 4, 1, "C2S_EQUIP_CHANGE_PAWN_EQUIP_REQ");
        public static readonly PacketId S2C_EQUIP_CHANGE_PAWN_EQUIP_RES = new PacketId(29, 4, 2, "S2C_EQUIP_CHANGE_PAWN_EQUIP_RES"); // ポーン装備変更に
        public static readonly PacketId S2C_EQUIP_29_4_16_NTC = new PacketId(29, 4, 16, "S2C_EQUIP_29_4_16_NTC");
        public static readonly PacketId C2S_EQUIP_CHANGE_PAWN_STORAGE_EQUIP_REQ = new PacketId(29, 5, 1, "C2S_EQUIP_CHANGE_PAWN_STORAGE_EQUIP_REQ");
        public static readonly PacketId S2C_EQUIP_CHANGE_PAWN_STORAGE_EQUIP_RES = new PacketId(29, 5, 2, "S2C_EQUIP_CHANGE_PAWN_STORAGE_EQUIP_RES"); // ポーン倉庫装備変更に
        public static readonly PacketId C2S_EQUIP_CHANGE_CHARACTER_EQUIP_JOB_ITEM_REQ = new PacketId(29, 6, 1, "C2S_EQUIP_CHANGE_CHARACTER_EQUIP_JOB_ITEM_REQ");
        public static readonly PacketId S2C_EQUIP_CHANGE_CHARACTER_EQUIP_JOB_ITEM_RES = new PacketId(29, 6, 2, "S2C_EQUIP_CHANGE_CHARACTER_EQUIP_JOB_ITEM_RES"); // ジョブ専用アイテム装備変更に
        public static readonly PacketId S2C_EQUIP_29_6_16_NTC = new PacketId(29, 6, 16, "S2C_EQUIP_29_6_16_NTC");
        public static readonly PacketId C2S_EQUIP_CHANGE_PAWN_EQUIP_JOB_ITEM_REQ = new PacketId(29, 7, 1, "C2S_EQUIP_CHANGE_PAWN_EQUIP_JOB_ITEM_REQ");
        public static readonly PacketId S2C_EQUIP_CHANGE_PAWN_EQUIP_JOB_ITEM_RES = new PacketId(29, 7, 2, "S2C_EQUIP_CHANGE_PAWN_EQUIP_JOB_ITEM_RES"); // ポーンジョブ専用アイテム装備変更に
        public static readonly PacketId S2C_EQUIP_29_7_16_NTC = new PacketId(29, 7, 16, "S2C_EQUIP_29_7_16_NTC");
        public static readonly PacketId C2S_EQUIP_UPDATE_HIDE_CHARACTER_HEAD_ARMOR_REQ = new PacketId(29, 8, 1, "C2S_EQUIP_UPDATE_HIDE_CHARACTER_HEAD_ARMOR_REQ");
        public static readonly PacketId S2C_EQUIP_UPDATE_HIDE_CHARACTER_HEAD_ARMOR_RES = new PacketId(29, 8, 2, "S2C_EQUIP_UPDATE_HIDE_CHARACTER_HEAD_ARMOR_RES"); // 頭装備表示切り替えに
        public static readonly PacketId C2S_EQUIP_UPDATE_HIDE_PAWN_HEAD_ARMOR_REQ = new PacketId(29, 9, 1, "C2S_EQUIP_UPDATE_HIDE_PAWN_HEAD_ARMOR_REQ");
        public static readonly PacketId S2C_EQUIP_UPDATE_HIDE_PAWN_HEAD_ARMOR_RES = new PacketId(29, 9, 2, "S2C_EQUIP_UPDATE_HIDE_PAWN_HEAD_ARMOR_RES"); // ポーン頭装備表示切り替えに
        public static readonly PacketId C2S_EQUIP_UPDATE_HIDE_CHARACTER_LANTERN_REQ = new PacketId(29, 10, 1, "C2S_EQUIP_UPDATE_HIDE_CHARACTER_LANTERN_REQ");
        public static readonly PacketId S2C_EQUIP_UPDATE_HIDE_CHARACTER_LANTERN_RES = new PacketId(29, 10, 2, "S2C_EQUIP_UPDATE_HIDE_CHARACTER_LANTERN_RES"); // ランタン装備表示切り替えに
        public static readonly PacketId C2S_EQUIP_UPDATE_HIDE_PAWN_LANTERN_REQ = new PacketId(29, 11, 1, "C2S_EQUIP_UPDATE_HIDE_PAWN_LANTERN_REQ");
        public static readonly PacketId S2C_EQUIP_UPDATE_HIDE_PAWN_LANTERN_RES = new PacketId(29, 11, 2, "S2C_EQUIP_UPDATE_HIDE_PAWN_LANTERN_RES"); // ポーンランタン装備表示切り替えに
        public static readonly PacketId C2S_EQUIP_GET_EQUIP_PRESET_LIST_REQ = new PacketId(29, 12, 1, "C2S_EQUIP_GET_EQUIP_PRESET_LIST_REQ");
        public static readonly PacketId S2C_EQUIP_GET_EQUIP_PRESET_LIST_RES = new PacketId(29, 12, 2, "S2C_EQUIP_GET_EQUIP_PRESET_LIST_RES"); // 装備プリセットリスト取得に
        public static readonly PacketId C2S_EQUIP_UPDATE_EQUIP_PRESET_REQ = new PacketId(29, 13, 1, "C2S_EQUIP_UPDATE_EQUIP_PRESET_REQ");
        public static readonly PacketId S2C_EQUIP_UPDATE_EQUIP_PRESET_RES = new PacketId(29, 13, 2, "S2C_EQUIP_UPDATE_EQUIP_PRESET_RES"); // 装備プリセット更新に
        public static readonly PacketId C2S_EQUIP_UPDATE_EQUIP_PRESET_NAME_REQ = new PacketId(29, 14, 1, "C2S_EQUIP_UPDATE_EQUIP_PRESET_NAME_REQ");
        public static readonly PacketId S2C_EQUIP_UPDATE_EQUIP_PRESET_NAME_RES = new PacketId(29, 14, 2, "S2C_EQUIP_UPDATE_EQUIP_PRESET_NAME_RES"); // 装備プリセット名更新に
        public static readonly PacketId C2S_EQUIP_GET_CRAFT_LOCKED_ELEMENT_LIST_REQ = new PacketId(29, 15, 1, "C2S_EQUIP_GET_CRAFT_LOCKED_ELEMENT_LIST_REQ");
        public static readonly PacketId S2C_EQUIP_GET_CRAFT_LOCKED_ELEMENT_LIST_RES = new PacketId(29, 15, 2, "S2C_EQUIP_GET_CRAFT_LOCKED_ELEMENT_LIST_RES"); // ロックされているクレストリスト取得に
        public static readonly PacketId S2C_EQUIP_29_16_16_NTC = new PacketId(29, 16, 16, "S2C_EQUIP_29_16_16_NTC");
        public static readonly PacketId S2C_EQUIP_29_17_16_NTC = new PacketId(29, 17, 16, "S2C_EQUIP_29_17_16_NTC");
        public static readonly PacketId S2C_EQUIP_29_18_16_NTC = new PacketId(29, 18, 16, "S2C_EQUIP_29_18_16_NTC");
        public static readonly PacketId C2S_EQUIP_29_19_1_REQ = new PacketId(29, 19, 1, "C2S_EQUIP_29_19_1_REQ");
        public static readonly PacketId S2C_EQUIP_29_19_2_RES = new PacketId(29, 19, 2, "S2C_EQUIP_29_19_2_RES");
        public static readonly PacketId C2S_EQUIP_29_20_1_REQ = new PacketId(29, 20, 1, "C2S_EQUIP_29_20_1_REQ");
        public static readonly PacketId S2C_EQUIP_29_20_2_RES = new PacketId(29, 20, 2, "S2C_EQUIP_29_20_2_RES");
        public static readonly PacketId S2C_EQUIP_29_20_16_NTC = new PacketId(29, 20, 16, "S2C_EQUIP_29_20_16_NTC");
        public static readonly PacketId C2S_EQUIP_29_21_1_REQ = new PacketId(29, 21, 1, "C2S_EQUIP_29_21_1_REQ");
        public static readonly PacketId S2C_EQUIP_29_21_2_RES = new PacketId(29, 21, 2, "S2C_EQUIP_29_21_2_RES");
        public static readonly PacketId C2S_EQUIP_29_22_1_REQ = new PacketId(29, 22, 1, "C2S_EQUIP_29_22_1_REQ");
        public static readonly PacketId S2C_EQUIP_29_22_2_RES = new PacketId(29, 22, 2, "S2C_EQUIP_29_22_2_RES");
        public static readonly PacketId S2C_EQUIP_29_22_16_NTC = new PacketId(29, 22, 16, "S2C_EQUIP_29_22_16_NTC");
        public static readonly PacketId S2C_EQUIP_29_23_16_NTC = new PacketId(29, 23, 16, "S2C_EQUIP_29_23_16_NTC");

// Group: 30 - (CRAFT)
        public static readonly PacketId C2S_CRAFT_GET_CRAFT_PROGRESS_LIST_REQ = new PacketId(30, 0, 1, "C2S_CRAFT_GET_CRAFT_PROGRESS_LIST_REQ");
        public static readonly PacketId S2C_CRAFT_GET_CRAFT_PROGRESS_LIST_RES = new PacketId(30, 0, 2, "S2C_CRAFT_GET_CRAFT_PROGRESS_LIST_RES"); // クラフト状況リスト取得に
        public static readonly PacketId C2S_CRAFT_START_CRAFT_REQ = new PacketId(30, 1, 1, "C2S_CRAFT_START_CRAFT_REQ");
        public static readonly PacketId S2C_CRAFT_START_CRAFT_RES = new PacketId(30, 1, 2, "S2C_CRAFT_START_CRAFT_RES"); // クラフト開始に
        public static readonly PacketId C2S_CRAFT_GET_CRAFT_PRODUCT_INFO_REQ = new PacketId(30, 2, 1, "C2S_CRAFT_GET_CRAFT_PRODUCT_INFO_REQ");
        public static readonly PacketId S2C_CRAFT_GET_CRAFT_PRODUCT_INFO_RES = new PacketId(30, 2, 2, "S2C_CRAFT_GET_CRAFT_PRODUCT_INFO_RES"); // クラフト完成品情報取得に
        public static readonly PacketId C2S_CRAFT_GET_CRAFT_PRODUCT_REQ = new PacketId(30, 3, 1, "C2S_CRAFT_GET_CRAFT_PRODUCT_REQ");
        public static readonly PacketId S2C_CRAFT_GET_CRAFT_PRODUCT_RES = new PacketId(30, 3, 2, "S2C_CRAFT_GET_CRAFT_PRODUCT_RES"); // クラフト完成品受け取りに
        public static readonly PacketId C2S_CRAFT_CANCEL_CRAFT_REQ = new PacketId(30, 4, 1, "C2S_CRAFT_CANCEL_CRAFT_REQ");
        public static readonly PacketId S2C_CRAFT_CANCEL_CRAFT_RES = new PacketId(30, 4, 2, "S2C_CRAFT_CANCEL_CRAFT_RES"); // クラフト中断に
        public static readonly PacketId C2S_CRAFT_START_EQUIP_GRADE_UP_REQ = new PacketId(30, 5, 1, "C2S_CRAFT_START_EQUIP_GRADE_UP_REQ");
        public static readonly PacketId S2C_CRAFT_START_EQUIP_GRADE_UP_RES = new PacketId(30, 5, 2, "S2C_CRAFT_START_EQUIP_GRADE_UP_RES"); // 武具強化に
        public static readonly PacketId C2S_CRAFT_START_ATTACH_ELEMENT_REQ = new PacketId(30, 6, 1, "C2S_CRAFT_START_ATTACH_ELEMENT_REQ");
        public static readonly PacketId S2C_CRAFT_START_ATTACH_ELEMENT_RES = new PacketId(30, 6, 2, "S2C_CRAFT_START_ATTACH_ELEMENT_RES"); // エレメント付与に
        public static readonly PacketId C2S_CRAFT_START_DETACH_ELEMENT_REQ = new PacketId(30, 7, 1, "C2S_CRAFT_START_DETACH_ELEMENT_REQ");
        public static readonly PacketId S2C_CRAFT_START_DETACH_ELEMENT_RES = new PacketId(30, 7, 2, "S2C_CRAFT_START_DETACH_ELEMENT_RES"); // エレメント破棄に
        public static readonly PacketId C2S_CRAFT_START_EQUIP_COLOR_CHANGE_REQ = new PacketId(30, 8, 1, "C2S_CRAFT_START_EQUIP_COLOR_CHANGE_REQ");
        public static readonly PacketId S2C_CRAFT_START_EQUIP_COLOR_CHANGE_RES = new PacketId(30, 8, 2, "S2C_CRAFT_START_EQUIP_COLOR_CHANGE_RES"); // 武具カラー変更に
        public static readonly PacketId C2S_CRAFT_START_QUALITY_UP_REQ = new PacketId(30, 9, 1, "C2S_CRAFT_START_QUALITY_UP_REQ");
        public static readonly PacketId S2C_CRAFT_START_QUALITY_UP_RES = new PacketId(30, 9, 2, "S2C_CRAFT_START_QUALITY_UP_RES"); // クラフト品質改良要求に
        public static readonly PacketId C2S_CRAFT_CRAFT_SKILL_UP_REQ = new PacketId(30, 10, 1, "C2S_CRAFT_CRAFT_SKILL_UP_REQ");
        public static readonly PacketId S2C_CRAFT_CRAFT_SKILL_UP_RES = new PacketId(30, 10, 2, "S2C_CRAFT_CRAFT_SKILL_UP_RES"); // クラフトスキルアップに
        public static readonly PacketId C2S_CRAFT_30_11_1_REQ = new PacketId(30, 11, 1, "C2S_CRAFT_30_11_1_REQ");
        public static readonly PacketId S2C_CRAFT_30_11_2_RES = new PacketId(30, 11, 2, "S2C_CRAFT_30_11_2_RES");
        public static readonly PacketId C2S_CRAFT_RESET_CRAFTPOINT_REQ = new PacketId(30, 12, 1, "C2S_CRAFT_RESET_CRAFTPOINT_REQ");
        public static readonly PacketId S2C_CRAFT_RESET_CRAFTPOINT_RES = new PacketId(30, 12, 2, "S2C_CRAFT_RESET_CRAFTPOINT_RES"); // クラフトポイントリセットに
        public static readonly PacketId C2S_CRAFT_GET_CRAFT_SETTING_REQ = new PacketId(30, 13, 1, "C2S_CRAFT_GET_CRAFT_SETTING_REQ");
        public static readonly PacketId S2C_CRAFT_GET_CRAFT_SETTING_RES = new PacketId(30, 13, 2, "S2C_CRAFT_GET_CRAFT_SETTING_RES"); // クラフト設定取得要求に
        public static readonly PacketId C2S_CRAFT_CRAFT_TIME_SAVE_REQ = new PacketId(30, 14, 1, "C2S_CRAFT_CRAFT_TIME_SAVE_REQ");
        public static readonly PacketId S2C_CRAFT_CRAFT_TIME_SAVE_RES = new PacketId(30, 14, 2, "S2C_CRAFT_CRAFT_TIME_SAVE_RES"); // クラフト時間短縮要求に
        public static readonly PacketId C2S_CRAFT_GET_CRAFT_IR_COLLECTION_VALUE_LIST_REQ = new PacketId(30, 15, 1, "C2S_CRAFT_GET_CRAFT_IR_COLLECTION_VALUE_LIST_REQ");
        public static readonly PacketId S2C_CRAFT_GET_CRAFT_IR_COLLECTION_VALUE_LIST_RES = new PacketId(30, 15, 2, "S2C_CRAFT_GET_CRAFT_IR_COLLECTION_VALUE_LIST_RES"); // クラフトIR補正値リスト取得要求に
        public static readonly PacketId C2S_CRAFT_RELEASED_CRAFT_RECIPE_LIST_GET_REQ = new PacketId(30, 16, 1, "C2S_CRAFT_RELEASED_CRAFT_RECIPE_LIST_GET_REQ");
        public static readonly PacketId S2C_CRAFT_RELEASED_CRAFT_RECIPE_LIST_GET_RES = new PacketId(30, 16, 2, "S2C_CRAFT_RELEASED_CRAFT_RECIPE_LIST_GET_RES"); // 解放済みクラフトレシピリスト取得に
        public static readonly PacketId C2S_CRAFT_CRAFT_SKILL_ANALYZE_REQ = new PacketId(30, 17, 1, "C2S_CRAFT_CRAFT_SKILL_ANALYZE_REQ");
        public static readonly PacketId S2C_CRAFT_CRAFT_SKILL_ANALYZE_RES = new PacketId(30, 17, 2, "S2C_CRAFT_CRAFT_SKILL_ANALYZE_RES"); // クラフトスキル分析に
        public static readonly PacketId S2C_CRAFT_30_18_16_NTC = new PacketId(30, 18, 16, "S2C_CRAFT_30_18_16_NTC");
        public static readonly PacketId S2C_CRAFT_30_19_16_NTC = new PacketId(30, 19, 16, "S2C_CRAFT_30_19_16_NTC");
        public static readonly PacketId S2C_CRAFT_30_20_16_NTC = new PacketId(30, 20, 16, "S2C_CRAFT_30_20_16_NTC");
        public static readonly PacketId S2C_CRAFT_30_21_16_NTC = new PacketId(30, 21, 16, "S2C_CRAFT_30_21_16_NTC");
        public static readonly PacketId S2C_CRAFT_30_22_16_NTC = new PacketId(30, 22, 16, "S2C_CRAFT_30_22_16_NTC");
        public static readonly PacketId S2C_CRAFT_30_23_16_NTC = new PacketId(30, 23, 16, "S2C_CRAFT_30_23_16_NTC");
        public static readonly PacketId S2C_CRAFT_30_24_16_NTC = new PacketId(30, 24, 16, "S2C_CRAFT_30_24_16_NTC");
        public static readonly PacketId C2S_CRAFT_DRAGON_SKILL_COMPOSE_REQ = new PacketId(30, 25, 1, "C2S_CRAFT_DRAGON_SKILL_COMPOSE_REQ");
        public static readonly PacketId S2C_CRAFT_DRAGON_SKILL_COMPOSE_RES = new PacketId(30, 25, 2, "S2C_CRAFT_DRAGON_SKILL_COMPOSE_RES"); // 竜スキル合成要求に
        public static readonly PacketId C2S_CRAFT_DRAGON_SKILL_ANALYZE_REQ = new PacketId(30, 26, 1, "C2S_CRAFT_DRAGON_SKILL_ANALYZE_REQ");
        public static readonly PacketId S2C_CRAFT_DRAGON_SKILL_ANALYZE_RES = new PacketId(30, 26, 2, "S2C_CRAFT_DRAGON_SKILL_ANALYZE_RES"); // 竜スキル合成分析要求に

// Group: 31 - (CLAN)
        public static readonly PacketId C2S_CLAN_31_0_1_REQ = new PacketId(31, 0, 1, "C2S_CLAN_31_0_1_REQ");
        public static readonly PacketId S2C_CLAN_31_0_2_RES = new PacketId(31, 0, 2, "S2C_CLAN_31_0_2_RES");
        public static readonly PacketId C2S_CLAN_CLAN_CREATE_REQ = new PacketId(31, 1, 1, "C2S_CLAN_CLAN_CREATE_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_CREATE_RES = new PacketId(31, 1, 2, "S2C_CLAN_CLAN_CREATE_RES"); // クラン作成に
        public static readonly PacketId C2S_CLAN_CLAN_UPDATE_REQ = new PacketId(31, 2, 1, "C2S_CLAN_CLAN_UPDATE_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_UPDATE_RES = new PacketId(31, 2, 2, "S2C_CLAN_CLAN_UPDATE_RES"); // クラン更新に
        public static readonly PacketId S2C_CLAN_31_2_16_NTC = new PacketId(31, 2, 16, "S2C_CLAN_31_2_16_NTC");
        public static readonly PacketId C2S_CLAN_CLAN_GET_MEMBER_LIST_REQ = new PacketId(31, 3, 1, "C2S_CLAN_CLAN_GET_MEMBER_LIST_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_GET_MEMBER_LIST_RES = new PacketId(31, 3, 2, "S2C_CLAN_CLAN_GET_MEMBER_LIST_RES"); // クランメンバー取得に
        public static readonly PacketId C2S_CLAN_CLAN_GET_MY_MEMBER_LIST_REQ = new PacketId(31, 4, 1, "C2S_CLAN_CLAN_GET_MY_MEMBER_LIST_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_GET_MY_MEMBER_LIST_RES = new PacketId(31, 4, 2, "S2C_CLAN_CLAN_GET_MY_MEMBER_LIST_RES"); // 自分のクランメンバー取得に
        public static readonly PacketId C2S_CLAN_CLAN_SEARCH_REQ = new PacketId(31, 5, 1, "C2S_CLAN_CLAN_SEARCH_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SEARCH_RES = new PacketId(31, 5, 2, "S2C_CLAN_CLAN_SEARCH_RES"); // クラン検索に
        public static readonly PacketId C2S_CLAN_CLAN_GET_INFO_REQ = new PacketId(31, 6, 1, "C2S_CLAN_CLAN_GET_INFO_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_GET_INFO_RES = new PacketId(31, 6, 2, "S2C_CLAN_CLAN_GET_INFO_RES"); // クラン情報取得に
        public static readonly PacketId C2S_CLAN_CLAN_REGISTER_JOIN_REQ = new PacketId(31, 7, 1, "C2S_CLAN_CLAN_REGISTER_JOIN_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_REGISTER_JOIN_RES = new PacketId(31, 7, 2, "S2C_CLAN_CLAN_REGISTER_JOIN_RES"); // クラン加入申請に
        public static readonly PacketId S2C_CLAN_31_7_16_NTC = new PacketId(31, 7, 16, "S2C_CLAN_31_7_16_NTC");
        public static readonly PacketId C2S_CLAN_CLAN_CANCEL_JOIN_REQ = new PacketId(31, 8, 1, "C2S_CLAN_CLAN_CANCEL_JOIN_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_CANCEL_JOIN_RES = new PacketId(31, 8, 2, "S2C_CLAN_CLAN_CANCEL_JOIN_RES"); // クラン加入申請キャンセルに
        public static readonly PacketId S2C_CLAN_31_8_16_NTC = new PacketId(31, 8, 16, "S2C_CLAN_31_8_16_NTC");
        public static readonly PacketId C2S_CLAN_CLAN_GET_JOIN_REQUESTED_LIST_REQ = new PacketId(31, 9, 1, "C2S_CLAN_CLAN_GET_JOIN_REQUESTED_LIST_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_GET_JOIN_REQUESTED_LIST_RES = new PacketId(31, 9, 2, "S2C_CLAN_CLAN_GET_JOIN_REQUESTED_LIST_RES"); // クラン加入申請リスト取得に
        public static readonly PacketId C2S_CLAN_CLAN_GET_MY_JOIN_REQUEST_LIST_REQ = new PacketId(31, 10, 1, "C2S_CLAN_CLAN_GET_MY_JOIN_REQUEST_LIST_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_GET_MY_JOIN_REQUEST_LIST_RES = new PacketId(31, 10, 2, "S2C_CLAN_CLAN_GET_MY_JOIN_REQUEST_LIST_RES"); // クラン加入申請中リスト取得に
        public static readonly PacketId C2S_CLAN_CLAN_APPROVE_JOIN_REQ = new PacketId(31, 11, 1, "C2S_CLAN_CLAN_APPROVE_JOIN_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_APPROVE_JOIN_RES = new PacketId(31, 11, 2, "S2C_CLAN_CLAN_APPROVE_JOIN_RES"); // クラン加入許可(拒否)に
        public static readonly PacketId C2S_CLAN_CLAN_LEAVE_MEMBER_REQ = new PacketId(31, 12, 1, "C2S_CLAN_CLAN_LEAVE_MEMBER_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_LEAVE_MEMBER_RES = new PacketId(31, 12, 2, "S2C_CLAN_CLAN_LEAVE_MEMBER_RES"); // クラン脱退に
        public static readonly PacketId S2C_CLAN_31_12_16_NTC = new PacketId(31, 12, 16, "S2C_CLAN_31_12_16_NTC");
        public static readonly PacketId C2S_CLAN_CLAN_EXPEL_MEMBER_REQ = new PacketId(31, 13, 1, "C2S_CLAN_CLAN_EXPEL_MEMBER_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_EXPEL_MEMBER_RES = new PacketId(31, 13, 2, "S2C_CLAN_CLAN_EXPEL_MEMBER_RES"); // クラン除名に
        public static readonly PacketId C2S_CLAN_CLAN_NEGOTIATE_MASTER_REQ = new PacketId(31, 14, 1, "C2S_CLAN_CLAN_NEGOTIATE_MASTER_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_NEGOTIATE_MASTER_RES = new PacketId(31, 14, 2, "S2C_CLAN_CLAN_NEGOTIATE_MASTER_RES"); // クランマスター譲渡に
        public static readonly PacketId S2C_CLAN_31_14_16_NTC = new PacketId(31, 14, 16, "S2C_CLAN_31_14_16_NTC");
        public static readonly PacketId C2S_CLAN_CLAN_SET_MEMBER_RANK_REQ = new PacketId(31, 15, 1, "C2S_CLAN_CLAN_SET_MEMBER_RANK_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SET_MEMBER_RANK_RES = new PacketId(31, 15, 2, "S2C_CLAN_CLAN_SET_MEMBER_RANK_RES"); // クランメンバーランク設定に
        public static readonly PacketId S2C_CLAN_31_15_16_NTC = new PacketId(31, 15, 16, "S2C_CLAN_31_15_16_NTC");
        public static readonly PacketId C2S_CLAN_CLAN_GET_MEMBER_NUM_REQ = new PacketId(31, 16, 1, "C2S_CLAN_CLAN_GET_MEMBER_NUM_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_GET_MEMBER_NUM_RES = new PacketId(31, 16, 2, "S2C_CLAN_CLAN_GET_MEMBER_NUM_RES"); // クランメンバー数取得に
        public static readonly PacketId C2S_CLAN_CLAN_SETTING_UPDATE_REQ = new PacketId(31, 17, 1, "C2S_CLAN_CLAN_SETTING_UPDATE_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SETTING_UPDATE_RES = new PacketId(31, 17, 2, "S2C_CLAN_CLAN_SETTING_UPDATE_RES"); // クラン設定更新要求に
        public static readonly PacketId C2S_CLAN_CLAN_INVITE_REQ = new PacketId(31, 18, 1, "C2S_CLAN_CLAN_INVITE_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_INVITE_RES = new PacketId(31, 18, 2, "S2C_CLAN_CLAN_INVITE_RES"); // クラン直接勧誘に
        public static readonly PacketId S2C_CLAN_31_18_16_NTC = new PacketId(31, 18, 16, "S2C_CLAN_31_18_16_NTC");
        public static readonly PacketId C2S_CLAN_CLAN_INVITE_ACCEPT_REQ = new PacketId(31, 19, 1, "C2S_CLAN_CLAN_INVITE_ACCEPT_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_INVITE_ACCEPT_RES = new PacketId(31, 19, 2, "S2C_CLAN_CLAN_INVITE_ACCEPT_RES"); // クラン直接勧誘承認に
        public static readonly PacketId C2S_CLAN_CLAN_SCOUT_ENTRY_REGISTER_REQ = new PacketId(31, 20, 1, "C2S_CLAN_CLAN_SCOUT_ENTRY_REGISTER_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SCOUT_ENTRY_REGISTER_RES = new PacketId(31, 20, 2, "S2C_CLAN_CLAN_SCOUT_ENTRY_REGISTER_RES"); // クランスカウトエントリーに
        public static readonly PacketId C2S_CLAN_CLAN_SCOUT_ENTRY_CANCEL_REQ = new PacketId(31, 21, 1, "C2S_CLAN_CLAN_SCOUT_ENTRY_CANCEL_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SCOUT_ENTRY_CANCEL_RES = new PacketId(31, 21, 2, "S2C_CLAN_CLAN_SCOUT_ENTRY_CANCEL_RES"); // クランスカウトエントリー解除に
        public static readonly PacketId C2S_CLAN_CLAN_SCOUT_ENTRY_GET_MY_REQ = new PacketId(31, 22, 1, "C2S_CLAN_CLAN_SCOUT_ENTRY_GET_MY_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SCOUT_ENTRY_GET_MY_RES = new PacketId(31, 22, 2, "S2C_CLAN_CLAN_SCOUT_ENTRY_GET_MY_RES"); // 自分のクランスカウトエントリー取得に
        public static readonly PacketId C2S_CLAN_CLAN_SCOUT_ENTRY_SEARCH_REQ = new PacketId(31, 23, 1, "C2S_CLAN_CLAN_SCOUT_ENTRY_SEARCH_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SCOUT_ENTRY_SEARCH_RES = new PacketId(31, 23, 2, "S2C_CLAN_CLAN_SCOUT_ENTRY_SEARCH_RES"); // クランスカウトエントリーリスト取得に
        public static readonly PacketId C2S_CLAN_CLAN_SCOUT_ENTRY_INVITE_REQ = new PacketId(31, 24, 1, "C2S_CLAN_CLAN_SCOUT_ENTRY_INVITE_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SCOUT_ENTRY_INVITE_RES = new PacketId(31, 24, 2, "S2C_CLAN_CLAN_SCOUT_ENTRY_INVITE_RES"); // クラン勧誘に
        public static readonly PacketId S2C_CLAN_31_24_16_NTC = new PacketId(31, 24, 16, "S2C_CLAN_31_24_16_NTC");
        public static readonly PacketId C2S_CLAN_CLAN_SCOUT_ENTRY_GET_INVITE_LIST_REQ = new PacketId(31, 25, 1, "C2S_CLAN_CLAN_SCOUT_ENTRY_GET_INVITE_LIST_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SCOUT_ENTRY_GET_INVITE_LIST_RES = new PacketId(31, 25, 2, "S2C_CLAN_CLAN_SCOUT_ENTRY_GET_INVITE_LIST_RES"); // クラン勧誘中リスト取得に
        public static readonly PacketId C2S_CLAN_CLAN_SCOUT_ENTRY_CANCEL_INVITE_REQ = new PacketId(31, 26, 1, "C2S_CLAN_CLAN_SCOUT_ENTRY_CANCEL_INVITE_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SCOUT_ENTRY_CANCEL_INVITE_RES = new PacketId(31, 26, 2, "S2C_CLAN_CLAN_SCOUT_ENTRY_CANCEL_INVITE_RES"); // クラン勧誘キャンセルに
        public static readonly PacketId S2C_CLAN_31_26_16_NTC = new PacketId(31, 26, 16, "S2C_CLAN_31_26_16_NTC");
        public static readonly PacketId C2S_CLAN_CLAN_SCOUT_ENTRY_GET_INVITED_LIST_REQ = new PacketId(31, 27, 1, "C2S_CLAN_CLAN_SCOUT_ENTRY_GET_INVITED_LIST_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SCOUT_ENTRY_GET_INVITED_LIST_RES = new PacketId(31, 27, 2, "S2C_CLAN_CLAN_SCOUT_ENTRY_GET_INVITED_LIST_RES"); // クラン勧誘受けてるリスト取得に
        public static readonly PacketId C2S_CLAN_CLAN_SCOUT_ENTRY_APPROVE_INVITED_REQ = new PacketId(31, 28, 1, "C2S_CLAN_CLAN_SCOUT_ENTRY_APPROVE_INVITED_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SCOUT_ENTRY_APPROVE_INVITED_RES = new PacketId(31, 28, 2, "S2C_CLAN_CLAN_SCOUT_ENTRY_APPROVE_INVITED_RES"); // クラン勧誘承認(拒否)に
        public static readonly PacketId C2S_CLAN_CLAN_GET_MY_INFO_REQ = new PacketId(31, 29, 1, "C2S_CLAN_CLAN_GET_MY_INFO_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_GET_MY_INFO_RES = new PacketId(31, 29, 2, "S2C_CLAN_CLAN_GET_MY_INFO_RES"); // 自クラン取得に
        public static readonly PacketId C2S_CLAN_CLAN_GET_HISTORY_REQ = new PacketId(31, 30, 1, "C2S_CLAN_CLAN_GET_HISTORY_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_GET_HISTORY_RES = new PacketId(31, 30, 2, "S2C_CLAN_CLAN_GET_HISTORY_RES"); // クランヒストリー取得に
        public static readonly PacketId C2S_CLAN_CLAN_BASE_GET_INFO_REQ = new PacketId(31, 31, 1, "C2S_CLAN_CLAN_BASE_GET_INFO_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_BASE_GET_INFO_RES = new PacketId(31, 31, 2, "S2C_CLAN_CLAN_BASE_GET_INFO_RES"); // クラン拠点情報取得に
        public static readonly PacketId C2S_CLAN_CLAN_BASE_RELEASE_REQ = new PacketId(31, 32, 1, "C2S_CLAN_CLAN_BASE_RELEASE_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_BASE_RELEASE_RES = new PacketId(31, 32, 2, "S2C_CLAN_CLAN_BASE_RELEASE_RES"); // クラン拠点解放に
        public static readonly PacketId C2S_CLAN_CLAN_SHOP_GET_FUNCTION_ITEM_LIST_REQ = new PacketId(31, 33, 1, "C2S_CLAN_CLAN_SHOP_GET_FUNCTION_ITEM_LIST_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SHOP_GET_FUNCTION_ITEM_LIST_RES = new PacketId(31, 33, 2, "S2C_CLAN_CLAN_SHOP_GET_FUNCTION_ITEM_LIST_RES"); // クランショップ買い切り商品リスト取得に
        public static readonly PacketId C2S_CLAN_CLAN_SHOP_GET_BUFF_ITEM_LIST_REQ = new PacketId(31, 34, 1, "C2S_CLAN_CLAN_SHOP_GET_BUFF_ITEM_LIST_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SHOP_GET_BUFF_ITEM_LIST_RES = new PacketId(31, 34, 2, "S2C_CLAN_CLAN_SHOP_GET_BUFF_ITEM_LIST_RES"); // クランショップバフ商品リスト取得に
        public static readonly PacketId C2S_CLAN_CLAN_SHOP_BUY_FUNCTION_ITEM_REQ = new PacketId(31, 35, 1, "C2S_CLAN_CLAN_SHOP_BUY_FUNCTION_ITEM_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SHOP_BUY_FUNCTION_ITEM_RES = new PacketId(31, 35, 2, "S2C_CLAN_CLAN_SHOP_BUY_FUNCTION_ITEM_RES"); // クランショップ買い切り商品購入に
        public static readonly PacketId C2S_CLAN_CLAN_SHOP_BUY_BUFF_ITEM_REQ = new PacketId(31, 36, 1, "C2S_CLAN_CLAN_SHOP_BUY_BUFF_ITEM_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_SHOP_BUY_BUFF_ITEM_RES = new PacketId(31, 36, 2, "S2C_CLAN_CLAN_SHOP_BUY_BUFF_ITEM_RES"); // クランショップバフ商品購入に
        public static readonly PacketId C2S_CLAN_CLAN_CONCIERGE_UPDATE_REQ = new PacketId(31, 37, 1, "C2S_CLAN_CLAN_CONCIERGE_UPDATE_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_CONCIERGE_UPDATE_RES = new PacketId(31, 37, 2, "S2C_CLAN_CLAN_CONCIERGE_UPDATE_RES"); // クラン管理人更新に
        public static readonly PacketId C2S_CLAN_CLAN_CONCIERGE_GET_LIST_REQ = new PacketId(31, 38, 1, "C2S_CLAN_CLAN_CONCIERGE_GET_LIST_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_CONCIERGE_GET_LIST_RES = new PacketId(31, 38, 2, "S2C_CLAN_CLAN_CONCIERGE_GET_LIST_RES"); // クラン管理人リスト取得に
        public static readonly PacketId C2S_CLAN_CLAN_PARTNER_PAWN_LIST_GET_REQ = new PacketId(31, 39, 1, "C2S_CLAN_CLAN_PARTNER_PAWN_LIST_GET_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_PARTNER_PAWN_LIST_GET_RES = new PacketId(31, 39, 2, "S2C_CLAN_CLAN_PARTNER_PAWN_LIST_GET_RES"); // クランパートナーポーンリスト取得に
        public static readonly PacketId C2S_CLAN_CLAN_PARTNER_PAWN_DATA_GET_REQ = new PacketId(31, 40, 1, "C2S_CLAN_CLAN_PARTNER_PAWN_DATA_GET_REQ");
        public static readonly PacketId S2C_CLAN_CLAN_PARTNER_PAWN_DATA_GET_RES = new PacketId(31, 40, 2, "S2C_CLAN_CLAN_PARTNER_PAWN_DATA_GET_RES"); // クランパートナーポーンデータ取得に
        public static readonly PacketId S2C_CLAN_31_41_16_NTC = new PacketId(31, 41, 16, "S2C_CLAN_31_41_16_NTC");
        public static readonly PacketId S2C_CLAN_31_42_16_NTC = new PacketId(31, 42, 16, "S2C_CLAN_31_42_16_NTC");
        public static readonly PacketId S2C_CLAN_31_43_16_NTC = new PacketId(31, 43, 16, "S2C_CLAN_31_43_16_NTC");
        public static readonly PacketId S2C_CLAN_31_44_16_NTC = new PacketId(31, 44, 16, "S2C_CLAN_31_44_16_NTC");
        public static readonly PacketId S2C_CLAN_31_45_16_NTC = new PacketId(31, 45, 16, "S2C_CLAN_31_45_16_NTC");
        public static readonly PacketId S2C_CLAN_31_46_16_NTC = new PacketId(31, 46, 16, "S2C_CLAN_31_46_16_NTC");
        public static readonly PacketId S2C_CLAN_31_47_16_NTC = new PacketId(31, 47, 16, "S2C_CLAN_31_47_16_NTC");
        public static readonly PacketId S2C_CLAN_31_48_16_NTC = new PacketId(31, 48, 16, "S2C_CLAN_31_48_16_NTC");
        public static readonly PacketId S2C_CLAN_31_49_16_NTC = new PacketId(31, 49, 16, "S2C_CLAN_31_49_16_NTC");
        public static readonly PacketId S2C_CLAN_31_50_16_NTC = new PacketId(31, 50, 16, "S2C_CLAN_31_50_16_NTC");
        public static readonly PacketId S2C_CLAN_31_51_16_NTC = new PacketId(31, 51, 16, "S2C_CLAN_31_51_16_NTC");
        public static readonly PacketId C2S_CLAN_SET_FURNITURE_REQ = new PacketId(31, 52, 1, "C2S_CLAN_SET_FURNITURE_REQ");
        public static readonly PacketId S2C_CLAN_SET_FURNITURE_RES = new PacketId(31, 52, 2, "S2C_CLAN_SET_FURNITURE_RES"); // クラン家具の設置に
        public static readonly PacketId C2S_CLAN_GET_FURNITURE_REQ = new PacketId(31, 53, 1, "C2S_CLAN_GET_FURNITURE_REQ");
        public static readonly PacketId S2C_CLAN_GET_FURNITURE_RES = new PacketId(31, 53, 2, "S2C_CLAN_GET_FURNITURE_RES"); // クラン家具の設置情報取得に

// Group: 32 - (RANDOM)
        public static readonly PacketId C2S_RANDOM_STAGE_RANDOM_STAGE_GET_INFO_REQ = new PacketId(32, 0, 1, "C2S_RANDOM_STAGE_RANDOM_STAGE_GET_INFO_REQ");
        public static readonly PacketId S2C_RANDOM_STAGE_RANDOM_STAGE_GET_INFO_RES = new PacketId(32, 0, 2, "S2C_RANDOM_STAGE_RANDOM_STAGE_GET_INFO_RES"); // ランダムステージ情報取得に
        public static readonly PacketId C2S_RANDOM_STAGE_RANDOM_STAGE_CLEAR_INFO_REQ = new PacketId(32, 1, 1, "C2S_RANDOM_STAGE_RANDOM_STAGE_CLEAR_INFO_REQ");
        public static readonly PacketId S2C_RANDOM_STAGE_RANDOM_STAGE_CLEAR_INFO_RES = new PacketId(32, 1, 2, "S2C_RANDOM_STAGE_RANDOM_STAGE_CLEAR_INFO_RES");

// Group: 33 - (JOB)
        public static readonly PacketId C2S_JOB_GET_JOB_CHANGE_LIST_REQ = new PacketId(33, 0, 1, "C2S_JOB_GET_JOB_CHANGE_LIST_REQ");
        public static readonly PacketId S2C_JOB_GET_JOB_CHANGE_LIST_RES = new PacketId(33, 0, 2, "S2C_JOB_GET_JOB_CHANGE_LIST_RES"); // ジョブチェンジリスト取得に
        public static readonly PacketId C2S_JOB_CHANGE_JOB_REQ = new PacketId(33, 1, 1, "C2S_JOB_CHANGE_JOB_REQ");
        public static readonly PacketId S2C_JOB_CHANGE_JOB_RES = new PacketId(33, 1, 2, "S2C_JOB_CHANGE_JOB_RES"); // ジョブチェンジに
        public static readonly PacketId S2C_JOB_33_1_16_NTC = new PacketId(33, 1, 16, "S2C_JOB_33_1_16_NTC");
        public static readonly PacketId C2S_JOB_CHANGE_PAWN_JOB_REQ = new PacketId(33, 2, 1, "C2S_JOB_CHANGE_PAWN_JOB_REQ");
        public static readonly PacketId S2C_JOB_CHANGE_PAWN_JOB_RES = new PacketId(33, 2, 2, "S2C_JOB_CHANGE_PAWN_JOB_RES"); // ポーンジョブチェンジに
        public static readonly PacketId S2C_JOB_33_2_16_NTC = new PacketId(33, 2, 16, "S2C_JOB_33_2_16_NTC");
        public static readonly PacketId S2C_JOB_33_3_16_NTC = new PacketId(33, 3, 16, "S2C_JOB_33_3_16_NTC");
        public static readonly PacketId S2C_JOB_33_4_16_NTC = new PacketId(33, 4, 16, "S2C_JOB_33_4_16_NTC");
        public static readonly PacketId C2S_JOB_RESET_JOBPOINT_REQ = new PacketId(33, 5, 1, "C2S_JOB_RESET_JOBPOINT_REQ");
        public static readonly PacketId S2C_JOB_RESET_JOBPOINT_RES = new PacketId(33, 5, 2, "S2C_JOB_RESET_JOBPOINT_RES"); // ジョブポイントリセットに
        public static readonly PacketId C2S_JOB_GET_EXP_MODE_REQ = new PacketId(33, 6, 1, "C2S_JOB_GET_EXP_MODE_REQ");
        public static readonly PacketId S2C_JOB_GET_EXP_MODE_RES = new PacketId(33, 6, 2, "S2C_JOB_GET_EXP_MODE_RES"); // 経験値入手モードの取得に
        public static readonly PacketId C2S_JOB_UPDATE_EXP_MODE_REQ = new PacketId(33, 7, 1, "C2S_JOB_UPDATE_EXP_MODE_REQ");
        public static readonly PacketId S2C_JOB_UPDATE_EXP_MODE_RES = new PacketId(33, 7, 2, "S2C_JOB_UPDATE_EXP_MODE_RES"); // 経験値入手モード更新に
        public static readonly PacketId C2S_JOB_GET_PLAY_POINT_LIST_REQ = new PacketId(33, 8, 1, "C2S_JOB_GET_PLAY_POINT_LIST_REQ");
        public static readonly PacketId S2C_JOB_GET_PLAY_POINT_LIST_RES = new PacketId(33, 8, 2, "S2C_JOB_GET_PLAY_POINT_LIST_RES"); // プレイポイント情報取得に
        public static readonly PacketId C2S_JOB_JOB_VALUE_SHOP_GET_LINEUP_REQ = new PacketId(33, 9, 1, "C2S_JOB_JOB_VALUE_SHOP_GET_LINEUP_REQ");
        public static readonly PacketId S2C_JOB_JOB_VALUE_SHOP_GET_LINEUP_RES = new PacketId(33, 9, 2, "S2C_JOB_JOB_VALUE_SHOP_GET_LINEUP_RES"); // プレイポイントショップラインナップ取得
        public static readonly PacketId C2S_JOB_JOB_VALUE_SHOP_BUY_ITEM_REQ = new PacketId(33, 10, 1, "C2S_JOB_JOB_VALUE_SHOP_BUY_ITEM_REQ");
        public static readonly PacketId S2C_JOB_JOB_VALUE_SHOP_BUY_ITEM_RES = new PacketId(33, 10, 2, "S2C_JOB_JOB_VALUE_SHOP_BUY_ITEM_RES"); // プレイポイントショップ購入
        public static readonly PacketId S2C_JOB_33_11_16_NTC = new PacketId(33, 11, 16, "S2C_JOB_33_11_16_NTC");
        public static readonly PacketId S2C_JOB_33_12_16_NTC = new PacketId(33, 12, 16, "S2C_JOB_33_12_16_NTC");
        public static readonly PacketId S2C_JOB_33_13_16_NTC = new PacketId(33, 13, 16, "S2C_JOB_33_13_16_NTC");
        public static readonly PacketId S2C_JOB_33_14_16_NTC = new PacketId(33, 14, 16, "S2C_JOB_33_14_16_NTC");
        public static readonly PacketId S2C_JOB_33_15_16_NTC = new PacketId(33, 15, 16, "S2C_JOB_33_15_16_NTC");
        public static readonly PacketId S2C_JOB_33_16_16_NTC = new PacketId(33, 16, 16, "S2C_JOB_33_16_16_NTC");
        public static readonly PacketId S2C_JOB_33_17_16_NTC = new PacketId(33, 17, 16, "S2C_JOB_33_17_16_NTC");
        public static readonly PacketId S2C_JOB_33_18_16_NTC = new PacketId(33, 18, 16, "S2C_JOB_33_18_16_NTC");
        public static readonly PacketId S2C_JOB_33_19_16_NTC = new PacketId(33, 19, 16, "S2C_JOB_33_19_16_NTC");
        public static readonly PacketId S2C_JOB_33_20_16_NTC = new PacketId(33, 20, 16, "S2C_JOB_33_20_16_NTC");

// Group: 34 - (ENTRY)
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_LIST_REQ = new PacketId(34, 0, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_LIST_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_LIST_RES = new PacketId(34, 0, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_LIST_RES"); // ボード情報取得に
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_LIST_REQ = new PacketId(34, 1, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_LIST_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_LIST_RES = new PacketId(34, 1, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_LIST_RES"); // エントリーボードアイテムリスト取得に
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_CREATE_REQ = new PacketId(34, 2, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_CREATE_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_CREATE_RES = new PacketId(34, 2, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_CREATE_RES"); // エントリーボードアイテム作成に
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_RECREATE_REQ = new PacketId(34, 3, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_RECREATE_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_RECREATE_RES = new PacketId(34, 3, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_RECREATE_RES"); // エントリーボードアイテム再作成に
        public static readonly PacketId S2C_ENTRY_34_3_16_NTC = new PacketId(34, 3, 16, "S2C_ENTRY_34_3_16_NTC");
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_ENTRY_REQ = new PacketId(34, 4, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_ENTRY_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_ENTRY_RES = new PacketId(34, 4, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_ENTRY_RES"); // エントリーボードアイテム参加に
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_LEAVE_REQ = new PacketId(34, 5, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_LEAVE_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_LEAVE_RES = new PacketId(34, 5, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_LEAVE_RES"); // エントリーボードアイテムから抜けるに
        public static readonly PacketId S2C_ENTRY_34_5_16_NTC = new PacketId(34, 5, 16, "S2C_ENTRY_34_5_16_NTC");
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_READY_REQ = new PacketId(34, 6, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_READY_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_READY_RES = new PacketId(34, 6, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_READY_RES"); // エントリーボード準備完了に
        public static readonly PacketId S2C_ENTRY_34_6_16_NTC = new PacketId(34, 6, 16, "S2C_ENTRY_34_6_16_NTC");
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_FORCE_START_REQ = new PacketId(34, 7, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_FORCE_START_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_FORCE_START_RES = new PacketId(34, 7, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_FORCE_START_RES"); // エントリー強制開始に
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_REQ = new PacketId(34, 8, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_RES = new PacketId(34, 8, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_RES"); // エントリーボードアイテム情報取得に
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_MYSELF_REQ = new PacketId(34, 9, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_MYSELF_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_MYSELF_RES = new PacketId(34, 9, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_MYSELF_RES"); // 自身のエントリーボードアイテム情報取得に
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_EXTEND_TIMEOUT_REQ = new PacketId(34, 10, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_EXTEND_TIMEOUT_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_EXTEND_TIMEOUT_RES = new PacketId(34, 10, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_EXTEND_TIMEOUT_RES"); // エントリーボードアイテムタイムアウト延長に
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_LOCK_REQ = new PacketId(34, 11, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_LOCK_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_LOCK_RES = new PacketId(34, 11, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_LOCK_RES"); // エントリーボードアイテムロックに
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_CHANGE_REQ = new PacketId(34, 12, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_CHANGE_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_CHANGE_RES = new PacketId(34, 12, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_CHANGE_RES"); // エントリーボードアイテム情報変更に
        public static readonly PacketId S2C_ENTRY_34_12_16_NTC = new PacketId(34, 12, 16, "S2C_ENTRY_34_12_16_NTC");
        public static readonly PacketId C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INVITE_REQ = new PacketId(34, 13, 1, "C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INVITE_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INVITE_RES = new PacketId(34, 13, 2, "S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INVITE_RES"); // エントリーボード招待に
        public static readonly PacketId S2C_ENTRY_34_13_16_NTC = new PacketId(34, 13, 16, "S2C_ENTRY_34_13_16_NTC");
        public static readonly PacketId S2C_ENTRY_34_14_16_NTC = new PacketId(34, 14, 16, "S2C_ENTRY_34_14_16_NTC");
        public static readonly PacketId S2C_ENTRY_34_15_16_NTC = new PacketId(34, 15, 16, "S2C_ENTRY_34_15_16_NTC");
        public static readonly PacketId S2C_ENTRY_34_16_16_NTC = new PacketId(34, 16, 16, "S2C_ENTRY_34_16_16_NTC");
        public static readonly PacketId S2C_ENTRY_34_17_16_NTC = new PacketId(34, 17, 16, "S2C_ENTRY_34_17_16_NTC");
        public static readonly PacketId S2C_ENTRY_34_18_16_NTC = new PacketId(34, 18, 16, "S2C_ENTRY_34_18_16_NTC");
        public static readonly PacketId C2S_ENTRY_BOARD_PARTY_RECRUIT_CATEGORY_LIST_REQ = new PacketId(34, 19, 1, "C2S_ENTRY_BOARD_PARTY_RECRUIT_CATEGORY_LIST_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_PARTY_RECRUIT_CATEGORY_LIST_RES = new PacketId(34, 19, 2, "S2C_ENTRY_BOARD_PARTY_RECRUIT_CATEGORY_LIST_RES"); // パーティ募集掲示板リスト取得に
        public static readonly PacketId C2S_ENTRY_BOARD_ITEM_KICK_REQ = new PacketId(34, 20, 1, "C2S_ENTRY_BOARD_ITEM_KICK_REQ");
        public static readonly PacketId S2C_ENTRY_BOARD_ITEM_KICK_RES = new PacketId(34, 20, 2, "S2C_ENTRY_BOARD_ITEM_KICK_RES"); // エントリメンバーキックに

// Group: 35 - (CONTEXT)
        public static readonly PacketId C2S_CONTEXT_GET_LOBBY_PLAYER_CONTEXT_REQ = new PacketId(35, 0, 1, "C2S_CONTEXT_GET_LOBBY_PLAYER_CONTEXT_REQ");
        public static readonly PacketId S2C_CONTEXT_GET_LOBBY_PLAYER_CONTEXT_RES = new PacketId(35, 0, 2, "S2C_CONTEXT_GET_LOBBY_PLAYER_CONTEXT_RES"); // ロビープレイヤーコンテキスト取得に
        public static readonly PacketId S2C_CONTEXT_35_0_16_NTC = new PacketId(35, 0, 16, "S2C_CONTEXT_35_0_16_NTC");
        public static readonly PacketId C2S_CONTEXT_GET_PARTY_PLAYER_CONTEXT_REQ = new PacketId(35, 1, 1, "C2S_CONTEXT_GET_PARTY_PLAYER_CONTEXT_REQ");
        public static readonly PacketId S2C_CONTEXT_GET_PARTY_PLAYER_CONTEXT_RES = new PacketId(35, 1, 2, "S2C_CONTEXT_GET_PARTY_PLAYER_CONTEXT_RES"); // パーティプレイヤーコンテキスト取得に
        public static readonly PacketId S2C_CONTEXT_35_1_16_NTC = new PacketId(35, 1, 16, "S2C_CONTEXT_35_1_16_NTC");
        public static readonly PacketId C2S_CONTEXT_GET_ALL_PLAYER_CONTEXT_REQ = new PacketId(35, 2, 1, "C2S_CONTEXT_GET_ALL_PLAYER_CONTEXT_REQ");
        public static readonly PacketId S2C_CONTEXT_GET_ALL_PLAYER_CONTEXT_RES = new PacketId(35, 2, 2, "S2C_CONTEXT_GET_ALL_PLAYER_CONTEXT_RES"); // 全部入りプレイヤーコンテキスト取得に
        public static readonly PacketId S2C_CONTEXT_35_2_16_NTC = new PacketId(35, 2, 16, "S2C_CONTEXT_35_2_16_NTC");
        public static readonly PacketId C2S_CONTEXT_GET_PARTY_MYPAWN_CONTEXT_REQ = new PacketId(35, 3, 1, "C2S_CONTEXT_GET_PARTY_MYPAWN_CONTEXT_REQ");
        public static readonly PacketId S2C_CONTEXT_GET_PARTY_MYPAWN_CONTEXT_RES = new PacketId(35, 3, 2, "S2C_CONTEXT_GET_PARTY_MYPAWN_CONTEXT_RES"); // パーティマイポーンコンテキスト取得に
        public static readonly PacketId S2C_CONTEXT_35_3_16_NTC = new PacketId(35, 3, 16, "S2C_CONTEXT_35_3_16_NTC");
        public static readonly PacketId C2S_CONTEXT_GET_PARTY_RENTED_PAWN_CONTEXT_REQ = new PacketId(35, 4, 1, "C2S_CONTEXT_GET_PARTY_RENTED_PAWN_CONTEXT_REQ");
        public static readonly PacketId S2C_CONTEXT_GET_PARTY_RENTED_PAWN_CONTEXT_RES = new PacketId(35, 4, 2, "S2C_CONTEXT_GET_PARTY_RENTED_PAWN_CONTEXT_RES"); // パーティレンタル済みポーンコンテキスト取得に
        public static readonly PacketId S2C_CONTEXT_35_4_16_NTC = new PacketId(35, 4, 16, "S2C_CONTEXT_35_4_16_NTC");
        public static readonly PacketId C2S_CONTEXT_GET_SET_CONTEXT_REQ = new PacketId(35, 7, 1, "C2S_CONTEXT_GET_SET_CONTEXT_REQ");
        public static readonly PacketId S2C_CONTEXT_GET_SET_CONTEXT_RES = new PacketId(35, 7, 2, "S2C_CONTEXT_GET_SET_CONTEXT_RES"); // セットコンテキスト取得に
        public static readonly PacketId C2S_CONTEXT_MASTER_THROW_REQ = new PacketId(35, 10, 1, "C2S_CONTEXT_MASTER_THROW_REQ");
        public static readonly PacketId S2C_CONTEXT_MASTER_THROW_RES = new PacketId(35, 10, 2, "S2C_CONTEXT_MASTER_THROW_RES"); // マスター移譲に
        public static readonly PacketId S2C_CONTEXT_35_10_16_NTC = new PacketId(35, 10, 16, "S2C_CONTEXT_35_10_16_NTC");
        public static readonly PacketId S2C_CONTEXT_35_11_16_NTC = new PacketId(35, 11, 16, "S2C_CONTEXT_35_11_16_NTC");
        public static readonly PacketId S2C_CONTEXT_35_12_16_NTC = new PacketId(35, 12, 16, "S2C_CONTEXT_35_12_16_NTC");
        public static readonly PacketId S2C_CONTEXT_35_13_16_NTC = new PacketId(35, 13, 16, "S2C_CONTEXT_35_13_16_NTC");
        public static readonly PacketId S2C_CONTEXT_35_14_16_NTC = new PacketId(35, 14, 16, "S2C_CONTEXT_35_14_16_NTC");
        public static readonly PacketId S2C_CONTEXT_35_15_16_NTC = new PacketId(35, 15, 16, "S2C_CONTEXT_35_15_16_NTC");

// Group: 36 - (BAZAAR)
        public static readonly PacketId C2S_BAZAAR_GET_CHARACTER_LIST_REQ = new PacketId(36, 0, 1, "C2S_BAZAAR_GET_CHARACTER_LIST_REQ");
        public static readonly PacketId S2C_BAZAAR_GET_CHARACTER_LIST_RES = new PacketId(36, 0, 2, "S2C_BAZAAR_GET_CHARACTER_LIST_RES"); // バザー利用状況取得に
        public static readonly PacketId C2S_BAZAAR_GET_ITEM_LIST_REQ = new PacketId(36, 1, 1, "C2S_BAZAAR_GET_ITEM_LIST_REQ");
        public static readonly PacketId S2C_BAZAAR_GET_ITEM_LIST_RES = new PacketId(36, 1, 2, "S2C_BAZAAR_GET_ITEM_LIST_RES"); // バザーアイテム情報一覧取得に
        public static readonly PacketId C2S_BAZAAR_GET_ITEM_INFO_REQ = new PacketId(36, 2, 1, "C2S_BAZAAR_GET_ITEM_INFO_REQ");
        public static readonly PacketId S2C_BAZAAR_GET_ITEM_INFO_RES = new PacketId(36, 2, 2, "S2C_BAZAAR_GET_ITEM_INFO_RES"); // バザーアイテムごとのバザー情報取得に
        public static readonly PacketId C2S_BAZAAR_GET_ITEM_HISTORY_INFO_REQ = new PacketId(36, 3, 1, "C2S_BAZAAR_GET_ITEM_HISTORY_INFO_REQ");
        public static readonly PacketId S2C_BAZAAR_GET_ITEM_HISTORY_INFO_RES = new PacketId(36, 3, 2, "S2C_BAZAAR_GET_ITEM_HISTORY_INFO_RES"); // バザーアイテムごとのバザー履歴情報取得に
        public static readonly PacketId C2S_BAZAAR_EXHIBIT_REQ = new PacketId(36, 4, 1, "C2S_BAZAAR_EXHIBIT_REQ");
        public static readonly PacketId S2C_BAZAAR_EXHIBIT_RES = new PacketId(36, 4, 2, "S2C_BAZAAR_EXHIBIT_RES"); // バザー出品開始に
        public static readonly PacketId C2S_BAZAAR_RE_EXHIBIT_REQ = new PacketId(36, 5, 1, "C2S_BAZAAR_RE_EXHIBIT_REQ");
        public static readonly PacketId S2C_BAZAAR_RE_EXHIBIT_RES = new PacketId(36, 5, 2, "S2C_BAZAAR_RE_EXHIBIT_RES"); // バザー再出品開始に
        public static readonly PacketId C2S_BAZAAR_CANCEL_REQ = new PacketId(36, 6, 1, "C2S_BAZAAR_CANCEL_REQ");
        public static readonly PacketId S2C_BAZAAR_CANCEL_RES = new PacketId(36, 6, 2, "S2C_BAZAAR_CANCEL_RES"); // バザー出品キャンセルに
        public static readonly PacketId C2S_BAZAAR_PROCEEDS_REQ = new PacketId(36, 7, 1, "C2S_BAZAAR_PROCEEDS_REQ");
        public static readonly PacketId S2C_BAZAAR_PROCEEDS_RES = new PacketId(36, 7, 2, "S2C_BAZAAR_PROCEEDS_RES"); // バザーアイテム購入に
        public static readonly PacketId S2C_BAZAAR_36_7_16_NTC = new PacketId(36, 7, 16, "S2C_BAZAAR_36_7_16_NTC");
        public static readonly PacketId C2S_BAZAAR_RECEIVE_PROCEEDS_REQ = new PacketId(36, 8, 1, "C2S_BAZAAR_RECEIVE_PROCEEDS_REQ");
        public static readonly PacketId S2C_BAZAAR_RECEIVE_PROCEEDS_RES = new PacketId(36, 8, 2, "S2C_BAZAAR_RECEIVE_PROCEEDS_RES"); // バザー売上金受け取りに
        public static readonly PacketId C2S_BAZAAR_GET_ITEM_PRICE_LIMIT_REQ = new PacketId(36, 9, 1, "C2S_BAZAAR_GET_ITEM_PRICE_LIMIT_REQ");
        public static readonly PacketId S2C_BAZAAR_GET_ITEM_PRICE_LIMIT_RES = new PacketId(36, 9, 2, "S2C_BAZAAR_GET_ITEM_PRICE_LIMIT_RES"); // バザー出品価格制限取得
        public static readonly PacketId C2S_BAZAAR_36_10_1_REQ = new PacketId(36, 10, 1, "C2S_BAZAAR_36_10_1_REQ");
        public static readonly PacketId S2C_BAZAAR_36_10_2_RES = new PacketId(36, 10, 2, "S2C_BAZAAR_36_10_2_RES");
        public static readonly PacketId C2S_BAZAAR_GET_EXHIBIT_POSSIBLE_NUM_REQ = new PacketId(36, 11, 1, "C2S_BAZAAR_GET_EXHIBIT_POSSIBLE_NUM_REQ");
        public static readonly PacketId S2C_BAZAAR_GET_EXHIBIT_POSSIBLE_NUM_RES = new PacketId(36, 11, 2, "S2C_BAZAAR_GET_EXHIBIT_POSSIBLE_NUM_RES"); // バザー出品可能上限取得に

// Group: 37 - (MAIL)
        public static readonly PacketId C2S_MAIL_MAIL_GET_LIST_HEAD_REQ = new PacketId(37, 0, 1, "C2S_MAIL_MAIL_GET_LIST_HEAD_REQ");
        public static readonly PacketId S2C_MAIL_MAIL_GET_LIST_HEAD_RES = new PacketId(37, 0, 2, "S2C_MAIL_MAIL_GET_LIST_HEAD_RES"); // メールリストヘッダ取得に
        public static readonly PacketId C2S_MAIL_MAIL_GET_LIST_DATA_REQ = new PacketId(37, 1, 1, "C2S_MAIL_MAIL_GET_LIST_DATA_REQ");
        public static readonly PacketId S2C_MAIL_MAIL_GET_LIST_DATA_RES = new PacketId(37, 1, 2, "S2C_MAIL_MAIL_GET_LIST_DATA_RES"); // メールリスト本体取得に
        public static readonly PacketId C2S_MAIL_MAIL_GET_LIST_FOOT_REQ = new PacketId(37, 2, 1, "C2S_MAIL_MAIL_GET_LIST_FOOT_REQ");
        public static readonly PacketId S2C_MAIL_MAIL_GET_LIST_FOOT_RES = new PacketId(37, 2, 2, "S2C_MAIL_MAIL_GET_LIST_FOOT_RES"); // メールリスト取得終了に
        public static readonly PacketId C2S_MAIL_MAIL_GET_TEXT_REQ = new PacketId(37, 3, 1, "C2S_MAIL_MAIL_GET_TEXT_REQ");
        public static readonly PacketId S2C_MAIL_MAIL_GET_TEXT_RES = new PacketId(37, 3, 2, "S2C_MAIL_MAIL_GET_TEXT_RES"); // メール本文取得に
        public static readonly PacketId C2S_MAIL_MAIL_DELETE_REQ = new PacketId(37, 4, 1, "C2S_MAIL_MAIL_DELETE_REQ");
        public static readonly PacketId S2C_MAIL_MAIL_DELETE_RES = new PacketId(37, 4, 2, "S2C_MAIL_MAIL_DELETE_RES"); // メール削除に
        public static readonly PacketId C2S_MAIL_MAIL_SEND_REQ = new PacketId(37, 5, 1, "C2S_MAIL_MAIL_SEND_REQ");
        public static readonly PacketId S2C_MAIL_MAIL_SEND_RES = new PacketId(37, 5, 2, "S2C_MAIL_MAIL_SEND_RES"); // メール送信に
        public static readonly PacketId S2C_MAIL_37_5_16_NTC = new PacketId(37, 5, 16, "S2C_MAIL_37_5_16_NTC");
        public static readonly PacketId C2S_MAIL_SYSTEM_MAIL_GET_LIST_HEAD_REQ = new PacketId(37, 6, 1, "C2S_MAIL_SYSTEM_MAIL_GET_LIST_HEAD_REQ");
        public static readonly PacketId S2C_MAIL_SYSTEM_MAIL_GET_LIST_HEAD_RES = new PacketId(37, 6, 2, "S2C_MAIL_SYSTEM_MAIL_GET_LIST_HEAD_RES"); // お知らせメールリストヘッダ取得に
        public static readonly PacketId C2S_MAIL_SYSTEM_MAIL_GET_LIST_DATA_REQ = new PacketId(37, 7, 1, "C2S_MAIL_SYSTEM_MAIL_GET_LIST_DATA_REQ");
        public static readonly PacketId S2C_MAIL_SYSTEM_MAIL_GET_LIST_DATA_RES = new PacketId(37, 7, 2, "S2C_MAIL_SYSTEM_MAIL_GET_LIST_DATA_RES"); // お知らせメールリスト本体取得に
        public static readonly PacketId C2S_MAIL_SYSTEM_MAIL_GET_LIST_FOOT_REQ = new PacketId(37, 8, 1, "C2S_MAIL_SYSTEM_MAIL_GET_LIST_FOOT_REQ");
        public static readonly PacketId S2C_MAIL_SYSTEM_MAIL_GET_LIST_FOOT_RES = new PacketId(37, 8, 2, "S2C_MAIL_SYSTEM_MAIL_GET_LIST_FOOT_RES"); // お知らせメールリスト取得終了に
        public static readonly PacketId C2S_MAIL_SYSTEM_MAIL_GET_TEXT_REQ = new PacketId(37, 9, 1, "C2S_MAIL_SYSTEM_MAIL_GET_TEXT_REQ");
        public static readonly PacketId S2C_MAIL_SYSTEM_MAIL_GET_TEXT_RES = new PacketId(37, 9, 2, "S2C_MAIL_SYSTEM_MAIL_GET_TEXT_RES"); // お知らせメール本文取得に
        public static readonly PacketId C2S_MAIL_SYSTEM_MAIL_GET_ITEM_REQ = new PacketId(37, 10, 1, "C2S_MAIL_SYSTEM_MAIL_GET_ITEM_REQ");
        public static readonly PacketId S2C_MAIL_SYSTEM_MAIL_GET_ITEM_RES = new PacketId(37, 10, 2, "S2C_MAIL_SYSTEM_MAIL_GET_ITEM_RES"); // お知らせメール添付アイテム取得に
        public static readonly PacketId C2S_MAIL_SYSTEM_MAIL_GET_ALL_ITEM_REQ = new PacketId(37, 11, 1, "C2S_MAIL_SYSTEM_MAIL_GET_ALL_ITEM_REQ");
        public static readonly PacketId S2C_MAIL_SYSTEM_MAIL_GET_ALL_ITEM_RES = new PacketId(37, 11, 2, "S2C_MAIL_SYSTEM_MAIL_GET_ALL_ITEM_RES"); // お知らせメール添付アイテム全取得に
        public static readonly PacketId C2S_MAIL_SYSTEM_MAIL_DELETE_REQ = new PacketId(37, 12, 1, "C2S_MAIL_SYSTEM_MAIL_DELETE_REQ");
        public static readonly PacketId S2C_MAIL_SYSTEM_MAIL_DELETE_RES = new PacketId(37, 12, 2, "S2C_MAIL_SYSTEM_MAIL_DELETE_RES"); // お知らせメール削除に
        public static readonly PacketId S2C_MAIL_37_13_16_NTC = new PacketId(37, 13, 16, "S2C_MAIL_37_13_16_NTC");

// Group: 38 - (RANKING)
        public static readonly PacketId C2S_RANKING_BOARD_LIST_REQ = new PacketId(38, 0, 1, "C2S_RANKING_BOARD_LIST_REQ");
        public static readonly PacketId S2C_RANKING_BOARD_LIST_RES = new PacketId(38, 0, 2, "S2C_RANKING_BOARD_LIST_RES"); // ランキングボードリストの取得に
        public static readonly PacketId C2S_RANKING_RANK_LIST_REQ = new PacketId(38, 1, 1, "C2S_RANKING_RANK_LIST_REQ");
        public static readonly PacketId S2C_RANKING_RANK_LIST_RES = new PacketId(38, 1, 2, "S2C_RANKING_RANK_LIST_RES"); // ランキング情報リストを順位で取得に
        public static readonly PacketId C2S_RANKING_RANK_LIST_BY_QUEST_SCHEDULE_ID_REQ = new PacketId(38, 2, 1, "C2S_RANKING_RANK_LIST_BY_QUEST_SCHEDULE_ID_REQ");
        public static readonly PacketId S2C_RANKING_RANK_LIST_BY_QUEST_SCHEDULE_ID_RES = new PacketId(38, 2, 2, "S2C_RANKING_RANK_LIST_BY_QUEST_SCHEDULE_ID_RES"); // ランキング情報リストを順位で取得に
        public static readonly PacketId C2S_RANKING_RANK_LIST_BY_CHARACTER_ID_REQ = new PacketId(38, 3, 1, "C2S_RANKING_RANK_LIST_BY_CHARACTER_ID_REQ");
        public static readonly PacketId S2C_RANKING_RANK_LIST_BY_CHARACTER_ID_RES = new PacketId(38, 3, 2, "S2C_RANKING_RANK_LIST_BY_CHARACTER_ID_RES"); // ランキング情報リストをキャラクタIDで取得に
        public static readonly PacketId C2S_RANKING_38_4_1_REQ = new PacketId(38, 4, 1, "C2S_RANKING_38_4_1_REQ");
        public static readonly PacketId S2C_RANKING_38_4_2_RES = new PacketId(38, 4, 2, "S2C_RANKING_38_4_2_RES");

// Group: 39 - (GACHA)
        public static readonly PacketId C2S_GACHA_GACHA_LIST_REQ = new PacketId(39, 0, 1, "C2S_GACHA_GACHA_LIST_REQ");
        public static readonly PacketId S2C_GACHA_GACHA_LIST_RES = new PacketId(39, 0, 2, "S2C_GACHA_GACHA_LIST_RES"); // ガチャリスト取得に
        public static readonly PacketId C2S_GACHA_GACHA_BUY_REQ = new PacketId(39, 1, 1, "C2S_GACHA_GACHA_BUY_REQ");
        public static readonly PacketId S2C_GACHA_GACHA_BUY_RES = new PacketId(39, 1, 2, "S2C_GACHA_GACHA_BUY_RES"); // ガチャ購入取得に

// Group: 40
        public static readonly PacketId C2S_40_2_1_REQ = new PacketId(40, 2, 1, "C2S_40_2_1_REQ");
        public static readonly PacketId S2C_40_2_2_RES = new PacketId(40, 2, 2, "S2C_40_2_2_RES");

// Group: 41 - (CHARACTER)
        public static readonly PacketId C2S_CHARACTER_EDIT_GET_UNLOCKED_EDIT_PARTS_LIST_REQ = new PacketId(41, 0, 1, "C2S_CHARACTER_EDIT_GET_UNLOCKED_EDIT_PARTS_LIST_REQ");
        public static readonly PacketId S2C_CHARACTER_EDIT_GET_UNLOCKED_EDIT_PARTS_LIST_RES = new PacketId(41, 0, 2, "S2C_CHARACTER_EDIT_GET_UNLOCKED_EDIT_PARTS_LIST_RES"); // 使用可能エディットパーツリスト取得に
        public static readonly PacketId C2S_CHARACTER_EDIT_GET_UNLOCKED_PAWN_EDIT_PARTS_LIST_REQ = new PacketId(41, 1, 1, "C2S_CHARACTER_EDIT_GET_UNLOCKED_PAWN_EDIT_PARTS_LIST_REQ");
        public static readonly PacketId S2C_CHARACTER_EDIT_GET_UNLOCKED_PAWN_EDIT_PARTS_LIST_RES = new PacketId(41, 1, 2, "S2C_CHARACTER_EDIT_GET_UNLOCKED_PAWN_EDIT_PARTS_LIST_RES"); // 使用可能ポーンエディットパーツリスト取得に
        public static readonly PacketId C2S_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_REQ = new PacketId(41, 2, 1, "C2S_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_REQ");
        public static readonly PacketId S2C_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_RES = new PacketId(41, 2, 2, "S2C_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_RES"); // キャラクタエディット更新
        public static readonly PacketId C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_REQ = new PacketId(41, 3, 1, "C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_REQ");
        public static readonly PacketId S2C_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_RES = new PacketId(41, 3, 2, "S2C_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_RES"); // ポーンエディット更新
        public static readonly PacketId S2C_CHARACTER_41_4_16_NTC = new PacketId(41, 4, 16, "S2C_CHARACTER_41_4_16_NTC");
        public static readonly PacketId C2S_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_EX_REQ = new PacketId(41, 5, 1, "C2S_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_EX_REQ");
        public static readonly PacketId S2C_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_EX_RES = new PacketId(41, 5, 2, "S2C_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_EX_RES"); // キャラクタエディット拡張更新
        public static readonly PacketId C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_EX_REQ = new PacketId(41, 6, 1, "C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_EX_REQ");
        public static readonly PacketId S2C_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_EX_RES = new PacketId(41, 6, 2, "S2C_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_EX_RES"); // ポーンエディット拡張更新
        public static readonly PacketId S2C_CHARACTER_41_7_16_NTC = new PacketId(41, 7, 16, "S2C_CHARACTER_41_7_16_NTC");
        public static readonly PacketId C2S_CHARACTER_EDIT_GET_SHOP_PRICE_REQ = new PacketId(41, 8, 1, "C2S_CHARACTER_EDIT_GET_SHOP_PRICE_REQ");
        public static readonly PacketId S2C_CHARACTER_EDIT_GET_SHOP_PRICE_RES = new PacketId(41, 8, 2, "S2C_CHARACTER_EDIT_GET_SHOP_PRICE_RES"); // 美容院と従者の転生の価格の取得

// Group: 42 - (PHOTO)
        public static readonly PacketId C2S_PHOTO_PHOTO_GET_AUTH_ADDRESS_REQ = new PacketId(42, 1, 1, "C2S_PHOTO_PHOTO_GET_AUTH_ADDRESS_REQ");
        public static readonly PacketId S2C_PHOTO_PHOTO_GET_AUTH_ADDRESS_RES = new PacketId(42, 1, 2, "S2C_PHOTO_PHOTO_GET_AUTH_ADDRESS_RES"); // フォトアップロード認証用アドレス取得に

// Group: 43 - (LOADING)
        public static readonly PacketId C2S_LOADING_INFO_LOADING_GET_INFO_REQ = new PacketId(43, 0, 1, "C2S_LOADING_INFO_LOADING_GET_INFO_REQ");
        public static readonly PacketId S2C_LOADING_INFO_LOADING_GET_INFO_RES = new PacketId(43, 0, 2, "S2C_LOADING_INFO_LOADING_GET_INFO_RES"); // ロード画面情報取得応答

// Group: 44 - (CERT)
        public static readonly PacketId C2S_CERT_CLIENT_CHALLENGE_REQ = new PacketId(44, 0, 1, "C2S_CERT_CLIENT_CHALLENGE_REQ");
        public static readonly PacketId S2C_CERT_CLIENT_CHALLENGE_RES = new PacketId(44, 0, 2, "S2C_CERT_CLIENT_CHALLENGE_RES"); // 暗号化した共通鍵と共通鍵で暗号化した合言葉のやりとりに
        public static readonly PacketId S2C_CERT_44_1_16_NTC = new PacketId(44, 1, 16, "S2C_CERT_44_1_16_NTC");

// Group: 45 - (STAMP)
        public static readonly PacketId C2S_STAMP_BONUS_GET_LIST_REQ = new PacketId(45, 0, 1, "C2S_STAMP_BONUS_GET_LIST_REQ");
        public static readonly PacketId S2C_STAMP_BONUS_GET_LIST_RES = new PacketId(45, 0, 2, "S2C_STAMP_BONUS_GET_LIST_RES");
        public static readonly PacketId C2S_STAMP_BONUS_CHECK_REQ = new PacketId(45, 1, 1, "C2S_STAMP_BONUS_CHECK_REQ");
        public static readonly PacketId S2C_STAMP_BONUS_CHECK_RES = new PacketId(45, 1, 2, "S2C_STAMP_BONUS_CHECK_RES");
        public static readonly PacketId C2S_STAMP_BONUS_UPDATE_REQ = new PacketId(45, 2, 1, "C2S_STAMP_BONUS_UPDATE_REQ");
        public static readonly PacketId S2C_STAMP_BONUS_UPDATE_RES = new PacketId(45, 2, 2, "S2C_STAMP_BONUS_UPDATE_RES"); // スタンプボーナスの情報更新
        public static readonly PacketId C2S_STAMP_BONUS_RECIEVE_DAILY_REQ = new PacketId(45, 3, 1, "C2S_STAMP_BONUS_RECIEVE_DAILY_REQ");
        public static readonly PacketId S2C_STAMP_BONUS_RECIEVE_DAILY_RES = new PacketId(45, 3, 2, "S2C_STAMP_BONUS_RECIEVE_DAILY_RES"); // 連続スタンプボーナスの受取要求
        public static readonly PacketId C2S_STAMP_BONUS_RECIEVE_TOTAL_REQ = new PacketId(45, 4, 1, "C2S_STAMP_BONUS_RECIEVE_TOTAL_REQ");
        public static readonly PacketId S2C_STAMP_BONUS_RECIEVE_TOTAL_RES = new PacketId(45, 4, 2, "S2C_STAMP_BONUS_RECIEVE_TOTAL_RES"); // 累計スタンプボーナスの受取要求

// Group: 46 - (NG)
        public static readonly PacketId C2S_NG_WORD_NG_WORD_GET_LIST_REQ = new PacketId(46, 0, 1, "C2S_NG_WORD_NG_WORD_GET_LIST_REQ");
        public static readonly PacketId S2C_NG_WORD_NG_WORD_GET_LIST_RES = new PacketId(46, 0, 2, "S2C_NG_WORD_NG_WORD_GET_LIST_RES"); // 禁止文言リストの取得

// Group: 47 - (EVENT)
        public static readonly PacketId C2S_EVENT_CODE_EVENT_CODE_INPUT_REQ = new PacketId(47, 0, 1, "C2S_EVENT_CODE_EVENT_CODE_INPUT_REQ");
        public static readonly PacketId S2C_EVENT_CODE_EVENT_CODE_INPUT_RES = new PacketId(47, 0, 2, "S2C_EVENT_CODE_EVENT_CODE_INPUT_RES"); // イベントコードの入力に

// Group: 48 - (DLC)
        public static readonly PacketId C2S_DLC_DLC_GET_BOUGHT_REQ = new PacketId(48, 0, 1, "C2S_DLC_DLC_GET_BOUGHT_REQ");
        public static readonly PacketId S2C_DLC_DLC_GET_BOUGHT_RES = new PacketId(48, 0, 2, "S2C_DLC_DLC_GET_BOUGHT_RES"); // ＤＬＣ購入履歴取得
        public static readonly PacketId C2S_DLC_DLC_USE_REQ = new PacketId(48, 1, 1, "C2S_DLC_DLC_USE_REQ");
        public static readonly PacketId S2C_DLC_DLC_USE_RES = new PacketId(48, 1, 2, "S2C_DLC_DLC_USE_RES"); // ＤＬＣ商品の使用
        public static readonly PacketId C2S_DLC_DLC_GET_HISTORY_REQ = new PacketId(48, 2, 1, "C2S_DLC_DLC_GET_HISTORY_REQ");
        public static readonly PacketId S2C_DLC_DLC_GET_HISTORY_RES = new PacketId(48, 2, 2, "S2C_DLC_DLC_GET_HISTORY_RES"); // ＤＬＣ使用履歴取得
        public static readonly PacketId C2S_DLC_48_3_1_REQ = new PacketId(48, 3, 1, "C2S_DLC_48_3_1_REQ");
        public static readonly PacketId S2C_DLC_48_3_2_RES = new PacketId(48, 3, 2, "S2C_DLC_48_3_2_RES");
        public static readonly PacketId S2C_DLC_48_3_16_NTC = new PacketId(48, 3, 16, "S2C_DLC_48_3_16_NTC");
        public static readonly PacketId S2C_DLC_48_4_16_NTC = new PacketId(48, 4, 16, "S2C_DLC_48_4_16_NTC");

// Group: 49 - (SUPPORT)
        public static readonly PacketId C2S_SUPPORT_POINT_SUPPORT_POINT_GET_RATE_REQ = new PacketId(49, 0, 1, "C2S_SUPPORT_POINT_SUPPORT_POINT_GET_RATE_REQ");
        public static readonly PacketId S2C_SUPPORT_POINT_SUPPORT_POINT_GET_RATE_RES = new PacketId(49, 0, 2, "S2C_SUPPORT_POINT_SUPPORT_POINT_GET_RATE_RES"); // サポートポイント還元率リスト取得要求に
        public static readonly PacketId C2S_SUPPORT_POINT_SUPPORT_POINT_USE_REQ = new PacketId(49, 1, 1, "C2S_SUPPORT_POINT_SUPPORT_POINT_USE_REQ");
        public static readonly PacketId S2C_SUPPORT_POINT_SUPPORT_POINT_USE_RES = new PacketId(49, 1, 2, "S2C_SUPPORT_POINT_SUPPORT_POINT_USE_RES"); // サポートポイント使用に

// Group: 50 - (ITEM)
        public static readonly PacketId C2S_ITEM_SORT_GET_ITEM_SORTDATA_BIN_REQ = new PacketId(50, 0, 1, "C2S_ITEM_SORT_GET_ITEM_SORTDATA_BIN_REQ");
        public static readonly PacketId S2C_ITEM_SORT_GET_ITEM_SORTDATA_BIN_RES = new PacketId(50, 0, 2, "S2C_ITEM_SORT_GET_ITEM_SORTDATA_BIN_RES"); // アイテムソートデータ取得要求に
        public static readonly PacketId S2C_ITEM_50_0_16_NTC = new PacketId(50, 0, 16, "S2C_ITEM_50_0_16_NTC");
        public static readonly PacketId C2S_ITEM_SORT_SET_ITEM_SORTDATA_BIN_REQ = new PacketId(50, 1, 1, "C2S_ITEM_SORT_SET_ITEM_SORTDATA_BIN_REQ");
        public static readonly PacketId S2C_ITEM_SORT_SET_ITEM_SORTDATA_BIN_RES = new PacketId(50, 1, 2, "S2C_ITEM_SORT_SET_ITEM_SORTDATA_BIN_RES"); // アイテムソートデータ保存要求に

// Group: 51 - (DISPEL)
        public static readonly PacketId C2S_DISPEL_GET_DISPEL_ITEM_LIST_REQ = new PacketId(51, 0, 1, "C2S_DISPEL_GET_DISPEL_ITEM_LIST_REQ");
        public static readonly PacketId S2C_DISPEL_GET_DISPEL_ITEM_LIST_RES = new PacketId(51, 0, 2, "S2C_DISPEL_GET_DISPEL_ITEM_LIST_RES");
        public static readonly PacketId C2S_DISPEL_EXCHANGE_DISPEL_ITEM_REQ = new PacketId(51, 1, 1, "C2S_DISPEL_EXCHANGE_DISPEL_ITEM_REQ");
        public static readonly PacketId S2C_DISPEL_EXCHANGE_DISPEL_ITEM_RES = new PacketId(51, 1, 2, "S2C_DISPEL_EXCHANGE_DISPEL_ITEM_RES"); // 解呪アイテム交換に
        public static readonly PacketId C2S_DISPEL_GET_DISPEL_ITEM_SETTING_REQ = new PacketId(51, 2, 1, "C2S_DISPEL_GET_DISPEL_ITEM_SETTING_REQ");
        public static readonly PacketId S2C_DISPEL_GET_DISPEL_ITEM_SETTING_RES = new PacketId(51, 2, 2, "S2C_DISPEL_GET_DISPEL_ITEM_SETTING_RES"); // 解呪アイテム設定情報取得要求に
        public static readonly PacketId C2S_DISPEL_LOCK_SETTING_REQ = new PacketId(51, 3, 1, "C2S_DISPEL_LOCK_SETTING_REQ");
        public static readonly PacketId S2C_DISPEL_LOCK_SETTING_RES = new PacketId(51, 3, 2, "S2C_DISPEL_LOCK_SETTING_RES"); // 解呪ロック設定に
        public static readonly PacketId C2S_DISPEL_GET_LOCK_SETTING_REQ = new PacketId(51, 4, 1, "C2S_DISPEL_GET_LOCK_SETTING_REQ");
        public static readonly PacketId S2C_DISPEL_GET_LOCK_SETTING_RES = new PacketId(51, 4, 2, "S2C_DISPEL_GET_LOCK_SETTING_RES"); // 解呪ロック設定情報取得要求に

// Group: 52 - (MY)
        public static readonly PacketId C2S_MY_ROOM_MY_ROOM_RELEASE_REQ = new PacketId(52, 0, 1, "C2S_MY_ROOM_MY_ROOM_RELEASE_REQ");
        public static readonly PacketId S2C_MY_ROOM_MY_ROOM_RELEASE_RES = new PacketId(52, 0, 2, "S2C_MY_ROOM_MY_ROOM_RELEASE_RES"); // 自室解放に
        public static readonly PacketId C2S_MY_ROOM_FURNITURE_LIST_GET_REQ = new PacketId(52, 1, 1, "C2S_MY_ROOM_FURNITURE_LIST_GET_REQ");
        public static readonly PacketId S2C_MY_ROOM_FURNITURE_LIST_GET_RES = new PacketId(52, 1, 2, "S2C_MY_ROOM_FURNITURE_LIST_GET_RES"); // 自室家具リスト取得に
        public static readonly PacketId C2S_MY_ROOM_FURNITURE_LAYOUT_REQ = new PacketId(52, 2, 1, "C2S_MY_ROOM_FURNITURE_LAYOUT_REQ");
        public static readonly PacketId S2C_MY_ROOM_FURNITURE_LAYOUT_RES = new PacketId(52, 2, 2, "S2C_MY_ROOM_FURNITURE_LAYOUT_RES"); // 自室家具設置に
        public static readonly PacketId C2S_MY_ROOM_MY_ROOM_BGM_UPDATE_REQ = new PacketId(52, 3, 1, "C2S_MY_ROOM_MY_ROOM_BGM_UPDATE_REQ");
        public static readonly PacketId S2C_MY_ROOM_MY_ROOM_BGM_UPDATE_RES = new PacketId(52, 3, 2, "S2C_MY_ROOM_MY_ROOM_BGM_UPDATE_RES"); // 自室BGM変更に
        public static readonly PacketId C2S_MY_ROOM_OTHER_ROOM_LAYOUT_GET_REQ = new PacketId(52, 4, 1, "C2S_MY_ROOM_OTHER_ROOM_LAYOUT_GET_REQ");
        public static readonly PacketId S2C_MY_ROOM_OTHER_ROOM_LAYOUT_GET_RES = new PacketId(52, 4, 2, "S2C_MY_ROOM_OTHER_ROOM_LAYOUT_GET_RES"); // 他人の部屋情報取得
        public static readonly PacketId C2S_MY_52_5_1_REQ = new PacketId(52, 5, 1, "C2S_MY_52_5_1_REQ");
        public static readonly PacketId S2C_MY_52_5_2_RES = new PacketId(52, 5, 2, "S2C_MY_52_5_2_RES");
        public static readonly PacketId S2C_MY_52_5_16_NTC = new PacketId(52, 5, 16, "S2C_MY_52_5_16_NTC");
        public static readonly PacketId C2S_MY_ROOM_UPDATE_PLANETARIUM_REQ = new PacketId(52, 6, 1, "C2S_MY_ROOM_UPDATE_PLANETARIUM_REQ");
        public static readonly PacketId S2C_MY_ROOM_UPDATE_PLANETARIUM_RES = new PacketId(52, 6, 2, "S2C_MY_ROOM_UPDATE_PLANETARIUM_RES"); // 自室のプラネタリウムの状態の更新
        public static readonly PacketId C2S_MY_ROOM_GET_OTHER_ROOM_PERMISSION_REQ = new PacketId(52, 7, 1, "C2S_MY_ROOM_GET_OTHER_ROOM_PERMISSION_REQ");
        public static readonly PacketId S2C_MY_ROOM_GET_OTHER_ROOM_PERMISSION_RES = new PacketId(52, 7, 2, "S2C_MY_ROOM_GET_OTHER_ROOM_PERMISSION_RES"); // 他人自室の公開範囲の取得
        public static readonly PacketId C2S_MY_ROOM_SET_OTHER_ROOM_PERMISSION_REQ = new PacketId(52, 8, 1, "C2S_MY_ROOM_SET_OTHER_ROOM_PERMISSION_REQ");
        public static readonly PacketId S2C_MY_ROOM_SET_OTHER_ROOM_PERMISSION_RES = new PacketId(52, 8, 2, "S2C_MY_ROOM_SET_OTHER_ROOM_PERMISSION_RES"); // 他人自室の公開範囲の設定

// Group: 53 - (PARTNER)
        public static readonly PacketId C2S_PARTNER_PAWN_PARTNER_PAWN_SET_REQ = new PacketId(53, 0, 1, "C2S_PARTNER_PAWN_PARTNER_PAWN_SET_REQ");
        public static readonly PacketId S2C_PARTNER_PAWN_PARTNER_PAWN_SET_RES = new PacketId(53, 0, 2, "S2C_PARTNER_PAWN_PARTNER_PAWN_SET_RES"); // パートナーポーン設定に
        public static readonly PacketId C2S_PARTNER_PAWN_RELEASED_EDIT_PARTS_LIST_GET_REQ = new PacketId(53, 1, 1, "C2S_PARTNER_PAWN_RELEASED_EDIT_PARTS_LIST_GET_REQ");
        public static readonly PacketId S2C_PARTNER_PAWN_RELEASED_EDIT_PARTS_LIST_GET_RES = new PacketId(53, 1, 2, "S2C_PARTNER_PAWN_RELEASED_EDIT_PARTS_LIST_GET_RES"); // 解放済みエディットパーツリスト取得に
        public static readonly PacketId C2S_PARTNER_PAWN_RELEASED_EMOTION_LIST_GET_REQ = new PacketId(53, 2, 1, "C2S_PARTNER_PAWN_RELEASED_EMOTION_LIST_GET_REQ");
        public static readonly PacketId S2C_PARTNER_PAWN_RELEASED_EMOTION_LIST_GET_RES = new PacketId(53, 2, 2, "S2C_PARTNER_PAWN_RELEASED_EMOTION_LIST_GET_RES"); // 解放済みエモーションリスト取得に
        public static readonly PacketId C2S_PARTNER_PAWN_RELEASED_PAWN_TALK_LIST_GET_REQ = new PacketId(53, 3, 1, "C2S_PARTNER_PAWN_RELEASED_PAWN_TALK_LIST_GET_REQ");
        public static readonly PacketId S2C_PARTNER_PAWN_RELEASED_PAWN_TALK_LIST_GET_RES = new PacketId(53, 3, 2, "S2C_PARTNER_PAWN_RELEASED_PAWN_TALK_LIST_GET_RES"); // 解放済みパートナーポーン会話リスト取得に
        public static readonly PacketId C2S_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_LIST_GET_REQ = new PacketId(53, 4, 1, "C2S_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_LIST_GET_REQ");
        public static readonly PacketId S2C_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_LIST_GET_RES = new PacketId(53, 4, 2, "S2C_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_LIST_GET_RES"); // ポーン好感度による解放可能要素リスト取得に
        public static readonly PacketId C2S_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_GET_REQ = new PacketId(53, 5, 1, "C2S_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_GET_REQ");
        public static readonly PacketId S2C_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_GET_RES = new PacketId(53, 5, 2, "S2C_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_GET_RES"); // ポーン好感度による要素解放に
        public static readonly PacketId C2S_PARTNER_PAWN_PAWN_LIKABILITY_RELEASED_REWARD_LIST_GET_REQ = new PacketId(53, 6, 1, "C2S_PARTNER_PAWN_PAWN_LIKABILITY_RELEASED_REWARD_LIST_GET_REQ");
        public static readonly PacketId S2C_PARTNER_PAWN_PAWN_LIKABILITY_RELEASED_REWARD_LIST_GET_RES = new PacketId(53, 6, 2, "S2C_PARTNER_PAWN_PAWN_LIKABILITY_RELEASED_REWARD_LIST_GET_RES"); // ポーン好感度による解放済み要素リスト取得に
        public static readonly PacketId C2S_PARTNER_PAWN_PARTNER_PAWN_NEXT_PRESENT_TIME_GET_REQ = new PacketId(53, 7, 1, "C2S_PARTNER_PAWN_PARTNER_PAWN_NEXT_PRESENT_TIME_GET_REQ");
        public static readonly PacketId S2C_PARTNER_PAWN_PARTNER_PAWN_NEXT_PRESENT_TIME_GET_RES = new PacketId(53, 7, 2, "S2C_PARTNER_PAWN_PARTNER_PAWN_NEXT_PRESENT_TIME_GET_RES"); // 次に欠片を渡せるまでの時間(sec)を取得に
        public static readonly PacketId C2S_PARTNER_PAWN_PRESENT_FOR_PARTNER_PAWN_REQ = new PacketId(53, 8, 1, "C2S_PARTNER_PAWN_PRESENT_FOR_PARTNER_PAWN_REQ");
        public static readonly PacketId S2C_PARTNER_PAWN_PRESENT_FOR_PARTNER_PAWN_RES = new PacketId(53, 8, 2, "S2C_PARTNER_PAWN_PRESENT_FOR_PARTNER_PAWN_RES"); // パートナーポーンにプレゼントに
        public static readonly PacketId S2C_PARTNER_53_9_16_NTC = new PacketId(53, 9, 16, "S2C_PARTNER_53_9_16_NTC");

// Group: 54 - (CRAFT)
        public static readonly PacketId C2S_CRAFT_RECIPE_GET_CRAFT_RECIPE_REQ = new PacketId(54, 0, 1, "C2S_CRAFT_RECIPE_GET_CRAFT_RECIPE_REQ");
        public static readonly PacketId S2C_CRAFT_RECIPE_GET_CRAFT_RECIPE_RES = new PacketId(54, 0, 2, "S2C_CRAFT_RECIPE_GET_CRAFT_RECIPE_RES"); // クラフト生産レシピ取得に
        public static readonly PacketId C2S_CRAFT_RECIPE_GET_CRAFT_RECIPE_DESIGNATE_REQ = new PacketId(54, 1, 1, "C2S_CRAFT_RECIPE_GET_CRAFT_RECIPE_DESIGNATE_REQ");
        public static readonly PacketId S2C_CRAFT_RECIPE_GET_CRAFT_RECIPE_DESIGNATE_RES = new PacketId(54, 1, 2, "S2C_CRAFT_RECIPE_GET_CRAFT_RECIPE_DESIGNATE_RES"); // クラフト生産レシピ取得に
        public static readonly PacketId C2S_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_REQ = new PacketId(54, 2, 1, "C2S_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_REQ");
        public static readonly PacketId S2C_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_RES = new PacketId(54, 2, 2, "S2C_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_RES"); // クラフト強化レシピ取得に

// Group: 55 - (JOB)
        public static readonly PacketId C2S_JOB_ORB_TREE_GET_JOB_ORB_TREE_STATUS_LIST_REQ = new PacketId(55, 0, 1, "C2S_JOB_ORB_TREE_GET_JOB_ORB_TREE_STATUS_LIST_REQ");
        public static readonly PacketId S2C_JOB_ORB_TREE_GET_JOB_ORB_TREE_STATUS_LIST_RES = new PacketId(55, 0, 2, "S2C_JOB_ORB_TREE_GET_JOB_ORB_TREE_STATUS_LIST_RES"); // 全ジョブオーブツリーの状態リスト取得に
        public static readonly PacketId C2S_JOB_ORB_TREE_GET_ALL_JOB_ORB_ELEMENT_LIST_REQ = new PacketId(55, 1, 1, "C2S_JOB_ORB_TREE_GET_ALL_JOB_ORB_ELEMENT_LIST_REQ");
        public static readonly PacketId S2C_JOB_ORB_TREE_GET_ALL_JOB_ORB_ELEMENT_LIST_RES = new PacketId(55, 1, 2, "S2C_JOB_ORB_TREE_GET_ALL_JOB_ORB_ELEMENT_LIST_RES"); // 指定ジョブのジョブオーブツリー全報酬要素リスト取得に
        public static readonly PacketId C2S_JOB_ORB_TREE_RELEASE_JOB_ORB_ELEMENT_REQ = new PacketId(55, 2, 1, "C2S_JOB_ORB_TREE_RELEASE_JOB_ORB_ELEMENT_REQ");
        public static readonly PacketId S2C_JOB_ORB_TREE_RELEASE_JOB_ORB_ELEMENT_RES = new PacketId(55, 2, 2, "S2C_JOB_ORB_TREE_RELEASE_JOB_ORB_ELEMENT_RES"); // オーブ成長要素の解放に
        public static readonly PacketId C2S_JOB_ORB_TREE_GET_CURRENCY_EXCHANGE_LIST_REQ = new PacketId(55, 3, 1, "C2S_JOB_ORB_TREE_GET_CURRENCY_EXCHANGE_LIST_REQ");
        public static readonly PacketId S2C_JOB_ORB_TREE_GET_CURRENCY_EXCHANGE_LIST_RES = new PacketId(55, 3, 2, "S2C_JOB_ORB_TREE_GET_CURRENCY_EXCHANGE_LIST_RES"); // 通貨両替レート情報取得
        public static readonly PacketId C2S_JOB_ORB_TREE_EXCHANGE_CURRENCY_REQ = new PacketId(55, 4, 1, "C2S_JOB_ORB_TREE_EXCHANGE_CURRENCY_REQ");
        public static readonly PacketId S2C_JOB_ORB_TREE_EXCHANGE_CURRENCY_RES = new PacketId(55, 4, 2, "S2C_JOB_ORB_TREE_EXCHANGE_CURRENCY_RES"); // 通貨両替
        public static readonly PacketId S2C_JOB_55_5_16_NTC = new PacketId(55, 5, 16, "S2C_JOB_55_5_16_NTC");

// Group: 56 - (SERVER)
        public static readonly PacketId C2S_SERVER_UI_SERVER_UI_COMMAND_REQ = new PacketId(56, 0, 1, "C2S_SERVER_UI_SERVER_UI_COMMAND_REQ");
        public static readonly PacketId S2C_SERVER_UI_SERVER_UI_COMMAND_RES = new PacketId(56, 0, 2, "S2C_SERVER_UI_SERVER_UI_COMMAND_RES"); // サーバー制御UIコマンド応答
        public static readonly PacketId S2C_SERVER_56_1_16_NTC = new PacketId(56, 1, 16, "S2C_SERVER_56_1_16_NTC");

// Group: 57 - (BOX)
        public static readonly PacketId C2S_BOX_GACHA_BOX_GACHA_LIST_REQ = new PacketId(57, 0, 1, "C2S_BOX_GACHA_BOX_GACHA_LIST_REQ");
        public static readonly PacketId S2C_BOX_GACHA_BOX_GACHA_LIST_RES = new PacketId(57, 0, 2, "S2C_BOX_GACHA_BOX_GACHA_LIST_RES"); // BOXガチャリスト取得に
        public static readonly PacketId C2S_BOX_GACHA_BOX_GACHA_BUY_REQ = new PacketId(57, 1, 1, "C2S_BOX_GACHA_BOX_GACHA_BUY_REQ");
        public static readonly PacketId S2C_BOX_GACHA_BOX_GACHA_BUY_RES = new PacketId(57, 1, 2, "S2C_BOX_GACHA_BOX_GACHA_BUY_RES"); // BOXガチャ購入取得に
        public static readonly PacketId C2S_BOX_GACHA_BOX_GACHA_RESET_REQ = new PacketId(57, 2, 1, "C2S_BOX_GACHA_BOX_GACHA_RESET_REQ");
        public static readonly PacketId S2C_BOX_GACHA_BOX_GACHA_RESET_RES = new PacketId(57, 2, 2, "S2C_BOX_GACHA_BOX_GACHA_RESET_RES"); // BOXガチャリセットに
        public static readonly PacketId C2S_BOX_GACHA_BOX_GACHA_DRAW_INFO_REQ = new PacketId(57, 3, 1, "C2S_BOX_GACHA_BOX_GACHA_DRAW_INFO_REQ");
        public static readonly PacketId S2C_BOX_GACHA_BOX_GACHA_DRAW_INFO_RES = new PacketId(57, 3, 2, "S2C_BOX_GACHA_BOX_GACHA_DRAW_INFO_RES"); // BOXガチャアイテム情報に

// Group: 58 - (PAWN)
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_INFO_REQ = new PacketId(58, 0, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_INFO_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_INFO_RES = new PacketId(58, 0, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_INFO_RES"); // ポーン隊出撃情報取得
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_MY_SALLY_INFO_REQ = new PacketId(58, 1, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_MY_SALLY_INFO_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_MY_SALLY_INFO_RES = new PacketId(58, 1, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_MY_SALLY_INFO_RES"); // ポーン隊出撃情報取得に
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CHARGE_SALLY_COUNT_REQ = new PacketId(58, 2, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CHARGE_SALLY_COUNT_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CHARGE_SALLY_COUNT_RES = new PacketId(58, 2, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CHARGE_SALLY_COUNT_RES"); // ポーン隊出撃回数補充に
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_SALLY_REQ = new PacketId(58, 3, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_SALLY_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_SALLY_RES = new PacketId(58, 3, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_SALLY_RES"); // ポーン隊出撃に
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CHANGE_GOLDEN_SALLY_REQ = new PacketId(58, 4, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CHANGE_GOLDEN_SALLY_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CHANGE_GOLDEN_SALLY_RES = new PacketId(58, 4, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CHANGE_GOLDEN_SALLY_RES"); // ポーン隊黄金石出撃に
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_REWARD_REQ = new PacketId(58, 5, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_REWARD_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_REWARD_RES = new PacketId(58, 5, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_REWARD_RES"); // ポーン隊報酬リストの取得
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CANCEL_SALLY_REQ = new PacketId(58, 6, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CANCEL_SALLY_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CANCEL_SALLY_RES = new PacketId(58, 6, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CANCEL_SALLY_RES"); // ポーン隊の出撃キャンセルに
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_REQ = new PacketId(58, 7, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_RES = new PacketId(58, 7, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_RES"); // ポーン隊報酬有無・セット応答
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_LIST_REQ = new PacketId(58, 8, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_LIST_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_LIST_RES = new PacketId(58, 8, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_LIST_RES"); // ポーン隊報酬アイテムリスト応答
        public static readonly PacketId C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_REQ = new PacketId(58, 9, 1, "C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_REQ");
        public static readonly PacketId S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_RES = new PacketId(58, 9, 2, "S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_RES"); // ポーン隊報酬アイテム拾う応答

// Group: 59 - (INFINITY)
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_CURRENT_EVENT_REQ = new PacketId(59, 0, 1, "C2S_INFINITY_DELIVERY_GET_CURRENT_EVENT_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_CURRENT_EVENT_RES = new PacketId(59, 0, 2, "S2C_INFINITY_DELIVERY_GET_CURRENT_EVENT_RES"); // 開催中イベント取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_CATEGORY_FROM_NPC_REQ = new PacketId(59, 1, 1, "C2S_INFINITY_DELIVERY_GET_CATEGORY_FROM_NPC_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_CATEGORY_FROM_NPC_RES = new PacketId(59, 1, 2, "S2C_INFINITY_DELIVERY_GET_CATEGORY_FROM_NPC_RES"); // カテゴリ取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_REQUIRED_REQ = new PacketId(59, 2, 1, "C2S_INFINITY_DELIVERY_GET_REQUIRED_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_REQUIRED_RES = new PacketId(59, 2, 2, "S2C_INFINITY_DELIVERY_GET_REQUIRED_RES"); // 納品項目取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_DELIVER_CLIENT_REQ = new PacketId(59, 3, 1, "C2S_INFINITY_DELIVERY_DELIVER_CLIENT_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_DELIVER_CLIENT_RES = new PacketId(59, 3, 2, "S2C_INFINITY_DELIVERY_DELIVER_CLIENT_RES"); // 納品要求に
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_CATEGORY_STATUS_REQ = new PacketId(59, 4, 1, "C2S_INFINITY_DELIVERY_GET_CATEGORY_STATUS_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_CATEGORY_STATUS_RES = new PacketId(59, 4, 2, "S2C_INFINITY_DELIVERY_GET_CATEGORY_STATUS_RES"); // 納品状況取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_EVENT_STATUS_REQ = new PacketId(59, 5, 1, "C2S_INFINITY_DELIVERY_GET_EVENT_STATUS_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_EVENT_STATUS_RES = new PacketId(59, 5, 2, "S2C_INFINITY_DELIVERY_GET_EVENT_STATUS_RES"); // 納品状況取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_NUM_BORDER_INFO_REQ = new PacketId(59, 6, 1, "C2S_INFINITY_DELIVERY_GET_NUM_BORDER_INFO_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_NUM_BORDER_INFO_RES = new PacketId(59, 6, 2, "S2C_INFINITY_DELIVERY_GET_NUM_BORDER_INFO_RES"); // 大口納品個数ボーダー情報取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_RANKING_BORDER_INFO_REQ = new PacketId(59, 7, 1, "C2S_INFINITY_DELIVERY_GET_RANKING_BORDER_INFO_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_RANKING_BORDER_INFO_RES = new PacketId(59, 7, 2, "S2C_INFINITY_DELIVERY_GET_RANKING_BORDER_INFO_RES"); // 大口納品ランキングボーダー情報取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_CATEGORY_LIST_REQ = new PacketId(59, 8, 1, "C2S_INFINITY_DELIVERY_GET_CATEGORY_LIST_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_CATEGORY_LIST_RES = new PacketId(59, 8, 2, "S2C_INFINITY_DELIVERY_GET_CATEGORY_LIST_RES"); // 大口納品カテゴリリスト取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_CATEGORY_INFO_REQ = new PacketId(59, 9, 1, "C2S_INFINITY_DELIVERY_GET_CATEGORY_INFO_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_CATEGORY_INFO_RES = new PacketId(59, 9, 2, "S2C_INFINITY_DELIVERY_GET_CATEGORY_INFO_RES"); // 大口納品カテゴリ情報取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_GET_CATEGORY_RANKING_REQ = new PacketId(59, 10, 1, "C2S_INFINITY_DELIVERY_GET_CATEGORY_RANKING_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_GET_CATEGORY_RANKING_RES = new PacketId(59, 10, 2, "S2C_INFINITY_DELIVERY_GET_CATEGORY_RANKING_RES"); // 大口納品カテゴリランキング取得に
        public static readonly PacketId C2S_INFINITY_DELIVERY_RECEIVE_BORDER_REWARD_REQ = new PacketId(59, 11, 1, "C2S_INFINITY_DELIVERY_RECEIVE_BORDER_REWARD_REQ");
        public static readonly PacketId S2C_INFINITY_DELIVERY_RECEIVE_BORDER_REWARD_RES = new PacketId(59, 11, 2, "S2C_INFINITY_DELIVERY_RECEIVE_BORDER_REWARD_RES"); // 大口納品報酬受け取りに

// Group: 60 - (NPC)
        public static readonly PacketId C2S_NPC_GET_NPC_EXTENDED_FACILITY_REQ = new PacketId(60, 0, 1, "C2S_NPC_GET_NPC_EXTENDED_FACILITY_REQ");
        public static readonly PacketId S2C_NPC_GET_NPC_EXTENDED_FACILITY_RES = new PacketId(60, 0, 2, "S2C_NPC_GET_NPC_EXTENDED_FACILITY_RES"); // NPC追加施設機能取得
        public static readonly PacketId S2C_NPC_60_1_16_NTC = new PacketId(60, 1, 16, "S2C_NPC_60_1_16_NTC");

// Group: 61 - (PAWN)
        public static readonly PacketId C2S_PAWN_TRAINING_GET_TRAINING_STATUS_REQ = new PacketId(61, 0, 1, "C2S_PAWN_TRAINING_GET_TRAINING_STATUS_REQ");
        public static readonly PacketId S2C_PAWN_TRAINING_GET_TRAINING_STATUS_RES = new PacketId(61, 0, 2, "S2C_PAWN_TRAINING_GET_TRAINING_STATUS_RES"); // ポーン思考育成情報取得
        public static readonly PacketId C2S_PAWN_TRAINING_SET_TRAINING_STATUS_REQ = new PacketId(61, 1, 1, "C2S_PAWN_TRAINING_SET_TRAINING_STATUS_REQ");
        public static readonly PacketId S2C_PAWN_TRAINING_SET_TRAINING_STATUS_RES = new PacketId(61, 1, 2, "S2C_PAWN_TRAINING_SET_TRAINING_STATUS_RES"); // ポーン思考育成情報設定
        public static readonly PacketId C2S_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_REQ = new PacketId(61, 2, 1, "C2S_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_REQ");
        public static readonly PacketId S2C_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_RES = new PacketId(61, 2, 2, "S2C_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_RES"); // 指導前の情報の取得
        public static readonly PacketId S2C_PAWN_61_3_16_NTC = new PacketId(61, 3, 16, "S2C_PAWN_61_3_16_NTC");
        public static readonly PacketId S2C_PAWN_61_4_16_NTC = new PacketId(61, 4, 16, "S2C_PAWN_61_4_16_NTC");
        public static readonly PacketId S2C_PAWN_61_5_16_NTC = new PacketId(61, 5, 16, "S2C_PAWN_61_5_16_NTC");

// Group: 62 - (SEASON)
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_ID_FROM_NPC_ID_REQ = new PacketId(62, 0, 1, "C2S_SEASON_DUNGEON_GET_ID_FROM_NPC_ID_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_ID_FROM_NPC_ID_RES = new PacketId(62, 0, 2, "S2C_SEASON_DUNGEON_GET_ID_FROM_NPC_ID_RES"); // 納品状況取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_INFO_REQ = new PacketId(62, 1, 1, "C2S_SEASON_DUNGEON_GET_INFO_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_INFO_RES = new PacketId(62, 1, 2, "S2C_SEASON_DUNGEON_GET_INFO_RES"); // 納品状況取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_EXTEND_INFO_REQ = new PacketId(62, 2, 1, "C2S_SEASON_DUNGEON_GET_EXTEND_INFO_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_EXTEND_INFO_RES = new PacketId(62, 2, 2, "S2C_SEASON_DUNGEON_GET_EXTEND_INFO_RES"); // シーズンダンジョン拡張情報取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_COMPLETE_REQ = new PacketId(62, 3, 1, "C2S_SEASON_DUNGEON_COMPLETE_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_COMPLETE_RES = new PacketId(62, 3, 2, "S2C_SEASON_DUNGEON_COMPLETE_RES"); // シーズンダンジョンコンプリートに
        public static readonly PacketId C2S_SEASON_DUNGEON_OPEN_COMPLETE_DOOR_REQ = new PacketId(62, 4, 1, "C2S_SEASON_DUNGEON_OPEN_COMPLETE_DOOR_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_OPEN_COMPLETE_DOOR_RES = new PacketId(62, 4, 2, "S2C_SEASON_DUNGEON_OPEN_COMPLETE_DOOR_RES"); // シーズンダンジョンコンプリート扉開閉に
        public static readonly PacketId C2S_SEASON_DUNGEON_EXEC_EX_RESERVATION_REQ = new PacketId(62, 5, 1, "C2S_SEASON_DUNGEON_EXEC_EX_RESERVATION_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_EXEC_EX_RESERVATION_RES = new PacketId(62, 5, 2, "S2C_SEASON_DUNGEON_EXEC_EX_RESERVATION_RES"); // 納品状況取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_NPC_ID_REQ = new PacketId(62, 6, 1, "C2S_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_NPC_ID_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_NPC_ID_RES = new PacketId(62, 6, 2, "S2C_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_NPC_ID_RES"); // 納品状況取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_EXTENDABLE_BLOCKADE_LIST_FROM_NPC_ID_REQ = new PacketId(62, 7, 1, "C2S_SEASON_DUNGEON_GET_EXTENDABLE_BLOCKADE_LIST_FROM_NPC_ID_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_EXTENDABLE_BLOCKADE_LIST_FROM_NPC_ID_RES = new PacketId(62, 7, 2, "S2C_SEASON_DUNGEON_GET_EXTENDABLE_BLOCKADE_LIST_FROM_NPC_ID_RES"); // シーズンダンジョン開拓可能封鎖リストの取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_OM_REQ = new PacketId(62, 8, 1, "C2S_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_OM_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_OM_RES = new PacketId(62, 8, 2, "S2C_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_OM_RES"); // OMに紐づいた封鎖ID取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_EX_REQUIRED_ITEM_REQ = new PacketId(62, 9, 1, "C2S_SEASON_DUNGEON_GET_EX_REQUIRED_ITEM_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_EX_REQUIRED_ITEM_RES = new PacketId(62, 9, 2, "S2C_SEASON_DUNGEON_GET_EX_REQUIRED_ITEM_RES"); // 納品状況取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_DELIVER_ITEM_FOR_EX_REQ = new PacketId(62, 10, 1, "C2S_SEASON_DUNGEON_DELIVER_ITEM_FOR_EX_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_DELIVER_ITEM_FOR_EX_RES = new PacketId(62, 10, 2, "S2C_SEASON_DUNGEON_DELIVER_ITEM_FOR_EX_RES"); // 納品状況取得に
        public static readonly PacketId S2C_SEASON_62_13_16_NTC = new PacketId(62, 13, 16, "S2C_SEASON_62_13_16_NTC");
        public static readonly PacketId S2C_SEASON_62_14_16_NTC = new PacketId(62, 14, 16, "S2C_SEASON_62_14_16_NTC");
        public static readonly PacketId S2C_SEASON_62_15_16_NTC = new PacketId(62, 15, 16, "S2C_SEASON_62_15_16_NTC");
        public static readonly PacketId S2C_SEASON_62_16_16_NTC = new PacketId(62, 16, 16, "S2C_SEASON_62_16_16_NTC");
        public static readonly PacketId S2C_SEASON_62_17_16_NTC = new PacketId(62, 17, 16, "S2C_SEASON_62_17_16_NTC");
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_LIST_FROM_OM_REQ = new PacketId(62, 18, 1, "C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_LIST_FROM_OM_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_LIST_FROM_OM_RES = new PacketId(62, 18, 2, "S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_LIST_FROM_OM_RES"); // OMに紐づいた英霊の試練情報取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_FOR_VIEW_REQ = new PacketId(62, 19, 1, "C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_FOR_VIEW_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_FOR_VIEW_RES = new PacketId(62, 19, 2, "S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_FOR_VIEW_RES"); // 試練の報酬りすと取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_SOUL_ORDEAL_READY_REQ = new PacketId(62, 20, 1, "C2S_SEASON_DUNGEON_SOUL_ORDEAL_READY_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_SOUL_ORDEAL_READY_RES = new PacketId(62, 20, 2, "S2C_SEASON_DUNGEON_SOUL_ORDEAL_READY_RES"); // 英霊の試練準備に
        public static readonly PacketId C2S_SEASON_DUNGEON_SOUL_ORDEAL_CANCEL_READY_REQ = new PacketId(62, 21, 1, "C2S_SEASON_DUNGEON_SOUL_ORDEAL_CANCEL_READY_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_SOUL_ORDEAL_CANCEL_READY_RES = new PacketId(62, 21, 2, "S2C_SEASON_DUNGEON_SOUL_ORDEAL_CANCEL_READY_RES"); // 英霊の試練準備キャンセルに
        public static readonly PacketId S2C_SEASON_62_22_16_NTC = new PacketId(62, 22, 16, "S2C_SEASON_62_22_16_NTC");
        public static readonly PacketId S2C_SEASON_62_23_16_NTC = new PacketId(62, 23, 16, "S2C_SEASON_62_23_16_NTC");
        public static readonly PacketId C2S_SEASON_DUNGEON_EXECUTE_SOUL_ORDEAL_REQ = new PacketId(62, 24, 1, "C2S_SEASON_DUNGEON_EXECUTE_SOUL_ORDEAL_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_EXECUTE_SOUL_ORDEAL_RES = new PacketId(62, 24, 2, "S2C_SEASON_DUNGEON_EXECUTE_SOUL_ORDEAL_RES"); // 英霊の試練起動に
        public static readonly PacketId C2S_SEASON_DUNGEON_INTERRUPT_SOUL_ORDEAL_REQ = new PacketId(62, 25, 1, "C2S_SEASON_DUNGEON_INTERRUPT_SOUL_ORDEAL_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_INTERRUPT_SOUL_ORDEAL_RES = new PacketId(62, 25, 2, "S2C_SEASON_DUNGEON_INTERRUPT_SOUL_ORDEAL_RES"); // 英霊の試練中断に
        public static readonly PacketId S2C_SEASON_62_26_16_NTC = new PacketId(62, 26, 16, "S2C_SEASON_62_26_16_NTC");
        public static readonly PacketId S2C_SEASON_62_27_16_NTC = new PacketId(62, 27, 16, "S2C_SEASON_62_27_16_NTC");
        public static readonly PacketId S2C_SEASON_62_28_16_NTC = new PacketId(62, 28, 16, "S2C_SEASON_62_28_16_NTC");
        public static readonly PacketId S2C_SEASON_62_29_16_NTC = new PacketId(62, 29, 16, "S2C_SEASON_62_29_16_NTC");
        public static readonly PacketId S2C_SEASON_62_30_16_NTC = new PacketId(62, 30, 16, "S2C_SEASON_62_30_16_NTC");
        public static readonly PacketId C2S_SEASON_DUNGEON_IS_UNLOCKED_KEY_POINT_DOOR_REQ = new PacketId(62, 31, 1, "C2S_SEASON_DUNGEON_IS_UNLOCKED_KEY_POINT_DOOR_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_IS_UNLOCKED_KEY_POINT_DOOR_RES = new PacketId(62, 31, 2, "S2C_SEASON_DUNGEON_IS_UNLOCKED_KEY_POINT_DOOR_RES"); // シーズンダンジョンキーポイント扉解放状況取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_IS_UNLOCKED_SOUL_ORDEAL_DOOR_REQ = new PacketId(62, 32, 1, "C2S_SEASON_DUNGEON_IS_UNLOCKED_SOUL_ORDEAL_DOOR_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_IS_UNLOCKED_SOUL_ORDEAL_DOOR_RES = new PacketId(62, 32, 2, "S2C_SEASON_DUNGEON_IS_UNLOCKED_SOUL_ORDEAL_DOOR_RES"); // シーズンダンジョン英霊の試練扉解放状況取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_REQ = new PacketId(62, 33, 1, "C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_RES = new PacketId(62, 33, 2, "S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_RES"); // シーズンダンジョン英霊の試練報酬取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_REQ = new PacketId(62, 34, 1, "C2S_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_RES = new PacketId(62, 34, 2, "S2C_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_RES"); // シーズンダンジョン英霊の試練報酬取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_BUFF_REQ = new PacketId(62, 35, 1, "C2S_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_BUFF_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_BUFF_RES = new PacketId(62, 35, 2, "S2C_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_BUFF_RES"); // シーズンダンジョン英霊の試練バフ取得に
        public static readonly PacketId C2S_SEASON_DUNGEON_UPDATE_KEY_POINT_DOOR_STATUS_REQ = new PacketId(62, 36, 1, "C2S_SEASON_DUNGEON_UPDATE_KEY_POINT_DOOR_STATUS_REQ");
        public static readonly PacketId S2C_SEASON_DUNGEON_UPDATE_KEY_POINT_DOOR_STATUS_RES = new PacketId(62, 36, 2, "S2C_SEASON_DUNGEON_UPDATE_KEY_POINT_DOOR_STATUS_RES"); // シーズンダンジョンキーポイント封鎖扉ステータス取得に
        public static readonly PacketId S2C_SEASON_62_37_16_NTC = new PacketId(62, 37, 16, "S2C_SEASON_62_37_16_NTC");
        public static readonly PacketId S2C_SEASON_62_38_16_NTC = new PacketId(62, 38, 16, "S2C_SEASON_62_38_16_NTC");
        public static readonly PacketId S2C_SEASON_62_39_16_NTC = new PacketId(62, 39, 16, "S2C_SEASON_62_39_16_NTC");

// Group: 63
        public static readonly PacketId S2C_63_0_16_NTC = new PacketId(63, 0, 16, "S2C_63_0_16_NTC");
        public static readonly PacketId S2C_63_1_16_NTC = new PacketId(63, 1, 16, "S2C_63_1_16_NTC");
        public static readonly PacketId S2C_63_2_16_NTC = new PacketId(63, 2, 16, "S2C_63_2_16_NTC");
        public static readonly PacketId S2C_63_3_16_NTC = new PacketId(63, 3, 16, "S2C_63_3_16_NTC");
        public static readonly PacketId S2C_63_6_16_NTC = new PacketId(63, 6, 16, "S2C_63_6_16_NTC");
        public static readonly PacketId S2C_63_7_16_NTC = new PacketId(63, 7, 16, "S2C_63_7_16_NTC");
        public static readonly PacketId S2C_63_10_16_NTC = new PacketId(63, 10, 16, "S2C_63_10_16_NTC");

// Group: 64 - (MANDRAGORA)
        public static readonly PacketId C2S_MANDRAGORA_GET_MY_MANDRAGORA_REQ = new PacketId(64, 0, 1, "C2S_MANDRAGORA_GET_MY_MANDRAGORA_REQ");
        public static readonly PacketId S2C_MANDRAGORA_GET_MY_MANDRAGORA_RES = new PacketId(64, 0, 2, "S2C_MANDRAGORA_GET_MY_MANDRAGORA_RES"); // 所持マンドラゴラ取得
        public static readonly PacketId C2S_MANDRAGORA_CREATE_MY_MANDRAGORA_REQ = new PacketId(64, 1, 1, "C2S_MANDRAGORA_CREATE_MY_MANDRAGORA_REQ");
        public static readonly PacketId S2C_MANDRAGORA_CREATE_MY_MANDRAGORA_RES = new PacketId(64, 1, 2, "S2C_MANDRAGORA_CREATE_MY_MANDRAGORA_RES"); // マンドラゴラを作成
        public static readonly PacketId C2S_MANDRAGORA_CHANGE_SOIL_REQ = new PacketId(64, 2, 1, "C2S_MANDRAGORA_CHANGE_SOIL_REQ");
        public static readonly PacketId S2C_MANDRAGORA_CHANGE_SOIL_RES = new PacketId(64, 2, 2, "S2C_MANDRAGORA_CHANGE_SOIL_RES"); // 土を変える
        public static readonly PacketId C2S_MANDRAGORA_GET_CRAFT_RECIPE_LIST_REQ = new PacketId(64, 3, 1, "C2S_MANDRAGORA_GET_CRAFT_RECIPE_LIST_REQ");
        public static readonly PacketId S2C_MANDRAGORA_GET_CRAFT_RECIPE_LIST_RES = new PacketId(64, 3, 2, "S2C_MANDRAGORA_GET_CRAFT_RECIPE_LIST_RES"); // 生産レシピ取得
        public static readonly PacketId C2S_MANDRAGORA_BEGIN_CRAFT_REQ = new PacketId(64, 4, 1, "C2S_MANDRAGORA_BEGIN_CRAFT_REQ");
        public static readonly PacketId S2C_MANDRAGORA_BEGIN_CRAFT_RES = new PacketId(64, 4, 2, "S2C_MANDRAGORA_BEGIN_CRAFT_RES"); // 育成開始
        public static readonly PacketId C2S_MANDRAGORA_CHANGE_GOLDEN_CRAFT_REQ = new PacketId(64, 5, 1, "C2S_MANDRAGORA_CHANGE_GOLDEN_CRAFT_REQ");
        public static readonly PacketId S2C_MANDRAGORA_CHANGE_GOLDEN_CRAFT_RES = new PacketId(64, 5, 2, "S2C_MANDRAGORA_CHANGE_GOLDEN_CRAFT_RES"); // 黄金石育成に変更
        public static readonly PacketId C2S_MANDRAGORA_GET_MANURE_ITEM_LIST_REQ = new PacketId(64, 6, 1, "C2S_MANDRAGORA_GET_MANURE_ITEM_LIST_REQ");
        public static readonly PacketId S2C_MANDRAGORA_GET_MANURE_ITEM_LIST_RES = new PacketId(64, 6, 2, "S2C_MANDRAGORA_GET_MANURE_ITEM_LIST_RES"); // 肥料アイテムの確認
        public static readonly PacketId C2S_MANDRAGORA_FINISH_CRAFT_CHECK_REQ = new PacketId(64, 7, 1, "C2S_MANDRAGORA_FINISH_CRAFT_CHECK_REQ");
        public static readonly PacketId S2C_MANDRAGORA_FINISH_CRAFT_CHECK_RES = new PacketId(64, 7, 2, "S2C_MANDRAGORA_FINISH_CRAFT_CHECK_RES"); // 育成完了確認
        public static readonly PacketId S2C_MANDRAGORA_64_7_16_NTC = new PacketId(64, 7, 16, "S2C_MANDRAGORA_64_7_16_NTC");
        public static readonly PacketId C2S_MANDRAGORA_FINISH_ITEM_GET_REQ = new PacketId(64, 8, 1, "C2S_MANDRAGORA_FINISH_ITEM_GET_REQ");
        public static readonly PacketId S2C_MANDRAGORA_FINISH_ITEM_GET_RES = new PacketId(64, 8, 2, "S2C_MANDRAGORA_FINISH_ITEM_GET_RES"); // 育成完了アイテム受け取り
        public static readonly PacketId C2S_MANDRAGORA_GET_SPECIES_LIST_REQ = new PacketId(64, 9, 1, "C2S_MANDRAGORA_GET_SPECIES_LIST_REQ");
        public static readonly PacketId S2C_MANDRAGORA_GET_SPECIES_LIST_RES = new PacketId(64, 9, 2, "S2C_MANDRAGORA_GET_SPECIES_LIST_RES"); // 図鑑一覧取得に
        public static readonly PacketId C2S_MANDRAGORA_CHANGE_SPECIES_REQ = new PacketId(64, 10, 1, "C2S_MANDRAGORA_CHANGE_SPECIES_REQ");
        public static readonly PacketId S2C_MANDRAGORA_CHANGE_SPECIES_RES = new PacketId(64, 10, 2, "S2C_MANDRAGORA_CHANGE_SPECIES_RES"); // 見た目（種類）を変える
        public static readonly PacketId C2S_MANDRAGORA_CHANGE_NAME_REQ = new PacketId(64, 11, 1, "C2S_MANDRAGORA_CHANGE_NAME_REQ");
        public static readonly PacketId S2C_MANDRAGORA_CHANGE_NAME_RES = new PacketId(64, 11, 2, "S2C_MANDRAGORA_CHANGE_NAME_RES"); // 名前を変える
        public static readonly PacketId C2S_MANDRAGORA_GET_SPECIES_CATEGORY_LIST_REQ = new PacketId(64, 12, 1, "C2S_MANDRAGORA_GET_SPECIES_CATEGORY_LIST_REQ");
        public static readonly PacketId S2C_MANDRAGORA_GET_SPECIES_CATEGORY_LIST_RES = new PacketId(64, 12, 2, "S2C_MANDRAGORA_GET_SPECIES_CATEGORY_LIST_RES"); // 図鑑カテゴリリスト再取得

// Group: 65 - (EQUIP)
        public static readonly PacketId S2C_EQUIP_65_0_16_NTC = new PacketId(65, 0, 16, "S2C_EQUIP_65_0_16_NTC");
        public static readonly PacketId C2S_EQUIP_ENHANCED_SUB_REQ = new PacketId(65, 1, 1, "C2S_EQUIP_ENHANCED_SUB_REQ");
        public static readonly PacketId S2C_EQUIP_ENHANCED_SUB_RES = new PacketId(65, 1, 2, "S2C_EQUIP_ENHANCED_SUB_RES"); // 武器新化：サブ強化通知
        public static readonly PacketId C2S_EQUIP_ENHANCED_AWAKEN_REQ = new PacketId(65, 2, 1, "C2S_EQUIP_ENHANCED_AWAKEN_REQ");
        public static readonly PacketId S2C_EQUIP_ENHANCED_AWAKEN_RES = new PacketId(65, 2, 2, "S2C_EQUIP_ENHANCED_AWAKEN_RES"); // 武器新化：覚醒進化通知
        public static readonly PacketId C2S_EQUIP_ENHANCED_GET_PACKS_REQ = new PacketId(65, 3, 1, "C2S_EQUIP_ENHANCED_GET_PACKS_REQ");
        public static readonly PacketId S2C_EQUIP_ENHANCED_GET_PACKS_RES = new PacketId(65, 3, 2, "S2C_EQUIP_ENHANCED_GET_PACKS_RES"); // 進化パック取得
        public static readonly PacketId C2S_EQUIP_ENHANCED_ENHANCE_ITEM_REQ = new PacketId(65, 4, 1, "C2S_EQUIP_ENHANCED_ENHANCE_ITEM_REQ");
        public static readonly PacketId S2C_EQUIP_ENHANCED_ENHANCE_ITEM_RES = new PacketId(65, 4, 2, "S2C_EQUIP_ENHANCED_ENHANCE_ITEM_RES"); // 進化リクエストの通知

// Group: 66 - (JOB)
        public static readonly PacketId C2S_JOB_EMBLEM_GET_EMBLEM_LIST_REQ = new PacketId(66, 0, 1, "C2S_JOB_EMBLEM_GET_EMBLEM_LIST_REQ");
        public static readonly PacketId S2C_JOB_EMBLEM_GET_EMBLEM_LIST_RES = new PacketId(66, 0, 2, "S2C_JOB_EMBLEM_GET_EMBLEM_LIST_RES"); // 全ジョブの証情報取得
        public static readonly PacketId C2S_JOB_EMBLEM_GET_EMBLEM_REQ = new PacketId(66, 1, 1, "C2S_JOB_EMBLEM_GET_EMBLEM_REQ");
        public static readonly PacketId S2C_JOB_EMBLEM_GET_EMBLEM_RES = new PacketId(66, 1, 2, "S2C_JOB_EMBLEM_GET_EMBLEM_RES"); // 証情報取得
        public static readonly PacketId C2S_JOB_EMBLEM_UPDATE_LEVEL_REQ = new PacketId(66, 2, 1, "C2S_JOB_EMBLEM_UPDATE_LEVEL_REQ");
        public static readonly PacketId S2C_JOB_EMBLEM_UPDATE_LEVEL_RES = new PacketId(66, 2, 2, "S2C_JOB_EMBLEM_UPDATE_LEVEL_RES"); // ジョブの証レベルアップ
        public static readonly PacketId C2S_JOB_EMBLEM_UPDATE_PARAM_LEVEL_REQ = new PacketId(66, 3, 1, "C2S_JOB_EMBLEM_UPDATE_PARAM_LEVEL_REQ");
        public static readonly PacketId S2C_JOB_EMBLEM_UPDATE_PARAM_LEVEL_RES = new PacketId(66, 3, 2, "S2C_JOB_EMBLEM_UPDATE_PARAM_LEVEL_RES"); // ジョブの証パラメータアップ
        public static readonly PacketId C2S_JOB_EMBLEM_RESET_PARAM_LEVEL_REQ = new PacketId(66, 4, 1, "C2S_JOB_EMBLEM_RESET_PARAM_LEVEL_REQ");
        public static readonly PacketId S2C_JOB_EMBLEM_RESET_PARAM_LEVEL_RES = new PacketId(66, 4, 2, "S2C_JOB_EMBLEM_RESET_PARAM_LEVEL_RES"); // ジョブの証パラメータリセット
        public static readonly PacketId C2S_JOB_EMBLEM_ATTACH_ELEMENT_REQ = new PacketId(66, 5, 1, "C2S_JOB_EMBLEM_ATTACH_ELEMENT_REQ");
        public static readonly PacketId S2C_JOB_EMBLEM_ATTACH_ELEMENT_RES = new PacketId(66, 5, 2, "S2C_JOB_EMBLEM_ATTACH_ELEMENT_RES"); // ジョブの証クレスト継承
        public static readonly PacketId C2S_JOB_EMBLEM_DETTACH_ELEMENT_REQ = new PacketId(66, 6, 1, "C2S_JOB_EMBLEM_DETTACH_ELEMENT_REQ");
        public static readonly PacketId S2C_JOB_EMBLEM_DETTACH_ELEMENT_RES = new PacketId(66, 6, 2, "S2C_JOB_EMBLEM_DETTACH_ELEMENT_RES"); // ジョブの証クレスト破棄
        public static readonly PacketId S2C_JOB_66_7_16_NTC = new PacketId(66, 7, 16, "S2C_JOB_66_7_16_NTC");

// Group: 67 - (RECYCLE)
        public static readonly PacketId C2S_RECYCLE_GET_INFO_REQ = new PacketId(67, 0, 1, "C2S_RECYCLE_GET_INFO_REQ");
        public static readonly PacketId S2C_RECYCLE_GET_INFO_RES = new PacketId(67, 0, 2, "S2C_RECYCLE_GET_INFO_RES");
        public static readonly PacketId C2S_RECYCLE_GET_LOT_FORECAST_REQ = new PacketId(67, 1, 1, "C2S_RECYCLE_GET_LOT_FORECAST_REQ");
        public static readonly PacketId S2C_RECYCLE_GET_LOT_FORECAST_RES = new PacketId(67, 1, 2, "S2C_RECYCLE_GET_LOT_FORECAST_RES"); // リサイクル抽選予想取得
        public static readonly PacketId C2S_RECYCLE_START_EXCHANGE_REQ = new PacketId(67, 2, 1, "C2S_RECYCLE_START_EXCHANGE_REQ");
        public static readonly PacketId S2C_RECYCLE_START_EXCHANGE_RES = new PacketId(67, 2, 2, "S2C_RECYCLE_START_EXCHANGE_RES"); // リサイクル開始
        public static readonly PacketId C2S_RECYCLE_RESET_COUNT_REQ = new PacketId(67, 3, 1, "C2S_RECYCLE_RESET_COUNT_REQ");
        public static readonly PacketId S2C_RECYCLE_RESET_COUNT_RES = new PacketId(67, 3, 2, "S2C_RECYCLE_RESET_COUNT_RES"); // リサイクル回数リセット

// Group: 68
        public static readonly PacketId S2C_68_0_1 = new PacketId(68, 0, 1, "S2C_68_0_1");
        public static readonly PacketId S2C_68_1_1 = new PacketId(68, 1, 1, "S2C_68_1_1");

// Group: 69 - (PAWN)
        public static readonly PacketId C2S_PAWN_SP_SKILL_GET_ACTIVE_SKILL_REQ = new PacketId(69, 0, 1, "C2S_PAWN_SP_SKILL_GET_ACTIVE_SKILL_REQ");
        public static readonly PacketId S2C_PAWN_SP_SKILL_GET_ACTIVE_SKILL_RES = new PacketId(69, 0, 2, "S2C_PAWN_SP_SKILL_GET_ACTIVE_SKILL_RES"); // ポーン特技のセット情報の取得
        public static readonly PacketId C2S_PAWN_SP_SKILL_GET_STOCK_SKILL_REQ = new PacketId(69, 1, 1, "C2S_PAWN_SP_SKILL_GET_STOCK_SKILL_REQ");
        public static readonly PacketId S2C_PAWN_SP_SKILL_GET_STOCK_SKILL_RES = new PacketId(69, 1, 2, "S2C_PAWN_SP_SKILL_GET_STOCK_SKILL_RES"); // ストックにあるポーン特技を取得
        public static readonly PacketId C2S_PAWN_SP_SKILL_GET_MAX_SKILL_NUM_REQ = new PacketId(69, 2, 1, "C2S_PAWN_SP_SKILL_GET_MAX_SKILL_NUM_REQ");
        public static readonly PacketId S2C_PAWN_SP_SKILL_GET_MAX_SKILL_NUM_RES = new PacketId(69, 2, 2, "S2C_PAWN_SP_SKILL_GET_MAX_SKILL_NUM_RES"); // スキル上限値を取得
        public static readonly PacketId C2S_PAWN_SP_SKILL_SET_ACTIVE_SKILL_REQ = new PacketId(69, 3, 1, "C2S_PAWN_SP_SKILL_SET_ACTIVE_SKILL_REQ");
        public static readonly PacketId S2C_PAWN_SP_SKILL_SET_ACTIVE_SKILL_RES = new PacketId(69, 3, 2, "S2C_PAWN_SP_SKILL_SET_ACTIVE_SKILL_RES"); // ポーン特技を設定する
        public static readonly PacketId C2S_PAWN_SP_SKILL_DELETE_STOCK_SKILL_REQ = new PacketId(69, 4, 1, "C2S_PAWN_SP_SKILL_DELETE_STOCK_SKILL_REQ");
        public static readonly PacketId S2C_PAWN_SP_SKILL_DELETE_STOCK_SKILL_RES = new PacketId(69, 4, 2, "S2C_PAWN_SP_SKILL_DELETE_STOCK_SKILL_RES"); // ストックのポーン特技を削除する
        public static readonly PacketId S2C_PAWN_69_5_16_NTC = new PacketId(69, 5, 16, "S2C_PAWN_69_5_16_NTC");
        public static readonly PacketId S2C_PAWN_69_6_16_NTC = new PacketId(69, 6, 16, "S2C_PAWN_69_6_16_NTC");
        public static readonly PacketId C2S_PAWN_SP_SKILL_USE_ITEM_REQ = new PacketId(69, 7, 1, "C2S_PAWN_SP_SKILL_USE_ITEM_REQ");
        public static readonly PacketId S2C_PAWN_SP_SKILL_USE_ITEM_RES = new PacketId(69, 7, 2, "S2C_PAWN_SP_SKILL_USE_ITEM_RES"); // ポーン特技のアイテムを使う

// Group: 70 - (ACTION)
        public static readonly PacketId C2S_ACTION_SET_PLAYER_ACTION_HISTORY_REQ = new PacketId(70, 0, 1, "C2S_ACTION_SET_PLAYER_ACTION_HISTORY_REQ");
        public static readonly PacketId S2C_ACTION_SET_PLAYER_ACTION_HISTORY_RES = new PacketId(70, 0, 2, "S2C_ACTION_SET_PLAYER_ACTION_HISTORY_RES"); // プレイヤーの行動履歴の設定

// Group: 71 - (BATTLE)
        public static readonly PacketId C2S_BATTLE_71_0_1_REQ = new PacketId(71, 0, 1, "C2S_BATTLE_71_0_1_REQ");
        public static readonly PacketId S2C_BATTLE_71_0_2_RES = new PacketId(71, 0, 2, "S2C_BATTLE_71_0_2_RES");
        public static readonly PacketId C2S_BATTLE_CONTENT_REWARD_LIST_REQ = new PacketId(71, 1, 1, "C2S_BATTLE_CONTENT_REWARD_LIST_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_REWARD_LIST_RES = new PacketId(71, 1, 2, "S2C_BATTLE_CONTENT_REWARD_LIST_RES"); // バトルコンテンツ：報酬リストを取得
        public static readonly PacketId C2S_BATTLE_CONTENT_GET_REWARD_REQ = new PacketId(71, 2, 1, "C2S_BATTLE_CONTENT_GET_REWARD_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_GET_REWARD_RES = new PacketId(71, 2, 2, "S2C_BATTLE_CONTENT_GET_REWARD_RES"); // バトルコンテンツ：報酬の取得
        public static readonly PacketId S2C_BATTLE_71_2_16_NTC = new PacketId(71, 2, 16, "S2C_BATTLE_71_2_16_NTC");
        public static readonly PacketId C2S_BATTLE_CONTENT_CONTENT_ENTRY_REQ = new PacketId(71, 3, 1, "C2S_BATTLE_CONTENT_CONTENT_ENTRY_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_CONTENT_ENTRY_RES = new PacketId(71, 3, 2, "S2C_BATTLE_CONTENT_CONTENT_ENTRY_RES"); // コンテンツのエントリーに
        public static readonly PacketId S2C_BATTLE_71_3_16_NTC = new PacketId(71, 3, 16, "S2C_BATTLE_71_3_16_NTC");
        public static readonly PacketId C2S_BATTLE_CONTENT_CONTENT_FIRST_FHASE_CHANGE_REQ = new PacketId(71, 4, 1, "C2S_BATTLE_CONTENT_CONTENT_FIRST_FHASE_CHANGE_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_CONTENT_FIRST_FHASE_CHANGE_RES = new PacketId(71, 4, 2, "S2C_BATTLE_CONTENT_CONTENT_FIRST_FHASE_CHANGE_RES"); // バトルコンテンツ最初のフェーズ変更
        public static readonly PacketId C2S_BATTLE_CONTENT_CONTENT_RESET_REQ = new PacketId(71, 5, 1, "C2S_BATTLE_CONTENT_CONTENT_RESET_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_CONTENT_RESET_RES = new PacketId(71, 5, 2, "S2C_BATTLE_CONTENT_CONTENT_RESET_RES"); // バトルコンテンツ：リセット
        public static readonly PacketId S2C_BATTLE_71_5_16_NTC = new PacketId(71, 5, 16, "S2C_BATTLE_71_5_16_NTC");
        public static readonly PacketId C2S_BATTLE_CONTENT_GET_CONTENT_STATUS_FROM_OM_REQ = new PacketId(71, 6, 1, "C2S_BATTLE_CONTENT_GET_CONTENT_STATUS_FROM_OM_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_GET_CONTENT_STATUS_FROM_OM_RES = new PacketId(71, 6, 2, "S2C_BATTLE_CONTENT_GET_CONTENT_STATUS_FROM_OM_RES"); // OMからのコンテンツ情報取得に
        public static readonly PacketId C2S_BATTLE_CONTENT_GET_PHASE_TO_CHANGE_FROM_OM_REQ = new PacketId(71, 7, 1, "C2S_BATTLE_CONTENT_GET_PHASE_TO_CHANGE_FROM_OM_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_GET_PHASE_TO_CHANGE_FROM_OM_RES = new PacketId(71, 7, 2, "S2C_BATTLE_CONTENT_GET_PHASE_TO_CHANGE_FROM_OM_RES"); // OMからのフェーズ情報取得に
        public static readonly PacketId C2S_BATTLE_CONTENT_PHASE_ENTRY_BEGIN_RECRUITMENT_REQ = new PacketId(71, 8, 1, "C2S_BATTLE_CONTENT_PHASE_ENTRY_BEGIN_RECRUITMENT_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_PHASE_ENTRY_BEGIN_RECRUITMENT_RES = new PacketId(71, 8, 2, "S2C_BATTLE_CONTENT_PHASE_ENTRY_BEGIN_RECRUITMENT_RES"); // 一斉ステージ移動待ち受け開始に
        public static readonly PacketId C2S_BATTLE_CONTENT_PHASE_ENTRY_GET_RECRUITMENT_STATE_REQ = new PacketId(71, 9, 1, "C2S_BATTLE_CONTENT_PHASE_ENTRY_GET_RECRUITMENT_STATE_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_PHASE_ENTRY_GET_RECRUITMENT_STATE_RES = new PacketId(71, 9, 2, "S2C_BATTLE_CONTENT_PHASE_ENTRY_GET_RECRUITMENT_STATE_RES"); // 一斉ステージ移動待ち受け状態取得に
        public static readonly PacketId C2S_BATTLE_CONTENT_PHASE_ENTRY_READY_REQ = new PacketId(71, 10, 1, "C2S_BATTLE_CONTENT_PHASE_ENTRY_READY_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_PHASE_ENTRY_READY_RES = new PacketId(71, 10, 2, "S2C_BATTLE_CONTENT_PHASE_ENTRY_READY_RES"); // 一斉ステージ移動準備に
        public static readonly PacketId S2C_BATTLE_71_10_16_NTC = new PacketId(71, 10, 16, "S2C_BATTLE_71_10_16_NTC");
        public static readonly PacketId C2S_BATTLE_CONTENT_PHASE_ENTRY_READY_CANCEL_REQ = new PacketId(71, 11, 1, "C2S_BATTLE_CONTENT_PHASE_ENTRY_READY_CANCEL_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_PHASE_ENTRY_READY_CANCEL_RES = new PacketId(71, 11, 2, "S2C_BATTLE_CONTENT_PHASE_ENTRY_READY_CANCEL_RES"); // 一斉ステージ移動準備キャンセルに
        public static readonly PacketId S2C_BATTLE_71_12_16_NTC = new PacketId(71, 12, 16, "S2C_BATTLE_71_12_16_NTC");
        public static readonly PacketId S2C_BATTLE_71_13_16_NTC = new PacketId(71, 13, 16, "S2C_BATTLE_71_13_16_NTC");
        public static readonly PacketId S2C_BATTLE_71_14_16_NTC = new PacketId(71, 14, 16, "S2C_BATTLE_71_14_16_NTC");
        public static readonly PacketId S2C_BATTLE_71_15_16_NTC = new PacketId(71, 15, 16, "S2C_BATTLE_71_15_16_NTC");
        public static readonly PacketId C2S_BATTLE_CONTENT_RESET_INFO_REQ = new PacketId(71, 16, 1, "C2S_BATTLE_CONTENT_RESET_INFO_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_RESET_INFO_RES = new PacketId(71, 16, 2, "S2C_BATTLE_CONTENT_RESET_INFO_RES"); // バトルコンテンツ：リセット情報の取得
        public static readonly PacketId C2S_BATTLE_CONTENT_INSTANT_CLEAR_INFO_REQ = new PacketId(71, 17, 1, "C2S_BATTLE_CONTENT_INSTANT_CLEAR_INFO_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_INSTANT_CLEAR_INFO_RES = new PacketId(71, 17, 2, "S2C_BATTLE_CONTENT_INSTANT_CLEAR_INFO_RES"); // バトルコンテンツ即時クリア情報取得
        public static readonly PacketId C2S_BATTLE_CONTENT_CHARACTER_INFO_REQ = new PacketId(71, 18, 1, "C2S_BATTLE_CONTENT_CHARACTER_INFO_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_CHARACTER_INFO_RES = new PacketId(71, 18, 2, "S2C_BATTLE_CONTENT_CHARACTER_INFO_RES"); // バトルコンテンツ：進行情報の取得
        public static readonly PacketId S2C_BATTLE_71_19_16_NTC = new PacketId(71, 19, 16, "S2C_BATTLE_71_19_16_NTC");
        public static readonly PacketId C2S_BATTLE_CONTENT_PARTY_MEMBER_INFO_REQ = new PacketId(71, 20, 1, "C2S_BATTLE_CONTENT_PARTY_MEMBER_INFO_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_RES = new PacketId(71, 20, 2, "S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_RES"); // パーティメンバのバトルコンテンツ情報を取得
        public static readonly PacketId C2S_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_REQ = new PacketId(71, 21, 1, "C2S_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_RES = new PacketId(71, 21, 2, "S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_RES"); // 特定のパーティメンバーのバトルコンテンツ情報更新
        public static readonly PacketId S2C_BATTLE_71_21_16_NTC = new PacketId(71, 21, 16, "S2C_BATTLE_71_21_16_NTC");
        public static readonly PacketId C2S_BATTLE_CONTENT_INFO_LIST_REQ = new PacketId(71, 22, 1, "C2S_BATTLE_CONTENT_INFO_LIST_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_INFO_LIST_RES = new PacketId(71, 22, 2, "S2C_BATTLE_CONTENT_INFO_LIST_RES"); // バトルコンテンツ情報を取得
        public static readonly PacketId C2S_BATTLE_CONTENT_INSTANT_COMPLETE_REQ = new PacketId(71, 23, 1, "C2S_BATTLE_CONTENT_INSTANT_COMPLETE_REQ");
        public static readonly PacketId S2C_BATTLE_CONTENT_INSTANT_COMPLETE_RES = new PacketId(71, 23, 2, "S2C_BATTLE_CONTENT_INSTANT_COMPLETE_RES"); // バトルコンテンツ即時クリア

// Group: 72 - (DAILY)
        public static readonly PacketId C2S_DAILY_MISSION_LIST_GET_REQ = new PacketId(72, 0, 1, "C2S_DAILY_MISSION_LIST_GET_REQ");
        public static readonly PacketId S2C_DAILY_MISSION_LIST_GET_RES = new PacketId(72, 0, 2, "S2C_DAILY_MISSION_LIST_GET_RES"); // デイリーミッションのリストの取得に
        public static readonly PacketId S2C_DAILY_72_1_16_NTC = new PacketId(72, 1, 16, "S2C_DAILY_72_1_16_NTC");
        public static readonly PacketId C2S_DAILY_MISSION_REWARD_RECEIVE_REQ = new PacketId(72, 2, 1, "C2S_DAILY_MISSION_REWARD_RECEIVE_REQ");
        public static readonly PacketId S2C_DAILY_MISSION_REWARD_RECEIVE_RES = new PacketId(72, 2, 2, "S2C_DAILY_MISSION_REWARD_RECEIVE_RES"); // デイリーミッションの報酬受け取りに
        public static readonly PacketId C2S_DAILY_MISSION_EVENT_LIST_GET_REQ = new PacketId(72, 3, 1, "C2S_DAILY_MISSION_EVENT_LIST_GET_REQ");
        public static readonly PacketId S2C_DAILY_MISSION_EVENT_LIST_GET_RES = new PacketId(72, 3, 2, "S2C_DAILY_MISSION_EVENT_LIST_GET_RES"); // デイリーミッション：期間イベントのリストの取得に

        #endregion

        #region GamePacketIdsInit

        private static Dictionary<int, PacketId> InitializeGamePacketIds()
        {
            Dictionary<int, PacketId> packetIds = new Dictionary<int, PacketId>();
            AddPacketIdEntry(packetIds, UNKNOWN);
// Group: 0 - (CONNECTION)
            AddPacketIdEntry(packetIds, C2S_CONNECTION_PING_REQ);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_PING_RES);
            AddPacketIdEntry(packetIds, C2S_CONNECTION_LOGIN_REQ);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_LOGIN_RES);
            AddPacketIdEntry(packetIds, C2S_CONNECTION_LOGOUT_REQ);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_LOGOUT_RES);
            AddPacketIdEntry(packetIds, C2S_CONNECTION_GET_LOGIN_ANNOUNCEMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_GET_LOGIN_ANNOUNCEMENT_RES);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_0_4_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CONNECTION_MOVE_IN_SERVER_REQ);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_MOVE_IN_SERVER_RES);
            AddPacketIdEntry(packetIds, C2S_CONNECTION_MOVE_OUT_SERVER_REQ);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_MOVE_OUT_SERVER_RES);
            AddPacketIdEntry(packetIds, C2S_CONNECTION_RESERVE_SERVER_REQ);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_RESERVE_SERVER_RES);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_0_9_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_0_10_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_0_11_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CONNECTION_0_12_16_NTC);

// Group: 1 - (SERVER)
            AddPacketIdEntry(packetIds, C2S_SERVER_GET_SERVER_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_GET_SERVER_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SERVER_GET_GAME_SETTING_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_GET_GAME_SETTING_RES);
            AddPacketIdEntry(packetIds, C2S_SERVER_GET_WORLD_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_GET_WORLD_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_SERVER_GET_REAL_TIME_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_GET_REAL_TIME_RES);
            AddPacketIdEntry(packetIds, C2S_SERVER_REPORT_SITE_GET_ADDRESS_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_REPORT_SITE_GET_ADDRESS_RES);
            AddPacketIdEntry(packetIds, C2S_SERVER_1_6_1_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_1_6_2_RES);
            AddPacketIdEntry(packetIds, C2S_SERVER_WEATHER_FORECAST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_WEATHER_FORECAST_GET_RES);
            AddPacketIdEntry(packetIds, C2S_SERVER_GAME_TIME_GET_BASEINFO_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_GAME_TIME_GET_BASEINFO_RES);
            AddPacketIdEntry(packetIds, C2S_SERVER_GET_SCREEN_SHOT_CATEGORY_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_GET_SCREEN_SHOT_CATEGORY_RES);
            AddPacketIdEntry(packetIds, S2C_SERVER_1_10_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SERVER_1_11_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SERVER_1_12_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SERVER_1_13_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SERVER_1_14_16_NTC);

// Group: 2 - (CHARACTER)
            AddPacketIdEntry(packetIds, C2S_CHARACTER_DECIDE_CHARACTER_ID_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_DECIDE_CHARACTER_ID_RES);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_1_1);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_CHARACTER_SEARCH_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_CHARACTER_SEARCH_RES);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_3_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_4_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_5_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_6_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_7_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_8_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_9_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_10_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_11_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_CHARACTER_POINT_REVIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_CHARACTER_POINT_REVIVE_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_CHARACTER_GOLDEN_REVIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_CHARACTER_GOLDEN_REVIVE_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_CHARACTER_PENALTY_REVIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_CHARACTER_PENALTY_REVIVE_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_PAWN_POINT_REVIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_PAWN_POINT_REVIVE_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_PAWN_GOLDEN_REVIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_PAWN_GOLDEN_REVIVE_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_GET_REVIVE_CHARGEABLE_TIME_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_GET_REVIVE_CHARGEABLE_TIME_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_CHARGE_REVIVE_POINT_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_CHARGE_REVIVE_POINT_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_GET_REVIVE_POINT_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_GET_REVIVE_POINT_RES);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_27_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_28_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_29_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_30_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_31_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_32_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_33_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_34_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_35_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_36_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_37_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_SET_ONLINE_STATUS_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_SET_ONLINE_STATUS_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_SWITCH_GAME_MODE_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_SWITCH_GAME_MODE_RES);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_2_39_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_CREATE_MODE_CHARACTER_EDIT_PARAM_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_CREATE_MODE_CHARACTER_EDIT_PARAM_RES);

// Group: 3 - (LOBBY)
            AddPacketIdEntry(packetIds, C2S_LOBBY_LOBBY_JOIN_REQ);
            AddPacketIdEntry(packetIds, S2C_LOBBY_LOBBY_JOIN_RES);
            AddPacketIdEntry(packetIds, C2S_LOBBY_LOBBY_LEAVE_REQ);
            AddPacketIdEntry(packetIds, S2C_LOBBY_LOBBY_LEAVE_RES);
            AddPacketIdEntry(packetIds, C2S_LOBBY_LOBBY_CHAT_MSG_REQ);
            AddPacketIdEntry(packetIds, S2C_LOBBY_LOBBY_CHAT_MSG_RES);
            AddPacketIdEntry(packetIds, S2C_LOBBY_LOBBY_CHAT_MSG_NTC);
            AddPacketIdEntry(packetIds, S2C_LOBBY_3_4_16_NTC);

// Group: 4 - (CHAT)
            AddPacketIdEntry(packetIds, C2S_CHAT_SEND_TELL_MSG_REQ);
            AddPacketIdEntry(packetIds, S2C_CHAT_SEND_TELL_MSG_RES);
            AddPacketIdEntry(packetIds, S2C_CHAT_4_0_16_NTC);

// Group: 5 - (USER)
            AddPacketIdEntry(packetIds, C2S_USER_LIST_GET_USER_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_USER_LIST_GET_USER_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_USER_LIST_USER_LIST_MAX_NUM_REQ);
            AddPacketIdEntry(packetIds, S2C_USER_LIST_USER_LIST_MAX_NUM_RES);
            AddPacketIdEntry(packetIds, S2C_USER_5_2_16_NTC);
            AddPacketIdEntry(packetIds, S2C_USER_5_3_16_NTC);

// Group: 6 - (PARTY)
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_CREATE_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_CREATE_RES);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_INVITE_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_INVITE_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_1_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_INVITE_CHARACTER_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_INVITE_CHARACTER_RES);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_INVITE_CANCEL_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_INVITE_CANCEL_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_3_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_INVITE_REFUSE_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_INVITE_REFUSE_RES);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_INVITE_PREPARE_ACCEPT_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_INVITE_PREPARE_ACCEPT_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_5_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_INVITE_ENTRY_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_INVITE_ENTRY_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_6_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_INVITE_ENTRY_CANCEL_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_INVITE_ENTRY_CANCEL_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_7_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_JOIN_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_JOIN_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_8_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_GET_CONTENT_NUMBER_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_GET_CONTENT_NUMBER_RES);
            AddPacketIdEntry(packetIds, C2S_PARTY_6_10_1_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_10_2_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_10_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_MEMBER_KICK_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_MEMBER_KICK_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_11_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_BREAKUP_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_BREAKUP_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_12_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_CHANGE_LEADER_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_CHANGE_LEADER_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_13_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_SEARCH_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_SEARCH_RES);
            AddPacketIdEntry(packetIds, C2S_PARTY_PARTY_MEMBER_SET_VALUE_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTY_PARTY_MEMBER_SET_VALUE_RES);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_15_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_16_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_17_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_18_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_19_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_20_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_21_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_22_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_24_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PARTY_6_26_16_NTC);

// Group: 7 - (QUICK)
            AddPacketIdEntry(packetIds, C2S_QUICK_PARTY_REGISTER_REQ);
            AddPacketIdEntry(packetIds, S2C_QUICK_PARTY_REGISTER_RES);
            AddPacketIdEntry(packetIds, S2C_QUICK_7_0_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUICK_PARTY_REGISTER_QUEST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUICK_PARTY_REGISTER_QUEST_RES);
            AddPacketIdEntry(packetIds, S2C_QUICK_7_1_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUICK_PARTY_ENTRY_REQ);
            AddPacketIdEntry(packetIds, S2C_QUICK_PARTY_ENTRY_RES);
            AddPacketIdEntry(packetIds, S2C_QUICK_7_3_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUICK_7_5_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUICK_7_6_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUICK_7_7_16_NTC);

// Group: 8 - (PAWN)
            AddPacketIdEntry(packetIds, C2S_PAWN_CREATE_MYPAWN_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_CREATE_MYPAWN_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_DELETE_MYPAWN_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_DELETE_MYPAWN_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_MYPAWN_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_MYPAWN_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_MYPAWN_DATA_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_MYPAWN_DATA_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_REGISTERED_PAWN_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_REGISTERED_PAWN_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_REGISTERED_PAWN_LIST_BY_CHARACTER_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_REGISTERED_PAWN_LIST_BY_CHARACTER_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_REGISTERED_PAWN_DATA_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_REGISTERED_PAWN_DATA_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_RENT_REGISTERED_PAWN_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_RENT_REGISTERED_PAWN_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_RENTED_PAWN_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_RENTED_PAWN_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_RENTED_PAWN_DATA_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_RENTED_PAWN_DATA_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_RETURN_RENTED_PAWN_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_RETURN_RENTED_PAWN_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_PARTY_PAWN_DATA_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_PARTY_PAWN_DATA_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_JOIN_PARTY_MYPAWN_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_JOIN_PARTY_MYPAWN_RES);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_12_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PAWN_JOIN_PARTY_RENTED_PAWN_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_JOIN_PARTY_RENTED_PAWN_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_FAVORITE_PAWN_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_FAVORITE_PAWN_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_SET_FAVORITE_PAWN_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_SET_FAVORITE_PAWN_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_DELETE_FAVORITE_PAWN_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_DELETE_FAVORITE_PAWN_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_OFFICIAL_PAWN_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_OFFICIAL_PAWN_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_LEGEND_PAWN_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_LEGEND_PAWN_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_PAWN_LOST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_PAWN_LOST_RES);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_19_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_LOST_PAWN_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_LOST_PAWN_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_LOST_PAWN_REVIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_LOST_PAWN_REVIVE_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_LOST_PAWN_POINT_REVIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_LOST_PAWN_POINT_REVIVE_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_LOST_PAWN_GOLDEN_REVIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_LOST_PAWN_GOLDEN_REVIVE_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_LOST_PAWN_WALLET_REVIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_LOST_PAWN_WALLET_REVIVE_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_8_25_1_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_25_2_RES);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_25_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PAWN_UPDATE_PAWN_REACTION_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_UPDATE_PAWN_REACTION_LIST_RES);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_26_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PAWN_UPDATE_PAWN_SHARE_RANGE_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_UPDATE_PAWN_SHARE_RANGE_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_PAWN_HISTORY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_PAWN_HISTORY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_PAWN_TOTAL_SCORE_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_PAWN_TOTAL_SCORE_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_NORA_PAWN_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_NORA_PAWN_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_FREE_RENTAL_PAWN_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_FREE_RENTAL_PAWN_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_GET_NORA_PAWN_DATA_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_GET_NORA_PAWN_DATA_RES);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_33_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_34_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_35_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_36_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_37_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_38_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_39_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_40_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_8_41_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXTRA_JOIN_PARTY_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXTRA_JOIN_PARTY_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXTRA_LEAVE_PARTY_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXTRA_LEAVE_PARTY_RES);

// Group: 9 - (BINARY)
            AddPacketIdEntry(packetIds, C2S_BINARY_SAVE_GET_CHARACTER_BIN_SAVEDATA_REQ);
            AddPacketIdEntry(packetIds, S2C_BINARY_SAVE_GET_CHARACTER_BIN_SAVEDATA_RES);
            AddPacketIdEntry(packetIds, C2S_BINARY_SAVE_SET_CHARACTER_BIN_SAVEDATA_REQ);
            AddPacketIdEntry(packetIds, S2C_BINARY_SAVE_SET_CHARACTER_BIN_SAVEDATA_RES);

// Group: 10 - (ITEM)
            AddPacketIdEntry(packetIds, C2S_ITEM_USE_BAG_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_USE_BAG_ITEM_RES);
            AddPacketIdEntry(packetIds, S2C_ITEM_10_0_16_NTC);
            AddPacketIdEntry(packetIds, C2S_ITEM_USE_JOB_ITEMS_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_USE_JOB_ITEMS_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_LOAD_STORAGE_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_LOAD_STORAGE_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_STORAGE_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_STORAGE_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_ITEM_STORE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_ITEM_STORE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_CONSUME_STORAGE_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_CONSUME_STORAGE_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_POST_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_POST_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_MOVE_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_MOVE_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_SELL_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_SELL_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_ITEM_STORAGE_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_ITEM_STORAGE_INFO_RES);
            AddPacketIdEntry(packetIds, S2C_ITEM_10_10_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ITEM_10_11_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ITEM_10_12_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ITEM_10_13_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ITEM_10_14_16_NTC);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_PAY_COST_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_PAY_COST_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_VALUABLE_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_VALUABLE_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_RECOVERY_VALUABLE_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_RECOVERY_VALUABLE_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_ITEM_STORAGE_CUSTOM_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_ITEM_STORAGE_CUSTOM_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_UPDATE_ITEM_STORAGE_CUSTOM_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_UPDATE_ITEM_STORAGE_CUSTOM_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_CHANGE_ATTR_DISCARD_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_CHANGE_ATTR_DISCARD_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_DEFAULT_STORAGE_EMPTY_SLOT_NUM_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_DEFAULT_STORAGE_EMPTY_SLOT_NUM_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_SPECIFIED_HAVING_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_SPECIFIED_HAVING_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_EMBODY_ITEMS_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_EMBODY_ITEMS_RES);
            AddPacketIdEntry(packetIds, C2S_ITEM_GET_EMBODY_PAY_COST_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_GET_EMBODY_PAY_COST_RES);

// Group: 11 - (QUEST)
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_LIGHT_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_LIGHT_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_SET_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_SET_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_1_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_MAIN_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_MAIN_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_2_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_TUTORIAL_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_TUTORIAL_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_LOT_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_LOT_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_PACKAGE_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_PACKAGE_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_MOB_HUNT_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_MOB_HUNT_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_7_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_TIME_LIMITED_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_TIME_LIMITED_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_WORLD_MANAGE_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_WORLD_MANAGE_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_9_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_END_CONTENTS_GROUP_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_END_CONTENTS_GROUP_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_QUEST_SCHEDULE_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_QUEST_SCHEDULE_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_MAIN_QUEST_COMPLETE_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_MAIN_QUEST_COMPLETE_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_11_13_1_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_13_2_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_SET_QUEST_INFO_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_SET_QUEST_INFO_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_CYCLE_CONTENTS_NEWS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_CYCLE_CONTENTS_NEWS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_PACKAGE_QUEST_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_PACKAGE_QUEST_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_PACKAGE_QUEST_INFO_DETAIL_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_PACKAGE_QUEST_INFO_DETAIL_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_ADD_PACKAGE_QUEST_POINT_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_ADD_PACKAGE_QUEST_POINT_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_AREA_BONUS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_AREA_BONUS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_QUEST_ORDER_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_QUEST_ORDER_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_20_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_QUEST_PROGRESS_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_QUEST_PROGRESS_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_21_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_LEADER_QUEST_PROGRESS_REQUEST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_LEADER_QUEST_PROGRESS_REQUEST_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_22_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_LIGHT_QUEST_GP_COMPLETE_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_LIGHT_QUEST_GP_COMPLETE_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_CHECK_QUEST_DISTRIBUTION_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_CHECK_QUEST_DISTRIBUTION_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_QUEST_CANCEL_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_QUEST_CANCEL_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_25_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_26_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_27_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_END_DISTRIBUTION_QUEST_CANCEL_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_END_DISTRIBUTION_QUEST_CANCEL_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_QUEST_COMPLETE_FLAG_CLEAR_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_QUEST_COMPLETE_FLAG_CLEAR_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_QUEST_DETAIL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_QUEST_DETAIL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_QUEST_COMPLETE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_QUEST_COMPLETE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_SET_PRIORITY_QUEST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_SET_PRIORITY_QUEST_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_32_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_CANCEL_PRIORITY_QUEST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_CANCEL_PRIORITY_QUEST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_PRIORITY_QUEST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_PRIORITY_QUEST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_SET_QUEST_OPEN_DATE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_SET_QUEST_OPEN_DATE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_QUEST_LAYOUT_FLAG_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_QUEST_LAYOUT_FLAG_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_CYCLE_CONTENTS_SITUATION_INFO_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_CYCLE_CONTENTS_SITUATION_INFO_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_PLAY_ENTRY_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_PLAY_ENTRY_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_39_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_11_40_1_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_40_2_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_40_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_PLAY_START_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_PLAY_START_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_PLAY_START_TIMER_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_PLAY_START_TIMER_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_42_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_PLAY_INTERRUPT_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_PLAY_INTERRUPT_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_43_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_PLAY_INTERRUPT_ANSWER_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_PLAY_INTERRUPT_ANSWER_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_PLAY_END_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_PLAY_END_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_45_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_CYCLE_CONTENTS_PLAY_START_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_CYCLE_CONTENTS_PLAY_START_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_CYCLE_CONTENTS_PLAY_END_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_CYCLE_CONTENTS_PLAY_END_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_47_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_CYCLE_CONTENTS_POINT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_CYCLE_CONTENTS_POINT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_CYCLE_CONTENTS_NOW_POINT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_CYCLE_CONTENTS_NOW_POINT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_CYCLE_CONTENTS_BORDER_REWARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_CYCLE_CONTENTS_BORDER_REWARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_CYCLE_CONTENTS_RANKING_REWARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_CYCLE_CONTENTS_RANKING_REWARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_CYCLE_CONTENTS_REWARD_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_CYCLE_CONTENTS_REWARD_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_11_53_1_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_53_2_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_11_54_1_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_54_2_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GP_CYCLE_CONTENTS_ROULETTE_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GP_CYCLE_CONTENTS_ROULETTE_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_AREA_INFO_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_AREA_INFO_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_QUEST_PARTY_BONUS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_QUEST_PARTY_BONUS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_SEND_LEADER_QUEST_ORDER_CONDITION_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_SEND_LEADER_QUEST_ORDER_CONDITION_INFO_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_58_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_SEND_LEADER_WAIT_ORDER_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_SEND_LEADER_WAIT_ORDER_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_59_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_QUEST_LOG_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_QUEST_LOG_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_REWARD_BOX_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_REWARD_BOX_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_REWARD_BOX_LIST_NUM_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_REWARD_BOX_LIST_NUM_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_REWARD_BOX_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_REWARD_BOX_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_NUM_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_LIST_NUM_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_NOT_RECV_CYCLE_CONTENTS_REWARD_ITEM_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_68_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_DELIVER_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_DELIVER_ITEM_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_69_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_DECIDE_DELIVERY_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_DECIDE_DELIVERY_ITEM_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_70_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_PARTY_QUEST_PROGRESS_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_PARTY_QUEST_PROGRESS_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_DEBUG_MAIN_QUEST_JUMP_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_DEBUG_MAIN_QUEST_JUMP_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_73_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_DEBUG_QUEST_RESET_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_DEBUG_QUEST_RESET_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_DEBUG_QUEST_RESET_ALL_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_DEBUG_QUEST_RESET_ALL_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_DEBUG_CYCLE_CONTENTS_POINT_UPLOAD_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_DEBUG_CYCLE_CONTENTS_POINT_UPLOAD_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_11_77_1_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_77_2_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_11_78_1_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_78_2_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_11_79_1_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_79_2_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_DEBUG_ENEMY_SET_PRESET_FIX_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_DEBUG_ENEMY_SET_PRESET_FIX_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_82_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_83_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_84_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_85_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_86_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_87_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_88_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_89_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_90_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_91_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_92_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_93_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_94_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_95_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_96_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_97_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_98_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_99_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_100_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_101_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_102_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_103_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_104_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_105_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_106_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_107_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_108_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_109_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_110_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_111_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_112_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_113_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_114_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_115_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_116_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_117_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_118_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_119_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_120_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_LEVEL_BONUS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_LEVEL_BONUS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_ADVENTURE_GUIDE_QUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_ADVENTURE_GUIDE_QUEST_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_ADVENTURE_GUIDE_QUEST_NOTICE_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_ADVENTURE_GUIDE_QUEST_NOTICE_RES);
            AddPacketIdEntry(packetIds, C2S_QUEST_SET_NAVIGATION_QUEST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_SET_NAVIGATION_QUEST_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_124_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_CANCEL_NAVIGATION_QUEST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_CANCEL_NAVIGATION_QUEST_RES);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_125_16_NTC);
            AddPacketIdEntry(packetIds, S2C_QUEST_11_126_16_NTC);
            AddPacketIdEntry(packetIds, C2S_QUEST_GET_END_CONTENTS_RECRUIT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_QUEST_GET_END_CONTENTS_RECRUIT_LIST_RES);

// Group: 12 - (STAGE)
            AddPacketIdEntry(packetIds, C2S_STAGE_GET_STAGE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_GET_STAGE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_AREA_CHANGE_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_AREA_CHANGE_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_GET_SP_AREA_CHANGE_ID_FROM_NPC_ID_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_GET_SP_AREA_CHANGE_ID_FROM_NPC_ID_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_GET_SP_AREA_CHANGE_LIST_FROM_OM_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_GET_SP_AREA_CHANGE_LIST_FROM_OM_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_GET_SP_AREA_CHANGE_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_GET_SP_AREA_CHANGE_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_UNISON_AREA_CHANGE_GET_RECRUITMENT_STATE_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_UNISON_AREA_CHANGE_GET_RECRUITMENT_STATE_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_UNISON_AREA_CHANGE_READY_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_UNISON_AREA_CHANGE_READY_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_UNISON_AREA_CHANGE_READY_CANCEL_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_UNISON_AREA_CHANGE_READY_CANCEL_RES);
            AddPacketIdEntry(packetIds, S2C_STAGE_12_9_16_NTC);
            AddPacketIdEntry(packetIds, S2C_STAGE_12_10_16_NTC);
            AddPacketIdEntry(packetIds, C2S_STAGE_GET_TICKET_DUNGEON_CATEGORY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_GET_TICKET_DUNGEON_CATEGORY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_GET_TICKET_DUNGEON_INFO_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_GET_TICKET_DUNGEON_INFO_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_STAGE_IS_EXIST_APP_CHARACTER_REQ);
            AddPacketIdEntry(packetIds, S2C_STAGE_IS_EXIST_APP_CHARACTER_RES);

// Group: 13 - (INSTANCE)
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_ENEMY_SET_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_ENEMY_SET_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_ENEMY_KILL_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_ENEMY_KILL_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_ITEM_SET_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_ITEM_SET_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_GATHERING_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_GATHERING_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_GATHERING_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_GATHERING_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_DROP_ITEM_SET_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_DROP_ITEM_SET_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_DROP_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_DROP_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_DROP_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_DROP_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_13_13_1_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_13_2_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_TRANING_ROOM_GET_ENEMY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_TRANING_ROOM_GET_ENEMY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_TRANING_ROOM_SET_ENEMY_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_TRANING_ROOM_SET_ENEMY_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_TREASURE_POINT_GET_CATEGORY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_TREASURE_POINT_GET_CATEGORY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_TREASURE_POINT_GET_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_TREASURE_POINT_GET_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_TREASURE_POINT_GET_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_TREASURE_POINT_GET_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_SET_OM_INSTANT_KEY_VALUE_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_SET_OM_INSTANT_KEY_VALUE_RES);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_20_16_NTC);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_OM_INSTANT_KEY_VALUE_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_OM_INSTANT_KEY_VALUE_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_RES);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_23_16_NTC);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_SET_INSTANT_KEY_VALUE_UL_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_SET_INSTANT_KEY_VALUE_UL_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_INSTANT_KEY_VALUE_UL_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_INSTANT_KEY_VALUE_UL_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_SET_INSTANT_KEY_VALUE_STR_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_SET_INSTANT_KEY_VALUE_STR_RES);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_INSTANT_KEY_VALUE_STR_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_INSTANT_KEY_VALUE_STR_RES);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_29_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_30_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_31_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_32_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_33_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_34_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_35_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_36_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_37_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_38_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_39_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_40_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_41_16_NTC);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_13_42_16_NTC);
            AddPacketIdEntry(packetIds, C2S_INSTANCE_GET_EX_OM_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_INSTANCE_GET_EX_OM_INFO_RES);

// Group: 14 - (WARP)
            AddPacketIdEntry(packetIds, C2S_WARP_RELEASE_WARP_POINT_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_RELEASE_WARP_POINT_RES);
            AddPacketIdEntry(packetIds, S2C_WARP_14_0_16_NTC);
            AddPacketIdEntry(packetIds, C2S_WARP_GET_RELEASE_WARP_POINT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_GET_RELEASE_WARP_POINT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_WARP_GET_WARP_POINT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_GET_WARP_POINT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_WARP_WARP_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_WARP_RES);
            AddPacketIdEntry(packetIds, C2S_WARP_GET_FAVORITE_WARP_POINT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_GET_FAVORITE_WARP_POINT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_WARP_FAVORITE_WARP_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_FAVORITE_WARP_RES);
            AddPacketIdEntry(packetIds, C2S_WARP_REGISTER_FAVORITE_WARP_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_REGISTER_FAVORITE_WARP_RES);
            AddPacketIdEntry(packetIds, C2S_WARP_PARTY_WARP_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_PARTY_WARP_RES);
            AddPacketIdEntry(packetIds, C2S_WARP_GET_AREA_WARP_POINT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_GET_AREA_WARP_POINT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_WARP_AREA_WARP_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_AREA_WARP_RES);
            AddPacketIdEntry(packetIds, C2S_WARP_GET_RETURN_LOCATION_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_GET_RETURN_LOCATION_RES);
            AddPacketIdEntry(packetIds, S2C_WARP_14_13_16_NTC);
            AddPacketIdEntry(packetIds, C2S_WARP_GET_START_POINT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_WARP_GET_START_POINT_LIST_RES);

// Group: 15
            AddPacketIdEntry(packetIds, S2C_15_65535_255);

// Group: 16 - (FRIEND)
            AddPacketIdEntry(packetIds, C2S_FRIEND_GET_FRIEND_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_FRIEND_GET_FRIEND_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_FRIEND_APPLY_FRIEND_REQ);
            AddPacketIdEntry(packetIds, S2C_FRIEND_APPLY_FRIEND_RES);
            AddPacketIdEntry(packetIds, S2C_FRIEND_16_1_16_NTC);
            AddPacketIdEntry(packetIds, C2S_FRIEND_APPROVE_FRIEND_REQ);
            AddPacketIdEntry(packetIds, S2C_FRIEND_APPROVE_FRIEND_RES);
            AddPacketIdEntry(packetIds, S2C_FRIEND_16_2_16_NTC);
            AddPacketIdEntry(packetIds, C2S_FRIEND_REMOVE_FRIEND_REQ);
            AddPacketIdEntry(packetIds, S2C_FRIEND_REMOVE_FRIEND_RES);
            AddPacketIdEntry(packetIds, S2C_FRIEND_16_3_16_NTC);
            AddPacketIdEntry(packetIds, C2S_FRIEND_REGISTER_FAVORITE_FRIEND_REQ);
            AddPacketIdEntry(packetIds, S2C_FRIEND_REGISTER_FAVORITE_FRIEND_RES);
            AddPacketIdEntry(packetIds, C2S_FRIEND_CANCEL_FRIEND_APPLICATION_REQ);
            AddPacketIdEntry(packetIds, S2C_FRIEND_CANCEL_FRIEND_APPLICATION_RES);
            AddPacketIdEntry(packetIds, S2C_FRIEND_16_5_16_NTC);
            AddPacketIdEntry(packetIds, C2S_FRIEND_GET_RECENT_CHARACTER_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_FRIEND_GET_RECENT_CHARACTER_LIST_RES);

// Group: 17 - (BLACK)
            AddPacketIdEntry(packetIds, C2S_BLACK_LIST_GET_BLACK_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_BLACK_LIST_GET_BLACK_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_BLACK_LIST_ADD_BLACK_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_BLACK_LIST_ADD_BLACK_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_BLACK_LIST_REMOVE_BLACK_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_BLACK_LIST_REMOVE_BLACK_LIST_RES);

// Group: 18 - (GROUP)
            AddPacketIdEntry(packetIds, C2S_GROUP_CHAT_GROUP_CHAT_GET_MEMBER_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_GROUP_CHAT_GROUP_CHAT_GET_MEMBER_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_GROUP_CHAT_GROUP_CHAT_INVITE_CHARACTER_REQ);
            AddPacketIdEntry(packetIds, S2C_GROUP_CHAT_GROUP_CHAT_INVITE_CHARACTER_RES);
            AddPacketIdEntry(packetIds, S2C_GROUP_18_1_16_NTC);
            AddPacketIdEntry(packetIds, C2S_GROUP_CHAT_GROUP_CHAT_LEAVE_CHARACTER_REQ);
            AddPacketIdEntry(packetIds, S2C_GROUP_CHAT_GROUP_CHAT_LEAVE_CHARACTER_RES);
            AddPacketIdEntry(packetIds, S2C_GROUP_18_2_16_NTC);
            AddPacketIdEntry(packetIds, C2S_GROUP_CHAT_GROUP_CHAT_KICK_CHARACTER_REQ);
            AddPacketIdEntry(packetIds, S2C_GROUP_CHAT_GROUP_CHAT_KICK_CHARACTER_RES);
            AddPacketIdEntry(packetIds, S2C_GROUP_18_3_16_NTC);
            AddPacketIdEntry(packetIds, S2C_GROUP_18_4_16_NTC);

// Group: 19 - (SKILL)
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_ACQUIRABLE_NORMAL_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_ACQUIRABLE_NORMAL_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_ACQUIRABLE_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_ACQUIRABLE_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_ACQUIRABLE_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_ACQUIRABLE_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_LEARN_NORMAL_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_LEARN_NORMAL_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_LEARN_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_LEARN_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_LEARN_ABILITY_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_LEARN_ABILITY_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_LEARN_PAWN_NORMAL_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_LEARN_PAWN_NORMAL_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_LEARN_PAWN_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_LEARN_PAWN_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_LEARN_PAWN_ABILITY_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_LEARN_PAWN_ABILITY_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_LEARNED_NORMAL_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_LEARNED_NORMAL_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_LEARNED_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_LEARNED_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_LEARNED_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_LEARNED_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_PAWN_LEARNED_NORMAL_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_PAWN_LEARNED_NORMAL_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_PAWN_LEARNED_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_PAWN_LEARNED_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_PAWN_LEARNED_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_PAWN_LEARNED_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_CHANGE_EX_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_CHANGE_EX_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_ABILITY_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_ABILITY_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_PAWN_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_PAWN_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_PAWN_ABILITY_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_PAWN_ABILITY_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_OFF_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_OFF_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_OFF_ABILITY_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_OFF_ABILITY_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_OFF_PAWN_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_OFF_PAWN_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_OFF_PAWN_ABILITY_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_OFF_PAWN_ABILITY_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_SET_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_SET_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_SET_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_SET_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_PAWN_SET_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_PAWN_SET_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_PAWN_SET_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_PAWN_SET_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_CURRENT_SET_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_CURRENT_SET_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_CURRENT_SET_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_CURRENT_SET_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_19_30_1_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_30_2_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_19_31_1_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_31_2_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_RELEASE_SKILL_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_RELEASE_SKILL_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_RELEASE_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_RELEASE_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_REGISTER_PRESET_ABILITY_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_REGISTER_PRESET_ABILITY_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_PRESET_ABILITY_NAME_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_PRESET_ABILITY_NAME_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_PRESET_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_PRESET_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_PRESET_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_PRESET_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_SET_PAWN_PRESET_ABILITY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_SET_PAWN_PRESET_ABILITY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_ABILITY_COST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_ABILITY_COST_RES);
            AddPacketIdEntry(packetIds, C2S_SKILL_GET_PAWN_ABILITY_COST_REQ);
            AddPacketIdEntry(packetIds, S2C_SKILL_GET_PAWN_ABILITY_COST_RES);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_41_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_42_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_43_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_44_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_45_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_46_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_47_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_48_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_49_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SKILL_19_50_16_NTC);

// Group: 20 - (SHOP)
            AddPacketIdEntry(packetIds, C2S_SHOP_GET_SHOP_GOODS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SHOP_GET_SHOP_GOODS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SHOP_BUY_SHOP_GOODS_REQ);
            AddPacketIdEntry(packetIds, S2C_SHOP_BUY_SHOP_GOODS_RES);

// Group: 21 - (INN)
            AddPacketIdEntry(packetIds, C2S_INN_GET_STAY_PRICE_REQ);
            AddPacketIdEntry(packetIds, S2C_INN_GET_STAY_PRICE_RES);
            AddPacketIdEntry(packetIds, C2S_INN_STAY_INN_REQ);
            AddPacketIdEntry(packetIds, S2C_INN_STAY_INN_RES);
            AddPacketIdEntry(packetIds, C2S_INN_GET_PENALTY_HEAL_STAY_PRICE_REQ);
            AddPacketIdEntry(packetIds, S2C_INN_GET_PENALTY_HEAL_STAY_PRICE_RES);
            AddPacketIdEntry(packetIds, C2S_INN_STAY_PENALTY_HEAL_INN_REQ);
            AddPacketIdEntry(packetIds, S2C_INN_STAY_PENALTY_HEAL_INN_RES);

// Group: 22 - (POTION)
            AddPacketIdEntry(packetIds, C2S_POTION_JOB_ELEMENT_CHECK_REQ);
            AddPacketIdEntry(packetIds, S2C_POTION_JOB_ELEMENT_CHECK_RES);
            AddPacketIdEntry(packetIds, C2S_POTION_JOB_ELEMENT_RELEASE_REQ);
            AddPacketIdEntry(packetIds, S2C_POTION_JOB_ELEMENT_RELEASE_RES);
            AddPacketIdEntry(packetIds, C2S_POTION_ORB_ELEMENT_CHECK_REQ);
            AddPacketIdEntry(packetIds, S2C_POTION_ORB_ELEMENT_CHECK_RES);
            AddPacketIdEntry(packetIds, C2S_POTION_ORB_ELEMENT_RELEASE_REQ);
            AddPacketIdEntry(packetIds, S2C_POTION_ORB_ELEMENT_RELEASE_RES);
            AddPacketIdEntry(packetIds, C2S_POTION_ADVENTURE_UTILITY_CHECK_REQ);
            AddPacketIdEntry(packetIds, S2C_POTION_ADVENTURE_UTILITY_CHECK_RES);
            AddPacketIdEntry(packetIds, C2S_POTION_ADVENTURE_UTILITY_RELEASE_REQ);
            AddPacketIdEntry(packetIds, S2C_POTION_ADVENTURE_UTILITY_RELEASE_RES);
            AddPacketIdEntry(packetIds, S2C_POTION_22_6_16_NTC);
            AddPacketIdEntry(packetIds, S2C_POTION_22_7_16_NTC);
            AddPacketIdEntry(packetIds, C2S_POTION_ELEMENT_GROUP_RELEASE_REQ);
            AddPacketIdEntry(packetIds, S2C_POTION_ELEMENT_GROUP_RELEASE_RES);
            AddPacketIdEntry(packetIds, C2S_POTION_ELEMENT_GROUP_CHECK_REQ);
            AddPacketIdEntry(packetIds, S2C_POTION_ELEMENT_GROUP_CHECK_RES);

// Group: 23 - (AREA)
            AddPacketIdEntry(packetIds, C2S_AREA_GET_AREA_MASTER_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_GET_AREA_MASTER_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_23_1_1_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_23_1_2_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_AREA_RANK_UP_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_AREA_RANK_UP_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_GET_AREA_SUPPLY_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_GET_AREA_SUPPLY_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_GET_AREA_SUPPLY_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_GET_AREA_SUPPLY_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_GET_AREA_POINT_DEBUG_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_GET_AREA_POINT_DEBUG_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_GET_AREA_RELEASE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_GET_AREA_RELEASE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_GET_LEADER_AREA_RELEASE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_GET_LEADER_AREA_RELEASE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_GET_AREA_QUEST_HINT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_GET_AREA_QUEST_HINT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_BUY_AREA_QUEST_HINT_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_BUY_AREA_QUEST_HINT_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_GET_SPOT_INFO_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_GET_SPOT_INFO_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_AREA_GET_AREA_BASE_INFO_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_GET_AREA_BASE_INFO_LIST_RES);
            AddPacketIdEntry(packetIds, S2C_AREA_23_13_16_NTC);
            AddPacketIdEntry(packetIds, S2C_AREA_23_14_16_NTC);
            AddPacketIdEntry(packetIds, C2S_AREA_23_15_1_REQ);
            AddPacketIdEntry(packetIds, S2C_AREA_23_15_2_RES);
            AddPacketIdEntry(packetIds, S2C_AREA_23_16_16_NTC);

// Group: 24 - (JOB)
            AddPacketIdEntry(packetIds, C2S_JOB_MASTER_GET_JOB_MASTER_ORDER_PROGRESS_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_MASTER_GET_JOB_MASTER_ORDER_PROGRESS_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_MASTER_REPORT_JOB_ORDER_PROGRESS_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_MASTER_REPORT_JOB_ORDER_PROGRESS_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_MASTER_ACTIVATE_JOB_ORDER_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_MASTER_ACTIVATE_JOB_ORDER_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_24_3_1_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_24_3_2_RES);
            AddPacketIdEntry(packetIds, S2C_JOB_24_4_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_24_5_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_24_6_16_NTC);

// Group: 25 - (ORB)
            AddPacketIdEntry(packetIds, C2S_ORB_DEVOTE_GET_ALL_ORB_ELEMENT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ORB_DEVOTE_GET_ALL_ORB_ELEMENT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ORB_DEVOTE_GET_RELEASE_ORB_ELEMENT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ORB_DEVOTE_GET_RELEASE_ORB_ELEMENT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ORB_DEVOTE_RELEASE_ORB_ELEMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_ORB_DEVOTE_RELEASE_ORB_ELEMENT_RES);
            AddPacketIdEntry(packetIds, C2S_ORB_DEVOTE_GET_PAWN_RELEASE_ORB_ELEMENT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ORB_DEVOTE_GET_PAWN_RELEASE_ORB_ELEMENT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ORB_DEVOTE_RELEASE_PAWN_ORB_ELEMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_ORB_DEVOTE_RELEASE_PAWN_ORB_ELEMENT_RES);
            AddPacketIdEntry(packetIds, C2S_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_REQ);
            AddPacketIdEntry(packetIds, S2C_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_RES);
            AddPacketIdEntry(packetIds, S2C_ORB_25_6_16_NTC);

// Group: 26 - (PROFILE)
            AddPacketIdEntry(packetIds, C2S_PROFILE_GET_CHARACTER_PROFILE_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_GET_CHARACTER_PROFILE_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_GET_MY_CHARACTER_PROFILE_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_GET_MY_CHARACTER_PROFILE_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_GET_AVAILABLE_BACKGROUND_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_GET_AVAILABLE_BACKGROUND_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_SET_ARISEN_PROFILE_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_SET_ARISEN_PROFILE_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_26_4_1_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_26_4_2_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_SET_PAWN_PROFILE_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_SET_PAWN_PROFILE_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_SET_PAWN_PROFILE_COMMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_SET_PAWN_PROFILE_COMMENT_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_UPDATE_ARISEN_PROFILE_SHARE_RANGE_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_UPDATE_ARISEN_PROFILE_SHARE_RANGE_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_SET_OBJECTIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_SET_OBJECTIVE_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_SET_MATCHING_PROFILE_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_SET_MATCHING_PROFILE_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_GET_MATCHING_PROFILE_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_GET_MATCHING_PROFILE_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_SET_SHORTCUT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_SET_SHORTCUT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_GET_SHORTCUT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_GET_SHORTCUT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_SET_COMMUNICATION_SHORTCUT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_SET_COMMUNICATION_SHORTCUT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_GET_COMMUNICATION_SHORTCUT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_GET_COMMUNICATION_SHORTCUT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_SET_MESSAGE_SET_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_SET_MESSAGE_SET_RES);
            AddPacketIdEntry(packetIds, C2S_PROFILE_GET_MESSAGE_SET_REQ);
            AddPacketIdEntry(packetIds, S2C_PROFILE_GET_MESSAGE_SET_RES);
            AddPacketIdEntry(packetIds, S2C_PROFILE_26_18_16_NTC);

// Group: 27 - (ACHIEVEMENT)
            AddPacketIdEntry(packetIds, C2S_ACHIEVEMENT_ACHIEVEMENT_GET_PROGRESS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_ACHIEVEMENT_GET_PROGRESS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ACHIEVEMENT_ACHIEVEMENT_GET_REWARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_ACHIEVEMENT_GET_REWARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ACHIEVEMENT_ACHIEVEMENT_GET_FURNITURE_REWARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_ACHIEVEMENT_GET_FURNITURE_REWARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ACHIEVEMENT_ACHIEVEMENT_GET_RECEIVABLE_REWARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_ACHIEVEMENT_GET_RECEIVABLE_REWARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ACHIEVEMENT_ACHIEVEMENT_REWARD_RECEIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_ACHIEVEMENT_REWARD_RECEIVE_RES);
            AddPacketIdEntry(packetIds, C2S_ACHIEVEMENT_27_5_1_REQ);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_27_5_2_RES);
            AddPacketIdEntry(packetIds, C2S_ACHIEVEMENT_27_6_1_REQ);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_27_6_2_RES);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_27_7_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_27_8_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_27_9_16_NTC);
            AddPacketIdEntry(packetIds, C2S_ACHIEVEMENT_ACHIEVEMENT_GET_CATEGORY_PROGRESS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ACHIEVEMENT_ACHIEVEMENT_GET_CATEGORY_PROGRESS_LIST_RES);

// Group: 28 - (GP)
            AddPacketIdEntry(packetIds, C2S_GP_GET_GP_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GET_GP_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GET_GP_DETAIL_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GET_GP_DETAIL_RES);
            AddPacketIdEntry(packetIds, C2S_GP_28_2_1_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_28_2_2_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GET_CAP_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GET_CAP_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GET_CAP_TO_GP_CHANGE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GET_CAP_TO_GP_CHANGE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_GP_CHANGE_CAP_TO_GP_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_CHANGE_CAP_TO_GP_RES);
            AddPacketIdEntry(packetIds, C2S_GP_COG_GET_ID_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_COG_GET_ID_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_SHOP_DISPLAY_GET_TYPE_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_SHOP_DISPLAY_GET_TYPE_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_SHOP_DISPLAY_GET_LINEUP_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_SHOP_DISPLAY_GET_LINEUP_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_SHOP_DISPLAY_BUY_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_SHOP_DISPLAY_BUY_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_SHOP_GET_COURSE_LINEUP_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_SHOP_GET_COURSE_LINEUP_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_SHOP_GET_ITEM_LINEUP_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_SHOP_GET_ITEM_LINEUP_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_SHOP_GET_PAWN_LINEUP_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_SHOP_GET_PAWN_LINEUP_RES);
            AddPacketIdEntry(packetIds, C2S_GP_28_13_1_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_28_13_2_RES);
            AddPacketIdEntry(packetIds, C2S_GP_28_14_1_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_28_14_2_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_SHOP_GET_BUY_HISTORY_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_SHOP_GET_BUY_HISTORY_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_SHOP_GET_CAP_CHARGE_URL_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_SHOP_GET_CAP_CHARGE_URL_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_COURSE_GET_AVAILABLE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_COURSE_GET_AVAILABLE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_COURSE_GET_VALID_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_COURSE_GET_VALID_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_COURSE_USE_FROM_AVAILABLE_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_COURSE_USE_FROM_AVAILABLE_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_COURSE_GET_VERSION_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_COURSE_GET_VERSION_RES);
            AddPacketIdEntry(packetIds, C2S_GP_28_21_1_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_28_21_2_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_EDIT_GET_VOICE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_EDIT_GET_VOICE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_SHOP_CAN_BUY_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_SHOP_CAN_BUY_RES);
            AddPacketIdEntry(packetIds, S2C_GP_28_24_16_NTC);
            AddPacketIdEntry(packetIds, S2C_GP_28_25_16_NTC);
            AddPacketIdEntry(packetIds, S2C_GP_28_26_16_NTC);
            AddPacketIdEntry(packetIds, S2C_GP_28_27_16_NTC);
            AddPacketIdEntry(packetIds, S2C_GP_28_28_16_NTC);
            AddPacketIdEntry(packetIds, S2C_GP_28_29_16_NTC);
            AddPacketIdEntry(packetIds, C2S_GP_GP_COURSE_EFFECT_MISMATCH_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_COURSE_EFFECT_MISMATCH_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_COURSE_INFO_RELOAD_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_COURSE_INFO_RELOAD_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GP_COURSE_EFFECT_RELOAD_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GP_COURSE_EFFECT_RELOAD_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GET_VALID_CHAT_COM_GROUP_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GET_VALID_CHAT_COM_GROUP_RES);
            AddPacketIdEntry(packetIds, C2S_GP_GET_UPDATE_APP_COURSE_BONUS_FLAG_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_GET_UPDATE_APP_COURSE_BONUS_FLAG_RES);
            AddPacketIdEntry(packetIds, C2S_GP_UPDATE_APP_COURSE_BONUS_REQ);
            AddPacketIdEntry(packetIds, S2C_GP_UPDATE_APP_COURSE_BONUS_RES);

// Group: 29 - (EQUIP)
            AddPacketIdEntry(packetIds, C2S_EQUIP_GET_CHARACTER_EQUIP_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_GET_CHARACTER_EQUIP_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_CHANGE_CHARACTER_EQUIP_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_CHANGE_CHARACTER_EQUIP_RES);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_1_16_NTC);
            AddPacketIdEntry(packetIds, C2S_EQUIP_CHANGE_CHARACTER_STORAGE_EQUIP_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_CHANGE_CHARACTER_STORAGE_EQUIP_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_GET_PAWN_EQUIP_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_GET_PAWN_EQUIP_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_CHANGE_PAWN_EQUIP_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_CHANGE_PAWN_EQUIP_RES);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_4_16_NTC);
            AddPacketIdEntry(packetIds, C2S_EQUIP_CHANGE_PAWN_STORAGE_EQUIP_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_CHANGE_PAWN_STORAGE_EQUIP_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_CHANGE_CHARACTER_EQUIP_JOB_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_CHANGE_CHARACTER_EQUIP_JOB_ITEM_RES);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_6_16_NTC);
            AddPacketIdEntry(packetIds, C2S_EQUIP_CHANGE_PAWN_EQUIP_JOB_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_CHANGE_PAWN_EQUIP_JOB_ITEM_RES);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_7_16_NTC);
            AddPacketIdEntry(packetIds, C2S_EQUIP_UPDATE_HIDE_CHARACTER_HEAD_ARMOR_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_UPDATE_HIDE_CHARACTER_HEAD_ARMOR_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_UPDATE_HIDE_PAWN_HEAD_ARMOR_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_UPDATE_HIDE_PAWN_HEAD_ARMOR_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_UPDATE_HIDE_CHARACTER_LANTERN_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_UPDATE_HIDE_CHARACTER_LANTERN_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_UPDATE_HIDE_PAWN_LANTERN_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_UPDATE_HIDE_PAWN_LANTERN_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_GET_EQUIP_PRESET_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_GET_EQUIP_PRESET_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_UPDATE_EQUIP_PRESET_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_UPDATE_EQUIP_PRESET_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_UPDATE_EQUIP_PRESET_NAME_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_UPDATE_EQUIP_PRESET_NAME_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_GET_CRAFT_LOCKED_ELEMENT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_GET_CRAFT_LOCKED_ELEMENT_LIST_RES);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_16_16_NTC);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_17_16_NTC);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_18_16_NTC);
            AddPacketIdEntry(packetIds, C2S_EQUIP_29_19_1_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_19_2_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_29_20_1_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_20_2_RES);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_20_16_NTC);
            AddPacketIdEntry(packetIds, C2S_EQUIP_29_21_1_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_21_2_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_29_22_1_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_22_2_RES);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_22_16_NTC);
            AddPacketIdEntry(packetIds, S2C_EQUIP_29_23_16_NTC);

// Group: 30 - (CRAFT)
            AddPacketIdEntry(packetIds, C2S_CRAFT_GET_CRAFT_PROGRESS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_GET_CRAFT_PROGRESS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_START_CRAFT_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_START_CRAFT_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_GET_CRAFT_PRODUCT_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_GET_CRAFT_PRODUCT_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_GET_CRAFT_PRODUCT_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_GET_CRAFT_PRODUCT_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_CANCEL_CRAFT_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_CANCEL_CRAFT_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_START_EQUIP_GRADE_UP_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_START_EQUIP_GRADE_UP_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_START_ATTACH_ELEMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_START_ATTACH_ELEMENT_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_START_DETACH_ELEMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_START_DETACH_ELEMENT_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_START_EQUIP_COLOR_CHANGE_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_START_EQUIP_COLOR_CHANGE_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_START_QUALITY_UP_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_START_QUALITY_UP_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_CRAFT_SKILL_UP_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_CRAFT_SKILL_UP_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_30_11_1_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_30_11_2_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_RESET_CRAFTPOINT_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_RESET_CRAFTPOINT_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_GET_CRAFT_SETTING_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_GET_CRAFT_SETTING_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_CRAFT_TIME_SAVE_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_CRAFT_TIME_SAVE_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_GET_CRAFT_IR_COLLECTION_VALUE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_GET_CRAFT_IR_COLLECTION_VALUE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_RELEASED_CRAFT_RECIPE_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_RELEASED_CRAFT_RECIPE_LIST_GET_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_CRAFT_SKILL_ANALYZE_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_CRAFT_SKILL_ANALYZE_RES);
            AddPacketIdEntry(packetIds, S2C_CRAFT_30_18_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CRAFT_30_19_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CRAFT_30_20_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CRAFT_30_21_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CRAFT_30_22_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CRAFT_30_23_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CRAFT_30_24_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CRAFT_DRAGON_SKILL_COMPOSE_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_DRAGON_SKILL_COMPOSE_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_DRAGON_SKILL_ANALYZE_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_DRAGON_SKILL_ANALYZE_RES);

// Group: 31 - (CLAN)
            AddPacketIdEntry(packetIds, C2S_CLAN_31_0_1_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_0_2_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_CREATE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_CREATE_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_UPDATE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_UPDATE_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_2_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_GET_MEMBER_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_GET_MEMBER_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_GET_MY_MEMBER_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_GET_MY_MEMBER_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SEARCH_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SEARCH_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_GET_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_GET_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_REGISTER_JOIN_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_REGISTER_JOIN_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_7_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_CANCEL_JOIN_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_CANCEL_JOIN_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_8_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_GET_JOIN_REQUESTED_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_GET_JOIN_REQUESTED_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_GET_MY_JOIN_REQUEST_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_GET_MY_JOIN_REQUEST_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_APPROVE_JOIN_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_APPROVE_JOIN_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_LEAVE_MEMBER_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_LEAVE_MEMBER_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_12_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_EXPEL_MEMBER_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_EXPEL_MEMBER_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_NEGOTIATE_MASTER_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_NEGOTIATE_MASTER_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_14_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SET_MEMBER_RANK_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SET_MEMBER_RANK_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_15_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_GET_MEMBER_NUM_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_GET_MEMBER_NUM_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SETTING_UPDATE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SETTING_UPDATE_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_INVITE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_INVITE_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_18_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_INVITE_ACCEPT_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_INVITE_ACCEPT_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SCOUT_ENTRY_REGISTER_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SCOUT_ENTRY_REGISTER_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SCOUT_ENTRY_CANCEL_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SCOUT_ENTRY_CANCEL_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SCOUT_ENTRY_GET_MY_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SCOUT_ENTRY_GET_MY_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SCOUT_ENTRY_SEARCH_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SCOUT_ENTRY_SEARCH_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SCOUT_ENTRY_INVITE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SCOUT_ENTRY_INVITE_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_24_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SCOUT_ENTRY_GET_INVITE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SCOUT_ENTRY_GET_INVITE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SCOUT_ENTRY_CANCEL_INVITE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SCOUT_ENTRY_CANCEL_INVITE_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_26_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SCOUT_ENTRY_GET_INVITED_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SCOUT_ENTRY_GET_INVITED_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SCOUT_ENTRY_APPROVE_INVITED_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SCOUT_ENTRY_APPROVE_INVITED_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_GET_MY_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_GET_MY_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_GET_HISTORY_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_GET_HISTORY_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_BASE_GET_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_BASE_GET_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_BASE_RELEASE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_BASE_RELEASE_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SHOP_GET_FUNCTION_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SHOP_GET_FUNCTION_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SHOP_GET_BUFF_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SHOP_GET_BUFF_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SHOP_BUY_FUNCTION_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SHOP_BUY_FUNCTION_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_SHOP_BUY_BUFF_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_SHOP_BUY_BUFF_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_CONCIERGE_UPDATE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_CONCIERGE_UPDATE_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_CONCIERGE_GET_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_CONCIERGE_GET_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_PARTNER_PAWN_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_PARTNER_PAWN_LIST_GET_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_CLAN_PARTNER_PAWN_DATA_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_CLAN_PARTNER_PAWN_DATA_GET_RES);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_41_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_42_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_43_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_44_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_45_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_46_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_47_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_48_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_49_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_50_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CLAN_31_51_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CLAN_SET_FURNITURE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_SET_FURNITURE_RES);
            AddPacketIdEntry(packetIds, C2S_CLAN_GET_FURNITURE_REQ);
            AddPacketIdEntry(packetIds, S2C_CLAN_GET_FURNITURE_RES);

// Group: 32 - (RANDOM)
            AddPacketIdEntry(packetIds, C2S_RANDOM_STAGE_RANDOM_STAGE_GET_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_RANDOM_STAGE_RANDOM_STAGE_GET_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_RANDOM_STAGE_RANDOM_STAGE_CLEAR_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_RANDOM_STAGE_RANDOM_STAGE_CLEAR_INFO_RES);

// Group: 33 - (JOB)
            AddPacketIdEntry(packetIds, C2S_JOB_GET_JOB_CHANGE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_GET_JOB_CHANGE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_CHANGE_JOB_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_CHANGE_JOB_RES);
            AddPacketIdEntry(packetIds, S2C_JOB_33_1_16_NTC);
            AddPacketIdEntry(packetIds, C2S_JOB_CHANGE_PAWN_JOB_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_CHANGE_PAWN_JOB_RES);
            AddPacketIdEntry(packetIds, S2C_JOB_33_2_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_3_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_4_16_NTC);
            AddPacketIdEntry(packetIds, C2S_JOB_RESET_JOBPOINT_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_RESET_JOBPOINT_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_GET_EXP_MODE_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_GET_EXP_MODE_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_UPDATE_EXP_MODE_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_UPDATE_EXP_MODE_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_GET_PLAY_POINT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_GET_PLAY_POINT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_JOB_VALUE_SHOP_GET_LINEUP_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_JOB_VALUE_SHOP_GET_LINEUP_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_JOB_VALUE_SHOP_BUY_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_JOB_VALUE_SHOP_BUY_ITEM_RES);
            AddPacketIdEntry(packetIds, S2C_JOB_33_11_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_12_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_13_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_14_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_15_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_16_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_17_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_18_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_19_16_NTC);
            AddPacketIdEntry(packetIds, S2C_JOB_33_20_16_NTC);

// Group: 34 - (ENTRY)
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_CREATE_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_CREATE_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_RECREATE_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_RECREATE_RES);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_3_16_NTC);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_ENTRY_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_ENTRY_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_LEAVE_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_LEAVE_RES);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_5_16_NTC);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_READY_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_READY_RES);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_6_16_NTC);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_FORCE_START_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_FORCE_START_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_MYSELF_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_MYSELF_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_EXTEND_TIMEOUT_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_EXTEND_TIMEOUT_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_LOCK_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_LOCK_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_CHANGE_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_CHANGE_RES);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_12_16_NTC);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INVITE_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INVITE_RES);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_13_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_14_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_15_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_16_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_17_16_NTC);
            AddPacketIdEntry(packetIds, S2C_ENTRY_34_18_16_NTC);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_PARTY_RECRUIT_CATEGORY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_PARTY_RECRUIT_CATEGORY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_ENTRY_BOARD_ITEM_KICK_REQ);
            AddPacketIdEntry(packetIds, S2C_ENTRY_BOARD_ITEM_KICK_RES);

// Group: 35 - (CONTEXT)
            AddPacketIdEntry(packetIds, C2S_CONTEXT_GET_LOBBY_PLAYER_CONTEXT_REQ);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_GET_LOBBY_PLAYER_CONTEXT_RES);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_0_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CONTEXT_GET_PARTY_PLAYER_CONTEXT_REQ);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_GET_PARTY_PLAYER_CONTEXT_RES);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_1_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CONTEXT_GET_ALL_PLAYER_CONTEXT_REQ);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_GET_ALL_PLAYER_CONTEXT_RES);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_2_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CONTEXT_GET_PARTY_MYPAWN_CONTEXT_REQ);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_GET_PARTY_MYPAWN_CONTEXT_RES);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_3_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CONTEXT_GET_PARTY_RENTED_PAWN_CONTEXT_REQ);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_GET_PARTY_RENTED_PAWN_CONTEXT_RES);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_4_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CONTEXT_GET_SET_CONTEXT_REQ);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_GET_SET_CONTEXT_RES);
            AddPacketIdEntry(packetIds, C2S_CONTEXT_MASTER_THROW_REQ);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_MASTER_THROW_RES);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_10_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_11_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_12_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_13_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_14_16_NTC);
            AddPacketIdEntry(packetIds, S2C_CONTEXT_35_15_16_NTC);

// Group: 36 - (BAZAAR)
            AddPacketIdEntry(packetIds, C2S_BAZAAR_GET_CHARACTER_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_GET_CHARACTER_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_GET_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_GET_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_GET_ITEM_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_GET_ITEM_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_GET_ITEM_HISTORY_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_GET_ITEM_HISTORY_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_EXHIBIT_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_EXHIBIT_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_RE_EXHIBIT_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_RE_EXHIBIT_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_CANCEL_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_CANCEL_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_PROCEEDS_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_PROCEEDS_RES);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_36_7_16_NTC);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_RECEIVE_PROCEEDS_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_RECEIVE_PROCEEDS_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_GET_ITEM_PRICE_LIMIT_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_GET_ITEM_PRICE_LIMIT_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_36_10_1_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_36_10_2_RES);
            AddPacketIdEntry(packetIds, C2S_BAZAAR_GET_EXHIBIT_POSSIBLE_NUM_REQ);
            AddPacketIdEntry(packetIds, S2C_BAZAAR_GET_EXHIBIT_POSSIBLE_NUM_RES);

// Group: 37 - (MAIL)
            AddPacketIdEntry(packetIds, C2S_MAIL_MAIL_GET_LIST_HEAD_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_MAIL_GET_LIST_HEAD_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_MAIL_GET_LIST_DATA_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_MAIL_GET_LIST_DATA_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_MAIL_GET_LIST_FOOT_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_MAIL_GET_LIST_FOOT_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_MAIL_GET_TEXT_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_MAIL_GET_TEXT_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_MAIL_DELETE_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_MAIL_DELETE_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_MAIL_SEND_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_MAIL_SEND_RES);
            AddPacketIdEntry(packetIds, S2C_MAIL_37_5_16_NTC);
            AddPacketIdEntry(packetIds, C2S_MAIL_SYSTEM_MAIL_GET_LIST_HEAD_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_SYSTEM_MAIL_GET_LIST_HEAD_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_SYSTEM_MAIL_GET_LIST_DATA_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_SYSTEM_MAIL_GET_LIST_DATA_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_SYSTEM_MAIL_GET_LIST_FOOT_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_SYSTEM_MAIL_GET_LIST_FOOT_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_SYSTEM_MAIL_GET_TEXT_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_SYSTEM_MAIL_GET_TEXT_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_SYSTEM_MAIL_GET_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_SYSTEM_MAIL_GET_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_SYSTEM_MAIL_GET_ALL_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_SYSTEM_MAIL_GET_ALL_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_MAIL_SYSTEM_MAIL_DELETE_REQ);
            AddPacketIdEntry(packetIds, S2C_MAIL_SYSTEM_MAIL_DELETE_RES);
            AddPacketIdEntry(packetIds, S2C_MAIL_37_13_16_NTC);

// Group: 38 - (RANKING)
            AddPacketIdEntry(packetIds, C2S_RANKING_BOARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_RANKING_BOARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_RANKING_RANK_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_RANKING_RANK_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_RANKING_RANK_LIST_BY_QUEST_SCHEDULE_ID_REQ);
            AddPacketIdEntry(packetIds, S2C_RANKING_RANK_LIST_BY_QUEST_SCHEDULE_ID_RES);
            AddPacketIdEntry(packetIds, C2S_RANKING_RANK_LIST_BY_CHARACTER_ID_REQ);
            AddPacketIdEntry(packetIds, S2C_RANKING_RANK_LIST_BY_CHARACTER_ID_RES);
            AddPacketIdEntry(packetIds, C2S_RANKING_38_4_1_REQ);
            AddPacketIdEntry(packetIds, S2C_RANKING_38_4_2_RES);

// Group: 39 - (GACHA)
            AddPacketIdEntry(packetIds, C2S_GACHA_GACHA_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_GACHA_GACHA_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_GACHA_GACHA_BUY_REQ);
            AddPacketIdEntry(packetIds, S2C_GACHA_GACHA_BUY_RES);

// Group: 40
            AddPacketIdEntry(packetIds, C2S_40_2_1_REQ);
            AddPacketIdEntry(packetIds, S2C_40_2_2_RES);

// Group: 41 - (CHARACTER)
            AddPacketIdEntry(packetIds, C2S_CHARACTER_EDIT_GET_UNLOCKED_EDIT_PARTS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_EDIT_GET_UNLOCKED_EDIT_PARTS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_EDIT_GET_UNLOCKED_PAWN_EDIT_PARTS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_EDIT_GET_UNLOCKED_PAWN_EDIT_PARTS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_RES);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_41_4_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_EX_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_EX_RES);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_EX_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_EX_RES);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_41_7_16_NTC);
            AddPacketIdEntry(packetIds, C2S_CHARACTER_EDIT_GET_SHOP_PRICE_REQ);
            AddPacketIdEntry(packetIds, S2C_CHARACTER_EDIT_GET_SHOP_PRICE_RES);

// Group: 42 - (PHOTO)
            AddPacketIdEntry(packetIds, C2S_PHOTO_PHOTO_GET_AUTH_ADDRESS_REQ);
            AddPacketIdEntry(packetIds, S2C_PHOTO_PHOTO_GET_AUTH_ADDRESS_RES);

// Group: 43 - (LOADING)
            AddPacketIdEntry(packetIds, C2S_LOADING_INFO_LOADING_GET_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_LOADING_INFO_LOADING_GET_INFO_RES);

// Group: 44 - (CERT)
            AddPacketIdEntry(packetIds, C2S_CERT_CLIENT_CHALLENGE_REQ);
            AddPacketIdEntry(packetIds, S2C_CERT_CLIENT_CHALLENGE_RES);
            AddPacketIdEntry(packetIds, S2C_CERT_44_1_16_NTC);

// Group: 45 - (STAMP)
            AddPacketIdEntry(packetIds, C2S_STAMP_BONUS_GET_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_STAMP_BONUS_GET_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_STAMP_BONUS_CHECK_REQ);
            AddPacketIdEntry(packetIds, S2C_STAMP_BONUS_CHECK_RES);
            AddPacketIdEntry(packetIds, C2S_STAMP_BONUS_UPDATE_REQ);
            AddPacketIdEntry(packetIds, S2C_STAMP_BONUS_UPDATE_RES);
            AddPacketIdEntry(packetIds, C2S_STAMP_BONUS_RECIEVE_DAILY_REQ);
            AddPacketIdEntry(packetIds, S2C_STAMP_BONUS_RECIEVE_DAILY_RES);
            AddPacketIdEntry(packetIds, C2S_STAMP_BONUS_RECIEVE_TOTAL_REQ);
            AddPacketIdEntry(packetIds, S2C_STAMP_BONUS_RECIEVE_TOTAL_RES);

// Group: 46 - (NG)
            AddPacketIdEntry(packetIds, C2S_NG_WORD_NG_WORD_GET_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_NG_WORD_NG_WORD_GET_LIST_RES);

// Group: 47 - (EVENT)
            AddPacketIdEntry(packetIds, C2S_EVENT_CODE_EVENT_CODE_INPUT_REQ);
            AddPacketIdEntry(packetIds, S2C_EVENT_CODE_EVENT_CODE_INPUT_RES);

// Group: 48 - (DLC)
            AddPacketIdEntry(packetIds, C2S_DLC_DLC_GET_BOUGHT_REQ);
            AddPacketIdEntry(packetIds, S2C_DLC_DLC_GET_BOUGHT_RES);
            AddPacketIdEntry(packetIds, C2S_DLC_DLC_USE_REQ);
            AddPacketIdEntry(packetIds, S2C_DLC_DLC_USE_RES);
            AddPacketIdEntry(packetIds, C2S_DLC_DLC_GET_HISTORY_REQ);
            AddPacketIdEntry(packetIds, S2C_DLC_DLC_GET_HISTORY_RES);
            AddPacketIdEntry(packetIds, C2S_DLC_48_3_1_REQ);
            AddPacketIdEntry(packetIds, S2C_DLC_48_3_2_RES);
            AddPacketIdEntry(packetIds, S2C_DLC_48_3_16_NTC);
            AddPacketIdEntry(packetIds, S2C_DLC_48_4_16_NTC);

// Group: 49 - (SUPPORT)
            AddPacketIdEntry(packetIds, C2S_SUPPORT_POINT_SUPPORT_POINT_GET_RATE_REQ);
            AddPacketIdEntry(packetIds, S2C_SUPPORT_POINT_SUPPORT_POINT_GET_RATE_RES);
            AddPacketIdEntry(packetIds, C2S_SUPPORT_POINT_SUPPORT_POINT_USE_REQ);
            AddPacketIdEntry(packetIds, S2C_SUPPORT_POINT_SUPPORT_POINT_USE_RES);

// Group: 50 - (ITEM)
            AddPacketIdEntry(packetIds, C2S_ITEM_SORT_GET_ITEM_SORTDATA_BIN_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_SORT_GET_ITEM_SORTDATA_BIN_RES);
            AddPacketIdEntry(packetIds, S2C_ITEM_50_0_16_NTC);
            AddPacketIdEntry(packetIds, C2S_ITEM_SORT_SET_ITEM_SORTDATA_BIN_REQ);
            AddPacketIdEntry(packetIds, S2C_ITEM_SORT_SET_ITEM_SORTDATA_BIN_RES);

// Group: 51 - (DISPEL)
            AddPacketIdEntry(packetIds, C2S_DISPEL_GET_DISPEL_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_DISPEL_GET_DISPEL_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_DISPEL_EXCHANGE_DISPEL_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_DISPEL_EXCHANGE_DISPEL_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_DISPEL_GET_DISPEL_ITEM_SETTING_REQ);
            AddPacketIdEntry(packetIds, S2C_DISPEL_GET_DISPEL_ITEM_SETTING_RES);
            AddPacketIdEntry(packetIds, C2S_DISPEL_LOCK_SETTING_REQ);
            AddPacketIdEntry(packetIds, S2C_DISPEL_LOCK_SETTING_RES);
            AddPacketIdEntry(packetIds, C2S_DISPEL_GET_LOCK_SETTING_REQ);
            AddPacketIdEntry(packetIds, S2C_DISPEL_GET_LOCK_SETTING_RES);

// Group: 52 - (MY)
            AddPacketIdEntry(packetIds, C2S_MY_ROOM_MY_ROOM_RELEASE_REQ);
            AddPacketIdEntry(packetIds, S2C_MY_ROOM_MY_ROOM_RELEASE_RES);
            AddPacketIdEntry(packetIds, C2S_MY_ROOM_FURNITURE_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_MY_ROOM_FURNITURE_LIST_GET_RES);
            AddPacketIdEntry(packetIds, C2S_MY_ROOM_FURNITURE_LAYOUT_REQ);
            AddPacketIdEntry(packetIds, S2C_MY_ROOM_FURNITURE_LAYOUT_RES);
            AddPacketIdEntry(packetIds, C2S_MY_ROOM_MY_ROOM_BGM_UPDATE_REQ);
            AddPacketIdEntry(packetIds, S2C_MY_ROOM_MY_ROOM_BGM_UPDATE_RES);
            AddPacketIdEntry(packetIds, C2S_MY_ROOM_OTHER_ROOM_LAYOUT_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_MY_ROOM_OTHER_ROOM_LAYOUT_GET_RES);
            AddPacketIdEntry(packetIds, C2S_MY_52_5_1_REQ);
            AddPacketIdEntry(packetIds, S2C_MY_52_5_2_RES);
            AddPacketIdEntry(packetIds, S2C_MY_52_5_16_NTC);
            AddPacketIdEntry(packetIds, C2S_MY_ROOM_UPDATE_PLANETARIUM_REQ);
            AddPacketIdEntry(packetIds, S2C_MY_ROOM_UPDATE_PLANETARIUM_RES);
            AddPacketIdEntry(packetIds, C2S_MY_ROOM_GET_OTHER_ROOM_PERMISSION_REQ);
            AddPacketIdEntry(packetIds, S2C_MY_ROOM_GET_OTHER_ROOM_PERMISSION_RES);
            AddPacketIdEntry(packetIds, C2S_MY_ROOM_SET_OTHER_ROOM_PERMISSION_REQ);
            AddPacketIdEntry(packetIds, S2C_MY_ROOM_SET_OTHER_ROOM_PERMISSION_RES);

// Group: 53 - (PARTNER)
            AddPacketIdEntry(packetIds, C2S_PARTNER_PAWN_PARTNER_PAWN_SET_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTNER_PAWN_PARTNER_PAWN_SET_RES);
            AddPacketIdEntry(packetIds, C2S_PARTNER_PAWN_RELEASED_EDIT_PARTS_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTNER_PAWN_RELEASED_EDIT_PARTS_LIST_GET_RES);
            AddPacketIdEntry(packetIds, C2S_PARTNER_PAWN_RELEASED_EMOTION_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTNER_PAWN_RELEASED_EMOTION_LIST_GET_RES);
            AddPacketIdEntry(packetIds, C2S_PARTNER_PAWN_RELEASED_PAWN_TALK_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTNER_PAWN_RELEASED_PAWN_TALK_LIST_GET_RES);
            AddPacketIdEntry(packetIds, C2S_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_LIST_GET_RES);
            AddPacketIdEntry(packetIds, C2S_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTNER_PAWN_PAWN_LIKABILITY_REWARD_GET_RES);
            AddPacketIdEntry(packetIds, C2S_PARTNER_PAWN_PAWN_LIKABILITY_RELEASED_REWARD_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTNER_PAWN_PAWN_LIKABILITY_RELEASED_REWARD_LIST_GET_RES);
            AddPacketIdEntry(packetIds, C2S_PARTNER_PAWN_PARTNER_PAWN_NEXT_PRESENT_TIME_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTNER_PAWN_PARTNER_PAWN_NEXT_PRESENT_TIME_GET_RES);
            AddPacketIdEntry(packetIds, C2S_PARTNER_PAWN_PRESENT_FOR_PARTNER_PAWN_REQ);
            AddPacketIdEntry(packetIds, S2C_PARTNER_PAWN_PRESENT_FOR_PARTNER_PAWN_RES);
            AddPacketIdEntry(packetIds, S2C_PARTNER_53_9_16_NTC);

// Group: 54 - (CRAFT)
            AddPacketIdEntry(packetIds, C2S_CRAFT_RECIPE_GET_CRAFT_RECIPE_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_RECIPE_GET_CRAFT_RECIPE_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_RECIPE_GET_CRAFT_RECIPE_DESIGNATE_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_RECIPE_GET_CRAFT_RECIPE_DESIGNATE_RES);
            AddPacketIdEntry(packetIds, C2S_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_REQ);
            AddPacketIdEntry(packetIds, S2C_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_RES);

// Group: 55 - (JOB)
            AddPacketIdEntry(packetIds, C2S_JOB_ORB_TREE_GET_JOB_ORB_TREE_STATUS_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_ORB_TREE_GET_JOB_ORB_TREE_STATUS_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_ORB_TREE_GET_ALL_JOB_ORB_ELEMENT_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_ORB_TREE_GET_ALL_JOB_ORB_ELEMENT_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_ORB_TREE_RELEASE_JOB_ORB_ELEMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_ORB_TREE_RELEASE_JOB_ORB_ELEMENT_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_ORB_TREE_GET_CURRENCY_EXCHANGE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_ORB_TREE_GET_CURRENCY_EXCHANGE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_ORB_TREE_EXCHANGE_CURRENCY_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_ORB_TREE_EXCHANGE_CURRENCY_RES);
            AddPacketIdEntry(packetIds, S2C_JOB_55_5_16_NTC);

// Group: 56 - (SERVER)
            AddPacketIdEntry(packetIds, C2S_SERVER_UI_SERVER_UI_COMMAND_REQ);
            AddPacketIdEntry(packetIds, S2C_SERVER_UI_SERVER_UI_COMMAND_RES);
            AddPacketIdEntry(packetIds, S2C_SERVER_56_1_16_NTC);

// Group: 57 - (BOX)
            AddPacketIdEntry(packetIds, C2S_BOX_GACHA_BOX_GACHA_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_BOX_GACHA_BOX_GACHA_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_BOX_GACHA_BOX_GACHA_BUY_REQ);
            AddPacketIdEntry(packetIds, S2C_BOX_GACHA_BOX_GACHA_BUY_RES);
            AddPacketIdEntry(packetIds, C2S_BOX_GACHA_BOX_GACHA_RESET_REQ);
            AddPacketIdEntry(packetIds, S2C_BOX_GACHA_BOX_GACHA_RESET_RES);
            AddPacketIdEntry(packetIds, C2S_BOX_GACHA_BOX_GACHA_DRAW_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_BOX_GACHA_BOX_GACHA_DRAW_INFO_RES);

// Group: 58 - (PAWN)
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_MY_SALLY_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_MY_SALLY_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CHARGE_SALLY_COUNT_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CHARGE_SALLY_COUNT_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_SALLY_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_SALLY_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CHANGE_GOLDEN_SALLY_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CHANGE_GOLDEN_SALLY_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_REWARD_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_SALLY_REWARD_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_CANCEL_SALLY_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_CANCEL_SALLY_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_EXPEDITION_PAWN_EXPEDITION_GET_REWARD_DROP_ITEM_RES);

// Group: 59 - (INFINITY)
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_CURRENT_EVENT_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_CURRENT_EVENT_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_CATEGORY_FROM_NPC_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_CATEGORY_FROM_NPC_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_REQUIRED_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_REQUIRED_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_DELIVER_CLIENT_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_DELIVER_CLIENT_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_CATEGORY_STATUS_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_CATEGORY_STATUS_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_EVENT_STATUS_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_EVENT_STATUS_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_NUM_BORDER_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_NUM_BORDER_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_RANKING_BORDER_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_RANKING_BORDER_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_CATEGORY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_CATEGORY_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_CATEGORY_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_CATEGORY_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_GET_CATEGORY_RANKING_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_GET_CATEGORY_RANKING_RES);
            AddPacketIdEntry(packetIds, C2S_INFINITY_DELIVERY_RECEIVE_BORDER_REWARD_REQ);
            AddPacketIdEntry(packetIds, S2C_INFINITY_DELIVERY_RECEIVE_BORDER_REWARD_RES);

// Group: 60 - (NPC)
            AddPacketIdEntry(packetIds, C2S_NPC_GET_NPC_EXTENDED_FACILITY_REQ);
            AddPacketIdEntry(packetIds, S2C_NPC_GET_NPC_EXTENDED_FACILITY_RES);
            AddPacketIdEntry(packetIds, S2C_NPC_60_1_16_NTC);

// Group: 61 - (PAWN)
            AddPacketIdEntry(packetIds, C2S_PAWN_TRAINING_GET_TRAINING_STATUS_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_TRAINING_GET_TRAINING_STATUS_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_TRAINING_SET_TRAINING_STATUS_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_TRAINING_SET_TRAINING_STATUS_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_TRAINING_GET_PREPARETION_INFO_TO_ADVICE_RES);
            AddPacketIdEntry(packetIds, S2C_PAWN_61_3_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_61_4_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_61_5_16_NTC);

// Group: 62 - (SEASON)
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_ID_FROM_NPC_ID_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_ID_FROM_NPC_ID_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_EXTEND_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_EXTEND_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_COMPLETE_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_COMPLETE_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_OPEN_COMPLETE_DOOR_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_OPEN_COMPLETE_DOOR_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_EXEC_EX_RESERVATION_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_EXEC_EX_RESERVATION_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_NPC_ID_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_NPC_ID_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_EXTENDABLE_BLOCKADE_LIST_FROM_NPC_ID_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_EXTENDABLE_BLOCKADE_LIST_FROM_NPC_ID_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_OM_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_OM_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_EX_REQUIRED_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_EX_REQUIRED_ITEM_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_DELIVER_ITEM_FOR_EX_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_DELIVER_ITEM_FOR_EX_RES);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_13_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_14_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_15_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_16_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_17_16_NTC);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_LIST_FROM_OM_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_LIST_FROM_OM_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_FOR_VIEW_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_FOR_VIEW_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_SOUL_ORDEAL_READY_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_SOUL_ORDEAL_READY_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_SOUL_ORDEAL_CANCEL_READY_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_SOUL_ORDEAL_CANCEL_READY_RES);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_22_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_23_16_NTC);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_EXECUTE_SOUL_ORDEAL_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_EXECUTE_SOUL_ORDEAL_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_INTERRUPT_SOUL_ORDEAL_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_INTERRUPT_SOUL_ORDEAL_RES);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_26_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_27_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_28_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_29_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_30_16_NTC);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_IS_UNLOCKED_KEY_POINT_DOOR_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_IS_UNLOCKED_KEY_POINT_DOOR_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_IS_UNLOCKED_SOUL_ORDEAL_DOOR_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_IS_UNLOCKED_SOUL_ORDEAL_DOOR_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_BUFF_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_BUFF_RES);
            AddPacketIdEntry(packetIds, C2S_SEASON_DUNGEON_UPDATE_KEY_POINT_DOOR_STATUS_REQ);
            AddPacketIdEntry(packetIds, S2C_SEASON_DUNGEON_UPDATE_KEY_POINT_DOOR_STATUS_RES);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_37_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_38_16_NTC);
            AddPacketIdEntry(packetIds, S2C_SEASON_62_39_16_NTC);

// Group: 63
            AddPacketIdEntry(packetIds, S2C_63_0_16_NTC);
            AddPacketIdEntry(packetIds, S2C_63_1_16_NTC);
            AddPacketIdEntry(packetIds, S2C_63_2_16_NTC);
            AddPacketIdEntry(packetIds, S2C_63_3_16_NTC);
            AddPacketIdEntry(packetIds, S2C_63_6_16_NTC);
            AddPacketIdEntry(packetIds, S2C_63_7_16_NTC);
            AddPacketIdEntry(packetIds, S2C_63_10_16_NTC);

// Group: 64 - (MANDRAGORA)
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_GET_MY_MANDRAGORA_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_GET_MY_MANDRAGORA_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_CREATE_MY_MANDRAGORA_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_CREATE_MY_MANDRAGORA_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_CHANGE_SOIL_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_CHANGE_SOIL_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_GET_CRAFT_RECIPE_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_GET_CRAFT_RECIPE_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_BEGIN_CRAFT_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_BEGIN_CRAFT_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_CHANGE_GOLDEN_CRAFT_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_CHANGE_GOLDEN_CRAFT_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_GET_MANURE_ITEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_GET_MANURE_ITEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_FINISH_CRAFT_CHECK_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_FINISH_CRAFT_CHECK_RES);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_64_7_16_NTC);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_FINISH_ITEM_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_FINISH_ITEM_GET_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_GET_SPECIES_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_GET_SPECIES_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_CHANGE_SPECIES_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_CHANGE_SPECIES_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_CHANGE_NAME_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_CHANGE_NAME_RES);
            AddPacketIdEntry(packetIds, C2S_MANDRAGORA_GET_SPECIES_CATEGORY_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_MANDRAGORA_GET_SPECIES_CATEGORY_LIST_RES);

// Group: 65 - (EQUIP)
            AddPacketIdEntry(packetIds, S2C_EQUIP_65_0_16_NTC);
            AddPacketIdEntry(packetIds, C2S_EQUIP_ENHANCED_SUB_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_ENHANCED_SUB_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_ENHANCED_AWAKEN_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_ENHANCED_AWAKEN_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_ENHANCED_GET_PACKS_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_ENHANCED_GET_PACKS_RES);
            AddPacketIdEntry(packetIds, C2S_EQUIP_ENHANCED_ENHANCE_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_EQUIP_ENHANCED_ENHANCE_ITEM_RES);

// Group: 66 - (JOB)
            AddPacketIdEntry(packetIds, C2S_JOB_EMBLEM_GET_EMBLEM_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_EMBLEM_GET_EMBLEM_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_EMBLEM_GET_EMBLEM_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_EMBLEM_GET_EMBLEM_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_EMBLEM_UPDATE_LEVEL_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_EMBLEM_UPDATE_LEVEL_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_EMBLEM_UPDATE_PARAM_LEVEL_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_EMBLEM_UPDATE_PARAM_LEVEL_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_EMBLEM_RESET_PARAM_LEVEL_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_EMBLEM_RESET_PARAM_LEVEL_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_EMBLEM_ATTACH_ELEMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_EMBLEM_ATTACH_ELEMENT_RES);
            AddPacketIdEntry(packetIds, C2S_JOB_EMBLEM_DETTACH_ELEMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_JOB_EMBLEM_DETTACH_ELEMENT_RES);
            AddPacketIdEntry(packetIds, S2C_JOB_66_7_16_NTC);

// Group: 67 - (RECYCLE)
            AddPacketIdEntry(packetIds, C2S_RECYCLE_GET_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_RECYCLE_GET_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_RECYCLE_GET_LOT_FORECAST_REQ);
            AddPacketIdEntry(packetIds, S2C_RECYCLE_GET_LOT_FORECAST_RES);
            AddPacketIdEntry(packetIds, C2S_RECYCLE_START_EXCHANGE_REQ);
            AddPacketIdEntry(packetIds, S2C_RECYCLE_START_EXCHANGE_RES);
            AddPacketIdEntry(packetIds, C2S_RECYCLE_RESET_COUNT_REQ);
            AddPacketIdEntry(packetIds, S2C_RECYCLE_RESET_COUNT_RES);

// Group: 68
            AddPacketIdEntry(packetIds, S2C_68_0_1);
            AddPacketIdEntry(packetIds, S2C_68_1_1);

// Group: 69 - (PAWN)
            AddPacketIdEntry(packetIds, C2S_PAWN_SP_SKILL_GET_ACTIVE_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_SP_SKILL_GET_ACTIVE_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_SP_SKILL_GET_STOCK_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_SP_SKILL_GET_STOCK_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_SP_SKILL_GET_MAX_SKILL_NUM_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_SP_SKILL_GET_MAX_SKILL_NUM_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_SP_SKILL_SET_ACTIVE_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_SP_SKILL_SET_ACTIVE_SKILL_RES);
            AddPacketIdEntry(packetIds, C2S_PAWN_SP_SKILL_DELETE_STOCK_SKILL_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_SP_SKILL_DELETE_STOCK_SKILL_RES);
            AddPacketIdEntry(packetIds, S2C_PAWN_69_5_16_NTC);
            AddPacketIdEntry(packetIds, S2C_PAWN_69_6_16_NTC);
            AddPacketIdEntry(packetIds, C2S_PAWN_SP_SKILL_USE_ITEM_REQ);
            AddPacketIdEntry(packetIds, S2C_PAWN_SP_SKILL_USE_ITEM_RES);

// Group: 70 - (ACTION)
            AddPacketIdEntry(packetIds, C2S_ACTION_SET_PLAYER_ACTION_HISTORY_REQ);
            AddPacketIdEntry(packetIds, S2C_ACTION_SET_PLAYER_ACTION_HISTORY_RES);

// Group: 71 - (BATTLE)
            AddPacketIdEntry(packetIds, C2S_BATTLE_71_0_1_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_0_2_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_REWARD_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_REWARD_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_GET_REWARD_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_GET_REWARD_RES);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_2_16_NTC);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_CONTENT_ENTRY_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_CONTENT_ENTRY_RES);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_3_16_NTC);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_CONTENT_FIRST_FHASE_CHANGE_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_CONTENT_FIRST_FHASE_CHANGE_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_CONTENT_RESET_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_CONTENT_RESET_RES);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_5_16_NTC);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_GET_CONTENT_STATUS_FROM_OM_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_GET_CONTENT_STATUS_FROM_OM_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_GET_PHASE_TO_CHANGE_FROM_OM_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_GET_PHASE_TO_CHANGE_FROM_OM_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_PHASE_ENTRY_BEGIN_RECRUITMENT_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_PHASE_ENTRY_BEGIN_RECRUITMENT_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_PHASE_ENTRY_GET_RECRUITMENT_STATE_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_PHASE_ENTRY_GET_RECRUITMENT_STATE_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_PHASE_ENTRY_READY_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_PHASE_ENTRY_READY_RES);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_10_16_NTC);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_PHASE_ENTRY_READY_CANCEL_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_PHASE_ENTRY_READY_CANCEL_RES);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_12_16_NTC);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_13_16_NTC);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_14_16_NTC);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_15_16_NTC);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_RESET_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_RESET_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_INSTANT_CLEAR_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_INSTANT_CLEAR_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_CHARACTER_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_CHARACTER_INFO_RES);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_19_16_NTC);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_PARTY_MEMBER_INFO_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_RES);
            AddPacketIdEntry(packetIds, S2C_BATTLE_71_21_16_NTC);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_INFO_LIST_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_INFO_LIST_RES);
            AddPacketIdEntry(packetIds, C2S_BATTLE_CONTENT_INSTANT_COMPLETE_REQ);
            AddPacketIdEntry(packetIds, S2C_BATTLE_CONTENT_INSTANT_COMPLETE_RES);

// Group: 72 - (DAILY)
            AddPacketIdEntry(packetIds, C2S_DAILY_MISSION_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_DAILY_MISSION_LIST_GET_RES);
            AddPacketIdEntry(packetIds, S2C_DAILY_72_1_16_NTC);
            AddPacketIdEntry(packetIds, C2S_DAILY_MISSION_REWARD_RECEIVE_REQ);
            AddPacketIdEntry(packetIds, S2C_DAILY_MISSION_REWARD_RECEIVE_RES);
            AddPacketIdEntry(packetIds, C2S_DAILY_MISSION_EVENT_LIST_GET_REQ);
            AddPacketIdEntry(packetIds, S2C_DAILY_MISSION_EVENT_LIST_GET_RES);

            return packetIds;
        }

        #endregion
        
        // initialize at the very end
        private static readonly Dictionary<int, PacketId> LoginPacketIds = InitializeLoginPacketIds();
        private static readonly Dictionary<int, PacketId> GamePacketIds = InitializeGamePacketIds();
    }
}
