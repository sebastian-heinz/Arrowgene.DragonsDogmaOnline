using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Server.Network
{
    public readonly struct PacketId
    {
        public static readonly PacketId UNKNOWN = new PacketId(0, 0, 0, "UNKNOWN");

        // public static readonly PacketId L2C_LOGIN_SERVER_CERT_NOTICE = new PacketId(0,0,0, "L2C_LOGIN_SERVER_CERT_NOTICE");
        public static readonly PacketId C2L_CLIENT_CHALLENGE_REQ = new PacketId(1, 0, 1, "C2L_CLIENT_CHALLENGE_REQ");
        public static readonly PacketId L2C_CLIENT_CHALLENGE_RES = new PacketId(1, 0, 2, "L2C_CLIENT_CHALLENGE_RES");
        public static readonly PacketId C2L_GET_GAME_SESSION_KEY_REQ = new PacketId(0, 1, 1, "C2L_GET_GAME_SESSION_KEY_REQ");
        public static readonly PacketId L2C_GET_GAME_SESSION_KEY_RES = new PacketId(0, 1, 2, "L2C_GET_GAME_SESSION_KEY_RES");
        public static readonly PacketId X1_REQ = new PacketId(0, 0, 1, "X1_REQ");
        public static readonly PacketId X1_RES = new PacketId(0, 0, 2, "X1_RES");
        public static readonly PacketId C2L_GET_ERROR_MESSAGE_LIST_REQ = new PacketId(3, 0, 1, "C2L_GET_ERROR_MESSAGE_LIST_REQ");
        public static readonly PacketId L2C_GET_ERROR_MESSAGE_LIST_RES = new PacketId(3, 0, 2, "L2C_GET_ERROR_MESSAGE_LIST_RES");
        public static readonly PacketId L2C_GET_ERROR_MESSAGE_LIST_NTC = new PacketId(3, 0, 0x10, "L2C_GET_ERROR_MESSAGE_LIST_NTC");
        public static readonly PacketId C2L_GET_LOGIN_SETTING_REQ = new PacketId(2, 2, 1, "C2L_GET_LOGIN_SETTING_REQ");
        public static readonly PacketId L2C_GET_LOGIN_SETTING_RES = new PacketId(2, 2, 2, "L2C_GET_LOGIN_SETTING_RES");
        public static readonly PacketId C2L_GP_COURSE_GET_INFO_REQ = new PacketId(4, 0, 1, "C2L_GP_COURSE_GET_INFO_REQ");
        public static readonly PacketId L2C_GP_COURSE_GET_INFO_RES = new PacketId(4, 0, 2, "L2C_GP_COURSE_GET_INFO_RES");
        public static readonly PacketId C2L_GET_CHARACTER_LIST_REQ = new PacketId(5, 0, 1, "C2L_GET_CHARACTER_LIST_REQ");
        public static readonly PacketId L2C_GET_CHARACTER_LIST_RES = new PacketId(5, 0, 2, "L2C_GET_CHARACTER_LIST_RES");
        
        public static readonly PacketId X60 = new PacketId(5, 1, 1, "X60");
        public static readonly PacketId X61 = new PacketId(5, 1, 2, "X61");
        public static readonly PacketId X62 = new PacketId(5, 5, 16, "X62");
        
        public static readonly PacketId X8 = new PacketId(2, 3, 16, "X8");
        
        public static readonly PacketId X9_REQ = new PacketId(2, 1, 1, "X9_REQ");
        public static readonly PacketId X9_RES = new PacketId(2, 1, 2, "X9_RES");
        
        public static readonly PacketId X10_REQ = new PacketId(0, 2, 1, "X10_REQ");
        public static readonly PacketId X10_RES = new PacketId(0, 2, 2, "X10_RES");
        private static Dictionary<int, PacketId> InitializePacketIds()
        {
            Dictionary<int, PacketId> packetIds = new Dictionary<int, PacketId>();
            AddPacketIdEntry(packetIds, UNKNOWN);
            // AddPacketIdEntry(packetIds, L2C_LOGIN_SERVER_CERT_NOTICE);
            AddPacketIdEntry(packetIds, C2L_CLIENT_CHALLENGE_REQ);
            AddPacketIdEntry(packetIds, L2C_CLIENT_CHALLENGE_RES);
            AddPacketIdEntry(packetIds, C2L_GET_GAME_SESSION_KEY_REQ);
            AddPacketIdEntry(packetIds, L2C_GET_GAME_SESSION_KEY_RES);
            AddPacketIdEntry(packetIds, X1_REQ);
            AddPacketIdEntry(packetIds, X1_RES);
            AddPacketIdEntry(packetIds, C2L_GET_ERROR_MESSAGE_LIST_REQ);
            AddPacketIdEntry(packetIds, L2C_GET_ERROR_MESSAGE_LIST_RES);
            AddPacketIdEntry(packetIds, L2C_GET_ERROR_MESSAGE_LIST_NTC);
            AddPacketIdEntry(packetIds, C2L_GET_LOGIN_SETTING_REQ);
            AddPacketIdEntry(packetIds, L2C_GET_LOGIN_SETTING_RES);
            AddPacketIdEntry(packetIds, C2L_GP_COURSE_GET_INFO_REQ);
            AddPacketIdEntry(packetIds, L2C_GP_COURSE_GET_INFO_RES);
            AddPacketIdEntry(packetIds, C2L_GET_CHARACTER_LIST_REQ);
            AddPacketIdEntry(packetIds, L2C_GET_CHARACTER_LIST_RES);
            
            
            AddPacketIdEntry(packetIds, X60);
            AddPacketIdEntry(packetIds, X61);
            AddPacketIdEntry(packetIds, X62);
            AddPacketIdEntry(packetIds, X8);
            AddPacketIdEntry(packetIds, X9_REQ);
            AddPacketIdEntry(packetIds, X9_RES);
            AddPacketIdEntry(packetIds, X10_REQ);
            AddPacketIdEntry(packetIds, X10_RES);

            return packetIds;
        }

        private static readonly Dictionary<int, PacketId> PacketIds = InitializePacketIds();

        private static void AddPacketIdEntry(Dictionary<int, PacketId> packetIds, PacketId packetId)
        {
            packetIds.Add(packetId.GetHashCode(), packetId);
        }

        private static int GetHashCode(byte groupId, ushort handlerId, byte handlerSubId)
        {
            return HashCode.Combine(groupId, handlerId, handlerSubId);
        }

        public static PacketId Get(byte groupId, ushort handlerId, byte handlerSubId)
        {
            int hashCode = GetHashCode(groupId, handlerId, handlerSubId);
            if (PacketIds.ContainsKey(hashCode))
            {
                return PacketIds[hashCode];
            }

            return new PacketId(groupId, handlerId, handlerSubId, "Unknown");
        }

        public readonly byte GroupId;
        public readonly ushort HandlerId;
        public readonly byte HandlerSubId;
        public readonly string Name;

        public PacketId(byte groupId, ushort handlerId, byte handlerSubId, string name)
        {
            GroupId = groupId;
            HandlerId = handlerId;
            HandlerSubId = handlerSubId;
            Name = name;
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
    }
}
