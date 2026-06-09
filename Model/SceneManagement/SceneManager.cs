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
    }
}
