using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Enemies
{
    public class InstancedEnemy : Enemy
    {
        private InstancedEnemy()
        {

        }

        public InstancedEnemy(Enemy enemy) : base(enemy)
        {
            IsKilled = false;
        }

        public bool IsKilled { get; set; }
    }
}
