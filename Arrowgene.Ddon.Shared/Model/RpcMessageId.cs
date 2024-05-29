using System;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum RpcMsgIdControl : UInt16
    {
        NET_MSG_ID_NOTHING = 0x0,
        NET_MSG_ID_ACT_BASE = 0x1,
        NET_MSG_ID_ACT_NORMAL = 0x2,
        NET_MSG_ID_ACT_NORMAL_EX = 0x3,
        NET_MSG_ID_ACT_RESET = 0x4,
        NET_MSG_ID_ACT_RESET_EX = 0x5,
        NET_MSG_ID_ACT_ACTION_ONLY = 0x6,
        NET_MSG_ID_ACT_ACTION_ONLY_EX = 0x7,
        NET_MSG_ID_ACT_SHL_SHOT = 0x8,
        NET_MSG_ID_ACT_TARGET = 0x9,
        NET_MSG_ID_ACT_DIE = 0xA,
        NET_MSG_ID_ACT_ENEMY_CLIMB = 0xB,
        NET_MSG_ID_ACT_CLIFF_HANG = 0xC,
        NET_MSG_ID_ACT_BOW = 0xD,
        NET_MSG_ID_ACT_REVIVE_CMC = 0xE,
        NET_MSG_ID_ACT_WAND_MAGIC_SHL_SET = 0xF,
        NET_MSG_ID_ACT_MAGIC_ITEM = 0x10,
        NET_MSG_ID_ACT_THROW_ITEM = 0x11,
        NET_MSG_ID_ACT_LIFT_BIGIN_ITEM = 0x12,
        NET_MSG_ID_ACT_RESPAWN = 0x13,
        NET_MSG_ID_ACT_WALL_CLIMB = 0x14,
        NET_MSG_ID_PERIODIC_TOP = 0x15,
        NET_MSG_ID_PERIODIC_NORMAL = 0x15,
        NET_MSG_ID_PERIODIC_ANGLE_Y = 0x16,
        NET_MSG_ID_PERIODIC_POS = 0x17,
        NET_MSG_ID_PERIODIC_CONDITION = 0x18,
        NET_MSG_ID_PERIODIC_TARGET = 0x19,
        NET_MSG_ID_PERIODIC_NOTHING = 0x1A,
        NET_MSG_ID_PERIODIC_CATCH = 0x1B,
        NET_MSG_ID_PERIODIC_ENEMY_CLIMB = 0x1C,
        NET_MSG_ID_PERIODIC_END = 0x1C,
        NET_MSG_ID_PERIODIC_INTERFACE = 0x1D,
        NET_MSG_ID_PERIODIC_BOTTOM = 0x1E,
        NET_MSG_ID_CATCH_REQUEST = 0x1E,
        NET_MSG_ID_CAUGHT_RESULT = 0x1F, //=> cContextInterface_isCaughtResult
        NET_MSG_ID_ACT_CAUGHT = 0x20,
        NET_MSG_ID_OM_PUT = 0x21,     //=> cContextInterface_getIsOmReleasePut
        NET_MSG_ID_OM_THROW = 0x22,   //=> cContextInterface_getIsOmReleaseThrow
        NET_MSG_ID_SHL_DELETE = 0x23, //=> cContextInterface_getShlDelte
        NET_MSG_ID_SHL_SHOT = 0x24,   //=> cContextInterface_isShotReqNoAct
        NET_MSG_ID_STICK_SHL = 0x25,  //=>cContextInterface_requestShlStickInfoContext
        NET_MSG_SHL_SLAVE_KILL_SEND = 0x26,
        NET_MSG_SHL_KILL_SYNC = 0x27,
        NET_MSG_STATE_LIVE = 0x28,
        NET_MSG_ID_EM5800 = 0x29,
        NET_MSG_ID_TARGET = 0x2A,
        NET_MSG_ID_SLAVE_DAMAGE = 0x2B,
        NET_MSG_ID_ACT_RESCUE = 0x2C,
        NET_MSG_ID_ACT_RESCUE_ONLY = 0x2D,
        NET_MSG_ID_ENEMYSTATUS_CTRL = 0x2E, //=> cContextInterface_setEnemyStatusChange
        NET_MSG_ID_ENEMYWAITTING = 0x2F,
        NET_MSG_ID_ENEMYSTARTWAIT = 0x30,
        NET_MSG_ID_CORE_POINT = 0x31,
        NET_MSG_ID_CORE_POINT_SLAVE = 0x32,
        NET_MSG_ID_OCD_HOLY_ABSORP = 0x33,
        NET_MSG_ID_ACT_DAMAGE = 0x34,
        NET_MSG_ID_MASTER_PARAM = 0x35,
        NET_MSG_ID_CUSTOM_SYNC = 0x36,
        NET_MSG_ID_SERVER_DAMAGE = 0x37,
        NET_MSG_ID_ACT_CATCH = 0x38,
        NET_MSG_ID_CS_CHANGE = 0x39,
        NET_MSG_ID_SOUL_ABSORP = 0x3A,
        NET_MSG_ID_SHL_REQUEST_FROM_SLAVE = 0x3B,
        NET_MSG_ID_ACT_LOBBY_OFF = 0x80,
        NET_MSG_ID_DEFAULT = 0xFF,
        NET_MSG_ID_PERIODIC_DEFAULT = 0xFF,
    }

    public enum RpcMsgIdSetNormal : UInt16
    {
        NET_MSG_ID_SET_NOTHING = 0x0,
        NET_MSG_ID_SET_BASE = 0x1,
        NET_MSG_ID_SET_REQUEST_MASTER = 0x2,
        NET_MSG_ID_SET_CHANGE_MASTER = 0x3,
        NET_MSG_ID_SET_RELEASE_MASTER = 0x4,
        NET_MSG_ID_SET_MASTER_INFO = 0x5,
        NET_MSG_ID_SET_THROW_MASTER = 0x6,
        NET_MSG_ID_SET_REQUEST_CREATE_CONTEXT = 0x7,
        NET_MSG_ID_SET_CREATE_CONTEXT = 0x8,
    }

    public enum RpcMsgIdGameNormal : UInt16
    {
        NET_MSG_ID_GAME_NOTHING = 0x0,
        NET_MSG_ID_GAME_STAGE = 0x1,
        NET_MSG_ID_GAME_REVIVE_STOCK = 0x2,
        NET_MSG_ID_GAME_PERIOD = 0x3,
        NET_MSG_ID_GAME_FLAG = 0x4,
        NET_MSG_ID_GAME_OM = 0x5,
        NET_MSG_ID_GAME_PAWN_ENTRY_PARTY = 0x6,
        NET_MSG_ID_GAME_PAWN_MSG = 0x7,
        NET_MSG_ID_GAME_ENTRY_PARTY = 0x8,
        NET_MSG_ID_GAME_DROP_ITEM = 0x9,
        NET_MSG_ID_GAME_SET_EM_DIE = 0xA,
        NET_MSG_ID_GAME_OPEN_DOOR = 0xB,
        NET_MSG_ID_GAME_FREEMARKER = 0xC,
        NET_MSG_ID_GAME_LOST_RETURN_REQ = 0xD,
        NET_MSG_ID_GAME_PAWN_ORDER = 0xE,
    }

    public enum RpcMsgIdGameEasy : UInt16
    {
        NET_MSG_ID_GAME_EASY_NOTHING = 0xF,
        NET_MSG_ID_GAME_EASY_WEAPON_LOAD = 0x10,
        NET_MSG_ID_GAME_EASY_PRT_SET = 0x11,
        NET_MSG_ID_GAME_EASY_PRT_INFO = 0x12,
        NET_MSG_ID_GAME_EASY_AREA_RELEASE = 0x13,
        NET_MSG_ID_GAME_EASY_EVENT = 0x14,
        NET_MSG_ID_GAME_EASY_NPC_MESSAGE = 0x15,
        NET_MSG_ID_GAME_EASY_AREA_JUMP_SYNC = 0x16,
    }

    public enum RpcMsgIdItem : UInt16
    {
        NET_MSG_ID_ITEM_NOTHING = 0x0,
    }

    public enum RpcMsgIdToolNormal : UInt16
    {
        NET_MSG_ID_TOOL_NOTHING = 0x0,
        NET_MSG_ID_TOOL_BASE = 0x1,
        NET_MSG_ID_TOOL_PAWN_SET = 0x2,
    }

    public enum RpcMsgIdToolEasy : UInt16
    {
        NET_MSG_ID_TOOL_EASY_NOTHING = 0x3,
        NET_MSG_ID_TOOL_EASY_DIP = 0x4,
        NET_MSG_ID_TOOL_EASY_DAMAGE_PROFILE_START = 0x5,
        NET_MSG_ID_TOOL_EASY_DAMAGE_PROFILE_END = 0x6,
        NET_MSG_ID_TOOL_EASY_DAMAGE_PROFILE_NODE = 0x7,
        NET_MSG_ID_TOOL_EASY_TEST = 0x8,
    }
}
