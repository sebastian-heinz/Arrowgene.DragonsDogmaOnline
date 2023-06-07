using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] WalletPointFields = new string[]
        {
            "character_id", "type", "value"
        };

        private readonly string SqlInsertWalletPoint = $"INSERT INTO `ddon_wallet_point` ({BuildQueryField(WalletPointFields)}) VALUES ({BuildQueryInsert(WalletPointFields)});";
        private readonly string SqlReplaceWalletPoint = $"INSERT OR REPLACE INTO `ddon_wallet_point` ({BuildQueryField(WalletPointFields)}) VALUES ({BuildQueryInsert(WalletPointFields)});";
        private static readonly string SqlUpdateWalletPoint = $"UPDATE `ddon_wallet_point` SET {BuildQueryUpdate(WalletPointFields)} WHERE `character_id`=@character_id AND `type`=@type";
        private static readonly string SqlSelectWalletPoints = $"SELECT {BuildQueryField(WalletPointFields)} FROM `ddon_wallet_point` WHERE `character_id`=@character_id;";
        private const string SqlDeleteWalletPoint = "DELETE FROM `ddon_wallet_point` WHERE `character_id`=@character_id AND `type`=@type";

        public bool InsertWalletPoint(uint characterId, CDataWalletPoint walletPoint)
        {
            return ExecuteNonQuery(SqlInsertWalletPoint, command =>
            {
                AddParameter(command, characterId, walletPoint);
            }) == 1;
        }
        
        public bool ReplaceWalletPoint(uint characterId, CDataWalletPoint walletPoint)
        {
            ExecuteNonQuery(SqlReplaceWalletPoint, command =>
            {
                AddParameter(command, characterId, walletPoint);
            });
            return true;
        }

        public bool UpdateWalletPoint(uint characterId, CDataWalletPoint updatedWalletPoint)
        {
            return ExecuteNonQuery(SqlUpdateWalletPoint, command =>
            {
                AddParameter(command, characterId, updatedWalletPoint);
            }) == 1;
        }

        public bool DeleteWalletPoint(uint characterId, WalletType type)
        {
            return ExecuteNonQuery(SqlDeleteWalletPoint, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@type", (byte) type);
            }) == 1;
        }

        private CDataWalletPoint ReadWalletPoint(DbDataReader reader)
        {
            CDataWalletPoint walletPoint = new CDataWalletPoint();
            walletPoint.Type = (WalletType) GetByte(reader, "type");
            walletPoint.Value = GetUInt32(reader, "value");
            return walletPoint;
        }

        private void AddParameter(TCom command, uint characterId, CDataWalletPoint walletPoint)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "type", (byte) walletPoint.Type);
            AddParameter(command, "value", walletPoint.Value);
        }
    }
}