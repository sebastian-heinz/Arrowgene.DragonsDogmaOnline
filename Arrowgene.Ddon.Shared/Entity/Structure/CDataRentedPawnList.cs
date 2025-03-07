using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRentedPawnList
    {
        public CDataRentedPawnList()
        {
            PawnListData = new CDataPawnListData();
        }

        public uint PawnId { get; set; }
        public uint SlotNo { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Sex { get; set; }
        public byte PawnState { get; set; }
        public byte CraftCount { get; set; }
        public byte AdventureCount {  get; set; }
        public CDataPawnListData PawnListData { get; set; }
        public PawnType PawnType { get; set; }

        public class Serializer : EntitySerializer<CDataRentedPawnList>
        {
            public override void Write(IBuffer buffer, CDataRentedPawnList obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.SlotNo);
                WriteMtString(buffer, obj.Name);
                WriteByte(buffer, obj.Sex);
                WriteByte(buffer, obj.PawnState);
                WriteByte(buffer, obj.CraftCount);
                WriteByte(buffer, obj.AdventureCount);
                WriteEntity<CDataPawnListData>(buffer, obj.PawnListData);
                WriteByte(buffer, (byte) obj.PawnType);
            }

            public override CDataRentedPawnList Read(IBuffer buffer)
            {
                CDataRentedPawnList obj = new CDataRentedPawnList();
                obj.PawnId = ReadUInt32(buffer);
                obj.SlotNo = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.Sex = ReadByte(buffer);
                obj.PawnState = ReadByte(buffer);
                obj.CraftCount = ReadByte(buffer);
                obj.AdventureCount = ReadByte(buffer);
                obj.PawnListData = ReadEntity<CDataPawnListData>(buffer);
                obj.PawnType = (PawnType) ReadByte(buffer);
                return obj;
            }
        }
    }
}
