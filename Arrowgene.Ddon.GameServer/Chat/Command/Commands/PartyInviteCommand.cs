using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;

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
                responses.Add(ChatResponse.CommandError(client, "no arguments provided"));
                return;
            }

            if (!client.Party.GetPlayerPartyMember(client).IsLeader)
            {
                responses.Add(ChatResponse.CommandError(client, "only the leader can send invites"));
                return;
            }

            // TODO: What happens if some smartass decides to place a space in their pawns name?
            if (command.Length == 1)
            {
                var tuple = client.Character.Pawns
                    .Select((pawn, index) => new {pawn = pawn, pawnNumber = (byte)(index+1)})
                    .Where(tuple => tuple.pawn.Character.FirstName == command[0])
                    .SingleOrDefault();

                if (tuple == null)
                {
                    responses.Add(ChatResponse.CommandError(client, "no pawn found by that name"));
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
                    responses.Add(ChatResponse.CommandError(client, "no player found by that name"));
                    return;
                }

                _inviteCharacterHandler.Handle(client, new StructurePacket<C2SPartyPartyInviteCharacterReq>(new C2SPartyPartyInviteCharacterReq()
                {
                    CharacterId = targetClient.Character.Id
                }));

                responses.Add(ChatResponse.ServerMessage(client, "invite sent to "+targetClient.Character.FirstName+" "+targetClient.Character.LastName));
            }
        }
    }
}