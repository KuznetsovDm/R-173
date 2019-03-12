using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using P2PMulticastNetwork.Interfaces;
using P2PMulticastNetwork.Model;

//It's should be faultless network
namespace P2PMulticastNetwork.Network
{
    public class DataEngineMiner : IDataProvider
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

        public event EventHandler<DataEventArgs> OnDataAvaliable;

        public void Dispose()
        {
            _actions?.Clear();
            _engine?.Dispose();
            _dataReceiver?.Dispose();
            _actions = null;
            _engine = null;
            _dataReceiver = null;
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
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    Result<byte[]> resut = await _dataReceiver.Receive();
                    if (resut.IsFailure)
                        Debug.WriteLine(resut.Error);
                    else
                        OnDataAvaliable?.Invoke(this, new DataEventArgs() { Data = resut.Value });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ReceiveCycle {ex}");
            }
        }
    }

    public class ActionEngine : IDisposable
    {
	    private CancellationTokenSource _cancellToken;

        public bool IsWork { get; private set; }

        public void Dispose()
        {
            if (IsWork)
            {
                Stop();
            }
            else if (_cancellToken != null)
            {
                Stop();
            }
        }

        public void Start(Func<CancellationToken, Task> action)
        {
            _cancellToken = new CancellationTokenSource();
            TaskEx.Run(() => action(_cancellToken.Token), _cancellToken.Token);
            IsWork = true;
        }

        public void Stop()
        {
            _cancellToken.Cancel();
            _cancellToken = null;
	        IsWork = false;
        }
    }

    public class PipelineDataModelProcessingBeginPoint
    {
	    private readonly IDataProvider _miner;

        public PipelineDataModelProcessingBeginPoint(IDataProvider miner)
        {
            _miner = miner;
        }

        public void Start()
        {
            _miner.Start();
        }

        public void Stop()
        {
            _miner.Stop();
        }

    }
}
