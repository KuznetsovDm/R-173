using System;

namespace P2PMulticastNetwork.Model
{
	public class DataEventArgs<T> : EventArgs
	{
		public T Data { get; set; }

		public DataEventArgs() { }

		public DataEventArgs(T data)
		{
			Data = data;
		}
	}
}
