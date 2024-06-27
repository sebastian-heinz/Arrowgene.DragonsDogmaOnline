using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentContentEntryNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_CONTENT_ENTRY_NTC;

        public S2CBattleContentContentEntryNtc()
        {
            Unk0 = new CDataBattleContentUnk0();
            Unk1 = new List<CDataBattleContentUnk2>();
        }

        public uint StageId { get; set; }
        public CDataBattleContentUnk0 Unk0 { get; set; }
        public List<CDataBattleContentUnk2> Unk1 {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentContentEntryNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentContentEntryNtc obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteEntity(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1);
            }

            public override S2CBattleContentContentEntryNtc Read(IBuffer buffer)
            {
                S2CBattleContentContentEntryNtc obj = new S2CBattleContentContentEntryNtc();
                obj.StageId = ReadUInt32(buffer);
                obj.Unk0 = ReadEntity<CDataBattleContentUnk0>(buffer);
                obj.Unk1 = ReadEntityList<CDataBattleContentUnk2>(buffer);
                return obj;
            }
        }
    }
}
