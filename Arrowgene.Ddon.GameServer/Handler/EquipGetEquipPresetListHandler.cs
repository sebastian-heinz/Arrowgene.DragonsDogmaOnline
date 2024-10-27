
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Drawing;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipGetEquipPresetListHandler : GameRequestPacketHandler<C2SEquipGetEquipPresetListReq, S2CEquipGetEquipPresetListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipGetEquipPresetListHandler));

        public EquipGetEquipPresetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEquipGetEquipPresetListRes Handle(GameClient client, C2SEquipGetEquipPresetListReq request)
        {
            var results = new S2CEquipGetEquipPresetListRes();

            CharacterCommon characterCommon;
            if (client.EquipPresetIndex == 0)
            {
                characterCommon = client.Character;
            }
            else
            {
                characterCommon = client.Character.PawnBySlotNo((byte)client.EquipPresetIndex);
            }

            foreach (var presetInfo in Server.Database.SelectEquipmentPresets(characterCommon.CommonId, characterCommon.Job))
            {
                var presetItems = Server.Database.SelectEquipmentPresetTemplate(characterCommon.CommonId, presetInfo.Job, presetInfo.PresetNo);
                foreach (var presetItem in presetItems)
                {
                    var matches = client.Character.Storage.FindItemByUIdInStorage(ItemManager.AllItemStorages, presetItem.ItemUId);
                    if (matches == null)
                    {
                        // Item was deleted or sold?
                        continue;
                    }

                    var storageType = matches.Item1;
                    var item = matches.Item2.Item2;

                    presetItem.ItemId = item.ItemId;
                    presetItem.Color = item.Color;
                    presetItem.PlusValue = item.PlusValue;
                    presetItem.EquipElementParamList = item.EquipElementParamList;

                    presetInfo.PresetEquipInfoList.Add(presetItem);
                }

                results.EquipPresetList.Add(presetInfo);
            }

            return results;
        }
    }
}
