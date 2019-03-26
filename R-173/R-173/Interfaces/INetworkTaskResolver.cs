using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R_173.Interfaces
{
    public interface INetworkTaskResolver
    {

    }

    public interface INetwokrLearningTaskDiskover
    {
        IEnumerable<object> NetworkTasks { get; set; }

        event EventHandler OnNewTask;
    }
}
