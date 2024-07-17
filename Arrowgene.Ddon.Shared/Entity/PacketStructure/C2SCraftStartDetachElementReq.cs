using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftStartDetachElementReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_START_DETACH_ELEMENT_REQ;

        public C2SCraftStartDetachElementReq()
        {
            EquipItemUId = string.Empty;
            CraftElementList = new List<CDataCraftElement>();
            CraftSupportPawnIDList = new List<CDataCraftSupportPawnID>();
        }

        public string EquipItemUId { get; set; }
        public List<CDataCraftElement> CraftElementList { get; set; }
        public uint CraftMainPawnId { get; set; }
        public List<CDataCraftSupportPawnID> CraftSupportPawnIDList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftStartDetachElementReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartDetachElementReq obj)
            {
                WriteMtString(buffer, obj.EquipItemUId);
                WriteEntityList(buffer, obj.CraftElementList);
                WriteUInt32(buffer, obj.CraftMainPawnId);
                WriteEntityList(buffer, obj.CraftSupportPawnIDList);
            }

            public override C2SCraftStartDetachElementReq Read(IBuffer buffer)
            {
                C2SCraftStartDetachElementReq obj = new C2SCraftStartDetachElementReq();
                obj.EquipItemUId = ReadMtString(buffer);
                obj.CraftElementList = ReadEntityList<CDataCraftElement>(buffer);
                obj.CraftMainPawnId = ReadUInt32(buffer);
                obj.CraftSupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                return obj;
            }
        }
    }
}
