using Graphics2026.Model.BufferObjects;
using Graphics2026.View.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Model.FramebufferObject
{
    internal class FBO : IBufferObject
    {
        protected int handle;
        protected Vector2i size;
        protected List<int> textures = new List<int>();

        private bool disposed = false;

        public FBO() {
            handle = GL.GenFramebuffer();
        }

        public FBO(Vector2i size) : this() { 
            this.size = size;
        }

        public void Bind()
        {
            BindFramebuffer(this);
        }

        protected virtual void BindFramebuffer(int frameBuffer)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.Viewport(0, 0, size.X, size.Y);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            Window window = Program.GetWindow();
            GL.Viewport(0, 0, window.Size.X, window.Size.Y);
        }

        public int AddTexture(FramebufferAttachment attachment = FramebufferAttachment.ColorAttachment0)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, handle);

            int texture = TextureLoader.CreateTexture2D(null, size.X, size.Y,
                new TexParameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear),
                new TexParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear));

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, TextureTarget.Texture2D, texture, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            return texture;
        }

        public int AddTexture(FramebufferAttachment attachment, PixelInternalFormat pixelInternalFormat, PixelFormat pixelFormat)
        {
            int texture = TextureLoader.CreateTexture2D(null, size.X, size.Y, pixelInternalFormat, pixelFormat,
                new TexParameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear),
                new TexParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear));

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, TextureTarget.Texture2D, texture, 0);

            return texture;
        }

        public int GetHandle() => handle;
        public static implicit operator int(FBO fbo) => fbo.handle;


        public void Dispose()
        {
            if (disposed)
                return;

            GL.DeleteFramebuffer(handle);
            foreach(int texture in textures)
            {
                GL.DeleteTexture(texture);
            }

            disposed = true;
        }
        public int SizeOfData()
        {
            throw new NotImplementedException();
        }
    }
}
