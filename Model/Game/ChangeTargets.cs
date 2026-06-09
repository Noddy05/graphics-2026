
using Graphics2026.Model.Game.Shopping;

namespace Graphics2026.Model.Game
{
    internal class ChangeTargets : ActiveRoleBehaviour
    {
        private PathfindingAgent agent;
        public readonly List<POI> targets = new();
        private int targetIndex;
        private Random rand;

        public ChangeTargets(PathfindingAgent agent)
        {
            rand = new Random();
            this.agent = agent;
        }

        
        protected override void Update(float deltaTime)
        {
            if (targets.Count == 0)
                return;

            if (agent.HasReachedTarget())
            {
                int prevTargetIndex = targetIndex;
                while(prevTargetIndex == targetIndex)
                    targetIndex = rand.Next(targets.Count);

                agent.SetTarget(targets[targetIndex]);
            }
        }
    }
}
