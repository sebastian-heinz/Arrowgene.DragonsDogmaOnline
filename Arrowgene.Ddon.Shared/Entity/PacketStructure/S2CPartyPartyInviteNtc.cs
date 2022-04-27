using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyPartyInviteNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_PARTY_INVITE_NTC;

        public S2CPartyPartyInviteNtc()
        {
            PartyListInfo = new CDataPartyListInfo();
            TimeoutSec = 0;
        }

        public CDataPartyListInfo PartyListInfo { get; set; }
        public ushort TimeoutSec { get; set; } // It COULD be Error, since that value is in the PS4 but missing in the PC, but its unlikely due to its position

        public class Serializer : PacketEntitySerializer<S2CPartyPartyInviteNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyPartyInviteNtc obj)
            {
                WriteEntity<CDataPartyListInfo>(buffer, obj.PartyListInfo);
                WriteUInt16(buffer, obj.TimeoutSec);
            }

            public override S2CPartyPartyInviteNtc Read(IBuffer buffer)
            {
                S2CPartyPartyInviteNtc obj = new S2CPartyPartyInviteNtc();
                obj.PartyListInfo = ReadEntity<CDataPartyListInfo>(buffer);
                obj.TimeoutSec = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}