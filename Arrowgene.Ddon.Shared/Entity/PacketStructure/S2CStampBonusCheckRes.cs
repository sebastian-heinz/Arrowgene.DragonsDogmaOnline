using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStampBonusCheckRes : ServerResponse
    {
        public S2CStampBonusCheckRes()
        {
            StampCheck = new List<CDataStampCheck>();
        }

        public override PacketId Id => PacketId.S2C_STAMP_BONUS_CHECK_RES;

        public List<CDataStampCheck> StampCheck { get; set; }
        public byte IsRecieveBonusDaily { get; set; } // May be flip-flopped with the other byte.
        public byte IsRecieveBonusTotal { get; set; } // May be flip-flopped with the other byte.


        public class Serializer : PacketEntitySerializer<S2CStampBonusCheckRes>
        {
            public override void Write(IBuffer buffer, S2CStampBonusCheckRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataStampCheck>(buffer, obj.StampCheck);
                WriteByte(buffer, obj.IsRecieveBonusDaily);
                WriteByte(buffer, obj.IsRecieveBonusTotal);
            }

            public override S2CStampBonusCheckRes Read(IBuffer buffer)
            {
                S2CStampBonusCheckRes obj = new S2CStampBonusCheckRes();
                ReadServerResponse(buffer, obj);
                obj.StampCheck = ReadEntityList<CDataStampCheck>(buffer);
                obj.IsRecieveBonusDaily = ReadByte(buffer);
                obj.IsRecieveBonusTotal = ReadByte(buffer);

                return obj;
            }
        }
    }
}
