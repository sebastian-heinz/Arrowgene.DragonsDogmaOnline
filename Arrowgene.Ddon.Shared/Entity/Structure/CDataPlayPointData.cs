using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataPlayPointData
    {
        public byte ExpMode;
        public uint PlayPoint;
    }

    public class CDataPlayPointDataSerializer : EntitySerializer<CDataPlayPointData>
    {
        public override void Write(IBuffer buffer, CDataPlayPointData obj)
        {
            WriteByte(buffer, obj.ExpMode);
            WriteUInt32(buffer, obj.PlayPoint);
        }

        public override CDataPlayPointData Read(IBuffer buffer)
        {
            CDataPlayPointData obj = new CDataPlayPointData();
            obj.ExpMode = ReadByte(buffer);
            obj.PlayPoint = ReadUInt32(buffer);
            return obj;
        }
    }
}
