using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobJobValueShopGetLineupHandler : GameStructurePacketHandler<C2SJobJobValueShopGetLineupReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobGetPlayPointListHandler));

        public JobJobValueShopGetLineupHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobJobValueShopGetLineupReq> packet)
        {
            client.Send(new S2CJobJobValueShopGetLineupRes()
            { 
                JobId = packet.Structure.JobId,
                JobValueType = packet.Structure.JobValueType,
                JobValueShopItemList = GetLineup(packet.Structure.JobId)
            });
        }

        //TODO: Move this into a Manager.
        private static List<CDataJobValueShopItem> GetLineup(JobId jobId)
        {
            List<uint> dragonTrinketIds = new List<uint>{
                0,
                16374,
                16375,
                16376,
                16377,
                16378,
                16379,
                16380,
                16381,
                16382,
                16725,
                21616
            };

            List<CDataJobValueShopItem> lineup = new List<CDataJobValueShopItem>();

            lineup.Add(new CDataJobValueShopItem()
            {
                LineupId = 1,
                ItemId = 13477, //Unappraised Moon Trinket (King)
                Price = 200,
                IsCountLimit = false,
                CanSelectStorage = true,
                UnableReason = 0
            });

            lineup.Add(new CDataJobValueShopItem()
            {
                LineupId = 2,
                ItemId = dragonTrinketIds[(byte)jobId], //Unidentified Dragon Trinket ([JOB])
                Price = 500,
                IsCountLimit = false,
                CanSelectStorage = true,
                UnableReason = 0
            });

            lineup.Add(new CDataJobValueShopItem()
            {
                LineupId = 3,
                ItemId = 18615, //Supreme Merit Medal
                Price = 2000,
                IsCountLimit = false,
                CanSelectStorage = true,
                UnableReason = 0
            });

            return lineup;
        }
    }
}
