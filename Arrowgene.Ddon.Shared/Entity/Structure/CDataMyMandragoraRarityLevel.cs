using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraRarityLevel
    {
        public MandragoraRarity RarityId { get; set; }
        public string Rarity { get; set; } = string.Empty;

        public class Serializer : EntitySerializer<CDataMyMandragoraRarityLevel>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraRarityLevel obj)
            {
                WriteUInt32(buffer, (uint)obj.RarityId);
                WriteMtString(buffer, obj.Rarity);
            }

            public override CDataMyMandragoraRarityLevel Read(IBuffer buffer)
            {
                CDataMyMandragoraRarityLevel obj = new CDataMyMandragoraRarityLevel();
                obj.RarityId = (MandragoraRarity)ReadUInt32(buffer);
                obj.Rarity = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
