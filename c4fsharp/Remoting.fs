namespace c4fsharp

open WebSharper

module Remoting =

    [<Rpc>]
    let Process input =
        async {
            return "You said: " + input
        }
