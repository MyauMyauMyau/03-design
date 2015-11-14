using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace PerfLogging
{
    class Program
    {
        static void Main(string[] args)
        {

            var sum = 0.0;
            using (PerfLogger.Measure(t => Console.WriteLine("for: {0}", t)))
                for (var i = 0; i < 100000000; i++) sum += i;
            using (PerfLogger.Measure(t => Console.WriteLine("linq: {0}", t)))
                sum -= Enumerable.Range(0, 100000000).Sum(i => (double)i);
            Console.WriteLine(sum);

        }
    }

    public class PerfLogger : IDisposable
    {
        public Stopwatch timer = new Stopwatch();
        public Action<TimeSpan> toDo;
        public static PerfLogger Measure(Action<TimeSpan> action)
        {
            var p = new PerfLogger { toDo = action };
            p.timer.Start();
            return p;
        }

        public void Dispose()
        {
            toDo(timer.Elapsed);
            timer.Stop();
        }
    }
}
