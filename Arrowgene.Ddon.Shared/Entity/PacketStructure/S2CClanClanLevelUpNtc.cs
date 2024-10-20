using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanLevelUpNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_LEVEL_UP_NTC;

        public uint ClanLevel;
        public uint NextClanPoint;

        public class Serializer : PacketEntitySerializer<S2CClanClanLevelUpNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanLevelUpNtc obj)
            {
                WriteUInt32(buffer, obj.ClanLevel);
                WriteUInt32(buffer, obj.NextClanPoint);
            }

            public override S2CClanClanLevelUpNtc Read(IBuffer buffer)
            {
                S2CClanClanLevelUpNtc obj = new S2CClanClanLevelUpNtc();

                obj.ClanLevel = ReadUInt32(buffer);
                obj.NextClanPoint = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
