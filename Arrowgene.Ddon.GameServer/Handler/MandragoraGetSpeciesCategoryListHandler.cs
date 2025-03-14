using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MandragoraGetSpeciesCategoryListHandler : GameRequestPacketHandler<C2SMandragoraGetSpeciesCategoryListReq, S2CMandragoraGetSpeciesCategoryListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MandragoraGetSpeciesCategoryListHandler));

        public MandragoraGetSpeciesCategoryListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMandragoraGetSpeciesCategoryListRes Handle(GameClient client, C2SMandragoraGetSpeciesCategoryListReq request)
        {
            S2CMandragoraGetSpeciesCategoryListRes res = new S2CMandragoraGetSpeciesCategoryListRes();

            res.MandragoraSpeciesCategoryList = new List<CDataMyMandragoraSpeciesCategory>
            {
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Normal,
                    CategoryName = "Normal Species",
                    DiscoveredSpeciesNumMaybe = 1
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Chilli,
                    CategoryName = "Chilli Species",
                    DiscoveredSpeciesNumMaybe = 5
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Albino,
                    CategoryName = "Albino Species",
                    DiscoveredSpeciesNumMaybe = 0
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Charcoal,
                    CategoryName = "Charcoal Species",
                    DiscoveredSpeciesNumMaybe = 0
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Veggie,
                    CategoryName = "Veggie Species",
                    DiscoveredSpeciesNumMaybe = 0
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Armored,
                    CategoryName = "Armored Species",
                    DiscoveredSpeciesNumMaybe = 0
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Clothed,
                    CategoryName = "Clothed Species",
                    DiscoveredSpeciesNumMaybe = 0
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Flowering,
                    CategoryName = "Flowering Species",
                    DiscoveredSpeciesNumMaybe = 0
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Barbarian,
                    CategoryName = "Barbarian Species",
                    DiscoveredSpeciesNumMaybe = 0
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Scroll,
                    CategoryName = "Scroll Species",
                    DiscoveredSpeciesNumMaybe = 0
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Helmet,
                    CategoryName = "Helmet Species",
                    DiscoveredSpeciesNumMaybe = 0
                },
                new CDataMyMandragoraSpeciesCategory
                {
                    SpeciesCategory = MandragoraSpeciesCategory.Special,
                    CategoryName = "Special Species",
                    DiscoveredSpeciesNumMaybe = 0
                }
            };

            return res;
        }
    }
}
