using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class DoubleByteThing
    {
        public byte unk1;
        public byte unk2;
    }

    public class DoubleByteThingSerializer : EntitySerializer<DoubleByteThing>
    {
        public override void Write(IBuffer buffer, DoubleByteThing obj)
        {
            WriteByte(buffer, obj.unk1);
            WriteByte(buffer, obj.unk2);
        }

        public override DoubleByteThing Read(IBuffer buffer)
        {
            DoubleByteThing obj = new DoubleByteThing();
            obj.unk1 = ReadByte(buffer);
            obj.unk2 = ReadByte(buffer);
            return obj;
        }
    }
}
