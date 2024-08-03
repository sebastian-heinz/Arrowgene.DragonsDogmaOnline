using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SDispelGetDispelItemSettingsReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_DISPEL_GET_DISPEL_ITEM_SETTING_REQ;

        public C2SDispelGetDispelItemSettingsReq()
        {
        }

        public ShopType ShopType { get; set; }

        public class Serializer : PacketEntitySerializer<C2SDispelGetDispelItemSettingsReq>
        {
            public override void Write(IBuffer buffer, C2SDispelGetDispelItemSettingsReq obj)
            {
                WriteUInt32(buffer, (uint) obj.ShopType);
            }

            public override C2SDispelGetDispelItemSettingsReq Read(IBuffer buffer)
            {
                C2SDispelGetDispelItemSettingsReq obj = new C2SDispelGetDispelItemSettingsReq();
                obj.ShopType = (ShopType) ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
