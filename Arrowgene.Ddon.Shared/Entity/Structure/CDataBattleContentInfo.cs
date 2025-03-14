using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentInfo
    {
        public CDataBattleContentInfo() 
        {
            BattleContentStageList = new List<CDataBattleContentStage>();
            BattleContentStageProgressionList = new List<CDataBattleContentStageProgression>();
            RareItemAppraisalList = new List<CDataCommonU32>();
            ItemTakeawayList = new List<CDataCommonU32>();
        }

        public GameMode GameMode { get; set; } // GameMode 2 or KeyId 2 is active?
        public string ContentName { get; set; } = string.Empty;
        public List<CDataBattleContentStage> BattleContentStageList { get; set; } // Information about each stage of the battle content
        public List<CDataBattleContentStageProgression> BattleContentStageProgressionList {  get; set; } // Looks to define all the different battle content and how they link together
        public List<CDataCommonU32> RareItemAppraisalList { get; set; } // Makes a ! on the Lyka NPC when these items are present
        public List<CDataCommonU32> ItemTakeawayList { get; set; } // Makes a ! on the Lyka NPC when these items are present

        public class Serializer : EntitySerializer<CDataBattleContentInfo>
        {
            public override void Write(IBuffer buffer, CDataBattleContentInfo obj)
            {
                WriteUInt32(buffer, (uint) obj.GameMode);
                WriteMtString(buffer, obj.ContentName);
                WriteEntityList(buffer, obj.BattleContentStageList);
                WriteEntityList(buffer, obj.BattleContentStageProgressionList);
                WriteEntityList(buffer, obj.RareItemAppraisalList);
                WriteEntityList(buffer, obj.ItemTakeawayList);
            }

            public override CDataBattleContentInfo Read(IBuffer buffer)
            {
                CDataBattleContentInfo obj = new CDataBattleContentInfo();
                obj.GameMode = (GameMode) ReadUInt32(buffer);
                obj.ContentName = ReadMtString(buffer);
                obj.BattleContentStageList = ReadEntityList<CDataBattleContentStage>(buffer);
                obj.BattleContentStageProgressionList = ReadEntityList<CDataBattleContentStageProgression>(buffer);
                obj.RareItemAppraisalList = ReadEntityList<CDataCommonU32>(buffer);
                obj.ItemTakeawayList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
