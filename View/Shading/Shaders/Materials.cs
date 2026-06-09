
using Graphics2026.View.Textures;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.View.Shading.Shaders
{
    internal class Materials
    {
        public static readonly TexturedShader floorShader;

        static Materials()
        {
            floorShader = new TexturedShader(
                new Texture(Program.LOCAL + "View/Textures/Bricks059_4K-PNG_Color.png", TextureTarget.Texture2D)
                .AddParameter(new TexParameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear))
                .AddParameter(new TexParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear))
                .AddParameter(new TexParameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat))
                .AddParameter(new TexParameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat))
            );
        }
    }
}
