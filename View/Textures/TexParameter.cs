
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.View.Textures
{
    internal struct TexParameter
    {
        public TextureParameterName parameterName;
        public int parameterValue;

        public TexParameter(TextureParameterName parameterName, int parameterValue)
        {
            this.parameterName = parameterName;
            this.parameterValue = parameterValue;
        }
    }
}
