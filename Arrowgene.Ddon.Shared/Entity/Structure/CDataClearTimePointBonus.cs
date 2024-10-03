using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClearTimePointBonus
    {
        public CDataClearTimePointBonus()
        {
        }

        public uint Seconds {  get; set; }
        public uint Ratio {  get; set; }

        public class Serializer : EntitySerializer<CDataClearTimePointBonus>
        {
            public override void Write(IBuffer buffer, CDataClearTimePointBonus obj)
            {
                WriteUInt32(buffer, obj.Seconds);
                WriteUInt32(buffer, obj.Ratio);
            }

            public override CDataClearTimePointBonus Read(IBuffer buffer)
            {
                CDataClearTimePointBonus obj = new CDataClearTimePointBonus();
                obj.Seconds = ReadUInt32(buffer);
                obj.Ratio = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
