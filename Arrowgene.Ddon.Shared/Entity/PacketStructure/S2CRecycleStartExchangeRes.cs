using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CRecycleStartExchangeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_RECYCLE_START_EXCHANGE_RES;

        public S2CRecycleStartExchangeRes()
        {
            ItemRewardList = new List<CDataRecycleItemLot>();
            WalletPointList = new List<CDataWalletPoint>();
            ItemUpdateResultList = new List<CDataItemUpdateResult>();
        }

        public byte Unk0 { get; set; }
        public List<CDataWalletPoint> WalletPointList { get; set; } // I don't see these rewards showing
        public List<CDataRecycleItemLot> ItemRewardList { get; set; } // Not sure what this does
        public List<CDataItemUpdateResult> ItemUpdateResultList { get; set; } // This shows the items you received

        public class Serializer : PacketEntitySerializer<S2CRecycleStartExchangeRes>
        {
            public override void Write(IBuffer buffer, S2CRecycleStartExchangeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.WalletPointList);
                WriteEntityList(buffer, obj.ItemRewardList);
                WriteEntityList(buffer, obj.ItemUpdateResultList);
            }

            public override S2CRecycleStartExchangeRes Read(IBuffer buffer)
            {
                S2CRecycleStartExchangeRes obj = new S2CRecycleStartExchangeRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadByte(buffer);
                obj.WalletPointList = ReadEntityList<CDataWalletPoint>(buffer);
                obj.ItemRewardList = ReadEntityList<CDataRecycleItemLot>(buffer);
                obj.ItemUpdateResultList = ReadEntityList<CDataItemUpdateResult>(buffer);
                return obj;
            }
        }
    }
}


