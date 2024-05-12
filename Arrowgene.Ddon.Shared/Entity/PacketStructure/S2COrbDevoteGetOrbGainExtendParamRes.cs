using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2COrbDevoteGetOrbGainExtendParamRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_RES;

        public S2COrbDevoteGetOrbGainExtendParamRes()
        {
            ExtendParam = new CDataOrbGainExtendParam();
        }

        public CDataOrbGainExtendParam ExtendParam {  get; set; }

        public class Serializer : PacketEntitySerializer<S2COrbDevoteGetOrbGainExtendParamRes>
        {
            public override void Write(IBuffer buffer, S2COrbDevoteGetOrbGainExtendParamRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataOrbGainExtendParam>(buffer, obj.ExtendParam);
            }

            public override S2COrbDevoteGetOrbGainExtendParamRes Read(IBuffer buffer)
            {
                S2COrbDevoteGetOrbGainExtendParamRes obj = new S2COrbDevoteGetOrbGainExtendParamRes();
                ReadServerResponse(buffer, obj);
                obj.ExtendParam = ReadEntity<CDataOrbGainExtendParam>(buffer);
                return obj;
            }
        }
    }
}
