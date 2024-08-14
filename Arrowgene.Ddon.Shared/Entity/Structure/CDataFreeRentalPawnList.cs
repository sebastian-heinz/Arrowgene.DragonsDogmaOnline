using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataFreeRentalPawnList
    {
        public uint PawnId { get; set; }
        public string Name { get; set; }
        public string ImageAddr { get; set; }
        public uint LineupId { get; set; }
        public ulong ExpireDateTime { get; set; }

        public CDataFreeRentalPawnList()
        {
        }

        public class Serializer : EntitySerializer<CDataFreeRentalPawnList>
        {
            public override void Write(IBuffer buffer, CDataFreeRentalPawnList obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteMtString(buffer, obj.Name);
                WriteMtString(buffer, obj.ImageAddr);
                WriteUInt32(buffer, obj.LineupId);
                WriteUInt64(buffer, obj.ExpireDateTime);
            }

            public override CDataFreeRentalPawnList Read(IBuffer buffer)
            {
                CDataFreeRentalPawnList obj = new CDataFreeRentalPawnList
                {
                    PawnId = ReadUInt32(buffer),
                    Name = ReadMtString(buffer),
                    ImageAddr = ReadMtString(buffer),
                    LineupId = ReadUInt32(buffer),
                    ExpireDateTime = ReadUInt64(buffer),
                };

                return obj;
            }
        }
    }
}
