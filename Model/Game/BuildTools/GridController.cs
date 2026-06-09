using Graphics2026.Controller;
using Graphics2026.Model.Actors.Gizmos;

namespace Graphics2026.Model.Game.BuildTools
{
    internal class GridController : ActiveRoleBehaviour
    {
        protected override void Update(float deltaTime)
        {
            float scrollY = Input.Scroll();
            if (scrollY != 0)
            {
                Grid.SetSubdivisions(float.Sign(scrollY) + Grid.GetSubdivisions());
            }
        }
    }
}
