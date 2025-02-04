namespace Arrowgene.Ddon.Shared.Model
{
    public enum ShopItemUnlockCondition : uint
    {
        /// <summary>
        /// [a blank string]
        /// </summary>
        None = 1,

        /// <summary>
        /// {Param2 -> ???} cleared with rank {Param1} or better. (Final result {Progress} rank)
        /// Not sure how this is parsed, probably needs a questScheduleId for a quest in the right category.
        /// </summary>
        ClearWithRank = 2,

        /// <summary>
        /// Defeat {Param3 -> EnemyName} x{Param1} ({Progress}/{Param1})
        /// </summary>
        DefeatEnemies = 3,

        /// <summary>
        /// Acquire War Mission Accumulated Points, {Param1} pt. ({Progress}/{Param1})
        /// </summary>
        WarMissionPoints = 4,

        /// <summary>
        /// "Currently selected job with Play point content already released."
        /// </summary>
        UnlockPlayPoints = 5,

        /// <summary>
        /// Defeat LV.{Param2} or more {Param3 -> EnemyName} x{Param1} ({Progress}/{Param1})
        /// </summary>
        DefeatEnemiesLevel = 6,
    }
}

//0,SHOP_GOODS_UNLOCK_CONDITION_1,,, ui\00_message\common\shop_goods_unlock_condition.gmd, \ui\uGUIPopDetail01.arc, uGUIPopDetail01.arc,0,
//1, SHOP_GOODS_UNLOCK_CONDITION_2,"%sの
//ランキング%d位以内に入賞","%s
//cleared with rank %d or better.",ui\00_message\common\shop_goods_unlock_condition.gmd,\ui\uGUIPopDetail01.arc,uGUIPopDetail01.arc,1,
//2, SHOP_GOODS_UNLOCK_CONDITION_3,"%sの
//%sを%d体討伐（%d/%d）","Defeat %s
//%s x%d. (%d/%d)",ui\00_message\common\shop_goods_unlock_condition.gmd,\ui\uGUIPopDetail01.arc,uGUIPopDetail01.arc,2,
//3, SHOP_GOODS_UNLOCK_CONDITION_4,"%sにて
//ウォーミッション累積ポイントを
//%s <UNIT PT>獲得（%s/%s）","Acquire
//%s
//War Mission Accumulated Points,
//%s <UNIT PT>. (%s/%s)",ui\00_message\common\shop_goods_unlock_condition.gmd,\ui\uGUIPopDetail01.arc,uGUIPopDetail01.arc,3,
//4, SHOP_GOODS_UNLOCK_CONDITION_5,"現在選択中のジョブで
//プレイポイントのコンテンツを解放済み","Currently selected job with Play point content
//already released",ui\00_message\common\shop_goods_unlock_condition.gmd,\ui\uGUIPopDetail01.arc,uGUIPopDetail01.arc,4,
//5, SHOP_GOODS_UNLOCK_CONDITION_6,"LV.%d以上の
//%sを%d体討伐（%d/%d）","Defeat LV.%d or more
//%s x%d. (%d/%d)",ui\00_message\common\shop_goods_unlock_condition.gmd,\ui\uGUIPopDetail01.arc,uGUIPopDetail01.arc,5,
