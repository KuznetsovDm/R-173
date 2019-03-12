using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P2PMulticastNetwork
{
    public delegate Task PipelineDelegate<in T>(T context);

    public class PipelineBuilder<T>
    {
        private readonly LinkedList<Func<T, PipelineDelegate<T>, Task>> _actions;

        public PipelineBuilder()
        {
            _actions = new LinkedList<Func<T, PipelineDelegate<T>, Task>>();
        }

        public virtual PipelineDelegate<T> Build()
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

        public virtual PipelineBuilder<T> Use(Func<T, PipelineDelegate<T>, Task> action)
        {
            _actions.AddFirst(action);
            return this;
        }
    }

    //player <-- mixer <-- global noize filter <-- local noize filter <-- playing filter <-- audio codec <-- data parsing <-- data avaliable
    //audio data avaliable --> audio codec --> data code --> send data
}
