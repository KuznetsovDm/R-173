using System;

namespace R_173.BL.Tasks
{
	public interface INetworkTaskSheduler
	{
		event EventHandler TaskCreated;
		event EventHandler TaskStarted;

		void Start();

		void Confirm();

		void Stop();
	}
}
