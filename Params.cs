using System.Numerics;

namespace ArSeqProduct
{
    // Static Class with Parameters
    public static class Params
    {
        // pMax -the number of logical processors
        public static readonly int pMax = Environment.ProcessorCount;

        // P -the number of used logical processors
        public static int P { get; set; } = pMax;
        // D – the difference (step) of the arithmetic sequence
        public static BigInteger D { get; set; } = BigInteger.One;
        // A– the beginning of the arithmetic sequence
        public static BigInteger A { get; set; } = BigInteger.One;
        // N – the number of the intervals in the sequence; N > 0; 
        public static BigInteger N { get; set; } = 1000;
        // T - the interval in minutes to report a progress of the task
        public static int T { get; set; } = 1;
        // S – the straid S = D * P, where P is the number of the used processors. 
        public static BigInteger S { get; set; }
        // E – the end of the last stride   A + (2 * H + 1) * S - D;
        public static BigInteger E { get; set; } = BigInteger.Zero;
        // H – half of quotient - the number of multiplaiers in each task - 1
        public static BigInteger H { get; set; }
        // R -the first multiplicand in the reminder of elements
        public static BigInteger R { get; set; }

        public static bool SetValue(string key, string value)
        {
            if (string.IsNullOrEmpty(value)) return false;

            switch (key.ToLower())
            {
                case "p": P = int.Parse(value); break;
                case "d": D = BigInteger.Parse(value); break;
                case "a": A = BigInteger.Parse(value); break;
                case "n": N = BigInteger.Parse(value); break;
                case "t": T = int.Parse(value); break;

                default: return false; // Key not recognized
            }
            return true; // Value set successfully
        }
    }
}

