using System;
using System.Collections.Generic;

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
