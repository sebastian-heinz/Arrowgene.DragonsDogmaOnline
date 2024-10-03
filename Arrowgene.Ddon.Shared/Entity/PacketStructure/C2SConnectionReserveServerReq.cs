using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SConnectionReserveServerReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONNECTION_RESERVE_SERVER_REQ;

        public C2SConnectionReserveServerReq()
        {
            ReserveInfoList = new List<CDataCommonU32>();
        }

        public ushort GameServerUniqueID { get; set; }
        public byte Type { get; set; }
        public byte RotationServerID { get; set; }
        public List<CDataCommonU32> ReserveInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SConnectionReserveServerReq>
        {
            public override void Write(IBuffer buffer, C2SConnectionReserveServerReq obj)
            {
                WriteUInt16(buffer, obj.GameServerUniqueID);
                WriteByte(buffer, obj.Type);
                WriteByte(buffer, obj.RotationServerID);
                WriteEntityList<CDataCommonU32>(buffer, obj.ReserveInfoList);
            }

            public override C2SConnectionReserveServerReq Read(IBuffer buffer)
            {
                C2SConnectionReserveServerReq obj = new C2SConnectionReserveServerReq();
                obj.GameServerUniqueID = ReadUInt16(buffer);
                obj.Type = ReadByte(buffer);
                obj.RotationServerID = ReadByte(buffer);
                obj.ReserveInfoList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }

    }
}