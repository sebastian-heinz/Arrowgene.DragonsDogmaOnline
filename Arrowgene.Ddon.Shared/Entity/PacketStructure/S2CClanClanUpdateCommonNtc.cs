using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanUpdateCommonNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_UPDATE_COMMON_NTC;

        public CDataClanNoticePackage Notice;

        public S2CClanClanUpdateCommonNtc()
        {
            Notice = new();
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanUpdateCommonNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanUpdateCommonNtc obj)
            {
                WriteEntity<CDataClanNoticePackage>(buffer, obj.Notice);
            }

            public override S2CClanClanUpdateCommonNtc Read(IBuffer buffer)
            {
                S2CClanClanUpdateCommonNtc obj = new S2CClanClanUpdateCommonNtc();

                obj.Notice = ReadEntity<CDataClanNoticePackage>(buffer);

                return obj;
            }
        }
    }
}
