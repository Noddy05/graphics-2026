using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Model.BufferObjects.UniformBufferObject
{
    internal class CameraBlock : UBO
    {
        private Matrix4 _view = Matrix4.Identity;
        public Matrix4 view
        {
            get => _view;
            set
            {
                if (_view != value)
                {
                    view = _view;
                    _view = value;
                    ViewUpdated();
                }
            }
        }
        private Matrix4 _projection = Matrix4.Identity;
        public Matrix4 projection
        {
            get => _projection;
            set
            {
                if (_projection != value)
                {
                    projection = _projection;
                    _projection = value;
                    ProjectionUpdated();
                }
            }
        }

        public CameraBlock(int bindingPoint) : base(bindingPoint) { }
        public CameraBlock(int bindingPoint, BufferUsageHint hint) : base(bindingPoint, hint) { }

        private void ViewUpdated() => BindSubData(DataConverter.ToFloatArray(view), 0);
        private void ProjectionUpdated() => BindSubData(DataConverter.ToFloatArray(projection), 16);

        protected override float[] GetData()
            => DataConverter.ToFloatArray(view, projection);

        protected override int NumFloats() => 2 * 16;
    }
}
