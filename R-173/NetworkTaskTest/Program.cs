using System;
using P2PMulticastNetwork.Model;

namespace NetworkTaskTest
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var player = new Player();

			while (true)
			{
				try
				{
					Console.WriteLine("Enter frequency: ");
					var frequency = int.Parse(Console.ReadLine());
					// ReSharper disable once AssignNullToNotNullAttribute

					Console.WriteLine("Enter task id: ");
					var networkTaskId = Guid.Parse(Console.ReadLine());

					player.NetworkTaskData = new NetworkTaskData
					{
						Id = networkTaskId,
						Frequency = frequency
					};

					Console.ReadKey();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}
	}
}
