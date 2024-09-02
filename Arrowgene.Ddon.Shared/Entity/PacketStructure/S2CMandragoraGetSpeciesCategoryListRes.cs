using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMandragoraGetSpeciesCategoryListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MANDRAGORA_GET_SPECIES_CATEGORY_LIST_RES;

        public List<CDataMyMandragoraSpeciesCategory> MandragoraSpeciesCategoryList { get; set; }

        public S2CMandragoraGetSpeciesCategoryListRes()
        {
            MandragoraSpeciesCategoryList = new List<CDataMyMandragoraSpeciesCategory>();
        }

        public class Serializer : PacketEntitySerializer<S2CMandragoraGetSpeciesCategoryListRes>
        {
            public override void Write(IBuffer buffer, S2CMandragoraGetSpeciesCategoryListRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataMyMandragoraSpeciesCategory>(buffer, obj.MandragoraSpeciesCategoryList);
            }

            public override S2CMandragoraGetSpeciesCategoryListRes Read(IBuffer buffer)
            {
                S2CMandragoraGetSpeciesCategoryListRes obj = new S2CMandragoraGetSpeciesCategoryListRes();

                ReadServerResponse(buffer, obj);

                obj.MandragoraSpeciesCategoryList = ReadEntityList<CDataMyMandragoraSpeciesCategory>(buffer);

                return obj;
            }
        }
    }
}
