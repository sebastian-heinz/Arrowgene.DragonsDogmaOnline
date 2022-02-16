using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client
{
    public abstract class ClientResourceFile
    {
        public string MagicTag { get; set; }
        public uint MagicId { get; set; }

        public void Open(string path)
        {
            IBuffer buffer = new StreamBuffer(path);
            buffer.SetPositionStart();
            if (buffer.Size < 4)
            {
                return;
            }

            MagicTag = buffer.ReadFixedString(4);
            MagicId = buffer.ReadUInt32(Endianness.Little);
            Read(buffer);
        }

        protected abstract void Read(IBuffer buffer);

        protected void WriteMtString(IBuffer buffer, string str)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(str);
            buffer.WriteUInt16((ushort) utf8.Length, Endianness.Little);
            buffer.WriteBytes(utf8);
        }

        protected string ReadMtString(IBuffer buffer)
        {
            ushort len = buffer.ReadUInt16(Endianness.Little);
            string str = buffer.ReadString(len, Encoding.UTF8);
            return str;
        }

        protected void WriteMtArray<T>(IBuffer buffer, List<T> entities, Action<IBuffer, T> writer)
        {
            buffer.WriteUInt32((uint) entities.Count, Endianness.Little);
            for (int i = 0; i < entities.Count; i++)
            {
                writer.Invoke(buffer, entities[i]);
            }
        }

        protected List<T> ReadMtArray<T>(IBuffer buffer, Func<IBuffer, T> reader)
        {
            List<T> entities = new List<T>();
            uint len = buffer.ReadUInt32(Endianness.Little);
            for (int i = 0; i < len; i++)
            {
                entities.Add(reader.Invoke(buffer));
            }

            return entities;
        }

        protected void WriteUInt32(IBuffer buffer, uint value)
        {
            buffer.WriteUInt32(value, Endianness.Little);
        }

        protected uint ReadUInt32(IBuffer buffer)
        {
            return buffer.ReadUInt32(Endianness.Little);
        }

        protected ushort ReadUInt16(IBuffer buffer)
        {
            return buffer.ReadUInt16(Endianness.Little);
        }
    }
}
