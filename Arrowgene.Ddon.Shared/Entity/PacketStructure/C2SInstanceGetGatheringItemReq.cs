using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetGatheringItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_GATHERING_ITEM_REQ;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public List<CDataGatheringItemGetRequest> GatheringItemGetRequestList { get; set; }

        public C2SInstanceGetGatheringItemReq()
        {
            LayoutId = new CDataStageLayoutId();
            GatheringItemGetRequestList = new List<CDataGatheringItemGetRequest>();
        }

        public class Serializer : PacketEntitySerializer<C2SInstanceGetGatheringItemReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceGetGatheringItemReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList<CDataGatheringItemGetRequest>(buffer, obj.GatheringItemGetRequestList);
            }

            public override C2SInstanceGetGatheringItemReq Read(IBuffer buffer)
            {
                C2SInstanceGetGatheringItemReq obj = new C2SInstanceGetGatheringItemReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.GatheringItemGetRequestList = ReadEntityList<CDataGatheringItemGetRequest>(buffer);
                return obj;
            }
        }
    }
}
