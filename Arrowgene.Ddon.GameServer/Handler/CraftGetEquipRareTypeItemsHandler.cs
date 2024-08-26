#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Chat.Command.Commands;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetEquipRareTypeItemsHandler : GameRequestPacketHandler<C2SCraftGetEquipRareTypeItemsReq, S2CCraftGetEquipRareTypeItemsRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetEquipRareTypeItemsHandler));


        public CraftGetEquipRareTypeItemsHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftGetEquipRareTypeItemsRes Handle(GameClient client, C2SCraftGetEquipRareTypeItemsReq request)
        {
            
            List<CDataCommonU32> Test = new List<CDataCommonU32>();

            var res = new S2CCraftGetEquipRareTypeItemsRes()
            {
                UnkList = Test
            };
            return res;
        }
    }
}
