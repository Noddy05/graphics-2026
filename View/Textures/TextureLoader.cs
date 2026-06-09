using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Graphics2026.View.Textures
{
    internal class TextureLoader
    {
        public static readonly Texture white = new Texture(GenerateColorTexture(Color4.White), TextureTarget.Texture2D);
        public static readonly Texture red = new Texture(GenerateColorTexture(Color4.Red), TextureTarget.Texture2D);
        public static readonly Texture blue = new Texture(GenerateColorTexture(Color4.Blue), TextureTarget.Texture2D);
        public static readonly Texture green = new Texture(GenerateColorTexture(Color4.Lime), TextureTarget.Texture2D);
        public static readonly Texture black = new Texture(GenerateColorTexture(Color4.Black), TextureTarget.Texture2D);

        public static int GenerateColorTexture(Color4 color) => LoadNewTexture2D(
            [
                (byte)(color.R * 255),
                (byte)(color.G * 255),
                (byte)(color.B * 255),
                (byte)(color.A * 255),
            ], 1, 1);

        public static int LoadNewTexture2D(string imagePath,
            params (TextureParameterName textureParameterName, int parameter)[] parameters)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException();
            }

            byte[] imageData = LoadImageData(imagePath, out int width, out int height);
            return LoadNewTexture2D(imageData, width, height, parameters);
        }
        public static int LoadNewTexture2D(byte[] imageData, int width, int height,
            params (TextureParameterName textureParameterName, int parameter)[] parameters)
        {
            int newTexture = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, newTexture);

            foreach (var parameter in parameters)
            {
                GL.TexParameter(TextureTarget.Texture2D, parameter.textureParameterName,
                    parameter.parameter);
            }

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, imageData);

            return newTexture;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability",
            "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        private static byte[] LoadImageData(string filePath, out int width, out int height)
        {
            using Bitmap bitmap = new Bitmap(filePath);
            width = bitmap.Width; height = bitmap.Height;

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] bytes = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            bitmap.UnlockBits(data);
            //Flip on y-axis
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bytes;
        }
    }
}
