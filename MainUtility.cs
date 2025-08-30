using System.Numerics;
using System.Text;
using System.Timers;
namespace ArSeqProduct
{

    internal partial class Program
    {
        public static int taskRep = 0;
        static void TimeRestart(int interval)
        {
            sw.Restart();
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Stop();
            timer.Interval = interval * 60_000;
            timer.Start();
        }

        private static void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            repFlag = true;
            lock (logLock) taskRep = Params.P;
        }
        public static void RepReset()
        {
            lock (logLock)
            {
                if (taskRep > 0) taskRep--;
                if (taskRep < 0) taskRep = 0;
                if (taskRep == 0) repFlag = false;
            }
        }
        public static void TimeStop()
        {
            timer.Stop();
            timer.Elapsed -= OnTimedEvent;
            sw.Stop();
            repFlag = false;
            lock (logLock) taskRep = 0;
        }
        public static void LogWrite(string message)
        {
            lock (logLock)
                log.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\n - {message}");
        }
        public static string ParamsError(Dictionary<string, string> command)
        {
            //          int pMax = Environment.ProcessorCount;
            StringBuilder sb = new();
            if (command.Count == 0)
                sb.AppendLine("No valid command line arguments.");
            if (Params.P <= 0) Params.P = 1;
            Console.WriteLine($" {Params.P} processors used out of {Params.pMax}");
            if (Params.A < 1 || Params.N < 1)
                sb.AppendLine("A, N  must be > 0");
            if (Params.D < 0) sb.AppendLine("D must be > 0");
             
            if (Params.T < 0 || Params.T > 60)
                sb.AppendLine("T must be between 0 and 60 minutes.");
            return sb.ToString();
        }

        public static int CalcParams()
        {
            BigInteger n = Params.N, d = Params.D, r; // a = Params.A, o = Params.O;
           int p = Params.P;
           if (n < 3 * p) p = (int)n / 3; if (p < 1) p = 1;

           BigInteger h = BigInteger.DivRem(n - p, p * 2, out r);
           BigInteger l = 2 * h + 1;
        
           Params.H = h;
           Params.S = d * p;
           Params.R = r;
           Console.WriteLine($"S={Params.S}, H={Params.H}, R={Params.R}");
           LogWrite($"S={Params.S}, H={Params.H}, R={Params.R}");
           return p;
           //if ((p == 1 || (n < 4 * p)) && o == 0) return 1;
           //Params.N -= (r * d);
           //Console.WriteLine($"N={Params.N}");
           //LogWrite($"N={Params.N}");

        }
 
        public static BigInteger BalancedMultiply(BigInteger[] w)
        {
            int len = w.Length, i, j;
            if (len == 0) return BigInteger.One;
            if (len == 1) return w[0];
            while (len > 1)
            {
                i = len % 2; j = i;
                while (i < len - 1)
                {
                    w[j] = w[i] * w[i + 1];
                    j++; i += 2; 
                }
                len = j;
            }
            return w[0];
        }
    }
}