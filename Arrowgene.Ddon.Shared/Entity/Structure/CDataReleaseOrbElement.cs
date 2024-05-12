using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataReleaseOrbElement
{
    public UInt32 ElementId { get; set; }
    public byte PageNo { get; set; }
    public byte GroupNo { get; set; }
    public byte Index {  get; set; }

    public class Serializer : EntitySerializer<CDataReleaseOrbElement>
    {
        public override void Write(IBuffer buffer, CDataReleaseOrbElement obj)
        {
            WriteUInt32(buffer, obj.ElementId);
            WriteByte(buffer, obj.PageNo);
            WriteByte(buffer, obj.GroupNo);
            WriteByte(buffer, obj.Index);
        }

        public override CDataReleaseOrbElement Read(IBuffer buffer)
        {
            CDataReleaseOrbElement obj = new CDataReleaseOrbElement();
            obj.ElementId = ReadUInt32(buffer);
            obj.PageNo = ReadByte(buffer);
            obj.GroupNo = ReadByte(buffer);
            obj.Index = ReadByte(buffer);
            return obj;
        }
    }
}
