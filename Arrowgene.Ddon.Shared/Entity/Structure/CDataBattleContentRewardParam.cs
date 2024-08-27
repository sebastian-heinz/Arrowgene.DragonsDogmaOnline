using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentRewardParam
    {
        public CDataBattleContentRewardParam()
        {
        }

        public WalletType WalletType {  get; set; }
        public uint Amount { get; set; }
        public uint Bonus { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentRewardParam>
        {
            public override void Write(IBuffer buffer, CDataBattleContentRewardParam obj)
            {
                WriteByte(buffer, (byte) obj.WalletType);
                WriteUInt32(buffer, obj.Amount);
                WriteUInt32(buffer, obj.Bonus);
            }

            public override CDataBattleContentRewardParam Read(IBuffer buffer)
            {
                CDataBattleContentRewardParam obj = new CDataBattleContentRewardParam();
                obj.WalletType = (WalletType) ReadByte(buffer);
                obj.Amount = ReadUInt32(buffer);
                obj.Bonus = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
