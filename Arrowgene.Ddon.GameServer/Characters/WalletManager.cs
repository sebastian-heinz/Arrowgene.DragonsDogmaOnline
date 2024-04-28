using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using static Arrowgene.Ddon.Server.Network.Challenge;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class WalletManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WalletManager));

        private IDatabase _Database;

        public WalletManager(IDatabase Database)
        {
            _Database = Database;
        }
        public bool AddToWalletNtc(Client Client, Character Character, WalletType Type, uint Amount)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Where(wp => wp.Type == Type).Single();

            Wallet.Value += Amount;

            _Database.UpdateWalletPoint(Character.CharacterId, Wallet);

            CDataUpdateWalletPoint UpdateWalletPoint = new CDataUpdateWalletPoint();
            UpdateWalletPoint.Type = Type;
            UpdateWalletPoint.AddPoint = (int) Amount;
            UpdateWalletPoint.Value = Wallet.Value;

            S2CItemUpdateCharacterItemNtc UpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            UpdateCharacterItemNtc.UpdateType = 0;
            UpdateCharacterItemNtc.UpdateWalletList.Add(UpdateWalletPoint);

            Client.Send(UpdateCharacterItemNtc);

            return true;
        }

        public bool AddToWallet(Character Character, WalletType Type, uint Amount)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Where(wp => wp.Type == Type).Single();

            Wallet.Value += Amount;

            _Database.UpdateWalletPoint(Character.CharacterId, Wallet);

            return true;
        }

        public bool RemoveFromWallet(Character Character, WalletType Type, uint Amount)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Where(wp => wp.Type == Type).Single();

            if (Wallet.Value < Amount)
            {
                return false;
            }

            Wallet.Value -= Amount;

            _Database.UpdateWalletPoint(Character.CharacterId, Wallet);

            return true;
        }

        public bool RemoveFromWalletNtc(Client Client, Character Character, WalletType Type, uint Amount)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Where(wp => wp.Type == Type).Single();

            if (Wallet.Value < Amount)
            {
                return false;
            }

            Wallet.Value -= Amount;

            _Database.UpdateWalletPoint(Character.CharacterId, Wallet);

            CDataUpdateWalletPoint UpdateWalletPoint = new CDataUpdateWalletPoint();
            UpdateWalletPoint.Type = Type;
            UpdateWalletPoint.AddPoint = -(int)Amount;
            UpdateWalletPoint.Value = Wallet.Value;

            S2CItemUpdateCharacterItemNtc UpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            UpdateCharacterItemNtc.UpdateType = 0;
            UpdateCharacterItemNtc.UpdateWalletList.Add(UpdateWalletPoint);

            Client.Send(UpdateCharacterItemNtc);

            return true;
        }

        public uint GetWalletAmount(Character Character, WalletType Type)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Where(wp => wp.Type == Type).Single();
            return Wallet.Value;
        }
    }
}
