using Graphics2026.Model.Actors;
using OpenTK.Mathematics;

namespace Graphics2026.Shopping
{
    internal class Prefab
    {
        public string name = "";
        public string description = "";
        public IRenderable renderable;
        public Vector2i size;
        public int price;

        public Prefab(IRenderable renderable) {
            this.renderable = renderable;
        }
        public Prefab(string name, IRenderable renderable, int price)
        {
            this.name = name;
            this.renderable = renderable;
            this.price = price;
        }

        public Prefab Instantiate()
        {
            IRenderable parent = renderable.Clone();
            CloneChildren(parent, renderable.GetTransform());

            Prefab prefab = new Prefab(name, parent, price);
            prefab.description = description;
            prefab.size = size;

            return prefab;
        }

        private void CloneChildren(IRenderable newParent, Transform originalTransform)
        {
            foreach (Transform child in originalTransform.GetChildren())
            {
                IRenderable cloneOfChild = child.GetRenderable().Clone();
                cloneOfChild.SetParent(newParent.GetTransform());
                cloneOfChild.GetTransform().SetMatrixMod(child.GetMatrixMod());
                CloneChildren(cloneOfChild, child);
            }
        }
    }
}
