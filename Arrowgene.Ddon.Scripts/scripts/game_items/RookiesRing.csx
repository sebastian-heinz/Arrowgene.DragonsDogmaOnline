/**
 * @brief Settings for this object can be found in
 *   <assets>/scripts/settings/game_items/RookiesRing.csx
 */
public class GameItem : IGameItem
{
    private class RingMode
    {
        public const uint Constant = 0;
        public const uint Dynamic = 1;
    }

    public override ItemId ItemId => ItemId.RookiesRingOfBlessing;

    public GameItem()
    {
    }

    public override void OnUse(GameClient client)
    {
        // The rookies ring has no OnUse behavior
    }

    private double CalculateConstantBonus(CharacterCommon characterCommon)
    {
        if (characterCommon.ActiveCharacterJobData.Lv > LibDdon.GetSetting<uint>("RookiesRing", "RookiesRingMaxLevel"))
        {
            return 0;
        }

        return LibDdon.GetSetting<double>("RookiesRing", "RookiesRingBonus");
    }

    private double CalculateDynamicBonus(CharacterCommon characterCommon)
    {
        var dynamicBands = LibDdon.GetSetting<List<(uint MinLv, uint MaxLv, double ExpMultiplier)>>("RookiesRing", "DynamicExpBands");

        var characterLv = characterCommon.ActiveCharacterJobData.Lv;

        foreach (var band in dynamicBands)
        {
            if (characterLv >= band.MinLv && characterLv <= band.MaxLv)
            {
                return band.ExpMultiplier;
            }
        }
        return 0;
    }

    public override double GetBonusMultiplier(CharacterCommon characterCommon)
    {
        double bonus = 0;

        var mode = LibDdon.GetSetting<uint>("RookiesRing", "RookiesRingMode");
        switch (mode)
        {
            case RingMode.Dynamic:
                bonus = CalculateDynamicBonus(characterCommon);
                break;
            default:
                // Mode is either 0 or an invalid value, so treat it as zero
                bonus = CalculateConstantBonus(characterCommon);
                break;
        }

        return bonus;
    }
}

return new GameItem();
