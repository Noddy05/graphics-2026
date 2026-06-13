using Graphics2026.View.Shading.Shaders;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.View.Shading
{
    internal abstract class ShaderProgram : IDisposable
    {
        protected int handle;

        public static readonly PhysicalShader DEFAULT = new DefaultShader();

        public ShaderProgram(params Shader[] shaders)
        {
            handle = GL.CreateProgram();

            AttachShaders(shaders);

            string errorMessage = GL.GetProgramInfoLog(handle);
            if (!string.IsNullOrEmpty(errorMessage))
                Console.WriteLine(errorMessage);

            DetachShaders(shaders);
        }

        public int GetUniformLocation(string name) => GL.GetUniformLocation(handle, name);
        public int GetUniformBlockIndex(string name) => GL.GetUniformBlockIndex(handle, name);

        public virtual void UseProgram()
        {
            GL.UseProgram(handle);
        }

        private void AttachShaders(Shader[] shaders)
        {
            foreach (Shader shader in shaders)
            {
                GL.AttachShader(handle, shader);
            }
            GL.LinkProgram(handle);
        }
        private void DetachShaders(Shader[] shaders)
        {
            foreach (Shader shader in shaders)
            {
                GL.DetachShader(handle, shader);
                GL.DeleteShader(shader);
                shader.Dispose();
            }
        }

        public int GetHandle() => handle;
        public static implicit operator int(ShaderProgram program) => program.handle;

        public void Dispose()
        {
            GL.DeleteShader(handle);
        }
    }
}
