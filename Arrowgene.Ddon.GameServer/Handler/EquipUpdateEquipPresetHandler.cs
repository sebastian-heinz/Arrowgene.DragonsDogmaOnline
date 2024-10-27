using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipUpdateEquipPresetHandler : GameRequestPacketHandler<C2SEquipUpdateEquipPresetReq, S2CEquipUpdateEquipPresetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipUpdateEquipPresetHandler));

        public EquipUpdateEquipPresetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEquipUpdateEquipPresetRes Handle(GameClient client, C2SEquipUpdateEquipPresetReq request)
        {
            var result = new S2CEquipUpdateEquipPresetRes();

            CharacterCommon characterCommon;
            if (client.EquipPresetIndex == 0)
            {
                characterCommon = client.Character;
            }
            else
            {
                characterCommon = client.Character.PawnBySlotNo((byte) request.PawnId);
            }

            result.EquipPreset.PresetName = request.PresetName;
            result.EquipPreset.PresetNo = request.PresetNo;
            result.EquipPreset.PresetEquipInfoList = characterCommon.Equipment.AsCDataPresetEquipInfo((EquipType) request.Type);
            result.EquipPreset.Job = characterCommon.Job;

            // TODO: Make transaction
            Server.Database.DeleteEquipmentPreset(characterCommon.CommonId, characterCommon.Job, request.PresetNo);
            Server.Database.InsertEquipmentPreset(characterCommon.CommonId, characterCommon.Job, request.PresetNo, request.PresetName);

            foreach (var item in result.EquipPreset.PresetEquipInfoList)
            {
                Server.Database.InsertEquipmentPresetTemplate(characterCommon.CommonId, characterCommon.Job, request.PresetNo, item.EquipSlotNo, item.ItemUId);
            }

            return result;
        }
    }
}

