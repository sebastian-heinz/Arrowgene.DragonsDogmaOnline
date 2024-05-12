using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterLevelParam
    {
        public ushort Attack { get; set; }
        public ushort MagAttack { get; set; }
        public ushort Defence { get; set; }
        public ushort MagDefence { get; set; }
        public ushort Strength { get; set; }
        public ushort DownPower { get; set; }
        public ushort ShakePower { get; set; }
        public ushort StunPower { get; set; }
        public ushort Constitution { get; set; }
        public ushort Guts { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterLevelParam>
        {
            public override void Write(IBuffer buffer, CDataCharacterLevelParam obj)
            {
                WriteUInt16(buffer, obj.Attack);
                WriteUInt16(buffer, obj.MagAttack);
                WriteUInt16(buffer, obj.Defence);
                WriteUInt16(buffer, obj.MagDefence);
                WriteUInt16(buffer, obj.Strength);
                WriteUInt16(buffer, obj.DownPower);
                WriteUInt16(buffer, obj.ShakePower);
                WriteUInt16(buffer, obj.StunPower);
                WriteUInt16(buffer, obj.Constitution);
                WriteUInt16(buffer, obj.Guts);
            }

            public override CDataCharacterLevelParam Read(IBuffer buffer)
            {
                CDataCharacterLevelParam obj = new CDataCharacterLevelParam();
                obj.Attack = ReadUInt16(buffer);
                obj.MagAttack = ReadUInt16(buffer);
                obj.Defence = ReadUInt16(buffer);
                obj.MagDefence = ReadUInt16(buffer);
                obj.Strength = ReadUInt16(buffer);
                obj.DownPower = ReadUInt16(buffer);
                obj.ShakePower = ReadUInt16(buffer);
                obj.StunPower = ReadUInt16(buffer);
                obj.Constitution = ReadUInt16(buffer);
                obj.Guts = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}