using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPresetAbilityParam
    {
        public CDataPresetAbilityParam()
        {
            PresetNo=0;
            PresetName=string.Empty;
            AbilityList=new List<CDataSetAcquirementParam>();
        }

        public byte PresetNo { get; set; }
        public string PresetName { get; set; }
        public List<CDataSetAcquirementParam> AbilityList { get; set; }

        public class Serializer : EntitySerializer<CDataPresetAbilityParam>
        {
            public override void Write(IBuffer buffer, CDataPresetAbilityParam obj)
            {
                WriteByte(buffer, obj.PresetNo);
                WriteMtString(buffer, obj.PresetName);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.AbilityList);
            }

            public override CDataPresetAbilityParam Read(IBuffer buffer)
            {
                CDataPresetAbilityParam obj = new CDataPresetAbilityParam();
                obj.PresetNo = ReadByte(buffer);
                obj.PresetName = ReadMtString(buffer);
                obj.AbilityList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
