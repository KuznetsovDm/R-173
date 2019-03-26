using AustinHarris.JsonRpc;
using P2PMulticastNetwork.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public static void Handle(JsonRequest request)
        {
            Handle(request.JsonRpc);
        }

        public static Task<string> Handle(string jsonrpc)
        {
            return JsonRpcProcessor.Process(jsonrpc);
        }
    }
}
