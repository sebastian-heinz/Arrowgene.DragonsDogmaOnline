using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextGetLobbyPlayerContextNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_GET_LOBBY_PLAYER_CONTEXT_NTC;

        public S2CContextGetLobbyPlayerContextNtc()
        {
            CharacterId=0;
            Context=new CDataLobbyContextPlayer();
        }

        public uint CharacterId { get; set; }
        public CDataLobbyContextPlayer Context { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextGetLobbyPlayerContextNtc>
        {
            public override void Write(IBuffer buffer, S2CContextGetLobbyPlayerContextNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataLobbyContextPlayer>(buffer, obj.Context);
            }

            public override S2CContextGetLobbyPlayerContextNtc Read(IBuffer buffer)
            {
                S2CContextGetLobbyPlayerContextNtc obj = new S2CContextGetLobbyPlayerContextNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Context = ReadEntity<CDataLobbyContextPlayer>(buffer);
                return obj;
            }
        }
    }
}