using Graphics2026.View.Textures;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.View.Shading.Shaders
{
    internal class TexturedProcedural : PhysicalShader
    {
        private int ulTextureSampler;
        private int ulTextureScale;
        public Texture? texture;
        public float textureScale = 1f;

        public TexturedProcedural() : this(null) { }
        public TexturedProcedural(Texture? texture) :
            base(new Shader(ShaderType.VertexShader,
                Program.LOCAL + "View/Shading/Shaders/TexturedProcedural/vert.glsl"),
                new Shader(ShaderType.FragmentShader,
                Program.LOCAL + "View/Shading/Shaders/TexturedProcedural/frag.glsl"))
        {

            this.texture = texture;
            ulTextureSampler = GetUniformLocation("textureSampler");
            ulTextureScale = GetUniformLocation("textureScale");
        }
        public TexturedProcedural(Texture? texture, float textureScale) : this(texture)
        {
            this.textureScale = textureScale;
        }

        public override void UseProgram()
        {
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.UseProgram(handle);
            GL.Uniform1(ulTextureSampler, 0);
            GL.Uniform1(ulTextureScale, textureScale);

            if (texture == null)
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
