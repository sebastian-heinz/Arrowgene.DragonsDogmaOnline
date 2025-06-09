using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageDungeonStartNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_STAGE_DUNGEON_START_NTC; // This packet looks identical to BBM packet except for packet ID

        public S2CStageDungeonStartNtc()
        {
            EntryCostList = new List<CDataBattleContentUnk5>();
            Unk1 = string.Empty;
        }

        public uint Unk0 { get; set; }
        public string Unk1 { get; set; }
        public uint StageId { get; set; }
        public uint StartPos { get; set; }
        public bool Unk4 { get; set; }
        public List<CDataBattleContentUnk5> EntryCostList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CStageDungeonStartNtc>
        {
            public override void Write(IBuffer buffer, S2CStageDungeonStartNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.StageId);
                WriteUInt32(buffer, obj.StartPos);
                WriteBool(buffer, obj.Unk4);
                WriteEntityList(buffer, obj.EntryCostList);
            }

            public override S2CStageDungeonStartNtc Read(IBuffer buffer)
            {
                S2CStageDungeonStartNtc obj = new S2CStageDungeonStartNtc();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadMtString(buffer);
                obj.StageId = ReadUInt32(buffer);
                obj.StartPos = ReadUInt32(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.EntryCostList = ReadEntityList<CDataBattleContentUnk5>(buffer);
                return obj;
            }
        }
    }
}


