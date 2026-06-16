using Graphics2026.Model.Attachments.CameraControls;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.View.Shading.Shaders
{
    internal abstract class PhysicalShader : ShaderProgram
    {
        private int ulTransformationMatrix;

        public PhysicalShader(params Shader[] shaders) : base(shaders) {
            ulTransformationMatrix = GL.GetUniformLocation(handle, "transformationMatrix");
        }
        public virtual void ApplyTransform(Matrix4 transformationMatrix)
        {
            GL.UniformMatrix4(ulTransformationMatrix, false, ref transformationMatrix);
        }
    }
}
