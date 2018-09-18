using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioPipeline
{
    public delegate Task PipelineDelegate<T>(T context);

    public class PipelineBuilder<T>
    {
        private LinkedList<Func<T, PipelineDelegate<T>, Task>> _actions;

        public PipelineBuilder()
        {
            _actions = new LinkedList<Func<T, PipelineDelegate<T>, Task>>();
        }

        public PipelineDelegate<T> Build()
        {
            PipelineDelegate<T> currentDelegate = delegate {return TaskEx.FromResult(0); };
            foreach(var action in _actions)
            {
                var prevDelegate = currentDelegate;
                currentDelegate = async context =>
                {
                    await action(context, prevDelegate);
                };
            }

            return currentDelegate;
        }

        public PipelineBuilder<T> Use(Func<T, PipelineDelegate<T>, Task> action)
        {
            _actions.AddFirst(action);
            return this;
        }
    }

    //player <-- mixer <-- global noize filter <-- local noize filter <-- playing filter <-- audio codec <-- data parsing <-- data avaliable
    //audio data avaliable --> audio codec --> data code --> send data
}
