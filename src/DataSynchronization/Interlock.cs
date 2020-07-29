using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace parallel_programming.DataSynchronization
{
    public static class Interlock
    {
        public class Conta
        {
            private int _saldo;
            public int Saldo
            {
                get => _saldo;
                private set => _saldo = value;
            }

            public void Deposito(int qtd)
            {
                Interlocked.Add(ref _saldo, qtd);
            }

            public void Saque(int qtd)
            {
                Interlocked.Add(ref _saldo, -qtd);
            }
        }

        public static void Demo()
        {
            var tasks = new List<Task>();
            var conta = new Conta();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        conta.Deposito(100);
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        conta.Saque(100);
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Saldo: {conta.Saldo}");
        }

        public static void Main()
        {
            Demo();
        }
    }
}
