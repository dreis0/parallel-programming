using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace parallel_programming.DataSynchronization
{
    public static class SpinLockDemo
    {
        public class Conta
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

        public static void Demo()
        {
            var tasks = new List<Task>();
            var conta = new Conta();

            var spinLock = new SpinLock();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool lockAdquirido = false;
                        try
                        {
                            spinLock.Enter(ref lockAdquirido);
                            conta.Deposito(100);
                        }
                        finally
                        {
                            if (lockAdquirido)
                                spinLock.Exit();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool lockAdquirido = false;
                        try
                        {
                            spinLock.Enter(ref lockAdquirido);
                            conta.Saque(100);
                        }
                        finally
                        {
                            if (lockAdquirido)
                                spinLock.Exit();
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Saldo: {conta.Saldo}");
        }

        static SpinLock _spinLock = new SpinLock(true);
        public static void LockRecursionDemo(int x)
        {
            bool lockTaken = false;

            try
            {
                _spinLock.Enter(ref lockTaken);
            }
            catch (LockRecursionException e)
            {
                Console.WriteLine("Exception" +  e);
            }
            finally 
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Lock adquirido, x = {x}");
                    LockRecursionDemo(x - 1);
                    _spinLock.Exit();
                }
                else
                {
                    Console.WriteLine($"Falha ao adquirir lock, x = {x}");
                }
            }

        }

        public static void Main()
        {
            LockRecursionDemo(5);
        }
    }
}
