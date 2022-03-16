using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.Entity
{
    public abstract class EntitySerializer
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(EntitySerializer));

        private static readonly Dictionary<PacketId, EntitySerializer> LoginPacketSerializers;
        private static readonly Dictionary<PacketId, EntitySerializer> GamePacketSerializers;
        private static readonly Dictionary<Type, EntitySerializer> Serializers;
        private static readonly Dictionary<PacketId, IStructurePacketFactory> LoginStructurePacketFactories;
        private static readonly Dictionary<PacketId, IStructurePacketFactory> GameStructurePacketFactories;

        static EntitySerializer()
        {
            LoginPacketSerializers = new Dictionary<PacketId, EntitySerializer>();
            GamePacketSerializers = new Dictionary<PacketId, EntitySerializer>();
            Serializers = new Dictionary<Type, EntitySerializer>();
            LoginStructurePacketFactories = new Dictionary<PacketId, IStructurePacketFactory>();
            GameStructurePacketFactories = new Dictionary<PacketId, IStructurePacketFactory>();

            // Data structure serializers
            Create(
                new C2SActionSetPlayerActionHistoryReqElement.
                    Serializer()); // TODO naming convention C2S -> not a packet
            Create(new CData_35_14_16.Serializer());
            Create(new CDataAchievementIdentifierSerializer());
            Create(new CDataArisenProfileSerializer());
            Create(new CDataCharacterEquipDataSerializer());
            Create(new CDataCharacterEquipInfo.Serializer());
            Create(new CDataCharacterInfoSerializer());
            Create(new CDataCharacterJobDataSerializer());
            Create(new CDataCharacterListElement.Serializer());
            Create(new CDataCharacterListInfoSerializer());
            Create(new CDataCharacterMessageSerializer());
            Create(new CDataCharacterMsgSetSerializer());
            Create(new CDataCharacterName.Serializer());
            Create(new CDataCharacterReleaseElement.Serializer());
            Create(new CDataCommonU32.Serializer());
            Create(new CDataCommunicationShortCutSerializer());
            Create(new CDataCommunityCharacterBaseInfo.Serializer());
            Create(new CDataContextAcquirementData.Serializer());
            Create(new CDataContextBase.Serializer());
            Create(new CDataContextBaseUnk0.Serializer());
            Create(new CDataContextEquipData.Serializer());
            Create(new CDataContextEquipJobItemData.Serializer());
            Create(new CDataContextJobData.Serializer());
            Create(new CDataContextNormalSkillData.Serializer());
            Create(new CDataContextPlayerInfo.Serializer());
            Create(new CDataDeliveredItemRecord.Serializer());
            Create(new CDataDropItemSetInfo.Serializer());
            Create(new CDataEditInfoSerializer());
            Create(new CDataEquipElementParam.Serializer());
            Create(new CDataWeaponCrestData.Serializer());
            Create(new CDataArmorCrestData.Serializer());
            Create(new CDataEquipItemInfo.Serializer());
            Create(new CDataEquipJobItem.Serializer());
            Create(new CDataFavoriteWarpPoint.Serializer());
            Create(new CDataGameServerListInfoSerializer());
            Create(new CDataGameTimeBaseInfo.Serializer());
            Create(new CDataGPCourseValidSerializer());
            Create(new CDataItemList.Serializer());
            Create(new CDataItemSort.Serializer());
            Create(new CDataItemUpdateResult.Serializer());
            Create(new CDataJobChangeInfo.Serializer());
            Create(new CDataJobChangeJobResUnk0.Serializer());
            Create(new CDataJobChangeJobResUnk0Unk1.Serializer());
            Create(new CDataJobBaseInfo.Serializer());
            Create(new CDataJobPlayPointSerializer());
            Create(new CDataJumpLocationSerializer());
            Create(new CDataLayoutEnemyData.Serializer());
            Create(new CDataLearnedSetAcquirementParam.Serializer());
            Create(new CDataLearnNormalSkillParam.Serializer());
            Create(new CDataLobbyContextPlayer.Serializer());
            Create(new CDataLobbyMemberInfoSerializer());
            Create(new CDataLoginSettingSerializer());
            Create(new CDataMatchingProfileSerializer());
            Create(new CDataMoonSchedule.Serializer());
            Create(new CDataNamedEnemyParamClient.Serializer());
            Create(new CDataNormalSkillParam.Serializer());
            Create(new CDataOcdActive.Serializer());
            Create(new CDataOrbCategoryStatusSerializer());
            Create(new CDataOrbPageStatusSerializer());
            Create(new CDataPartyMember.Serializer());
            Create(new CDataPawnJobChangeInfo.Serializer());
            Create(new CDataPlayPointDataSerializer());
            Create(new CDataPresetAbilityParam.Serializer());
            Create(new CDataQuestAnnounce.Serializer());
            Create(new CDataQuestEnemyInfo.Serializer());
            Create(new CDataQuestFlag.Serializer());
            Create(new CDataQuestIdScheduleId.Serializer());
            Create(new CDataQuestKeyItemPointRecord.Serializer());
            Create(new CDataQuestLayoutFlag.Serializer());
            Create(new CDataQuestLayoutFlagSetInfo.Serializer());
            Create(new CDataQuestOrderConditionParam.Serializer());
            Create(new CDataQuestOrderList.Serializer());
            Create(new CDataQuestProcessState.Serializer());
            Create(new CDataQuestTalkInfo.Serializer());
            Create(new CDataRewardItem.Serializer());
            Create(new CDataSetAcquirementParam.Serializer());
            Create(new CDataShortCutSerializer());
            Create(new CDataStageAttribute.Serializer());
            Create(new CDataStageInfo.Serializer());
            Create(new CDataStageLayoutEnemyPresetEnemyInfoClient.Serializer());
            Create(new CDataStatusInfoSerializer());
            Create(new CDataTraningRoomEnemyHeader.Serializer());
            Create(new CDataUpdateWalletPoint.Serializer());
            Create(new CDataURLInfoSerializer());
            Create(new CDataWarpPoint.Serializer());
            Create(new CDataWeatherLoop.Serializer());
            Create(new CDataWeatherSchedule.Serializer());
            Create(new CStageLayoutId.Serializer());
            Create(new UnkownCharacterData0Serializer());
            Create(new UnkownCharacterData1Serializer());

            // Packet structure serializers
            Create(new C2LCreateCharacterDataReq.Serializer());
            Create(new C2LDecideCharacterIdReq.Serializer());
            Create(new C2LGetErrorMessageListReq.Serializer());
            Create(new C2LLoginReq.Serializer());
            Create(new C2SActionSetPlayerActionHistoryReq.Serializer());
            Create(new C2SCharacterCharacterGoldenReviveReq.Serializer());
            Create(new C2SCharacterCharacterPenaltyReviveReq.Serializer());
            Create(new C2SCharacterCharacterPointReviveReq.Serializer());
            Create(new C2SConnectionLoginReq.Serializer());
            Create(new C2SConnectionMoveInServerReq.Serializer());
            Create(new C2SConnectionMoveOutServerReq.Serializer());
            Create(new C2SContextGetSetContextReq.Serializer());
            Create(new C2SInstanceEnemyKillReq.Serializer());
            Create(new C2SInstanceExchangeOmInstantKeyValueReq.Serializer());
            Create(new C2SInstanceGetEnemySetListReq.Serializer());
            Create(new C2SInstanceGetGatheringItemListReq.Serializer());
            Create(new C2SInstanceGetGatheringItemReq.Serializer());
            Create(new C2SInstanceGetItemSetListReq.Serializer());
            Create(new C2SInstanceSetOmInstantKeyValueReq.Serializer());
            Create(new C2SItemUseBagItemReq.Serializer());
            Create(new C2SJobChangeJobReq.Serializer());
            Create(new C2SJobGetJobChangeListReq.Serializer());
            Create(new C2SLobbyChatMsgReq.Serializer());
            Create(new C2SLobbyJoinReq.Serializer());
            Create(new C2SLobbyLobbyDataMsgReq.Serializer());
            Create(new C2SPawnGetMypawnDataReq.Serializer());
            Create(new C2SPawnJoinPartyMypawnReq.Serializer());
            Create(new C2SQuestGetTutorialQuestListRes.Serializer());
            Create(new C2SServerGameTimeGetBaseInfoReq.Serializer());
            Create(new C2SSkillGetAbilityCostReq.Serializer());
            Create(new C2SSkillGetLearnedAbilityListReq.Serializer());
            Create(new C2SSkillGetLearnedNormalSkillListReq.Serializer());
            Create(new C2SSkillGetLearnedSkillListReq.Serializer());
            Create(new C2SSkillGetPresetAbilityListReq.Serializer());
            Create(new C2SSkillGetSetAbilityListReq.Serializer());
            Create(new C2SSkillGetSetSkillListReq.Serializer());
            Create(new C2SStageAreaChangeReq.Serializer());
            Create(new C2SStageGetStageListReq.Serializer());
            Create(new C2STraningRoomGetEnemyListReq.Serializer());
            Create(new C2STrainingRoomSetEnemyReq.Serializer());
            Create(new C2SWarpAreaWarpReq.Serializer());
            Create(new C2SWarpGetFavoriteWarpPointListReq.Serializer());
            Create(new C2SWarpGetReleaseWarpPointListReq.Serializer());
            Create(new C2SWarpGetReturnLocationReq.Serializer());
            Create(new C2SWarpGetStartPointListReq.Serializer());
            Create(new C2SWarpGetWarpPointListReq.Serializer());
            Create(new C2SWarpRegisterFavoriteWarpReq.Serializer());
            Create(new C2SWarpReleaseWarpPointReq.Serializer());
            Create(new C2SWarpWarpReq.Serializer());
            Create(new L2CCreateCharacterDataNtc.Serializer());
            Create(new L2CCreateCharacterDataRes.Serializer());
            Create(new L2CGetErrorMessageListNtc.Serializer());
            Create(new L2CGetErrorMessageListRes.Serializer());
            Create(new L2CDecideCharacterIdRes.Serializer());
            Create(new L2CGetGameSessionKeyRes.Serializer());
            Create(new L2CGetLoginSettingsRes.Serializer());
            Create(new L2CLoginRes.Serializer());
            Create(new L2CLoginWaitNumNtc.Serializer());
            Create(new L2CNextConnectionServerNtc.Serializer());
            Create(new S2CActionSetPlayerActionHistoryRes.Serializer());
            Create(new S2CChangeCharacterEquipLobbyNotice.Serializer());
            Create(new S2CCharacterCharacterGoldenReviveRes.Serializer());
            Create(new S2CCharacterCharacterPenaltyReviveRes.Serializer());
            Create(new S2CCharacterCharacterPointReviveRes.Serializer());
            Create(new S2CCharacterDecideCharacterIdRes.Serializer());
            Create(new S2CCharacterContentsReleaseElementNotice.Serializer());
            Create(new S2CClanClanGetMyMemberListRes.Serializer());
            Create(new S2CConnectionLoginRes.Serializer());
            Create(new S2CConnectionLogoutRes.Serializer());
            Create(new S2CConnectionMoveInServerRes.Serializer());
            Create(new S2CConnectionMoveOutServerRes.Serializer());
            Create(new S2CContext_35_14_16_Ntc.Serializer());
            Create(new S2CContext_35_3_16_Ntc.Serializer());
            Create(new S2CContextGetLobbyPlayerContextNtc.Serializer());
            Create(new S2CContextGetSetContextRes.Serializer());
            Create(new S2CContextSetContextBaseNotice.Serializer());
            Create(new S2CInstanceEnemyKillRes.Serializer());
            Create(new S2CInstanceEnemyRepopNtc.Serializer());
            Create(new S2CInstanceExchangeOmInstantKeyValueRes.Serializer());
            Create(new S2CInstanceGetEnemySetListRes.Serializer());
            Create(new S2CInstanceGetGatheringItemListRes.Serializer());
            Create(new S2CInstanceGetGatheringItemRes.Serializer());
            Create(new S2CInstanceGetItemSetListRes.Serializer());
            Create(new S2CInstanceSetOmInstantKeyValueRes.Serializer());
            Create(new S2CItemExtendItemSlotNtc.Serializer());
            Create(new S2CItemUpdateCharacterItemNtc.Serializer());
            Create(new S2CItemSortGetItemSortdataBinRes.Serializer());
            Create(new S2CItemSortGetItemSortdataBinNtc.Serializer());
            Create(new S2CItemUseBagItemRes.Serializer());
            Create(new S2CJobChangeJobNtc.Serializer());
            Create(new S2CJobChangeJobRes.Serializer());
            Create(new S2CJobGetJobChangeListRes.Serializer());
            Create(new S2CLobbyChatMsgRes.Serializer());
            Create(new S2CLobbyChatMsgNotice.Serializer());
            Create(new S2CLobbyJoinRes.Serializer());
            Create(new S2CLobbyLobbyDataMsgNotice.Serializer());
            Create(new S2CParty_6_8_16_Ntc.Serializer());
            Create(new S2CPawn_8_37_16_Ntc.Serializer());
            Create(new S2CPawnGetMypawnDataRes.Serializer());
            Create(new S2CPawnJoinPartyMypawnRes.Serializer());
            Create(new S2CQuestGetPartyQuestProgressInfoRes.Serializer());
            Create(new S2CServerGameTimeGetBaseInfoRes.Serializer());
            Create(new S2CSkillGetAbilityCostRes.Serializer());
            Create(new S2CSkillGetCurrentSetSkillListRes.Serializer());
            Create(new S2CSkillGetLearnedAbilityListRes.Serializer());
            Create(new S2CSkillGetLearnedNormalSkillListRes.Serializer());
            Create(new S2CSkillGetLearnedSkillListRes.Serializer());
            Create(new S2CSkillGetPresetAbilityListRes.Serializer());
            Create(new S2CSkillGetSetAbilityListRes.Serializer());
            Create(new S2CSkillGetSetSkillListRes.Serializer());
            Create(new S2CStageAreaChangeRes.Serializer());
            Create(new S2CStageGetStageListRes.Serializer());
            Create(new S2CTraningRoomGetEnemyListRes.Serializer());
            Create(new S2CTraningRoomSetEnemyRes.Serializer());
            Create(new S2CUserListJoinNtc.Serializer());
            Create(new S2CUserListLeaveNtc.Serializer());
            Create(new S2CWarpAreaWarpRes.Serializer());
            Create(new S2CWarpGetFavoriteWarpPointListRes.Serializer());
            Create(new S2CWarpGetReleaseWarpPointListRes.Serializer());
            Create(new S2CWarpGetReturnLocationRes.Serializer());
            Create(new S2CWarpGetStartPointListRes.Serializer());
            Create(new S2CWarpGetWarpPointListRes.Serializer());
            Create(new S2CWarpRegisterFavoriteWarpRes.Serializer());
            Create(new S2CWarpReleaseWarpPointRes.Serializer());
            Create(new S2CWarpWarpRes.Serializer());

            Create(new ServerRes.Serializer());
        }

        private static void Create<T>(PacketEntitySerializer<T> serializer) where T : class, IPacketStructure, new()
        {
            Type type = serializer.GetEntityType();
            Serializers.Add(type, serializer);

            PacketId packetId = new T().Id;
            if (packetId != PacketId.UNKNOWN)
            {
                if (packetId.ServerType == ServerType.Login)
                {
                    if (LoginPacketSerializers.ContainsKey(packetId))
                    {
                        Logger.Error(
                            $"PacketId:{packetId}({packetId.Name}) has already been added to `LoginPacketSerializers` lookup");
                    }
                    else
                    {
                        LoginPacketSerializers.Add(packetId, serializer);
                    }

                    if (LoginStructurePacketFactories.ContainsKey(packetId))
                    {
                        Logger.Error(
                            $"PacketId:{packetId}({packetId.Name}) has already been added to `LoginStructurePacketFactories` lookup");
                    }
                    else
                    {
                        LoginStructurePacketFactories.Add(packetId, serializer);
                    }
                }
                else if (packetId.ServerType == ServerType.Game)
                {
                    if (GamePacketSerializers.ContainsKey(packetId))
                    {
                        Logger.Error(
                            $"PacketId:{packetId}({packetId.Name}) has already been added to `GamePacketSerializers` lookup");
                    }
                    else
                    {
                        GamePacketSerializers.Add(packetId, serializer);
                    }

                    if (GameStructurePacketFactories.ContainsKey(packetId))
                    {
                        Logger.Error(
                            $"PacketId:{packetId}({packetId.Name}) has already been added to `GameStructurePacketFactories` lookup");
                    }
                    else
                    {
                        GameStructurePacketFactories.Add(packetId, serializer);
                    }
                }
            }
        }

        private static void Create<T>(EntitySerializer<T> serializer) where T : class, new()
        {
            if (typeof(IPacketStructure).IsAssignableFrom(typeof(T))
                && typeof(T) != typeof(ServerRes)) // ServerRes is exception to this rule as it is a generic response.
            {
                Logger.Error($"EntitySerializer<{typeof(T)}> should be PacketEntitySerializer<{typeof(T)}> " +
                             $"because {typeof(T)} is assignable from `IPacketStructure`, indicating it is a PacketStructure");
            }

            Type type = serializer.GetEntityType();
            if (Serializers.ContainsKey(type))
            {
                Logger.Error($"Type:{type} has already been added to `Serializers` lookup");
                return;
            }

            Serializers.Add(type, serializer);
        }

        /// <summary>
        /// Provides a Serializer for a specific type of Structure
        /// </summary>
        public static EntitySerializer<T> Get<T>() where T : class, new()
        {
            Type type = typeof(T);
            if (!Serializers.ContainsKey(type))
            {
                return null;
            }

            object obj = Serializers[type];
            EntitySerializer<T> serializer = obj as EntitySerializer<T>;
            return serializer;
        }

        /// <summary>
        /// Provides a Serializer for a PacketId
        /// </summary>
        public static EntitySerializer Get(PacketId packetId)
        {
            if (packetId.ServerType == ServerType.Login && LoginPacketSerializers.ContainsKey(packetId))
            {
                return LoginPacketSerializers[packetId];
            }

            if (packetId.ServerType == ServerType.Game && GamePacketSerializers.ContainsKey(packetId))
            {
                return GamePacketSerializers[packetId];
            }

            return null;
        }

        /// <summary>
        /// Creates a StructuredPacket from a Packet
        /// </summary>
        public static IStructurePacket CreateStructurePacket(Packet packet)
        {
            PacketId packetId = packet.Id;
            if (packetId.ServerType == ServerType.Login && LoginStructurePacketFactories.ContainsKey(packetId))
            {
                return LoginStructurePacketFactories[packetId].Create(packet);
            }

            if (packetId.ServerType == ServerType.Game && GameStructurePacketFactories.ContainsKey(packetId))
            {
                return GameStructurePacketFactories[packetId].Create(packet);
            }

            return null;
        }

        public abstract void WriteObj(IBuffer buffer, object obj);
        public abstract object ReadObj(IBuffer buffer);
        protected abstract Type GetEntityType();
    }

    /// <summary>
    /// PacketStructure Serializer
    /// </summary>
    public abstract class PacketEntitySerializer<T> : EntitySerializer<T>, IStructurePacketFactory
        where T : class, IPacketStructure, new()
    {
        public IStructurePacket Create(Packet packet)
        {
            return new StructurePacket<T>(packet);
        }
    }

    /// <summary>
    /// Generic Object Serializer
    /// </summary>
    public abstract class EntitySerializer<T> : EntitySerializer where T : class, new()
    {
        public override void WriteObj(IBuffer buffer, object obj)
        {
            if (obj is T t)
            {
                Write(buffer, t);
            }
        }

        public override object ReadObj(IBuffer buffer)
        {
            return Read(buffer);
        }

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

        protected void WriteFloat(IBuffer buffer, float value)
        {
            buffer.WriteFloat(value, Endianness.Big);
        }

        protected float ReadFloat(IBuffer buffer)
        {
            return buffer.ReadFloat(Endianness.Big);
        }

        protected void WriteDouble(IBuffer buffer, double value)
        {
            buffer.WriteDouble(value, Endianness.Big);
        }

        protected double ReadDouble(IBuffer buffer)
        {
            return buffer.ReadDouble(Endianness.Big);
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

        protected void WriteInt64(IBuffer buffer, long value)
        {
            buffer.WriteInt64(value, Endianness.Big);
        }

        protected long ReadInt64(IBuffer buffer)
        {
            return buffer.ReadInt64(Endianness.Big);
        }

        protected void WriteInt32(IBuffer buffer, int value)
        {
            buffer.WriteInt32(value, Endianness.Big);
        }

        protected int ReadInt32(IBuffer buffer)
        {
            return buffer.ReadInt32(Endianness.Big);
        }

        protected void WriteInt16(IBuffer buffer, short value)
        {
            buffer.WriteInt16(value, Endianness.Big);
        }

        protected short ReadInt16(IBuffer buffer)
        {
            return buffer.ReadInt16(Endianness.Big);
        }

        protected void WriteBool(IBuffer buffer, bool value)
        {
            buffer.WriteBool(value);
        }

        protected void WriteByteArray(IBuffer buffer, byte[] value)
        {
            buffer.WriteBytes(value);
        }

        protected void WriteByte(IBuffer buffer, byte value)
        {
            buffer.WriteByte(value);
        }

        protected bool ReadBool(IBuffer buffer)
        {
            return buffer.ReadBool();
        }

        protected byte[] ReadByteArray(IBuffer buffer, int length)
        {
            return buffer.ReadBytes(length);
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

        protected void WriteEntity<TEntity>(IBuffer buffer, TEntity entity) where TEntity : class, new()
        {
            EntitySerializer<TEntity> serializer = Get<TEntity>();
            if (serializer == null)
            {
                // error
                return;
            }

            serializer.Write(buffer, entity);
        }

        public static void WriteMtArray<TEntity>(IBuffer buffer, List<TEntity> entities,
            Action<IBuffer, TEntity> writer)
        {
            buffer.WriteMtArray(entities, writer, Endianness.Big);
        }

        public static List<TEntity> ReadMtArray<TEntity>(IBuffer buffer, Func<IBuffer, TEntity> reader)
        {
            return buffer.ReadMtArray(reader, Endianness.Big);
        }

        protected void WriteEntityList<TEntity>(IBuffer buffer, List<TEntity> entities) where TEntity : class, new()
        {
            WriteUInt32(buffer, (uint) entities.Count);
            for (int i = 0; i < entities.Count; i++)
            {
                WriteEntity(buffer, entities[i]);
            }
        }

        protected List<TEntity> ReadEntityList<TEntity>(IBuffer buffer) where TEntity : class, new()
        {
            List<TEntity> entities = new List<TEntity>();
            uint len = ReadUInt32(buffer);
            for (int i = 0; i < len; i++)
            {
                entities.Add(ReadEntity<TEntity>(buffer));
            }

            return entities;
        }

        protected TEntity ReadEntity<TEntity>(IBuffer buffer) where TEntity : class, new()
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
