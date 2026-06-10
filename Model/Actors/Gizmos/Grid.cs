using Graphics2026.View;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Actors.Gizmos
{
    internal class Grid : Surface
    {
        private static int gridSubdivisions = 0;
        private const int maxSubdivisions = 3;

        public Grid() { }
        public Grid(string name) : base(name) { }
        public Grid(Vector2i size) : base(size) { }
        public Grid(string name, Vector2i size) : base(name, size) { }

        public Vector2i GridSize() => (Vector2i)size;
        public virtual void SetSize(Vector2i size)
        {
            this.size = size;
        }

        public static void SetSubdivisions(int newDivisions)
        {
            gridSubdivisions = newDivisions;
            gridSubdivisions = int.Clamp(gridSubdivisions, 0, maxSubdivisions);
        }
        public static int GetSubdivisions() => gridSubdivisions;
        public static int GetSubdivisionsPow2() => 1 << gridSubdivisions;

        private void RenderGrid()
        {
            Vector3 center = new Vector3(size.X, 0, size.Y) / 2f;

            WireRenderer.SetColor(Color4.White);
            for (int x = 0; x <= size.X; x++)
            {
                WireRenderer.DrawLine(new Vector3(x, 0, 0) - center,
                    new Vector3(x, 0, size.Y) - center, transform.WorldTransform());
            }
            for (int y = 0; y <= size.Y; y++)
            {
                WireRenderer.DrawLine(new Vector3(0, 0, y) - center,
                    new Vector3(size.X, 0, y) - center, transform.WorldTransform());
            }
        }
        
        public override void Render()
        {
            if (gridSubdivisions == maxSubdivisions)
            {
                Vector2i prevGridSize = GridSize();
                Vector3 prevSize = transform.localSize;

                size = Vector2i.One;
                transform.localSize *= new Vector3(prevGridSize.X, 1, prevGridSize.Y);
                RenderGrid();
                size = prevGridSize;
                transform.localSize = prevSize;
                return;
            }
            int scaling = 1 << gridSubdivisions;
            size *= scaling;
            transform.localSize /= scaling;
            RenderGrid();
            size /= scaling;
            transform.localSize *= scaling;
        }

        public Vector3 SnapToGrid(Vector3 point)
        {
            if (gridSubdivisions == maxSubdivisions)
                return point;

            int scaling = 1 << gridSubdivisions;
            Vector3 offset = Vector3.Zero;

            if (gridSubdivisions == 0)
                offset = RoundOffset();

            return Vector3.Round(point * scaling - offset) / scaling + offset;
        }

        public Vector3 RoundOffset() => (transform.Right() *
            ((size.X + 1) % 2 + 2 * MathF.Ceiling(transform.localPosition.X)) -
            transform.Forward() * ((size.Y + 1) % 2 + 2 * MathF.Ceiling(transform.localPosition.Z))) / 2f;

        public Vector3 GridSpaceToWorldSpace(Vector2i tile)
        {
            Vector2 tilePosition = tile - size / 2;
            return Helper.ApplyMatrix(Vector3.Floor(new Vector3(tilePosition.X, 0, tilePosition.Y)),
                transform.WorldTransform()) + RoundOffset3D();
        }
        public Vector2i WorldSpaceToGridSpace(Vector3 position)
        {
            return (Vector2i)Vector2.Floor(Helper.ApplyMatrix(position - RoundOffset3D(),
                transform.WorldTransform().Inverted()).Xz) + GridSize() / 2;
        }
    }
}
