using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.BufferObjects
{
    internal class VAO : IDisposable
    {
        private int handle;
        private IBufferObject vbo;

        public VAO(IBufferObject vbo)
        {
            handle = GL.GenVertexArray();
            this.vbo = vbo;
        }

        /// <summary>
        /// Size, stride and offset is given in terms of data-units. If the data unit is float,
        /// then a stride of 3 represents a stride of 3 * sizeof(float)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size">Size in terms of data-units</param>
        /// <param name="pointerType"></param>
        /// <param name="stride">Stride in terms of data-units</param>
        /// <param name="offset">Offset in terms of data-units</param>
        public void BindAttribPointer(int index, int size, VertexAttribPointerType pointerType, int offset)
        {
            Bind();

            GL.VertexAttribPointer(index, size, pointerType, false, vbo.SizeOfData(), offset);
            GL.EnableVertexAttribArray(index);

            Unbind();
        }

        public int GetHandle() => handle;
        public static implicit operator int(VAO vao) => vao.handle;

        public void Bind()
        {
            GL.BindVertexArray(handle);
            vbo.Bind();
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
            vbo.Unbind();
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(handle);
        }

    }
}
