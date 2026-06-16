using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Model.BufferObjects.UniformBufferObject
{
    internal class LightBlock : UniformBufferBlock
    {
        public Vector3 lightFromDirection = Vector3.UnitY;

        public override int BlockSize() => 3 * sizeof(float);

        public override float[] GetData() => DataConverter.ToFloatArray(lightFromDirection);
    }
}
