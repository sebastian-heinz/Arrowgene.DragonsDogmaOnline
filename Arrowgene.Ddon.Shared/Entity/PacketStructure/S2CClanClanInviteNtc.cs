using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanInviteNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_INVITE_NTC;

        public uint ClanId;
        public string ClanName;
        public CDataCharacterListElement CharacterListElement;

        public S2CClanClanInviteNtc()
        {
            ClanName = string.Empty;
            CharacterListElement = new CDataCharacterListElement();
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanInviteNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanInviteNtc obj)
            {
                WriteUInt32(buffer, obj.ClanId);
                WriteMtString(buffer, obj.ClanName);
                WriteEntity<CDataCharacterListElement>(buffer, obj.CharacterListElement);
            }

            public override S2CClanClanInviteNtc Read(IBuffer buffer)
            {
                S2CClanClanInviteNtc obj = new S2CClanClanInviteNtc();

                obj.ClanId = ReadUInt32(buffer);
                obj.ClanName = ReadMtString(buffer);
                obj.CharacterListElement = ReadEntity<CDataCharacterListElement>(buffer);

                return obj;
            }
        }
    }
}
