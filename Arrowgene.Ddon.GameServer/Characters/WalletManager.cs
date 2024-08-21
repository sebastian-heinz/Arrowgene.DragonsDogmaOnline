#nullable enable
using System.Data.Common;
using System.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

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
        public bool AddToWalletNtc(Client Client, Character Character, WalletType Type, uint Amount, ItemNoticeType updateType = ItemNoticeType.Default, DbConnection? connectionIn = null)
        {
            CDataUpdateWalletPoint UpdateWalletPoint = AddToWallet(Character, Type, Amount, connectionIn);

            S2CItemUpdateCharacterItemNtc UpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            UpdateCharacterItemNtc.UpdateType = updateType;
            UpdateCharacterItemNtc.UpdateWalletList.Add(UpdateWalletPoint);

            Client.Send(UpdateCharacterItemNtc);

            return true;
        }

        public CDataUpdateWalletPoint AddToWallet(Character Character, WalletType Type, uint Amount, DbConnection? connectionIn = null)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Single(wp => wp.Type == Type);

            Wallet.Value += Amount;

            _Database.UpdateWalletPoint(Character.CharacterId, Wallet, connectionIn);

            CDataUpdateWalletPoint UpdateWalletPoint = new CDataUpdateWalletPoint();
            UpdateWalletPoint.Type = Type;
            UpdateWalletPoint.AddPoint = (int) Amount;
            UpdateWalletPoint.Value = Wallet.Value;
            return UpdateWalletPoint;
        }

        public CDataUpdateWalletPoint RemoveFromWallet(Character Character, WalletType Type, uint Amount, DbConnection? connectionIn = null)
        {
            CDataWalletPoint Wallet = Character.WalletPointList.Where(wp => wp.Type == Type).Single();

            if (Wallet.Value < Amount)
            {
                return null;
            }

            Wallet.Value -= Amount;

            _Database.UpdateWalletPoint(Character.CharacterId, Wallet, connectionIn);

            CDataUpdateWalletPoint UpdateWalletPoint = new CDataUpdateWalletPoint();
            UpdateWalletPoint.Type = Type;
            UpdateWalletPoint.AddPoint = -(int)Amount;
            UpdateWalletPoint.Value = Wallet.Value;

            return UpdateWalletPoint;
        }

        public bool RemoveFromWalletNtc(Client Client, Character Character, WalletType Type, uint Amount, DbConnection? connectionIn = null)
        {
            CDataUpdateWalletPoint UpdateWalletPoint = RemoveFromWallet(Character, Type, Amount, connectionIn);

            if(UpdateWalletPoint == null)
            {
                return false;
            }

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
