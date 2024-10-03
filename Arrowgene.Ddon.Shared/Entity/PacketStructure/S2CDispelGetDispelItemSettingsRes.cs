using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CDispelGetDispelItemSettingsRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_DISPEL_GET_DISPEL_ITEM_SETTING_RES;

        public S2CDispelGetDispelItemSettingsRes()
        {
            CategoryList = new List<CDataDispelItemCategoryInfo>();
        }

        public List<CDataDispelItemCategoryInfo> CategoryList {  get; set; }
        public bool Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CDispelGetDispelItemSettingsRes>
        {
            public override void Write(IBuffer buffer, S2CDispelGetDispelItemSettingsRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.CategoryList);
                WriteBool(buffer, obj.Unk1);
            }

            public override S2CDispelGetDispelItemSettingsRes Read(IBuffer buffer)
            {
                S2CDispelGetDispelItemSettingsRes obj = new S2CDispelGetDispelItemSettingsRes();
                ReadServerResponse(buffer, obj);
                obj.CategoryList = ReadEntityList<CDataDispelItemCategoryInfo>(buffer);
                obj.Unk1 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
