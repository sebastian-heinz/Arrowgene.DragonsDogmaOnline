using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer
{
    /// <summary>
    /// Convenience methods for sending to a specific groups of clients,
    /// or special packets that could be handy.
    /// </summary>
    public class GameRouter
    {
        public void Send<TResStruct>(Client client, TResStruct res) where TResStruct : class, IPacketStructure, new ()
        {
            client.Send(res);
        }

        public void Send(ChatResponse response)
        {
            S2CLobbyChatMsgNotice notice = new S2CLobbyChatMsgNotice();
            notice.Type = response.Type;
            notice.MessageFlavor = response.MessageFlavor;
            notice.PhrasesCategory = response.PhrasesCategory;
            notice.PhrasesIndex = response.PhrasesIndex;
            notice.Message = response.Message;
            notice.CharacterBaseInfo.CharacterId = response.CharacterId;
            notice.CharacterBaseInfo.CharacterName.FirstName = response.FirstName;
            notice.CharacterBaseInfo.CharacterName.LastName = response.LastName;
            notice.CharacterBaseInfo.ClanName = response.ClanName;
            foreach (GameClient client in Unique(response.Recipients))
            {
                client.Send(notice);
            }
        }

        private List<T> GetClients<T>(List<T> clients, params T[] excepts)
        {
            if (excepts.Length == 0)
            {
                return clients;
            }

            foreach (T except in excepts)
            {
                clients.Remove(except);
            }

            return clients;
        }
        
        private List<T> Unique<T>(List<T> clients)
        {
            HashSet<T> unique = new HashSet<T>(clients);
            return new List<T>(unique);
        }
    }
}
