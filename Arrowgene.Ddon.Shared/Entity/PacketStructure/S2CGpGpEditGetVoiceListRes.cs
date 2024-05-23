using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpGpEditGetVoiceListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GP_EDIT_GET_VOICE_LIST_RES;

        public S2CGpGpEditGetVoiceListRes()
        {
            VoiceList = new List<CDataEditParam>();
        }

        public List<CDataEditParam> VoiceList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGpGpEditGetVoiceListRes>
        {
            public override void Write(IBuffer buffer, S2CGpGpEditGetVoiceListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataEditParam>(buffer, obj.VoiceList);
            }

            public override S2CGpGpEditGetVoiceListRes Read(IBuffer buffer)
            {
                S2CGpGpEditGetVoiceListRes obj = new S2CGpGpEditGetVoiceListRes();
                ReadServerResponse(buffer, obj);
                obj.VoiceList = ReadEntityList<CDataEditParam>(buffer);
                return obj;
            }
        }
    }
}