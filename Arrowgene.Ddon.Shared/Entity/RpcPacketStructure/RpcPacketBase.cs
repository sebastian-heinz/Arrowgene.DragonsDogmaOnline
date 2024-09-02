using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;

namespace Arrowgene.Ddon.Shared.Entity.RpcPacketStructure
{
    public abstract class RpcPacketBase : IRpcPacket
    {
        public RpcPacketBase()
        {
        }

        public abstract void Handle(Character character, RpcPacketHeader packetHeader, IBuffer buffer);

        public byte[] ReadBytes(IBuffer buffer, int length)
        {
            return buffer.ReadBytes(length);
        }

        public byte ReadByte(IBuffer buffer)
        {
            return buffer.ReadByte();
        }

        public UInt16 ReadUInt16(IBuffer buffer)
        {
            return buffer.ReadUInt16(Endianness.Big);
        }

        public UInt32 ReadUInt32(IBuffer buffer)
        {
            return buffer.ReadUInt32(Endianness.Big);
        }

        public UInt64 ReadUInt64(IBuffer buffer)
        {
            return buffer.ReadUInt64(Endianness.Big);
        }

        public bool ReadBool(IBuffer buffer) 
        {
            return buffer.ReadBool();
        }

        public double ReadDouble(IBuffer buffer) 
        {
            return buffer.ReadDouble(Endianness.Big);
        }
        public float ReadFloat(IBuffer buffer)
        {
            return buffer.ReadFloat(Endianness.Big);
        }
    }
}
