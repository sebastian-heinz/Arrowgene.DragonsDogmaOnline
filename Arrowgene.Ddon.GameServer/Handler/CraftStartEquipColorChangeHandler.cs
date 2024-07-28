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
using Arrowgene.Ddon.Database;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartEquipColorChangeHandler : GameStructurePacketHandler<C2SCraftStartEquipColorChangeReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipColorChangeHandler));
        private readonly EquipManager _equipManager;

        public CraftStartEquipColorChangeHandler(DdonGameServer server) : base(server)
        {
            _equipManager = Server.EquipManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftStartEquipColorChangeReq> packet)
        {
            Character common = client.Character;
            uint charId = client.Character.CharacterId;
            string equipItemUID = packet.Structure.EquipItemUID;
            byte color = packet.Structure.Color;
            List<CDataCraftColorant> colorlist = new List<CDataCraftColorant>(); // this is probably for consuming the dye
            uint pawnId = packet.Structure.CraftMainPawnID;
            ushort equipslot = 0;
            byte equiptype = 0;


            List<CDataCharacterEquipInfo> characterEquipList = common.Equipment.AsCDataCharacterEquipInfo(EquipType.Performance)
                        .Union(common.Equipment.AsCDataCharacterEquipInfo(EquipType.Visual))
                        .ToList();

            var equipInfo = characterEquipList.FirstOrDefault(info => info.EquipItemUId == equipItemUID);
            equipslot = equipInfo.EquipCategory;
            equiptype = (byte)equipInfo.EquipType;

            CDataEquipSlot EquipmentSlot = new CDataEquipSlot()
            {
                CharId = charId,
                PawnId = pawnId,
                EquipType = equiptype,
                EquipSlot = equipslot,
            };

            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUID = equipItemUID,
                EquipSlot = EquipmentSlot
            };

            // TODO: Potentially the packets changed in S3.
            

            S2CCraftStartEquipColorChangeRes res = new S2CCraftStartEquipColorChangeRes()
                {
                    ColorNo = color,
                    CurrentEquipInfo = CurrentEquipInfo
                };
            client.Send(res);
        }
    }
}