using System;
using System.Collections.Generic;

namespace Arrowgene.Ddo.GameServer.Network
{
    public readonly struct PacketId
    {
        public static readonly PacketId UNKNOWN = new PacketId(0, 0, 0, "UNKNOWN");

        // public static readonly PacketId L2C_LOGIN_SERVER_CERT_NOTICE = new PacketId(0,0,0, "L2C_LOGIN_SERVER_CERT_NOTICE");
        public static readonly PacketId C2L_CLIENT_CHALLENGE_REQ = new PacketId(1, 0, 1, "C2L_CLIENT_CHALLENGE_REQ");
        public static readonly PacketId L2C_CLIENT_CHALLENGE_RES = new PacketId(1, 0, 2, "L2C_CLIENT_CHALLENGE_RES");
        public static readonly PacketId C2L_SESSION_KEY_REQ = new PacketId(0, 1, 1, "C2L_SESSION_KEY_REQ");
        public static readonly PacketId L2C_SESSION_KEY_RES = new PacketId(0, 1, 2, "L2C_SESSION_KEY_RES");
        public static readonly PacketId X1_REQ = new PacketId(0, 0, 1, "X1_REQ");
        public static readonly PacketId X1_RES = new PacketId(0, 0, 2, "X1_RES");
        public static readonly PacketId X2_REQ = new PacketId(3, 0, 1, "X2_REQ");
        public static readonly PacketId X2_RES = new PacketId(3, 0, 0x10, "X2_RES Character Data?");

        private static Dictionary<int, PacketId> InitializePacketIds()
        {
            Dictionary<int, PacketId> packetIds = new Dictionary<int, PacketId>();
            AddPacketIdEntry(packetIds, UNKNOWN);
            // AddPacketIdEntry(packetIds, L2C_LOGIN_SERVER_CERT_NOTICE);
            AddPacketIdEntry(packetIds, C2L_CLIENT_CHALLENGE_REQ);
            AddPacketIdEntry(packetIds, L2C_CLIENT_CHALLENGE_RES);
            AddPacketIdEntry(packetIds, C2L_SESSION_KEY_REQ);
            AddPacketIdEntry(packetIds, L2C_SESSION_KEY_RES);
            AddPacketIdEntry(packetIds, X1_REQ);
            AddPacketIdEntry(packetIds, X1_RES);
            AddPacketIdEntry(packetIds, X2_REQ);
            AddPacketIdEntry(packetIds, X2_RES);
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
