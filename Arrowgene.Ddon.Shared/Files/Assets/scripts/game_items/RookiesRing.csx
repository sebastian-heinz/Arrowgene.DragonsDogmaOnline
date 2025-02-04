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

    public override void OnUse(DdonGameServer server, GameClient client)
    {
        // The rookies ring has no OnUse behavior
    }

    private double CalculateConstantBonus(DdonGameServer server, CharacterCommon characterCommon)
    {
        if (characterCommon.ActiveCharacterJobData.Lv > server.GameLogicSettings.Get<uint>("RookiesRing", "RookiesRingMaxLevel"))
        {
            return 0;
        }

        return server.GameLogicSettings.Get<double>("RookiesRing", "RookiesRingBonus");
    }

    private double CalculateDynamicBonus(DdonGameServer server, CharacterCommon characterCommon)
    {
        var dynamicBands = server.GameLogicSettings.Get<List<(uint MinLv, uint MaxLv, double ExpMultiplier)>>("RookiesRing", "DynamicExpBands");

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

    public override double GetBonusMultiplier(DdonGameServer server, CharacterCommon characterCommon)
    {
        double bonus = 0;

        var mode = server.GameLogicSettings.Get<uint>("RookiesRing", "RookiesRingMode");
        switch (mode)
        {
        case RingMode.Dynamic:
            bonus = CalculateDynamicBonus(server, characterCommon);
            break;
        default:
            // Mode is either 0 or an invalid value, so treat it as zero
            bonus = CalculateConstantBonus(server, characterCommon);
            break;
        }

        return bonus;
    }
}

return new GameItem();
