using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Model.BufferObjects.UniformBufferObject
{
    internal class CameraBlock : UniformBufferBlock
    {
        public Matrix4 view = Matrix4.Identity;
        public Matrix4 projection = Matrix4.Identity;

        public override int BlockSize() => 32 * sizeof(float);

        public override float[] GetData() => Helper.Matrix4ToFloatArray([view, projection]);
    }
}
