using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentStatus
    {
        public CDataBattleContentStatus()
        {
            BattleContentSituationData = new CDataBattleContentSituationData();
            BattleContentAvailableRewardsList = new List<CDataBattleContentAvailableRewards>();
        }

        public CDataBattleContentSituationData BattleContentSituationData { get; set; }
        public List<CDataBattleContentAvailableRewards> BattleContentAvailableRewardsList { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentStatus>
        {
            public override void Write(IBuffer buffer, CDataBattleContentStatus obj)
            {
                WriteEntity(buffer, obj.BattleContentSituationData);
                WriteEntityList(buffer, obj.BattleContentAvailableRewardsList);
            }

            public override CDataBattleContentStatus Read(IBuffer buffer)
            {
                CDataBattleContentStatus obj = new CDataBattleContentStatus();
                obj.BattleContentSituationData = ReadEntity<CDataBattleContentSituationData>(buffer);
                obj.BattleContentAvailableRewardsList = ReadEntityList<CDataBattleContentAvailableRewards>(buffer);
                return obj;
            }
        }
    }
}


