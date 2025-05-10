#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class WalletManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WalletManager));

        private readonly DdonGameServer Server;
        private readonly Dictionary<WalletType, uint> WalletLimits;

        public WalletManager(DdonGameServer server)
        {
            Server = server;
            WalletLimits = server.GameSettings.GameServerSettings.WalletLimits;
        }

        public S2CItemUpdateCharacterItemNtc AddToWalletNtc(Client Client, Character Character, WalletType Type, uint Amount,  uint BonusAmount = 0, ItemNoticeType updateType = ItemNoticeType.Default, DbConnection? connectionIn = null)
        {
            CDataUpdateWalletPoint UpdateWalletPoint = AddToWallet(Character, Type, Amount, BonusAmount, connectionIn);

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = updateType;
            updateCharacterItemNtc.UpdateWalletList.Add(UpdateWalletPoint);

            return updateCharacterItemNtc;
        }

        public CDataUpdateWalletPoint AddToWallet(Character Character, WalletType Type, uint Amount, uint BonusAmount = 0, DbConnection? connectionIn = null)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Single(wp => wp.Type == Type);

            if (Wallet.Value < WalletLimits[Type])
            {
                Wallet.Value = Math.Min(Wallet.Value + Amount, WalletLimits[Type]);
            }

            Server.Database.UpdateWalletPoint(Character.CharacterId, Wallet, connectionIn);

            CDataUpdateWalletPoint UpdateWalletPoint = new CDataUpdateWalletPoint();
            UpdateWalletPoint.Type = Type;
            UpdateWalletPoint.AddPoint = (int) Amount;
            UpdateWalletPoint.ExtraBonusPoint = BonusAmount;
            UpdateWalletPoint.Value = Wallet.Value;
            return UpdateWalletPoint;
        }

        public CDataUpdateWalletPoint RemoveFromWallet(Character Character, WalletType Type, uint Amount, DbConnection? connectionIn = null)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Where(wp => wp.Type == Type).SingleOrDefault()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_WALLET, $"Invalid wallet type {Type} for character {Character.CharacterId}");

            if (Wallet.Value < Amount)
            {
                return null;
            }

            Wallet.Value -= Amount;

            Server.Database.UpdateWalletPoint(Character.CharacterId, Wallet, connectionIn);

            CDataUpdateWalletPoint UpdateWalletPoint = new CDataUpdateWalletPoint();
            UpdateWalletPoint.Type = Type;
            UpdateWalletPoint.AddPoint = -(int)Amount;
            UpdateWalletPoint.Value = Wallet.Value;

            return UpdateWalletPoint;
        }

        public bool RemoveFromWalletNtc(Client client, Character character, WalletType walletType, uint amount, DbConnection? connectionIn = null)
        {
            client.Send(RemoveFromWalletNtc2(character, walletType, amount, connectionIn));
            return true;
        }

        public S2CItemUpdateCharacterItemNtc RemoveFromWalletNtc2(Character character, WalletType walletType, uint amount, DbConnection? connectionIn = null)
        {
            CDataUpdateWalletPoint UpdateWalletPoint = RemoveFromWallet(character, walletType, amount, connectionIn) ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_WALLET, $"Unable to remove {amount} from {walletType}");

            S2CItemUpdateCharacterItemNtc UpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            UpdateCharacterItemNtc.UpdateType = 0;
            UpdateCharacterItemNtc.UpdateWalletList.Add(UpdateWalletPoint);

            return UpdateCharacterItemNtc;
        }

        public uint GetWalletAmount(Character Character, WalletType Type)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Where(wp => wp.Type == Type).SingleOrDefault()
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_WALLET, $"Invalid wallet type {Type} for character {Character.CharacterId}");
            return Wallet.Value;
        }

        public uint GetScaledWalletAmount(WalletType type, uint amount)
        {
            double modifier = 1.0;
            switch (type)
            {
                case WalletType.Gold:
                    modifier = Server.GameSettings.GameServerSettings.GoldModifier;
                    break;
                case WalletType.RiftPoints:
                    modifier = Server.GameSettings.GameServerSettings.RiftModifier;
                    break;
                case WalletType.BloodOrbs:
                    modifier = Server.GameSettings.GameServerSettings.BoModifier;
                    break;
                case WalletType.HighOrbs:
                    modifier = Server.GameSettings.GameServerSettings.HoModifier;
                    break;
                default:
                    modifier = 1.0;
                    break;
            }

            return (uint)(amount * modifier);
        }
    }
}
