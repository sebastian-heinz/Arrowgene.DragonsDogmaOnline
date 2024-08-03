using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftStartEquipColorChangeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_START_EQUIP_COLOR_CHANGE_REQ;

        public C2SCraftStartEquipColorChangeReq()
        {
            EquipItemUID = string.Empty;
            CraftColorantList = new List<CDataCraftColorant>();
            CraftSupportPawnIDList = new List<CDataCraftSupportPawnID>();
        }

        public string EquipItemUID { get; set; }
        public byte Color { get; set; }
        public List<CDataCraftColorant> CraftColorantList { get; set; }
        public uint CraftMainPawnID { get; set; }
        public List<CDataCraftSupportPawnID> CraftSupportPawnIDList { get; set; }
        public class Serializer : PacketEntitySerializer<C2SCraftStartEquipColorChangeReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartEquipColorChangeReq obj)
            {
                WriteMtString(buffer, obj.EquipItemUID);
                WriteByte(buffer, obj.Color);
                WriteEntityList<CDataCraftColorant>(buffer, obj.CraftColorantList);
                WriteUInt32(buffer, obj.CraftMainPawnID);
                WriteEntityList<CDataCraftSupportPawnID>(buffer, obj.CraftSupportPawnIDList);
            }

            public override C2SCraftStartEquipColorChangeReq Read(IBuffer buffer)
            {
                C2SCraftStartEquipColorChangeReq obj = new C2SCraftStartEquipColorChangeReq();
                obj.EquipItemUID = ReadMtString(buffer);
                obj.Color = ReadByte(buffer);
                obj.CraftColorantList = ReadEntityList<CDataCraftColorant>(buffer);
                obj.CraftMainPawnID = ReadUInt32(buffer);
                obj.CraftSupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                return obj;
            }
        }
    }
}
