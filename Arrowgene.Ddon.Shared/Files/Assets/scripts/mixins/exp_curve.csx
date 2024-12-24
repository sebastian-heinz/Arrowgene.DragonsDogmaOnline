/**
 * @breif This mixin is used to calculate the exp amount for a given
 * enemy based on observations from realworld exp data. It should be
 * possible to call this script directly from the server code or from
 * other scripts.
 */

public class Mixin : IExpCurveMixin
{
    public override uint GetExpValue(InstancedEnemy enemy)
    {
        // TODO: Implement algorithm to calculate exp amount
        return enemy.GetDroppedExperience();
    }
}

return new Mixin();
