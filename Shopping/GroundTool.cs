using Graphics2026.Controller;
using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Game.BuildTools;
using Graphics2026.Model.Mesh;
using Graphics2026.View;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Shopping
{
    internal class GroundTool : BuildTool
    {
        private Actor currentPrefab;
        private Dictionary<Grid, int[][]> floors = new();

        public GroundTool() : base([ SurfaceType.Floor ])
        {
            isActive = true;

            currentPrefab = new Actor();
            currentPrefab.mesh = MeshGenerator.Cube();
            currentPrefab.mesh.BakeTransformation(Matrix4.CreateTranslation(Vector3.UnitY) 
                * Matrix4.CreateScale(0.5f, 0.02f, 0.5f));
            currentPrefab.shader = new DefaultLit();
            currentPrefab.SetRenderStatus(false);
        }

        protected override void Update(float deltaTime)
        {
            Surface? surface = SurfaceSelect.RaycastSelectSurface(out Vector3 point, out float rayLength, [SurfaceType.Floor]);
            Grid? grid = surface as Grid;

            if (grid == null)
                return;

            Vector3 position = grid.SnapToGrid(point);

            WireRenderer.DrawLine(Vector3.Zero, point);
            currentPrefab.transform.localPosition = position;
            currentPrefab.RenderFamilyWithShader(Builder.HIGHLIGHT_SHADER);

            if (!Input.GetMouseButtonDown(MouseButton.Left))
                return;

            if (!floors.ContainsKey(grid))
            {
                int[][] floorGrid = new int[grid.GridSize().X][];
                for (int i = 0; i < floorGrid.Length; i++)
                    floorGrid[i] = new int[grid.GridSize().Y];

                floors.Add(grid, floorGrid);
            }

            Actor placed = currentPrefab.Clone();
            placed.SetRenderStatus(true);
        }
    }
}
