using Graphics2026.Model.VertexData;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Graphics2026.Model.BufferObjects
{
    internal abstract class BufferObject<T> : IBufferObject where T : struct
    {
        private int handle;
        protected BufferUsageHint hint;
        protected T[] data = [];
        protected bool discardData = false;
        private int sizeOfData = 0;

        public int SizeOfData() => sizeOfData;
        public BufferObject(T[] data, BufferUsageHint hint)
        {
            sizeOfData = Marshal.SizeOf(default(T));
            handle = GL.GenBuffer();
            this.hint = hint;
            UpdateData(data);
        }
        public BufferObject(T[] data) : this([], BufferUsageHint.DynamicDraw) { }
        public BufferObject() : this([]) { }

        public int GetHandle() => handle;
        public static implicit operator int(BufferObject<T> bufferObject) => bufferObject.handle;
        public void SetBufferUsageHint(BufferUsageHint hint) { this.hint = hint; }
        public BufferUsageHint GetBufferUsageHint() => hint;

        public void UpdateData(T[] data)
        {
            BufferTarget target = GetBufferTarget();
            GL.BindBuffer(target, handle);
            GL.BufferData(target, data.Length * sizeOfData, data, hint);
            GL.BindBuffer(target, 0);

            if (!discardData)
                this.data = data;
        }

        public int GetLengthOfData() => data.Length;
        public T[] GetData() => data;

        protected abstract BufferTarget GetBufferTarget();

        public void Bind()
        {
            GL.BindBuffer(GetBufferTarget(), handle);
        }
        public void Unbind()
        {
            GL.BindBuffer(GetBufferTarget(), 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(handle);
        }
    }
}
