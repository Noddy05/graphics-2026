using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.BufferObjects
{
    internal class EBO : BufferObject<uint>
    {
        public EBO(uint[] data, BufferUsageHint hint) : base(data, hint) { }
        public EBO(uint[] data) : base(data) { }
        public EBO() : base() { }

        protected override BufferTarget GetBufferTarget() => BufferTarget.ElementArrayBuffer;

    }
}
