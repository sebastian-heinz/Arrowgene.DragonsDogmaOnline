#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ChatSendTellMsgHandler : GameRequestPacketHandler<C2SChatSendTellMsgReq, S2CChatSendTellMsgRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ChatSendTellMsgHandler));

        public ChatSendTellMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CChatSendTellMsgRes Handle(GameClient client, C2SChatSendTellMsgReq request)
        {
            S2CChatSendTellMsgRes res = new S2CChatSendTellMsgRes();

            GameClient targetClient = Server.ClientLookup.GetClientByCharacterId(request.CharacterInfo.CharacterId);

            S2CLobbyChatMsgNotice noticeSource = new S2CLobbyChatMsgNotice
            {
                Type = LobbyChatMsgType.Tell,
                HandleId = client.Character.CharacterId,
                CharacterBaseInfo = request.CharacterInfo,
                MessageFlavor = request.MessageFlavor,
                PhrasesCategory = request.PhrasesCategory,
                PhrasesIndex = request.PhrasesIndex,
                Message = request.Message
            };
            client.Send(noticeSource);

            S2CLobbyChatMsgNotice noticeTarget = new S2CLobbyChatMsgNotice
            {
                Type = LobbyChatMsgType.Tell,
                HandleId = client.Character.CharacterId,
                CharacterBaseInfo = new CDataCommunityCharacterBaseInfo
                {
                    CharacterId = client.Character.CharacterId,
                    CharacterName = new CDataCharacterName
                    {
                        FirstName = client.Character.FirstName,
                        LastName = client.Character.LastName
                    },
                    // TODO: retrieve actual clan name
                    ClanName = ""
                },
                MessageFlavor = request.MessageFlavor,
                PhrasesCategory = request.PhrasesCategory,
                PhrasesIndex = request.PhrasesIndex,
                Message = request.Message
            };
            targetClient.Send(noticeTarget);

            res.CharacterBaseInfo = request.CharacterInfo;

            return res;
        }
    }
}
