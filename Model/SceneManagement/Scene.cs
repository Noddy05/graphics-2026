using Graphics2026.Model.Actors;

namespace Graphics2026.Model.SceneManagement
{
    internal class Scene : Parent
    {
        protected readonly List<Transform> children = new();
        protected string name;

        public Scene() : this("New Scene") { }
        public Scene(string name)
        {
            this.name = name;
        }
        public string GetName() => name;

        public void AddToScene(IRenderable renderable)
        {
            children.Add(renderable.GetTransform());
            renderable.SetParent(this);
        }
    }
}
