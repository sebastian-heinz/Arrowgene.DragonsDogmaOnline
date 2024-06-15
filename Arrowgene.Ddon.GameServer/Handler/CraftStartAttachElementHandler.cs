#nullable enable
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartAttachElementHandler : GameStructurePacketHandler<C2SCraftStartAttachElementReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartAttachElementHandler));
        private static readonly List<StorageType> STORAGE_TYPES = new List<StorageType> {
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
        };

        private readonly ItemManager _itemManager;
        private readonly EquipManager _equipManager;

        public CraftStartAttachElementHandler(DdonGameServer server) : base(server)
        {
            _itemManager = Server.ItemManager;
            _equipManager = Server.EquipManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftStartAttachElementReq> packet)
        {


            // TODO: All the logic needed for crests

            List<CDataEquipElementParam> elementlist = new List<CDataEquipElementParam>();
            elementlist.Add(new CDataEquipElementParam { SlotNo = 1, ItemId = 13398});

            var res = new S2CCraftStartAttachElementRes()
            {
                Gold = 20,
                EquipElementParamList = elementlist,
            };
            client.Send(res);
        }
    }
}