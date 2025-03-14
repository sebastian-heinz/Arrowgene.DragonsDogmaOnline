using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataNoraPawnInfo
    {
        public uint Version { get; set; }
        public string Name { get; set; } = string.Empty;
        public CDataEditInfo EditInfo { get; set; } = new();
        public byte Job { get; set; }
        public List<CDataCharacterEquipData> CharacterEquipData { get; set; } = new();
        public List<CDataCharacterEquipData> CharacterEquipViewData { get; set; } = new();

        public class Serializer : EntitySerializer<CDataNoraPawnInfo>
        {
            public override void Write(IBuffer buffer, CDataNoraPawnInfo obj)
            {
                WriteUInt32(buffer, obj.Version);
                WriteMtString(buffer, obj.Name);
                WriteEntity(buffer, obj.EditInfo);
                WriteByte(buffer, obj.Job);
                WriteEntityList(buffer, obj.CharacterEquipData);
                WriteEntityList(buffer, obj.CharacterEquipViewData);
            }

            public override CDataNoraPawnInfo Read(IBuffer buffer)
            {
                CDataNoraPawnInfo obj = new CDataNoraPawnInfo();
                obj.Version = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                obj.Job = ReadByte(buffer);
                obj.CharacterEquipData = ReadEntityList<CDataCharacterEquipData>(buffer);
                obj.CharacterEquipViewData = ReadEntityList<CDataCharacterEquipData>(buffer);
                return obj;
            }
        }
    }
}
