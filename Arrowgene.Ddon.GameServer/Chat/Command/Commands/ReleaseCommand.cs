using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Chat.Command.Commands
{
    public class ReleaseCommand : ChatCommand
    {
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "release";
        public override string HelpText => "usage: `/release` - Release all warp points";

        private DdonGameServer _server;

        public ReleaseCommand(DdonGameServer server)
        {
            _server = server;
        }

        public override void Execute(string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
        {
            List<ReleasedWarpPoint> allWarpPoints = _server.AssetRepository.WarpPoints.Select(wp => new ReleasedWarpPoint()
            {
                WarpPointId = wp.WarpPointId,
                // WDT must ALWAYS be the first favorite, otherwise the client doesn't behave properly
                FavoriteSlotNo = wp.WarpPointId == 1 ? 1u : 0u
            }).ToList();
            _server.Database.InsertIfNotExistsReleasedWarpPoints(client.Character.CharacterId, allWarpPoints);
            client.Character.ReleasedWarpPoints = _server.Database.SelectReleasedWarpPoints(client.Character.CharacterId);
            responses.Add(ChatResponse.CommandError(client, "all warp points unlocked"));
        }
    }
}