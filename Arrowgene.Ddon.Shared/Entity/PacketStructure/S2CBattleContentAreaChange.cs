using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentAreaChange : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_CONTENT_AREA_CHANGE_NTC;

        public S2CBattleContentAreaChange()
        {
            Unk5 = new List<CDataBattleContentUnk5>();
            Unk1 = string.Empty;
        }

        public uint Unk0 { get; set; }
        public string Unk1 { get; set; }
        public uint StageId { get; set; }
        public uint StartPos { get; set; }
        public bool Unk4 { get; set; }
        public List<CDataBattleContentUnk5> Unk5 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentAreaChange>
        {
            public override void Write(IBuffer buffer, S2CBattleContentAreaChange obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.StageId);
                WriteUInt32(buffer, obj.StartPos);
                WriteBool(buffer, obj.Unk4);
                WriteEntityList(buffer, obj.Unk5);
            }

            public override S2CBattleContentAreaChange Read(IBuffer buffer)
            {
                S2CBattleContentAreaChange obj = new S2CBattleContentAreaChange();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadMtString(buffer);
                obj.StageId = ReadUInt32(buffer);
                obj.StartPos = ReadUInt32(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.Unk5 = ReadEntityList<CDataBattleContentUnk5>(buffer);
                return obj;
            }
        }
    }
}


