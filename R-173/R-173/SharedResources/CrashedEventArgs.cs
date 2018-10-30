using System;
using System.Collections.Generic;

namespace R_173.SharedResources
{
    public class CrashedEventArgs : EventArgs
    {
        public IList<string> Errors { get; set; }
    }
}
