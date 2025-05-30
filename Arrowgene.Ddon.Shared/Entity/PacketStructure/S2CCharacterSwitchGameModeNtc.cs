using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterSwitchGameModeNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_SWITCH_GAME_MODE_NTC;

        public S2CCharacterSwitchGameModeNtc()
        {
            CharacterInfo = new CDataCharacterInfo();
        }

        public GameMode GameMode { get; set; }
        public bool CreateCharacter { get; set; }
        public CDataCharacterInfo CharacterInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterSwitchGameModeNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterSwitchGameModeNtc obj)
            {
                WriteUInt32(buffer, (uint) obj.GameMode);
                WriteBool(buffer, obj.CreateCharacter);
                WriteEntity<CDataCharacterInfo>(buffer, obj.CharacterInfo);
            }

            public override S2CCharacterSwitchGameModeNtc Read(IBuffer buffer)
            {
                S2CCharacterSwitchGameModeNtc obj = new S2CCharacterSwitchGameModeNtc();
                obj.GameMode = (GameMode) ReadUInt32(buffer);
                obj.CreateCharacter = ReadBool(buffer);
                obj.CharacterInfo = ReadEntity<CDataCharacterInfo>(buffer);
                return obj;
            }
        }
    }
}
