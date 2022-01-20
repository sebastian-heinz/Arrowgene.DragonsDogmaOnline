using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LDecideCharacterIdReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2L_DECIDE_CHARACTER_ID_REQ;

        public uint CharacterId { get; set; }
        public uint ClientVersion { get; set; }
        public byte Type { get; set; }
        public byte RotationServerId { get; set; }
        public uint WaitNum { get; set; }
        public byte Counter { get; set; }


        public class Serializer : EntitySerializer<C2LDecideCharacterIdReq>
        {
            public override void Write(IBuffer buffer, C2LDecideCharacterIdReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.ClientVersion);
                WriteByte(buffer, obj.Type);
                WriteByte(buffer, obj.RotationServerId);
                WriteUInt32(buffer, obj.WaitNum);
                WriteByte(buffer, obj.Counter);
            }

            public override C2LDecideCharacterIdReq Read(IBuffer buffer)
            {
                C2LDecideCharacterIdReq obj = new C2LDecideCharacterIdReq();

                obj.CharacterId = buffer.ReadUInt32(Endianness.Big);
                obj.ClientVersion = buffer.ReadUInt32(Endianness.Big);
                obj.Type = buffer.ReadByte();
                obj.RotationServerId = buffer.ReadByte();
                obj.WaitNum = buffer.ReadUInt32(Endianness.Big);
                obj.Counter = buffer.ReadByte();
                return obj;
            }
        }
    }
}
