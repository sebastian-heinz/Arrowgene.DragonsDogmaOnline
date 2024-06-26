using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterSwitchGameModeHandler : GameRequestPacketHandler<C2SCharacterSwitchGameModeReq, S2CCharacterSwitchGameModeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterSwitchGameModeHandler));

        public CharacterSwitchGameModeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterSwitchGameModeRes Handle(GameClient client, C2SCharacterSwitchGameModeReq packet)
        {
            Logger.Info($"GameMode={packet.Unk0}");

            S2CCharacterSwitchGameModeNtc ntc = new S2CCharacterSwitchGameModeNtc()
            {
                Unk0 = (uint) packet.Unk0,
                Unk1 = false,

                CharacterInfo = new CDataCharacterInfo()
                {
                    CharacterId = client.Character.CharacterId,
                    UserId = client.Character.UserId,
                    Version = client.Character.Version,
                    FirstName = client.Character.FirstName,
                    LastName = client.Character.LastName,
                    EditInfo = client.Character.EditInfo,
                    StatusInfo = client.Character.StatusInfo,
                    Job = client.Character.Job,
                    CharacterJobDataList = client.Character.CharacterJobDataList,
                    PlayPointList = client.Character.PlayPointList,
                    CharacterEquipDataList = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                        Equips = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Performance)
                    }},
                    CharacterEquipViewDataList = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                        Equips = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Visual)
                    }},
                    CharacterEquipJobItemList = client.Character.Equipment.getJobItemsAsCDataEquipJobItem(client.Character.Job),
                    JewelrySlotNum = client.Character.JewelrySlotNum,
                    // Unk0
                    // CharacterItemSlotInfo
                    WalletPointList = client.Character.WalletPointList,
                    MyPawnSlotNum = client.Character.MyPawnSlotNum,
                    RentalPawnSlotNum = client.Character.RentalPawnSlotNum,
                    OrbStatusList = client.Character.OrbStatusList,
                    MsgSetList = client.Character.MsgSetList,
                    ShortCutList = client.Character.ShortCutList,
                    CommunicationShortCutList = client.Character.CommunicationShortCutList,
                    MatchingProfile = client.Character.MatchingProfile,
                    ArisenProfile = client.Character.ArisenProfile,
                    HideEquipHead = client.Character.HideEquipHead,
                    HideEquipLantern = client.Character.HideEquipLantern,
                    HideEquipHeadPawn = client.Character.HideEquipHeadPawn,
                    HideEquipLanternPawn = client.Character.HideEquipLanternPawn,
                    ArisenProfileShareRange = client.Character.ArisenProfileShareRange,
                    OnlineStatus = client.Character.OnlineStatus
                }
            };
            client.Send(ntc);

            return new S2CCharacterSwitchGameModeRes()
            {
                Unk0 = packet.Unk0,
            };
        }
    }
}
