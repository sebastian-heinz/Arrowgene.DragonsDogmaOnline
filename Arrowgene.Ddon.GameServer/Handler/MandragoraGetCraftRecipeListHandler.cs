using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
                    RecipeId = 1,
                    ItemId = 17923, // Bonus Dungeon Ticket XP
                    Time = 30,
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
                            ItemId = 17881,
                            Num = 2,
                            SortNo = 1,
                            IsSp = false
                        },
                        new CDataMDataCraftMaterial
                        {
                            ItemId = 7896,
                            Num = 5,
                            SortNo = 2,
                            IsSp = false
                        }
                    }
                }
            };


            return res;
        }
    }
}
