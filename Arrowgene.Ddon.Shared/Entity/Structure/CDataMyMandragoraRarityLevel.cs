using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraRarityLevel
    {
        public uint RarityId { get; set; }
        public string Rarity { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraRarityLevel>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraRarityLevel obj)
            {
                WriteUInt32(buffer, obj.RarityId);
                WriteMtString(buffer, obj.Rarity);
            }

            public override CDataMyMandragoraRarityLevel Read(IBuffer buffer)
            {
                CDataMyMandragoraRarityLevel obj = new CDataMyMandragoraRarityLevel();
                obj.RarityId = ReadUInt32(buffer);
                obj.Rarity = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
