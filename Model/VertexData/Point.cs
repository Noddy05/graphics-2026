using Graphics2026.Model.BufferObjects;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.InteropServices;

namespace Graphics2026.Model.VertexData
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Point : IVertex
    {
        public float x, y, z;

        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Point(Vector3 v)
        {
            x = v.X;
            y = v.Y;
            z = v.Z;
        }

        public static void BindVAOAttribPointers(VAO vao)
        {
            vao.BindAttribPointer(0, 3, VertexAttribPointerType.Float, 0);
        }

        public void BakeTransformation(Matrix4 transform)
        {
            Vector3 val = (new Vector4(x, y, z, 1) * transform).Xyz;
            x = val.X;
            y = val.Y;
            z = val.Z;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public void Flip() { }
        public IVertex Clone() => new Point(x, y, z);
    }
}
