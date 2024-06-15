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
            EquipItemUID = string.Empty;
            CraftElementList = new List<CDataCraftElement>();
            SupportPawnIDList = new List<CDataCraftSupportPawnID>();
        }

        public string EquipItemUID { get; set; }
        public List<CDataCraftElement> CraftElementList { get; set; }
        public uint CraftMainPawnID { get; set; }
        public List<CDataCraftSupportPawnID> SupportPawnIDList { get; set; }


        public class Serializer : PacketEntitySerializer<C2SCraftStartAttachElementReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartAttachElementReq obj)
            {
                WriteMtString(buffer, obj.EquipItemUID);
                WriteEntityList<CDataCraftElement>(buffer, obj.CraftElementList);
                WriteUInt32(buffer, obj.CraftMainPawnID);
                WriteEntity(buffer, obj.SupportPawnIDList);
            }

            public override C2SCraftStartAttachElementReq Read(IBuffer buffer)
            {
                C2SCraftStartAttachElementReq obj = new C2SCraftStartAttachElementReq();
                obj.EquipItemUID = ReadMtString(buffer);
                obj.CraftElementList = ReadEntityList<CDataCraftElement>(buffer);
                obj.CraftMainPawnID = ReadUInt32(buffer);
                obj.SupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                return obj;
            }
        }
    }
}