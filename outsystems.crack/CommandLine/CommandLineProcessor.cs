using outsystems.crack.Logging;

namespace outsystems.crack.CommandLine
{
    /// <summary>
    /// Class for processing command-line arguments.
    /// </summary>
    public class CommandLineProcessor
    {
        public static readonly string UsernameArgument = "-u";
        public static readonly string PaswordListArgument = "-p";
        public static readonly string HashArgument = "-H"; 

        public Dictionary<string, string> ArgumentList { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineProcessor"/> class.
        /// </summary>
        public CommandLineProcessor() => ArgumentList = new Dictionary<string, string>();

        /// <summary>
        /// Parses the command-line arguments and populates the argument list.
        /// </summary>
        /// <param name="args">The command-line arguments array.</param>
        /// <returns>True if all required arguments are present; otherwise, false.</returns>
        public bool ParseArguments(string[] args)
        {
            if (args.Length == 1 && (args[0] == "-h"))
            {
                ShowHelp();
                return false;
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    // If the next argument also starts with "-" or doesn't exist, this is a flag without a value
                    if (i == args.Length - 1 || args[i + 1].StartsWith("-"))
                    {
                        ArgumentList[args[i]] = string.Empty;
                    }
                    else
                    {
                        ArgumentList[args[i]] = args[++i];
                    }
                }
            }

            return AreAllArgumentsPresent();
        }

        /// <summary>
        /// Displays the help message for command-line usage.
        /// </summary>
        private static void ShowHelp()
        {
            string c = @"Usage: outsystems.crack [-options]

options:
	-u <username>		outsystems account username
	-p <wordlist>		path to your wordlist
	-H <hash>	        outsystems account password hash 
	-h			show this help message and exit";

            Console.WriteLine(c);
        }

        /// <summary>
        /// Checks if all required arguments are present.
        /// </summary>
        /// <returns>True if all required arguments are present; otherwise, false.</returns>
        private bool AreAllArgumentsPresent()
        {
            if (
                !ArgumentList.ContainsKey(UsernameArgument) ||
                !ArgumentList.ContainsKey(PaswordListArgument) ||
                !ArgumentList.ContainsKey(HashArgument))
            {
                Logger.Print($"[*] Invalid arguments.", LogType.ERROR);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retrieves the value associated with the specified key from the argument list.
        /// </summary>
        /// <param name="key">The key whose value to retrieve.</param>
        /// <returns>The value associated with the key, or an empty string if the key is not found.</returns>
        public string GetValueFromKey(string key)
        {
            return ArgumentList.TryGetValue(key, out var value) ? value : string.Empty;
        }
    }
}
