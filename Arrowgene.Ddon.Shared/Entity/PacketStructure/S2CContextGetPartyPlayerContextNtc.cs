using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextGetPartyPlayerContextNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_GET_PARTY_PLAYER_CONTEXT_NTC;

        public S2CContextGetPartyPlayerContextNtc()
        {
            CharacterId = 0;
            Context = new CDataPartyPlayerContext();
        }

        public uint CharacterId { get; set; }
        public CDataPartyPlayerContext Context { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextGetPartyPlayerContextNtc>
        {
            public override void Write(IBuffer buffer, S2CContextGetPartyPlayerContextNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataPartyPlayerContext>(buffer, obj.Context);
            }

            public override S2CContextGetPartyPlayerContextNtc Read(IBuffer buffer)
            {
                S2CContextGetPartyPlayerContextNtc obj = new S2CContextGetPartyPlayerContextNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Context = ReadEntity<CDataPartyPlayerContext>(buffer);
                return obj;
            }
        }
    }
}
