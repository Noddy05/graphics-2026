using Graphics2026.Model.Actors;

namespace Graphics2026.Shopping
{
    internal class Prefab
    {
        public string name;
        public string description = "";
        public IRenderable actor;
        public int price;

        public Prefab(string name, IRenderable actor, int price)
        {
            this.name = name;
            this.actor = actor;
            this.price = price;
        }

        public Prefab Instantiate()
        {
            IRenderable parent = actor.Clone();
            CloneChildren(parent);

            Prefab prefab = new Prefab(name, parent, price);
            prefab.description = description;

            return prefab;
        }

        private void CloneChildren(IRenderable parent)
        {
            foreach (Transform child in parent.GetTransform().GetChildren())
            {
                IRenderable cloneOfChild = child.GetRenderable().Clone();
                cloneOfChild.SetParent(parent.GetTransform());
                CloneChildren(cloneOfChild);
            }
        }
    }
}
