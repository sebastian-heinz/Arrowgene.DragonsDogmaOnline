using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterEquipData
    {
        public CDataCharacterEquipData()
        {
            Equips = new List<CDataEquipItemInfo>();
        }

        public List<CDataEquipItemInfo> Equips;

        public class Serializer : EntitySerializer<CDataCharacterEquipData>
        {
            public override void Write(IBuffer buffer, CDataCharacterEquipData obj)
            {
                WriteEntityList(buffer, obj.Equips);
            }

            public override CDataCharacterEquipData Read(IBuffer buffer)
            {
                CDataCharacterEquipData obj = new CDataCharacterEquipData();
                obj.Equips = ReadEntityList<CDataEquipItemInfo>(buffer);
                return obj;
            }
        }
    }
}
