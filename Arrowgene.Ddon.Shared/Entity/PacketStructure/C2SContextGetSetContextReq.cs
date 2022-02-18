using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SContextGetSetContextReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONTEXT_GET_SET_CONTEXT_REQ;

        public C2SContextGetSetContextReq()
        {
            ContextId=0;
            UniqueId=0;
            StageNo=0;
            EncountArea=0;
            MasterIndex=0;
            Unk0=0;
        }

        public uint ContextId { get; set; } // "Id" in the client code, renamed to ContextId to avoid collision with PacketId
        public ulong UniqueId { get; set; }
        public int StageNo { get; set; }
        public int EncountArea { get; set; }
        public int MasterIndex { get; set; }
        public uint Unk0 { get; set; }

        public class Serializer : EntitySerializer<C2SContextGetSetContextReq>
        {            
            static Serializer()
            {
                Id = PacketId.C2S_CONTEXT_GET_SET_CONTEXT_REQ;
            }
            
            public override void Write(IBuffer buffer, C2SContextGetSetContextReq obj)
            {
                WriteUInt32(buffer, obj.ContextId);
                WriteUInt64(buffer, obj.UniqueId);
                WriteInt32(buffer, obj.StageNo);
                WriteInt32(buffer, obj.EncountArea);
                WriteInt32(buffer, obj.MasterIndex);
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SContextGetSetContextReq Read(IBuffer buffer)
            {
                C2SContextGetSetContextReq obj = new C2SContextGetSetContextReq();
                obj.ContextId = ReadUInt32(buffer);
                obj.UniqueId = ReadUInt64(buffer);
                obj.StageNo = ReadInt32(buffer);
                obj.EncountArea = ReadInt32(buffer);
                obj.MasterIndex = ReadInt32(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
