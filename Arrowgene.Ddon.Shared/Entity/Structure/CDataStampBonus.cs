using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStampBonus
    {
        //BonusType 1-5 map to WalletType 1-5, but the others are not included.
        //Alternatively, you can use an ItemID here.
        public uint BonusType { get; set; }
        public uint BonusValue { get; set; }

        public class Serializer : EntitySerializer<CDataStampBonus>
        {
            public override void Write(IBuffer buffer, CDataStampBonus obj)
            {
                WriteUInt32(buffer, obj.BonusType);
                WriteUInt32(buffer, obj.BonusValue);
            }

            public override CDataStampBonus Read(IBuffer buffer)
            {
                CDataStampBonus obj = new CDataStampBonus();
                obj.BonusType = ReadUInt32(buffer);
                obj.BonusValue = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
