using Graphics2026.Model.Attachments.CameraControls;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.View.Shading.Shaders
{
    internal abstract class PhysicalShader : ShaderProgram
    {
        private int ulTransformationMatrix;
        private int ulCameraMatrix;
        private int ulProjectionMatrix;

        public PhysicalShader(params Shader[] shaders) : base(shaders) {
            ulTransformationMatrix = GL.GetUniformLocation(handle, "transformationMatrix");
            ulCameraMatrix = GL.GetUniformLocation(handle, "cameraMatrix");
            ulProjectionMatrix = GL.GetUniformLocation(handle, "projectionMatrix");
        }
        public virtual void ApplyTransform(Matrix4 transformationMatrix)
        {
            GL.UniformMatrix4(ulTransformationMatrix, false, ref transformationMatrix);
        }
        public virtual void ApplyProjection(Camera? camera)
        {
            Matrix4 projectionMatrix = Matrix4.Identity;

            if (camera != null)
                projectionMatrix = camera.GetProjection();

            GL.UniformMatrix4(ulProjectionMatrix, false, ref projectionMatrix);
        }
        public virtual void ApplyCamera(Camera? camera)
        {
            Matrix4 cameraMatrix = Matrix4.Identity;

            if (camera != null)
                cameraMatrix = camera.GetCameraMatrix();

            GL.UniformMatrix4(ulCameraMatrix, false, ref cameraMatrix);
        }
        public virtual void ApplyCameraAndProjection(Camera? camera)
        {
            ApplyProjection(camera);
            ApplyCamera(camera);
        }
    }
}
