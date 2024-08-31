using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemLeaveHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemLeaveReq, S2CEntryBoardEntryBoardItemLeaveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemLeaveHandler));

        public EntryBoardEntryBoardItemLeaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemLeaveRes Handle(GameClient client, C2SEntryBoardEntryBoardItemLeaveReq request)
        {
            var contentId = Server.ExmManager.GetContentIdForCharacter(client.Character);
            var data = Server.ExmManager.GetEntryItemDataForContent(contentId);
            var characterIds = Server.ExmManager.GetCharacterIdsForContent(contentId);

            if (data.PartyLeaderCharacterId == client.Character.CharacterId)
            {
                Server.ExmManager.RemoveGroupForContent(contentId);
                foreach (var characterId in characterIds)
                {
                    var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                    if (memberClient != null)
                    {
                        memberClient.Send(new S2CEntryBoardEntryBoardItemLeaveNtc());
                    }
                }
            }
            else
            {
                // TODO: This might be wrong
                client.Send(new S2CEntryBoardEntryBoardItemLeaveNtc());
                var leaderClient = Server.ClientLookup.GetClientByCharacterId(data.PartyLeaderCharacterId);
                leaderClient.Send(new S2CEntryBoardEntryBoardItemLeaveNtc());
            }

            return new S2CEntryBoardEntryBoardItemLeaveRes();
        }
    }
}
