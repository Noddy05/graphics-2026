

namespace Graphics2026.Model.Actors
{
    internal interface IRenderable
    {
        public void Render();
        public void SetRenderStatus(bool shouldRender);
        public bool GetRenderStatus();
        public string GetName();
        public Transform GetTransform();
        public void SetParent(Transform? parent);
    }
}
