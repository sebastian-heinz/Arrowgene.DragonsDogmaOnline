using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2COrbDevoteReleasePawnOrbElementRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ORB_DEVOTE_RELEASE_PAWN_ORB_ELEMENT_RES;

        public S2COrbDevoteReleasePawnOrbElementRes()
        {
        }

        public uint PawnId {  get; set; }
        public uint RestOrb { get; set; }
        public OrbGainParamType GainParamType { get; set; }
        public uint GainParamValue { get; set; }

        public class Serializer : PacketEntitySerializer<S2COrbDevoteReleasePawnOrbElementRes>
        {
            public override void Write(IBuffer buffer, S2COrbDevoteReleasePawnOrbElementRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.RestOrb);
                WriteByte(buffer, (byte)obj.GainParamType);
                WriteUInt32(buffer, obj.GainParamValue);
            }

            public override S2COrbDevoteReleasePawnOrbElementRes Read(IBuffer buffer)
            {
                S2COrbDevoteReleasePawnOrbElementRes obj = new S2COrbDevoteReleasePawnOrbElementRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.RestOrb = ReadUInt32(buffer);
                obj.GainParamType = (OrbGainParamType)ReadByte(buffer);
                obj.GainParamValue = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
