using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics2026.Model.Actors
{
    /// <summary>
    /// An IParent is an interface for a type of Transform that doesn't have a Renderable and no Positions/Rotation..., 
    /// and has only Transforms as its children.
    /// </summary>
    internal interface IParent
    {
        public Transform? GetChild(int i);
        public List<Transform> GetChildren();
        public void AddChild(Transform transform);

        /// <summary>
        /// Checks if a transform is a descendent of this transform.
        /// <br></br>
        /// AKA is a child of the transform, or a child of a child and so on...
        /// </summary>
        /// <returns></returns>
        public bool IsDescendentOfTransform(Transform descendent);
        public List<Transform> GetDescendents();
    }
}
