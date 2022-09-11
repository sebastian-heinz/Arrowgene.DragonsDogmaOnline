using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Model
{
    public interface IPartyMember
    {
        Character Character { get; set; }
        Party Party { get; set; }

        void Send<TResStruct>(TResStruct res) where TResStruct : class, IPacketStructure, new();
        void Send(Packet packet);
    }
}