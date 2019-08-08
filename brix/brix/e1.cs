using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Brix
{
    public class e1
    {
        Queue<int> queqe;
        static bool[] cashier;
        Thread enqueue;
        Thread[] busyCashier;
        //int freeCashier = -1;
        static int c = 10;
        //static bool mutex = false;
        static Mutex mutex = new Mutex();
        static Mutex mutex2 = new Mutex();

        public e1()
        {
            queqe = new Queue<int>();
            cashier = new bool[5]; // false = free , true = busy
            busyCashier = new Thread[5];

            // init
            for (int i = 0; i < cashier.Length; i++)
            {
                cashier[i] = false;
            }

        }

        public void Run()
        {
            enqueue = new Thread(EnqueueClient);
            enqueue.Start();
            BusyCashierThread();
        }

        private void EnqueueClient()
        {
            for (int i = 0; i < 10; i++)
            {
                queqe.Enqueue(i);
                Console.WriteLine($"client {i} wait");
                Thread.Sleep(1 * 1000);
            }
            Console.WriteLine($"enqueue Thread finished");
            enqueue.Abort();
        }
        private void BusyCashierThread()
        {
            for (int i = 0; i < cashier.Length; i++)
            {
                busyCashier[i] = new Thread(() => BusyCashier(i));
                busyCashier[i].Start();
            }
        }

        private void BusyCashier(int i)
        {
            while (c > 0)
            {
                int freeCashier = -1;
                while (freeCashier == -1)
                {
                    freeCashier = FreeCashier();
                }

                // critical section - dequeue
                mutex.WaitOne();
                if (queqe.Count > 0 && freeCashier != -1)
                {
                    int client = queqe.Dequeue();
                    c--;
                    cashier[freeCashier] = true;
                    Console.WriteLine($"client {client} entered to cashier {freeCashier}");
                    mutex.ReleaseMutex();

                    int tic = TimeBusy() * 1000;
                    Thread.Sleep(tic);

                    mutex2.WaitOne();
                    cashier[freeCashier] = false;
                    Console.WriteLine($"client {client} finished");
                    freeCashier = -1;
                    mutex2.ReleaseMutex();

                }
                else
                    mutex.ReleaseMutex();

            }
            Console.WriteLine($"cashier {i} Thread finished");
            busyCashier[i-1].Abort();
        }
        private int FreeCashier()
        {
            for (int i = 0; i < cashier.Length; i++)
            {
                if (!cashier[i])
                {
                    return i;
                }

            }
            return -1;
        }
        private int TimeBusy()
        {
            Random random = new Random();
            return random.Next(1, cashier.Length);
        }


    }
}
