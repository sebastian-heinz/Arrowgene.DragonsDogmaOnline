using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanPointAddNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_POINT_ADD_NTC;

        public uint ClanPoint;
        public uint TotalClanPoint;
        public uint MoneyClanPoint;

        public class Serializer : PacketEntitySerializer<S2CClanClanPointAddNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanPointAddNtc obj)
            {
                WriteUInt32(buffer, obj.ClanPoint);
                WriteUInt32(buffer, obj.TotalClanPoint);
                WriteUInt32(buffer, obj.MoneyClanPoint);

            }

            public override S2CClanClanPointAddNtc Read(IBuffer buffer)
            {
                S2CClanClanPointAddNtc obj = new S2CClanClanPointAddNtc();

                obj.ClanPoint = ReadUInt32(buffer);                
                obj.TotalClanPoint = ReadUInt32(buffer);
                obj.MoneyClanPoint = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
