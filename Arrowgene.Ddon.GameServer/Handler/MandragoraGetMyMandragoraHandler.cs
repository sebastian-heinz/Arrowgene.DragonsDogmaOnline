using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MandragoraGetMyMandragoraHandler : GameRequestPacketHandler<C2SMandragoraGetMyMandragoraReq, S2CMandragoraGetMyMandragoraRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MandragoraGetMyMandragoraHandler));

        public MandragoraGetMyMandragoraHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMandragoraGetMyMandragoraRes Handle(GameClient client, C2SMandragoraGetMyMandragoraReq request)
        {
            S2CMandragoraGetMyMandragoraRes res = new S2CMandragoraGetMyMandragoraRes();

            res.MandragoraFurnitureItemList = new List<CDataMyMandragoraFurnitureItem>
            {
                new()
                {
                    MandragoraId = 1,
                    FurnitureItemId = ItemId.MandragoraPottedPlant1Normal
                },
                new()
                {
                    MandragoraId = 2,
                    FurnitureItemId = ItemId.MandragoraPottedPlant2Passport
                },
                new()
                {
                    MandragoraId = 3,
                    FurnitureItemId = ItemId.MandragoraPottedPlant3Passport
                }
            };

            res.MandragoraList = new List<CDataMyMandragora>
            {
                new CDataMyMandragora()
                {
                    SpeciesIndex = 3,
                    SpeciesCategory = MandragoraSpeciesCategory.Normal,
                    MandragoraId = 1,
                    MandragoraName = "Scoperta",
                    Unk4 = 0,
                    Unk5 = 0,
                    Unk6 = 6,
                    Unk7 = new CDataMyMandragoraUnk1Unk7
                    {
                        Unk0 = 1,
                        Unk1 = 0,
                        Unk2 = new List<CDataMyMandragoraUnk1Unk7Unk2>(),
                        Unk3 = 0
                    }
                },
                new CDataMyMandragora
                {
                    SpeciesIndex = 101,
                    SpeciesCategory = MandragoraSpeciesCategory.Chilli,
                    MandragoraId = 2,
                    MandragoraName = "Creazione",
                    Unk4 = 0,
                    Unk5 = 0,
                    Unk6 = 5,
                    Unk7 = new CDataMyMandragoraUnk1Unk7
                    {
                        Unk0 = 2,
                        Unk1 = 0,
                        Unk2 = new List<CDataMyMandragoraUnk1Unk7Unk2>(),
                        Unk3 = 0
                    }
                },
                new CDataMyMandragora
                {
                    SpeciesIndex = 1,
                    SpeciesCategory = MandragoraSpeciesCategory.Normal,
                    MandragoraId = 3,
                    MandragoraName = "Strano",
                    Unk4 = 0,
                    Unk5 = 0,
                    Unk6 = 4,
                    Unk7 = new CDataMyMandragoraUnk1Unk7
                    {
                        Unk0 = 3,
                        Unk1 = 0,
                        Unk2 = new List<CDataMyMandragoraUnk1Unk7Unk2>(),
                        Unk3 = 0
                    }
                }
            };

            res.MandragoraCraftCategoriesMaybe = new List<CDataMyMandragoraCraftCategory>
            {
                new CDataMyMandragoraCraftCategory
                {
                    CategoryId = 1,
                    CategoryName = "Dungeon Tickets"
                },
                new CDataMyMandragoraCraftCategory
                {
                    CategoryId = 2,
                    CategoryName = "Materials"
                }
            };

            res.Unk3 = new List<CDataMyMandragoraUnk3>
            {
                new CDataMyMandragoraUnk3
                {
                    Unk0 = 5,
                    Unk1 = 1
                }
            };

            res.MandragoraFertilizerItemList = new List<CDataMyMandragoraFertilizerItem>
            {
                new CDataMyMandragoraFertilizerItem
                {
                    ItemId = 17959,
                    ItemNum = 1
                }
            };

            res.MandragoraCultivationMaterialMaxMaybe = 20;

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

            res.RarityLevelList = new List<CDataMyMandragoraRarityLevel>
            {
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = MandragoraRarity.LimitedRare,
                    Rarity = "Limited Rare"
                },
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = MandragoraRarity.Common,
                    Rarity = "Common"
                },
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = MandragoraRarity.Uncommon,
                    Rarity = "Uncommon"
                },
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = MandragoraRarity.Rare,
                    Rarity = "Rare"
                },
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = MandragoraRarity.MysticRare,
                    Rarity = "Mystic Rare"
                }
            };

            res.FreeMandragoraIdListMaybe = new List<CDataCommonU8>
            {
                new CDataCommonU8(1),
                new CDataCommonU8(2),
                new CDataCommonU8(3)
            };

            return res;
        }
    }
}
