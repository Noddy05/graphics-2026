using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Model.BufferObjects.UniformBufferObject
{
    internal class UBO<T> : IBufferObject where T : UniformBufferBlock, new()
    {
        protected BufferUsageHint hint = BufferUsageHint.StaticCopy;
        protected int bindingPoint;
        protected T block;
        protected const BufferTarget UB = BufferTarget.UniformBuffer;

        public UBO(int bindingPoint)
        {
            block = new T();
            this.bindingPoint = bindingPoint;
            SetBindingPoint();
        }
        public UBO(int bindingPoint, BufferUsageHint hint) : this(bindingPoint)
        {
            this.hint = hint;
        }

        public T GetBlock() => block;

        public void SetBindingPoint()
        {
            Bind();
            GL.BufferData(UB, block.BlockSize(), 0, hint);
            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, bindingPoint, block.GetHandle(), 0, block.BlockSize());
            ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
                Console.WriteLine(err);
            Unbind();
        }

        public void BindData()
        {
            Bind();
            float[] data = block.GetData();
            GL.BufferSubData(UB, 0, block.BlockSize(), data);
            Unbind();
        }

        public void Bind()
        {
            GL.BindBuffer(UB, block.GetHandle());
        }

        public void Unbind()
        {
            GL.BindBuffer(UB, 0);
        }

        public int GetHandle() => block.GetHandle();

        public int SizeOfData() => block.BlockSize();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
