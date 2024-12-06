using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Server.Network
{
    public class PacketQueue : Queue<(Client Client, Packet Packet)>
    {
        public PacketQueue() : base() { }
        public PacketQueue(IEnumerable<(Client Client, Packet Packet)> collection) : base(collection) { }
        public void Send()
        {
            while (this.Any())
            {
                (var client, var packet) = this.Dequeue();
                client.Send(packet);
            }
        }

        public void Enqueue<TResStruct>(Client client, TResStruct res)
            where TResStruct : class, IPacketStructure, new()
        {
            StructurePacket<TResStruct> packet = new StructurePacket<TResStruct>(res);
            this.Enqueue((client, packet));
        }

        public void AddRange(IEnumerable<(Client Client, Packet Packet)> other)
        {
            foreach(var item in other)
            {
                this.Enqueue(item);
            }
        }
    }
}
