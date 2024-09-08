namespace Arrowgene.Ddon.Shared.Model
{
    public class InstancedEnemy : Enemy
    {
        public InstancedEnemy()
        {

        }

        public InstancedEnemy(Enemy enemy) : base(enemy)
        {
            IsKilled = false;
            IsRequired = true;
            RepopWaitSecond = 60;
        }

        public InstancedEnemy(InstancedEnemy enemy) : base (enemy)
        {
            IsKilled = false;
            IsRequired = enemy.IsRequired;
            RepopWaitSecond = enemy.RepopWaitSecond;
        }

        public bool IsRequired { get; set; }
        public bool IsKilled { get; set; }
        public uint RepopWaitSecond {  get; set; }
    }
}
