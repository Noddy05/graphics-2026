using Graphics2026.Model.Actors;
using Graphics2026.Model.VertexData;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game.BuildTools.Deprecated
{
    internal class Tile : Actor
    {
        public Tile()
        {
            shader = Materials.floorShader;
            GenMesh();
        }

        private void GenMesh()
        {
            // 3---2
            // | \ |
            // 0---1
            Vertex[] vertices =
            {
                new Vertex(new Vector3(0.0f, 0.05f, 0.0f), Vector3.UnitY, new Vector2(0, 0)),
                new Vertex(new Vector3(0.0f, 0.05f, 1.0f), Vector3.UnitY, new Vector2(0, 1)),
                new Vertex(new Vector3(1.0f, 0.05f, 1.0f), Vector3.UnitY, new Vector2(1, 1)),
                new Vertex(new Vector3(1.0f, 0.05f, 0.0f), Vector3.UnitY, new Vector2(1, 0)),
            };
            uint[] indices =
            {
                0, 1, 3,
                1, 2, 3
            };

            mesh = new Mesh<Vertex>(vertices, indices, BufferUsageHint.DynamicCopy);
        }
    }
}
