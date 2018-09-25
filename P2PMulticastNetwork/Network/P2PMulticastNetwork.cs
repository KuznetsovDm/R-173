using CSharpFunctionalExtensions;
using RadioPipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using P2PMulticastNetwork.Interfaces;

//It's should be faultless network
namespace P2PMulticastNetwork
{
    public class DataEngineMiner : IDataMiner
    {
        private List<Action<byte[]>> _actions;
        private IDataReceiver _dataReceiver;
        private ActionEngine _engine;

        public DataEngineMiner(IDataReceiver receiver)
        {
            _dataReceiver = receiver;
            _actions = new List<Action<byte[]>>();
            _engine = new ActionEngine();
        }

        public void Dispose()
        {
            _actions?.Clear();
            _engine?.Dispose();
            _dataReceiver?.Dispose();
            _actions = null;
            _engine = null;
            _dataReceiver = null;
        }

        public void OnDataAwaliable(Action<byte[]> action)
        {
            _actions.Add(action);
        }

        public void Start()
        {
            _engine.Start(ReceiveCycle);
        }

        public void Stop()
        {
            _engine.Stop();
        }

        private async Task ReceiveCycle(CancellationToken token)
        {
            try
            {
                while(true)
                {
                    token.ThrowIfCancellationRequested();
                    Result<byte[]> resut = await _dataReceiver.Receive();
                    if(resut.IsFailure)
                        Debug.WriteLine(resut.Error);
                    else
                        _actions.ForEach(action => action(resut.Value));
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error in ReceiveCycle {ex.ToString()}");
            }
        }
    }

    public class ActionEngine : IDisposable
    {
        private Task _task;
        private CancellationTokenSource _cancellToken;

        public bool IsWork { get; private set; }

        public void Dispose()
        {
            if(IsWork)
            {
                Stop();
            }
            else if(_cancellToken != null)
            {
                Stop();
            }
        }

        public void Start(Func<CancellationToken, Task> action)
        {
            _cancellToken = new CancellationTokenSource();
            _task = TaskEx.Run(() => action(_cancellToken.Token), _cancellToken.Token);
            IsWork = true;
        }

        public void Stop()
        {
            _cancellToken.Cancel();
            _cancellToken = null;
            _task = null;
            IsWork = false;
        }
    }

    public class PipelineDataModelProcessingBeginPoint
    {
        private IPiplineProcessingInput _pipline;

        private IDataMiner _miner;

        public PipelineDataModelProcessingBeginPoint(IDataMiner miner, IPiplineProcessingInput pipline)
        {
            _miner = miner;
            _pipline = pipline;
        }

        public void Start()
        {
            _miner.Start();
        }

        public void Stot()
        {
            _miner.Stop();
        }

    }
}
