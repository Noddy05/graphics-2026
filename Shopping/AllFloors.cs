
using Graphics2026.Model.Actors;
using Graphics2026.Model.Mesh;
using Graphics2026.View.Shading.Shaders;
using Graphics2026.View.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Shopping
{
    internal static class AllFloors
    {
        private static List<Prefab> floors = new();

        static AllFloors()
        {
            Actor actor = new Actor("Tile actor");
            actor.mesh = MeshGenerator.Cube();
            actor.mesh.BakeTransformation(Matrix4.CreateTranslation(Vector3.UnitY)
                * Matrix4.CreateScale(0.5f, 0.02f, 0.5f));
            actor.shader = new TexturedProcedural(new Texture(Program.ASSETS + "tile_texture.jpg", TextureTarget.Texture2D)
                .AddParameter(new TexParameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear))
                .AddParameter(new TexParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear))
                .AddParameter(new TexParameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat))
                .AddParameter(new TexParameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat)),
                10f
            );


            Prefab tilePrefab = new Prefab("Tile", actor, 15);
            floors.Add(tilePrefab);
        }

        public static Prefab GetFloor(int index) => floors[index - 1];
    }
}
