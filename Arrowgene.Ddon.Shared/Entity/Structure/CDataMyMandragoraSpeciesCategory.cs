using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraSpeciesCategory
    {
        public MandragoraSpeciesCategory SpeciesCategory { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// The UI will show the category as ??? if num is 0, in the original server though those categories were not shown at all.
        /// </summary>
        public uint DiscoveredSpeciesNumMaybe { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraSpeciesCategory>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraSpeciesCategory obj)
            {
                WriteUInt32(buffer, (uint)obj.SpeciesCategory);
                WriteMtString(buffer, obj.CategoryName);
                WriteUInt32(buffer, obj.DiscoveredSpeciesNumMaybe);
            }

            public override CDataMyMandragoraSpeciesCategory Read(IBuffer buffer)
            {
                CDataMyMandragoraSpeciesCategory obj = new CDataMyMandragoraSpeciesCategory();
                obj.SpeciesCategory = (MandragoraSpeciesCategory)ReadUInt32(buffer);
                obj.CategoryName = ReadMtString(buffer);
                obj.DiscoveredSpeciesNumMaybe = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
