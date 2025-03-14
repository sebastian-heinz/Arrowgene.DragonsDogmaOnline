using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataTreasurePoint
    {
        public uint Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataTreasurePoint>
        {
            public override void Write(IBuffer buffer, CDataTreasurePoint obj)
            {
                WriteUInt32(buffer, obj.Index);
                WriteMtString(buffer, obj.Name);
                WriteBool(buffer, obj.Unk2);
            }

            public override CDataTreasurePoint Read(IBuffer buffer)
            {
                CDataTreasurePoint obj = new CDataTreasurePoint();
                obj.Index = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.Unk2 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
