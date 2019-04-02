using System;

namespace R_173.Interfaces
{
	public interface INetworkTaskScheduler
	{
		event EventHandler TaskCreated;
		event EventHandler TaskStarted;

		void Start();

		void Confirm();

		void Stop();
	}
}
