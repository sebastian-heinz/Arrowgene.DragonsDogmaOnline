using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MandragoraGetSpeciesListHandler : GameRequestPacketHandler<C2SMandragoraGetSpeciesListReq, S2CMandragoraGetSpeciesListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MandragoraGetSpeciesListHandler));

        public MandragoraGetSpeciesListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMandragoraGetSpeciesListRes Handle(GameClient client, C2SMandragoraGetSpeciesListReq request)
        {
            S2CMandragoraGetSpeciesListRes res = new S2CMandragoraGetSpeciesListRes();

            if (request.SpeciesCategory == MandragoraSpeciesCategory.Normal)
            {
                res.MandragoraSpeciesList = new List<CDataMyMandragoraSpecies>
                {
                    new CDataMyMandragoraSpecies
                    {
                        Index = 1,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Common,
                        Unk3 = 0,
                        Visible = true,
                        Unk5 = true,
                        FirstDiscovery = "1",
                        DiscoveredDate = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - 100,
                        New = false
                    },
                    new CDataMyMandragoraSpecies
                    {
                        Index = 2,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Common,
                        Unk3 = 0,
                        Visible = true,
                        Unk5 = false,
                        FirstDiscovery = "2",
                        DiscoveredDate = 0,
                        New = false
                    },
                    new CDataMyMandragoraSpecies
                    {
                        Index = 3,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Uncommon,
                        Unk3 = 0,
                        Visible = true,
                        Unk5 = false,
                        FirstDiscovery = "3",
                        DiscoveredDate = 0,
                        New = false
                    },
                    new CDataMyMandragoraSpecies
                    {
                        Index = 4,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Common,
                        Unk3 = 0,
                        Visible = true,
                        Unk5 = false,
                        FirstDiscovery = "4",
                        DiscoveredDate = 0,
                        New = false
                    },
                    new CDataMyMandragoraSpecies
                    {
                        Index = 5,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Rare,
                        Unk3 = 0,
                        Visible = true,
                        Unk5 = false,
                        FirstDiscovery = "5",
                        DiscoveredDate = 0,
                        New = false
                    }
                };
            }
            if (request.SpeciesCategory == MandragoraSpeciesCategory.Chilli)
            {
                res.MandragoraSpeciesList = new List<CDataMyMandragoraSpecies>
                {
                    new CDataMyMandragoraSpecies
                    {
                        Index = 101,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Common,
                        Unk3 = 0,
                        Visible = true,
                        Unk5 = true,
                        FirstDiscovery = "101",
                        DiscoveredDate = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() - 100,
                        New = false
                    },
                    new CDataMyMandragoraSpecies
                    {
                        Index = 102,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Common,
                        Unk3 = 0,
                        Visible = true,
                        Unk5 = false,
                        FirstDiscovery = "102",
                        DiscoveredDate = 0,
                        New = false
                    },
                    new CDataMyMandragoraSpecies
                    {
                        Index = 103,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Uncommon,
                        Unk3 = 0,
                        Visible = true,
                        Unk5 = false,
                        FirstDiscovery = "103",
                        DiscoveredDate = 0,
                        New = false
                    },
                    new CDataMyMandragoraSpecies
                    {
                        Index = 104,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Uncommon,
                        Unk3 = 0,
                        Visible = true,
                        Unk5 = false,
                        FirstDiscovery = "104",
                        DiscoveredDate = 0,
                        New = false
                    },
                    new CDataMyMandragoraSpecies
                    {
                        Index = 105,
                        Unk1 = 0,
                        Rarity = MandragoraRarity.Common,
                        Unk3 = 0,
                        Visible = false,
                        Unk5 = false,
                        FirstDiscovery = "105",
                        DiscoveredDate = 0,
                        New = false
                    }
                };
            }


            return res;
        }
    }
}
