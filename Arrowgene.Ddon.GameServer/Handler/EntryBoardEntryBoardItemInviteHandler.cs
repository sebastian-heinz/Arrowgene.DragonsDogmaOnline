using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Diagnostics.Metrics;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemInviteHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemInviteReq, S2CEntryBoardEntryBoardItemInviteRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemLeaveHandler));

        public EntryBoardEntryBoardItemInviteHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemInviteRes Handle(GameClient client, C2SEntryBoardEntryBoardItemInviteReq request)
        {
            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);
            foreach (var characterId in request.CharacterIds)
            {
                if (!data.Members.Contains(characterId.Value))
                {
                    var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId.Value);

                    var ntc = new S2CEntryBoardEntryBoardItemInviteNtc()
                    {
                        BoardId = data.BoardId,
                        ItemId = data.EntryItem.Id,
                        Comment = data.EntryItem.Param.Comment
                    };
                    GameStructure.CDataCharacterListElement(ntc.Character, client.Character);

                    memberClient.Send(ntc);
                }
            }
            return new S2CEntryBoardEntryBoardItemInviteRes();
        }
    }
}
