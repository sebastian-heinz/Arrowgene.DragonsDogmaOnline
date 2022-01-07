using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct DoubleByteThing
    {
        byte unk1;
        byte unk2;    
    }

    public class DoubleByteThingSerializer : EntitySerializer<DoubleByteThing>
    {
        public override void Write(IBuffer buffer, DoubleByteThing obj)
        {
            throw new NotImplementedException();
        }

        public override DoubleByteThing Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
