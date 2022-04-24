using System;
namespace Arrowgene.Ddon.GameServer
{
    public class ClientConnectionChangeArgs: EventArgs
    {
        public enum EventType {
            CONNECT,
            DISCONNECT
        }

        public ClientConnectionChangeArgs(EventType eventType, GameClient client)
        {
            Action = eventType;
            Client = client;
        }

        public EventType Action;
        public GameClient Client;        
    }
}