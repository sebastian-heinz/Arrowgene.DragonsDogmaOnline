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
            Test.Add(new CDataCommonU32 { Value = request.Unk0 });
            Test.Add(new CDataCommonU32 { Value = request.Unk1 });
            // No idea if this is what I'm meant to do, but it isn't causing crashes/errors?
            // I've tried sending just an empty list, but no behaviour changes.

            var res = new S2CCraftGetEquipRareTypeItemsRes()
            {
                UnkList = Test
            };
            return res;
        }
    }
}
