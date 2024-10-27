using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipUpdateEquipPresetNameHandler : GameRequestPacketHandler<C2SEquipUpdateEquipPresetNameReq, S2CEquipUpdateEquipPresetNameRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipUpdateEquipPresetNameHandler));

        public EquipUpdateEquipPresetNameHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEquipUpdateEquipPresetNameRes Handle(GameClient client, C2SEquipUpdateEquipPresetNameReq request)
        {
            CharacterCommon characterCommon;
            if (client.EquipPresetIndex == 0)
            {
                characterCommon = client.Character;
            }
            else
            {
                characterCommon = client.Character.PawnBySlotNo((byte)client.EquipPresetIndex);
            }

            var result = Server.Database.SelectEquipmentPresets(characterCommon.CommonId, characterCommon.Job).Where(x => x.PresetNo == request.PresetNo).FirstOrDefault();
            Server.Database.UpdateEquipmentPreset(characterCommon.CommonId, result.Job, result.PresetNo, request.PresetName);
            return new S2CEquipUpdateEquipPresetNameRes();
        }
    }
}

