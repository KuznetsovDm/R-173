using System;
using System.Collections.Generic;

namespace R_173.BL
{
    public interface IStep<T>
    {
        bool StartIfInputConditionsAreRight(T model, out IList<string> errors);
        event EventHandler Completed;
        event EventHandler<CrashedEventArgs> Crashed;
    }
}
