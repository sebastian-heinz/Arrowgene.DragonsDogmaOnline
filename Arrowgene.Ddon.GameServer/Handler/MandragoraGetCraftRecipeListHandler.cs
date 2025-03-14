using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MandragoraGetCraftRecipeListHandler : GameRequestPacketHandler<C2SMandragoraGetCraftRecipeListReq, S2CMandragoraGetCraftRecipeListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MandragoraGetCraftRecipeListHandler));

        public MandragoraGetCraftRecipeListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMandragoraGetCraftRecipeListRes Handle(GameClient client, C2SMandragoraGetCraftRecipeListReq request)
        {
            S2CMandragoraGetCraftRecipeListRes res = new S2CMandragoraGetCraftRecipeListRes();


            // TODO: what to do with request.Unk0  after choosing "raise"
            // TODO: very likely category requested and category answered
            res.Unk0 = request.Unk0;
            res.MandragoraCraftRecipeList = new List<CDataMyMandragoraCraftRecipe>
            {
                new CDataMyMandragoraCraftRecipe
                {
                    RecipeId = 123,
                    ItemId = ItemId.BonusDungeonTicketXp,
                    Time = 64800,
                    Unk3 = 3000,
                    Unk4 = new List<CDataMyMandragoraCraftRecipeUnk4>
                    {
                        new CDataMyMandragoraCraftRecipeUnk4
                        {
                            Unk0 = 1,
                            Unk1 = 1
                        }
                    },
                    Unk5 = false,
                    CraftMaterialList = new List<CDataMDataCraftMaterial>
                    {
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.BonePearl,
                            Num = 2,
                            SortNo = 1,
                            IsSp = false
                        },
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.Garnet,
                            Num = 5,
                            SortNo = 2,
                            IsSp = false
                        }
                    }
                },
                new CDataMyMandragoraCraftRecipe
                {
                    RecipeId = 456,
                    ItemId = ItemId.BonusDungeonTicketR, // Bonus Dungeon Ticket RIM
                    Time = 64800,
                    Unk3 = 3000,
                    Unk4 = new List<CDataMyMandragoraCraftRecipeUnk4>
                    {
                        new CDataMyMandragoraCraftRecipeUnk4
                        {
                            Unk0 = 1,
                            Unk1 = 1
                        }
                    },
                    Unk5 = true,
                    CraftMaterialList = new List<CDataMDataCraftMaterial>
                    {
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.BonePearl,
                            Num = 1,
                            SortNo = 1,
                            IsSp = false
                        },
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.KeyOfGemstones,
                            Num = 1,
                            SortNo = 2,
                            IsSp = false
                        },
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.Garnet,
                            Num = 3,
                            SortNo = 3,
                            IsSp = false
                        }
                    }
                },
                new CDataMyMandragoraCraftRecipe
                {
                    RecipeId = 789,
                    ItemId = ItemId.MandragoraLeaf, // Mandragora Leaf
                    Time = 64800,
                    Unk3 = 3000,
                    Unk4 = new List<CDataMyMandragoraCraftRecipeUnk4>
                    {
                        new CDataMyMandragoraCraftRecipeUnk4
                        {
                            Unk0 = 1,
                            Unk1 = 1
                        }
                    },
                    Unk5 = false,
                    CraftMaterialList = new List<CDataMDataCraftMaterial>
                    {
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.ClearWater,
                            Num = 1,
                            SortNo = 1,
                            IsSp = false
                        },
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.PlanktonLiquid,
                            Num = 1,
                            SortNo = 2,
                            IsSp = false
                        },
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.AshenGrass,
                            Num = 2,
                            SortNo = 3,
                            IsSp = false
                        }
                    }
                },
                new CDataMyMandragoraCraftRecipe
                {
                    RecipeId = 101112,
                    ItemId = ItemId.MandragoraTwig, // Mandragora Twig
                    Time = 64800,
                    Unk3 = 3000,
                    Unk4 = new List<CDataMyMandragoraCraftRecipeUnk4>
                    {
                        new CDataMyMandragoraCraftRecipeUnk4
                        {
                            Unk0 = 1,
                            Unk1 = 1
                        }
                    },
                    Unk5 = true,
                    CraftMaterialList = new List<CDataMDataCraftMaterial>
                    {
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.ClearWater,
                            Num = 1,
                            SortNo = 1,
                            IsSp = false
                        },
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.PlanktonLiquid,
                            Num = 1,
                            SortNo = 2,
                            IsSp = false
                        },
                        new CDataMDataCraftMaterial
                        {
                            ItemId = ItemId.RathniteLocalHardwood,
                            Num = 2,
                            SortNo = 3,
                            IsSp = false
                        }
                    }
                }
            };


            return res;
        }
    }
}
