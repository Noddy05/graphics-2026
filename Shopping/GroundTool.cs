using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Game.BuildTools;
using Graphics2026.Model.Mesh;
using Graphics2026.View;
using OpenTK.Mathematics;

namespace Graphics2026.Shopping
{
    internal class GroundTool : BuildTool
    {
        private Actor currentPrefab;

        public GroundTool() : base([ SurfaceType.Floor ])
        {
            isActive = true;

            currentPrefab = new Actor();
            currentPrefab.mesh = MeshGenerator.Cube();
            currentPrefab.mesh.BakeTransformation(Matrix4.CreateScale(0.5f) 
                * Matrix4.CreateTranslation(Vector3.UnitY * 0.5f));
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
        }
    }
}
