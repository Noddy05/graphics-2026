using Graphics2026.View.Textures;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.View.Shading.Shaders
{
    internal class TexturedShader : PhysicalShader
    {
        private int ulTextureSampler;
        public Texture? texture;

        public TexturedShader() : this(null) { }
        public TexturedShader(Texture? texture) :
            base(new Shader(ShaderType.VertexShader,
                Program.LOCAL + "View/Shading/Shaders/Textured/vert.glsl"),
                new Shader(ShaderType.FragmentShader,
                Program.LOCAL + "View/Shading/Shaders/Textured/frag.glsl")) {

            this.texture = texture;
            ulTextureSampler = GetUniformLocation("textureSampler");
        }

        public override void UseProgram()
        {
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.UseProgram(handle);
            GL.Uniform1(ulTextureSampler, 0);

            if(texture == null)
            {
                TextureLoader.white.Bind();
            } 
            else
            {
                texture.Bind();
            }
        }
    }
}
