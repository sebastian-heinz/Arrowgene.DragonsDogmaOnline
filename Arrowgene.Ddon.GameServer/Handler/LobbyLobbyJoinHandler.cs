using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyJoinHandler : StructurePacketHandler<GameClient, C2SLobbyJoinReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyJoinHandler));


        public LobbyLobbyJoinHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SLobbyJoinReq> packet)
        {
            client.Character.OnlineStatus = OnlineStatus.Online;
            var resp = new S2CLobbyJoinRes()
            {
                CharacterId = client.Character.CharacterId,
                LobbyMemberInfoList = new List<CDataLobbyMemberInfo>()
                {
                    new CDataLobbyMemberInfo()
                    {
                        CharacterId = client.Character.CharacterId,
                        FirstName = client.Character.FirstName,
                        LastName = client.Character.LastName,
                        ClanName = "",
                        Unk0 = 1, // Platform PC?
                        Unk1 = 0,
                        OnlineStatus = OnlineStatus.Online  // OnlineStatus?
                    },
                }
            };
            client.Send(resp);

            // Notify new player of already present players
            S2CUserListJoinNtc alreadyPresentUsersNtc = new S2CUserListJoinNtc();
            List<S2CContextGetLobbyPlayerContextNtc> alreadyPresentPlayerContextNtcs =
                new List<S2CContextGetLobbyPlayerContextNtc>();
            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                if (otherClient != client)
                {
                    alreadyPresentUsersNtc.UserList.Add
                    (
                        new CDataLobbyMemberInfo()
                        {
                            CharacterId = otherClient.Character.CharacterId,
                            FirstName = otherClient.Character.FirstName,
                            LastName = otherClient.Character.LastName,
                            ClanName = "",
                            PawnId = 0,
                            Unk0 = 1,
                            Unk1 = 0,
                            OnlineStatus = OnlineStatus.Online
                        }
                    );

                    S2CContextGetLobbyPlayerContextNtc lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();
                    GameStructure.S2CContextGetLobbyPlayerContextNtc(lobbyPlayerContextNtc, otherClient.Character);
                    alreadyPresentPlayerContextNtcs.Add(lobbyPlayerContextNtc);
                }
            }

            client.Send(alreadyPresentUsersNtc);

            foreach (S2CContextGetLobbyPlayerContextNtc alreadyPresentPlayerContextNtc in
                    alreadyPresentPlayerContextNtcs)
            {
                client.Send(alreadyPresentPlayerContextNtc);
            }

            // Notify already present players of the new player
            S2CUserListJoinNtc newUserNtc = new S2CUserListJoinNtc();
            newUserNtc.UserList = resp.LobbyMemberInfoList;

            S2CContextGetLobbyPlayerContextNtc newUserContextNtc = new S2CContextGetLobbyPlayerContextNtc();
            GameStructure.S2CContextGetLobbyPlayerContextNtc(newUserContextNtc, client.Character);
            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                if (otherClient != client)
                {
                    otherClient.Send(newUserNtc);
                    otherClient.Send(newUserContextNtc);
                }
            }
        }
    }
}
