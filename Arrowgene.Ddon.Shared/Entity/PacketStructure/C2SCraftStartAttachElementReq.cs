using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftStartAttachElementReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_START_ATTACH_ELEMENT_REQ;

        public C2SCraftStartAttachElementReq()
        {
            EquipItemUId = string.Empty;
            CraftElementList = new List<CDataCraftElement>();
            CraftSupportPawnIDList = new List<CDataCraftSupportPawnID>();

        }

        public string EquipItemUId { get; set; }
        public List<CDataCraftElement> CraftElementList {  get; set; }
        public uint CraftMainPawnId { get; set; }
        public List <CDataCraftSupportPawnID> CraftSupportPawnIDList {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftStartAttachElementReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartAttachElementReq obj)
            {
                WriteMtString(buffer, obj.EquipItemUId);
                WriteEntityList(buffer, obj.CraftElementList);
                WriteUInt32(buffer, obj.CraftMainPawnId);
                WriteEntityList(buffer, obj.CraftSupportPawnIDList);
            }

            public override C2SCraftStartAttachElementReq Read(IBuffer buffer)
            {
                C2SCraftStartAttachElementReq obj = new C2SCraftStartAttachElementReq();
                obj.EquipItemUId = ReadMtString(buffer);
                obj.CraftElementList = ReadEntityList<CDataCraftElement>(buffer);
                obj.CraftMainPawnId = ReadUInt32(buffer);
                obj.CraftSupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                return obj;
            }
        }
    }
}
