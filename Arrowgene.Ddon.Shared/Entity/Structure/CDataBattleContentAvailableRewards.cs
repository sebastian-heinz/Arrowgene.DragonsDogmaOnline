using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentAvailableRewards
    {
        public CDataBattleContentAvailableRewards()
        {
        }

        public uint Id { get; set; }
        public byte Amount { get; set; } // Might represent how many rewards are still available

        public class Serializer : EntitySerializer<CDataBattleContentAvailableRewards>
        {
            public override void Write(IBuffer buffer, CDataBattleContentAvailableRewards obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteByte(buffer, obj.Amount);
            }

            public override CDataBattleContentAvailableRewards Read(IBuffer buffer)
            {
                CDataBattleContentAvailableRewards obj = new CDataBattleContentAvailableRewards();
                obj.Id = ReadUInt32(buffer);
                obj.Amount = ReadByte(buffer);
                return obj;
            }
        }
    }
}

