using System.Runtime.CompilerServices;

namespace outsystems.crack.Logging
{
    /// <summary>
    /// A logger class for logging messages with different log types and colors.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Prints a log message with the specified log type, caller name, and line number.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="type">The type of log (INFO, WARN, ERROR, FATAL).</param>
        /// <param name="callerName">The name of the caller method (automatically provided).</param>
        /// <param name="lineNumber">The line number in the source code (automatically provided).</param>
        public static void Print(string message, LogType type, [CallerMemberName] string callerName = "", [CallerLineNumber] int lineNumber = 0)
        {
            Console.Write($" {DateTime.Now}");

            WriteWithColor($" [{type}]", TypeColor(type));
            WriteWithColor($" [{callerName}:{lineNumber}]", ConsoleColor.Magenta);

            Console.Write($" {message}\n");
        }

        /// <summary>
        /// Writes the specified text to the console with the specified color.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="color">The color to use for the text.</param>
        private static void WriteWithColor(string text, ConsoleColor color)
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = color;

            Console.Write(text);
            Console.ForegroundColor = current;
        }

        /// <summary>
        /// Determines the console color based on the log type.
        /// </summary>
        /// <param name="type">The log type.</param>
        /// <returns>The console color corresponding to the log type.</returns>
        private static ConsoleColor TypeColor(LogType type)
        {
            return type switch
            {
                LogType.INFO => ConsoleColor.Cyan,
                LogType.WARN => ConsoleColor.Yellow,
                LogType.ERROR => ConsoleColor.Red,
                LogType.FATAL => ConsoleColor.DarkRed,
                _ => ConsoleColor.White,
            };
        }
    }

    /// <summary>
    /// Enumeration for log types.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Informational message.
        /// </summary>
        INFO,

        /// <summary>
        /// Warning message.
        /// </summary>
        WARN,

        /// <summary>
        /// Error message.
        /// </summary>
        ERROR,

        /// <summary>
        /// Fatal error message.
        /// </summary>
        FATAL
    }
}
