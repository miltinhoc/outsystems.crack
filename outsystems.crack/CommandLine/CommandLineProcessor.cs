using outsystems.crack.Logging;

namespace outsystems.crack.CommandLine
{
    public class CommandLineProcessor
    {
        public static readonly string UsernameArgument = "-u";
        public static readonly string PaswordListArgument = "-p";
        public static readonly string HashArgument = "-H"; 

        public Dictionary<string, string> ArgumentList { get; private set; }

        public CommandLineProcessor() => ArgumentList = new Dictionary<string, string>();

        public bool ParseArguments(string[] args)
        {
            if (args.Length == 1 && (args[0] == "-h"))
            {
                ShowHelp();
                return false;
            }

            for (int i = 0; i < args.Length; i++)
            {
                // If this argument starts with "-"
                if (args[i].StartsWith("-"))
                {
                    // If the next argument also starts with "-" or doesn't exist, this is a flag without a value
                    if (i == args.Length - 1 || args[i + 1].StartsWith("-"))
                    {
                        ArgumentList[args[i]] = string.Empty;
                    }
                    else // This argument has a corresponding value
                    {
                        ArgumentList[args[i]] = args[++i];
                    }
                }
            }

            return AreAllArgumentsPresent();
        }

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

        public string GetValueFromKey(string key)
        {
            return ArgumentList.TryGetValue(key, out var value) ? value : string.Empty;
        }
    }
}
