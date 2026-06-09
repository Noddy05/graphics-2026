using OpenTK.Graphics.OpenGL;

namespace Graphics2026.View.Textures
{
    internal class Texture
    {
        private int handle;
        private TextureTarget target;

        public Texture(int handle, TextureTarget target)
        {
            this.handle = handle;
            this.target = target;
        }
        public Texture(string source, TextureTarget target)
        {
            handle = TextureLoader.LoadNewTexture2D(source);
            this.target = target;
        }

        public static implicit operator int(Texture texture) => texture.handle;

        public void Bind()
        {
            GL.BindTexture(target, handle);
        }

        public Texture AddParameter(TexParameter parameter)
        {
            GL.BindTexture(target, handle);
            GL.TexParameter(target, parameter.parameterName, parameter.parameterValue);
            GL.BindTexture(target, 0);

            return this;
        }
    }
}
