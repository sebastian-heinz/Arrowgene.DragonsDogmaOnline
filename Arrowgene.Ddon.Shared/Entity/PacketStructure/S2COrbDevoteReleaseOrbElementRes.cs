using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2COrbDevoteReleaseHandlerRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ORB_DEVOTE_RELEASE_ORB_ELEMENT_RES;

        public S2COrbDevoteReleaseHandlerRes()
        {
        }

        public UInt32 RestOrb {  get; set; }
        public OrbGainParamType GainParamType {  get; set; }
        public UInt32 GainParamValue { get; set; }

        public class Serializer : PacketEntitySerializer<S2COrbDevoteReleaseHandlerRes>
        {
            public override void Write(IBuffer buffer, S2COrbDevoteReleaseHandlerRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.RestOrb);
                WriteByte(buffer, (byte) obj.GainParamType);
                WriteUInt32(buffer, obj.GainParamValue);
            }

            public override S2COrbDevoteReleaseHandlerRes Read(IBuffer buffer)
            {
                S2COrbDevoteReleaseHandlerRes obj = new S2COrbDevoteReleaseHandlerRes();
                ReadServerResponse(buffer, obj);
                obj.RestOrb = ReadUInt32(buffer);
                obj.GainParamType = (OrbGainParamType) ReadByte(buffer);
                obj.GainParamValue = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
