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
        public byte EquipToCharacter { get; set; }

        /// <summary>
        /// The ID of the restriction, if any, that was provided in S2CInstanceGetGatheringItemListRes.
        /// Presumably so you can do the actual paying of the cost here.
        /// </summary>
        public uint RestrictionId { get; set; }
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
                WriteByte(buffer, obj.EquipToCharacter);
                WriteUInt32(buffer, obj.RestrictionId);
                WriteEntityList<CDataGatheringItemGetRequest>(buffer, obj.GatheringItemGetRequestList);
            }

            public override C2SInstanceGetGatheringItemReq Read(IBuffer buffer)
            {
                C2SInstanceGetGatheringItemReq obj = new C2SInstanceGetGatheringItemReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.EquipToCharacter = ReadByte(buffer);
                obj.RestrictionId = ReadUInt32(buffer);
                obj.GatheringItemGetRequestList = ReadEntityList<CDataGatheringItemGetRequest>(buffer);
                return obj;
            }
        }
    }
}
