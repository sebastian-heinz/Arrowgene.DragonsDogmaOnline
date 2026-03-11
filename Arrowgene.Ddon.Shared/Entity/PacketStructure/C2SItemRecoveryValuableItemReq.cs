using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemRecoveryValuableItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_RECOVERY_VALUABLE_ITEM_REQ;

        public ItemId ItemId { get; set; }
        public StorageType DestinationStorage { get; set; }
        public List<CDataWalletPoint> Price { get; set; } = [];

        public class Serializer : PacketEntitySerializer<C2SItemRecoveryValuableItemReq>
        {
            public override void Write(IBuffer buffer, C2SItemRecoveryValuableItemReq obj)
            {
                WriteUInt32(buffer, (uint)obj.ItemId);
                WriteByte(buffer, (byte)obj.DestinationStorage);
                WriteEntityList(buffer, obj.Price);
            }

            public override C2SItemRecoveryValuableItemReq Read(IBuffer buffer)
            {
                C2SItemRecoveryValuableItemReq obj = new C2SItemRecoveryValuableItemReq();
                obj.ItemId = (ItemId)ReadUInt32(buffer);
                obj.DestinationStorage = (StorageType)ReadByte(buffer);
                obj.Price = ReadEntityList<CDataWalletPoint>(buffer);

                return obj;
            }
        }
    }
}
