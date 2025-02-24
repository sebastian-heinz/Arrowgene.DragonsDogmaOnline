using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnList
    {
        public int PawnId { get; set; }
        public uint SlotNo { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Sex { get; set; }
        public PawnState PawnState { get; set; }
        public byte ShareRange { get; set; }
        public CDataPawnListData PawnListData { get; set; } = new();
        public uint Unk0 { get; set; }
        public uint TrainingExp { get; set; }
        public uint Unk2 { get; set; } // CDataPawnTrainingPreparationInfoToAdvice.Unk0

        public class Serializer : EntitySerializer<CDataPawnList>
        {
            public override void Write(IBuffer buffer, CDataPawnList obj)
            {
                WriteInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.SlotNo);
                WriteMtString(buffer, obj.Name);
                WriteByte(buffer, obj.Sex);
                WriteByte(buffer, (byte)obj.PawnState);
                WriteByte(buffer, obj.ShareRange);
                WriteEntity<CDataPawnListData>(buffer, obj.PawnListData);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.TrainingExp);
                WriteUInt32(buffer, obj.Unk2);
            }

            public override CDataPawnList Read(IBuffer buffer)
            {
                CDataPawnList obj = new CDataPawnList();
                obj.PawnId = ReadInt32(buffer);
                obj.SlotNo = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.Sex = ReadByte(buffer);
                obj.PawnState = (PawnState)ReadByte(buffer);
                obj.ShareRange = ReadByte(buffer);
                obj.PawnListData = ReadEntity<CDataPawnListData>(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.TrainingExp = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
