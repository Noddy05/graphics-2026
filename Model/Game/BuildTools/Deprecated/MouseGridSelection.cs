using Graphics2026.Controller;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.Model.Rays;
using Graphics2026.Model.SceneManagement;
using Graphics2026.View;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Model.Game.BuildTools.Deprecated
{
    internal static class MouseGridSelection
    {
        private static bool firstTileIsSelected = false;
        private static Vector2i firstTile;
        private static Vector2i lastTile;

        public static bool SelectSquare(out Vector2i _firstTile, out Vector2i _lastTile, out float rayLength, Surface grid)
        {
            _firstTile = firstTile;
            _lastTile = lastTile;

            bool clickedTile = SelectedTile(out Vector2i tile, out rayLength, grid);
            if (clickedTile)
            {
                _lastTile = tile;
                lastTile = tile;
            }

            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                firstTileIsSelected = clickedTile;
                _firstTile = tile;
                firstTile = tile;
            }

            return firstTileIsSelected;
        }

        public static Vector2i[] SquaresInsideBounds(Vector2i firstSquare, Vector2i lastSquare)
        {
            int x0 = Math.Min(firstSquare.X, lastSquare.X);
            int x1 = Math.Max(firstSquare.X, lastSquare.X);
            int y0 = Math.Min(firstSquare.Y, lastSquare.Y);
            int y1 = Math.Max(firstSquare.Y, lastSquare.Y);

            Vector2i[] output = new Vector2i[(x1 - x0 + 1) * (y1 - y0 + 1)];

            int i = 0;
            for (int x = x0; x <= x1; x++)
            {
                for (int y = y0; y <= y1; y++)
                {
                    output[i++] = new Vector2i(x, y);
                }
            }

            return output;
        }

        public static void DrawSquares(Vector2i firstSquare, Vector2i lastSquare, Surface grid)
        {
            WireRenderer.SetColor(Color4.Red);

            Vector2i[] squares = SquaresInsideBounds(firstSquare, lastSquare);
            for (int i = 0; i < squares.Length; i++)
            {
                Vector3 tileCorner = GridSpaceToWorldSpace(squares[i], grid);

                WireRenderer.DrawLine(tileCorner,
                    tileCorner + grid.transform.Right() - grid.transform.Forward());
                WireRenderer.DrawLine(tileCorner + grid.transform.Right(),
                    tileCorner - grid.transform.Forward());
            }
        }

        public static bool SelectedTile(out Vector2i tile, out float rayLength, Surface grid)
        {
            tile = Vector2i.Zero;
            rayLength = float.PositiveInfinity;

            if (Camera.current == null)
                return false;

            Ray cameraRay = Camera.current.MouseRayInWorld();

            return TileIntersect(cameraRay, out tile, out rayLength, grid);
        }

        private static bool TileIntersect(Ray ray, out Vector2i tile, out float rayLength, Surface grid)
        {
            tile = Vector2i.Zero;

            if (!SelectPoint(ray, out Vector3 hit, out rayLength, grid))
                return false;

            tile = WorldSpaceToGridSpace(hit, grid);

            return tile.X - grid.transform.localPosition.X >= -grid.GetSize().X / 2 && tile.X
                - grid.transform.localPosition.X < grid.GetSize().X / 2f
                && tile.Y - grid.transform.localPosition.Z >= -grid.GetSize().Y / 2 && tile.Y
                - grid.transform.localPosition.Z < grid.GetSize().Y / 2f;
        }

        public static Ray GetMouseRay()
        {
            if (Camera.current == null)
                throw new Exception("No camera!");

            Ray ray = Camera.current.MouseRay();
            Vector3 worldSpaceDirection = (Camera.current.transform.LocalRotation() * new Vector4(ray.direction, 1)).Xyz;
            // = (Camera.current.transform.LocalRotation() * new Vector4(ray.direction, 1) 
            // * Camera.current.transform.LocalPosition()).Xyz;

            return new Ray(Camera.current.transform.localPosition, worldSpaceDirection);
        }

        public static bool SelectPoint(Ray ray, out Vector3 point, out float rayLength, Surface grid)
        {
            point = Vector3.Zero;
            rayLength = float.PositiveInfinity;

            Vector3 pointInPlane = grid.transform.localPosition;
            Vector3 planeNormal = grid.transform.Up();
            float denominator = Vector3.Dot(ray.direction, planeNormal);
            if (denominator == 0)
                return false;

            rayLength = Vector3.Dot(pointInPlane - ray.origin, planeNormal) / denominator;
            point = ray.origin + rayLength * ray.direction;

            //Check if outside 
            if (2 * MathF.Abs(Vector3.Dot(point, grid.transform.Right())) > grid.GetSize().X ||
                2 * MathF.Abs(Vector3.Dot(point, grid.transform.Forward())) > grid.GetSize().Y)
                return false;

            if (rayLength < 0)
            {
                rayLength = float.PositiveInfinity;
                return false;
            }

            return true;
        }

        public static Vector3 GridSpaceToWorldSpace(Vector2i tile, Surface grid)
            => Helper.ApplyMatrix(Vector3.Floor(new Vector3(tile.X, 0, tile.Y)),
                grid.transform.WorldTransform()) - RoundOffset3D(grid);
        public static Vector2i WorldSpaceToGridSpace(Vector3 position, Surface grid)
            => (Vector2i)Vector2.Floor(Helper.ApplyMatrix(position + RoundOffset3D(grid),
                grid.transform.WorldTransform().Inverted()).Xz);

        public static Vector3 RoundOffset3D(Surface grid) => (grid.transform.Right() *
            (grid.GetSize().X % 2 + 2 * MathF.Ceiling(grid.transform.localPosition.X)) -
            grid.transform.Forward() * (grid.GetSize().Y % 2 + 2 * MathF.Ceiling(grid.transform.localPosition.Z))) / 2f;

        public static Floor? FindFloor(out Vector3 point, out float rayLength)
        {
            Floor? selectedGrid = null;

            point = Vector3.Zero;
            rayLength = float.PositiveInfinity;

            foreach (Surface grid in SceneManager.GetSurfaces())
            {
                if (grid.GetType() != typeof(Floor))
                    continue;

                if (!SelectPoint(GetMouseRay(), out Vector3 p, out float length, grid))
                    continue;

                if (length < rayLength)
                {
                    rayLength = length;
                    selectedGrid = (Floor)grid;
                    point = p;
                }
            }

            return selectedGrid;
        }
    }
}
