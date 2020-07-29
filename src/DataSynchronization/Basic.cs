using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace parallel_programming.DataSynchronization
{
    public static class Basic
    {
        public interface IConta
        {
            void Deposito(double qtd);
            void Saque(double qtd);

            double Saldo { get; }
        }

        //Exemplo com conta em banco
        public class ContaUnsafe : IConta
        {
            public double Saldo { get; set; }
            public void Deposito(double qtd)
            {
                Saldo += qtd;
            }

            public void Saque(double qtd)
            {
                Saldo -= qtd;
            }
        }

        public class ContaSafe : IConta
        {
            public double Saldo { get; set; }
            public object lockToken = new object();

            public void Deposito(double qtd)
            {
                lock (lockToken)
                {
                    Saldo += qtd;
                }
            }

            public void Saque(double qtd)
            {
                lock (lockToken)
                {
                    Saldo -= qtd;
                }
            }
        }

        public static void Demo<T>() where T : IConta, new()
        {
                var tasks = new List<Task>();
                var conta = new T();

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
            Demo<ContaSafe>();
        }
    }
}
