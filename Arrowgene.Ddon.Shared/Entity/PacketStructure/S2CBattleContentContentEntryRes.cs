using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentContentEntryRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_CONTENT_ENTRY_RES;

        public class Serializer : PacketEntitySerializer<S2CBattleContentContentEntryRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentContentEntryRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CBattleContentContentEntryRes Read(IBuffer buffer)
            {
                S2CBattleContentContentEntryRes obj = new S2CBattleContentContentEntryRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}

