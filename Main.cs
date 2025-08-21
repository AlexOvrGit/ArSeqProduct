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
                    args = "P=7,B = 1, D=1, E=10000;E=100000;T=1,D=1,E=1000000";
                pass++;

                var command = ParseCommand(args, out tail);
                int p = Params.P, t = Params.T;
                BigInteger b = Params.B, e = Params.E, d = Params.D, er = e;
                Console.WriteLine($"Parameters set: b={b}, e={e}, d={d}, t={t}");
                TimeRestart(t);
                string error = ParamsError(command);
                if (!(string.IsNullOrEmpty(error)))
                {
                    Console.WriteLine(error);
                    LogWrite(error);
                    args = tail;
                    continue;
                }
                BigInteger result = 1, m = b;
                if (CalcParams() == 1)
                {
                    Console.WriteLine("Single-thread calculation");

                    while (m <= e) { result *= m; m += d; }

                    Console.WriteLine($"Result: {result}");
                    LogWrite($"Result: {result}");
                    args = tail;
                    continue;
                }
                Console.WriteLine("Multi-thread calculation");
                m = b + p * Params.H * d;
                Thread[] threads = new Thread[p];
                BigInteger[] results = new BigInteger[p];

                int completedThreads = 0;
                for (int i = 0; i < p; i++)
                {
                    int index = i;
                    if (i > 0) m += d;
                    result = TaskExe(index, m, logLock);
                    lock (logLock)
                    {
                        results[index] = result;
                        completedThreads++;
                    }
                }

                Console.WriteLine("All threads completed.");



                //               Console.WriteLine("---------------------");

                // Multiply all partial products together
                BigInteger total = 1;
                foreach (var part in results) total *= part;
                // { total *= part; Console.WriteLine($"\nPart RESULT: {part}"); }

                lCountTime = sw.ElapsedMilliseconds;
                string sFormat = ((double)total < 1.0E30) ? "{0:N0}" : "{0:E8}";
                Console.WriteLine("Product = " + sFormat, total);
                                                                                                               Console.Write("Count Time = {0}ms", lCountTime);

  //              Console.Write(", Output Time = {1}ms",
  //                  sw.ElapsedMilliseconds - lCountTime);
                Console.WriteLine();

               //Console.WriteLine("--Simple check--");
               //BigInteger check = 1;
               //m = b;
               //while (m <= er) { check *= m; m += d; }
               //Console.WriteLine($"Check: {check}");
               args = tail;
                Console.WriteLine("---------------------");
            }
            TimeStop();
            Console.WriteLine("Enter key to exit");
            Console.ReadKey();
        }

    }
}
