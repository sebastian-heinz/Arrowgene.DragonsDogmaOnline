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
        }

        public InstancedEnemy(InstancedEnemy enemy) : base (enemy)
        {
            IsKilled = false;
            Index = enemy.Index;
            IsRequired = enemy.IsRequired;
        }

        public bool IsRequired { get; set; }
        public bool IsKilled { get; set; }
        public byte Index {  get; set; }
    }
}
