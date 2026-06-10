using Graphics2026.Model.Actors;
using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Mathematics;
using Graphics2026.Model.VertexData;
using Graphics2026.Model.Game;
using Graphics2026.Model.MeshGeneration;
using Graphics2026.Model.Mesh;
using Graphics2026.Model.Game.BuildTools;

namespace Graphics2026.Model.SceneManagement
{
    internal class TestSceneFactory
    {
        public static void InitializeScene()
        {
            //SceneManager.CurrentScene().actors.Add(actor);

            Actor cameraActor = new Actor();
            cameraActor.AddAttachment<MovementControls>();
            Camera camera = cameraActor.AddAttachment<PerspectiveCamera>(85f, 0.1f, 1000f);
            Camera.current = camera;

            Actor cylinder = new Actor();
            Mesh<Vertex> cylinderMesh = MeshGenerator.Cylinder(64);
            //cylinderMesh = Mesh<Vertex>.Combine(cylinderMesh, MeshGenerator.RegularFilledPolygon(15));

            cylinder.mesh = cylinderMesh;
            cylinder.transform.localPosition = new Vector3(5, 1, 5);
            cylinder.shader = new DefaultLit();

            SimpleActor cube = new SimpleActor("cube");
            cube.mesh = MeshGenerator.SimpleQuad();
            cube.transform.localPosition = new Vector3(0, 0, -10f);
            //cube.shader = new DefaultLit();

            /*
            Surface surface = new Surface(new Vector2(2, 1));
            surface.transform.localRotation.X = 90f;
            surface.transform.localPosition = new Vector3(-1, 5, 0);
            surface.transform.localSize = new Vector3(3, 1, 1);
            surface.SetType(SurfaceType.Wall);
            SceneManager.CurrentScene().surfaces.Add(surface);
            */

            Floor floor = new Floor();
            floor.SetSize(new Vector2i(20, 30));
            SceneManager.CurrentScene().surfaces.Add(floor);

            /*
            Floor floorGrid = new Floor();
            floorGrid.SetGridSize(new Vector2i(10, 10));
            floorGrid.transform.rotation.Z = 60f;
            floorGrid.transform.rotation.X = 20f;
            floorGrid.transform.rotation.Y = 50f;
            floorGrid.transform.position = new Vector3(0, -1, -3.2f);
            //new MouseGridSelection(floorGrid);
            SceneManager.CurrentScene().grids.Add(floorGrid);


            floorGrid = new Floor();
            floorGrid.SetGridSize(new Vector2i(5, 5));
            floorGrid.transform.position = new Vector3(0, -5f, 0);
            //new MouseGridSelection(floorGrid);
            SceneManager.CurrentScene().grids.Add(floorGrid);*/

            Actor oneWayDoor = new Actor();
            oneWayDoor.mesh = MeshImport.OBJ(Program.LOCAL + "Assets/one_way_door.obj")[0];
            oneWayDoor.transform.localPosition = new Vector3(14.5f, 0, -0.5f);
            oneWayDoor.transform.localSize *= 0.5f;
            oneWayDoor.shader = new DefaultLit();

            NavigationGrid navGrid = new NavigationGrid();
            navGrid.SetSize(new Vector2i(20, 30));
            SceneManager.CurrentScene().renderables.Add(navGrid);

            /*
            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                navGrid.AddBlockage(new Vector2i(rand.Next(navGrid.GetGridSize().X - 1), 
                    rand.Next(navGrid.GetGridSize().Y - 1)));
            }

            List<POI> pois = new List<POI>();
            for (int i = 0; i < 10; i++)
            {
                pois.Add(new POI(new Vector2i(rand.Next(navGrid.GetGridSize().X - 1),
                    rand.Next(navGrid.GetGridSize().Y - 1)), navGrid));
            }

            for (int i = 0; i < 1_000; i++)
            {
                PathfindingAgent agent = new PathfindingAgent(navGrid);
                agent.coordinate = new Vector2i(rand.Next(navGrid.GetGridSize().X - 1), rand.Next(navGrid.GetGridSize().Y - 1));
                agent.SetSpeed(rand.NextSingle() * 2.5f + 2.5f);
                ChangeTargets agentTargets = new ChangeTargets(agent);

                agentTargets.targets.AddRange(pois);
            }
            */
        }
    }
}
