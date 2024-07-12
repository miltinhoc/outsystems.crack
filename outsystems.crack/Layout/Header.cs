using System.Reflection;

namespace outsystems.crack.Layout
{
    /// <summary>
    /// Class responsible for displaying a custom header with a logo and version information.
    /// </summary>
    public class Header
    {
        private static readonly Dictionary<string, ConsoleColor> logo = new();
        private static bool isInit;
        private static readonly ConsoleColor color = ConsoleColor.DarkRed;

        /// <summary>
        /// Initializes the header with the logo and version information.
        /// </summary>
        public static void Init()
        {
            isInit = true;
            logo.Add(@"", Console.ForegroundColor);
            logo.Add(@"  ▒█████    ██████     ▄████▄   ██▀███   ▄▄▄       ▄████▄   ██ ▄█▀", color);
            logo.Add(@" ▒██▒  ██▒▒██    ▒    ▒██▀ ▀█  ▓██ ▒ ██▒▒████▄    ▒██▀ ▀█   ██▄█▒ ", color);
            logo.Add(@" ▒██░  ██▒░ ▓██▄      ▒▓█    ▄ ▓██ ░▄█ ▒▒██  ▀█▄  ▒▓█    ▄ ▓███▄░ ", color);
            logo.Add(@" ▒██   ██░  ▒   ██▒   ▒▓▓▄ ▄██▒▒██▀▀█▄  ░██▄▄▄▄██ ▒▓▓▄ ▄██▒▓██ █▄ ", color);
            logo.Add(@" ░ ████▓▒░▒██████▒▒   ▒ ▓███▀ ░░██▓ ▒██▒ ▓█   ▓██▒▒ ▓███▀ ░▒██▒ █▄", color);
            logo.Add(@" ░ ▒░▒░▒░ ▒ ▒▓▒ ▒ ░   ░ ░▒ ▒  ░░ ▒▓ ░▒▓░ ▒▒   ▓▒█░░ ░▒ ▒  ░▒ ▒▒ ▓▒", color);
            logo.Add(@"   ░ ▒ ▒░ ░ ░▒  ░ ░     ░  ▒     ░▒ ░ ▒░  ▒   ▒▒ ░  ░  ▒   ░ ░▒ ▒░", color);
            logo.Add(@" ░ ░ ░ ▒  ░  ░  ░     ░          ░░   ░   ░   ▒   ░        ░ ░░ ░ ", color);
            logo.Add(@"     ░ ░        ░     ░ ░         ░           ░  ░░ ░      ░  ░   ", color);
            logo.Add(@"                      ░                           ░               ", color);
            logo.Add($" [miltinh0c] (v{Assembly.GetExecutingAssembly().GetName().Version})\n", ConsoleColor.White);
        }

        /// <summary>
        /// Draws the header with the logo and version information.
        /// </summary>
        public static void Draw()
        {
            if (!isInit)
            {
                Init();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            ConsoleColor startColor = ConsoleColor.White;

            foreach (KeyValuePair<string, ConsoleColor> keyValue in logo)
            {
                if (Console.ForegroundColor != keyValue.Value)
                    Console.ForegroundColor = keyValue.Value;

                Console.WriteLine(keyValue.Key);
            }

            Console.ForegroundColor = startColor;
        }
    }
}
