using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2COrbDevoteReleaseOrbElementRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ORB_DEVOTE_RELEASE_ORB_ELEMENT_RES;

        public S2COrbDevoteReleaseOrbElementRes()
        {
        }

        public uint RestOrb {  get; set; }
        public OrbGainParamType GainParamType {  get; set; }
        public uint GainParamValue { get; set; }

        public class Serializer : PacketEntitySerializer<S2COrbDevoteReleaseOrbElementRes>
        {
            public override void Write(IBuffer buffer, S2COrbDevoteReleaseOrbElementRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.RestOrb);
                WriteByte(buffer, (byte) obj.GainParamType);
                WriteUInt32(buffer, obj.GainParamValue);
            }

            public override S2COrbDevoteReleaseOrbElementRes Read(IBuffer buffer)
            {
                S2COrbDevoteReleaseOrbElementRes obj = new S2COrbDevoteReleaseOrbElementRes();
                ReadServerResponse(buffer, obj);
                obj.RestOrb = ReadUInt32(buffer);
                obj.GainParamType = (OrbGainParamType) ReadByte(buffer);
                obj.GainParamValue = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
