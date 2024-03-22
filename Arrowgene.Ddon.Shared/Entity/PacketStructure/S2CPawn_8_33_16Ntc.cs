using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawn_8_33_16Ntc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_8_33_16_NTC;

        public S2CPawn_8_33_16Ntc()
        {
            OwnerFirstName = string.Empty;
            OwnerLastName = string.Empty;
            OwnerClanName = string.Empty;
            PawnProfile = new CDataArisenProfile();
            Unk7 = string.Empty;
        }

        public uint CharacterId { get; set; } // Always the player character id?
        public uint PawnId { get; set; }
        public uint OwnerCharacterId { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerClanName { get; set; }
        public CDataArisenProfile PawnProfile { get; set; }
        public string Unk7 { get; set; } // Probably a profile description
        public uint Unk8 { get; set; } // Probably a profile setting

        public class Serializer : PacketEntitySerializer<S2CPawn_8_33_16Ntc>
        {
            public override void Write(IBuffer buffer, S2CPawn_8_33_16Ntc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.OwnerCharacterId);
                WriteMtString(buffer, obj.OwnerFirstName);
                WriteMtString(buffer, obj.OwnerLastName);
                WriteMtString(buffer, obj.OwnerClanName);
                WriteEntity<CDataArisenProfile>(buffer, obj.PawnProfile);
                WriteMtString(buffer, obj.Unk7);
                WriteUInt32(buffer, obj.Unk8);
            }

            public override S2CPawn_8_33_16Ntc Read(IBuffer buffer)
            {
                S2CPawn_8_33_16Ntc obj = new S2CPawn_8_33_16Ntc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.OwnerCharacterId = ReadUInt32(buffer);
                obj.OwnerFirstName = ReadMtString(buffer);
                obj.OwnerLastName = ReadMtString(buffer);
                obj.OwnerClanName = ReadMtString(buffer);
                obj.PawnProfile = ReadEntity<CDataArisenProfile>(buffer);
                obj.Unk7 = ReadMtString(buffer);
                obj.Unk8 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}