using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Model
{
    public interface IPartyMember
    {
        Character Character { get; set; }
        Party Party { get; set; }

        CDataPartyMember AsCDataPartyMember();
        Packet AsContextPacket();

        void Send<TResStruct>(TResStruct res) where TResStruct : class, IPacketStructure, new();
        void Send(Packet packet);
    }
}