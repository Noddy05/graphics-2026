using Graphics2026.Model.VertexData;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.BufferObjects
{
    internal partial class VBO<T> : BufferObject<T> where T : struct, IVertex
    {
        public VBO(T[] data, BufferUsageHint hint) : base(data, hint) { }
        public VBO(T[] data) : base(data) { }
        public VBO() : base() { }

        protected override BufferTarget GetBufferTarget() => BufferTarget.ArrayBuffer;
    }
}
