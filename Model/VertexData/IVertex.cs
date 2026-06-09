
using Graphics2026.Model.BufferObjects;
using OpenTK.Mathematics;

namespace Graphics2026.Model.VertexData
{
    internal interface IVertex
    {
        public abstract static void BindVAOAttribPointers(VAO vao);
        public abstract void BakeTransformation(Matrix4 transform);
        public abstract void Flip();
        public IVertex Clone();
    }
}
