using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaGetAreaSupplyInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_GET_AREA_SUPPLY_INFO_RES;

        public S2CAreaGetAreaSupplyInfoRes()
        {
            RewardItemInfoList = new();
        }

        public byte SupplyGrade { get; set; }
        public List<CDataRewardItemInfo> RewardItemInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaGetAreaSupplyInfoRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetAreaSupplyInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.SupplyGrade);
                WriteEntityList(buffer, obj.RewardItemInfoList);
            }

            public override S2CAreaGetAreaSupplyInfoRes Read(IBuffer buffer)
            {
                S2CAreaGetAreaSupplyInfoRes obj = new S2CAreaGetAreaSupplyInfoRes();
                ReadServerResponse(buffer, obj);
                obj.SupplyGrade = ReadByte(buffer);
                obj.RewardItemInfoList = ReadEntityList<CDataRewardItemInfo>(buffer);
                return obj;
            }
        }
    }
}
