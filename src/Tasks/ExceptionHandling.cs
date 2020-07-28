using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace parallel_programming.Tasks
{
    internal static class ExceptionHandling
    {

        public static void HandleAllExceptions()
        {
            var task1 = Task.Factory.StartNew(() =>
            {
                throw new InvalidOperationException() { Source = "task1" };
            });

            var task2 = Task.Factory.StartNew(() =>
            {
                throw new AccessViolationException() { Source = "task2" };
            });

            try
            {
                Task.WaitAll(task1, task2);
            }
            catch (AggregateException aggException)
            {
                foreach(var e in aggException.InnerExceptions)
                    Console.WriteLine("Erro tratado");
            }


            Console.WriteLine("Fim do método");
        }

        public static void HandleOneTypeOfException()
        {
            var task1 = Task.Factory.StartNew(() =>
            {
                throw new InvalidOperationException() { Source = "task1" };
            });

            var task2 = Task.Factory.StartNew(() =>
            {
                throw new AccessViolationException() { Source = "task2" };
            });

            try
            {
                Task.WaitAll(task1, task2);
            }
            catch (AggregateException aggException)
            {
                aggException.Handle(e =>
                {
                    if (e is InvalidOperationException invalidEx)
                    {
                        Console.WriteLine("Erro tratado");
                        return true;
                    }
                    return false;
                });
            }

            Console.WriteLine("Fim do método");
        }

        public static void Main()
        {
            try
            {
                HandleOneTypeOfException();
            }
            catch (Exception)
            {
                Console.WriteLine("Erro não tratado pelo método");
            }
        }
    }
}
