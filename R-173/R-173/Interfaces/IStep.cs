using System;
using System.Collections.Generic;
using R_173.SharedResources;

namespace R_173.Interfaces
{
    public interface IStep<T> : IDisposable
    {
        bool StartIfInputConditionsAreRight(T model, out IList<string> errors);
        event EventHandler Completed;
        event EventHandler<CrashedEventArgs> Crashed;
        void Freeze();
        void Unfreeze();
        void Reset();
    }
}
