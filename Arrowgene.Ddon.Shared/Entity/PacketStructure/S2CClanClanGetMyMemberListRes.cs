using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanGetMyMemberListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_GET_MY_MEMBER_LIST_RES;

        public S2CClanClanGetMyMemberListRes()
        {
            CharacterId = 0;
            FirstName = "";
            LastName = "";
        }

        public uint CharacterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public class Serializer : PacketEntitySerializer<S2CClanClanGetMyMemberListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanGetMyMemberListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, 1);
                WriteByteArray(buffer, new byte[]{ 0x0, 0x0, 0x0, 0x1,
                    0x0, 0x0, 0x0, 0x0, 0x5C, 0x68, 0x7C, 0xB8, 0x0, 0x0, 0x0, 0x0, 0x5D, 0x9A, 0x1B, 0xE4,
                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1F, 0xFE,}
                );
                WriteUInt32(buffer, obj.CharacterId);
                WriteMtString(buffer, obj.FirstName);
                WriteMtString(buffer, obj.LastName);
                WriteMtString(buffer, "Clan");
                WriteByteArray(buffer, new byte[]{    0x0, 0x5, 0x1, 0x5, 0x78, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x3, 0x0, 0x0, 0x0, 0x0, 0x5C, 0x68, 0x9B, 0x34, 0x0, 0x0}
                );
                
            }

            public override S2CClanClanGetMyMemberListRes Read(IBuffer buffer)
            {
                S2CClanClanGetMyMemberListRes obj = new S2CClanClanGetMyMemberListRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterId = ReadUInt32(buffer);
                obj.CharacterId = ReadUInt32(buffer);
                ReadUInt32(buffer);
                ReadUInt32(buffer);
                obj.FirstName = ReadMtString(buffer);
                obj.LastName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
