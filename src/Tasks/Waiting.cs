using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace parallel_programming.Tasks
{
    internal static class Waiting
    {
        public static void Main()
        {
            var cancellationSource = new CancellationTokenSource();
            var token = cancellationSource.Token;

            var task = new Task(() =>
            {
                Console.WriteLine("Você tem cinco segundos para apertar uma tecla");
                bool cancelled = token.WaitHandle.WaitOne(5000);
                Console.WriteLine(cancelled ? "Tecla apertada a tempo" : "Tecla não apertada a tempo");
            }, token);

            task.Start();

            Console.ReadKey();
            cancellationSource.Cancel();

            Console.ReadKey();
        }
    }
}
