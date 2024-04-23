using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class CharacterManager
    {
        public static readonly uint BASE_HEALTH = 760U;
        public static readonly uint BASE_STAMINA = 750U;

        private readonly IDatabase _Database;

        public CharacterManager(IDatabase Database) 
        {
            _Database = Database;
        }

        public void UpdateCharacterExtendedParams(Character Character)
        {
            var ExtendedParams = Character.ExtendedParams;

            /**
             * There are two physical attack traits and two magic attack traits in
             * the stats menu. This corresponds with the attack of the weapon and
             * the iniate attack of the character on the currently selected job.
             * Similar distinction made with magic. The Gain* stats are extra stats
             * on top of the iniate stats. These come from armors and BO/HO trees.
             */
            Character.StatusInfo.GainAttack = ExtendedParams.Attack;
            Character.StatusInfo.GainDefense = ExtendedParams.Defence;
            Character.StatusInfo.GainMagicAttack = ExtendedParams.MagicAttack;
            Character.StatusInfo.GainMagicDefense = ExtendedParams.MagicDefence;
            Character.StatusInfo.GainStamina = ExtendedParams.StaminaMax;
            Character.StatusInfo.GainHP = ExtendedParams.HpMax;

            // These values reflect in the UI what the health bar displays
            // TODO: Remove this when C2SLobbyLobbyDataMsgReq parsing is supported
            // TODO: Server alone is not able to account for HP added by gear
            // TODO: So we need to "trust" the value sent to the server is correct
            Character.StatusInfo.HP = CharacterManager.BASE_HEALTH + ExtendedParams.HpMax;
            Character.StatusInfo.WhiteHP = CharacterManager.BASE_HEALTH + ExtendedParams.HpMax;
        }

        public void UpdateCharacterExtendedParamsNtc(GameClient Client, Character Character)
        {
            UpdateCharacterExtendedParams(Character);
            NotifyClientOfCharacterStatus(Client, Character);
        }

        private void NotifyClientOfCharacterStatus(GameClient Client, Character Character)
        {
            S2CContextGetLobbyPlayerContextNtc ntc1 = new S2CContextGetLobbyPlayerContextNtc();
            GameStructure.S2CContextGetLobbyPlayerContextNtc(ntc1, Character);
            Client.Send(ntc1);

            S2CCharacterGetCharacterStatusNtc ntc2 = new S2CCharacterGetCharacterStatusNtc
            {
                CharacterId = Character.CharacterId,
                HideHead = Character.HideEquipHead,
                HideLantern = Character.HideEquipLantern,
                JewelryNum = (byte)(Character.JewelrySlotNum + Character.ExtendedParams.JewelrySlot),
                StatusInfo = Character.StatusInfo,
                JobParam = Character.ActiveCharacterJobData
            };

            GameStructure.CDataCharacterLevelParam(ntc2.CharacterParam, Character);
            ntc2.EditInfo = Character.EditInfo;
            ntc2.EquipDataList = Character.Equipment.getEquipmentAsCDataEquipItemInfo(Character.Job, EquipType.Performance);
            ntc2.VisualEquipDataList = Character.Equipment.getEquipmentAsCDataEquipItemInfo(Character.Job, EquipType.Visual);
            ntc2.EquipJobItemList = Character.Equipment.getJobItemsAsCDataEquipJobItem(Character.Job);

            // Client.Party.SendToAll(ntc);
            Client.Send(ntc2);
        }
    }
}
