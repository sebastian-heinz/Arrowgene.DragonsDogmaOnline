using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraBreedType
    {
        public uint BreedId { get; set; }
        public string BreedName { get; set; }
        public uint DiscoveredBreedNumMaybe { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraBreedType>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraBreedType obj)
            {
                WriteUInt32(buffer, obj.BreedId);
                WriteMtString(buffer, obj.BreedName);
                WriteUInt32(buffer, obj.DiscoveredBreedNumMaybe);
            }

            public override CDataMyMandragoraBreedType Read(IBuffer buffer)
            {
                CDataMyMandragoraBreedType obj = new CDataMyMandragoraBreedType();
                obj.BreedId = ReadUInt32(buffer);
                obj.BreedName = ReadMtString(buffer);
                obj.DiscoveredBreedNumMaybe = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
