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
            if (Params.B < 0 || Params.E < Params.B)
                sb.AppendLine("E must be > B > 0");
            if (Params.D < 0) sb.AppendLine("D must be > 0");
            if (((Params.E - Params.B) % Params.D) != 0)
                sb.AppendLine("(E - B) must be divisible by D.");
            if (Params.T < 0 || Params.T > 60)
                sb.AppendLine("T must be between 0 and 60 minutes.");
            return sb.ToString();
        }

        public static int CalcParams()
        {
            BigInteger p = Params.P, b = Params.B, e = Params.E, d = Params.D,
                n = (e - b) / d + 1, r = 0;

            Params.S = d * p;
            Params.H = BigInteger.DivRem(n - p, p * 2, out r);
            Params.R = r;
            Console.WriteLine($"S={Params.S}, H={Params.H}, R={Params.R}");
            LogWrite($"S={Params.S}, H={Params.H}, R={Params.R}");
            if (p == 1 || (n < 5 * p)) return 1;
            Params.E -= (r * d);
            Console.WriteLine($"E={Params.E}");
            LogWrite($"E={Params.E}");
            return 0;

        }
    }
}