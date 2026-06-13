using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.BufferObjects.UniformBufferObject
{
    internal abstract class UniformBufferBlock
    {
        protected int handle;
        public UniformBufferBlock()
        {
            handle = GL.GenBuffer();
        }

        public int GetHandle() => handle;

        public abstract int BlockSize();
        public abstract float[] GetData();
    }
}
