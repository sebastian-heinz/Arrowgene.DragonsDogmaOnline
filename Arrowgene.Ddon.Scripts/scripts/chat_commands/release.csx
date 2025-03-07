public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.Admin;
    public override string CommandName            => "release";
    public override string HelpText               => "usage: `/release` - Release all warp points";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
            List<ReleasedWarpPoint> allWarpPoints = server.AssetRepository.WarpPoints.Select(wp => new ReleasedWarpPoint()
            {
                WarpPointId = wp.WarpPointId,
                // WDT must ALWAYS be the first favorite, otherwise the client doesn't behave properly
                FavoriteSlotNo = wp.WarpPointId == 1 ? 1u : 0u
            }).ToList();
            server.Database.InsertIfNotExistsReleasedWarpPoints(client.Character.CharacterId, allWarpPoints);
            client.Character.ReleasedWarpPoints = server.Database.SelectReleasedWarpPoints(client.Character.CharacterId);
            responses.Add(ChatResponse.CommandError(client, "all warp points unlocked"));
    }
}

return new ChatCommand();