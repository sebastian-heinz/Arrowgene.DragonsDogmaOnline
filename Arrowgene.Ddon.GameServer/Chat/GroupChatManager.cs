using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.GameServer.Chat
{
    public class GroupChatManager(DdonGameServer Server)
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GroupChatManager));

        public List<CDataCharacterListElement> GetGroupChatMembers(ulong groupId, DbConnection? connectionIn = null)
        {
            if (groupId == 0)
            {
                return [];
            }

            return Server.Database.SelectGroupChatMembers(groupId, connectionIn);
        }

        public bool JoinGroupChatOnLogin(GameClient client, out PacketQueue queue, DbConnection? connectionIn = null)
        {
            queue = new();

            // Group Chat
            (ulong groupId, string groupName) = Server.Database.SelectGroupChatId(client.Character.CharacterId, connectionIn);
            client.Character.GroupChatId = groupId;

            if (groupId == 0)
            {
                return false;
            }

            S2CGroupChatInviteCharacterNtc invitePacket = new()
            {
                GroupId = groupId,
                InviterInfo = new()
                {
                    CharacterId = 0,
                    CharacterName = new()
                    {
                        FirstName = $"\"{groupName}\"",
                        LastName = ""
                    }
                },
                VisitorInfo = client.Character.CDataCharacterListElement
            };

            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                if (groupId != 0 && otherClient?.Character.GroupChatId == groupId)
                {
                    otherClient.Enqueue(invitePacket, queue);
                }
            }

            Server.RpcManager.AnnounceOthers("internal/chat",
                RpcInternalCommand.JoinGroupChat,
                RpcPacketData.FromPacket(invitePacket, client.Character.CharacterId, client.Character.ClanId)
            );

            return true;
        }

        public bool JoinGroupChatByName(GameClient client, string name, out PacketQueue queue, DbConnection? connectionIn = null)
        {
            queue = new();

            (ulong groupId, string groupName) = Server.Database.SelectGroupChatName(name, connectionIn);

            if (groupId == 0)
            {
                return false;
            }

            if (LeaveGroupChat(client, out var leaveQueue, connectionIn))
            {
                queue.AddRange(leaveQueue);
            }

            client.Character.GroupChatId = groupId;
            Server.Database.InsertGroupChatMember(client.Character.CharacterId, groupId, connectionIn);

            var invitePacket = new S2CGroupChatInviteCharacterNtc()
            {
                GroupId = groupId,
                InviterInfo = new()
                {
                    CharacterId = 0,
                    CharacterName = new()
                    {
                        FirstName = $"\"{groupName}\"",
                        LastName = ""
                    }
                },
                VisitorInfo = client.Character.CDataCharacterListElement
            };

            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                if (groupId != 0 && otherClient?.Character.GroupChatId == groupId)
                {
                    otherClient.Enqueue(invitePacket, queue);
                }
            }

            Server.RpcManager.AnnounceOthers("internal/chat", 
                RpcInternalCommand.JoinGroupChat, 
                RpcPacketData.FromPacket(invitePacket, client.Character.CharacterId, client.Character.ClanId)
            );

            return true;
        }

        /// <summary>
        /// Announces the client leaving the group to properly maintain member lists held by otehr clients.
        /// Does not write to the DB.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public bool LeaveGroupChatOnDisconnect(GameClient client, out PacketQueue queue)
        {
            queue = new();

            if (client.Character.GroupChatId == 0)
            {
                return false;
            }

            S2CGroupChatLeaveCharacterNtc leavePacket = new()
            {
                GroupId = client.Character.GroupChatId,
                LeaveCharacterId = client.Character.CharacterId
            };

            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                if (otherClient == client) continue;

                if (otherClient?.Character.GroupChatId == client.Character.GroupChatId)
                {
                    otherClient.Enqueue(leavePacket, queue);
                }
            }

            Server.RpcManager.AnnounceOthers("internal/chat",
                RpcInternalCommand.LeaveGroupChat,
                RpcPacketData.FromPacket(leavePacket, client.Character.CharacterId, client.Character.ClanId)
            );

            return true;
        }

        /// <summary>
        /// Announces the client leaving the group, and also writes to the DB, permanently leaving the group chat.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="queue"></param>
        /// <param name="connectionIn"></param>
        /// <returns></returns>
        public bool LeaveGroupChat(GameClient client, out PacketQueue queue, DbConnection? connectionIn = null)
        {
            queue = new();

            if (LeaveGroupChatOnDisconnect(client, out var leaveQueue))
            {
                queue.AddRange(leaveQueue);
            }
            else
            {
                return false;
            }

            client.Character.GroupChatId = 0;
            Server.Database.DeleteGroupChatMember(client.Character.CharacterId, connectionIn);
            
            return true;
        }

        public bool CreateGroupChat(GameClient client, string name, string desc, out PacketQueue queue, DbConnection? connectionIn = null)
        {
            queue = new();

            (ulong groupId, string groupName) = Server.Database.SelectGroupChatName(name, connectionIn);

            if (groupId != 0)
            {
                return false;
            }

            if (LeaveGroupChat(client, out var leaveQueue, connectionIn))
            {
                queue.AddRange(leaveQueue);
            }

            client.Character.GroupChatId = (ulong)Server.Database.InsertGroupChatGroup(name, desc, connectionIn);
            Server.Database.InsertGroupChatMember(client.Character.CharacterId, client.Character.GroupChatId, connectionIn);

            client.Enqueue(new S2CGroupChatInviteCharacterNtc()
            {
                GroupId = client.Character.GroupChatId,
                InviterInfo = new()
                {
                    CharacterId = 0,
                    CharacterName = new()
                    {
                        FirstName = $"\"{name}\"",
                        LastName = ""
                    }
                },
                VisitorInfo = client.Character.CDataCharacterListElement
            }, queue);

            return true;
        }
    }
}
