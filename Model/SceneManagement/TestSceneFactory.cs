using Graphics2026.Model.Actors;
using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Mathematics;
using Graphics2026.Model.VertexData;
using Graphics2026.Model.MeshGeneration;
using Graphics2026.Model.Mesh;
using Graphics2026.View.Textures;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.SceneManagement
{
    internal class TestSceneFactory
    {
        public static void InitializeScene()
        {
            Actor cameraActor = new Actor();
            cameraActor.AddAttachment<MovementControls>();
            Camera camera = cameraActor.AddAttachment<PerspectiveCamera>(85f, 0.1f, 1000f);
            Camera.current = camera;

            Actor cylinder = new Actor().AddToScene();
            Mesh<Vertex> cylinderMesh = MeshGenerator.Cylinder(64);

            cylinder.mesh = cylinderMesh;
            cylinder.transform.localPosition = new Vector3(5, 1, 5);
            cylinder.shader = new DefaultLit();

            Actor quad = new Actor("quad").AddToScene();
            quad.mesh = MeshGenerator.Quad();
            quad.transform.localPosition = new Vector3(0, 0, -10f);
            quad.shader = new TexturedUnlit();
            ((TexturedUnlit)quad.shader).texture = new Texture(Program.GetWindow().fboTexture, TextureTarget.Texture2D);
                /* = new Texture(Program.LOCAL 
                + "/View/Textures/Bricks059_4K-PNG_Color.png", TextureTarget.Texture2D)*/;

            Actor oneWayDoor = new Actor().AddToScene();
            oneWayDoor.mesh = MeshImport.OBJ(Program.LOCAL + "Assets/one_way_door.obj")[0];
            oneWayDoor.transform.localPosition = new Vector3(14.5f, 0, -0.5f);
            oneWayDoor.transform.localSize *= 0.5f;
            oneWayDoor.shader = new DefaultLit();
        }
    }
}
