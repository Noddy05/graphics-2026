using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;

namespace Graphics2026.Model.SceneManagement
{
    internal class Scene
    {
        public readonly List<Surface> surfaces = new();
        //public readonly List<Actor> actors = new();
        public readonly List<IRenderable> renderables = new();

        /// <summary>
        /// Finds the first actor with the given name
        /// <br></br>
        /// Returns null if no actor was found
        /// </summary>
        public Actor? FindActor(string name)
        {
            foreach (IRenderable renderable in renderables)
            {
                if (renderable.GetType() != typeof(Actor))
                    continue;

                if (renderable.GetName() == name)
                    return (Actor)renderable;
            }

            return null;
        }
        /// <summary>
        /// Finds the first surface with the given name
        /// <br></br>
        /// Returns null if no surface was found
        /// </summary>
        public Surface? FindSurface(string name)
        {
            foreach (Surface surface in surfaces)
            {
                if (surface.name == name)
                    return surface;
            }

            return null;
        }
    }
}
