using Graphics2026.Model.BufferObjects;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Graphics2026.Model.VertexData
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Vertex : IVertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 textureCoordinate;

        public Vertex()
        {
            position = Vector3.Zero;
            normal = Vector3.UnitY;
            textureCoordinate = Vector2.Zero;
        }
        public Vertex(Vector3 position)
        {
            this.position = position;
            normal = Vector3.UnitY;
            textureCoordinate = Vector2.Zero;
        }
        public Vertex(Point point)
        {
            position = new Vector3(point.x, point.y, point.z);
            normal = Vector3.UnitY;
            textureCoordinate = new Vector2(point.x, point.z);
        }
        public Vertex(Vector3 position, Vector3 normal)
        {
            this.position = position;
            this.normal = normal;
            textureCoordinate = Vector2.Zero;
        }
        public Vertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            this.position = position;
            this.normal = normal;
            this.textureCoordinate = textureCoordinate;
        }

        public static void BindVAOAttribPointers(VAO vao)
        {
            vao.BindAttribPointer(0, 3, VertexAttribPointerType.Float, 0 * sizeof(float));
            vao.BindAttribPointer(1, 3, VertexAttribPointerType.Float, 3 * sizeof(float));
            vao.BindAttribPointer(2, 2, VertexAttribPointerType.Float, 6 * sizeof(float));
        }

        public void BakeTransformation(Matrix4 transform)
        {
            Vector3 val = (new Vector4(position, 1) * transform).Xyz;
            normal = ((new Vector4(normal, 1) * transform - new Vector4(0, 0, 0, 1) * transform).Xyz).Normalized();
            position = val;
        }

        public void Flip()
        {
            normal *= -1;
        }
        public IVertex Clone() => new Vertex(position, normal, textureCoordinate);
    }
}
