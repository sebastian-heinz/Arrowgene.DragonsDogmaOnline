using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpLeaderWarpNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_WARP_LEADER_WARP_NTC;

        public uint CharacterId { get; set; }
        public uint DestPointId { get; set; }
        public uint RestSecond { get; set; }

        public class Serializer : PacketEntitySerializer<S2CWarpLeaderWarpNtc>
        {
            public override void Write(IBuffer buffer, S2CWarpLeaderWarpNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.DestPointId);
                WriteUInt32(buffer, obj.RestSecond);
            }

            public override S2CWarpLeaderWarpNtc Read(IBuffer buffer)
            {
                S2CWarpLeaderWarpNtc obj = new S2CWarpLeaderWarpNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.DestPointId = ReadUInt32(buffer);
                obj.RestSecond = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}