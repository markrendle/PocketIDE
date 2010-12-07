namespace PocketIDE
{
    public class Snippet
    {
        public static readonly Snippet ConsoleWriteLine = new Snippet("Console.WriteLine(", ");");
        public static readonly Snippet ForLoop = new Snippet("for (int i = 0; i < 0", "; i++)\r{\r  \r}");
        public static readonly Snippet ForEachLoop = new Snippet("foreach (var item in items", ")\r{\r  \r}");

        private readonly string _before;
        private readonly string _after;

        public Snippet(string before, string after)
        {
            _before = before;
            _after = after;
        }

        public string After
        {
            get { return _after; }
        }

        public string Before
        {
            get { return _before; }
        }
    }
}