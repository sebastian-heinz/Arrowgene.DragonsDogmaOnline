using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStampBonusGetListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_STAMP_BONUS_GET_LIST_RES;

        public S2CStampBonusGetListRes()
        {
            DailyStamp = new List<CDataStampBonusDaily>();
            TotalStamp = new CDataStampBonusTotal();
        }

        public List<CDataStampBonusDaily> DailyStamp { get; set; }
        public CDataStampBonusTotal TotalStamp { get; set; }

        public class Serializer : PacketEntitySerializer<S2CStampBonusGetListRes>
        {
            public override void Write(IBuffer buffer, S2CStampBonusGetListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataStampBonusDaily>(buffer, obj.DailyStamp);
                WriteEntity<CDataStampBonusTotal>(buffer, obj.TotalStamp);
            }

            public override S2CStampBonusGetListRes Read(IBuffer buffer)
            {
                S2CStampBonusGetListRes obj = new S2CStampBonusGetListRes();
                ReadServerResponse(buffer, obj);
                obj.DailyStamp = ReadEntityList<CDataStampBonusDaily>(buffer);
                obj.TotalStamp = ReadEntity<CDataStampBonusTotal>(buffer);
                return obj;
            }
        }
    }
}
