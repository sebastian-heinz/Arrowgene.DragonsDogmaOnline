using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MailMailSendHandler : GameRequestPacketHandler<C2SMailMailSendReq, S2CMailMailSendRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MailMailSendHandler));

        public MailMailSendHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMailMailSendRes Handle(GameClient client, C2SMailMailSendReq request)
        {
            MailMessage message = new()
            {
                MessageState = MailState.Unopened,
                SenderName = client.Character.CDataCharacterName.ToString(),
                Title = "",
                Body = request.MailText,
                SendDate = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                BaseInfo = client.Character.CDataCommunityCharacterBaseInfo
            };

            PacketQueue queue = new();
            List<CDataCommonU32> resultList = [];

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var id in request.CharacterIdList)
                {
                    var characterId = id.Value;

                    // Catch DB errors here because we don't want to DC players for sending mail, even if the mail doesn't succeed.
                    try
                    {
                        message.CharacterId = characterId;
                        message.MessageId = (ulong)Server.Database.InsertMailMessage(message, connection);

                        GameClient receiverClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                        if (receiverClient == null)
                        {
                            Server.RpcManager.AnnounceMail(client, message);
                        }
                        else
                        {
                            receiverClient.Enqueue(new S2CMailMailSendNtc()
                            {
                                MailInfo = message.ToCDataMailInfo(0)
                            }, queue);
                        }

                        resultList.Add(id);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Error when sending mail {client.Character.CharacterId} > {characterId} : {ex.Message}");
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_MAIL_SEND);
                    }
                }
            });

            queue.Send();
            
            return new()
            {
                CharacterIdList = resultList,
            };
        }
    }
}
