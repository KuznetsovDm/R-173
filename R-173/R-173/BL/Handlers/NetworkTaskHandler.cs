using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2PMulticastNetwork;
using P2PMulticastNetwork.Model;
using R_173.Interfaces;

namespace R_173.BL.Handlers
{
	public class NetworkTaskHandler : IInvoker
	{
		public Task Invoke(DataModel model, PipelineDelegate<DataModel> next)
		{
			throw new NotImplementedException();
		}
	}
}
