using CSharpFunctionalExtensions;
using P2PMulticastNetwork.Model;
using RadioPipeline;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//It's should be faultless network
namespace P2PMulticastNetwork
{
    public interface IDataProvider : IDisposable
    {
        void OnDataAwaliable(Action<byte[]> action);
    }

    public interface IDataReceiver : IDisposable
    {
        Task<Result<byte[]>> Receive();
    }

    public interface IDataTransmitter : IDisposable
    {
        Result Write(byte[] data);
    }

    public interface IDataMiner : IDataProvider, IDisposable
    {
        void ReloadDataReceiver(IDataReceiver dataReceiver);
        void Start();
        void Stop();
    }

    public interface IDataAsByteConverter<T>
    {
        T ConvertFrom(byte[] bytes);
        byte[] ConvertToBytes(T data);
    }

    public class DataEngineMiner : IDataMiner
    {
        private List<Action<byte[]>> _actions;
        private IDataReceiver _dataReceiver;
        private ActionEngine _engine;

        public DataEngineMiner()
        {
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

        public void ReloadDataReceiver(IDataReceiver dataReceiver)
        {
            _dataReceiver = dataReceiver;
            //reload actionEngine
            if(_engine.IsWork)
            {
                _engine.Stop();
                _engine.Start(ReceiveCycle);
            }
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

    public class DataProcessingBuilder<T> : IDisposable
    {
        private IDataProvider _dataProvider;
        private IDataAsByteConverter<T> _converter;
        private PipelineDelegate<T> _pipeline;
        private PipelineBuilder<T> _builder;

        public DataProcessingBuilder(IDataAsByteConverter<T> converter, IDataProvider provider, PipelineBuilder<T> builder)
        {
            if(provider == null
                || converter == null
                || builder == null)
                throw new ArgumentNullException();

            _builder = new PipelineBuilder<T>();

            _dataProvider = provider;
            _converter = converter;
            BindOnDataAvaliable();
        }

        private void BindOnDataAvaliable()
        {
            _dataProvider.OnDataAwaliable((bytes) =>
            {
                T data = _converter.ConvertFrom(bytes);
                _pipeline.Invoke(data);
            });
        }

        public DataProcessingBuilder<T> Use(Func<T, PipelineDelegate<T>, Task> action)
        {
            _builder.Use(action);
            return this;
        }

        public void Build()
        {
            _pipeline = _builder.Build();
        }

        public void Dispose()
        {
            _dataProvider?.Dispose();
            _dataProvider = null;
            _converter = null;
            _pipeline = null;
        }
    }
}
