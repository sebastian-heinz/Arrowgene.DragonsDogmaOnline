using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyJoinHandler : GameRequestPacketHandler<C2SLobbyJoinReq, S2CLobbyJoinRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyJoinHandler));


        public LobbyLobbyJoinHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CLobbyJoinRes Handle(GameClient client, C2SLobbyJoinReq request)
        {
            client.Character.OnlineStatus = OnlineStatus.Online;

            // Notify new player of already present players
            S2CUserListJoinNtc alreadyPresentUsersNtc = new S2CUserListJoinNtc();
            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                if (otherClient != client && otherClient.Character != null)
                {
                    alreadyPresentUsersNtc.UserList.Add
                    (
                        new CDataLobbyMemberInfo()
                        {
                            CharacterId = otherClient.Character.CharacterId,
                            FirstName = otherClient.Character.FirstName,
                            LastName = otherClient.Character.LastName,
                            ClanName = otherClient.Character.ClanName.ShortName,
                            PawnId = 0,
                            Unk0 = 1,
                            Unk1 = 0,
                            OnlineStatus = OnlineStatus.Online
                        }
                    );
                }
            }

            client.Send(alreadyPresentUsersNtc);

            // Notify already present players of the new player
            S2CUserListJoinNtc newUserNtc = new S2CUserListJoinNtc();
            newUserNtc.UserList = new List<CDataLobbyMemberInfo>()
            {
                new CDataLobbyMemberInfo()
                {
                    CharacterId = client.Character.CharacterId,
                    FirstName = client.Character.FirstName,
                    LastName = client.Character.LastName,
                    ClanName = client.Character.ClanName.ShortName,
                    Unk0 = 1, // Platform PC?
                    Unk1 = 0,
                    OnlineStatus = OnlineStatus.Online  // OnlineStatus?
                },
            };

            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                if (otherClient != client)
                {
                    otherClient.Send(newUserNtc);
                }
            }

            Server.BazaarManager.NotifySoldExhibitions(client);

            var allUsers = newUserNtc.UserList.Concat(alreadyPresentUsersNtc.UserList).ToList();
            return new S2CLobbyJoinRes()
            {
                CharacterId = client.Character.CharacterId,
                LobbyMemberInfoList = allUsers
            };
        }
    }
}
