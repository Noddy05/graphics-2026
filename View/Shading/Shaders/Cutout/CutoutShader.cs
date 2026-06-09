using OpenTK.Graphics.OpenGL;

namespace Graphics2026.View.Shading.Shaders
{
    internal class CutoutShader : PhysicalShader
    {
        public CutoutShader():
            base(new Shader(ShaderType.VertexShader,
                Program.LOCAL + "View/Shading/Shaders/Cutout/vert.glsl"),
                new Shader(ShaderType.FragmentShader,
                Program.LOCAL + "View/Shading/Shaders/Cutout/frag.glsl")) {
        }
    }
}
