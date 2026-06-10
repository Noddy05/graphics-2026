using Graphics2026.Controller;
using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Mesh;
using Graphics2026.Model.SceneManagement;
using Graphics2026.Model.VertexData;
using Graphics2026.View;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Model.Game.BuildTools.Walls
{
    internal class WallBuilder : BuildTool
    {
        private Vector3? previousPoint;

        public WallBuilder() : base([ SurfaceType.Floor ])
        {

        }

        protected override void EndOfUpdates()
        {
            previousPoint = null;
        }
        protected override void Update(float deltaTime)
        {
            WireRenderer.SetColor(Color4.Red);
            WireRenderer.DrawInFront(true);
            if (previousPoint != null)
                WireRenderer.DrawSphere((Vector3)previousPoint, 0.1f);
            WireRenderer.DrawInFront(false);

            if (Input.GetKeyDown(Keys.Escape))
                previousPoint = null;

            Surface? surface = SurfaceSelect.RaycastSelectSurface(out Vector3 point, out float length);
            if (surface == null)
                return;

            if (!surface.IsType(SurfaceType.Floor))
                return;

            Floor floor = (Floor)surface;
            Vector3 snappedPoint = floor.SnapToGrid(point);

            WireRenderer.DrawInFront(true);
            WireRenderer.DrawSphere(snappedPoint, 0.1f);
            WireRenderer.DrawInFront(false);


            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                if(previousPoint != null)
                {
                    CreateWall((Vector3)previousPoint, snappedPoint);
                }
                previousPoint = snappedPoint;
            }
        }

        public static WallPrefab CreateWall(Vector3 A, Vector3 B)
        {
            Mesh<Vertex> cylinderMesh = MeshGenerator.Cylinder(16);

            float wallThickness = 0.1f;
            float wallLength = Helper.Flatten(A - B).Length / 2f;

            WallPrefab wall = new WallPrefab(A.Xz, B.Xz);
            wall.thickness = wallThickness;
            wall.UpdateNavObstacle();

            DefaultLit shader = new DefaultLit();
            shader.color = Color4.Pink;

            Wall cubePart = new Wall("Cube part of wall");
            cubePart.transform.SetParent(wall.transform);
            cubePart.mesh = MeshGenerator.Cube();
            cubePart.shader = shader;
            cubePart.transform.localSize = new Vector3(wallLength, 1, wallThickness);
            cubePart.transform.localPosition.Y = 1;

            Actor cylinderLeft = new Actor();
            cylinderLeft.transform.SetParent(wall.transform);
            cylinderLeft.shader = shader;
            cylinderLeft.mesh = cylinderMesh;
            cylinderLeft.transform.localPosition.X = -wallLength;
            cylinderLeft.transform.localSize = new Vector3(wallThickness, 1, wallThickness);
            cylinderLeft.transform.localPosition.Y = 1;

            Actor cylinderRight = new Actor();
            cylinderRight.transform.SetParent(wall.transform);
            cylinderRight.shader = shader;
            cylinderRight.mesh = cylinderMesh;
            cylinderRight.transform.localPosition.X = wallLength;
            cylinderRight.transform.localSize = new Vector3(wallThickness, 1, wallThickness);
            cylinderRight.transform.localPosition.Y = 1;

            Surface wallSurface = new Surface(new Vector2(2 * wallLength, 2));
            wallSurface.transform.SetParent(wall.transform);
            wallSurface.transform.localPosition.Y = 1;
            wallSurface.transform.localRotation.X = 90f;
            wallSurface.SetType(SurfaceType.Wall);
            SceneManager.CurrentScene().surfaces.Add(wallSurface);

            wall.SetWallToCutout(cubePart);
            wall.transform.localPosition = (A + B) / 2f;
            wall.transform.localRotation.Y = MathF.Atan2(B.Z - A.Z, A.X - B.X) * 180f / MathF.PI;
            wall.transform.localSize.Y = 2;

            return wall;
        }
    }
}
