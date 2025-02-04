using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPlayPointData
    {
        public ExpMode ExpMode;
        public uint PlayPoint;

        public class Serializer : EntitySerializer<CDataPlayPointData>
        {
            public override void Write(IBuffer buffer, CDataPlayPointData obj)
            {
                WriteByte(buffer, (byte)obj.ExpMode);
                WriteUInt32(buffer, obj.PlayPoint);
            }

            public override CDataPlayPointData Read(IBuffer buffer)
            {
                CDataPlayPointData obj = new CDataPlayPointData();
                obj.ExpMode = (ExpMode)ReadByte(buffer);
                obj.PlayPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
