using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace parallel_programming.DataSynchronization
{
    //Mutex is a wait handle type
    public static class MutexDemo
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

            public void Tansferir(Conta dest, double qtd)
            {
                dest.Deposito(qtd);
                Saque(qtd);
            }
        }

        public static void Demo()
        {
            var tasks = new List<Task>();
            var conta = new Conta();
            var mutex = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool hasLock = mutex.WaitOne();
                        try
                        {
                            conta.Deposito(100);
                        }
                        finally
                        {
                            if (hasLock)
                                mutex.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool hasLock = mutex.WaitOne();
                        try
                        {
                            conta.Saque(100);
                        }
                        finally
                        {
                            if (hasLock)
                                mutex.ReleaseMutex();
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Saldo: {conta.Saldo}");
        }

        public static void DemoWithMultipleMutex()
        {
            var tasks = new List<Task>();

            var c1 = new Conta();
            var c2 = new Conta();

            var mutex1 = new Mutex();
            var mutex2 = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool hasLock = mutex1.WaitOne();
                        try
                        {
                            c1.Deposito(100);
                        }
                        finally
                        {
                            if (hasLock)
                                mutex1.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool hasLock = mutex1.WaitOne();
                        try
                        {
                            c1.Saque(100);
                        }
                        finally
                        {
                            if (hasLock)
                                mutex1.ReleaseMutex();
                        }
                    }
                }));
            }

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool hasLock = mutex2.WaitOne();
                        try
                        {
                            c2.Deposito(100);
                        }
                        finally
                        {
                            if (hasLock)
                                mutex2.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool hasLock = mutex2.WaitOne();
                        try
                        {
                            c2.Saque(100);
                        }
                        finally
                        {
                            if (hasLock)
                                mutex2.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool hasLock = WaitHandle.WaitAll(new[] { mutex1, mutex2 });
                        try
                        {
                            c1.Tansferir(c2, i);
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                mutex1.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool hasLock = WaitHandle.WaitAll(new[] { mutex1, mutex2 });
                        try
                        {
                            c2.Tansferir(c1, i);
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                mutex1.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Saldo c1: {c1.Saldo}");
            Console.WriteLine($"Saldo c2: {c2.Saldo}");
        }


        public static void Main()
        {
            DemoWithMultipleMutex();
        }
    }
}
