using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraCraftRecipeUnk4
    {
        /// <summary>
        /// TODO: num?
        /// </summary>
        public byte Unk0 { get; set; }

        /// <summary>
        /// TODO: ItemId?
        /// </summary>
        public uint Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraCraftRecipeUnk4>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraCraftRecipeUnk4 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataMyMandragoraCraftRecipeUnk4 Read(IBuffer buffer)
            {
                CDataMyMandragoraCraftRecipeUnk4 obj = new CDataMyMandragoraCraftRecipeUnk4();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
