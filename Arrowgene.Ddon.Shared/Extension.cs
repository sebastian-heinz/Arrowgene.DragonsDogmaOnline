using System;
using System.Text;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared
{
    public static class Extension
    {
        public static void WriteMtString(this IBuffer buffer, string str)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(str);
            buffer.WriteUInt16((ushort) utf8.Length, Endianness.Big);
            buffer.WriteBytes(utf8);
        }

        public static string ReadMtString(this IBuffer buffer)
        {
            ushort len = buffer.ReadUInt16(Endianness.Big);
            string str = buffer.ReadString(len, Encoding.UTF8);
            return str;
        }

        public static bool ReadEnumByte<TEnum>(this IBuffer buffer, out TEnum value) where TEnum : struct
        {
            byte enumByte = buffer.ReadByte();
            value = default;
            if (Enum.IsDefined(typeof(TEnum), enumByte))
            {
                value = (TEnum) (ValueType) enumByte;
                return true;
            }

            return false;
        }

        public static void WriteEnumByte<TEnum>(this IBuffer buffer, TEnum value) where TEnum : struct
        {
            buffer.WriteByte((byte) (ValueType) value);
        }
    }
}
