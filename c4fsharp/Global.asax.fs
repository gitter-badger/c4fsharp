namespace c4fsharp

type Global() =
    inherit System.Web.HttpApplication()

    member g.Application_Start(sender: obj, args: System.EventArgs) =
        ()
