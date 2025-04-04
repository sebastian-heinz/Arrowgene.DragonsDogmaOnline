using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanGetFurnitureHandler : GameRequestPacketHandler<C2SClanGetFurnitureReq, S2CClanGetFurnitureRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanGetFurnitureHandler));

        /// <summary>
        /// Required default furniture.
        /// </summary>
        private static readonly Dictionary<byte, ItemId> MandatoryTypes = new()
        {
            { ClanFurnitureType.PlazaObject, ItemId.Fountain },
            { ClanFurnitureType.Statue, ItemId.WhiteDragonStatue0 },
            { ClanFurnitureType.DiningTable, ItemId.DiningTable1 },
            { ClanFurnitureType.LargeCarpet, ItemId.LargeCarpet },
            { ClanFurnitureType.HallDecoration, ItemId.HallDecoration }
        };

        /// <summary>
        /// Maps an item to the internal ID of the furniture. 
        /// </summary>
        private static readonly Dictionary<ItemId, byte> ItemIdToLayoutId = new()
        {
            {ItemId.Fountain, ClanFurnitureType.PlazaObject},
            {ItemId.WhiteDragonStatue0, ClanFurnitureType.Statue},
            {ItemId.DiningTable1, ClanFurnitureType.DiningTable},
            {ItemId.LargeCarpet, ClanFurnitureType.LargeCarpet},
            {ItemId.ChristmasTree1, ClanFurnitureType.PlazaObject},
            {ItemId.LargeCarpetChristmas, ClanFurnitureType.LargeCarpet},
            {ItemId.HallDecoration, ClanFurnitureType.HallDecoration},
            {ItemId.HallDecorationChristmas, ClanFurnitureType.HallDecoration},
            {ItemId.LittleFrost1, ClanFurnitureType.LoungeBoard},
            {ItemId.ThirdAnniversaryPuppet1, ClanFurnitureType.PlazaObject},
            {ItemId.ThirdAnniversaryMenu, ClanFurnitureType.DiningTable},
            {ItemId.LargeCarpetThirdAnniversary, ClanFurnitureType.LargeCarpet},
            {ItemId.HallDecorationThirdAnniversary, ClanFurnitureType.HallDecoration},
            {ItemId.ClanHallHalloweenDecoration, ClanFurnitureType.HallDecoration},
            {ItemId.ClanHallHalloweenLargeCarpet, ClanFurnitureType.LargeCarpet},
            {ItemId.ClanHallHalloweenCuisine, ClanFurnitureType.DiningTable},
            {ItemId.ClanHallHalloweenPlush, ClanFurnitureType.LoungeBoard},
            {ItemId.ClanHallHalloweenTree, ClanFurnitureType.PlazaObject},
            {ItemId.ClanHallChristmasCuisine, ClanFurnitureType.DiningTable},
            {ItemId.ClanHallWhiteTree, ClanFurnitureType.PlazaObject}
        };

        /// <summary>
        /// Maps purchased clan upgrade LineupId to the item ID.
        /// </summary>
        private static readonly Dictionary<uint, ItemId> LineupIdToFurnitureId = new()
        {
            {53, ItemId.ChristmasTree1},
            {54, ItemId.LargeCarpetChristmas},
            {55, ItemId.HallDecorationChristmas},
            {56, ItemId.LittleFrost1},
            {57, ItemId.ThirdAnniversaryPuppet1},
            {58, ItemId.ThirdAnniversaryMenu},
            {59, ItemId.LargeCarpetThirdAnniversary},
            {60, ItemId.HallDecorationThirdAnniversary},
            {64, ItemId.ClanHallHalloweenDecoration},
            {63, ItemId.ClanHallHalloweenLargeCarpet},
            {62, ItemId.ClanHallHalloweenCuisine},
            {61, ItemId.ClanHallHalloweenPlush},
            {65, ItemId.ClanHallHalloweenTree},
            {66, ItemId.ClanHallChristmasCuisine},
            {67, ItemId.ClanHallWhiteTree}
        };
        
        public ClanGetFurnitureHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanGetFurnitureRes Handle(GameClient client, C2SClanGetFurnitureReq request)
        {
            var res = new S2CClanGetFurnitureRes();

            List<uint> baseFuncs = new();
            Dictionary<byte, uint> customDict = new();
            Server.Database.ExecuteInTransaction(connection =>
            {
                baseFuncs = Server.Database.SelectClanShopPurchases(client.Character.ClanId, connection);
                customDict = Server.Database.SelectClanBaseCustomizations(client.Character.ClanId, connection)
                    .Where(x => x.Type != 1)
                    .ToDictionary(key => key.Type, value => value.Id);
            });

            foreach (var furnitureId in MandatoryTypes.Values)
            {
                res.FurnitureLayouts.Add(new()
                {
                    ItemID = furnitureId,
                    OmID = 0,
                    LayoutID = 0
                });
            }

            foreach (var func in baseFuncs)
            {
                if (LineupIdToFurnitureId.ContainsKey(func))
                {
                    var furnitureId = LineupIdToFurnitureId[func];
                    res.FurnitureLayouts.Add(new()
                    {
                        ItemID = furnitureId,
                        OmID = 0,
                        LayoutID = 0
                    });
                }
            }

            foreach ((var type, var defaultId) in MandatoryTypes)
            {
                var furnitureId = defaultId;
                if (customDict.ContainsKey(type))
                {
                    furnitureId = (ItemId)customDict[type];
                }

                var match = res.FurnitureLayouts.Find(x => x.ItemID == furnitureId);
                if (match != null)
                {
                    match.LayoutID = type;
                }
                else
                {
                    var backupMatch = res.FurnitureLayouts.Find(x => x.ItemID == defaultId);
                    backupMatch.LayoutID = type;
                }
            }

            return res;
        }
    }
}
