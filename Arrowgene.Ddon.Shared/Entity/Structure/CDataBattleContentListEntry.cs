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
            BattleContentUnk1 = new List<CDataBattleContentUnk1>();
            Unk2 = new List<CDataCommonU32>();
            Unk3 = new List<CDataCommonU32>();
        }

        public uint Unk0 { get; set; }
        public string ContentName { get; set; }
        public List<CDataBattleContentStage> BattleContentStageList { get; set; }
        public List<CDataBattleContentUnk1> BattleContentUnk1 {  get; set; }
        public List<CDataCommonU32> Unk2 { get; set; }
        public List<CDataCommonU32> Unk3 { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentListEntry>
        {
            public override void Write(IBuffer buffer, CDataBattleContentListEntry obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.ContentName);
                WriteEntityList(buffer, obj.BattleContentStageList);
                WriteEntityList(buffer, obj.BattleContentUnk1);
                WriteEntityList(buffer, obj.Unk2);
                WriteEntityList(buffer, obj.Unk3);
            }

            public override CDataBattleContentListEntry Read(IBuffer buffer)
            {
                CDataBattleContentListEntry obj = new CDataBattleContentListEntry();
                obj.Unk0 = ReadUInt32(buffer);
                obj.ContentName = ReadMtString(buffer);
                obj.BattleContentStageList = ReadEntityList<CDataBattleContentStage>(buffer);
                obj.BattleContentUnk1 = ReadEntityList<CDataBattleContentUnk1>(buffer);
                obj.Unk2 = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk3 = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
