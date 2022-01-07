using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity
{
    public abstract class EntitySerializer
    {
        private static readonly Dictionary<Type, EntitySerializer> Serializers =
            new Dictionary<Type, EntitySerializer>(
                new[]
                {
                    Create(new CDataCharacterListElementSerializer()),
                    Create(new CDataCharacterListInfoSerializer()),
                    Create(new CDataEditInfoSerializer()),
                    Create(new CDataEquipElementParamSerializer()),
                    Create(new CDataEquipElementUnkTypeSerializer()),
                    Create(new CDataEquipElementUnkType2Serializer()),
                    Create(new CDataEquipItemInfoSerializer()),
                    Create(new CDataGPCourseValidSerializer()),
                    Create(new CDataMatchingProfileSerializer()),
                    Create(new DoubleByteThingSerializer()),
                }
            );

        private static KeyValuePair<Type, EntitySerializer> Create(EntitySerializer serializer)
        {
            return new KeyValuePair<Type, EntitySerializer>(serializer.GetEntityType(), serializer);
        }

        public static void RegisterReader(EntitySerializer reader)
        {
            Serializers.Add(reader.GetType(), reader);
        }

        public static EntitySerializer GetReader(Type type)
        {
            return Serializers[type];
        }

        protected abstract Type GetEntityType();
    }

    public abstract class EntitySerializer<T> : EntitySerializer
    {
        public abstract void Write(IBuffer buffer, T obj);
        public abstract T Read(IBuffer buffer);

        protected override Type GetEntityType()
        {
            return typeof(T);
        }

        public byte[] Write(T entity)
        {
            IBuffer buffer = new StreamBuffer();
            Write(buffer, entity);
            return buffer.GetAllBytes();
        }

        public T Read(byte[] data)
        {
            IBuffer buffer = new StreamBuffer(data);
            buffer.SetPositionStart();
            return Read(buffer);
        }
    }
}
