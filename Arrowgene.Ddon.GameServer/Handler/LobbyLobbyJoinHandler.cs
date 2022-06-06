using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            var resp = new S2CLobbyJoinRes()
            {
                CharacterId = client.Character.Id,
                LobbyMemberInfoList = new List<CDataLobbyMemberInfo>()
                {
                    new CDataLobbyMemberInfo()
                    {
                        CharacterId = client.Character.Id,
                        FirstName = client.Character.FirstName,
                        LastName = client.Character.LastName,
                        ClanName = "ABC",
                        Unk0 = 1, // Platform PC
                        Unk1 = 0,
                        Unk2 = 8  // OnlineStatus
                    },
                }
            };
            client.Send(resp);

            // NTC
            //client.Send(GameFull.Dump_14);
            //client.Send(InGameDump.Dump_15);
            //client.Send(InGameDump.Dump_16);

            // Notify new player of already present players
            S2CUserListJoinNtc alreadyPresentUsersNtc = new S2CUserListJoinNtc();
            List<S2CContextGetLobbyPlayerContextNtc> alreadyPresentPlayerContextNtcs =
                new List<S2CContextGetLobbyPlayerContextNtc>();
            foreach (GameClient otherClient in Server.Clients)
            {
                if (otherClient != client)
                {
                    alreadyPresentUsersNtc.UserList.Add
                    (
                        new CDataLobbyMemberInfo()
                        {
                            CharacterId = otherClient.Character.Id,
                            FirstName = otherClient.Character.FirstName,
                            LastName = otherClient.Character.LastName,
                            ClanName = "XYZ",
                            PawnId = 0,
                            Unk0 = 1,
                            Unk1 = 0,
                            Unk2 = 8
                        }
                    );

                    S2CContextGetLobbyPlayerContextNtc alreadyPresentPlayerContextNtc = EntitySerializer
                        .Get<S2CContextGetLobbyPlayerContextNtc>().Read(SelectedDump.data_Dump_LobbyPlayerContext);
                    alreadyPresentPlayerContextNtc.CharacterId = otherClient.Character.Id;
                    alreadyPresentPlayerContextNtc.Context.Base.CharacterId = otherClient.Character.Id;
                    alreadyPresentPlayerContextNtc.Context.Base.FirstName = otherClient.Character.FirstName;
                    alreadyPresentPlayerContextNtc.Context.Base.LastName = otherClient.Character.LastName;
                    alreadyPresentPlayerContextNtc.Context.Base.PosX = 0;
                    alreadyPresentPlayerContextNtc.Context.Base.PosY = 0;
                    alreadyPresentPlayerContextNtc.Context.Base.PosZ = 0;
                    alreadyPresentPlayerContextNtc.Context.EditInfo = otherClient.Character.Visual;
                    alreadyPresentPlayerContextNtcs.Add(alreadyPresentPlayerContextNtc);
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

            S2CContextGetLobbyPlayerContextNtc newUserContextNtc = EntitySerializer
                .Get<S2CContextGetLobbyPlayerContextNtc>().Read(SelectedDump.data_Dump_LobbyPlayerContext);
            newUserContextNtc.CharacterId = client.Character.Id;
            newUserContextNtc.Context.Base.CharacterId = client.Character.Id;
            newUserContextNtc.Context.Base.FirstName = client.Character.FirstName;
            newUserContextNtc.Context.Base.LastName = client.Character.LastName;
            newUserContextNtc.Context.Base.PosX = 0;
            newUserContextNtc.Context.Base.PosY = 0;
            newUserContextNtc.Context.Base.PosZ = 0;
            newUserContextNtc.Context.EditInfo = client.Character.Visual;

            foreach (GameClient otherClient in Server.Clients)
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
