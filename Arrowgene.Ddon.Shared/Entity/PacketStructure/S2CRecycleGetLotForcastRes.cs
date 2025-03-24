using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CRecycleGetLotForcastRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_RECYCLE_GET_LOT_FORECAST_RES;

        public S2CRecycleGetLotForcastRes()
        {
            ItemRewardList = new List<CDataRecycleItemLot>();
            WalletPointList = new List<CDataWalletPoint>();
        }

        public uint NumberOfItemsInLottery { get; set; } // ?
        public List<CDataRecycleItemLot> ItemRewardList { get; set; } // Lottery items?
        public List<CDataWalletPoint> WalletPointList { get; set; } // Shows possible currency rewards for the dissaembly

        public class Serializer : PacketEntitySerializer<S2CRecycleGetLotForcastRes>
        {
            public override void Write(IBuffer buffer, S2CRecycleGetLotForcastRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.NumberOfItemsInLottery);
                WriteEntityList(buffer, obj.ItemRewardList);
                WriteEntityList(buffer, obj.WalletPointList);
            }

            public override S2CRecycleGetLotForcastRes Read(IBuffer buffer)
            {
                S2CRecycleGetLotForcastRes obj = new S2CRecycleGetLotForcastRes();
                ReadServerResponse(buffer, obj);
                obj.NumberOfItemsInLottery = ReadUInt32(buffer);
                obj.ItemRewardList = ReadEntityList<CDataRecycleItemLot>(buffer);
                obj.WalletPointList = ReadEntityList<CDataWalletPoint>(buffer);
                return obj;
            }
        }
    }
}

