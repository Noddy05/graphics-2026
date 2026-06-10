using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.Model.Mesh;
using Graphics2026.View.Shading.Shaders;
using Graphics2026.View.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Shopping
{
    internal class CreateStore
    {
        public CreateStore()
        {
            Actor cameraActor = new Actor();
            cameraActor.AddAttachment<MovementControls>();
            cameraActor.transform.localPosition.Y = 3f;
            Camera camera = cameraActor.AddAttachment<PerspectiveCamera>(85f, 0.1f, 1000f);
            Camera.current = camera;

            TexturedProcedural groundShader = new TexturedProcedural(
                new Texture(Program.LOCAL + "Assets/dirt_texture.jpg", TextureTarget.Texture2D)
                .AddParameter(new TexParameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear))
                .AddParameter(new TexParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear))
                .AddParameter(new TexParameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat))
                .AddParameter(new TexParameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat))
            );
            groundShader.textureScale = 1f;

            Actor ground = new Actor();
            ground.mesh = MeshGenerator.Quad();
            ground.shader = groundShader;
            ground.transform.localSize *= 10;
            ground.transform.localPosition.Y = -0.001f;

            Grid grid = new Grid(new Vector2i(10, 10));
            grid.SetType(SurfaceType.Floor);
            Program.GetWindow().GetRenderer()!.drawSurfaces[SurfaceType.Floor] = true;

            Actor customer = new Actor();
            customer.mesh = MeshGenerator.Cylinder(32);
            customer.shader = new DefaultLit();
            customer.mesh.BakeTransformation(Matrix4.CreateScale(new Vector3(0.5f, 1, 0.5f)) 
                * Matrix4.CreateTranslation(new Vector3(0, 1, 0)));
        }
    }
}
