namespace c4fsharp

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server

type Action =
    | [<EndPoint "GET /">] Home
    | [<EndPoint "GET /about">] About

module Common =
    open WebSharper.UI.Next.Html

    let href txt url =
        aAttr [attr.``href`` url] [text txt]

    let ( => ) text url =
        href text url

module Templating =
    open WebSharper.UI.Next.Html
    open Common

    type MainTemplate = Templating.Template<"Main.html">

    let TopNav : Doc list =
        [
            li ["Google+" => "https://plus.google.com/114125245508430492423/post"]
            li ["Twitter" => "http://twitter.com/c4fsharp"]
            li ["GitHub" => "https://github.com/c4fsharp"]
            li ["Vimeo Channel" => "http://vimeo.com/channels/c4fsharp"]
            li ["YouTube Channel" => "http://www.youtube.com/channel/UCCQPh0mSMaVpRcKUeWPotSA/feed"]
            li ["The F# Software Foundation" => "http://fsharp.org/"]
        ]

    let Main ctx action title body =
        Content.Doc(
            MainTemplate.Doc(title = title, topnav = TopNav, body = body))

module Site =
    open WebSharper.UI.Next.Html
    open Common

    let Links (ctx: Context<Action>) endpoint : Doc =
        let ( => ) txt act =
             liAttr [if endpoint = act then yield attr.``class`` "active"] [
                aAttr [attr.href (ctx.Link act)] [text txt]
             ]
        ul [
            "Home" => Action.Home
            "About" => Action.About
        ] :> _

    let HomePage ctx =
        Templating.Main ctx Action.Home "Community for F#" [Links ctx Action.Home]

    let AboutPage ctx =
        Templating.Main ctx Action.About "About" [Links ctx Action.About]

    [<Website>]
    let Main =
        Application.MultiPage <| fun ctx -> function
            | Action.Home -> HomePage ctx
            | Action.About -> AboutPage ctx

module SelfHostedServer =

    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin

    [<EntryPoint>]
    let Main = function
        | [| rootDirectory; url |] ->
            use server = WebApp.Start(url, fun appB ->
                appB.UseStaticFiles(
                        StaticFileOptions(
                            FileSystem = PhysicalFileSystem(rootDirectory)))
                    .UseSitelet(rootDirectory, Site.Main)
                |> ignore)
            stdout.WriteLine("Serving {0}", url)
            stdin.ReadLine() |> ignore
            0
        | _ ->
            eprintfn "Usage: c4fsharp ROOT_DIRECTORY URL"
            1