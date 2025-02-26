using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    /// <summary>
    /// Type used to serialize pair data (T,T) of native types
    /// byte, u8, i8, u16, i16, u32, i32. u64, and i64.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CDataCommonPair<T>
    {
        public CDataCommonPair(T valueA, T valueB)
        {
            ValueA = valueA;
            ValueB = valueB;
        }

        public CDataCommonPair()
        {
        }

        public T ValueA { get; set; }
        public T ValueB { get; set; }

        public class Serializer : EntitySerializer<CDataCommonPair<T>>
        {
            public override void Write(IBuffer buffer, CDataCommonPair<T> obj)
            {
                WriteT(buffer, obj.ValueA);
                WriteT(buffer, obj.ValueB);
            }

            public override CDataCommonPair<T> Read(IBuffer buffer)
            {
                CDataCommonPair<T> obj = new CDataCommonPair<T>();
                obj.ValueA = ReadT<T>(buffer);
                obj.ValueB = ReadT<T>(buffer);
                return obj;
            }
        }
    }
}
