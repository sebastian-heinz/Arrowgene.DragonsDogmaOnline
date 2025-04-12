using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Shared.Network;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "invite";
    public override string HelpText               => "usage: `/invite [Pawn/Player Name]`";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        var inviteCharacterHandler = new PartyPartyInviteCharacterHandler(server);
        var inviteMypawnHandler = new PawnJoinPartyMyPawnHandler(server);

        if (command.Length == 0)
        {
            // check expected length before accessing
            responses.Add(ChatResponse.CommandError(client, "No arguments provided."));
            return;
        }

        if (client.Party.ContentId != 0)
        {
            responses.Add(ChatResponse.CommandError(client, "Use the recruitment board to invite players to the party."));
            return;
        }

        if (client.GameMode == GameMode.BitterblackMaze && client.Party.Members.Count >= 4)
        {
            responses.Add(ChatResponse.CommandError(client, "This game mode only supports 4 players."));
            return;
        }

        if (!client.Party.GetPlayerPartyMember(client).IsLeader)
        {
            responses.Add(ChatResponse.CommandError(client, "Only the party leader can invite."));
            return;
        }

        if (!StageManager.IsSafeArea(client.Character.Stage))
        {
            responses.Add(ChatResponse.CommandError(client, "You must be in a safe area to invite others."));
            return;
        }

        // TODO: What happens if some smartass decides to place a space in their pawns name?
        if (command.Length == 1)
        {
            var myTuple = client.Character.Pawns
                .Select((pawn, index) => new { pawn = pawn, pawnNumber = (byte)(index + 1) })
                .FirstOrDefault(tuple => tuple.pawn.Name == command[0]);
            //var rentedTuple = client.Character.RentedPawns
            //    .Select((pawn, index) => new { pawn = pawn, pawnNumber = (byte)(index + 1) })
            //    .FirstOrDefault(tuple => tuple.pawn.Name == command[0]);

            if (myTuple != null)
            {
                if (client.Party.Contains(myTuple.pawn))
                {
                    responses.Add(ChatResponse.CommandError(client, "The party already contains that pawn."));
                    return;
                }
                inviteMypawnHandler.Handle(client, new StructurePacket<C2SPawnJoinPartyMyPawnReq>(new C2SPawnJoinPartyMyPawnReq()
                {
                    PawnNumber = myTuple.pawnNumber
                }));
            }
            //else if (rentedTuple != null)
            //{
            //    if (client.Party.Contains(rentedTuple.pawn))
            //    {
            //        responses.Add(ChatResponse.CommandError(client, "The party already contains that pawn."));
            //        return;
            //    }

            //    inviteMypawnHandler.Handle(client, new StructurePacket<C2SPawnJoinPartyRentedPawnReq>(new C2SPawnJoinPartyRentedPawnReq()
            //    {
            //        SlotNo = myTuple.pawnNumber
            //    }));
            //}
            else
            {
                responses.Add(ChatResponse.CommandError(client, "No pawn was found by that name."));
                return;
            }
        }
        else
        {
            GameClient targetClient = server.ClientLookup.GetClientByCharacterName(command[0], command[1]);

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

            if (targetClient.GameMode != client.GameMode)
            {
                responses.Add(ChatResponse.CommandError(client, "You cannot invite players which are in different game modes."));
                return;
            }

            if (BannedStageIds.Contains(client.Character.Stage.Id))
            {
                responses.Add(ChatResponse.CommandError(client, "You cannot use invite players from this area."));
                return;
            }

            if (client.Party.Contains(targetClient.Character))
            {
                responses.Add(ChatResponse.CommandError(client, "The party already contains that player."));
            }

            if (!StageManager.IsSafeArea(targetClient.Character.Stage))
            {
                responses.Add(ChatResponse.CommandError(client, "The invited player is not in a safe area."));
                return;
            }

            //TODO: Revisit how to check if a player can actually receive an invite or not.

            inviteCharacterHandler.Handle(client, new StructurePacket<C2SPartyPartyInviteCharacterReq>(new C2SPartyPartyInviteCharacterReq()
            {
                CharacterId = targetClient.Character.CharacterId
            }));

            responses.Add(ChatResponse.ServerMessage(client, $"Party invite sent to {targetClient.Character.FirstName} {targetClient.Character.LastName}."));
        }
    }

    private HashSet<uint> BannedStageIds = new()
    {
        347
    };
}

return new ChatCommand();
