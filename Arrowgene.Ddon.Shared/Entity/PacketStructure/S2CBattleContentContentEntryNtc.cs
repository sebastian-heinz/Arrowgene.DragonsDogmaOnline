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
            Unk0 = new List<CDataBattleContentUnk3>();
        }

        public uint StageId { get; set; }
        public List<CDataBattleContentUnk3> Unk0 {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentContentEntryNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentContentEntryNtc obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteEntityList(buffer, obj.Unk0);
            }

            public override S2CBattleContentContentEntryNtc Read(IBuffer buffer)
            {
                S2CBattleContentContentEntryNtc obj = new S2CBattleContentContentEntryNtc();
                obj.StageId = ReadUInt32(buffer);
                obj.Unk0 = ReadEntityList<CDataBattleContentUnk3>(buffer);
                return obj;
            }
        }
    }
}
