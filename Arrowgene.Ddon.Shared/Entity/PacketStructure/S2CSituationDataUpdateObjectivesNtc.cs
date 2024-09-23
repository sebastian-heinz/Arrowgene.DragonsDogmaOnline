using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSituationDataUpdateObjectivesNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_63_2_16_NTC;

        public S2CSituationDataUpdateObjectivesNtc()
        {
            ObjectiveList = new List<CDataSituationObjective>();
        }

        public bool Unk0 {  get; set; }
        public byte Unk1 {  get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 {  get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }
        public List<CDataSituationObjective> ObjectiveList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSituationDataUpdateObjectivesNtc>
        {
            public override void Write(IBuffer buffer, S2CSituationDataUpdateObjectivesNtc obj)
            {
                WriteBool(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
                WriteUInt32(buffer, obj.Unk5);
                WriteEntityList(buffer, obj.ObjectiveList);
            }

            public override S2CSituationDataUpdateObjectivesNtc Read(IBuffer buffer)
            {
                S2CSituationDataUpdateObjectivesNtc obj = new S2CSituationDataUpdateObjectivesNtc();
                obj.Unk0 = ReadBool(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                obj.ObjectiveList = ReadEntityList<CDataSituationObjective>(buffer);
                return obj;
            }
        }
    }
}
