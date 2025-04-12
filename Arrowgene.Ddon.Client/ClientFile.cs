using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Client
{
    public abstract class ClientFile
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ClientFile));

        public FileInfo FilePath { get; protected set; }
        
        public ClientFile()
        {
            FilePath = null;
        }
        
        public byte[] Save()
        {
            IBuffer buffer = new StreamBuffer();
            Write(buffer);
            return buffer.GetAllBytes();
        }        
        
        public void Open(IBuffer buffer)
        {
            buffer.SetPositionStart();
            if (buffer.Size < 4)
            {
                return;
            }

            Read(buffer);
        }
        
        public void Open(byte[] data)
        {
            if (data == null)
            {
                Logger.Error("Invalid data (data == null)");
                return;
            }
            IBuffer buffer = new StreamBuffer(data);
            Open(buffer);
        }

        public void Open(string path)
        {
            FilePath = new FileInfo(path);
            if (!FilePath.Exists)
            {
                Logger.Error($"File does not exists: {path}");
                return;
            }

            IBuffer buffer = new StreamBuffer(path);
            Open(buffer);
        }

        public abstract void Read(IBuffer buffer);

        public abstract void Write(IBuffer buffer);

        protected void WriteMtString(IBuffer buffer, string str)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(str);
            buffer.WriteUInt16((ushort) utf8.Length, Endianness.Little);
            buffer.WriteBytes(utf8);
        }

        protected string ReadMtString16(IBuffer buffer)
        {
            ushort len = buffer.ReadUInt16(Endianness.Little);
            string str = buffer.ReadString(len, Encoding.UTF8);
            return str;
        }
        
        protected string ReadMtString32(IBuffer buffer)
        {
            uint len = buffer.ReadUInt32(Endianness.Little);
            string str = buffer.ReadString((int)len, Encoding.UTF8);
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

        protected byte[] ReadBytes(IBuffer buffer, int length)
        {
            return buffer.ReadBytes(length);
        }

        protected void WriteUInt32(IBuffer buffer, uint value)
        {
            buffer.WriteUInt32(value, Endianness.Little);
        }

        protected uint ReadUInt32(IBuffer buffer)
        {
            return buffer.ReadUInt32(Endianness.Little);
        }
        
        protected ulong ReadUInt64(IBuffer buffer)
        {
            return buffer.ReadUInt64(Endianness.Little);
        }
        
        protected float ReadFloat(IBuffer buffer)
        {
            return buffer.ReadFloat(Endianness.Little);
        }

        protected MtVector3 ReadMtVector3(IBuffer buffer)
        {
            MtVector3 vec = new MtVector3();
            vec.X = ReadFloat(buffer);
            vec.Y = ReadFloat(buffer);
            vec.Z = ReadFloat(buffer);
            return vec;
        }

        protected ushort ReadUInt16(IBuffer buffer)
        {
            return buffer.ReadUInt16(Endianness.Little);
        }

        protected bool ReadBool(IBuffer buffer)
        {
            return buffer.ReadByte() != 0;
        }

        protected byte ReadByte(IBuffer buffer)
        {
            return buffer.ReadByte();
        }

        protected void WriteInt32(IBuffer buffer, int value)
        {
            buffer.WriteInt32(value, Endianness.Little);
        }

        protected int ReadInt32(IBuffer buffer)
        {
            return buffer.ReadInt32(Endianness.Little);
        }

        protected short ReadInt16(IBuffer buffer)
        {
            return buffer.ReadInt16(Endianness.Little);
        }
    }
}
