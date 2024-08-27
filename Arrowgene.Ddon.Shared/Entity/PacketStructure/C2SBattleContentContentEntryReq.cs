using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentContentEntryReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_CONTENT_ENTRY_REQ;

        public C2SBattleContentContentEntryReq()
        {
        }

        public uint Unk0 { get; set; } // Comes from S2CBattleContentGetContentStatusFromOmRes.Unk0
        public uint Unk1 { get; set; } // Comes from S2CBattleContentGetContentStatusFromOmRes.Unk2

        public class Serializer : PacketEntitySerializer<C2SBattleContentContentEntryReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentContentEntryReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override C2SBattleContentContentEntryReq Read(IBuffer buffer)
            {
                C2SBattleContentContentEntryReq obj = new C2SBattleContentContentEntryReq();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}

