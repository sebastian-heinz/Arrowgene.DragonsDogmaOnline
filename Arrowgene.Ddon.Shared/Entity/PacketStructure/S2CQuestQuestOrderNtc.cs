using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestQuestOrderNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_QUEST_ORDER_NTC;

        public S2CQuestQuestOrderNtc()
        {
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestQuestOrderNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestQuestOrderNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override S2CQuestQuestOrderNtc Read(IBuffer buffer)
            {
                S2CQuestQuestOrderNtc obj = new S2CQuestQuestOrderNtc();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
