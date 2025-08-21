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
        // B– the beginning of the arithmetic sequence
        public static BigInteger B { get; set; } = BigInteger.One;
        // E – the end of the arithmetic sequence; E > B; (E - B) mod D = 0.
        public static BigInteger E { get; set; } = 1000;
        // T - the interval in minutes to report a progress of the task
        public static int T { get; set; } = 1;
        // S – the straid S = D * P, where P is the number of the used processors. 
        public static BigInteger S { get; set; }
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
                case "b": B = BigInteger.Parse(value); break;
                case "e": E = BigInteger.Parse(value); break;
                case "t": T = int.Parse(value); break;

                default: return false; // Key not recognized
            }
            return true; // Value set successfully
        }
    }
}

