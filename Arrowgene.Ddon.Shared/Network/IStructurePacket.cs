using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.Shared.Network
{
    public interface IStructurePacket<TStruct> : IStructurePacket where TStruct : class, IPacketStructure, new()
    {
        public TStruct Structure { get; set; }
    }

    /// <summary>
    /// StructurePacket is defined as a packet, that contains a accessible structure of properties
    /// </summary>
    public interface IStructurePacket : IPacket
    {
        string PrintStructure();
    }
}
