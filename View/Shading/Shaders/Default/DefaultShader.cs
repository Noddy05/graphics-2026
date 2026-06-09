using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.View.Shading.Shaders
{
    internal class DefaultShader : PhysicalShader
    {
        public Color4 color;
        private int ulColor;

        public DefaultShader() : this(new Color4(1f, 0f, 1f, 1f)) { }
        public DefaultShader(Color4 color):
            base(new Shader(ShaderType.VertexShader,
                Program.LOCAL + "View/Shading/Shaders/Default/default.vert"),
                new Shader(ShaderType.FragmentShader,
                Program.LOCAL + "View/Shading/Shaders/Default/default.frag")) {

            this.color = color;
            ulColor = GetUniformLocation("color");
        }

        public override void UseProgram()
        {
            GL.UseProgram(handle);
            GL.Uniform4(ulColor, color);
        }
    }
}
