using Graphics2026.Controller;
using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Mesh;
using Graphics2026.Model.MeshGeneration;
using Graphics2026.Model.VertexData;
using Graphics2026.View;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace Graphics2026.Model.Game.BuildTools.Walls
{
    internal class WallMod : BuildTool
    {
        private Mesh<Vertex> windowFrameMesh;
        private Actor preVisualization;
        private SimpleActor visualizeCutout;
        private Vector2 requiredSpace = Vector2.One * 1.3f;

        public WallMod() : base([ SurfaceType.Wall ])
        {
            windowFrameMesh = Mesh<Vertex>.Combine(MeshImport.OBJ(Program.LOCAL + "Assets/window_frame.obj"));

            visualizeCutout = new SimpleActor();
            visualizeCutout.mesh = MeshGenerator.Cutout();
            visualizeCutout.SetRenderStatus(false);

            preVisualization = new Actor();
            preVisualization.mesh = windowFrameMesh;
            preVisualization.SetRenderStatus(false);
        }

        private WallPrefab? hoveredWall;
        protected override void EndOfUpdates()
        {
            RemoveTemporaryCutouts();
        }
        protected override void Update(float deltaTime)
        {
            Surface? surface = SurfaceSelect.RaycastSelectSurface(out Vector3 point, out float length);
            if (surface == null)
            {
                RemoveTemporaryCutouts();
                return;
            }

            if (!surface.IsType(SurfaceType.Wall))
            {
                RemoveTemporaryCutouts();
                return;
            }

            WallPrefab wallPrefab = (WallPrefab)surface.transform.GetParent()!.GetRenderable();
            if(wallPrefab != hoveredWall)
                RemoveTemporaryCutouts();

            hoveredWall = wallPrefab;

            visualizeCutout.transform.localPosition = point;
            visualizeCutout.transform.localRotation = wallPrefab.transform.localRotation;
            visualizeCutout.transform.localSize.Z = 2 * wallPrefab.thickness + 0.01f;
            wallPrefab.SetTemporaryCutout([ visualizeCutout ]);

            preVisualization.transform.localPosition = point;
            preVisualization.transform.localRotation = wallPrefab.transform.localRotation;

            if(!CanPlace(point, wallPrefab, surface))
            {
                preVisualization.RenderFamilyWithShader(Builder.HIGHLIGHT_ERROR_SHADER);
                return;
            }
            preVisualization.RenderFamilyWithShader(Builder.HIGHLIGHT_SHADER);

            if (!Input.GetMouseButtonDown(MouseButton.Left))
                return;

            //Create cutout:
            SimpleActor cutout = new SimpleActor();
            cutout.mesh = MeshGenerator.Cutout();
            cutout.transform.localPosition = point;
            cutout.transform.localRotation = wallPrefab.transform.localRotation;
            cutout.transform.localSize.Z = 2 * wallPrefab.thickness + 0.01f;

            Actor frame = new Actor();
            frame.mesh = windowFrameMesh;
            frame.shader = new DefaultLit();
            frame.transform.localPosition = point;
            frame.transform.localRotation = wallPrefab.transform.localRotation;

            wallPrefab.AddCutout(cutout);
        }

        private bool CanPlace(Vector3 point, WallPrefab prefab, Surface surface)
        {
            Vector3 corner = (prefab.transform.Up() * requiredSpace.Y
                + prefab.transform.Right() * requiredSpace.X) / 2f;

            if (!surface.IsPointContained(surface.PointToGridSpace(point + corner))
                || !surface.IsPointContained(surface.PointToGridSpace(point - corner)))
            {
                return false;
            }

            return true;
        }

        private void RemoveTemporaryCutouts()
        {
            if (hoveredWall == null)
                return;

            hoveredWall.SetTemporaryCutout([]);
            hoveredWall = null;
        }

        private void DrawPointInGridSpace(Vector3 point, Surface surface)
        {
            Vector2 gridPoint = surface.PointToGridSpace(point);
            WireRenderer.DrawLine(new Vector3(gridPoint.X, 0, gridPoint.Y), Vector3.Zero);

            Vector2 gridCorner1 = new Vector2(-surface.GetSize().X / 2, -surface.GetSize().Y / 2);
            Vector2 gridCorner2 = new Vector2(-surface.GetSize().X / 2, surface.GetSize().Y / 2);
            Vector2 gridCorner3 = new Vector2(surface.GetSize().X / 2, surface.GetSize().Y / 2);
            Vector2 gridCorner4 = new Vector2(surface.GetSize().X / 2, -surface.GetSize().Y / 2);
            WireRenderer.DrawLine(new Vector3(gridCorner1.X, 0, gridCorner1.Y),
                new Vector3(gridCorner2.X, 0, gridCorner2.Y));
            WireRenderer.DrawLine(new Vector3(gridCorner2.X, 0, gridCorner2.Y),
                new Vector3(gridCorner3.X, 0, gridCorner3.Y));
            WireRenderer.DrawLine(new Vector3(gridCorner3.X, 0, gridCorner3.Y),
                new Vector3(gridCorner4.X, 0, gridCorner4.Y));
            WireRenderer.DrawLine(new Vector3(gridCorner1.X, 0, gridCorner1.Y),
                new Vector3(gridCorner4.X, 0, gridCorner4.Y));
        }
    }
}
