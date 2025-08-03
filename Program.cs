using System.Diagnostics;
using System.Numerics;

namespace ArSeqProduct
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            int nArg = 0, nLen = args.Length, iDiff, iFrom, iTo;
            Stopwatch sw = new();
            sw.Start();
            long lCountTime = 0;
            while (nLen - nArg >= 3)
            {
                iDiff = int.Parse(args[nArg]);
                iFrom = int.Parse(args[nArg + 1]);
                iTo = int.Parse(args[nArg + 2]);
                Console.WriteLine("Diff={0}, From={1}, To={2}",
                    iDiff, iFrom, iTo);
                sw.Restart();
                BigInteger bigProduct = FastFactorial(iDiff, iFrom, iTo);
                lCountTime = sw.ElapsedMilliseconds;
                string sFormat = ((double)bigProduct < 1.0E30) ? "{0:N0}" : "{0:E8}";
                Console.WriteLine("Product = " + sFormat, bigProduct);
                Console.WriteLine("Count Time = {0}ms, Output Time = {1}ms",
                  lCountTime, sw.ElapsedMilliseconds - lCountTime);
                nArg += 3;
            }
            sw.Stop();
            Console.WriteLine("Enter key to exit");
            Console.ReadKey();
        }

    }
}
