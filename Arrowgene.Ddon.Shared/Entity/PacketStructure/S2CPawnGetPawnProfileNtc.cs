using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetPawnProfileNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_PAWN_PROFILE_NTC;

        public S2CPawnGetPawnProfileNtc()
        {
            OwnerFirstName = string.Empty;
            OwnerLastName = string.Empty;
            OwnerClanName = string.Empty;
            PawnProfile = new CDataArisenProfile();
            Unk7 = string.Empty;
        }

        public uint CharacterId { get; set; } // Always the player character id?
        public uint PawnId { get; set; }

        // CDataPawnProfile
        //     CDataCommunityCharacterBaseInfo
        //     CDataArisenProfile
        //     Comment
        //     RentalCost
        public uint OwnerCharacterId { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public string OwnerClanName { get; set; }
        public CDataArisenProfile PawnProfile { get; set; }
        public string Unk7 { get; set; } // Probably a profile description (Comment)
        public uint Unk8 { get; set; } // Probably a profile setting (RentalCost)

        public class Serializer : PacketEntitySerializer<S2CPawnGetPawnProfileNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnGetPawnProfileNtc obj)
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

            public override S2CPawnGetPawnProfileNtc Read(IBuffer buffer)
            {
                S2CPawnGetPawnProfileNtc obj = new S2CPawnGetPawnProfileNtc();
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
