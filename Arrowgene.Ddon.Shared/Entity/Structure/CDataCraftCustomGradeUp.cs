using System.Linq;
using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftCustomGradeUp
    {

        public string GradeUpItemUID { get; set; }
        public uint GradeUpItemID { get; set; }
        public List<CDataCommonU32> GradeUpItemIDList { get; set; }
        public uint AddEquipPoint { get; set; }
        public uint TotalEquipPoint { get; set; }
        public uint EquipGrade { get; set; }
        public uint Gold { get; set; }
        public bool IsGreatSuccess { get; set; }
        public CDataCurrentEquipInfo CurrentEquip { get; set; }
        public uint BeforeItemID { get; set; }
        public bool Unk0 { get; set; }
        public CDataCraftStartEquipGradeUpUnk0 Unk1 { get; set; }


        public CDataCraftCustomGradeUp()
        {
            GradeUpItemUID = "B0B78A5C";
            GradeUpItemID = 1674;
            GradeUpItemIDList = new List<CDataCommonU32>();
            AddEquipPoint = 100;
            TotalEquipPoint = 250;
            EquipGrade = 2;
            Gold = 400;
            IsGreatSuccess = true;
            CurrentEquip = new CDataCurrentEquipInfo();
            BeforeItemID = 62;
            Unk0 = true;
            Unk1 = new CDataCraftStartEquipGradeUpUnk0();
        }

        public class Serializer : EntitySerializer<CDataCraftCustomGradeUp>
        {
            public override void Write(IBuffer buffer, CDataCraftCustomGradeUp obj)
            {
                WriteMtString(buffer, obj.GradeUpItemUID);
                WriteUInt32(buffer, obj.GradeUpItemID);
                WriteEntityList<CDataCommonU32>(buffer, obj.GradeUpItemIDList);
                WriteUInt32(buffer, obj.AddEquipPoint);
                WriteUInt32(buffer, obj.TotalEquipPoint);
                WriteUInt32(buffer, obj.EquipGrade);
                WriteUInt32(buffer, obj.Gold);
                WriteBool(buffer, obj.IsGreatSuccess);
                WriteEntity<CDataCurrentEquipInfo>(buffer, obj.CurrentEquip);
                WriteUInt32(buffer, obj.BeforeItemID);
                WriteBool(buffer, obj.Unk0);
                WriteEntity<CDataCraftStartEquipGradeUpUnk0>(buffer, obj.Unk1);
            }

            public override CDataCraftCustomGradeUp Read(IBuffer buffer)
            {
                CDataCraftCustomGradeUp obj = new CDataCraftCustomGradeUp();
                obj.GradeUpItemUID = ReadMtString(buffer);
                obj.GradeUpItemID = ReadUInt32(buffer);
                obj.GradeUpItemIDList = ReadEntityList<CDataCommonU32>(buffer);
                obj.AddEquipPoint = ReadUInt32(buffer);
                obj.TotalEquipPoint = ReadUInt32(buffer);
                obj.EquipGrade = ReadUInt32(buffer);
                obj.Gold = ReadUInt32(buffer);
                obj.IsGreatSuccess = ReadBool(buffer);
                obj.CurrentEquip = ReadEntity<CDataCurrentEquipInfo>(buffer);
                obj.BeforeItemID = ReadUInt32(buffer);
                obj.Unk0 = ReadBool(buffer);
                obj.Unk1 = ReadEntity<CDataCraftStartEquipGradeUpUnk0>(buffer);
                return obj;
                
            }
        }
    }
}
