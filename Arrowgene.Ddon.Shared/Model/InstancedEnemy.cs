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
        }

        public InstancedEnemy(InstancedEnemy enemy) : base (enemy)
        {
            IsKilled = false;
            Index = enemy.Index;
        }

        public bool IsKilled { get; set; }
        public byte Index {  get; set; }
    }
}
