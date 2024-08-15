using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterDecideCharacterIdRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_DECIDE_CHARACTER_ID_RES;

        public S2CCharacterDecideCharacterIdRes()
        {
            CharacterId = 0;
            CharacterInfo = new CDataCharacterInfo();
            IsReturnPrepare = false;
            BinaryData = new byte[C2SBinarySaveSetCharacterBinSaveDataReq.ARRAY_SIZE];
            Unk1 = 0;
        }

        public uint CharacterId { get; set; }
        public CDataCharacterInfo CharacterInfo { get; set; }
        public bool IsReturnPrepare { get; set; }
        public byte[] BinaryData { get; set; }
        public byte Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterDecideCharacterIdRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterDecideCharacterIdRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataCharacterInfo>(buffer, obj.CharacterInfo);
                WriteBool(buffer, obj.IsReturnPrepare);
                WriteByteArray(buffer, obj.BinaryData);
                WriteByte(buffer, obj.Unk1);
            }

            public override S2CCharacterDecideCharacterIdRes Read(IBuffer buffer)
            {
                S2CCharacterDecideCharacterIdRes obj = new S2CCharacterDecideCharacterIdRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterId = ReadUInt32(buffer);
                obj.CharacterInfo = ReadEntity<CDataCharacterInfo>(buffer);
                obj.IsReturnPrepare = ReadBool(buffer);
                obj.BinaryData = ReadByteArray(buffer, C2SBinarySaveSetCharacterBinSaveDataReq.ARRAY_SIZE);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
