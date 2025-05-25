using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemStatParam
    {
        public EquipStatId StatId { get; set; }
        public byte Add { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemStatParam>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemStatParam obj)
            {
                WriteByte(buffer, (byte) obj.StatId);
                WriteByte(buffer, obj.Add);
            }

            public override CDataJobEmblemStatParam Read(IBuffer buffer)
            {
                CDataJobEmblemStatParam obj = new CDataJobEmblemStatParam();
                obj.StatId = (EquipStatId) ReadByte(buffer);
                obj.Add = ReadByte(buffer);
                return obj;
            }
        }
    }
}
