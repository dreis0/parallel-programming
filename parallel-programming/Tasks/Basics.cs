using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace parallel_programming.Tasks
{
    internal static class Basics
    {
        public static void Write(char c)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(c);
            }
        }

        public static void WriteAny(object o)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(o);
                Console.Write(Task.CurrentId);
            }
        }

        public static void WriteCurrentTask()
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(Task.CurrentId);            }
        }

        public static void Main()
        {
            Task.Factory.StartNew(() => WriteCurrentTask());
            
            var t = new Task(() => WriteCurrentTask());
            t.Start();

            Task.Factory.StartNew(WriteAny, "Any");

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
