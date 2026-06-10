
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Game.BuildTools.Interior;
using Graphics2026.View;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game
{
    internal class NavigationGrid : Grid
    {
        private static int gridSubdivisions = 3;
        private List<Vector2i> blockedSquares = new List<Vector2i>();

        private Vector2i Size => GridSize() * (1 << gridSubdivisions);

        public NavigationGrid() : base() {
            SetType(SurfaceType.Floor);
        }
        public NavigationGrid(Vector2i[] squares) : this() {
            foreach(Vector2i v in squares) { 
                if(0 <= v.X && v.X < Size.X &&
                    0 <= v.Y && v.Y < Size.Y)
                    blockedSquares.Add(v);
            }
        }

        public void AddBlockage(Vector2i coordinate)
        {
            blockedSquares.Add(coordinate);
        }
        private int Divisions() => 1 << gridSubdivisions;

        public float[][] CalculateCosts(Vector2i target)
        {
            float[][] costs = new float[Size.X][];
            for (int x = 0; x < costs.Length; x++)
            {
                costs[x] = new float[Size.Y];
                for(int y = 0; y < costs[x].Length; y++)
                {
                    Vector2 pos = new Vector2(x, y) / Divisions() - size / 2f;
                    float d = NavigationObstacle.SDF(pos);
                    if(4 * d < 1)
                        costs[x][y] = float.PositiveInfinity;
                }
            }

            Queue<Vector2i> queue = new Queue<Vector2i>();
            queue.Enqueue(target);
            costs[target.X][target.Y] = 1;

            while (queue.TryDequeue(out Vector2i nextCoordinate))
            {
                foreach (Vector2i neighbour in NeighbouringSquares(nextCoordinate, costs))
                    queue.Enqueue(neighbour);
            }

            return costs;
        }

        public override void Render()
        {
            base.Render();
            WireRenderer.SetColor(Color4.Red);
            /*
            int divisions = Divisions();
            float[][] costs = CalculateCosts(new Vector2i(0, 0));
            for (int x = 0; x < GridSize().X * divisions; x++)
            {
                for(int y = 0; y < GridSize().Y * divisions; y++)
                {
                    if (costs[x][y] == float.PositiveInfinity)
                        WireRenderer.DrawCircle(new Vector3(x, 0, y) / divisions 
                            - new Vector3(size.X, 0, size.Y) / 2f, 1f / divisions);
                }
            }
            */
        }

        // 7 0 1
        // 6   2
        // 5 4 3
        private static (Vector2i direction, float cost)[] validDirections = {
            (new Vector2i( 0,  1 ), 1),
            (new Vector2i( 1,  1 ), 1.5f),
            (new Vector2i( 1,  0 ), 1),
            (new Vector2i( 1, -1 ), 1.5f),
            (new Vector2i( 0, -1 ), 1),
            (new Vector2i(-1, -1 ), 1.5f),
            (new Vector2i(-1,  0 ), 1),
            (new Vector2i(-1,  1 ), 1.5f),
        };
        private static (Vector2i direction, float cost)[] straightDirections = {
            (new Vector2i( 0,  1 ), 1),
            (new Vector2i( 1,  0 ), 1),
            (new Vector2i( 0, -1 ), 1),
            (new Vector2i(-1,  0 ), 1),
        };

        public List<Vector2i> WalkableDirections(Vector2i currentCoordinate, float[][] currentCost)
        {
            return WalkableDirections(currentCoordinate, currentCost, validDirections);
        }
        public List<Vector2i> WalkableStraightDirections(Vector2i currentCoordinate, float[][] currentCost)
        {
            return WalkableDirections(currentCoordinate, currentCost, straightDirections);
        }
        public List<Vector2i> WalkableDirections(Vector2i currentCoordinate, float[][] currentCost, 
            (Vector2i direction, float cost)[] directions)
        {
            List<Vector2i> neighbours = new();

            foreach ((Vector2i direction, _) in directions)
            {
                Vector2i coordinate = currentCoordinate + direction;
                if (coordinate.X < 0 || coordinate.X >= size.X ||
                    coordinate.Y < 0 || coordinate.Y >= size.Y)
                    continue;

                if (currentCost[coordinate.X][coordinate.Y] == float.PositiveInfinity)
                    continue;

                neighbours.Add(direction);
            }

            return neighbours;
        }

        private List<Vector2i> NeighbouringSquares(Vector2i currentCoordinate, float[][] currentCost)
        {
            float baseCost = currentCost[currentCoordinate.X][currentCoordinate.Y];

            List<Vector2i> neighbours = new();

            foreach((Vector2i direction, float cost) in validDirections)
            {
                Vector2i coordinate = currentCoordinate + direction;
                if (coordinate.X < 0 || coordinate.X >= size.X ||
                    coordinate.Y < 0 || coordinate.Y >= size.Y)
                    continue;

                if (currentCost[coordinate.X][coordinate.Y] == float.PositiveInfinity)
                    continue;

                float newCost = baseCost + cost;
                if (currentCost[coordinate.X][coordinate.Y] > newCost || currentCost[coordinate.X][coordinate.Y] == 0)
                {
                    currentCost[coordinate.X][coordinate.Y] = newCost;
                    neighbours.Add(coordinate);
                }
            }

            return neighbours;
        }

        public List<Vector2i> GetBlockedSquares() => blockedSquares;
    }
}
