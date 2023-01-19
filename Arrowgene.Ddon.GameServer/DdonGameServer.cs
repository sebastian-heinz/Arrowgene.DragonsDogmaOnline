using System.Linq;
/*
 * This file is part of Arrowgene.Ddon.GameServer
 *
 * Arrowgene.Ddon.GameServer is a server implementation for the game "Dragons Dogma Online".
 * Copyright (C) 2019-2022 DDON Team
 *
 * Github: https://github.com/sebastian-heinz/Ddo-server
 *
 * Arrowgene.Ddon.GameServer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ddon.GameServer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ddon.GameServer. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Chat;
using Arrowgene.Ddon.GameServer.Chat.Command;
using Arrowgene.Ddon.GameServer.Chat.Log;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Enemy;
using Arrowgene.Ddon.GameServer.GatheringItems;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.GameServer
{
    public class DdonGameServer : DdonServer<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DdonGameServer));

        public DdonGameServer(GameServerSetting setting, IDatabase database, AssetRepository assetRepository)
            : base(setting.ServerSetting, database, assetRepository)
        {
            LogProvider.ConfigureNamespace(typeof(DdonGameServer).Namespace, setting.ServerSetting);
            
            Setting = new GameServerSetting(setting);
            Router = new GameRouter();
            ChatManager = new ChatManager(this, Router);
            EnemyManager = new EnemyManager(assetRepository, database);
            GatheringItemManager = new GatheringItemManager(assetRepository, database);
            PartyManager = new PartyManager();
            ClientLookup = new GameClientLookup();
            ChatLogHandler = new ChatLogHandler();

            S2CStageGetStageListRes stageListPacket =
                EntitySerializer.Get<S2CStageGetStageListRes>().Read(GameDump.data_Dump_19);
            StageList = stageListPacket.StageList;

            assetRepository.AssetChanged += AssetRepositoryOnAssetChanged;
        }

        public event EventHandler<ClientConnectionChangeArgs> ClientConnectionChangeEvent;
        public GameServerSetting Setting { get; }
        public ChatManager ChatManager { get; }
        public EnemyManager EnemyManager { get; }
        public GatheringItemManager GatheringItemManager { get; }
        public PartyManager PartyManager { get; }
        public GameRouter Router { get; }

        public ChatLogHandler ChatLogHandler { get; }

        public List<CDataStageInfo> StageList { get; }

        public override GameClientLookup ClientLookup { get; }

        public override void Start()
        {
            LoadChatHandler();
            LoadPacketHandler();
            base.Start();
        }

        protected override void ClientConnected(GameClient client)
        {
            client.InitializeChallenge();

            EventHandler<ClientConnectionChangeArgs> connectionChangeEvent = ClientConnectionChangeEvent;
            if (connectionChangeEvent != null)
            {
                ClientConnectionChangeArgs connectionChangeEventArgs
                    = new ClientConnectionChangeArgs(ClientConnectionChangeArgs.EventType.CONNECT, client);
                connectionChangeEvent(this, connectionChangeEventArgs);
            }
        }

        protected override void ClientDisconnected(GameClient client)
        {
            ClientLookup.Remove(client);

            Account account = client.Account;
            if (account != null)
            {
                Database.DeleteConnection(Id, client.Account.Id);
            }

            PartyGroup party = client.Party;
            if (party != null)
            {
                party.Leave(client);
            }

            EventHandler<ClientConnectionChangeArgs> connectionChangeEvent = ClientConnectionChangeEvent;
            if (connectionChangeEvent != null)
            {
                ClientConnectionChangeArgs connectionChangeEventArgs
                    = new ClientConnectionChangeArgs(ClientConnectionChangeArgs.EventType.DISCONNECT, client);
                connectionChangeEvent(this, connectionChangeEventArgs);
            }
        }

        public override GameClient NewClient(ITcpSocket socket)
        {
            GameClient newClient = new GameClient(socket,
                new PacketFactory(Setting.ServerSetting, PacketIdResolver.GamePacketIdResolver), GatheringItemManager);
            ClientLookup.Add(newClient);
            return newClient;
        }

        public void LoadCharacterPawns(Character character)
        {
            character.Pawns = AssetRepository.MyPawnAsset.Select(myPawnCsvData => LoadPawn(character, myPawnCsvData)).ToList();
        }

        private Pawn LoadPawn(Character character, MyPawnCsv myPawnCsvData)
        {
            S2CContextGetPartyMypawnContextNtc pcapPawn = EntitySerializer.Get<S2CContextGetPartyMypawnContextNtc>().Read(SelectedDump.data_Dump_Pawn35_3_16);
            Pawn pawn = new Pawn(character.Id);
            pawn.Id = myPawnCsvData.PawnId;
            pawn.Character.Id = character.Id; // TODO
            pawn.CharacterId = character.Id; // pawns characterId, refers to the owner
            pawn.Character.Server = AssetRepository.ServerList[0];
            
            pawn.HmType = myPawnCsvData.HmType;
            pawn.PawnType = myPawnCsvData.PawnType;
            pawn.Character.FirstName = myPawnCsvData.Name;
            //pawn.Character.LastName = Pawns dont have Last Name
            pawn.Character.EditInfo = new CDataEditInfo()
            {
                Sex = myPawnCsvData.Sex,
                Voice = myPawnCsvData.Voice,
                VoicePitch = myPawnCsvData.VoicePitch,
                Personality = myPawnCsvData.Personality,
                SpeechFreq = myPawnCsvData.SpeechFreq,
                BodyType = myPawnCsvData.BodyType,
                Hair = myPawnCsvData.Hair,
                Beard = myPawnCsvData.Beard,
                Makeup = myPawnCsvData.Makeup,
                Scar = myPawnCsvData.Scar,
                EyePresetNo = myPawnCsvData.EyePresetNo,
                NosePresetNo = myPawnCsvData.NosePresetNo,
                MouthPresetNo = myPawnCsvData.MouthPresetNo,
                EyebrowTexNo = myPawnCsvData.EyebrowTexNo,
                ColorSkin = myPawnCsvData.ColorSkin,
                ColorHair = myPawnCsvData.ColorHair,
                ColorBeard = myPawnCsvData.ColorBeard,
                ColorEyebrow = myPawnCsvData.ColorEyebrow,
                ColorREye = myPawnCsvData.ColorREye,
                ColorLEye = myPawnCsvData.ColorLEye,
                ColorMakeup = myPawnCsvData.ColorMakeup,
                Sokutobu = myPawnCsvData.Sokutobu,
                Hitai = myPawnCsvData.Hitai,
                MimiJyouge = myPawnCsvData.MimiJyouge,
                Kannkaku = myPawnCsvData.Kannkaku,
                MabisasiJyouge = myPawnCsvData.MabisasiJyouge,
                HanakuchiJyouge = myPawnCsvData.HanakuchiJyouge,
                AgoSakiHaba = myPawnCsvData.AgoSakiHaba,
                AgoZengo = myPawnCsvData.AgoZengo,
                AgoSakiJyouge = myPawnCsvData.AgoSakiJyouge,
                HitomiOokisa = myPawnCsvData.HitomiOokisa,
                MeOokisa = myPawnCsvData.MeOokisa,
                MeKaiten = myPawnCsvData.MeKaiten,
                MayuKaiten = myPawnCsvData.MayuKaiten,
                MimiOokisa = myPawnCsvData.MimiOokisa,
                MimiMuki = myPawnCsvData.MimiMuki,
                ElfMimi = myPawnCsvData.ElfMimi,
                MikenTakasa = myPawnCsvData.MikenTakasa,
                MikenHaba = myPawnCsvData.MikenHaba,
                HohoboneRyou = myPawnCsvData.HohoboneRyou,
                HohoboneJyouge = myPawnCsvData.HohoboneJyouge,
                Hohoniku = myPawnCsvData.Hohoniku,
                ErahoneJyouge = myPawnCsvData.ErahoneJyouge,
                ErahoneHaba = myPawnCsvData.ErahoneHaba,
                HanaJyouge = myPawnCsvData.HanaJyouge,
                HanaHaba = myPawnCsvData.HanaHaba,
                HanaTakasa = myPawnCsvData.HanaTakasa,
                HanaKakudo = myPawnCsvData.HanaKakudo,
                KuchiHaba = myPawnCsvData.KuchiHaba,
                KuchiAtsusa = myPawnCsvData.KuchiAtsusa,
                EyebrowUVOffsetX = myPawnCsvData.EyebrowUVOffsetX,
                EyebrowUVOffsetY = myPawnCsvData.EyebrowUVOffsetY,
                Wrinkle = myPawnCsvData.Wrinkle,
                WrinkleAlbedoBlendRate = myPawnCsvData.WrinkleAlbedoBlendRate,
                WrinkleDetailNormalPower = myPawnCsvData.WrinkleDetailNormalPower,
                MuscleAlbedoBlendRate = myPawnCsvData.MuscleAlbedoBlendRate,
                MuscleDetailNormalPower = myPawnCsvData.MuscleDetailNormalPower,
                Height = myPawnCsvData.Height,
                HeadSize = myPawnCsvData.HeadSize,
                NeckOffset = myPawnCsvData.NeckOffset,
                NeckScale = myPawnCsvData.NeckScale,
                UpperBodyScaleX = myPawnCsvData.UpperBodyScaleX,
                BellySize = myPawnCsvData.BellySize,
                TeatScale = myPawnCsvData.TeatScale,
                TekubiSize = myPawnCsvData.TekubiSize,
                KoshiOffset = myPawnCsvData.KoshiOffset,
                KoshiSize = myPawnCsvData.KoshiSize,
                AnkleOffset = myPawnCsvData.AnkleOffset,
                Fat = myPawnCsvData.Fat,
                Muscle = myPawnCsvData.Muscle,
                MotionFilter = myPawnCsvData.MotionFilter
            };
            pawn.Character.Job = myPawnCsvData.Job;
            pawn.Character.HideEquipHead = myPawnCsvData.HideEquipHead;
            pawn.Character.HideEquipLantern = myPawnCsvData.HideEquipLantern;
            pawn.Character.StatusInfo = new CDataStatusInfo()
            {
                HP = (uint) pcapPawn.Context.PlayerInfo.HP,
                Stamina = (uint) pcapPawn.Context.PlayerInfo.Stamina,
                RevivePoint = pcapPawn.Context.PlayerInfo.RevivePoint,
                MaxHP = (uint) pcapPawn.Context.PlayerInfo.MaxHP,
                MaxStamina = (uint) pcapPawn.Context.PlayerInfo.MaxStamina,
                WhiteHP = (uint) pcapPawn.Context.PlayerInfo.WhiteHP,
                GainHP = pcapPawn.Context.PlayerInfo.GainHp,
                GainStamina = pcapPawn.Context.PlayerInfo.GainStamina,
                GainAttack = pcapPawn.Context.PlayerInfo.GainAttack,
                GainDefense = pcapPawn.Context.PlayerInfo.GainDefense,
                GainMagicAttack = pcapPawn.Context.PlayerInfo.GainMagicAttack,
                GainMagicDefense = pcapPawn.Context.PlayerInfo.GainMagicDefense
            };
            //pawn.Character.PlayPointList
            pawn.Character.CharacterJobDataList = new List<CDataCharacterJobData>(){ new CDataCharacterJobData {
                    Job = myPawnCsvData.Job,
                    //Exp = myPawnCsvData.Exp,
                    //JobPoint = myPawnCsvData.JobPoint,
                    Lv = myPawnCsvData.JobLv,
                    Atk = (ushort) pcapPawn.Context.PlayerInfo.Atk,
                    Def = (ushort) pcapPawn.Context.PlayerInfo.Def,
                    MAtk = (ushort) pcapPawn.Context.PlayerInfo.MAtk,
                    MDef = (ushort) pcapPawn.Context.PlayerInfo.MDef,
                    Strength = (ushort) pcapPawn.Context.PlayerInfo.Strength,
                    DownPower = (ushort) pcapPawn.Context.PlayerInfo.DownPower,
                    ShakePower = (ushort) pcapPawn.Context.PlayerInfo.ShakePower,
                    //StunPower = (ushort) pcapPawn.Context.PlayerInfo.StunPower,
                    Consitution = (ushort) pcapPawn.Context.PlayerInfo.Constitution,
                    Guts = (ushort) pcapPawn.Context.PlayerInfo.Guts,
                    FireResist = pcapPawn.Context.ResistInfo.FireResist,
                    IceResist = pcapPawn.Context.ResistInfo.IceResist,
                    ThunderResist = pcapPawn.Context.ResistInfo.ThunderResist,
                    HolyResist = pcapPawn.Context.ResistInfo.HolyResist,
                    DarkResist = pcapPawn.Context.ResistInfo.DarkResist,
                    SpreadResist = pcapPawn.Context.ResistInfo.SpreadResist,
                    FreezeResist = pcapPawn.Context.ResistInfo.FreezeResist,
                    ShockResist = pcapPawn.Context.ResistInfo.ShockResist,
                    AbsorbResist = pcapPawn.Context.ResistInfo.AbsorbResist,
                    DarkElmResist = pcapPawn.Context.ResistInfo.DarkElmResist,
                    PoisonResist = pcapPawn.Context.ResistInfo.PoisonResist,
                    SlowResist = pcapPawn.Context.ResistInfo.SlowResist,
                    SleepResist = pcapPawn.Context.ResistInfo.SleepResist,
                    StunResist = pcapPawn.Context.ResistInfo.StunResist,
                    WetResist = pcapPawn.Context.ResistInfo.WetResist,
                    OilResist = pcapPawn.Context.ResistInfo.OilResist,
                    SealResist = pcapPawn.Context.ResistInfo.SealResist,
                    CurseResist = pcapPawn.Context.ResistInfo.CurseResist,
                    SoftResist = pcapPawn.Context.ResistInfo.SoftResist,
                    StoneResist = pcapPawn.Context.ResistInfo.StoneResist,
                    GoldResist = pcapPawn.Context.ResistInfo.GoldResist,
                    FireReduceResist = pcapPawn.Context.ResistInfo.FireReduceResist,
                    IceReduceResist = pcapPawn.Context.ResistInfo.IceReduceResist,
                    ThunderReduceResist = pcapPawn.Context.ResistInfo.ThunderReduceResist,
                    HolyReduceResist = pcapPawn.Context.ResistInfo.HolyReduceResist,
                    DarkReduceResist = pcapPawn.Context.ResistInfo.DarkReduceResist,
                    AtkDownResist = pcapPawn.Context.ResistInfo.AtkDownResist,
                    DefDownResist = pcapPawn.Context.ResistInfo.DefDownResist,
                    MAtkDownResist = pcapPawn.Context.ResistInfo.MAtkDownResist,
                    MDefDownResist = pcapPawn.Context.ResistInfo.MDefDownResist,
            }};
            pawn.Character.Equipment = new Equipment(new Dictionary<JobId, Dictionary<EquipType, List<Item>>>() 
            { 
                { 
                    myPawnCsvData.Job, 
                    new Dictionary<EquipType, List<Item>> 
                    {
                        {
                            EquipType.Performance,
                            new List<Item>() {
                                new Item {
                                    ItemId = myPawnCsvData.Primary,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 0,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                        new CDataArmorCrestData {
                                            u0 = 1,
                                            u1 = 1,
                                            u2 = 0x59,
                                            u3 = 0x04
                                        }
                                    },
                                    // Empty EquipElementParamList
                                },
                                new Item {
                                    ItemId = myPawnCsvData.Secondary,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.Head,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 3,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                        new CDataArmorCrestData {
                                            u0 = 1,
                                            u1 = 1,
                                            u2 = 0x29D,
                                            u3 = 0x01
                                        }
                                    },
                                    // Empty EquipElementParamList
                                },
                                new Item {
                                    ItemId = myPawnCsvData.Body,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 4,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                        new CDataArmorCrestData {
                                            u0 = 1,
                                            u1 = 1,
                                            u2 = 0x280,
                                            u3 = 0x01
                                        }
                                    },
                                    // Empty EquipElementParamList
                                },
                                new Item {
                                    ItemId = myPawnCsvData.BodyClothing,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.Arm,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 3,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                        new CDataArmorCrestData {
                                            u0 = 1,
                                            u1 = 1,
                                            u2 = 0x1D2,
                                            u3 = 0x01
                                        }
                                    },
                                    // Empty EquipElementParamList
                                },
                                new Item {
                                    ItemId = myPawnCsvData.Leg,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 3,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    ArmorCrestDataList = new List<CDataArmorCrestData>() {
                                        new CDataArmorCrestData {
                                            u0 = 1,
                                            u1 = 1,
                                            u2 = 0x225,
                                            u3 = 0x01
                                        }
                                    },
                                    // Empty EquipElementParamList
                                },
                                new Item {
                                    ItemId = myPawnCsvData.LegWear,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.OverWear,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.JewelrySlot1,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 0,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    // Empty ArmorCrestDataList
                                    EquipElementParamList = new List<CDataEquipElementParam>() {
                                        new CDataEquipElementParam {
                                            SlotNo = 0x2,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x3,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x4,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x5,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x6,
                                            ItemId = 0x50
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x7,
                                            ItemId = 0x3C
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x8,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x9,
                                            ItemId = 0x07
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xA,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xB,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xC,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xD,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xE,
                                            ItemId = 0x00
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xF,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x10,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x11,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x12,
                                            ItemId = 0x05
                                        },
                                    }
                                },
                                new Item {
                                    ItemId = myPawnCsvData.JewelrySlot2,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 0,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    // Empty ArmorCrestDataList
                                    EquipElementParamList = new List<CDataEquipElementParam>() {
                                        new CDataEquipElementParam {
                                            SlotNo = 0x2,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x3,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x4,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x5,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x6,
                                            ItemId = 0x50
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x7,
                                            ItemId = 0x3C
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x8,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x9,
                                            ItemId = 0x07
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xA,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xB,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xC,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xD,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xE,
                                            ItemId = 0x00
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xF,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x10,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x11,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x12,
                                            ItemId = 0x05
                                        },
                                    }
                                },
                                new Item {
                                    ItemId = myPawnCsvData.JewelrySlot3,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 0,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    // Empty ArmorCrestDataList
                                    EquipElementParamList = new List<CDataEquipElementParam>() {
                                        new CDataEquipElementParam {
                                            SlotNo = 0x2,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x3,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x4,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x5,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x6,
                                            ItemId = 0x50
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x7,
                                            ItemId = 0x3C
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x8,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x9,
                                            ItemId = 0x07
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xA,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xB,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xC,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xD,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xE,
                                            ItemId = 0x00
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xF,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x10,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x11,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x12,
                                            ItemId = 0x05
                                        },
                                    }
                                },
                                new Item {
                                    ItemId = myPawnCsvData.JewelrySlot4,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 0,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    // Empty ArmorCrestDataList
                                    EquipElementParamList = new List<CDataEquipElementParam>() {
                                        new CDataEquipElementParam {
                                            SlotNo = 0x2,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x3,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x4,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x5,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x6,
                                            ItemId = 0x50
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x7,
                                            ItemId = 0x3C
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x8,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x9,
                                            ItemId = 0x07
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xA,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xB,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xC,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xD,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xE,
                                            ItemId = 0x00
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xF,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x10,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x11,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x12,
                                            ItemId = 0x05
                                        },
                                    }
                                },
                                new Item {
                                    ItemId = myPawnCsvData.JewelrySlot5,
                                    Unk3 = 0,
                                    Color = 0,
                                    PlusValue = 0,
                                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                                    // Empty ArmorCrestDataList
                                    EquipElementParamList = new List<CDataEquipElementParam>() {
                                        new CDataEquipElementParam {
                                            SlotNo = 0x2,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x3,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x4,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x5,
                                            ItemId = 0x02
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x6,
                                            ItemId = 0x50
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x7,
                                            ItemId = 0x3C
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x8,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x9,
                                            ItemId = 0x07
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xA,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xB,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xC,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xD,
                                            ItemId = 0x04
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xE,
                                            ItemId = 0x00
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0xF,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x10,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x11,
                                            ItemId = 0x05
                                        },
                                        new CDataEquipElementParam {
                                            SlotNo = 0x12,
                                            ItemId = 0x05
                                        },
                                    }
                                },
                                new Item {
                                    ItemId = myPawnCsvData.Lantern,
                                    Unk3 = 0,
                                }
                            }
                        },
                        {
                            EquipType.Visual,
                            new List<Item>() {
                                new Item {
                                    ItemId = myPawnCsvData.VPrimary,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.VSecondary,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.VHead,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.VBody,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.VBodyClothing,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.VArm,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.VLeg,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.VLegWear,
                                    Unk3 = 0,
                                    Color = 0
                                },
                                new Item {
                                    ItemId = myPawnCsvData.VOverWear,
                                    Unk3 = 0,
                                    Color = 0,
                                },
                                null,
                                null,
                                null,
                                null,
                                null,
                                null
                            }
                        }
                    }
                }
            });
            pawn.Character.CharacterEquipJobItemListDictionary = new Dictionary<JobId, List<CDataEquipJobItem>>() { { myPawnCsvData.Job, new List<CDataEquipJobItem>() {
                new CDataEquipJobItem {
                    JobItemId = myPawnCsvData.JobItem1,
                    EquipSlotNo = myPawnCsvData.JobItemSlot1
                },
                new CDataEquipJobItem {
                    JobItemId = myPawnCsvData.JobItem2,
                    EquipSlotNo = myPawnCsvData.JobItemSlot2
                }
            }}};
            pawn.Character.NormalSkills = new List<CDataNormalSkillParam>() {
                new CDataNormalSkillParam() {
                    Job = myPawnCsvData.Job,
                    SkillNo = myPawnCsvData.NormalSkill1,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = myPawnCsvData.Job,
                    SkillNo = myPawnCsvData.NormalSkill2,
                    Index = 0,
                    PreSkillNo = 0
                },
                new CDataNormalSkillParam() {
                    Job = myPawnCsvData.Job,
                    SkillNo = myPawnCsvData.NormalSkill3,
                    Index = 0,
                    PreSkillNo = 0
                }
            };
            pawn.Character.CustomSkills = new List<CustomSkill>() {
                // Main Palette
                new CustomSkill() {
                    Job = myPawnCsvData.Job,
                    SlotNo = 1,
                    SkillId = myPawnCsvData.CustomSkillId1,
                    SkillLv = myPawnCsvData.CustomSkillLv1
                },
                new CustomSkill() {
                    Job = myPawnCsvData.Job,
                    SlotNo = 2,
                    SkillId = myPawnCsvData.CustomSkillId2,
                    SkillLv = myPawnCsvData.CustomSkillLv2
                },
                new CustomSkill() {
                    Job = myPawnCsvData.Job,
                    SlotNo = 3,
                    SkillId = myPawnCsvData.CustomSkillId3,
                    SkillLv = myPawnCsvData.CustomSkillLv3
                },
                new CustomSkill() {
                    Job = myPawnCsvData.Job,
                    SlotNo = 4,
                    SkillId = myPawnCsvData.CustomSkillId4,
                    SkillLv = myPawnCsvData.CustomSkillLv4
                }
            };
            pawn.Character.Abilities = new List<Ability>() {
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 1,
                    AbilityId = myPawnCsvData.AbilityId1,
                    AbilityLv = myPawnCsvData.AbilityLv1
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 2,
                    AbilityId = myPawnCsvData.AbilityId2,
                    AbilityLv = myPawnCsvData.AbilityLv2
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 3,
                    AbilityId = myPawnCsvData.AbilityId3,
                    AbilityLv = myPawnCsvData.AbilityLv3
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 4,
                    AbilityId = myPawnCsvData.AbilityId4,
                    AbilityLv = myPawnCsvData.AbilityLv4
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 5,
                    AbilityId = myPawnCsvData.AbilityId5,
                    AbilityLv = myPawnCsvData.AbilityLv5
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 6,
                    AbilityId = myPawnCsvData.AbilityId6,
                    AbilityLv = myPawnCsvData.AbilityLv6
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 7,
                    AbilityId = myPawnCsvData.AbilityId7,
                    AbilityLv = myPawnCsvData.AbilityLv7
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 8,
                    AbilityId = myPawnCsvData.AbilityId8,
                    AbilityLv = myPawnCsvData.AbilityLv8
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 9,
                    AbilityId = myPawnCsvData.AbilityId9,
                    AbilityLv = myPawnCsvData.AbilityLv9
                },
                new Ability() {
                    EquippedToJob = myPawnCsvData.Job,
                    Job = 0,
                    SlotNo = 10,
                    AbilityId = myPawnCsvData.AbilityId10,
                    AbilityLv = myPawnCsvData.AbilityLv10
                }
            };
            pawn.PawnReactionList = new List<CDataPawnReaction>()
            {
                new CDataPawnReaction()
                {
                    ReactionType = 1,
                    MotionNo = myPawnCsvData.MetPartyMembersId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 2,
                    MotionNo = myPawnCsvData.QuestClearId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 10,
                    MotionNo = myPawnCsvData.SpecialSkillInspirationMomentId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 4,
                    MotionNo = myPawnCsvData.LevelUpId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 11,
                    MotionNo = myPawnCsvData.SpecialSkillUseId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 6,
                    MotionNo = myPawnCsvData.PlayerDeathId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 7,
                    MotionNo = myPawnCsvData.WaitingOnLobbyId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 8,
                    MotionNo = myPawnCsvData.WaitingOnAdventureId
                },
                new CDataPawnReaction()
                {
                    ReactionType = 9,
                    MotionNo = myPawnCsvData.EndOfCombatId
                }
            };
            pawn.SpSkillList = new List<CDataSpSkill>()
            {
                new CDataSpSkill()
                {
                    SpSkillId = myPawnCsvData.SpSkillSlot1Id,
                    SpSkillLv = myPawnCsvData.SpSkillSlot1Lv
                },
                new CDataSpSkill()
                {
                    SpSkillId = myPawnCsvData.SpSkillSlot2Id,
                    SpSkillLv = myPawnCsvData.SpSkillSlot2Lv
                },
                new CDataSpSkill()
                {
                    SpSkillId = myPawnCsvData.SpSkillSlot3Id,
                    SpSkillLv = myPawnCsvData.SpSkillSlot3Lv
                }
            };
            return pawn;
        }

        private void AssetRepositoryOnAssetChanged(object sender, AssetChangedEventArgs e)
        {
            if(e.Key == AssetRepository.MyPawnAssetKey)
            {
                // Reload all client's pawns when the MyPawn asset csv has changed.
                // TODO: Remove when we implement pawns properly in DB.
                foreach (Character character in ClientLookup.GetAllCharacter())
                {
                    LoadCharacterPawns(character);
                }
            }
        }

        private void LoadChatHandler()
        {
            ChatManager.AddHandler(ChatLogHandler);
            ChatManager.AddHandler(new ChatCommandHandler(this));
        }

        private void LoadPacketHandler()
        {
            AddHandler(new AchievementAchievementGetReceivableRewardListHandler(this));

            AddHandler(new AreaGetAreaBaseInfoListHandler(this));
            AddHandler(new AreaGetAreaQuestHintListHandler(this));
            AddHandler(new AreaGetLeaderAreaReleaseListHandler(this));

            AddHandler(new BattleContentInfoListHandler(this));
            AddHandler(new BinarySaveSetCharacterBinSavedataHandler(this));
            AddHandler(new BlackListGetBlackListHandler(this));

            AddHandler(new ActionSetPlayerActionHistoryHandler(this));

            AddHandler(new CharacterCommunityCharacterStatusGetHandler(this));
            AddHandler(new CharacterDecideCharacterIdHandler(this));
            AddHandler(new CharacterCharacterGoldenReviveHandler(this));
            AddHandler(new CharacterCharacterPenaltyReviveHandler(this));
            AddHandler(new CharacterCharacterPointReviveHandler(this));
            AddHandler(new CharacterCharacterSearchHandler(this));
            AddHandler(new CharacterPawnGoldenReviveHandler(this));
            AddHandler(new CharacterPawnPointReviveHandler(this));
            AddHandler(new CharacterSetOnlineStatusHandler(this));

            AddHandler(new ClanClanBaseGetInfoHandler(this));
            AddHandler(new ClanClanConciergeGetListHandler(this));
            AddHandler(new ClanClanConciergeUpdateHandler(this));
            AddHandler(new ClanClanGetJoinRequestedListHandler(this));
            AddHandler(new ClanClanGetMyInfoHandler(this));
            AddHandler(new ClanClanGetMyMemberListHandler(this));
            AddHandler(new ClanClanPartnerPawnDataGetHandler(this));
            AddHandler(new ClanClanSettingUpdateHandler(this));
            AddHandler(new ClanGetFurnitureHandler(this));
            AddHandler(new ClanSetFurnitureHandler(this));

            AddHandler(new ClientChallengeHandler(this));

            AddHandler(new ConnectionGetLoginAnnouncementHandler(this));
            AddHandler(new ConnectionLoginHandler(this));
            AddHandler(new ConnectionLogoutHandler(this));
            AddHandler(new ConnectionMoveInServerHandler(this));
            AddHandler(new ConnectionMoveOutServerHandler(this));
            AddHandler(new ConnectionPingHandler(this));
            AddHandler(new ConnectionReserveServerHandler(this));

            AddHandler(new ContextGetSetContextHandler(this));
            AddHandler(new ContextSetContextHandler(this));
            AddHandler(new CraftGetCraftProgressListHandler(this));
            AddHandler(new DailyMissionListGetHandler(this));

            AddHandler(new EquipChangeCharacterEquipHandler(this));
            AddHandler(new EquipChangeCharacterStorageEquipHandler(this));
            AddHandler(new EquipGetCharacterEquipListHandler(this));
            AddHandler(new EquipUpdateHideCharacterHeadArmorHandler(this));
            AddHandler(new EquipUpdateHideCharacterLanternHandler(this));
            AddHandler(new EquipUpdateHidePawnHeadArmorHandler(this));
            AddHandler(new EquipUpdateHidePawnLanternHandler(this));

            AddHandler(new FriendGetFriendListHandler(this));
            AddHandler(new FriendGetRecentCharacterListHandler(this));


            AddHandler(new Gp_28_2_1_Handler(this));
            AddHandler(new GpGetUpdateAppCourseBonusFlagHandler(this));
            AddHandler(new GpGetValidChatComGroupHandler(this));

            AddHandler(new GroupChatGroupChatGetMemberListHandler(this));

            AddHandler(new InnGetStayPriceHandler(this));
            AddHandler(new InnStayInnHandler(this));

            AddHandler(new InstanceEnemyGroupEntryHandler(this));
            AddHandler(new InstanceEnemyGroupLeaveHandler(this));
            AddHandler(new InstanceEnemyKillHandler(this));
            AddHandler(new InstanceExchangeOmInstantKeyValueHandler(this));
            AddHandler(new InstanceGetEnemySetListHandler(this));
            AddHandler(new InstanceGetGatheringItemHandler(this));
            AddHandler(new InstanceGetGatheringItemListHandler(this));
            AddHandler(new InstanceGetItemSetListHandler(this));
            AddHandler(new InstanceSetOmInstantKeyValueHandler(this));
            AddHandler(new InstanceTreasurePointGetCategoryListHandler(this));
            AddHandler(new InstanceTreasurePointGetListHandler(this));

            AddHandler(new InstanceGetOmInstantKeyValueAllHandler(this));

            AddHandler(new ItemGetStorageItemListHandler(this));
            AddHandler(new ItemMoveItemHandler(this));
            AddHandler(new ItemSortGetItemSortDataBinHandler(this));
            AddHandler(new ItemSortSetItemSortDataBinHandler(this));
            AddHandler(new ItemUseBagItemHandler(this));

            AddHandler(new JobChangeJobHandler(this));
            AddHandler(new JobGetJobChangeListHandler(this));
            AddHandler(new JobUpdateExpModeHandler(this));

            AddHandler(new LoadingInfoLoadingGetInfoHandler(this));

            AddHandler(new LobbyLobbyJoinHandler(this));
            AddHandler(new LobbyLobbyLeaveHandler(this));
            AddHandler(new LobbyLobbyChatMsgHandler(this));
            AddHandler(new LobbyLobbyDataMsgHandler(this));

            AddHandler(new MailMailGetListDataHandler(this));
            AddHandler(new MailMailGetListFootHandler(this));
            AddHandler(new MailMailGetListHeadHandler(this));
            AddHandler(new MailSystemMailGetListDataHandler(this));
            AddHandler(new MailSystemMailGetListFootHandler(this));
            AddHandler(new MailSystemMailGetListHeadHandler(this));

            AddHandler(new MandragoraGetMyMandragoraHandler(this));

            AddHandler(new MyRoomFurnitureListGetHandler(this));
            AddHandler(new MyRoomMyRoomBgmUpdateHandler(this));
            AddHandler(new MyRoomUpdatePlanetariumHandler(this));

            AddHandler(new NpcGetExtendedFacilityHandler(this));

            AddHandler(new OrbDevoteGetOrbGainExtendParamHandler(this));

            AddHandler(new PartnerPawnPawnLikabilityReleasedRewardListGetHandler(this));
            AddHandler(new PartnerPawnPawnLikabilityRewardListGetHandler(this));

            AddHandler(new PartyPartyBreakupHandler(this));
            AddHandler(new PartyPartyChangeLeaderHandler(this));
            AddHandler(new PartyPartyCreateHandler(this));
            AddHandler(new PartyPartyInviteCancelHandler(this));
            AddHandler(new PartyPartyInviteCharacterHandler(this));
            AddHandler(new PartyPartyInviteEntryHandler(this));
            AddHandler(new PartyPartyInvitePrepareAcceptHandler(this));
            AddHandler(new PartyPartyInviteRefuseHandler(this));
            AddHandler(new PartyPartyJoinHandler(this));
            AddHandler(new PartyPartyLeaveHandler(this));
            AddHandler(new PartyPartyMemberKickHandler(this));
            AddHandler(new PartySendBinaryMsgAllHandler(this));
            AddHandler(new PartySendBinaryMsgHandler(this));

            AddHandler(new PawnGetLostPawnListHandler(this));
            AddHandler(new PawnGetMypawnDataHandler(this));
            AddHandler(new PawnGetMyPawnListHandler(this));
            AddHandler(new PawnGetNoraPawnListHandler(this));
            AddHandler(new PawnGetPawnHistoryListHandler(this));
            AddHandler(new PawnGetPawnTotalScoreHandler(this));
            AddHandler(new PawnGetRegisteredPawnDataHandler(this));
            AddHandler(new PawnGetRentedPawnListHandler(this));
            AddHandler(new PawnJoinPartyMypawnHandler(this));
            AddHandler(new PawnTrainingGetPreparetionInfoToAdviceHandler(this));
            
            AddHandler(new ProfileGetCharacterProfileHandler(this));
            AddHandler(new ProfileGetMyCharacterProfileHandler(this));
            
            AddHandler(new QuestEndDistributionQuestCancelHandler(this));
            AddHandler(new QuestGetAdventureGuideQuestListHandler(this));
            AddHandler(new QuestGetAdventureGuideQuestNoticeHandler(this));
            AddHandler(new QuestGetAreaBonusListHandler(this));
            AddHandler(new QuestGetCycleContentsStateListHandler(this));
            AddHandler(new QuestGetLevelBonusListHandler(this));
            AddHandler(new QuestGetLotQuestListHandler(this));
            AddHandler(new QuestGetMainQuestListHandler(this));
            AddHandler(new QuestGetPackageQuestListHandler(this));
            AddHandler(new QuestGetPartyQuestProgressInfoHandler(this));
            AddHandler(new QuestGetPriorityQuestHandler(this));
            AddHandler(new QuestGetQuestCompletedListHandler(this));
            AddHandler(new QuestGetQuestPartyBonusListHandler(this));
            AddHandler(new QuestGetSetQuestListHandler(this));
            AddHandler(new QuestGetTutorialQuestListHandler(this));
            AddHandler(new QuestGetWorldManageQuestListHandler(this));
            AddHandler(new QuestQuestProgressHandler(this));
            AddHandler(new QuestSendLeaderQuestOrderConditionInfoHandler(this));
            AddHandler(new QuestSendLeaderWaitOrderQuestListHandler(this));

            AddHandler(new ServerGameTimeGetBaseinfoHandler(this));
            AddHandler(new ServerGetGameSettingHandler(this));
            AddHandler(new ServerGetRealTimeHandler(this));
            AddHandler(new ServerGetServerListHandler(this));

            AddHandler(new SkillChangeExSkillHandler(this));
            AddHandler(new SkillGetAbilityCostHandler(this));
            AddHandler(new SkillGetAcquirableAbilityListHandler(this));
            AddHandler(new SkillGetAcquirableSkillListHandler(this));
            AddHandler(new SkillGetCurrentSetSkillListHandler(this));
            AddHandler(new SkillGetLearnedAbilityListHandler(this));
            AddHandler(new SkillGetLearnedNormalSkillListHandler(this));
            AddHandler(new SkillGetLearnedSkillListHandler(this));
            AddHandler(new SkillGetPresetAbilityListHandler(this));
            AddHandler(new SkillGetSetAbilityListHandler(this));
            AddHandler(new SkillGetSetSkillListHandler(this));
            AddHandler(new SkillSetAbilityHandler(this));
            AddHandler(new SkillSetOffAbilityHandler(this));
            AddHandler(new SkillSetOffSkillHandler(this));
            AddHandler(new SkillSetSkillHandler(this));
            AddHandler(new SetShortcutHandler(this));
            AddHandler(new SetCommunicationShortcutHandler(this));

            AddHandler(new StageAreaChangeHandler(this));
            AddHandler(new StageGetStageListHandler(this));

            AddHandler(new StampBonusCheckHandler(this));

            AddHandler(new WarpAreaWarpHandler(this));
            AddHandler(new WarpGetFavoriteWarpPointListHandler(this));
            AddHandler(new WarpGetReleaseWarpPointListHandler(this));
            AddHandler(new WarpGetReturnLocationHandler(this));
            AddHandler(new WarpGetStartPointListHandler(this));
            AddHandler(new WarpGetWarpPointListHandler(this));
            AddHandler(new WarpRegisterFavoriteWarpHandler(this));
            AddHandler(new WarpReleaseWarpPointHandler(this));
            AddHandler(new WarpWarpHandler(this));
        }
    }
}
