using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LCreateCharacterDataReq : IPacketStructure
    {
        public C2LCreateCharacterDataReq()
        {
            CharacterInfo = new CDataCharacterInfo();
            WaitNum = 0;
            RotationServerId = 0;
        }

        public PacketId Id => PacketId.C2L_CREATE_CHARACTER_DATA_REQ;

        public CDataCharacterInfo CharacterInfo { get; set; }
        public uint WaitNum { get; set; }
        public byte RotationServerId { get; set; }

        public class Serializer : PacketEntitySerializer<C2LCreateCharacterDataReq>
        {
            
            public override void Write(IBuffer buffer, C2LCreateCharacterDataReq obj)
            {
                WriteEntity(buffer, obj.CharacterInfo);
                WriteUInt32(buffer, obj.WaitNum);
                WriteByte(buffer, obj.RotationServerId);
            }

            public override C2LCreateCharacterDataReq Read(IBuffer buffer)
            {
                C2LCreateCharacterDataReq obj = new C2LCreateCharacterDataReq();
                obj.CharacterInfo = ReadEntity<CDataCharacterInfo>(buffer);
                obj.WaitNum = ReadUInt32(buffer);
                obj.RotationServerId = ReadByte(buffer);
                return obj;
            }
        }
    }
}
