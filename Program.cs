namespace Graphics2026
{
    internal class Program
    {
        public const string LOCAL = "../../../";
        public const string ASSETS = LOCAL + "Assets/";

        private static Window window = new Window();

        static void Main(string[] args)
        {
            window.Run();
        }

        public static Window GetWindow() => window;
    }
}
