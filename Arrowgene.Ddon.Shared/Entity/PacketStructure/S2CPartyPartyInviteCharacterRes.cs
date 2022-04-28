using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteCharacterRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_CHARACTER_RES;

        public S2CPartyPartyInviteCharacterRes()
        {
            TimeoutSec = 0;
            Info = new CDataPartyListInfo();
        }

        public ushort TimeoutSec { get; set; } // In the PS4 version has no name and its not even used
        public CDataPartyListInfo Info { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteCharacterRes>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteCharacterRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt16(buffer, obj.TimeoutSec);
                WriteEntity<CDataPartyListInfo>(buffer, obj.Info);
            }

            public override S2CPartyPartyInviteCharacterRes Read(IBuffer buffer)
            {
                S2CPartyPartyInviteCharacterRes obj = new S2CPartyPartyInviteCharacterRes();
                ReadServerResponse(buffer, obj);
                obj.TimeoutSec = ReadUInt16(buffer);
                obj.Info = ReadEntity<CDataPartyListInfo>(buffer);
                return obj;
            }
        }
    }
}