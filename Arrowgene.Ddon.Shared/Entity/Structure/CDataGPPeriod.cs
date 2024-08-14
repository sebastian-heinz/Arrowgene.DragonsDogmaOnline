using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPPeriod
    {
        public uint GP { get; set; }
        public bool isFreeGP { get; set; }
        public DateTimeOffset Period { get; set; }

        public CDataGPPeriod()
        {
            GP = 0;
            isFreeGP = false;
            Period = DateTimeOffset.UtcNow;
        }

        public class Serializer : EntitySerializer<CDataGPPeriod>
        {
            public override void Write(IBuffer buffer, CDataGPPeriod obj)
            {
                WriteUInt32(buffer, obj.GP);
                WriteBool(buffer, obj.isFreeGP);
                WriteUInt64(buffer, (ulong)obj.Period.ToUnixTimeSeconds());
            }

            public override CDataGPPeriod Read(IBuffer buffer)
            {
                CDataGPPeriod obj = new CDataGPPeriod();
                obj.GP = ReadUInt32(buffer);
                obj.isFreeGP = ReadBool(buffer);
                obj.Period = DateTimeOffset.FromUnixTimeSeconds((long)ReadUInt64(buffer));
                return obj;
            }
        }
    }
}
