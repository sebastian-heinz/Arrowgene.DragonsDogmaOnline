using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftStartEquipGradeUpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_START_EQUIP_GRADE_UP_REQ;

        public C2SCraftStartEquipGradeUpReq()
        {
            EquipItemUID = string.Empty;
            CraftMaterialList = new List<CDataCraftMaterial>();
            CraftSupportPawnIDList = new List<CDataCraftSupportPawnID>();
            Unk0 = new List<CDataCommonU32>();
        }

        public string EquipItemUID { get; set; }
        public List<CDataCraftMaterial> CraftMaterialList { get; set; }
        public uint CraftMainPawnID { get; set; }
        public List<CDataCraftSupportPawnID> CraftSupportPawnIDList { get; set; }
        public List<CDataCommonU32> Unk0 { get; set; }
        

        public class Serializer : PacketEntitySerializer<C2SCraftStartEquipGradeUpReq>
        {
            public override void Write(IBuffer buffer, C2SCraftStartEquipGradeUpReq obj)
            {
                WriteMtString(buffer, obj.EquipItemUID);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.CraftMaterialList);
                WriteUInt32(buffer, obj.CraftMainPawnID);
                WriteEntityList<CDataCraftSupportPawnID>(buffer, obj.CraftSupportPawnIDList);
                WriteEntityList<CDataCommonU32>(buffer, obj.Unk0);
            }

            public override C2SCraftStartEquipGradeUpReq Read(IBuffer buffer)
            {
                C2SCraftStartEquipGradeUpReq obj = new C2SCraftStartEquipGradeUpReq();
                obj.EquipItemUID = ReadMtString(buffer);
                obj.CraftMaterialList = ReadEntityList<CDataCraftMaterial>(buffer);
                obj.CraftMainPawnID = ReadUInt32(buffer);
                obj.CraftSupportPawnIDList = ReadEntityList<CDataCraftSupportPawnID>(buffer);
                obj.Unk0 = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
