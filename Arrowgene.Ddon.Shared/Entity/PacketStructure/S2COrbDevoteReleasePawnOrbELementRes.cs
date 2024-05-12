using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2COrbDevoteReleasePawnOrbELementRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ORB_DEVOTE_RELEASE_PAWN_ORB_ELEMENT_RES;

        public S2COrbDevoteReleasePawnOrbELementRes()
        {
        }

        public UInt32 PawnId {  get; set; }
        public UInt32 RestOrb { get; set; }
        public OrbGainParamType GainParamType { get; set; }
        public UInt32 GainParamValue { get; set; }

        public class Serializer : PacketEntitySerializer<S2COrbDevoteReleasePawnOrbELementRes>
        {
            public override void Write(IBuffer buffer, S2COrbDevoteReleasePawnOrbELementRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.RestOrb);
                WriteByte(buffer, (byte)obj.GainParamType);
                WriteUInt32(buffer, obj.GainParamValue);
            }

            public override S2COrbDevoteReleasePawnOrbELementRes Read(IBuffer buffer)
            {
                S2COrbDevoteReleasePawnOrbELementRes obj = new S2COrbDevoteReleasePawnOrbELementRes();
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
