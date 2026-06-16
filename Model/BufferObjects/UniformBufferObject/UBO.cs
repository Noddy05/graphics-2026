using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.BufferObjects.UniformBufferObject
{
    internal abstract class UBO
    {
        protected BufferUsageHint hint;
        protected readonly int bindingPoint;
        protected const BufferTarget UB = BufferTarget.UniformBuffer;
        protected int handle;

        public UBO(int bindingPoint) : this(bindingPoint, BufferUsageHint.StaticCopy) { }
        public UBO(int bindingPoint, BufferUsageHint hint)
        {
            handle = GL.GenBuffer();
            this.hint = hint;
            this.bindingPoint = bindingPoint;
            SetBindingPoint();
        }

        protected abstract int NumFloats();
        public int SizeOfData() => NumFloats() * sizeof(float);
        protected abstract float[] GetData();

        private void SetBindingPoint()
        {
            Bind();
            GL.BufferData(UB, SizeOfData(), IntPtr.Zero, hint);
            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, bindingPoint, handle, 0, SizeOfData());
            Unbind();
        }

        public void BindData()
        {
            Bind();
            GL.BufferSubData(UB, 0, SizeOfData(), GetData());
            Unbind();
        }
        public void BindSubData(float[] data, int offset, int size)
        {
            Bind();
            GL.BufferSubData(UB, offset * sizeof(float), size * sizeof(float), data);
            Unbind();
        }

        public void Bind()
        {
            GL.BindBuffer(UB, handle);
        }

        public void Unbind()
        {
            GL.BindBuffer(UB, 0);
        }

        public int GetHandle() => handle;


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
