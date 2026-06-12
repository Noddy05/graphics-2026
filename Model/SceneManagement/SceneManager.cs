using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;

namespace Graphics2026.Model.SceneManagement
{
    internal static class SceneManager
    {
        private static Scene currentScene = new Scene();
        public static void SetScene(Scene scene)
        {
            currentScene = scene;
        }

        public static Scene CurrentScene() => currentScene;
        public static void AddToScene(IRenderable renderable) => currentScene.AddToScene(renderable);
        public static void AddToScene(Surface surface) => currentScene.AddToScene(surface);
        public static List<Surface> GetSurfaces() => currentScene.GetSurfaces();
    }
}
