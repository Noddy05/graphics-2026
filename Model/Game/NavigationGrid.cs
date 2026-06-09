
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.View;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game
{
    internal class NavigationGrid : Grid
    {
        private List<Vector2i> blockedSquares = new List<Vector2i>();

        public NavigationGrid() { }
        public NavigationGrid(Vector2i[] squares) {
            foreach(Vector2i v in squares) { 
                if(0 <= v.X && v.X < GridSize().X &&
                    0 <= v.Y && v.Y < GridSize().Y)
                    blockedSquares.Add(v);
            }
        }

        public void AddBlockage(Vector2i coordinate)
        {
            blockedSquares.Add(coordinate);
        }

        public float[][] CalculateCosts(Vector2i target)
        {
            float[][] costs = new float[GridSize().X][];
            for (int i = 0; i < costs.Length; i++)
            {
                costs[i] = new float[GridSize().Y];
            }

            foreach (Vector2i blockage in blockedSquares)
            {
                costs[blockage.X][blockage.Y] = float.PositiveInfinity;
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
            foreach (Vector2i v in blockedSquares)
            {
                WireRenderer.DrawCircle(GridSpaceToWorldSpace(v), 0.5f);
            }
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
