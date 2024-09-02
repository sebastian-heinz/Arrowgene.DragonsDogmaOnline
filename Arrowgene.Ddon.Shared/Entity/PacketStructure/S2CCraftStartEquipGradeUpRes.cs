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
        public uint GradeUpItemID { get; set; }
        public List<CDataCommonU32> GradeUpItemIDList { get; set; }
        public uint AddEquipPoint { get; set; }
        public uint TotalEquipPoint { get; set; }
        public uint EquipGrade { get; set; }
        public uint Gold { get; set; }
        public bool IsGreatSuccess { get; set; }
        public CDataCurrentEquipInfo CurrentEquip { get; set; }
        public uint BeforeItemID { get; set; }
        public bool Upgradable { get; set; }
        public CDataCraftStartEquipGradeUpUnk0 Unk1 { get; set; } // Dragon Augments?

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
                WriteUInt32(buffer, obj.GradeUpItemID);
                WriteEntityList<CDataCommonU32>(buffer, obj.GradeUpItemIDList);
                WriteUInt32(buffer, obj.AddEquipPoint);
                WriteUInt32(buffer, obj.TotalEquipPoint);
                WriteUInt32(buffer, obj.EquipGrade);
                WriteUInt32(buffer, obj.Gold);
                WriteBool(buffer, obj.IsGreatSuccess);
                WriteEntity<CDataCurrentEquipInfo>(buffer, obj.CurrentEquip);
                WriteUInt32(buffer, obj.BeforeItemID);
                WriteBool(buffer, obj.Upgradable);
                WriteEntity<CDataCraftStartEquipGradeUpUnk0>(buffer, obj.Unk1);
            }

            public override S2CCraftStartEquipGradeUpRes Read(IBuffer buffer)
            {
                S2CCraftStartEquipGradeUpRes obj = new S2CCraftStartEquipGradeUpRes();
                ReadServerResponse(buffer, obj);
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
                obj.Upgradable = ReadBool(buffer);
                obj.Unk1 = ReadEntity<CDataCraftStartEquipGradeUpUnk0>(buffer);
                return obj;
            }
        }
    }
}
