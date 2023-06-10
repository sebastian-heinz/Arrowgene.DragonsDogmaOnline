using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetPackageQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_PACKAGE_QUEST_LIST_REQ;

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestGetPackageQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetPackageQuestListReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SQuestGetPackageQuestListReq Read(IBuffer buffer)
            {
                C2SQuestGetPackageQuestListReq obj = new C2SQuestGetPackageQuestListReq();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}