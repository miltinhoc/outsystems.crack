
using outsystems.crack.CommandLine;
using outsystems.crack.Crypto;
using outsystems.crack.Layout;
using outsystems.crack.Logging;

namespace outsystems.crack
{
    public static class Program
    {
        private static CryptoHelper _cryptoHelper;

        public static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            CommandLineProcessor processor = new CommandLineProcessor();

            if (!processor.ParseArguments(args))
            {
                
                return;
            }

            Header.Draw();
            Status responseStatus = Status.NOT_FOUND;

            _cryptoHelper = new CryptoHelper();

            try
            {
                responseStatus = _cryptoHelper.Start(
                    processor.GetValueFromKey(CommandLineProcessor.PaswordListArgument), 
                    processor.GetValueFromKey(CommandLineProcessor.HashArgument),
                    processor.GetValueFromKey(CommandLineProcessor.UsernameArgument));
            }
            catch(Exception ex)
            {
                Logger.Print(ex.Message, LogType.ERROR);
            }

            var username = processor.GetValueFromKey(CommandLineProcessor.UsernameArgument);
            switch (responseStatus)
            {
                case Status.FOUND:
                    Logger.Print($"[*] Password for user '{username}' -> '{CryptoHelper.Password}'. Found in {_cryptoHelper.GetElapsedTime()} seconds", LogType.INFO);
                    break;
                case Status.NOT_FOUND:
                    Logger.Print($"[*] Password for user '{username}' not found", LogType.INFO);
                    break;
                case Status.CANCELLED:
                    Logger.Print($"[*] Cancelled by the user", LogType.WARN);
                    break;
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _cryptoHelper.Stop();
            e.Cancel = true;
        }
    }
}