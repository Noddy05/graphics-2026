using Graphics2026.Controller;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.Model.SceneManagement;
using Graphics2026.View;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Model.Game.BuildTools.Deprecated
{
    internal enum SnapType
    {
        Wall,
        Floor
    }

    internal class BuildOnFloor : ActiveRoleBehaviour
    {
        private Floor? selectedGrid = null;
        Vector2i firstTile = Vector2i.Zero, lastTile = Vector2i.Zero;

        protected override void Update(float deltaTime)
        {
            if (selectedGrid != null && Camera.current != null &&
                MouseGridSelection.SelectPoint(Camera.current.MouseRayInWorld(), out Vector3 hit, out _, selectedGrid))
                WireRenderer.DrawLine(selectedGrid.transform.localPosition, hit);

            if (!Input.GetMouseButton(MouseButton.Left))
            {
                PlaceTiles();
                selectedGrid = null;
                return;
            }

            if (Input.GetMouseButtonDown(MouseButton.Left))
                FindFloor();

            if (selectedGrid == null)
                return;

            if (MouseGridSelection.SelectSquare(out Vector2i first, out Vector2i last, out _, selectedGrid))
            {
                firstTile = first;
                lastTile = last;
            }

            MouseGridSelection.DrawSquares(firstTile, lastTile, selectedGrid);
        }

        private void PlaceTiles()
        {
            if (selectedGrid == null)
                return;

            Vector2i[] selectedSquares = MouseGridSelection.SquaresInsideBounds(firstTile, lastTile);

            int successes = 0;
            foreach (Vector2i square in selectedSquares)
            {
                if (selectedGrid.PlaceFloorTile(square))
                    successes++;
            }

            Console.WriteLine($"Placed {successes} tiles");
        }

        private void FindFloor()
        {
            float rayDistance = float.MaxValue;
            foreach (Surface grid in SceneManager.GetSurfaces())
            {
                if (grid.GetType() != typeof(Floor))
                    continue;

                if (!MouseGridSelection.SelectSquare(out Vector2i _, out Vector2i _, out float rayLength, grid))
                    continue;

                if (rayLength < rayDistance)
                {
                    rayDistance = rayLength;
                    selectedGrid = (Floor)grid;
                }
            }
        }
    }
}
