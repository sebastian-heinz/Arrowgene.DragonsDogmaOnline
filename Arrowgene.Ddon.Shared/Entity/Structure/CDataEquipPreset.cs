using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipPreset
    {
        public CDataEquipPreset()
        {
            PresetName = string.Empty;
            PresetEquipInfoList = new List<CDataPresetEquipInfo>();
            PresetJobItemList = new List<CDataEquipJobItem>();
        }

        public uint PresetNo { get; set; }
        public string PresetName { get; set; }
        public JobId Job { get; set; }
        public List<CDataPresetEquipInfo> PresetEquipInfoList { get; set; }
        public List<CDataEquipJobItem> PresetJobItemList { get; set; }

        public class Serializer : EntitySerializer<CDataEquipPreset>
        {
            public override void Write(IBuffer buffer, CDataEquipPreset obj)
            {
                WriteUInt32(buffer, obj.PresetNo);
                WriteMtString(buffer, obj.PresetName);
                WriteByte(buffer, (byte) obj.Job);
                WriteEntityList(buffer, obj.PresetEquipInfoList);
                WriteEntityList(buffer, obj.PresetJobItemList);
            }

            public override CDataEquipPreset Read(IBuffer buffer)
            {
                CDataEquipPreset obj = new CDataEquipPreset();
                obj.PresetNo = ReadUInt32(buffer);
                obj.PresetName = ReadMtString(buffer);
                obj.Job = (JobId)ReadByte(buffer);
                obj.PresetEquipInfoList = ReadEntityList<CDataPresetEquipInfo>(buffer);
                obj.PresetJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                return obj;
            }
        }
    }
}
