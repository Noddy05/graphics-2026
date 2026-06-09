using Graphics2026.Model.Actors;
using Graphics2026.View;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game.Shopping
{
    internal class POI : ActiveRoleBehaviour
    {
        public Vector2i coordinate;

        public NavigationGrid navigationGrid;
        private float[][] costs;
        private Random rand;

        public POI(Vector2i coordinate, NavigationGrid navigationGrid)
        {
            rand = new Random();
            this.coordinate = coordinate;

            this.navigationGrid = navigationGrid;
            costs = navigationGrid.CalculateCosts(coordinate);
        }

        public float[][] GetCosts() => costs;
        public void RecalculateCosts() => costs = navigationGrid.CalculateCosts(coordinate);

        public Vector2i GetRecommendedDirection(Vector2i currentPosition)
        {
            Vector2i direction = Vector2i.Zero;
            if (currentPosition == coordinate)
                return direction;

            float bestCost = float.PositiveInfinity;
            foreach (Vector2i dir in navigationGrid.WalkableDirections(currentPosition, costs))
            {
                float cost = costs[currentPosition.X + dir.X][currentPosition.Y + dir.Y];
                if (cost < bestCost)
                {
                    bestCost = cost;
                    direction = dir;
                }
            }

            return direction;
        }

        public Vector2i GetRecommendedWithRandomnessDirection(Vector2i currentPosition)
        {
            Vector2i bestDirection = GetRecommendedDirection(currentPosition);
            float bestCost = costs[currentPosition.X + bestDirection.X][currentPosition.Y + bestDirection.Y];

            if (bestCost % 1 == 0)
                return bestDirection;

            List<Vector2i> directions = new([bestDirection]);
            foreach (Vector2i dir in navigationGrid.WalkableStraightDirections(currentPosition, costs))
            {
                float cost = costs[currentPosition.X + dir.X][currentPosition.Y + dir.Y];
                if (cost == bestCost + 0.5f)
                {
                    directions.Add(dir);
                }
            }

            return directions[rand.Next(directions.Count)];
        }

        protected override void Update(float deltaTime)
        {
            WireRenderer.SetColor(Color4.Lime);
            WireRenderer.DrawCircle(navigationGrid.GridSpaceToWorldSpace(coordinate), 0.5f);
        }
    }
}
