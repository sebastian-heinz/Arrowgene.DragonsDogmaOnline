using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLostPawn
    {
        public CDataLostPawn()
        {
            PawnListData = new CDataPawnListData();
        }

        public int PawnId { get; set; }
        public uint SlotNo { get; set; }
        public string Name { get; set; }
        public byte Sex { get; set; }
        public byte PawnState { get; set; }
        public byte ShareRange { get; set; }
        public uint ReviveCost { get; set; }
        public CDataPawnListData PawnListData { get; set; }

        public class Serializer : EntitySerializer<CDataLostPawn>
        {
            public override void Write(IBuffer buffer, CDataLostPawn obj)
            {
                WriteInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.SlotNo);
                WriteMtString(buffer, obj.Name);
                WriteByte(buffer, obj.Sex);
                WriteByte(buffer, obj.PawnState);
                WriteByte(buffer, obj.ShareRange);
                WriteUInt32(buffer, obj.ReviveCost);
                WriteEntity<CDataPawnListData>(buffer, obj.PawnListData);
            }

            public override CDataLostPawn Read(IBuffer buffer)
            {
                CDataLostPawn obj = new CDataLostPawn();
                obj.PawnId = ReadInt32(buffer);
                obj.SlotNo = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.Sex = ReadByte(buffer);
                obj.PawnState = ReadByte(buffer);
                obj.ShareRange = ReadByte(buffer);
                obj.ReviveCost = ReadUInt32(buffer);
                obj.PawnListData = ReadEntity<CDataPawnListData>(buffer);
                return obj;
            }
        }
    }
}