using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;
using System.Runtime;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterSwitchGameModeNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_SWITCH_GAME_MODE_NTC;

        public S2CCharacterSwitchGameModeNtc()
        {
            CharacterInfo = new CDataCharacterInfo();
        }

        public uint Unk0 { get; set; }
        public bool Unk1 { get; set; }
        public CDataCharacterInfo CharacterInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterSwitchGameModeNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterSwitchGameModeNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteBool(buffer, obj.Unk1);
                WriteEntity<CDataCharacterInfo>(buffer, obj.CharacterInfo);
            }

            public override S2CCharacterSwitchGameModeNtc Read(IBuffer buffer)
            {
                S2CCharacterSwitchGameModeNtc obj = new S2CCharacterSwitchGameModeNtc();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadBool(buffer);
                obj.CharacterInfo = ReadEntity<CDataCharacterInfo>(buffer);
                return obj;
            }
        }
    }
}
