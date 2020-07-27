using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace parallel_programming.Tasks
{
    public static class Waiting2
    {
        public static void WaitForOneTask()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task1 = new Task(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("task 1 completa");
            }, token);

            task1.Start();
            task1.Wait(token);

            Console.WriteLine("Fim do método");
            Console.ReadKey();
        }

        public static void WaitForAllTasks()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task1 = new Task(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("task 1 completa");
            }, token);

            task1.Start();
            var task2 = Task.Factory.StartNew(() => Thread.Sleep(3000), token);

            Task.WaitAll(task1, task2);

            Console.WriteLine("Fim do método");
            Console.ReadKey();
        }

        public static void WaitForShortestTask()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task1 = new Task(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("task 1 completa");
            }, token);

            task1.Start();
            var task2 = Task.Factory.StartNew(() => Thread.Sleep(3000), token);

            Task.WaitAny(task1, task2);

            Console.WriteLine("Fim do método");
            Console.ReadKey();
        }

        public static void WaitForShortestTaskWithTimeout()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task1 = new Task(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }
                Console.WriteLine("task 1 completa");
            }, token);

            task1.Start();
            var task2 = Task.Factory.StartNew(() => Thread.Sleep(6000), token);

            Task.WaitAny(new[] { task1, task2 }, 2000, token); 
            Console.WriteLine("Fim do método");
            //Imprime "Fim do Método" antes de "task 1 completa" pois para de esperar após 2s

            Console.ReadKey();
        }

        public static void Main()
        {
            WaitForShortestTaskWithTimeout();
        }
    }
}
