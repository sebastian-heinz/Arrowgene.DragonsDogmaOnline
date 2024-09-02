using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
                    FurnitureItemId = 17370
                },
                new()
                {
                    MandragoraId = 2,
                    FurnitureItemId = 17371
                },
                new()
                {
                    MandragoraId = 3,
                    FurnitureItemId = 17372
                }
            };

            res.MandragoraList = new List<CDataMyMandragora>
            {
                new CDataMyMandragora()
                {
                    Unk0 = 1,
                    Unk1 = 1,
                    MandragoraId = 1,
                    MandragoraName = "Scoperta",
                    Unk4 = 0,
                    Unk5 = 0,
                    Unk6 = 1,
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
                    Unk0 = 1,
                    Unk1 = 1,
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
                    Unk0 = 0,
                    Unk1 = 0,
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

            res.MandragoraBreedTypeList = new List<CDataMyMandragoraBreedType>
            {
                new CDataMyMandragoraBreedType
                {
                    BreedId = 1,
                    BreedName = "ノーマル種",
                    DiscoveredBreedNumMaybe = 1
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 2,
                    BreedName = "チリ種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 4,
                    BreedName = "アルビノ種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 6,
                    BreedName = "チャコ種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 9,
                    BreedName = "ベジ種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 3,
                    BreedName = "鎧亜種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 5,
                    BreedName = "衣亜種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 7,
                    BreedName = "花亜種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 8,
                    BreedName = "蛮亜種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 11,
                    BreedName = "巻亜種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 10,
                    BreedName = "鉢亜種",
                    DiscoveredBreedNumMaybe = 0
                },
                new CDataMyMandragoraBreedType
                {
                    BreedId = 99,
                    BreedName = "特別種",
                    DiscoveredBreedNumMaybe = 0
                }
            };

            res.RarityLevelList = new List<CDataMyMandragoraRarityLevel>
            {
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = 1,
                    Rarity = "Limited Rare"
                },
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = 2,
                    Rarity = "Common"
                },
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = 3,
                    Rarity = "Uncommon"
                },
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = 4,
                    Rarity = "Rare"
                },
                new CDataMyMandragoraRarityLevel
                {
                    RarityId = 5,
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
