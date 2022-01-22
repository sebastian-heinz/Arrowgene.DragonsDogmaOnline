using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity
{
    public abstract class EntitySerializer
    {
        private static readonly Dictionary<Type, EntitySerializer> Serializers =
            new Dictionary<Type, EntitySerializer>(
                new[]
                {
                    // Data structure serializers
                    Create(new CDataAchievementIdentifierSerializer()),
                    Create(new CDataArisenProfileSerializer()),
                    Create(new CDataCharacterEquipDataSerializer()),
                    Create(new CDataCharacterInfoSerializer()),
                    Create(new CDataCharacterJobDataSerializer()),
                    Create(new CDataCharacterListElementSerializer()),
                    Create(new CDataCharacterListInfoSerializer()),
                    Create(new CDataCharacterMessageSerializer()),
                    Create(new CDataCharacterMsgSetSerializer()),
                    Create(new CDataCommunicationShortCutSerializer()),
                    Create(new CDataEditInfoSerializer()),
                    Create(new CDataEquipElementParamSerializer()),
                    Create(new CDataEquipElementUnkTypeSerializer()),
                    Create(new CDataEquipElementUnkType2Serializer()),
                    Create(new CDataEquipItemInfoSerializer()),
                    Create(new CDataEquipJobItemSerializer()),
                    Create(new CDataGameServerListInfoSerializer()),
                    Create(new CDataGPCourseValidSerializer()),
                    Create(new CDataJobPlayPointSerializer()),
                    Create(new CDataLobbyChatMsgNoticeSerializer()),
                    Create(new CDataLobbyChatMsgNoticeCharacterSerializer()),
                    Create(new CDataLobbyChatMsgReqSerializer()),
                    Create(new CDataLobbyChatMsgResSerializer()),
                    Create(new CDataLoginSettingSerializer()),
                    Create(new CDataMatchingProfileSerializer()),
                    Create(new CDataOrbCategoryStatusSerializer()),
                    Create(new CDataOrbPageStatusSerializer()),
                    Create(new CDataPlayPointDataSerializer()),
                    Create(new CDataShortCutSerializer()),
                    Create(new CDataStatusInfoSerializer()),
                    Create(new CDataURLInfoSerializer()),
                    Create(new CDataWarpPointSerializer()),
                    Create(new DoubleByteThingSerializer()),
                    Create(new UnkownCharacterData0Serializer()),
                    Create(new UnkownCharacterData1Serializer()),

                    // Packet structure serializers
                    Create(new C2LCreateCharacterDataReq.Serializer()),
                    Create(new C2LDecideCharacterIdReq.Serializer()),
                    Create(new C2LGetErrorMessageListReq.Serializer()),
                    Create(new C2LLoginReq.Serializer()),
                    Create(new C2SConnectionLoginReq.Serializer()),
                    Create(new C2SConnectionMoveInServerReq.Serializer()),
                    Create(new C2SConnectionMoveOutServerReq.Serializer()),
                    Create(new C2SWarpReqSerializer()),
                    Create(new L2CCreateCharacterDataNtc.Serializer()),
                    Create(new L2CCreateCharacterDataRes.Serializer()),
                    Create(new L2CGetErrorMessageListNtc.Serializer()),
                    Create(new L2CGetErrorMessageListRes.Serializer()),
                    Create(new L2CDecideCharacterIdRes.Serializer()),
                    Create(new L2CGetGameSessionKeyRes.Serializer()),
                    Create(new L2CGetLoginSettingsRes.Serializer()),
                    Create(new L2CLoginRes.Serializer()),
                    Create(new L2CLoginWaitNumNtc.Serializer()),
                    Create(new L2CNextConnectionServerNtc.Serializer()),
                    Create(new S2CConnectionLoginRes.Serializer()),
                    Create(new S2CConnectionLogoutRes.Serializer()),
                    Create(new S2CConnectionMoveOutServerRes.Serializer()),
                    Create(new S2CWarpResSerializer()),

                    Create(new ServerRes.Serializer()),
                }
            );

        private static KeyValuePair<Type, EntitySerializer> Create(EntitySerializer serializer)
        {
            return new KeyValuePair<Type, EntitySerializer>(serializer.GetEntityType(), serializer);
        }

        public static void RegisterReader(EntitySerializer reader)
        {
            Serializers.Add(reader.GetEntityType(), reader);
        }

        public static EntitySerializer<T> Get<T>()
        {
            Type type = typeof(T);
            object obj = Serializers[type];
            EntitySerializer<T> serializer = obj as EntitySerializer<T>;
            return serializer;
        }

        protected abstract Type GetEntityType();
    }

    public abstract class EntitySerializer<T> : EntitySerializer
    {
        public abstract void Write(IBuffer buffer, T obj);
        public abstract T Read(IBuffer buffer);

        public List<T> ReadList(IBuffer buffer)
        {
            return ReadEntityList<T>(buffer);
        }

        public void WriteList(IBuffer buffer, List<T> entities)
        {
            WriteEntityList<T>(buffer, entities);
        }

        protected override Type GetEntityType()
        {
            return typeof(T);
        }

        protected void WriteUInt64(IBuffer buffer, ulong value)
        {
            buffer.WriteUInt64(value, Endianness.Big);
        }

        protected ulong ReadUInt64(IBuffer buffer)
        {
            return buffer.ReadUInt64(Endianness.Big);
        }

        protected void WriteUInt32(IBuffer buffer, uint value)
        {
            buffer.WriteUInt32(value, Endianness.Big);
        }

        protected uint ReadUInt32(IBuffer buffer)
        {
            return buffer.ReadUInt32(Endianness.Big);
        }

        protected void WriteUInt16(IBuffer buffer, ushort value)
        {
            buffer.WriteUInt16(value, Endianness.Big);
        }

        protected ushort ReadUInt16(IBuffer buffer)
        {
            return buffer.ReadUInt16(Endianness.Big);
        }

        protected void WriteBool(IBuffer buffer, bool value)
        {
            buffer.WriteBool(value);
        }

        protected void WriteByte(IBuffer buffer, byte value)
        {
            buffer.WriteByte(value);
        }

        protected bool ReadBool(IBuffer buffer)
        {
            return buffer.ReadBool();
        }

        protected byte ReadByte(IBuffer buffer)
        {
            return buffer.ReadByte();
        }

        protected void WriteMtString(IBuffer buffer, string str)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(str);
            buffer.WriteUInt16((ushort) utf8.Length, Endianness.Big);
            buffer.WriteBytes(utf8);
        }

        protected string ReadMtString(IBuffer buffer)
        {
            ushort len = buffer.ReadUInt16(Endianness.Big);
            string str = buffer.ReadString(len, Encoding.UTF8);
            return str;
        }

        protected void WriteServerResponse(IBuffer buffer, ServerResponse value)
        {
            buffer.WriteUInt32(value.Error, Endianness.Big);
            buffer.WriteUInt32(value.Result, Endianness.Big);
        }

        protected void ReadServerResponse(IBuffer buffer, ServerResponse value)
        {
            value.Error = buffer.ReadUInt32(Endianness.Big);
            value.Result = buffer.ReadUInt32(Endianness.Big);
        }

        protected void WriteEntity<TEntity>(IBuffer buffer, TEntity entity)
        {
            EntitySerializer<TEntity> serializer = Get<TEntity>();
            if (serializer == null)
            {
                // error
                return;
            }

            serializer.Write(buffer, entity);
        }

        protected void WriteEntityList<TEntity>(IBuffer buffer, List<TEntity> entities)
        {
            WriteUInt32(buffer, (uint) entities.Count);
            for (int i = 0; i < entities.Count; i++)
            {
                WriteEntity(buffer, entities[i]);
            }
        }

        protected List<TEntity> ReadEntityList<TEntity>(IBuffer buffer)
        {
            List<TEntity> entities = new List<TEntity>();
            uint len = ReadUInt32(buffer);
            for (int i = 0; i < len; i++)
            {
                entities.Add(ReadEntity<TEntity>(buffer));
            }

            return entities;
        }

        protected TEntity ReadEntity<TEntity>(IBuffer buffer)
        {
            EntitySerializer<TEntity> serializer = Get<TEntity>();
            if (serializer == null)
            {
                // error
                return default;
            }

            return serializer.Read(buffer);
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
