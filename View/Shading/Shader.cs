using OpenTK.Graphics.OpenGL;

namespace Graphics2026.View.Shading
{
    internal class Shader : IDisposable
    {
        private int handle;
        private string shaderSourceLocation;


        public Shader(ShaderType shaderType, string shaderSourceLocation)
        {
            handle = GL.CreateShader(shaderType);
            this.shaderSourceLocation = shaderSourceLocation;
            Init();
        }

        private void Init()
        {
            GL.ShaderSource(handle, File.ReadAllText(shaderSourceLocation));
            GL.CompileShader(handle);
        }


        public int GetHandle() => handle;
        public static implicit operator int(Shader shader) => shader.handle;

        public void Dispose()
        {
            GL.DeleteShader(handle);
        }
    }
}
