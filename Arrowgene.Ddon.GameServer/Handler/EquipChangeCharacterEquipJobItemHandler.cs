#nullable enable
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipChangeCharacterEquipJobItemHandler : GameStructurePacketHandler<C2SEquipChangeCharacterEquipJobItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterEquipJobItemHandler));

        private readonly EquipManager _equipManager;

        public EquipChangeCharacterEquipJobItemHandler(DdonGameServer server) : base(server)
        {
            _equipManager = server.EquipManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangeCharacterEquipJobItemReq> packet)
        {
            _equipManager.EquipJobItem(Server, client, client.Character, packet.Structure.ChangeEquipJobItemList);
        }
    }
}