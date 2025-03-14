using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLostPawnList
    {
        public CDataLostPawnList()
        {
            PawnListData = new CDataPawnListData();
        }

        public int PawnId { get; set; }
        public uint SlotNo { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Sex { get; set; }
        public PawnState PawnState { get; set; }
        public byte ShareRange { get; set; }
        public uint ReviveCost { get; set; }
        public CDataPawnListData PawnListData { get; set; }

        public class Serializer : EntitySerializer<CDataLostPawnList>
        {
            public override void Write(IBuffer buffer, CDataLostPawnList obj)
            {
                WriteInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.SlotNo);
                WriteMtString(buffer, obj.Name);
                WriteByte(buffer, obj.Sex);
                WriteByte(buffer, (byte) obj.PawnState);
                WriteByte(buffer, obj.ShareRange);
                WriteUInt32(buffer, obj.ReviveCost);
                WriteEntity<CDataPawnListData>(buffer, obj.PawnListData);
            }

            public override CDataLostPawnList Read(IBuffer buffer)
            {
                CDataLostPawnList obj = new CDataLostPawnList();
                obj.PawnId = ReadInt32(buffer);
                obj.SlotNo = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.Sex = ReadByte(buffer);
                obj.PawnState = (PawnState) ReadByte(buffer);
                obj.ShareRange = ReadByte(buffer);
                obj.ReviveCost = ReadUInt32(buffer);
                obj.PawnListData = ReadEntity<CDataPawnListData>(buffer);
                return obj;
            }
        }
    }
}
