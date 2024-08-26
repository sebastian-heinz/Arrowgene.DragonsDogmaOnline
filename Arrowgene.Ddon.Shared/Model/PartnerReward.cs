using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class PartnerReward
    {
        public static CDataPartnerPawnReward CreateEmoteReward(EmoteId emoteId)
        {
            return new CDataPartnerPawnReward()
            {
                Type = PawnLikabilityRewardType.Emotion,
                Value = new CDataPartnerPawnRewardParam()
                {
                    ParamTypeId = 0,
                    UID = (uint)emoteId
                }
            };
        }

        public static CDataPartnerPawnReward CreateAbilityReward(AbilityId abilityId)
        {
            return new CDataPartnerPawnReward()
            {
                Type = PawnLikabilityRewardType.Ability,
                Value = new CDataPartnerPawnRewardParam()
                {
                    ParamTypeId = 0,
                    UID = (uint)abilityId
                }
            };
        }

        public static CDataPartnerPawnReward CreateCommunicationReward(uint communicationId)
        {
            return new CDataPartnerPawnReward()
            {
                Type = PawnLikabilityRewardType.Talk,
                Value = new CDataPartnerPawnRewardParam()
                {
                    ParamTypeId = 0,
                    UID = communicationId
                }
            };
        }

        public static CDataPartnerPawnReward CreateHairstyleReward(HairStyleId hairstyleId, uint unk0)
        {
            return new CDataPartnerPawnReward()
            {
                Type = PawnLikabilityRewardType.Hair,
                Value = new CDataPartnerPawnRewardParam()
                {
                    ParamTypeId = unk0,
                    UID = (uint) hairstyleId
                }
            };
        }
    }
}
