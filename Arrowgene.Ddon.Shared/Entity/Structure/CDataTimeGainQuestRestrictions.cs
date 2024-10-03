using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTimeGainQuestRestrictions
    {
        public CDataTimeGainQuestRestrictions()
        {
            Unk1List = new List<CDataCommonU32>();
            Unk2List = new List<CDataTimeGainQuestUnk1Unk2>();
            Unk5List = new List<CDataCommonU8>();
        }

        public uint Unk0 { get; set; }
        public List<CDataCommonU32> Unk1List {  get; set; }
        public List<CDataTimeGainQuestUnk1Unk2> Unk2List { get; set; }
        public bool RestrictArmor { get; set; }
        public bool RestrictJewlery { get; set; }
        public List<CDataCommonU8> Unk5List {  get; set; }

        public class Serializer : EntitySerializer<CDataTimeGainQuestRestrictions>
        {
            public override void Write(IBuffer buffer, CDataTimeGainQuestRestrictions obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1List);
                WriteEntityList(buffer, obj.Unk2List);
                WriteBool(buffer, obj.RestrictArmor);
                WriteBool(buffer, obj.RestrictJewlery);
                WriteEntityList(buffer, obj.Unk5List);
            }

            public override CDataTimeGainQuestRestrictions Read(IBuffer buffer)
            {
                CDataTimeGainQuestRestrictions obj = new CDataTimeGainQuestRestrictions();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1List = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk2List = ReadEntityList<CDataTimeGainQuestUnk1Unk2>(buffer);
                obj.RestrictArmor = ReadBool(buffer);
                obj.RestrictJewlery = ReadBool(buffer);
                obj.Unk5List = ReadEntityList<CDataCommonU8>(buffer);
                return obj;
            }
        }
    }
}
