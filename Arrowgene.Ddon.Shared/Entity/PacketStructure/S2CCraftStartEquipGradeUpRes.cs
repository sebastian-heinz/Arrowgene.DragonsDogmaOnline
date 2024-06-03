using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftStartEquipGradeUpRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_START_EQUIP_GRADE_UP_RES;

        public string GradeUpItemUID { get; set; }
        public int GradeUpItemID { get; set; }
        public List<CDataCommonU32> GradeUpItemIDList { get; set; }
        public int AddEquipPoint { get; set; }
        public int TotalEquipPoint { get; set; }
        public int EquipGrade { get; set; }
        public int Gold { get; set; }
        public bool IsGreatSuccess { get; set; }
        public CDataCurrentEquipInfo CurrentEquip { get; set; }
        public int BeforeItemID { get; set; }
        public bool Unk0 { get; set; }
        public CDataCraftStartEquipGradeUpUnk0 Unk1 { get; set; }

        public S2CCraftStartEquipGradeUpRes()
        {
            GradeUpItemUID = string.Empty;
            GradeUpItemIDList = new List<CDataCommonU32>();
            CurrentEquip = new CDataCurrentEquipInfo();
            Unk1 = new CDataCraftStartEquipGradeUpUnk0();
        }

        public class Serializer : PacketEntitySerializer<S2CCraftStartEquipGradeUpRes>
        {
            public override void Write(IBuffer buffer, S2CCraftStartEquipGradeUpRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteMtString(buffer, obj.GradeUpItemUID);
                WriteInt32(buffer, obj.GradeUpItemID);
                WriteEntityList<CDataCommonU32>(buffer, obj.GradeUpItemIDList);
                WriteInt32(buffer, obj.AddEquipPoint);
                WriteInt32(buffer, obj.TotalEquipPoint);
                WriteInt32(buffer, obj.EquipGrade);
                WriteInt32(buffer, obj.Gold);
                WriteBool(buffer, obj.IsGreatSuccess);
                WriteEntity<CDataCurrentEquipInfo>(buffer, obj.CurrentEquip);
                WriteInt32(buffer, obj.BeforeItemID);
                WriteBool(buffer, obj.Unk0);
                WriteEntity<CDataCraftStartEquipGradeUpUnk0>(buffer, obj.Unk1);
            }

            public override S2CCraftStartEquipGradeUpRes Read(IBuffer buffer)
            {
                S2CCraftStartEquipGradeUpRes obj = new S2CCraftStartEquipGradeUpRes();
                ReadServerResponse(buffer, obj);
                obj.GradeUpItemUID = ReadMtString(buffer);
                obj.GradeUpItemID = ReadInt32(buffer);
                obj.GradeUpItemIDList = ReadEntityList<CDataCommonU32>(buffer);
                obj.AddEquipPoint = ReadInt32(buffer);
                obj.TotalEquipPoint = ReadInt32(buffer);
                obj.EquipGrade = ReadInt32(buffer);
                obj.Gold = ReadInt32(buffer);
                obj.IsGreatSuccess = ReadBool(buffer);
                obj.CurrentEquip = ReadEntity<CDataCurrentEquipInfo>(buffer);
                obj.BeforeItemID = ReadInt32(buffer);
                obj.Unk0 = ReadBool(buffer);
                obj.Unk1 = ReadEntity<CDataCraftStartEquipGradeUpUnk0>(buffer);
                return obj;
            }
        }
    }
}
