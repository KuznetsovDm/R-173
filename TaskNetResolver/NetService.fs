module NetService
open System
open System.Net
open System.Net.Sockets
open System.Threading

type TcpListener with
    member listener.AsyncTcpClient() = Async.FromBeginEnd(listener.BeginAcceptTcpClient, listener.EndAcceptTcpClient)

type NetServiceConnectionHandler(?port) =

    let maxConnections = 1
    let cts = CancellationTokenSource()
    let listener =
        let port = defaultArg port 33100
        let listener = new TcpListener(IPAddress.Any, port)
        listener

    let onConnected = new Event<TcpClient>()

    let tcpClientHandler() = async {
        let! client = listener.AsyncTcpClient()
        onConnected.Trigger(client)
    }

    member this.OnConnected =
        onConnected.Publish

    member this.Begin(token : CancellationToken) =
        listener.Start()
        listener.Server.Listen(maxConnections)
        token.Register((fun x -> listener.Stop()))
        Async.Start(tcpClientHandler(), token)

    interface IDisposable with
        member x.Dispose() = listener.Server.Close()

