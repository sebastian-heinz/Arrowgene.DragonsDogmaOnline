using Arrowgene.Ddon.Shared.Entity;
namespace Arrowgene.Ddon.Server.Network
{
    public class StructurePacket<TStruct> : Packet where TStruct : IPacketStructure
    {
        private TStruct _structure;

        public StructurePacket(Packet packet) : base(packet.Id, packet.Data, packet.Source, packet.Count)
        {
            _structure = default;
        }

        public StructurePacket(TStruct structure) : base(structure.Id, null)
        {
            SetStructure(structure);
        }

        public TStruct Structure
        {
            get => GetStructure();
            set => SetStructure(value);
        }

        private void SetStructure(TStruct structure)
        {
            Data = EntitySerializer.Get<TStruct>().Write(structure);
            _structure = structure;
        }

        private TStruct GetStructure()
        {
            if (_structure == null)
            {
                _structure = EntitySerializer.Get<TStruct>().Read(AsBuffer());
            }

            return _structure;
        }
    }
}
