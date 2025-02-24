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

        private static Dictionary<byte, uint> MandatoryTypes = new()
        {
            { 50, 18484 },
            { 52, 18505 },
            { 57, 18638 },
            { 58, 18639 },
            { 60, 21131 }
        };
        private static Dictionary<uint, byte> ItemIdToLayoutId = new()
        {
            {18484, 50},
            {18505, 52},
            {18638, 57},
            {18639, 58},
            {21129, 50},
            {21130, 58},
            {21131, 60},
            {21132, 60},
            {21133, 59},
            {21713, 50},
            {21714, 57},
            {21715, 58},
            {21776, 60},
            {23124, 60},
            {23125, 58},
            {23126, 57},
            {23127, 59},
            {23128, 50},
            {23129, 57},
            {23130, 50}
        };
        private static Dictionary<uint, uint> LineupIdToFurnitureId = new()
        {
            {53, 21129},
            {54, 21130},
            {55, 21132},
            {56, 21133},
            {57, 21713},
            {58, 21714},
            {59, 21715},
            {60, 21776},
            {64, 23124},
            {63, 23125},
            {62, 23126},
            {61, 23127},
            {65, 23128},
            {66, 23129},
            {67, 23130}
        };
        
        public ClanGetFurnitureHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanGetFurnitureRes Handle(GameClient client, C2SClanGetFurnitureReq request)
        {
            var res = new S2CClanGetFurnitureRes();
            var pcap = new S2CClanGetFurnitureRes.Serializer().Read(FurnitureData);

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
                uint furnitureId = defaultId;
                if (customDict.ContainsKey(type))
                {
                    furnitureId = customDict[type];
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

        private readonly byte[] FurnitureData =
        {
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x14, 0x0, 0x0, 0x48, 0x34,
            0x0, 0x0, 0x0, 0x0, 0x32, 0x0, 0x0, 0x52, 0x89, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x54, 0xD1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x5A, 0x58, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x5A, 0x5A, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x48, 0x49, 0x0, 0x0, 0x0,
            0x0, 0x34, 0x0, 0x0, 0x48, 0xCE, 0x0, 0x0, 0x0, 0x0, 0x39, 0x0, 0x0, 0x54, 0xD2, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x5A, 0x56, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x5A,
            0x59, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x48, 0xCF, 0x0, 0x0, 0x0, 0x0, 0x3A, 0x0,
            0x0, 0x52, 0x8A, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x54, 0xD3, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x5A, 0x55, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x52, 0x8D, 0x0, 0x0,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x5A, 0x57, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x52, 0x8B,
            0x0, 0x0, 0x0, 0x0, 0x3C, 0x0, 0x0, 0x52, 0x8C, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x55, 0x10, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x5A, 0x54, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0x0, 0x22
        };
    }
}
