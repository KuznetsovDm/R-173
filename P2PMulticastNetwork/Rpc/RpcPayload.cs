using AustinHarris.JsonRpc;
using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;
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

    public class TcpRpcHandler
    {
        private readonly TcpClient _client;

        public TcpRpcHandler(TcpClient client)
        {
            _client = client;
        }

        public JsonResponse SendAndReceive(JsonRequest request)
        {
            var writer = new BinaryWriter(_client.GetStream());
            var reader = new BinaryReader(_client.GetStream());
            var rpc = JsonConvert.SerializeObject(request);
            writer.Write(rpc);
            var responceString = reader.ReadString();
            var responce = JsonConvert.DeserializeObject<JsonResponse>(responceString);
            return responce;
        }

        public void ReceiveAndSend()
        {
            var writer = new BinaryWriter(_client.GetStream());
            var reader = new BinaryReader(_client.GetStream());
            var responce = reader.ReadString();
            var result = Rpc.Handle(responce).Result;
            writer.Write(result);
        }
    }

}
