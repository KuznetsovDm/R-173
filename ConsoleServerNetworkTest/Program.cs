using P2PMulticastNetwork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleServerNetworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IDataMiner miner1 = new DataEngineMiner();
            int operations = 0;
            miner1.OnDataAwaliable((data) =>
            {
                Console.WriteLine(Encoding.UTF8.GetString(data));
            });

            var options = new MulticastConnectionOptions
            {
                ExclusiveAddressUse = false,
                MulticastLoopback = true,
                UseBind = true
            };
            IDataReceiver receiver = new UdpMulticastConnection(options);
            miner1.ReloadDataReceiver(receiver);

            var timer = Stopwatch.StartNew();
            miner1.Start();
            Console.ReadKey();
            miner1.Stop();
            timer.Stop();
            Console.WriteLine($"oper = {operations} times = {timer.Elapsed} operation per miliseconds {operations / timer.ElapsedMilliseconds}");
            Console.ReadKey();
        }

    }
}
