module TestTable.Tcp
open System.Net.Sockets
open System.Net
open System

let ip = IPAddress.Loopback
let port = 33100
let addr = IPEndPoint(ip, port)
let connection = new TcpClient()

type Input =
    | Try
    | Stop

let readKey = 
    printfn "print simbol to end -> e or any other simbol to try connect."
    let key = Console.ReadKey()
    match key.Key with
        | ConsoleKey.E -> Stop
        | _ -> Try

let tryConnect() = 
    try
        try
            connection.Connect(addr)
        with 
            ex -> 
                printfn "error: %s" ex.Message
    finally
        connection.Close()

let iterations = seq {1..30}

iterations 
    |> (fun x -> readKey)
    |> (fun x -> 
        match x with
            | Try -> tryConnect()
            | _ -> ignore())
