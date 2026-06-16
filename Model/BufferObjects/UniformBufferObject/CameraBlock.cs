using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Model.BufferObjects.UniformBufferObject
{
    internal class CameraBlock : UBO 
    {
        private Matrix4 view = Matrix4.Identity;
        private Matrix4 projection = Matrix4.Identity;

        public CameraBlock(int bindingPoint) : base(bindingPoint) { }
        public CameraBlock(int bindingPoint, BufferUsageHint hint) : base(bindingPoint, hint) { }

        public void UpdateView(Matrix4 view)
        {
            this.view = view;
            BindSubData(Helper.Matrix4ToFloatArray(view), 0, 16);
        }
        public void UpdateProjection(Matrix4 projection)
        {
            this.projection = projection;
            BindSubData(Helper.Matrix4ToFloatArray(projection), 16, 16);
        }

        protected override float[] GetData()
            => Helper.Matrix4ToFloatArray([view, projection]);

        protected override int NumFloats() => 2 * 16;
    }
}
