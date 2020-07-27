using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace parallel_programming.Tasks
{
    internal static class Cancellation
    {
        public static void SoftWayOfCancellation()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var t = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    if (token.IsCancellationRequested)
                        break;
                    else
                        Console.WriteLine($"{i++}");
                }
            }, token);

            t.Start();
            token.Register(() => Console.WriteLine("Soft cancellation requested"));

            Console.ReadKey();
            cts.Cancel();
        }

        public static void TraditionalWayOfCancellation()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var t = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}");
                }
            }, token);

            token.Register(() => Console.WriteLine("Traditional cancellation requested"));

            t.Start();

            Console.ReadKey();
            cts.Cancel();
        }

        public static void CompositeCancellation()
        {
            var cts1 = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();

            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token);

            var t = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    linkedTokenSource.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}");
                }
            }, linkedTokenSource.Token);

            cts1.Token.Register(() => Console.WriteLine("Cancelled by token 1"));
            cts2.Token.Register(() => Console.WriteLine("Cancelled by token 2"));

            t.Start();

            var c = Console.ReadKey();
            if (c.KeyChar == '1')
                cts1.Cancel();
            else
                cts2.Cancel();
        }

        public static void Main()
        {
            //TraditionalWayOfCancellation();
            //SoftWayOfCancellation();
            CompositeCancellation();

            Console.ReadKey();
        }
    }
}
