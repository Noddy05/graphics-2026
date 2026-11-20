using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics2026.Model.Actors
{
    internal class Parent : IParent
    {
        private List<Transform> children = new List<Transform>();

        public void AddChild(Transform transform)
        {
            children.Add(transform);
            transform.SetParent(this);
        }

        public Transform? GetChild(int i)
        {
            if (i < children.Count)
                return children[i];

            return null;
        }

        public List<Transform> GetChildren() => children;

        public List<Transform> GetDescendents()
        {
            List<Transform> descendents = [.. children];
            foreach (Transform child in children)
            {
                descendents.AddRange(child.GetDescendents());
            }

            return descendents;
        }
        public bool IsDescendentOfTransform(Transform descendent) => GetDescendents().Contains(descendent);
    }
}
