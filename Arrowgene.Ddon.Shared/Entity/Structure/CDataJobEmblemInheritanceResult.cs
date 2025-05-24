using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemInheritanceResult
    {
        public CDataJobEmblemInheritanceResult()
        {
            EquipElementParamList = new();
            Unk1List = new();
            Unk2List = new();
        }

        public JobId JobId { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        public uint Unk0 { get; set; }
        public List<CDataCommonU32> Unk1List { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsItemLost { get; set; } // Controls GG prompt
        public List<CDataJobEmblemUnk0> Unk2List { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemInheritanceResult>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemInheritanceResult obj)
            {
                WriteByte(buffer, (byte)obj.JobId);
                WriteEntityList(buffer, obj.EquipElementParamList);
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1List);
                WriteBool(buffer, obj.IsSuccess);
                WriteBool(buffer, obj.IsItemLost);
                WriteEntityList(buffer, obj.Unk2List);
            }

            public override CDataJobEmblemInheritanceResult Read(IBuffer buffer)
            {
                CDataJobEmblemInheritanceResult obj = new CDataJobEmblemInheritanceResult();
                obj.JobId = (JobId)ReadByte(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1List = ReadEntityList<CDataCommonU32>(buffer);
                obj.IsSuccess = ReadBool(buffer);
                obj.IsItemLost = ReadBool(buffer);
                obj.Unk2List = ReadEntityList<CDataJobEmblemUnk0>(buffer);
                return obj;
            }
        }
    }
}

