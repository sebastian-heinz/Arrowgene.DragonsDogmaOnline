using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataNoraPawnInfo
    {
        public CDataNoraPawnInfo() {
            Name = string.Empty;
            EditInfo = new();
            CharacterEquipData = new();
        }

        public uint Version { get; set; }
        public string Name { get; set; }
        public CDataEditInfo EditInfo { get; set; }
        public byte Job { get; set; }
        public List<CDataCharacterEquipData> CharacterEquipData { get; set; }

        public class Serializer : EntitySerializer<CDataNoraPawnInfo>
        {
            public override void Write(IBuffer buffer, CDataNoraPawnInfo obj)
            {
                WriteUInt32(buffer, obj.Version);
                WriteMtString(buffer, obj.Name);
                WriteEntity<CDataEditInfo>(buffer, obj.EditInfo);
                WriteByte(buffer, obj.Job);
                WriteEntityList<CDataCharacterEquipData>(buffer, obj.CharacterEquipData);
            }

            public override CDataNoraPawnInfo Read(IBuffer buffer)
            {
                CDataNoraPawnInfo obj = new CDataNoraPawnInfo();
                obj.Version = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                obj.Job = ReadByte(buffer);
                obj.CharacterEquipData = ReadEntityList<CDataCharacterEquipData>(buffer);
                return obj;
            }
        }
    }
}
