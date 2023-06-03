using outsystems.crack.Helper;
using outsystems.crack.Logging;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace outsystems.crack.Crypto
{
    public class CryptoHelper
    {
        // Constants and read-only fields.
        private const int SaltSize = 44;
        private const string Prefix = "$1$";
        private static readonly object SyncLock = new object();

        private volatile bool _stop;
        private Stopwatch _stopwatch;

        // Private instance fields.
        private static Timer _timer;
        private static string _currentPassword;
        private static int _count;
        private static int _lastCount;
        private static bool _firstRun = true;
        private static string _password;

        // Public properties.
        public static string Password => _password;
        public static int Count => _count;

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

        public void Stop() => _stop = true;

        public string GetElapsedTime() => _stopwatch?.Elapsed.TotalSeconds.ToString("0.000", CultureInfo.InvariantCulture) ?? TimeSpan.Zero.ToString();

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

        private static string GetHashed(string plain)
        {
            return BitConverter.ToString(ComputeSHA512(EncodeUTF16(plain))).Replace("-", "");
        }

        private static string GeneratePassword(string username, string password, string salt) => $"{username}{password}{salt}";

        private static string GeneratePassword(string password, string salt) => $"{password}{salt}";

        private static byte[] EncodeUTF16(string text) => new UnicodeEncoding().GetBytes(text);

        private static byte[] ComputeSHA512(byte[] input)
        {
            using (SHA512 sha512Managed = SHA512.Create())
            {
                return sha512Managed.ComputeHash(input);
            }
        }
    }
    public enum Status
    {
        FOUND,
        NOT_FOUND,
        CANCELLED
    }
}
