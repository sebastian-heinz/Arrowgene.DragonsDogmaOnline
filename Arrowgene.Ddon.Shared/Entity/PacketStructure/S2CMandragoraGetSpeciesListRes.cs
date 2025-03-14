using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMandragoraGetSpeciesListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MANDRAGORA_GET_SPECIES_LIST_RES;

        public List<CDataMyMandragoraSpecies> MandragoraSpeciesList { get; set; }

        public S2CMandragoraGetSpeciesListRes()
        {
            MandragoraSpeciesList = new List<CDataMyMandragoraSpecies>();
        }

        public class Serializer : PacketEntitySerializer<S2CMandragoraGetSpeciesListRes>
        {
            public override void Write(IBuffer buffer, S2CMandragoraGetSpeciesListRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataMyMandragoraSpecies>(buffer, obj.MandragoraSpeciesList);
            }

            public override S2CMandragoraGetSpeciesListRes Read(IBuffer buffer)
            {
                S2CMandragoraGetSpeciesListRes obj = new S2CMandragoraGetSpeciesListRes();

                ReadServerResponse(buffer, obj);

                obj.MandragoraSpeciesList = ReadEntityList<CDataMyMandragoraSpecies>(buffer);

                return obj;
            }
        }
    }
}
