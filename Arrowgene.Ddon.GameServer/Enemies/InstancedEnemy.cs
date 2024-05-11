using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Enemies
{
    public class InstancedEnemy : Enemy
    {
#if false
        public InstancedEnemy()
        {
        }
#endif

        public InstancedEnemy(Enemy enemy) : base(enemy)
        {
            IsAlive = true;
        }

        public bool IsAlive { get; set; }
    }
}
