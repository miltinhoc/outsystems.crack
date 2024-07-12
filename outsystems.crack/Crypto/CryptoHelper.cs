using outsystems.crack.Helper;
using outsystems.crack.Logging;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace outsystems.crack.Crypto
{
    /// <summary>
    /// A helper class for cryptographic operations.
    /// </summary>
    public class CryptoHelper
    {
        private const int SaltSize = 44;
        private const string Prefix = "$1$";
        private static readonly object SyncLock = new object();

        private volatile bool _stop;
        private Stopwatch _stopwatch;

        private static Timer _timer;
        private static string _currentPassword;
        private static int _count;
        private static int _lastCount;
        private static bool _firstRun = true;
        private static string _password;

        public static string Password => _password;
        public static int Count => _count;

        /// <summary>
        /// Starts the password cracking process.
        /// </summary>
        /// <param name="wordlistPath">The path to the wordlist file.</param>
        /// <param name="hash">The hashed password.</param>
        /// <param name="username">The username associated with the hashed password.</param>
        /// <returns>The status of the cracking process.</returns>
        public Status Start(string wordlistPath, string hash, string username)
        {
            if (string.IsNullOrEmpty(wordlistPath) || string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("Input arguments cannot be null or empty.");
            }

            Logger.Print($"[*] Trying to find password for user '{username}'..", LogType.INFO);

            _timer = new Timer(_ => OnCallBack(), null, 1000, Timeout.Infinite);

            FormatHash(hash, out string salt, out string hashed);
            _stopwatch = Stopwatch.StartNew();

            foreach (string password in File.ReadLines(wordlistPath))
            {
                _currentPassword = password;
                if (_stop)
                {
                    _stopwatch.Stop();
                    return Status.CANCELLED;
                }

                string generatedWithUsername = GeneratePassword(username, password, salt);
                string generatedWithoutUsername = GeneratePassword(password, salt);

                if (hashed == GetHashed(generatedWithoutUsername) || hashed == GetHashed(generatedWithUsername))
                {
                    _password = password;
                    _stopwatch.Stop();
                    return Status.FOUND;
                }

                lock (SyncLock)
                {
                    _count++;
                }
            }

            _stopwatch.Stop();
            _timer.Dispose();
            return Status.NOT_FOUND;
        }

        /// <summary>
        /// Callback method for the timer to log progress.
        /// </summary>
        private static void OnCallBack()
        {
            lock (SyncLock)
            {
                if (_firstRun)
                {
                    _firstRun = false;
                }
                else
                {
                    ConsoleHelper.ClearLastConsoleLine();
                }

                string countDiff = (_count - _lastCount).ToString("N0", CultureInfo.InvariantCulture);
                Logger.Print($"[*] trying password '{_currentPassword}'... | {countDiff} tries/s", LogType.INFO);

                _lastCount = _count;
            }

            _timer.Change(1000, Timeout.Infinite);
        }

        /// <summary>
        /// Stops the password cracking process.
        /// </summary>
        public void Stop() => _stop = true;

        /// <summary>
        /// Gets the elapsed time of the password cracking process.
        /// </summary>
        /// <returns>The elapsed time in seconds.</returns>
        public string GetElapsedTime() => _stopwatch?.Elapsed.TotalSeconds.ToString("0.000", CultureInfo.InvariantCulture) ?? TimeSpan.Zero.ToString();

        /// <summary>
        /// Formats the hash by extracting the salt and hashed components.
        /// </summary>
        /// <param name="hash">The hashed password.</param>
        /// <param name="salt">The extracted salt.</param>
        /// <param name="hashed">The extracted hashed component.</param>
        private static void FormatHash(string hash, out string salt, out string hashed)
        {
            string dataWithoutPrefix = hash.Replace(Prefix, "");
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataWithoutPrefix);

            byte[] saltArr = new byte[SaltSize];
            byte[] hashedArr = new byte[dataWithoutPrefix.Length - SaltSize];

            Array.Copy(dataBytes, 0, saltArr, 0, SaltSize);
            Array.Copy(dataBytes, SaltSize, hashedArr, 0, hashedArr.Length);

            hashed = Encoding.UTF8.GetString(hashedArr);
            salt = Encoding.UTF8.GetString(saltArr);
        }

        /// <summary>
        /// Generates the hash of the given plain text.
        /// </summary>
        /// <param name="plain">The plain text to hash.</param>
        /// <returns>The hashed value as a hex string.</returns>
        private static string GetHashed(string plain)
        {
            return BitConverter.ToString(ComputeSHA512(EncodeUTF16(plain))).Replace("-", "");
        }

        /// <summary>
        /// Generates a password using the username, password, and salt.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The generated password.</returns>
        private static string GeneratePassword(string username, string password, string salt) => $"{username}{password}{salt}";

        /// <summary>
        /// Generates a password using the password and salt.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The generated password.</returns>
        private static string GeneratePassword(string password, string salt) => $"{password}{salt}";

        /// <summary>
        /// Encodes the given text to UTF-16 bytes.
        /// </summary>
        /// <param name="text">The text to encode.</param>
        /// <returns>The UTF-16 encoded bytes.</returns>
        private static byte[] EncodeUTF16(string text) => new UnicodeEncoding().GetBytes(text);

        /// <summary>
        /// Computes the SHA-512 hash of the given input.
        /// </summary>
        /// <param name="input">The input bytes to hash.</param>
        /// <returns>The SHA-512 hash.</returns>
        private static byte[] ComputeSHA512(byte[] input)
        {
            using (SHA512 sha512Managed = SHA512.Create())
            {
                return sha512Managed.ComputeHash(input);
            }
        }
    }

    /// <summary>
    /// Enum representing the status of the password cracking process.
    /// </summary>
    public enum Status
    {
        FOUND,
        NOT_FOUND,
        CANCELLED
    }
}
