using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyLeaveHandler : GameStructurePacketHandler<C2SPartyPartyLeaveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyLeaveHandler));

        public PartyPartyLeaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyLeaveReq> packet)
        {
            PartyGroup party = client.Party;

            if (party == null)
            {
                Logger.Error(client, "Could not leave party, does not exist");
                // todo return error
                return;
            }

            party.Leave(client);
            Logger.Info(client, $"Left PartyId:{party.Id}");

            if (party.ContentId != 0)
            {
                var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);
                if (!data.IsInRecreate)
                {
                    Server.BoardManager.RemoveCharacterFromGroup(client.Character);
                    Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.Online);

                    if (BoardManager.BoardIdIsExm(party.ContentId))
                    {
                        Server.PartyQuestContentManager.RemovePartyMember(party.Id, client.Character);
                    }
                }
                else
                {
                    Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.EntryBoard);
                }
            }
            else
            {
                Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.Online);
            }

            if (party.MemberCount() == 1 && party.Leader != null && !BoardManager.BoardIdIsExm(party.ContentId))
            {
                Server.CharacterManager.UpdateOnlineStatus(party.Leader.Client, party.Leader.Client.Character, OnlineStatus.Online);
            }

            S2CPartyPartyLeaveNtc partyLeaveNtc = new S2CPartyPartyLeaveNtc();
            partyLeaveNtc.CharacterId = client.Character.CharacterId;
            party.SendToAll(partyLeaveNtc);

            client.Send(new S2CPartyPartyLeaveRes());
        }
    }
}
