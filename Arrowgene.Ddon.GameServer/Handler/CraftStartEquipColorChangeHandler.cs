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
    public class CraftStartEquipColorChangeHandler : GameRequestPacketHandler<C2SCraftStartEquipColorChangeReq, S2CCraftStartEquipColorChangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipColorChangeHandler));

        public CraftStartEquipColorChangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftStartEquipColorChangeRes Handle(GameClient client, C2SCraftStartEquipColorChangeReq request)
        {
            Character common = client.Character;
            uint charId = client.Character.CharacterId;
            string equipItemUID = request.EquipItemUID;
            byte color = request.Color;
            List<CDataCraftColorant> colorlist = new List<CDataCraftColorant>(); // this is probably for consuming the dye
            uint pawnId = request.CraftMainPawnID;
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
                CharacterId = charId,
                PawnId = pawnId,
                EquipType = (EquipType)equiptype,
                EquipSlotNo = equipslot,
            };

            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
                EquipSlot = EquipmentSlot
            };

            // TODO: Potentially the packets changed in S3.
            

            var res = new S2CCraftStartEquipColorChangeRes()
                {
                    ColorNo = color,
                    CurrentEquipInfo = CurrentEquipInfo
                };
            return res;
        }
    }
}