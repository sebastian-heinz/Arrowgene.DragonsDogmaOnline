using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLockCostData
    {
        public uint Unk0 { get; set; }
        /// <summary>
        /// This is interpreted strangely; the UI will always talk about Red Dragon Marks, 
        /// but the check for having enough currency will use whatever you provide here.
        /// </summary>
        public WalletType WalletType { get; set; }
        public uint Cost { get; set; }


        public class Serializer : EntitySerializer<CDataDispelLockCostData>
        {
            public override void Write(IBuffer buffer, CDataDispelLockCostData obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, (byte)obj.WalletType);
                WriteUInt32(buffer, obj.Cost);

            }

            public override CDataDispelLockCostData Read(IBuffer buffer)
            {
                CDataDispelLockCostData obj = new CDataDispelLockCostData();
                obj.Unk0 = ReadUInt32(buffer);
                obj.WalletType = (WalletType)ReadByte(buffer);
                obj.Cost = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
