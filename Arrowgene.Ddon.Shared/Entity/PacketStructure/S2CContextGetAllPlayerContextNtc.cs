using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextGetAllPlayerContextNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_GET_ALL_PLAYER_CONTEXT_NTC;

        public S2CContextGetAllPlayerContextNtc()
        {
            CharacterId = 0;
            Context = new CDataAllPlayerContext();
        }

        public uint CharacterId { get; set; }
        public CDataAllPlayerContext Context { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextGetAllPlayerContextNtc>
        {
            public override void Write(IBuffer buffer, S2CContextGetAllPlayerContextNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataAllPlayerContext>(buffer, obj.Context);
            }

            public override S2CContextGetAllPlayerContextNtc Read(IBuffer buffer)
            {
                S2CContextGetAllPlayerContextNtc obj = new S2CContextGetAllPlayerContextNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Context = ReadEntity<CDataAllPlayerContext>(buffer);
                return obj;
            }
        }

    }
}