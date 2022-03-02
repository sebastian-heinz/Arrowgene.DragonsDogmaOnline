using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CParty_6_8_16_Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_6_8_16_NTC;


        public S2CParty_6_8_16_Ntc()
        {
            CharacterId = 0;
            FirstName = "";
            LastName = "";
            ClanName = "";
        }

        public uint CharacterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClanName { get; set; }

        public class Serializer : PacketEntitySerializer<S2CParty_6_8_16_Ntc>
        {
            // Reference: InGameDump.Dump_103
            // 00 20 4F D8 - perhaps targer?
            // 00 20 4F D8 - perhaps leader?

            // 00 00 00 01 - perhaps list? 

            // 00 20 4F D8 - char id
            // 00 04 52 75 6D 69 - first name
            // 00 08 41 6D 65 73 74 72 69 73 - last name

            // 00 03 53 3B 52 -- clan name
            // 00 05 01 
            // 05 78 00 00 
            // 00 00 01 01
            // 00 00 00 00 
            // 00 00 00 00 
            // 01 00 00 02 
            // 00 00 00 00
            // 00 00 00 00 
            // 00 D5 5A    

            public override void Write(IBuffer buffer, S2CParty_6_8_16_Ntc obj)
            {
                WriteByte(buffer, 0);
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, 0x00000001);
                WriteUInt32(buffer, obj.CharacterId);
                WriteMtString(buffer, obj.FirstName);
                WriteMtString(buffer, obj.LastName);
                WriteMtString(buffer, obj.ClanName);
                buffer.WriteBytes(new byte[]
                {
                    0x0, 0x5, 0x1, 0x5, 0x78, 0x0, 0x0, 0x0, 0x0, 0x1, 0x1,
                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x2, 0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0, 0x0
                });
            }

            public override S2CParty_6_8_16_Ntc Read(IBuffer buffer)
            {
                S2CParty_6_8_16_Ntc obj = new S2CParty_6_8_16_Ntc();
                ReadByte(buffer);
                ReadUInt32(buffer);
                ReadUInt32(buffer);
                ReadUInt32(buffer);
                obj.CharacterId = ReadUInt32(buffer);
                obj.FirstName = ReadMtString(buffer);
                obj.LastName = ReadMtString(buffer);
                obj.ClanName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
