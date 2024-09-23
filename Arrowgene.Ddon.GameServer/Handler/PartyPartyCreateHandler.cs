using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyCreateHandler : GameStructurePacketHandler<C2SPartyPartyCreateReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyCreateHandler));

        public PartyPartyCreateHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyCreateReq> packet)
        {
            S2CPartyPartyCreateRes res = new S2CPartyPartyCreateRes();

            PartyGroup party = Server.PartyManager.NewParty();
            if (party == null)
            {
                Logger.Error(client, "can not create party (Server.PartyManager.NewParty() == null)");
                res.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                client.Send(res);
                return;
            }

            ErrorRes<PlayerPartyMember> host = party.AddHost(client);
            if (host.HasError)
            {
                Logger.Error(client, "failed to create and join new party");
                res.Error = (uint)host.ErrorCode;
                client.Send(res);
                return;
            }

            ErrorRes<PlayerPartyMember> join = party.Join(client);
            if (join.HasError)
            {
                Logger.Error(client, "Failed to join new party");
                res.Error = (uint)join.ErrorCode;
                client.Send(res);
                return;
            }

            var quests = Server.Database.GetQuestProgressByType(client.Character.CommonId, QuestType.All);
            foreach (var quest in quests)
            {
                if (quest.VariantId != 0)
                {
                    Logger.Debug($"Getting quest progress. Adding {quest.QuestId} with variant {quest.VariantId}");
                    party.QuestState.AddNewQuest(quest.QuestId, quest.Step, true, (uint)quest.VariantId);
                    continue;
                }

                party.QuestState.AddNewQuest(quest.QuestId, quest.Step, true);
            }

            // Add quest for debug command
            party.QuestState.AddNewQuest(QuestManager.GetQuest(70000001));

            S2CPartyPartyJoinNtc ntc = new S2CPartyPartyJoinNtc();
            ntc.HostCharacterId = client.Character.CharacterId;
            ntc.LeaderCharacterId = client.Character.CharacterId;
            ntc.PartyMembers.Add(join.Value.GetCDataPartyMember());
            client.Send(ntc);

            res.PartyId = party.Id;
            client.Send(res);

            Logger.Info(client, $"created party with PartyId:{party.Id}");
        }
    }
}
