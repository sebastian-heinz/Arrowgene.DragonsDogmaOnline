using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Globalization;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ProfileSetMessageSetHandler : GameRequestPacketHandler<C2SProfileSetMessageSetReq, S2CProfileSetMessageSetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ProfileSetMessageSetHandler));

        public ProfileSetMessageSetHandler(DdonGameServer server) : base(server)
        {
        }

        public static readonly string ChatBubblePrefix = "\uF00D";

        public override S2CProfileSetMessageSetRes Handle(GameClient client, C2SProfileSetMessageSetReq request)
        {
            // Do some filtering to save on DB time.
            // If they've never set a message before, they'll have a blank list on their character and we need to write the whole list.
            // Otherwise, we can write just the changed entries.
            List<CDataCharacterMsgSet> changed = [];
            if (client.Character.MsgSetList.Count > 0)
            {
                for (int i = 0; i < request.MessageSetList.Count; i++)
                {
                    var oldSet = client.Character.MsgSetList[i];
                    var newSet = request.MessageSetList[i];

                    bool setChanged = newSet.MsgSetName != oldSet.MsgSetName;
                    var changedMessages = new List<CDataCharacterMessage>();

                    for (int j = 0; j < newSet.CharacterMessageList.Count; j++)
                    {
                        var oldMsg = oldSet.CharacterMessageList[j];
                        var newMsg = newSet.CharacterMessageList[j];

                        // Restore the weird chat bubble thing that you can't normally type.
                        if (newMsg.Message.Length > 0 && StringInfo.GetNextTextElement(newMsg.Message) != ChatBubblePrefix)
                        {
                            newMsg.Message = $"{ChatBubblePrefix}{newMsg.Message}";
                        }

                        if (oldMsg.Message != newMsg.Message
                            || oldMsg.Emotion != newMsg.Emotion
                            || oldMsg.EmotoChat != newMsg.EmotoChat)
                        {
                            changedMessages.Add(newMsg);
                        }
                    }

                    if (setChanged || changedMessages.Count > 0)
                    {
                        changed.Add(new CDataCharacterMsgSet
                        {
                            SetNo = newSet.SetNo,
                            MsgSetName = newSet.MsgSetName,
                            CharacterMessageList = changedMessages
                        });
                    }
                }
            }
            else
            {
                changed = request.MessageSetList;
            }

            client.Character.MsgSetList = request.MessageSetList;
            Server.Database.UpsertCommunicationSet(client.Character.CharacterId, changed);

            return new();
        }
    }
}
