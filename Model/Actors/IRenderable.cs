

using Graphics2026.View.Shading.Shaders;

namespace Graphics2026.Model.Actors
{
    internal interface IRenderable
    {
        public void Render();
        public void RenderWithShader(PhysicalShader shader);
        public void RenderFamilyWithShader(PhysicalShader shader);
        public void SetRenderStatus(bool shouldRender);
        public bool GetRenderStatus();
        public string GetName();
        public Transform GetTransform();
        public void SetParent(Transform? parent);
    }
}
