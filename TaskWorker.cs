using System.Numerics;

namespace ArSeqProduct
{
    internal partial class Program
    {
        public static BigInteger Worker(int t, BigInteger m, Lock logLock)
        {
            BigInteger result = m, s = Params.S, s2 = s * s, ds2 = 2 * s2, h = Params.H, n = Params.N;
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
                lock (logLock)
                {
                    if (Params.R <= 0) break;
                    Params.E += Params.D;
                    Params.R--;
                    m = Params.E;

                }
                result *= m;
            }
            lock (logLock) Console.WriteLine($"index={t},result={result}");
            //even={Big.even}, odd={Big.odd}, all={Big.even * Big.odd}
            return result;
        }
        public static (BigInteger even, BigInteger odd) Worker2(int t, BigInteger m, Lock logLock)
        {
            BigInteger m2 = 2 * m + 1, s = Params.S, s2 = s * s, ds2 = 2 * s2, h = Params.H, n = Params.N;
            (BigInteger even, BigInteger odd) Big = (m, m + 1);
            m *= m;
            for (; h > 0; h--)
            {
                m -= s2;
                Big.even *= m;
                Big.odd *= (m + m2);
                s2 += ds2;
            }
            //         return result;
            while (Params.R > 0)
            {
                lock (logLock)
                {
                    if (Params.R <= 0) break;
                    n += Params.D;
                    Params.R--;
                    m = Params.N;
                }
                Big.even *= m;
                Big.odd *= (m + 1);
            }
            lock (logLock) Console.WriteLine
    ($"index={t},result={Big.even}");
            //even={Big.even}, odd={Big.odd}, all={Big.even * Big.odd}

            return Big;
        }
    }
}
