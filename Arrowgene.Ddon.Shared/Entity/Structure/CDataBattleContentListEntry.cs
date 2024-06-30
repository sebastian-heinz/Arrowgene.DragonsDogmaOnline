using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentListEntry
    {
        public CDataBattleContentListEntry() 
        {
            BattleContentStageList = new List<CDataBattleContentStage>();
            BattleContentParameters = new List<CDataBattleContentParameters>();
            AppraisalItemList = new List<CDataCommonU32>();
            Unk3 = new List<CDataCommonU32>();
        }

        public uint Unk0 { get; set; } // GameMode 2 or KeyId 2 is active?
        public string ContentName { get; set; }
        public List<CDataBattleContentStage> BattleContentStageList { get; set; }
        public List<CDataBattleContentParameters> BattleContentParameters {  get; set; }
        public List<CDataCommonU32> AppraisalItemList { get; set; } // Appears to be items like "Bitterblack Deed Box"
        public List<CDataCommonU32> Unk3 { get; set; } // Appears to be items/rewards like "Fruit of Inspiration (Concentration)"

        public class Serializer : EntitySerializer<CDataBattleContentListEntry>
        {
            public override void Write(IBuffer buffer, CDataBattleContentListEntry obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.ContentName);
                WriteEntityList(buffer, obj.BattleContentStageList);
                WriteEntityList(buffer, obj.BattleContentParameters);
                WriteEntityList(buffer, obj.AppraisalItemList);
                WriteEntityList(buffer, obj.Unk3);
            }

            public override CDataBattleContentListEntry Read(IBuffer buffer)
            {
                CDataBattleContentListEntry obj = new CDataBattleContentListEntry();
                obj.Unk0 = ReadUInt32(buffer);
                obj.ContentName = ReadMtString(buffer);
                obj.BattleContentStageList = ReadEntityList<CDataBattleContentStage>(buffer);
                obj.BattleContentParameters = ReadEntityList<CDataBattleContentParameters>(buffer);
                obj.AppraisalItemList = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk3 = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
