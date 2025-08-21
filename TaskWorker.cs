using System.Numerics;

namespace ArSeqProduct
{
    internal partial class Program
    {
        public static BigInteger TaskExe(int t, BigInteger m, Lock logLock)
        {
            BigInteger result = m, s = Params.S, s2 = s * s, ds2 = 2 * s2, h = Params.H;

            m *= m;
            for (; h > 0; h--)
            {
                result *= (m - s2);
                m -= s2;
                s2 += ds2;
            }
            //         return result;
            while (Params.R > 0)
            {
                m = BigInteger.One;
                lock (logLock)
                {
                    if (Params.R <= 0) break;
                    Params.E += Params.D;
                    Params.R--;
                    m = Params.E;
                }
                result *= m;
            }
            return result;
        }
    }
}
