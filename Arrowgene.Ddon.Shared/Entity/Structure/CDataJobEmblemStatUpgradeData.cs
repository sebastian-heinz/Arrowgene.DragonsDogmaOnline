using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemStatUpgradeData
    {
        public byte Level { get; set; }
        public EquipStatId StatId { get; set; }
        public ushort Amount { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemStatUpgradeData>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemStatUpgradeData obj)
            {
                WriteByte(buffer, obj.Level);
                WriteByte(buffer, (byte) obj.StatId);
                WriteUInt16(buffer, obj.Amount);
            }

            public override CDataJobEmblemStatUpgradeData Read(IBuffer buffer)
            {
                CDataJobEmblemStatUpgradeData obj = new CDataJobEmblemStatUpgradeData();
                obj.Level = ReadByte(buffer);
                obj.StatId = (EquipStatId) ReadByte(buffer);
                obj.Amount = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
