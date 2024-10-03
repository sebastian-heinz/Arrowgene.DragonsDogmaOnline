using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageUnisonAreaChangeReadyCancelReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_UNISON_AREA_CHANGE_READY_CANCEL_REQ;

        public C2SStageUnisonAreaChangeReadyCancelReq()
        {
        }

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SStageUnisonAreaChangeReadyCancelReq>
        {
            public override void Write(IBuffer buffer, C2SStageUnisonAreaChangeReadyCancelReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SStageUnisonAreaChangeReadyCancelReq Read(IBuffer buffer)
            {
                C2SStageUnisonAreaChangeReadyCancelReq obj = new C2SStageUnisonAreaChangeReadyCancelReq();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
