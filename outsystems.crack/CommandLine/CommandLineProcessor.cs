namespace outsystems.crack.CommandLine
{
    public class CommandLineProcessor
    {
        public static readonly string UsernameArgument = "-u";
        public static readonly string PaswordListArgument = "-p";
        public static readonly string HashArgument = "-h"; 

        public Dictionary<string, string> ArgumentList { get; private set; }

        public CommandLineProcessor() => ArgumentList = new Dictionary<string, string>();

        public bool ParseArguments(string[] args)
        {
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

        private bool AreAllArgumentsPresent()
        {
            return (ArgumentList.ContainsKey(UsernameArgument)
                && ArgumentList.ContainsKey(PaswordListArgument)
                && ArgumentList.ContainsKey(HashArgument));
        }

        public string GetValueFromKey(string key)
        {
            return ArgumentList.TryGetValue(key, out var value) ? value : string.Empty;
        }
    }
}
