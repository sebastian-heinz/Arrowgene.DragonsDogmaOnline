using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstancePlTouchOmNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_PL_TOUCH_OM_NTC;

        public C2SInstancePlTouchOmNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public uint SubGroupId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInstancePlTouchOmNtc>
        {
            public override void Write(IBuffer buffer, C2SInstancePlTouchOmNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.SubGroupId);
            }

            public override C2SInstancePlTouchOmNtc Read(IBuffer buffer)
            {
                C2SInstancePlTouchOmNtc obj = new C2SInstancePlTouchOmNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SubGroupId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
