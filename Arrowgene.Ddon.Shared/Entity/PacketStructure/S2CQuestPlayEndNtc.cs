using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayEndNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_PLAY_END_NTC;

        public S2CQuestPlayEndNtc()
        {
            ContentsPlayEnd = new CDataContentsPlayEnd();
        }

        public int Unk0 {  get; set; }
        public CDataContentsPlayEnd ContentsPlayEnd {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayEndNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayEndNtc obj)
            {
                WriteInt32(buffer, obj.Unk0);
                WriteEntity(buffer, obj.ContentsPlayEnd);
            }

            public override S2CQuestPlayEndNtc Read(IBuffer buffer)
            {
                S2CQuestPlayEndNtc obj = new S2CQuestPlayEndNtc();
                obj.Unk0 = ReadInt32(buffer);
                obj.ContentsPlayEnd = ReadEntity<CDataContentsPlayEnd>(buffer);
                return obj;
            }
        }
    }
}
