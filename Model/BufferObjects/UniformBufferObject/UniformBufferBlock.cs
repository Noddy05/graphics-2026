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

        public virtual int BlockSize() => GetData().Length * sizeof(float);
        public abstract float[] GetData();
    }
}
