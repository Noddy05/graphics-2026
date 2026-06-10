using OpenTK.Mathematics;

namespace Graphics2026.Model.Game.BuildTools.Interior
{
    internal abstract class NavigationObstacle
    {
        protected static List<NavigationObstacle> obstacles = new();

        public NavigationObstacle() {
            obstacles.Add(this);
        }

        public static List<NavigationObstacle> GetObstacles() => obstacles;
        public static bool IsLocatedBlocked(Vector2 position, float minDistanceToObstacles)
            => SDF(position) < minDistanceToObstacles;
        public static float SDF(Vector2 position)
        {
            float minDistance = float.PositiveInfinity;

            // Speed up by excluding far away obstacles:
            foreach (NavigationObstacle obstacle in obstacles)
            {
                minDistance = MathF.Min(minDistance, obstacle.DistanceToObject(position));
            }

            return minDistance;
        }
        public abstract float DistanceToObject(Vector2 position);
    }
}
