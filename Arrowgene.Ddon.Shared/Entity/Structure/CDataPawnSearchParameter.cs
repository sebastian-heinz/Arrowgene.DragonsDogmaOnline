using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnSearchParameter
    {
        public CDataPawnSearchParameter()
        {
            CraftSkillList = new List<CDataPawnCraftSkill>();
            OwnerCharacterName = new CDataCharacterName();
            PawnName = string.Empty;
            CharacterParam = new CDataCharacterSearchParameter();
            DragonAbilitiesList = new List<CDataDragonAbility>();
        }

        public byte CraftRankMin { get; set; }
        public byte CraftRankMax { get; set; }
        public List<CDataPawnCraftSkill> CraftSkillList { get; set; }
        public PawnSex Sex {  get; set; }
        public CDataCharacterName OwnerCharacterName { get; set; }
        public string PawnName { get; set; }
        public CDataCharacterSearchParameter CharacterParam { get; set; }
        public bool IsFriend { get; set; }
        public bool IsClan { get; set; }
        public List<CDataDragonAbility> DragonAbilitiesList {  get; set; }

        public class Serializer : EntitySerializer<CDataPawnSearchParameter>
        {
            public override void Write(IBuffer buffer, CDataPawnSearchParameter obj)
            {
                WriteByte(buffer, obj.CraftRankMin);
                WriteByte(buffer, obj.CraftRankMax);
                WriteEntityList(buffer, obj.CraftSkillList);
                WriteByte(buffer, (byte) obj.Sex);
                WriteEntity(buffer, obj.OwnerCharacterName);
                WriteMtString(buffer, obj.PawnName);
                WriteEntity(buffer, obj.CharacterParam);
                WriteBool(buffer, obj.IsFriend);
                WriteBool(buffer, obj.IsClan);
                WriteEntityList(buffer, obj.DragonAbilitiesList);
            }

            public override CDataPawnSearchParameter Read(IBuffer buffer)
            {
                CDataPawnSearchParameter obj = new CDataPawnSearchParameter();
                obj.CraftRankMin = ReadByte(buffer);
                obj.CraftRankMax = ReadByte(buffer);
                obj.CraftSkillList = ReadEntityList<CDataPawnCraftSkill>(buffer);
                obj.Sex = (PawnSex) ReadByte(buffer);
                obj.OwnerCharacterName = ReadEntity<CDataCharacterName>(buffer);
                obj.PawnName = ReadMtString(buffer);
                obj.CharacterParam = ReadEntity<CDataCharacterSearchParameter>(buffer);
                obj.IsFriend = ReadBool(buffer);
                obj.IsClan = ReadBool(buffer);
                obj.DragonAbilitiesList = ReadEntityList<CDataDragonAbility>(buffer);
                return obj;
            }
        }
    }
}
