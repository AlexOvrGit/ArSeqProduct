using System.Diagnostics;
using System.Numerics;

namespace ArSeqProduct
{

    internal partial class Program
    {

        private static readonly Lock logLock = new();
        private static bool repFlag = false;
        private static readonly System.Timers.Timer timer = new(60_000); // Timer set to 1 minute
        private static readonly Stopwatch sw = new();
        public static long lCountTime = 0;
        private static
        StreamWriter log = new("ArSeqProduct_log.txt", false);
        public static string tail = string.Empty;
        public static int pass = 0;
        public static string args = string.Empty;
        static void Main() // Main method to run the progr// amasync Task
        {

            log.AutoFlush = true;

            while (pass == 0 || !(string.IsNullOrEmpty(args)))
            {
                if (pass == 0)
                    args = " a= 4, d=2, n=40;";
                pass++;

                var command = ParseCommand(args, out tail);
                int p = Params.P, t = Params.T;
                BigInteger a = Params.A, n = Params.N, d = Params.D, er = n;
                Console.WriteLine($"Parameters set: a={a}, n={n}, d={d}, t={t}");
                TimeRestart(t);
                string error = ParamsError(command);
                if (!(string.IsNullOrEmpty(error)))
                {
                    Console.WriteLine(error);
                    LogWrite(error);
                    args = tail;
                    continue;
                }
                BigInteger result = 1, m = a;
                if (CalcParams() == 1)
                {
                    Console.WriteLine("Single-thread calculation");

                    while (m <= n) { result *= m; m += d; }

                    Console.WriteLine($"Result: {result}");
                    LogWrite($"Result: {result}");
                    args = tail;
                    continue;
                }
                Console.WriteLine("Multi-thread calculation");
                m = a + Params.S * Params.H; // m is the first multiplicand for the  thread
                Thread[] threads = new Thread[p];
                BigInteger[] results = new BigInteger[p];

                int completedThreads = 0;
                for (int i = 0; i < p; i++)
                {
                    int index = i;
                    if (i > 0) m += d;
                    result = Worker(index, m, logLock);
                    lock (logLock)
                    {
                        results[index] = result;
                        completedThreads++;
                    }
                }

                Console.WriteLine("All threads completed.");
                lCountTime = sw.ElapsedMilliseconds;
                Console.WriteLine(" Worker's Time = {0}ms", sw.ElapsedMilliseconds);

                Console.WriteLine($"BalancedMultiply {results.Length} partial products together");
                BigInteger total = BalancedMultiply(results);
                Console.WriteLine("BalancedMultiply {0} partial products Time = {1}ms",
                    results.Length, sw.ElapsedMilliseconds - lCountTime);
                lCountTime = sw.ElapsedMilliseconds;
                string sFormat = ((double)total < 1.0E30) ? "{0:N0}" : "{0:E8}";
                Console.Write("Product = " + sFormat, total);
                Console.Write("Count Time = {0}ms", lCountTime);
                Console.WriteLine(", Output Time = {0}ms",
                  sw.ElapsedMilliseconds - lCountTime);
                args = tail;
                Console.WriteLine("---------------------");
            }
            TimeStop();
            Console.WriteLine("Enter key to exit");
            Console.ReadKey();
        }

    }
}
