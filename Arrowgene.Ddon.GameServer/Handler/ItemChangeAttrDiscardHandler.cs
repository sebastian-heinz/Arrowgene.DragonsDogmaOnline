using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemChangeAttrDiscardHandler : GameRequestPacketHandler<C2SItemChangeAttrDiscardReq, S2CItemChangeAttrDiscardRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemChangeAttrDiscardHandler));

        public ItemChangeAttrDiscardHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemChangeAttrDiscardRes Handle(GameClient client, C2SItemChangeAttrDiscardReq request)
        {
            Server.ItemManager.SetSafetySetting(client, client.Character, request.ItemList, request.DiscardSetting);

            return new S2CItemChangeAttrDiscardRes();
        }
    }
}
