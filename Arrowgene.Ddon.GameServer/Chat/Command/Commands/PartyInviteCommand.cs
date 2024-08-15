using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class PartyInviteCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;

        public override string Key => "invite";
        public override string HelpText => "usage: `/invite [Pawn/Player Name]`";

        private DdonGameServer _server;
        private PartyPartyInviteCharacterHandler _inviteCharacterHandler;
        private PawnJoinPartyMypawnHandler _inviteMypawnHandler;

        public PartyInviteCommand(DdonGameServer server)
        {
            _server = server;
            _inviteCharacterHandler = new PartyPartyInviteCharacterHandler(server);
            _inviteMypawnHandler = new PawnJoinPartyMypawnHandler(server);
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            if (command.Length == 0)
            {
                // check expected length before accessing
                responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
                return;
            }

            if (!client.Party.GetPlayerPartyMember(client).IsLeader)
            {
                responses.Add(ChatResponse.CommandError(client, "Only the party leader can invite players."));
                return;
            }

            // TODO: What happens if some smartass decides to place a space in their pawns name?
            if (command.Length == 1)
            {
                var tuple = client.Character.Pawns
                    .Select((pawn, index) => new {pawn = pawn, pawnNumber = (byte)(index+1)})
                    .Where(tuple => tuple.pawn.Name == command[0])
                    .FirstOrDefault();

                if (tuple == null)
                {
                    responses.Add(ChatResponse.CommandError(client, "No pawn was found by that name."));
                    return;
                }

                _inviteMypawnHandler.Handle(client, new StructurePacket<C2SPawnJoinPartyMypawnReq>(new C2SPawnJoinPartyMypawnReq()
                {
                    PawnNumber = tuple.pawnNumber
                }));
            }
            else
            {
                GameClient targetClient = _server.ClientLookup.GetClientByCharacterName(command[0], command[1]);

                if (targetClient == null)
                {
                    responses.Add(ChatResponse.CommandError(client, "No player was found by that name."));
                    return;
                }

                if (targetClient == client)
                {
                    responses.Add(ChatResponse.CommandError(client, "You cannot invite yourself."));
                    return;
                }

                //TODO: Revisit how to check if a player can actually receive an invite or not.

                _inviteCharacterHandler.Handle(client, new StructurePacket<C2SPartyPartyInviteCharacterReq>(new C2SPartyPartyInviteCharacterReq()
                {
                    CharacterId = targetClient.Character.CharacterId
                }));

                responses.Add(ChatResponse.ServerMessage(client, $"Party invite sent to {targetClient.Character.FirstName} {targetClient.Character.LastName}."));
            }
        }
    }
}
