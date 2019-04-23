using AustinHarris.JsonRpc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PMulticastNetwork.Rpc
{
    public static class Rpc
    {
        public static JsonRequest Request(string method, params object[] args)
        {
            return new JsonRequest { Method = method, Params = args };
        }

        public static Task<string> Handle(JsonRequest request)
        {
            return Handle(JsonConvert.SerializeObject(request));
        }

        public static Task<string> Handle(string jsonrpc)
        {
            return JsonRpcProcessor.Process(jsonrpc);
        }
    }

    public class TcpRpcHandler : IDisposable
    {
        private TcpClient _client;

        public TcpRpcHandler(TcpClient client)
        {
            _client = client;
        }

        public void SendRequest(JsonRequest request)
        {
            var writer = new BinaryWriter(_client.GetStream(), Encoding.UTF8);
            var rpc = JsonConvert.SerializeObject(request);
            writer.Write(rpc);
            writer.Flush();
        }

        public void SendRequest(string method, params object[] args)
        {
            var req = Rpc.Request(method, args);
            SendRequest(req);
        }

        public string ReceiveString()
        {
            var stream = _client.GetStream();
            var reader = new BinaryReader(_client.GetStream());
            return reader.ReadString();
        }

        public void Dispose()
        {
            if (_client != null)
            {
                if (_client.Connected)
                    _client.Client.Shutdown(SocketShutdown.Both);
                _client.Close();
                _client = null;
            }
        }
    }

}
