using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class NpcGetExtendedFacilityHandler : GameRequestPacketHandler<C2SNpcGetNpcExtendedFacilityReq, S2CNpcGetNpcExtendedFacilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(NpcGetExtendedFacilityHandler));

        public NpcGetExtendedFacilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CNpcGetNpcExtendedFacilityRes Handle(GameClient client, C2SNpcGetNpcExtendedFacilityReq request)
        {
            var result = new S2CNpcGetNpcExtendedFacilityRes();
            if (gNpcExtendedBehavior.ContainsKey(request.NpcId))
            {
                result.NpcId = request.NpcId;
                gNpcExtendedBehavior[request.NpcId](Server, client, result);
            }
            return result;
        }

        private static bool CheckUnlockConditions(DdonGameServer server, GameClient client, EpitaphBarrier barrier)
        {
            foreach (var sectionId in barrier.DependentSectionIds)
            {
                var sectionInfo = server.EpitaphRoadManager.GetSectionById(sectionId);
                foreach (var unlockId in sectionInfo.UnlockDependencies)
                {
                    if (!client.Character.EpitaphRoadState.UnlockedContent.Contains(unlockId))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static readonly Dictionary<NpcId, Action<DdonGameServer, GameClient, S2CNpcGetNpcExtendedFacilityRes>> gNpcExtendedBehavior = new Dictionary<NpcId, Action<DdonGameServer, GameClient, S2CNpcGetNpcExtendedFacilityRes>>()
        {
            [NpcId.Pehr1] = (DdonGameServer server, GameClient client, S2CNpcGetNpcExtendedFacilityRes result) =>
            {
                if (client.Character.CompletedQuests.ContainsKey((QuestId) 60300020) || (client.QuestState.IsQuestActive(60300020) && client.QuestState.GetQuestState(60300020).Step > 2))
                {
                    result.ExtendedMenuItemList.Add(new CDataNpcExtendedFacilityMenuItem() { FunctionClass = NpcFunction.WarMissions, FunctionSelect = NpcFunction.HeroicSpiritSleepingPath, Unk2 = 4452 });
                }
            },
            [NpcId.Anita1] = (DdonGameServer server, GameClient client, S2CNpcGetNpcExtendedFacilityRes result) =>
            {
                var barrier = server.EpitaphRoadManager.GetBarrier(NpcId.Anita1);
                if (!client.Character.EpitaphRoadState.UnlockedContent.Contains(barrier.EpitaphId))
                {
                    if (!CheckUnlockConditions(server, client, barrier))
                    {
                        return;
                    }
                    result.ExtendedMenuItemList.Add(new CDataNpcExtendedFacilityMenuItem() { FunctionClass = NpcFunction.WarMissions, FunctionSelect = NpcFunction.GiveSpirits });
                }
            },
            [NpcId.Isel1] = (DdonGameServer server, GameClient client, S2CNpcGetNpcExtendedFacilityRes result) =>
            {
                var barrier = server.EpitaphRoadManager.GetBarrier(NpcId.Isel1);
                if (!client.Character.EpitaphRoadState.UnlockedContent.Contains(barrier.EpitaphId))
                {
                    if (!CheckUnlockConditions(server, client, barrier))
                    {
                        return;
                    }
                    result.ExtendedMenuItemList.Add(new CDataNpcExtendedFacilityMenuItem() { FunctionClass = NpcFunction.WarMissions, FunctionSelect = NpcFunction.GiveSpirits });
                }
            },
            [NpcId.Damad1] = (DdonGameServer server, GameClient client, S2CNpcGetNpcExtendedFacilityRes result) =>
            {
                var barrier = server.EpitaphRoadManager.GetBarrier(NpcId.Damad1);
                if (!client.Character.EpitaphRoadState.UnlockedContent.Contains(barrier.EpitaphId))
                {
                    if (!CheckUnlockConditions(server, client, barrier))
                    {
                        return;
                    }
                    result.ExtendedMenuItemList.Add(new CDataNpcExtendedFacilityMenuItem() { FunctionClass = NpcFunction.WarMissions, FunctionSelect = NpcFunction.GiveSpirits });
                }
            }
#if false
            // NPC which controls entrance to Memory of Megadosys
            // Currently commented out since area is not completed and personal quest is missing
            [NpcId.Morgan] = new List<CDataNpcExtendedFacilityMenuItem>()
            {
                // Memory of Megadosys
                new CDataNpcExtendedFacilityMenuItem() { FunctionClass = NpcFunction.WarMissions, FunctionSelect = NpcFunction.HeroicSpiritSleepingPath, Unk2 = 4452}
            },
#endif
        };

        private readonly byte[] pcap_data = new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xC2, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x45, 0x00, 0x00, 0x00, 0x43, 0x00, 0x00, 0x11, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x43, 0x20, 0xFB, 0xE8, 0xC0, 0xA0, 0xEC};
    }
}
