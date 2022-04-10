using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInnStayInnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INN_STAY_INN_REQ;

        public uint Data0 { get; set; }
        public uint Point { get; set; }
        public uint HpMax { get; set; }
        public uint Data1 { get; set; }

        public C2SInnStayInnReq()
        {
            Data0 = 0;
            Point = 0;
            HpMax = 0;
            Data1 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SInnStayInnReq>
        {
            public override void Write(IBuffer buffer, C2SInnStayInnReq obj)
            {
                WriteUInt32(buffer, obj.Data0);
                WriteUInt32(buffer, obj.Point);
                WriteUInt32(buffer, obj.HpMax);
                WriteUInt32(buffer, obj.Data1);
            }

            public override C2SInnStayInnReq Read(IBuffer buffer)
            {
                C2SInnStayInnReq obj = new C2SInnStayInnReq();
                obj.Data0 = ReadUInt32(buffer);
                obj.Point = ReadUInt32(buffer);
                obj.HpMax = ReadUInt32(buffer);
                obj.Data1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
