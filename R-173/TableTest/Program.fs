// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
module TableTest.Table

open P2PMulticastNetwork
open System
open System.Net.Sockets
open P2PMulticastNetwork.Extensions
open System.Net
open System.Threading
open P2PMulticastNetwork.Network

let showLocalEp() = 
    use socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP)
    socket.Connect("10.0.1.20", 1337)
    Console.WriteLine(socket.LocalEndPoint)

showLocalEp()

let table =
    let udpOption = new UdpConnectionOption()
    let t = new RedistLocalConnectionTable(udpOption, new RedistLocalConnectionTable.RedistributableTableOption())
    t

let connectionHandler = new EventHandler<ConnectionArgs>((fun sender args -> printfn "connect %A" args.Info.Endpoint))
let diconnectHandler = new EventHandler<ConnectionArgs>((fun sender args -> printfn "disconnect %A" args.Info.Endpoint))
table.OnConnected.AddHandler(connectionHandler)
table.OnDisconnected.AddHandler(diconnectHandler)

let dataToSend = new NotificationData()
dataToSend.Endpoint <- new IPEndPoint(IPAddress.Broadcast, 33100)
dataToSend.Id <- Guid.NewGuid()
table.Register(dataToSend)

let bytes = dataToSend.Serialize()
let sendTo = new IPEndPoint(IPAddress.Broadcast, 33100)
let client = new UdpClient()
while true do
    let result = client.Send(bytes, bytes.Length, sendTo)
    Console.ReadKey()
